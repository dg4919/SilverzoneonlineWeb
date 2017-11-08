using ApsMind.Olympiad.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize]
    public class ErrorController : Controller
    {
        private DatabaseContext objDB = new DatabaseContext();

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult Global()
        {
            return View();
        }
        public ActionResult CustomError()
        {
            return View();
        }

    }
}
