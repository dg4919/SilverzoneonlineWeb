using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Portal.Models;
using System.Data.Entity;
namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize]
    public class QuizPaperMappingController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /DataEntryOpeartor/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /DataEntryOpeartor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public void TestPaperMappingList(int QuizId = 0, int UserId = 0)
        {
            var RoleID = ApplicationSession.CurrentUser.RoleId;
            var UserID = ApplicationSession.CurrentUser.Id;
            if (ApplicationSession.CurrentUser.RoleId == 1)
            {
                ViewBag.TestPaperMapping = (from QuizpaperList in objDB.QuizPaperMappings
                                            join QuizeName in objDB.Quizs on QuizpaperList.QuizId equals QuizeName.QuizId
                                            join CreateBy in objDB.UserProfiles on QuizpaperList.CreatedBy equals CreateBy.Id
                                            join UserName in objDB.UserProfiles on QuizpaperList.UserId equals UserName.Id
                                            where (QuizId == 0 || QuizId == 0 || QuizpaperList.QuizId == QuizId)
                                            && (UserId == 0 || UserId == 0 || QuizpaperList.UserId == UserId)
                                            select new
                                            {
                                                Id = QuizpaperList.Id,
                                                QuizId = QuizeName.QuizName,
                                                UserId = UserName.UserName,
                                                CreatedBy = CreateBy.UserName,
                                                CreatedOn = QuizpaperList.CreatedOn,


                                            }).ToList();

            }
            else
            {
                ViewBag.TestPaperMapping = (from QuizpaperList in objDB.QuizPaperMappings
                                            join QuizeName in objDB.Quizs on QuizpaperList.QuizId equals QuizeName.QuizId
                                            join CreateBy in objDB.UserProfiles on QuizpaperList.CreatedBy equals CreateBy.Id
                                            join UserName in objDB.UserProfiles on QuizpaperList.UserId equals UserName.Id
                                            where (QuizId == 0 || QuizId == 0 || QuizpaperList.QuizId == QuizId)
                                            && (UserID == 0 || UserID == 0 || QuizpaperList.UserId == UserID)
                                            select new
                                            {
                                                Id = QuizpaperList.Id,
                                                QuizId = QuizeName.QuizName,
                                                UserId = UserName.UserName,
                                                CreatedBy = CreateBy.UserName,
                                                CreatedOn = QuizpaperList.CreatedOn,


                                            }).ToList();

            }
            if (RoleID == 1)
            {
                ViewBag.QuizeId = (from QuizList in objDB.Quizs

                                   select new
                                   {
                                       QuizId = QuizList.QuizId,
                                       QuizeName = QuizList.QuizName,




                                   }).ToList();
            }
            else
            {
                ViewBag.QuizeId = (from QuizList in objDB.Quizs
                                   join QuizPaperMappingId in objDB.QuizPaperMappings on QuizList.QuizId equals QuizPaperMappingId.QuizId
                                   where ApplicationSession.CurrentUser.Id == QuizPaperMappingId.UserId
                                   select new
                                   {
                                       QuizId = QuizList.QuizId,
                                       QuizeName = QuizList.QuizName,




                                   }).ToList();
            }
            if (RoleID == 1)
            {
                ViewBag.UserRole = objDB.UserProfiles.Where(x => x.IsActive == true && x.RoleId == 3).ToList();
            }
            else
            {
                ViewBag.UserRole = objDB.UserProfiles.Where(x => x.IsActive == true && x.Id == ApplicationSession.CurrentUser.Id).ToList();
            }
            var result = ViewBag.TestPaperMapping;
        }

        
        public ActionResult Create(int Id = 0, int QuizId = 0, int UserId = 0)
        {
            var RoleID = ApplicationSession.CurrentUser.RoleId;

            ViewBag.RoleId = RoleID;
            TestPaperMappingList(QuizId, UserId);
            QuizPaperMapping ObjQuizPaperMapping = objDB.QuizPaperMappings.Where(x => x.Id == Id).FirstOrDefault();
            return View(ObjQuizPaperMapping);
        }

        //
        // POST: /DataEntryOpeartor/Create
        [HttpPost]
        public ActionResult Create(QuizPaperMapping ObjQuizPaperMapping, String Command)
        {
            try
            {
                var RoleID = ApplicationSession.CurrentUser.RoleId;
                if (Command == "Save")
                {

                    QuizPaperMapping QuizPaperMappingExit1 = objDB.QuizPaperMappings.Where(x => x.UserId == ObjQuizPaperMapping.UserId && x.QuizId == ObjQuizPaperMapping.QuizId).FirstOrDefault();
                    if (QuizPaperMappingExit1 == null)
                    {
                        ObjQuizPaperMapping.CreatedBy = ApplicationSession.CurrentUser.Id;
                        ObjQuizPaperMapping.CreatedOn = System.DateTime.Now;
                        objDB.QuizPaperMappings.Add(ObjQuizPaperMapping);
                        TestPaperMappingList();
                        ViewBag.RoleId = RoleID;
                        ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
               "Success" + "!</strong>" + " New record created successfully" + ".</div>";
                    }
                    else
                    {
                        TestPaperMappingList();
                        ViewBag.QuizeId = objDB.Quizs.Where(x => x.IsActive == true).ToList();
                        ViewBag.UserRole = objDB.UserProfiles.Where(x => x.IsActive == true && x.RoleId == 3).ToList();
                        ViewBag.Message = "<div class=\"alert alert-warning\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                            "Info" + "!</strong>" + "Record Alredy Exit" + ".</div>";
                        TestPaperMappingList();
                        ViewBag.RoleId = RoleID;
                        return View(ObjQuizPaperMapping);
                    }



                }

                else
                {
                    ModelState.Clear();
                    ViewBag.RoleId = RoleID;
                    TestPaperMappingList(ObjQuizPaperMapping.QuizId, ObjQuizPaperMapping.UserId);
                    return View(ObjQuizPaperMapping);

                }
                ViewBag.RoleId = RoleID;
                objDB.SaveChanges();
                TestPaperMappingList();
                //ViewBag.QuizeId = objDB.Quizs.Where(x => x.IsActive == true).ToList();
                //ViewBag.UserRole = objDB.UserProfiles.Where(x => x.IsActive == true && x.RoleId == 3).ToList();
                return View(ObjQuizPaperMapping);
            }
            catch (Exception ex)
            {

                var RoleID = ApplicationSession.CurrentUser.RoleId;
                ViewBag.RoleId = RoleID;
                Common.LogError("Error", ex);
                ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                       "Danger" + "!</strong>" + ex.Message + ".</div>";
                TestPaperMappingList();
                return View(ObjQuizPaperMapping);
            }
        }

        public ActionResult Remove(Int32 QuizPaperMappingId = 0)
        {
            var quizpapermappingexit = objDB.QuizPaperMappings.Where(x => x.Id == QuizPaperMappingId).FirstOrDefault();
            objDB.QuizPaperMappings.Remove(quizpapermappingexit);
            objDB.SaveChanges();
            return RedirectToAction("Create");



        }

        
    }
}
