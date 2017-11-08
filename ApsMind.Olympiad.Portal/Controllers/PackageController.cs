using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Portal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize(new string[] { "1", "3" })]
    [ValidateInput(false)]
    public class PackageController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Package/
        public ActionResult Index(String PackageName, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                ViewBag.PackageName = PackageName;
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
               
                if (ApplicationSession.CurrentUser.RoleId == 1)
                {
                    var objPackageList = (from PackageLIst in objDB.Package

                                          join CreateBy in objDB.UserProfiles on PackageLIst.CreatedBy equals CreateBy.Id
                                          orderby PackageLIst.PackageId
                                          where (PackageLIst.PackageName.Contains(PackageName)
                                           || PackageName == null || PackageName == "")
                                            && (StartDate == null || EndDate == null || (PackageLIst.CreatedOn >= StartDate && PackageLIst.CreatedOn <= EndDate))

                                select new
                                          {

                                              PackageId = PackageLIst.PackageId,
                                              PackageName = PackageLIst.PackageName,
                                              Description=PackageLIst.Description,
                                              Price=PackageLIst.Price,
                                              BundlePrice=PackageLIst.BundlePrice,
                                              IsActive = PackageLIst.IsActive,
                                              CreatedBy = CreateBy.UserName,
                                              CreatedOn = PackageLIst.CreatedOn
                                           ,

                                          }).ToList();
                    var Count = objPackageList.Count();
                    ViewBag.TotalPackage = Count;
                    return View(objPackageList);
                }
                else
                {
                    var objPackageList = (from PackageLIst in objDB.Package

                                          join CreateBy in objDB.UserProfiles on PackageLIst.CreatedBy equals CreateBy.Id
                                          orderby PackageLIst.PackageId
                                          where (PackageLIst.PackageName.Contains(PackageName)
                                            || PackageName == null || PackageName == "")
                                             && (StartDate == null || EndDate == null || (PackageLIst.CreatedOn >= StartDate && PackageLIst.CreatedOn <= EndDate))
                                              && (ApplicationSession.CurrentUser.Id == PackageLIst.CreatedBy)
                                          select new
                                          {

                                              PackageId = PackageLIst.PackageId,
                                              PackageName = PackageLIst.PackageName,
                                              Description = PackageLIst.Description,
                                              Price = PackageLIst.Price,
                                              BundlePrice = PackageLIst.BundlePrice,
                                              IsActive = PackageLIst.IsActive,
                                              CreatedBy = CreateBy.UserName,
                                              CreatedOn = PackageLIst.CreatedOn
                                           ,

                                          }).ToList();
                    var Count = objPackageList.Count();
                    ViewBag.TotalPackage = Count;
                    return View(objPackageList);
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

        //[HttpGet]
        //public JsonResult GetClasses(int subject_id)
        //{
        //   // var classlist = objDB.StudentClass.Where(x => x.SubjectId == subject_id);
        //    Console.Write(classlist);
        //    return Json(classlist, JsonRequestBehavior.AllowGet);

        //}


        //
        // GET: /Package/Create
        public ActionResult Create(int PackageId = 0)
        {
            Package ObjPackage = objDB.Package.Where(x => x.PackageId == PackageId).FirstOrDefault();
            TempData["TestPaper"] = new SelectList(objDB.Quizs.Where(x => x.IsActive == true), "QuizId", "QuizName");
           
            TempData["Subject"] = new SelectList(objDB.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
            TempData["PaperType"] = new SelectList(objDB.PaperType.Where(x => x.IsActive == true), "PaperTypeId", "PaperTypeName");
            return View(ObjPackage);
        }

        //public ActionResult Search_TestPaper(TestPaperSearch_ViewModel model)
        //{
          
        //}
        //
        // POST: /Package/Create
        [HttpPost]
        public ActionResult Create(Package ObjPackage)
        {
            try
            {
                if (ObjPackage.PackageName != null)
                {
                    Package existing =objDB.Package.Where(x => x.PackageId == ObjPackage.PackageId).FirstOrDefault();


                    if (existing == null)
                    {
                        ObjPackage.CreatedBy = ApplicationSession.CurrentUser.Id;
                        ObjPackage.CreatedOn = System.DateTime.Now;
                        objDB.Package.Add(ObjPackage);
                        objDB.SaveChanges();
                        TempData["Message"] = "New record created successfully!";
                        //ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                        //            "Success" + "!</strong>" + " New record created successfully" + ".</div>";

                    }
                    else
                    {
                        ObjPackage.CreatedBy = existing.CreatedBy;
                        ObjPackage.CreatedOn = existing.CreatedOn;
                        ObjPackage.ModifiedBy = ApplicationSession.CurrentUser.Id;
                        ObjPackage.ModifiedOn = System.DateTime.Now;
                        objDB.Entry(existing).CurrentValues.SetValues(ObjPackage);
                        objDB.Entry(existing).State = EntityState.Modified;
                        objDB.SaveChanges();
                        TempData["Message"] = " Record has  been Updated successfully!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                      "Error" + "!</strong>" + "Please Enter Package" + ".</div>";
                    return View(ObjPackage);
                }





            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View(ObjPackage);
            }
        }


    }
}