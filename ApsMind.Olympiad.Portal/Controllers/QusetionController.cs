using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Portal.Models;
using System.IO;
using System.Data.Entity;
using PagedList;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize]
    [ValidateInput(false)]
    public class QusetionController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        //
        // GET: /Qusetion/
        //public ActionResult Index()
        //{
        //    SeachQusetion ObjSeachQuestion = new SeachQusetion();
        //    ViewBag.ParentCategoryId = 0;
        //    List<UserProfile> lst = (from cretedbylist in objDB.Questions
        //                             join createdby in objDB.UserProfiles on cretedbylist.CreatedBy equals createdby.UserId
        //                             select createdby
        //                           ).Distinct().ToList();
        //    ViewBag.createdBy = new SelectList(lst.ToList(), "UserId", "Name");
        //    ObjSeachQuestion.ListQuizView = new List<_ViewAllQuestions>(); //GetQuestionList(null, null, null, 1, 0, 0, 0, 0, 10, null);
        //    ViewBag.QuestionLevelQuestion = new SelectList(objDB.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList(), "KeywordValue", "KeywordText");
        //    return View(ObjSeachQuestion);
        //}




        //[HttpPost]
        //public ActionResult Index(String QuestionText, DateTime? StartDate, DateTime? EndDate, int QuestionNumber = 0, int QuestionLevelId = 0, int CreatedBy = 0, int SubjectId = 0, int ParentCategoryId = 0, int PerPage = 0, String TestPaperName = null)
        //{

        //    Common.SetSearchConditionValue(Common.searchConditions, "QuestionText", QuestionText);
        //    Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
        //    Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
        //    Common.SetSearchConditionValue(Common.searchConditions, "QuestionNumber", QuestionNumber);
        //    Common.SetSearchConditionValue(Common.searchConditions, "QuestionLevelId", QuestionLevelId);
        //    Common.SetSearchConditionValue(Common.searchConditions, "CreatedBy", CreatedBy);
        //    Common.SetSearchConditionValue(Common.searchConditions, "SubjectId", SubjectId);
        //    Common.SetSearchConditionValue(Common.searchConditions, "ParentCategoryId", ParentCategoryId);
        //    Common.SetSearchConditionValue(Common.searchConditions, "TestPaperName", TestPaperName);
        //    Common.SetSearchConditionValue(Common.searchConditions, "PerPage", PerPage);
        //    TempData["SearchConditions"] = Common.searchConditions;
        //    ViewBag.QuestionNumber = QuestionNumber;
        //    ViewBag.QuestionText = QuestionText;
        //    ViewBag.StartDate = StartDate;
        //    ViewBag.EndDate = EndDate;
        //    ViewBag.QuizName = TestPaperName;
        //    List<UserProfile> lst = (from cretedbylist in objDB.Questions
        //                             join createdby in objDB.UserProfiles on cretedbylist.CreatedBy equals createdby.UserId
        //                             select createdby

        //                            ).Distinct().ToList();
        //    ViewBag.createdBy = new SelectList(lst.ToList(), "UserId", "Name");
        //    SeachQusetion ObjSeachQuestion = new SeachQusetion();
        //    ObjSeachQuestion.ListQuizView = GetQuestionList(QuestionText, StartDate, EndDate, QuestionNumber, QuestionLevelId, CreatedBy, SubjectId, ParentCategoryId, PerPage, TestPaperName);
        //    ViewBag.ParentCategoryId = ParentCategoryId;


        //    ViewBag.QuestionLevelQuestion = new SelectList(objDB.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList(), "KeywordValue", "KeywordText");
        //    return View(ObjSeachQuestion);
        //}

        public ActionResult GetPrint(int id)
        {
            ViewQuizDetails objViewQuizDetails = GetQuiz(id);
            if (objViewQuizDetails == null)
                return RedirectToAction("Index");

            return View(objViewQuizDetails);
        }




        [HttpGet]
        public ActionResult AddParaGraph()
        {
            return PartialView(new Para());
        }

        [HttpGet]
        public ActionResult QuizPrint(int id)
        {
            ViewQuizDetails objViewQuizDetails = GetQuiz(id);
            if (objViewQuizDetails == null)
                return RedirectToAction("Index");

            return View(objViewQuizDetails);

        }

        [HttpPost]
        public ActionResult AddParaGraph(Para ObjPara, HttpPostedFileBase ImageUrl)
        {

            try
            {
                string ParaImagePath = "";
                string ImagePath = "/Images/ParaImage";
                if (ImageUrl != null && ImageUrl.ContentLength > 0)
                {
                    ParaImagePath = Path.GetFileName(ImageUrl.FileName);
                    String FileName = ImagePath + "/" + ParaImagePath + Path.GetExtension(ImageUrl.FileName);
                    ImageUrl.SaveAs(Server.MapPath("~" + FileName));
                    ObjPara.ImageUrl = FileName;
                }

                ObjPara.CreatedBy = ApplicationSession.CurrentUser.Id;
                ObjPara.CreatedOn = System.DateTime.Now;
                db.Paras.Add(ObjPara);
                db.SaveChanges();
                ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                            "Success" + "!</strong>" + " New record created successfully" + ".</div>";
                return RedirectToAction("Create");



            }
            catch
            {

                return View(ObjPara);
            }
        }

        //
        // GET: /Qusetion/Create
        public ActionResult Create(int quizId,int QuestionId)
        {
            int RoleID = ApplicationSession.CurrentUser.RoleId;
            var quiz = db.Quizs.Where(x => x.QuizId == quizId).FirstOrDefault();
                QuestionGetPage();
                Question ObjQuestion = db.Questions.Where(x => x.QuestionId == QuestionId).FirstOrDefault();
               
                int ClassId = quiz.ClassId;
                int SubjectId = quiz.SubjectId;
                int PaperTypeId = quiz.PaperTypeId;
                ViewBag.Options = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList();
                ViewBag.ParaId = db.Paras.Where(x => x.IsActive == true).ToList();
                ViewBag.ChapterId = db.Chapter.Where(x => x.ClassId == ClassId && x.SubjectId == SubjectId && x.PaperTypeId == PaperTypeId && x.IsActive == true).ToList();
               
                ViewBag.QuizID = quizId;
                return View(ObjQuestion);
       }



        public void QuestionGetPage()
        {
            ViewBag.QuizeId = db.Quizs.Where(x => x.IsActive == true).ToList();
            ViewBag.ParaId = db.Paras.Where(x => x.IsActive == true).ToList();
            ViewBag.QuestionLevel1 = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
            var result = ViewBag.QuestionLevel;

        }

        //
        // POST: /Qusetion/Create
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(AddQuestionViewModel model, HttpPostedFileBase ImageUrl, HttpPostedFileBase[] optionImage, String[] OptionText, int? AnswerIndex)
        {
           Handler.ImageHandler objImageHandler = new Handler.ImageHandler();
            string ImagePath = "/Images/QuestionImage";
            string ImagePathOption = "/Images/QuestionOptionImage";
            int qId = Convert.ToInt32(Request["QuizID"]);
            var _quiz = db.Quizs.Where(x => x.QuizId ==qId).FirstOrDefault();
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (AnswerIndex != -1 && model.QuestionText != null || Url != null && model.QuestionLevel != 0)
                    {
                       var _question = db.Questions.Where(x => x.QuestionId == model.QuestionId).FirstOrDefault();
                    
                       _question.QuestionText = model.QuestionText;
                       if (ImageUrl != null && ImageUrl.ContentLength > 0)
                       {
                           String FileName = objImageHandler.SaveFile(ImagePath + "/", Path.GetExtension(ImageUrl.FileName), ImageUrl.InputStream);
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
                      db.SaveChanges();
                      ViewBag.ParaId = db.Paras.Where(x => x.IsActive == true).ToList();
                      ViewBag.ChapterId = db.Chapter.Where(x => x.ClassId ==_quiz.ClassId && x.SubjectId == _quiz.SubjectId && x.PaperTypeId ==_quiz.PaperTypeId && x.IsActive == true).ToList();
                      model.QuestionLevelList = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();

                      var _questionOption = db.QuestionOptions.Where(x => x.QuestionId == model.QuestionId).ToList();                     
                        for (int i=0;i<OptionText.Count();i++)
                        {
                           var File = optionImage[i];
                            if (File != null && File.ContentLength > 0)
                            {
                                String FileName = objImageHandler.SaveFile(ImagePathOption + "/", Path.GetExtension(File.FileName), File.InputStream);
                                _questionOption[i].ImageUrl = FileName;

                            }
                            //else
                            //{
                            //    _questionOption.ImageUrl = (HiddenImagePath[i]) == string.Empty ? null : (HiddenImagePath[i]);
                            //}
                            
                           _questionOption[i].OptionText = OptionText[i].ToString();
                           _questionOption[i].QuestionId = _question.QuestionId;
                         
                           _questionOption[i].IsAnswer = AnswerIndex == i ? true : false;                            
                        }
                        db.SaveChanges();
                        //QuizDetail _quizDetail = new QuizDetail();
                        //_quizDetail.QuizId =model.QuizId;
                        //_quizDetail.QuestionId = _question.QuestionId;
                        //db.QuizDetails.Add(_quizDetail);
                        //db.SaveChanges();
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
                       
                    }
                   
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error Occured", ex.Message);
                    transaction.Rollback();
                    //model.ParagraphList = db.Paras.Where(x => x.IsActive == true).ToList();
                    //model.ChapterList = db.Chapter.Where(x => x.ClassId == model.ClassId && x.SubjectId == model.CategoryId && x.PaperTypeId == model.PaperTypeId && x.IsActive == true).ToList();
                    //model.QuestionLevelList = db.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList();
                    //ViewBag.Options1 = db.QuestionOptions.Where(x => x.QuestionId == model.QuestionId).ToList();
                    //if (model.QuestionId > 0)
                    //{
                    //    ViewBag.Options = db.QuestionOptions.Where(x => x.QuestionId == model.QuestionId).ToList();
                    //}
                    //else
                    //{
                    //    List<QuestionOption> PostedQuesOpts = new List<QuestionOption>();
                    //    for (int i = 0; i < OptionText.Length; i++)
                    //    {
                    //        QuestionOption objnew = new QuestionOption() { ImageUrl = "", IsAnswer = false, OptionId = 0, OptionText = OptionText[i], QuestionId = 0 };
                    //        if (i == AnswerIndex)
                    //        {
                    //            objnew.IsAnswer = true;
                    //        }
                    //        PostedQuesOpts.Add(objnew);
                    //    }

                    //    ViewBag.Options = PostedQuesOpts;
                    //}
                    Common.LogError("Error", ex);
                    return View(model);
                }
            }
            return RedirectToAction("Details", "Quiz", new { @ClassId =_quiz.ClassId, @SubjectId =_quiz.SubjectId, @QuizId =_quiz.QuizId});
        }

        public ActionResult Remove(Int32 QuestionId = 0, Int32 QuizId = 0, int ParentId = 0)
        {

            var ObjQuestionExit = db.Questions.Where(x => x.QuestionId == QuestionId).FirstOrDefault();
            int? Time = ObjQuestionExit.EstimatedTimeInSecond;
            Double Marks = ObjQuestionExit.Mark;
            var objQuizExit = db.Quizs.Where(X => X.QuizId == QuizId).FirstOrDefault();
            int? TotalTime = objQuizExit.TotalTimeInSecond - Time;
            Double TotalMarks = objQuizExit.TotalScore - Marks;
            objQuizExit.TotalScore = TotalMarks;
            objQuizExit.TotalTimeInSecond = (int)TotalTime;

            db.Entry(objQuizExit).CurrentValues.SetValues(objQuizExit);
            db.Entry(objQuizExit).State = EntityState.Modified;
            db.SaveChanges();

            db.Questions.Remove(ObjQuestionExit);

            var objQuestionOption = db.QuestionOptions.Where(x => x.QuestionId == QuestionId).ToList();
            db.QuestionOptions.RemoveRange(objQuestionOption);

            var objQuizDetail = db.QuizDetails.Where(x => x.QuestionId == QuestionId).ToList();
            db.QuizDetails.RemoveRange(objQuizDetail);
            db.SaveChanges();
            TempData["DeleteQuestion"] = " Record has  been Delete successfully!";
            return RedirectToAction("Create", "Quiz", new { QuizId = QuizId, ParentId = ParentId });


        }

        #region "User Method"

        private ViewQuizDetails GetQuiz(int QuizId)
        {
            ViewQuizDetails varQuiz = (from q in db.Quizs
                                       join c in db.Categorys on q.SubjectId equals c.Id
                                       join i in db.Instructions on q.QuizId equals i.QuizId into iTemp
                                       from i in iTemp.DefaultIfEmpty()
                                       where q.QuizId == QuizId
                                       select new ViewQuizDetails
                                       {
                                           QuizId = q.QuizId,
                                           QuizName = q.QuizName,
                                           CategoryName = c.CategoryName,
                                           Instruction = i.InstructionText
                                       }).FirstOrDefault();

            if (varQuiz == null)
                return null;

            List<ViewQuestion> varQuizDetails = (from qd in db.QuizDetails
                                                 join q in db.Questions on qd.QuestionId equals q.QuestionId
                                                 join p in db.Paras on q.ParaId equals p.ParaId into pTemp
                                                 from p in pTemp.DefaultIfEmpty()
                                                 where qd.QuizId == varQuiz.QuizId
                                                       && q.IsActive == true
                                                 select new ViewQuestion
                                                 {
                                                     QuestionId = qd.QuestionId,
                                                     QuestionText = q.QuestionText,
                                                     ImageUrl = q.ImageUrl,
                                                     Mark = q.Mark,
                                                     ParaId = q.ParaId,
                                                     ParaText = p.ParaText,
                                                     ParaImage = p.ImageUrl,
                                                 }
                                  ).ToList();

            int?[] paraId = new int?[varQuizDetails.Distinct().Count(x => x.ParaId != null)];
            int counter = 0;
            foreach (var que in varQuizDetails)
            {
                que.Options = db.QuestionOptions.Where(x => x.QuestionId == que.QuestionId).ToList();
                if (que.ParaId != null && paraId.Count(x => x.Equals(que.ParaId)) == 0)
                {
                    paraId[counter++] = que.ParaId;
                }
                else
                {
                    que.ParaId = null;
                }
            }

            varQuiz.QuestionList = varQuizDetails;
            return varQuiz;
        }

       // private List<_ViewAllQuestions> GetQuestionList(String QuestionText, DateTime? StartDate, DateTime? EndDate, int? QuestionNumber, int? QuestionLevelId, int? CreatedBy, int? SubjectId = 0, int? ParentCategoryId = 0, int? PerPage = 10, String TestPaperName = null)
       // {
       //     var result = objDB.Questions.ToList();
       //     var CUser = ApplicationSession.CurrentUser;


       //     var objQusetionList = (from QUsetionLIst in objDB.Questions
       //                            join QuestionLevel in objDB.Keywords on QUsetionLIst.QuestionLevel equals QuestionLevel.KeywordValue
       //                            join CategoryName in objDB.Categorys on QUsetionLIst.CategoryId equals CategoryName.CategoryId
       //                            join ClassName in objDB.Categorys on CategoryName.ParentCategoryId equals ClassName.CategoryId
       //                            join QuizID in objDB.QuizDetails on QUsetionLIst.QuestionId equals QuizID.QuestionId
       //                            join QuizMappingID in objDB.QuizPaperMappings on QuizID.QuizId equals QuizMappingID.QuizId into qpmlist
       //                            from qpm in qpmlist.DefaultIfEmpty()
       //                            join QuizIsPublish in objDB.Quizs on QuizID.QuizId equals QuizIsPublish.QuizId
       //                            join ParaName in objDB.Paras on QUsetionLIst.ParaId equals ParaName.ParaId into dt
       //                            from d in dt.DefaultIfEmpty()
       //                            join OptionText in objDB.QuestionOptions on QUsetionLIst.QuestionId equals OptionText.QuestionId
       //                            where QuestionLevel.KeywordType == "QuestionLevel"
       //                            && (QUsetionLIst.QuestionText.Contains(QuestionText)
       //                                   || QuestionText == null || QuestionText == "")
       //                                   && ((QuizIsPublish.QuizName == TestPaperName
       //                                          || TestPaperName == null || TestPaperName == ""))
       //                            && (QuestionLevelId == 0 || QuestionLevelId == 0 || QUsetionLIst.QuestionLevel == QuestionLevelId)
       //                            && (QuestionNumber == 0 || QuestionNumber == 0 || QUsetionLIst.QuestionId == QuestionNumber)
       //                            && (CreatedBy == 0 || CreatedBy == 0 || QUsetionLIst.CreatedBy == CreatedBy)
       //                            && (StartDate == null || EndDate == null || (QUsetionLIst.CreatedOn >= StartDate && QUsetionLIst.CreatedOn <= EndDate))
       //                           && (ParentCategoryId == 0 || ClassName.CategoryId == ParentCategoryId || CategoryName.CategoryId == ParentCategoryId)
       //                            && (CUser.RoleId == 1 || ApplicationSession.CurrentUser.UserId == qpm.UserId)

       //                            group OptionText by new

       //                            {
       //                                QUsetionLIst.QuestionId,
       //                                QUsetionLIst.QuestionText,
       //                                QuestionLevel.KeywordText,
       //                                QUsetionLIst.CategoryId,
       //                                QUsetionLIst.ImageUrl,
       //                                CategoryName.CategoryName,
       //                                d.ParaText,
       //                                QuizIsPublish.IsPublish,
       //                                QuizID.QuizId,
       //                                QuizIsPublish.QuizName
       //                            }

       //                                into grouped

       //                                select new QuestionView
       //                                {
       //                                    NumberInGroup = grouped.Count(),
       //                                    Options = grouped.ToList(),
       //                                    QuestionId = grouped.Key.QuestionId,
       //                                    QuestionText = grouped.Key.QuestionText,
       //                                    QuestionLevel = grouped.Key.KeywordText,
       //                                    CategoryId = grouped.Key.CategoryId,
       //                                    QuestionImage = grouped.Key.ImageUrl,
       //                                    CategoryName = grouped.Key.CategoryName,
       //                                    IsPublish = grouped.Key.IsPublish,
       //                                    // ParaName = (d != null ? d.ParaText : String.Empty),
       //                                    ParaName = grouped.Key.ParaText,
       //                                    QuizId = grouped.Key.QuizId,
       //                                    QuizName = grouped.Key.QuizName

       //                                }


       //                            //select new QuestionView


       //).ToList();

       //     List<_ViewAllQuestions> quizList = (from item in objQusetionList
       //                                         group item.QuizId by new { item.QuizId, item.QuizName, item.IsPublish, item.CategoryId, item.CategoryName } into grouped

       //                                         select new _ViewAllQuestions
       //                                         {
       //                                             QuizId = grouped.Key.QuizId,
       //                                             QuizName = grouped.Key.QuizName,
       //                                             IsPublish = grouped.Key.IsPublish,
       //                                             CategoryId = grouped.Key.CategoryId,
       //                                             CategoryName = grouped.Key.CategoryName

       //                                         }).Distinct().ToList();


       //     var ParaList = (from item in quizList
       //                     join para in objQusetionList on new { a = item.QuizId } equals new { a = para.QuizId }
       //                     group para.ParaName by new { para.QuizId, para.ParaName } into grouped
       //                     select new
       //                     {

       //                         grouped.Key.QuizId,
       //                         grouped.Key.ParaName


       //                     }).Distinct().ToList();

       //     foreach (var q in quizList)
       //     {

       //         var paraname = ParaList.Where(x => x.QuizId == q.QuizId).Distinct().ToList();

       //         //////para extracted
       //         foreach (var para in paraname)
       //         {
       //             _Paragraph obj = new _Paragraph();
       //             obj.paratext = para.ParaName;
       //             //adding related questions
       //             List<_Question> allQnsinpara = (from qns in objQusetionList
       //                                             where qns.QuizId == q.QuizId && qns.ParaName == obj.paratext
       //                                             select new _Question
       //                                             {
       //                                                 QuestionId = qns.QuestionId,
       //                                                 QuestionImage = qns.QuestionImage,
       //                                                 QuestionLevelId = qns.QuestionLevelId,
       //                                                 QuestionText = qns.QuestionText,
       //                                                 Options = qns.Options


       //                                             }).ToList();
       //             obj.questionList = allQnsinpara;
       //             //question adding closed
       //             /////////para sdded
       //             q.paragraph.Add(obj);
       //         }



       //     }


       //     //objQusetionList = objQusetionList.Take(PerPage).ToList();
       //     var TotalQuestion = objQusetionList.Count();
       //     ViewBag.TotalQuestion = TotalQuestion;
       //     return quizList;



       // }

        #endregion
    }
}
