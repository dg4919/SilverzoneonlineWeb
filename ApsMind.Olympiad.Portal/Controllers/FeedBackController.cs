using ApsMind.Olympiad.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Portal.Models;

namespace ApsMind.Olympiad.Portal.Controllers
{
      [CustomAuthorize(new string[] { "1" })]
    public class FeedBackController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /FeedBack/
        //
        // GET: /Instruction/
        public ActionResult Index(String UserName, String EmailID, DateTime? StartDate, DateTime? EndDate, int ClassUser = 0)
        {
            try
            {
                ViewBag.ClassUser = new SelectList(objDB.Categorys.Where(x => x.IsActive == true && x.ParentCategoryId == null).ToList(), "Id", "CategoryName");
                ViewBag.UserName = UserName;
                ViewBag.EmailID = EmailID;
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
                var ObjFeddBackList = (from Feedbacklist in objDB.FeedBacks
                                       join classs in objDB.Categorys on Feedbacklist.Class equals classs.Id 
                                       join CreateBy in objDB.UserProfiles on Feedbacklist.CreatedBy equals CreateBy.Id
                                       orderby Feedbacklist.FeedBackId
                                       where (Feedbacklist.UserName.Contains(UserName)
                                                || UserName == null || UserName == "")
                                       && (Feedbacklist.EmailId.Contains(EmailID)
                                      || EmailID == null || EmailID == "")
                                       && (ClassUser == 0 || ClassUser == 0 || Feedbacklist.Class == ClassUser)
                                                 && (StartDate == null || EndDate == null || (Feedbacklist.CreatedOn >= StartDate && Feedbacklist.CreatedOn <= EndDate))
                                       select new
                                       {
                                           FeedBackId = Feedbacklist.FeedBackId,
                                           UserID = Feedbacklist.UserID,
                                           UserName = Feedbacklist.UserName,
                                           EmailId = Feedbacklist.EmailId,
                                           City = Feedbacklist.City,
                                           Class = classs.CategoryName,
                                           Subject = Feedbacklist.Subject,
                                           Message = Feedbacklist.Message
                                        ,
                                           CreatedBy = CreateBy.UserName,
                                           CreatedOn = Feedbacklist.CreatedOn
                                        ,


                                       }).ToList();

                var Count = ObjFeddBackList.Count();
                ViewBag.TotalFeedback = Count;
                return View(ObjFeddBackList);
            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View();
            }
        }



        public ActionResult Remove(Int32 QuestionId = 0, Int32 QuizId = 0, int ParentId = 0, int FeedbackId = 0)
        {


            var objFeedbackExit = objDB.FeedBacks.Where(x => x.FeedBackId == FeedbackId).ToList();
            objDB.FeedBacks.RemoveRange(objFeedbackExit);


            objDB.SaveChanges();
            TempData["Message"] = " Record has  been Delete successfully!";
            return RedirectToAction("Index");


        }
    }
}
