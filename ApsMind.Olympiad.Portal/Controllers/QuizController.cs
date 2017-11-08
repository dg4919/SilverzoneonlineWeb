using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Portal.Models;
using System.Data.Entity;
using System.IO;

namespace ApsMind.Olympiad.Portal.Controllers
{
    //[ValidateInput(false)]
    //[CustomAuthorize(new string[] { "1", "3" })]
    public class QuizController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        //
        // GET: /Quize/
        public ActionResult Index(Quiz ObjQuiz, string Name, int ClassId = 0, int SubjectId=0, DateTime? StartDate = null, DateTime? EndDate = null)
        {

            //ViewBag.FreeTypesQuestion = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue !=3).ToList();
            var objQuizeList = getQuizData(ObjQuiz, ClassId,SubjectId, StartDate, EndDate);
            TempData["Subject"] = new SelectList(db.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
            ViewBag.ClassId = 0;
            ViewBag.SubjectId = 0;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
          
            return View(ObjQuiz);
        }

        [HttpPost]
        public ActionResult Index(Quiz ObjQuiz, int? ParentId, int ClassId = 0, int SubjectId=0, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            //ViewBag.FreeTypesQuestion = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
            var objQuizeList = getQuizData(ObjQuiz, ClassId,SubjectId, StartDate, EndDate);
            TempData["Subject"] = new SelectList(db.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
            ViewBag.ClassId = ClassId;
            ViewBag.SubjectId = SubjectId;
            return View(ObjQuiz);
        }

        [HttpGet]
        public JsonResult GetSubjects(int id)
        {
            var sublist = db.Subject.Where(x => x.ClassId == id && x.IsActive == true);
            // Console.Write(classlist);
            return Json(sublist, JsonRequestBehavior.AllowGet);

        }

        //[HttpGet]
        //public JsonResult GetClasses(int id)
        //{
        //    var classlist = db.StudentClass.Where(x => x.Id == id && x.IsActive == true);
        //    return Json(classlist, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult AddQuestion(int ClassId, int SubjectId, int PaperTypeId,int QuizId, int QuestionId = 0)
        {

            try
            {
                AddQuestionViewModel _addQuestionViewModel = new AddQuestionViewModel();
                _addQuestionViewModel.CategoryId = SubjectId;
                _addQuestionViewModel.ClassId = ClassId;
                _addQuestionViewModel.PaperTypeId = PaperTypeId;
                _addQuestionViewModel.Subjectid = SubjectId;
                _addQuestionViewModel.QuizId = QuizId;

                _addQuestionViewModel.ParagraphList = db.Paras.Where(x => x.IsActive == true).ToList();
                _addQuestionViewModel.ChapterList = db.Chapter.Where(x => x.ClassId == ClassId && x.SubjectId == SubjectId && x.PaperTypeId == PaperTypeId && x.IsActive == true).ToList();
                _addQuestionViewModel.QuestionLevelList = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
                ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList();
                Question objQuestion = db.Questions.Where(x => x.QuestionId == QuestionId).FirstOrDefault();                
                return PartialView("AddQuestion", _addQuestionViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddQuestion(AddQuestionViewModel model,HttpPostedFileBase Url,HttpPostedFileBase[] optionImage,String[] OptionText,int? AnswerIndex)
        {           

            Handler.ImageHandler objImageHandler = new Handler.ImageHandler();
            string ImagePath = "/Images/QuestionImage";
            string ImagePathOption = "/Images/QuestionOptionImage";
            int classid= Convert.ToInt32(Request["classid"]);
             int subjectid= Convert.ToInt32(Request["subjectid"]);
             int quizid= Convert.ToInt32(Request["quizid"]);
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (AnswerIndex != -1  && model.QuestionText != null || Url != null && model.QuestionLevel != 0)
                        {
                            var _question = new Question();
                            _question.QuestionText = model.QuestionText;
                            if (Url != null && Url.ContentLength > 0)
                            {
                                String FileName = objImageHandler.SaveFile(ImagePath + "/", Path.GetExtension(Url.FileName), Url.InputStream);
                                _question.ImageUrl = FileName;
                            }
                            _question.ParaId = model.ParaId;
                            _question.ChapterId = model.ChapterId;
                            _question.TopicId = model.TopicId;
                            _question.CategoryId = model.CategoryId;
                            _question.QuestionLevel = model.QuestionLevel;
                            _question.Mark = model.Mark;
                            _question.EstimatedTimeInSecond = model.EstimatedTimeInSecond;
                            _question.IsActive = model.IsActive;
                            _question.CreatedBy = ApplicationSession.CurrentUser.Id;
                            _question.CreatedOn = DateTime.Now;
                            db.Questions.Add(_question);
                            //return PartialView("AddQuestion", model);
                            db.SaveChanges();
                          
                            model.ParagraphList = db.Paras.Where(x => x.IsActive == true).ToList();
                            model.ChapterList = db.Chapter.Where(x => x.ClassId == classid && x.SubjectId == subjectid && x.PaperTypeId == model.PaperTypeId && x.IsActive == true).ToList();
                            model.QuestionLevelList = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();


                            for (int i = 0; i < 5; i++)
                            {
                                QuestionOption _questionOption = new QuestionOption();
                                var File = optionImage[i];
                                if (File != null && File.ContentLength > 0)
                                {
                                    String FileName = objImageHandler.SaveFile(ImagePathOption + "/", Path.GetExtension(File.FileName), File.InputStream);
                                    _questionOption.ImageUrl = FileName;

                                }
                                _questionOption.OptionText = OptionText[i];
                                _questionOption.OptionId = i;
                                _questionOption.QuestionId = _question.QuestionId;
                                _questionOption.IsAnswer = Convert.ToBoolean(AnswerIndex);
                                if (i == AnswerIndex)
                                {
                                    _questionOption.IsAnswer = true;
                                }
                                else
                                {
                                    _questionOption.IsAnswer = false;
                                }
                                ViewBag.Options = db.QuestionOptions.Where(x => x.QuestionId == quizid).ToList();
                                db.QuestionOptions.Add(_questionOption);
                      
                               
                            }
                            QuizDetail _quizDetail = new QuizDetail();
                            _quizDetail.QuizId =quizid;
                            _quizDetail.QuestionId = _question.QuestionId;
                            db.QuizDetails.Add(_quizDetail);
                            db.SaveChanges();
                            transaction.Commit();
                            //------------------
                            //View Question 

                            ViewBag.QuestionList = (from Ques in db.Questions
                                                    where (Ques.QuestionId == model.QuestionId)
                                                    select new
                                                    {
                                                        QuestionId = Ques.QuestionId,
                                                        QuestionText = Ques.QuestionText,
                                                        Mark = Ques.Mark,
                                                        Time = Ques.EstimatedTimeInSecond
                                                    }).ToList();
                            ViewBag.TotalQuestion = ViewBag.QuestionList;

                            var Result = ViewBag.QuestionList;
                            //-------------------
                            ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == model.QuestionId).ToList();
                            ModelState.Clear();
                            return Json(Result);
                        }
                        else
                        {
                            model.ParagraphList = db.Paras.Where(x => x.IsActive == true).ToList();
                            model.ChapterList = db.Chapter.Where(x => x.ClassId ==model.ClassId && x.SubjectId == model.CategoryId && x.PaperTypeId == model.PaperTypeId && x.IsActive == true).ToList();
                            model.QuestionLevelList = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
                            ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == model.QuestionId).ToList();
                             if (model.QuestionId > 0)
                                {
                                    ViewBag.Options = db.QuestionOptions.Where(x => x.QuestionId == model.QuestionId).ToList();
                                }
                                else
                                {
                                    List<QuestionOption> PostedQuesOpts = new List<QuestionOption>();
                                    for (int i = 0; i < OptionText.Length; i++)
                                    {
                                        QuestionOption objnew = new QuestionOption() { ImageUrl = "", IsAnswer = false, OptionId = 0, OptionText = OptionText[i], QuestionId = 0 };
                                        if (i == AnswerIndex)
                                        {
                                            objnew.IsAnswer = true;
                                        }
                                        PostedQuesOpts.Add(objnew);
                                    }

                                    ViewBag.Options = PostedQuesOpts;
                                }
                             TempData["Message"] = "You are not authorized.";
                              ViewBag.sms = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                                    "Danger" + "!</strong>" + "Select Question and Option" + ".</div>";
                              //var Result = new { ObjQuestion = ObjQuestion, Message = ViewBag.Message };
                              return PartialView("AddQuestion", model);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Error Occured", ex.Message);
                        transaction.Rollback();
                        model.ParagraphList = db.Paras.Where(x => x.IsActive == true).ToList();
                        model.ChapterList = db.Chapter.Where(x => x.ClassId == model.ClassId && x.SubjectId == model.CategoryId && x.PaperTypeId == model.PaperTypeId && x.IsActive == true).ToList();
                        model.QuestionLevelList = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
                        ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == model.QuestionId).ToList();
                         if (model.QuestionId > 0)
                            {
                                ViewBag.Options = db.QuestionOptions.Where(x => x.QuestionId ==model.QuestionId).ToList();
                            }
                            else
                            {
                                List<QuestionOption> PostedQuesOpts = new List<QuestionOption>();
                                for (int i = 0; i < OptionText.Length; i++)
                                {
                                    QuestionOption objnew = new QuestionOption() { ImageUrl = "", IsAnswer = false, OptionId = 0, OptionText = OptionText[i], QuestionId = 0 };
                                    if (i == AnswerIndex)
                                    {
                                        objnew.IsAnswer = true;
                                    }
                                    PostedQuesOpts.Add(objnew);
                                }

                                ViewBag.Options = PostedQuesOpts;
                            }
                            Common.LogError("Error", ex);
                            return View();
                    }
                }
    

        }


        [HttpGet]
        public ActionResult ViewQuestion(int QuestionId)
        {
            ViewQuestion objQuestionView = (from q in db.Questions
                                            join p in db.Paras on q.ParaId equals p.ParaId into pTemp
                                            from p in pTemp.DefaultIfEmpty()
                                            join qd in db.QuizDetails on q.QuestionId equals qd.QuestionId
                                            join qz in db.Quizs on qd.QuizId equals qz.QuizId
                                            where q.QuestionId == QuestionId
                                            select new ViewQuestion
                                            {
                                                ImageUrl = q.ImageUrl,
                                                Options = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList(),
                                                QuestionId = q.QuestionId,
                                                QuestionText = q.QuestionText,
                                                Mark = q.Mark,
                                                ParaId = q.ParaId,
                                                ParaText = p.ParaText,
                                                QuizId = qz.QuizId,
                                                IsPublished = qz.IsPublish,
                                            }).FirstOrDefault();

            //ViewQuestion objQuestionView = (from q in db.Questions
            //                                join c in db.Categorys on q.CategoryId equals c.CategoryId
            //                                join p in db.Paras on q.ParaId equals p.ParaId into pTemp
            //                                from p in pTemp.DefaultIfEmpty()
            //                                join qd in db.QuizDetails on q.QuestionId equals qd.QuestionId
            //                                join qz in db.Quizs on qd.QuizId equals qz.QuizId
            //                                where q.QuestionId == QuestionId
            //                                select new ViewQuestion
            //                                          {
            //                                              ImageUrl = q.ImageUrl,
            //                                              Options = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList(),
            //                                              QuestionId = q.QuestionId,
            //                                              QuestionText = q.QuestionText,
            //                                              Mark = q.Mark,
            //                                              ParaId = q.ParaId,
            //                                              ParaText = p.ParaText,
            //                                              QuizId = qz.QuizId,
            //                                              IsPublished = qz.IsPublish,
            //                                              CategoryId = c.CategoryId,
            //                                              CategoryText = c.CategoryName
            //                                          }).FirstOrDefault();

            return View(objQuestionView);
        }
        //[ValidateInput(false)]
        //[HttpPost]
        //public ActionResult AddQuestion(Question ObjQuestion, string[] OptionText, HttpPostedFileBase Url, int? AnswerIndex, HttpPostedFileBase[] OptionImage, String[] HiddenImagePath)
        //{
        //    int ClassId = Convert.ToInt32(TempData["ClassId"]);
        //    int SubjectId = Convert.ToInt32(TempData["SubjectId"]);
        //    int PaperTypeId = Convert.ToInt32(TempData["PaperTypeId"]);
        //    Handler.ImageHandler objImageHandler = new Handler.ImageHandler();
        //    string ImagePath = "/Images/QuestionImage";
        //    string ImagePathOption = "/Images/QuestionOptionImage";

        //    try
        //    {
        //        //ADd Question

        //        //List<long> Qids = new List<long>();
        //        if (AnswerIndex != -1 && ClassId != 0 && ObjQuestion.QuestionText != null || Url != null && ObjQuestion.QuestionLevel != 0)
        //        {

        //            if (Url != null && Url.ContentLength > 0)
        //            {

        //                String FileName = objImageHandler.SaveFile(ImagePath + "/", Path.GetExtension(Url.FileName), Url.InputStream);
        //                ObjQuestion.ImageUrl = FileName;
        //            }
        //            ObjQuestion.CategoryId = ClassId;
        //            ObjQuestion.CreatedBy = ApplicationSession.CurrentUser.UserId;
        //            ObjQuestion.CreatedOn = System.DateTime.Now;
        //            db.Questions.Add(ObjQuestion);
        //            db.SaveChanges();

        //            //Qids.Add(ObjQuestion.QuestionId);

        //            // Add Question Option-----------------
        //            int count = OptionText.Count();
        //            for (int i = 0; i < count; i++)
        //            {
        //                TempData[i.ToString()] = count; ;
        //                QuestionOption ObjQuestionOption = new QuestionOption();
        //                var File = OptionImage[i];

        //                if (File != null && File.ContentLength > 0)
        //                {
        //                    String FileName = objImageHandler.SaveFile(ImagePathOption + "/", Path.GetExtension(File.FileName), File.InputStream);
        //                    ObjQuestionOption.ImageUrl = FileName;

        //                }
        //                else
        //                {
        //                    ObjQuestionOption.ImageUrl = (HiddenImagePath[i]) == string.Empty ? null : (HiddenImagePath[i]);
        //                }
        //                ObjQuestionOption.OptionText = (OptionText[i]).ToString();
        //                ObjQuestionOption.OptionId = i;
        //                ObjQuestionOption.IsAnswer = Convert.ToBoolean(AnswerIndex);
        //                if (i == AnswerIndex)
        //                {
        //                    ObjQuestionOption.IsAnswer = true;
        //                }
        //                else
        //                {
        //                    ObjQuestionOption.IsAnswer = false;
        //                }
        //                ObjQuestionOption.QuestionId = ObjQuestion.QuestionId;
        //                ViewBag.Options = db.QuestionOptions.Where(x => x.QuestionId == ObjQuestion.QuestionId).ToList();
        //                db.QuestionOptions.Add(ObjQuestionOption);

        //            }
        //            db.SaveChanges();


        //            //------------------
        //            //View Question 

        //            ViewBag.QuestionList = (from Ques in db.Questions
        //                                    where (Ques.QuestionId == ObjQuestion.QuestionId)
        //                                    select new
        //                                    {
        //                                        QuestionId = Ques.QuestionId,
        //                                        QuestionText = Ques.QuestionText,
        //                                        Mark = Ques.Mark,
        //                                        Time = Ques.EstimatedTimeInSecond
        //                                    }).ToList();
        //            ViewBag.TotalQuestion = ViewBag.QuestionList;
        //            var Result = ViewBag.QuestionList;
        //            //-------------------

        //            TempData["ParaGraph"] = new SelectList(db.Paras.Where(x => x.IsActive == true), "ParaId", "ParaText");
        //            TempData["Chapter"] = new SelectList(db.Chapter.Where(x => x.ClassId == ClassId && x.SubjectId == SubjectId && x.PaperTypeId == PaperTypeId && x.IsActive == true), "ChapterId", "ChapterName");
        //            ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == ObjQuestion.QuestionId).ToList();
        //            ViewBag.QuestionLevel = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
        //            ModelState.Clear();
        //            return Json(Result);
        //        }
        //        else
        //        {
        //            TempData["ParaGraph"] = new SelectList(db.Paras.Where(x => x.IsActive == true), "ParaId", "ParaText");
        //            TempData["Chapter"] = new SelectList(db.Chapter.Where(x => x.ClassId == ClassId && x.SubjectId == SubjectId && x.PaperTypeId == PaperTypeId && x.IsActive == true), "ChapterId", "ChapterName");
        //            ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == ObjQuestion.QuestionId).ToList();
        //            ViewBag.QuestionLevel = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();

        //            if (ObjQuestion.QuestionId > 0)
        //            {
        //                ViewBag.Options = db.QuestionOptions.Where(x => x.QuestionId == ObjQuestion.QuestionId).ToList();
        //            }
        //            else
        //            {
        //                List<QuestionOption> PostedQuesOpts = new List<QuestionOption>();
        //                for (int i = 0; i < OptionText.Length; i++)
        //                {
        //                    QuestionOption objnew = new QuestionOption() { ImageUrl = "", IsAnswer = false, OptionId = 0, OptionText = OptionText[i], QuestionId = 0 };
        //                    if (i == AnswerIndex)
        //                    {
        //                        objnew.IsAnswer = true;
        //                    }
        //                    PostedQuesOpts.Add(objnew);
        //                }

        //                ViewBag.Options = PostedQuesOpts;
        //            }
        //            TempData["Message"] = "You are not authorized.";
        //            ViewBag.sms = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
        //                  "Danger" + "!</strong>" + "Select Question and Option" + ".</div>";
        //            //var Result = new { ObjQuestion = ObjQuestion, Message = ViewBag.Message };
        //            return View(ObjQuestion);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ParaGraph"] = new SelectList(db.Paras.Where(x => x.IsActive == true), "ParaId", "ParaText");
        //        ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == ObjQuestion.QuestionId).ToList();
        //        ViewBag.QuestionLevel = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
        //        if (ObjQuestion.QuestionId > 0)
        //        {
        //            ViewBag.Options = db.QuestionOptions.Where(x => x.QuestionId == ObjQuestion.QuestionId).ToList();
        //        }
        //        else
        //        {
        //            List<QuestionOption> PostedQuesOpts = new List<QuestionOption>();
        //            for (int i = 0; i < OptionText.Length; i++)
        //            {
        //                QuestionOption objnew = new QuestionOption() { ImageUrl = "", IsAnswer = false, OptionId = 0, OptionText = OptionText[i], QuestionId = 0 };
        //                if (i == AnswerIndex)
        //                {
        //                    objnew.IsAnswer = true;
        //                }
        //                PostedQuesOpts.Add(objnew);
        //            }

        //            ViewBag.Options = PostedQuesOpts;
        //        }
        //        Common.LogError("Error", ex);
        //        return View();
        //    }

        //}

        public ActionResult QuestionDetails(int QuestionId)
        {
            Question objQuestion = db.Questions.Where(x => x.QuestionId == QuestionId).FirstOrDefault();
            var objQuestionList = (from QuestionList in db.Questions

                                   where QuestionList.QuestionId == QuestionId
                                   select new QuestionView
                                   {
                                       QuestionId = QuestionList.QuestionId
                                       ,
                                       QuestionText = QuestionList.QuestionText
                                    ,

                                   }).FirstOrDefault();
            return View(objQuestionList);

        }

        public void QuizeGetPage(int? ParentId, int QuizId = 0)
        {

        }

        [HttpGet]
        public ActionResult Create(int? ClassId, int QuizId = 0, int QuestionId = 0)
        {
                   
               try
               {

                   int RoleID = ApplicationSession.CurrentUser.RoleId;
                   if (RoleID == 2)
                   {

                       var Result = (from QuizList in db.Quizs
                                     join QuizPaperMappingId in db.QuizPaperMappings on QuizList.QuizId equals QuizPaperMappingId.QuizId
                                     where ApplicationSession.CurrentUser.Id == QuizPaperMappingId.UserId
                                     where QuizPaperMappingId.QuizId == QuizId
                                     select new
                                     {
                                         QuizId = QuizList.QuizId,
                                         QuizeName = QuizList.QuizName,
                                     }).ToList();

                       if (Result.Count == 0)
                       {
                           return RedirectToAction("CustomError", "Error");
                       }
                       else
                       {
                          Quiz ObjQuiz = db.Quizs.Where(x => x.QuizId == QuizId).FirstOrDefault();

                           ViewBag.Questions = (from QList in db.Questions
                                                join QuizeDetilsList in db.QuizDetails on QList.QuestionId equals QuizeDetilsList.QuestionId
                                                where QuizeDetilsList.QuizId == QuizId
                                                select QList
                                                ).ToList();

                           ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList();
                           var result = ViewBag.Options1;
                           TempData["Options"] = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList();
                           var Test1 = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
                           var Test2 = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
                           ViewBag.QuestionLevel = Test1;
                           ViewBag.FeeType = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
                           TempData["ParaGraph"] = new SelectList(db.Paras.Where(x => x.IsActive == true), "ParaId", "ParaText");
                           TempData["PaperType"] = new SelectList(db.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName");
                           TempData["StudentClass"] = new SelectList(db.StudentClass.Distinct().Where(x => x.IsActive == true), "StudentClassId", "ClassName");
                           TempData["Subject"] = new SelectList(db.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
                           return View(ObjQuiz);
                       }

                   }
                   else
                   {

                     
                       Quiz ObjQuiz = db.Quizs.Where(x => x.QuizId == QuizId).FirstOrDefault();

                       ViewBag.Questions = (from QList in db.Questions
                                            join QuizeDetilsList in db.QuizDetails on QList.QuestionId equals QuizeDetilsList.QuestionId
                                            where QuizeDetilsList.QuizId == QuizId
                                            select QList
                                            ).ToList();

                       ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList();
                       var result = ViewBag.Options1;
                       TempData["Options"] = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList();
                       var Test1 = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
                       var Test2 = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
                       ViewBag.QuestionLevel = Test1;
                       ViewBag.FeeType = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
                       TempData["ParaGraph"] = new SelectList(db.Paras.Where(x => x.IsActive == true), "ParaId", "ParaText");
                       TempData["PaperType"] = new SelectList(db.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName");
                       //TempData["StudentClass"] = new SelectList(db.StudentClass.Distinct().Where(x => x.IsActive == true), "StudentClassId", "ClassName");
                       TempData["Subject"] = new SelectList(db.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
                       return View(ObjQuiz);
                   }


               }

               catch (Exception ex)
               {
                   Common.LogError("Error", ex);
                   return View();
               }
             
        }

        //
        // POST: /Quize/Create
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Quiz model, int? ClassId, int[] QuestionId, String QuizName,FormCollection collection)
        {
          
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                            //Create Here
                        Quiz _quiz = new Quiz();
                        _quiz.QuizName = model.QuizName;
                        _quiz.PaperTypeId = model.PaperTypeId;
                        _quiz.ClassId = Convert.ToInt32(collection["Class"]);
                        _quiz.SubjectId =model.SubjectId;
                        _quiz.FreeTypeId = model.FreeTypeId;
                        _quiz.Price = model.Price;
                        _quiz.TotalScore = model.TotalScore;
                        _quiz.TotalTimeInSecond = model.TotalTimeInSecond;
                        _quiz.CreatedBy = ApplicationSession.CurrentUser.Id;
                        _quiz.CreatedOn = DateTime.Now;

                        db.Quizs.Add(_quiz);
                 
                        db.SaveChanges();
             
                     
                        transaction.Commit();
                        return RedirectToAction("Index", "Quiz");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return View(model);
                        //return Ok(new { result = "error", message = ex.Message });
                    }
                }
          
           
        }

        [HttpGet]
        public ActionResult Edit(int quizId)
        {

            try
            {
                var quiz = db.Quizs.Find(quizId);
                int RoleID = ApplicationSession.CurrentUser.RoleId;
                if (RoleID == 1)
                {
                         
                        ViewBag.FeeType = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
                        TempData["PaperType"] = new SelectList(db.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName");
                        TempData["Subject"] = new SelectList(db.Subject.Where(x =>x.ClassId==quiz.ClassId && x.IsActive == true), "SubjectId", "SubjectName", quiz.SubjectId);
                        TempData["StudentClass"] = new SelectList(db.StudentClass.Where(x =>x.IsActive == true), "StudentClassId", "ClassName",quiz.SubjectId);
                       
                        //ViewBag.Subject = db.Subject.Distinct().Where(x => x.ClassId == quiz.ClassId && x.IsActive == true);
                }

                return View(quiz);

            }

            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View();
            }

          

        }

        //
        // POST: /Quize/Create
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Quiz model, int ClassId, int[] QuestionId, String QuizName, FormCollection collection)
        {

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                        var _quiz = db.Quizs.Find(model.QuizId);
                        TempData["PaperType"] = new SelectList(db.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName");
                        TempData["Subject"] = new SelectList(db.Subject.Where(x =>x.ClassId==_quiz.ClassId && x.IsActive == true));
                        TempData["StudentClass"] = new SelectList(db.StudentClass.Where(x => x.IsActive == true), "StudentClassId", "ClassName", model.ClassId);

                        //TempData["StudentClass"] = new SelectList(db.StudentClass.Where(x => x.IsActive == true), "StudentClassId", "ClassName");
                        //TempData["Subject"] = new SelectList(db.Subject.Distinct().Where(x => x.ClassId ==model.ClassId && x.IsActive == true), "SubjectId", "SubjectName",model.SubjectId);
                        ViewBag.FeeType = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
                        _quiz.QuizName = model.QuizName;
                        _quiz.ClassId = model.ClassId;
                        _quiz.SubjectId = model.SubjectId;
                        _quiz.PaperTypeId = model.PaperTypeId;
                        _quiz.Price = model.Price;
                        _quiz.TotalScore = model.TotalScore;
                        _quiz.TotalTimeInSecond = model.TotalTimeInSecond;
                        _quiz.IsActive = model.IsActive;
                        _quiz.IsPublish = model.IsPublish;
                        _quiz.ModifiedBy = ApplicationSession.CurrentUser.Id;
                        _quiz.ModifiedOn = DateTime.Now;

                    db.SaveChanges();

                    transaction.Commit();
                    return RedirectToAction("Index", "Quiz");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return View(model);
                    //return Ok(new { result = "error", message = ex.Message });
                }
            }


        }
        private void LoadData(Quiz model)
        {
            //if (!db.Categorys.Any(x => x.CategoryTypeId == 2))
            //{
                ViewBag.Questions = (from QList in db.Questions
                                     join QuizeDetilsList in db.QuizDetails on QList.QuestionId equals QuizeDetilsList.QuestionId
                                     where QuizeDetilsList.QuizId == model.QuizId
                                     select QList
                                                   ).ToList();

                ViewBag.QuestionLevel = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
                TempData["ParaGraph"] = new SelectList(db.Paras.Where(x => x.IsActive == true), "ParaId", "ParaText");
                TempData["PaperType"] = new SelectList(db.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName");
                TempData["StudentClass"] = new SelectList(db.StudentClass.Where(x => x.IsActive == true), "StudentClassId", "ClassName");
                TempData["Options"] = db.QuestionOptions.Where(x => x.QuestionId == 1).ToList();
                TempData["Subject"] = new SelectList(db.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
                ViewBag.FeeType = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
                ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == model.QuizId).ToList();
            //}
           
        }

        [HttpGet]
        public ActionResult Details(int QuizId)
        {

            try
            {
                ViewBag.Subject = db.Quizs.Where(x => x.QuizId == QuizId).Select(x => new { x.Subject.SubjectName }).FirstOrDefault();
                var quiz = db.Quizs.Find(QuizId);
                int RoleID = ApplicationSession.CurrentUser.RoleId;
               

                    Quiz ObjQuiz = db.Quizs.Where(x => x.QuizId == QuizId).FirstOrDefault();

                    ViewBag.Questions = (from QList in db.Questions
                                         join QuizeDetilsList in db.QuizDetails on QList.QuestionId equals QuizeDetilsList.QuestionId
                                         where QuizeDetilsList.QuizId == QuizId
                                         select QList
                                         ).ToList();

                 
                    ViewBag.FeeType = db.Keywords.Where(x => x.KeywordType == "FreeType" && x.KeywordValue != 3).ToList();
                    TempData["ParaGraph"] = new SelectList(db.Paras.Where(x => x.IsActive == true), "ParaId", "ParaText");
                    TempData["PaperType"] = new SelectList(db.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName");
                    TempData["Subject"] = new SelectList(db.Subject.Where(x =>x.ClassId ==quiz.ClassId && x.IsActive == true));
                    TempData["StudentClass"] = new SelectList(db.StudentClass.Where(x =>x.IsActive == true), "StudentClassId", "ClassName",quiz.ClassId);
                    return View(ObjQuiz);
              }

            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View();
            }

        }

        #region Local Method
        private dynamic getQuizData(Quiz ObjQuiz, int ClassId = 0, int SubjectId=0, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            int counter = 0;
            var Userid = ApplicationSession.CurrentUser.Id;
            var RoleId = ApplicationSession.CurrentUser.RoleId;
            if (RoleId == 1)
            {

                var objQuizeList = (from QuizeLIst in db.Quizs
                                    join Class in db.StudentClass on QuizeLIst.ClassId equals Class.Id
                                    join SubjectName in db.Subject on QuizeLIst.SubjectId equals SubjectName.Id 
                                    //join SubjectName in db.Subject on QuizeLIst.SubjectId equals SubjectName.SubjectId
                                    //join ClassName in db.StudentClass on SubjectName.SubjectId equals ClassName.SubjectId
                                    join FreeType in db.Keywords on QuizeLIst.FreeTypeId equals FreeType.KeywordValue
                                    join CreateBy in db.UserProfiles on QuizeLIst.CreatedBy equals CreateBy.Id
                                    //join QuiztestPaper in db.QuizPaperMappings on QuizeLIst.QuizId equals QuiztestPaper.QuizId
                                    where FreeType.KeywordType == "FreeType"
                                  && (ObjQuiz.QuizName == null || ObjQuiz.QuizName == "" || QuizeLIst.QuizName.Contains(ObjQuiz.QuizName))
                                    && (ObjQuiz.FreeTypeId == 0 || QuizeLIst.FreeTypeId == ObjQuiz.FreeTypeId)
                                    && (StartDate == null || EndDate == null || (QuizeLIst.CreatedOn >= StartDate && QuizeLIst.CreatedOn <= EndDate))
                                   && (ObjQuiz.ClassId==0 ||QuizeLIst.ClassId==ObjQuiz.ClassId) &&( ObjQuiz.SubjectId==0 ||QuizeLIst.SubjectId==ObjQuiz.SubjectId)

                                    select new
                                    {
                                        QuizId = QuizeLIst.QuizId,
                                        QuizName = QuizeLIst.QuizName,
                                        PaperTypeId = QuizeLIst.PaperTypeId,
                                        ClassId=Class.Id,
                                        SubjectId = QuizeLIst.SubjectId,
                                        TotalScore = QuizeLIst.TotalScore,
                                        TotalTimeInSecond = QuizeLIst.TotalTimeInSecond,
                                        FreeTypeId = FreeType.KeywordText,
                                        Price = QuizeLIst.Price,
                                        IsActive = QuizeLIst.IsActive,
                                        CreatedBy = CreateBy.UserName,
                                        CreatedOn = QuizeLIst.CreatedOn,
                                        CategoryId = QuizeLIst.SubjectId,
                                        IsPublish = QuizeLIst.IsPublish
                                    }).Distinct().ToList();

                ViewBag.QuizList = objQuizeList;

                ViewBag.QuizeList = objQuizeList.Count();

                return objQuizeList;
            }
            else
            {
                var objQuizeList = (from QuizeLIst in db.Quizs
                                    join ClassName in db.StudentClass on QuizeLIst.ClassId equals ClassName.Id
                                    join SubjectName in db.Subject on QuizeLIst.SubjectId equals SubjectName.Id 
                                    //join SubjectName in db.Subject on QuizeLIst.SubjectId equals SubjectName.SubjectId
                                    //join ClassName in db.StudentClass on SubjectName. equals ClassName.SubjectId
                                    join FreeType in db.Keywords on QuizeLIst.FreeTypeId equals FreeType.KeywordValue
                                    join CreateBy in db.UserProfiles on QuizeLIst.CreatedBy equals CreateBy.Id
                                    join QuiztestPaper in db.QuizPaperMappings on QuizeLIst.QuizId equals QuiztestPaper.QuizId
                                    where FreeType.KeywordType == "FreeType"
                                  && (ObjQuiz.QuizName == null || ObjQuiz.QuizName == "" || QuizeLIst.QuizName.Contains(ObjQuiz.QuizName))
                                    && (ObjQuiz.FreeTypeId == 0 || QuizeLIst.FreeTypeId == ObjQuiz.FreeTypeId)
                                    && (StartDate == null || EndDate == null || (QuizeLIst.CreatedOn >= StartDate && QuizeLIst.CreatedOn <= EndDate))
                                    && (ClassId == 0 || ClassName.Id == ClassId || SubjectId==0 || SubjectName.Id == SubjectId)
                                    && ApplicationSession.CurrentUser.Id == QuiztestPaper.UserId
                                    select new
                                    {
                                        QuizId = QuizeLIst.QuizId,
                                        QuizName = QuizeLIst.QuizName,
                                        PaperTypeId=QuizeLIst.PaperTypeId,

                                        SubjectId = SubjectName.Id,
                                        TotalScore = QuizeLIst.TotalScore,
                                        TotalTimeInSecond = QuizeLIst.TotalTimeInSecond,
                                        FreeTypeId = FreeType.KeywordText,
                                        Price = QuizeLIst.Price,
                                        IsActive = QuizeLIst.IsActive,
                                        CreatedBy = CreateBy.UserName,
                                        CreatedOn = QuizeLIst.CreatedOn,
                                        CategoryId = QuizeLIst.SubjectId,
                                        IsPublish = QuizeLIst.IsPublish
                                    }).Distinct().ToList();
                ViewBag.QuizList = objQuizeList;
                ViewBag.QuizeList = objQuizeList.Count();

                return objQuizeList;
            }



        }
        #endregion


      
        [HttpPost]
        public ActionResult CreatePackage(PackageViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    
                    int packageId =PackageViewModel.Parse(model).PackageId;
                    Package pkg = new Package();
                    PackageDetails pkgdetails = new PackageDetails();
                  
                    pkg.PackageName = model.PackageName;
                    pkg.PackageImage = model.PackageImage;
                    pkg.Description = model.Description;
                    pkg.ClassId = model.ClassId;
                    pkg.SubjectId = model.SubjectId;
                    pkg.CreatedBy = Convert.ToInt32(User.Identity.Name);
                    pkg.CreatedOn = DateTime.Now;
                    pkg.ModifiedBy = Convert.ToInt32(User.Identity.Name);
                    pkg.ModifiedOn = DateTime.Now;
                    
                    pkg.IsActive = true;
                    foreach (var id in model.QuizesId)
                    {
                        pkgdetails.PackageId = packageId;
                        pkgdetails.TestPaperId = Convert.ToInt32(model.QuizesId);
                        db.PackageDetails.Add(pkgdetails);
                        db.SaveChanges();
                    }
                   
                    db.Package.Add(pkg);
                    db.SaveChanges();
                    RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save change,please try again");
                }
            }
           

            return View();
        }

        [HttpPut]
        public ActionResult UpdatePackage(int id)
        {
            var package = db.Package.All(x => x.PackageId == id);
            if (package == null)
                ModelState.AddModelError("", "not found");
            using(var transaction=db.Database.BeginTransaction())
            {
                try
                {
                    // update pacakage
                    db.Package.Find(id);

                }
                catch(Exception ex)
                {

                }
            }
            return View();
        }

    }
}
