using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using System.IO;
namespace ApsMind.Olympiad.Portal.Controllers
{
    
    public class HomeController : Controller
    {

        DatabaseContext objDB = new DatabaseContext();
        [CustomAuthorize(new string[] { "1","2" })]
        public ActionResult Index()
        {


            //compressimage();
            ViewBag.Title = "Home Page";


           

            return View();
        }

        //[CustomAuthorize]
        //public JsonResult UserProfileReport()
        //{

        //    var Result = (from Udetails in objDB.UserProfiles
        //                  join RoleName in objDB.UserRoles on Udetails.RoleId equals RoleName.RoleId
        //                  join CountryName in objDB.Locations on Udetails.Country equals CountryName.LocationId
        //                  join statename in objDB.Locations on Udetails.State equals statename.LocationId

        //                  join classs in objDB.Categorys on Udetails.Class equals classs.CategoryId into dt
        //                  from d in dt.DefaultIfEmpty()
        //                  select new UserProfileView
        //                  {
        //                      UserId = Udetails.UserId,
        //                      Name = Udetails.Name
        //                    ,

        //                      UserName = Udetails.UserName,
        //                      Password = Udetails.Password,
        //                      Role = RoleName.RoleName,
        //                      EmailId = Udetails.EmailId,
        //                      IsActive = Udetails.IsActive,
        //                      CreatedOn = Udetails.CreatedOn,
        //                      Country = CountryName.LocationName,
        //                      State = statename.LocationName,
        //                      City = Udetails.City,
        //                      Class = (d != null ? d.CategoryName : String.Empty),

        //                  }).ToList();

        //    var ActiveUser = objDB.UserProfiles.Where(x => x.IsActive == true).ToList();
        //    var InActiveuser = objDB.UserProfiles.Where(x => x.IsActive == false).ToList();
        //    var TotalUser = objDB.UserProfiles.ToList();
        //    var result = new
        //    {
        //        ActiveUser = ActiveUser.Count(),
        //        InActiveUser = InActiveuser.Count(),
        //        User = "TotalUser"

        //    };


        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult paysuccess()
        {

            return View();
        }
        public ActionResult payfailed()
        {

            return View();
        }

        //public ActionResult compressimage()
        //{
        //    Handler.ImageHandler objImageHandler = new Handler.ImageHandler();

        //    DirectoryInfo objDi = new DirectoryInfo(Server.MapPath("/Images/QuestionImage"+"/"));
          
        //    foreach (var item in objDi.GetFiles())
        //    {

        //        objImageHandler.CompressFile(item.FullName);
                
        //    }


        //    DirectoryInfo objOptionImage = new DirectoryInfo(Server.MapPath("/Images/QuestionOptionImage" + "/"));

        //    foreach (var item in objOptionImage.GetFiles())
        //    {

        //        objImageHandler.CompressFile(item.FullName);

        //    }
        //    DirectoryInfo objInstructionImage = new DirectoryInfo(Server.MapPath("/Images/InstructionImage" + "/"));

        //    foreach (var item in objInstructionImage.GetFiles())
        //    {

        //        objImageHandler.CompressFile(item.FullName);

        //    }

        //    DirectoryInfo objParaImage = new DirectoryInfo(Server.MapPath("/Images/ParaImage" + "/"));

        //    foreach (var item in objParaImage.GetFiles())
        //    {

        //        objImageHandler.CompressFile(item.FullName);

        //    }

        //    //DirectoryInfo objParaImage = new DirectoryInfo(Server.MapPath("/Images/ParaImage" + "/"));

        //    //foreach (var item in objParaImage.GetFiles())
        //    //{

        //    //    objImageHandler.CompressFile(item.FullName);

        //    //}




        //    //DirectoryInfo objDI = new DirectoryInfo("");
        //    //foreach (var item in objDI.GetFiles())
        //    //{

        //    //    objImageHandler.CompressFile(item.FullName);

        //    //}


        //    return null;

        //}

    }
}
