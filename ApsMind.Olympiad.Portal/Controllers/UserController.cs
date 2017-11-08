using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApsMind.Olympiad.Portal.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
           

            return View();
        }

        public ActionResult UserProfile()
        {
            ViewBag.VisitDateTime = DateTime.Now;
            return View();
        }
        public ActionResult TestZone()
        {
            return View();
        }

        public ActionResult availabletest()
        {
            return View();
        }
        public ActionResult testdetail()
        {
            return View();
        }

        public ActionResult testpaper()
        {

            return View();
        }
        //[HttpPost]
        //public ActionResult testpaper(HttpPostedFileBase file)
        //{

        //    if (file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);
        //        var path = Path.Combine(Server.MapPath("~/Notes/"), fileName);
        //        file.SaveAs(path);
        //    }

        //    return RedirectToAction("testpaper");
        //}
        public ActionResult customtest()
        {
            return View();
        }
        public ActionResult createtest()
        {
            return View();
        }
        public ActionResult infozone()
        {
            return View();
        }
        public ActionResult reports()
        {
            return View();
        }

        public ActionResult reporthistory()
        {
            return View();
        }
        public ActionResult testresult()
        {
            return View();
        }

        public   ActionResult mynotes()
        {
            return View();
        }

        public ActionResult CustomQuestionPaper()
        {
            return View();
        }
        public ActionResult UserAccountReport()
        {
            return View();
        }
       




	}
}