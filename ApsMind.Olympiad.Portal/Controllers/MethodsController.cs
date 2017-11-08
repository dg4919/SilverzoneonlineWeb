using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Portal;


namespace ApsMind.Olympiad.Portal.Controllers
{

    public class MethodsController : Controller
    {
        //
        // GET: /Methods/

        DatabaseContext objDB = new DatabaseContext();
        public ActionResult BindQuestionLevel()
        {

            List<Keyword> lst = objDB.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList(); ;
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BindFreeType()
        {

            List<Keyword> lst = objDB.Keywords.Where(x => x.KeywordType == "FreeType").ToList(); ;
            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BindSubject()
        {
           
            List<Category> lst = objDB.Categorys.Where(x => x.CategoryTypeId == 2).ToList(); ;
            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BindCreatedBy()
        {
           

            List<UserProfile> lst = (from cretedbylist in objDB.Questions
                                     join createdby in objDB.UserProfiles on cretedbylist.CreatedBy equals createdby.Id
                                     select createdby
                                    ).Distinct().ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public ActionResult RoleName()
        {
            // Select * from Category where CategoryTypeId=2
            List<UserRole> lst = objDB.UserRoles.Where(x => x.IsActive==true).ToList(); ;
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BindParaGraph()
        {
            // Select * from Category where CategoryTypeId=2
            List<Para> lst = objDB.Paras.Where(x => x.IsActive == true).ToList(); ;
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BindCountry()
        {
            
            List<Location> lst = objDB.Locations.Where(x => x.IsActive == true && x.LocationTypeId==1).ToList(); ;
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult State(Int64 Pid)
        {
            List<Location> lst = objDB.Locations.Where(x => x.ParentLocationId == Pid && x.IsActive == true).ToList();

            SelectList Objstate = new SelectList(lst, "LocationId", "LocationName");

            return Json(Objstate, JsonRequestBehavior.AllowGet);

        }
    }
}
