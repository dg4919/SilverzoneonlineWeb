using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize(new string[] { "1", "3" })]
    [ValidateInput(false)]
    public class ChapterController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Para/
        public ActionResult Index(String ChapterName, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                ViewBag.ChapterName = ChapterName;
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
                if (ApplicationSession.CurrentUser.RoleId == 1)
                {
                    var objChapterList = (from ChapterLIst in objDB.Chapter
                    join CreateBy in objDB.UserProfiles on ChapterLIst.CreatedBy equals CreateBy.Id
                     orderby ChapterLIst.ChapterId
                                        where (ChapterLIst.ChapterName.Contains(ChapterName)
                                         || ChapterName == null || ChapterName == "")
                                          && (StartDate == null || EndDate == null || (ChapterLIst.CreatedOn >= StartDate && ChapterLIst.CreatedOn <= EndDate))

                                        select new
                                        {

                                            ChapterId = ChapterLIst.ChapterId,
                                            ChapterName = ChapterLIst.ChapterName,
                                            Description=ChapterLIst.Description,
                                            Subject=ChapterLIst.Subject.SubjectName,
                                            Class=ChapterLIst.Class.ClassName,
                                            PaperType=ChapterLIst.PaperType.PaperTypeName,
                                            IsActive = ChapterLIst.IsActive,
                                            CreatedBy = CreateBy.UserName,
                                            CreatedOn = ChapterLIst.CreatedOn
                                         ,

                                        }).ToList();
                    var Count = objChapterList.Count();
                    ViewBag.TotalChapter = Count;
                    return View(objChapterList);
                }
                else
                {
                    var objChapterList = (from ChapterLIst in objDB.Chapter

                                        join CreateBy in objDB.UserProfiles on ChapterLIst.CreatedBy equals CreateBy.Id
                                        orderby ChapterLIst.ChapterId
                                        where (ChapterLIst.ChapterName.Contains(ChapterName)
                                          || ChapterName == null || ChapterName == "")
                                           && (StartDate == null || EndDate == null || (ChapterLIst.CreatedOn >= StartDate && ChapterLIst.CreatedOn <= EndDate))
                                            && (ApplicationSession.CurrentUser.Id == ChapterLIst.CreatedBy)
                                        select new
                                        {

                                            ChapterId = ChapterLIst.ChapterId,
                                            ChapterName = ChapterLIst.ChapterName,
                                            Description=ChapterLIst.Description,
                                            ClassId = ChapterLIst.ClassId,
                                            SubjectId = ChapterLIst.SubjectId,
                                            PaperTypeId=ChapterLIst.PaperTypeId,
                                            IsActive = ChapterLIst.IsActive,
                                            CreatedBy = CreateBy.UserName,
                                            CreatedOn = ChapterLIst.CreatedOn
                                         ,

                                        }).ToList();
                    var Count = objChapterList.Count();
                    ViewBag.TotalChapter = Count;
                    return View(objChapterList);
                }


            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View();
            }
        }

        //
        // GET: /Para/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSubjects(int id)
        {
            var sublist = objDB.Subject.Where(x=>x.ClassId==id && x.IsActive==true);
            // Console.Write(classlist);
            return Json(sublist, JsonRequestBehavior.AllowGet);

        }
        
        //
        // GET: /Chapter/Create
        public ActionResult Create()
        {
            TempData["StudentClass"] = new SelectList(objDB.StudentClass.Where(x => x.IsActive == true), "Id", "ClassName");
               //TempData["Subject"] = new SelectList(objDB.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
                TempData["PaperType"] = new SelectList(objDB.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName");
               return View();
           
        }

        //
        // POST: /Chapter/Create
        [HttpPost]
        public ActionResult Create(Chapter model, FormCollection collection)
        {
            try
            {
                model.SubjectId = Convert.ToInt32(collection["Subject"]);
                if (model.ChapterName != null)
                {
                    Chapter _chapter = new Chapter();
                    _chapter.ChapterName = model.ChapterName;
                    _chapter.Description = model.Description;
                    _chapter.ClassId = model.ClassId;
                    _chapter.SubjectId = model.SubjectId;
                    _chapter.PaperTypeId = model.PaperTypeId;
                    _chapter.IsActive = true;
                    _chapter.CreatedBy = ApplicationSession.CurrentUser.Id;
                    _chapter.CreatedOn = DateTime.Now;
                     objDB.Chapter.Add(_chapter);
                     objDB.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                      "Error" + "!</strong>" + "Please Enter Chapter" + ".</div>";
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View(model);
            }
        }

        public ActionResult Edit(int ChapterId)
        {
            Chapter objChapter = objDB.Chapter.Where(x => x.ChapterId == ChapterId).FirstOrDefault();
            TempData["StudentClass"] = new SelectList(objDB.StudentClass.Where(x => x.IsActive == true), "StudentClassId", "ClassName", objChapter.ClassId);
            TempData["Subject"] = new SelectList(objDB.Subject.Where(x =>x.ClassId==objChapter.ClassId && x.IsActive == true), "SubjectId", "SubjectName",objChapter.SubjectId);
            
            TempData["PaperType"] = new SelectList(objDB.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName",objChapter.PaperTypeId);
            return View(objChapter);
        }

        [HttpPost]
        public ActionResult Edit(Chapter model,int ChapterId)
        {
            Chapter objChapter = objDB.Chapter.Where(x => x.ChapterId == ChapterId).FirstOrDefault();
            TempData["Subject"] = new SelectList(objDB.Subject.Where(x =>x.ClassId==objChapter.ClassId && x.IsActive == true), "SubjectId", "SubjectName", objChapter.SubjectId);
            TempData["StudentClass"] = new SelectList(objDB.StudentClass.Where(x => x.IsActive == true), "StudentClassId", "ClassName", objChapter.ClassId);
            TempData["PaperType"] = new SelectList(objDB.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName", objChapter.PaperTypeId);

            objChapter.ChapterName = model.ChapterName;
            objChapter.Description = model.Description;
            objChapter.SubjectId = model.SubjectId;
            objChapter.ClassId = model.ClassId;
            objChapter.PaperTypeId = model.PaperTypeId;
            objChapter.IsActive = model.IsActive;
            objChapter.ModifiedBy = ApplicationSession.CurrentUser.Id;
            objChapter.ModifiedOn = DateTime.Now;
            objDB.SaveChanges();
            return RedirectToAction("Index", "Chapter");
        }
    }
}