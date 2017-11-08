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
    [CustomAuthorize]
    public class LocationController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Location/
        [HttpGet]
        public ActionResult Index(int LocationId = 0, int LocationTypeId = 0)
        {
            ViewBag.LocationTypeNmae = new SelectList(objDB.LocationTypes.ToList(), "LocationTypeId", "LocationTypeName");


            TempData["SearchConditions"] = Common.searchConditions;
            return View(GetLocationList(LocationId, LocationTypeId));
        }

        [HttpPost]
        public ActionResult Index(int LocationIdS = 0, String LocationName = null, int LocationTypeId = 0)
        {
            ViewBag.LocationTypeNmae = new SelectList(objDB.LocationTypes.ToList(), "LocationTypeId", "LocationTypeName");

            ViewBag.LocationId = LocationName;

            Common.SetSearchConditionValue(Common.searchConditions, "LocationId", LocationIdS);
            Common.SetSearchConditionValue(Common.searchConditions, "LocationTypeId", LocationTypeId);
            TempData["SearchConditions"] = Common.searchConditions;

            return View(GetLocationList(LocationIdS, LocationTypeId));
        }
        //

        // GET: /Location/Create
        public ActionResult Create(int LocationId = 0)
        {
            var ObjLocation = objDB.Locations.Where(x => x.Id == LocationId).FirstOrDefault();
            if (ObjLocation == null)
            {
                ObjLocation = new Location();
                ObjLocation.IsActive = true;
            }
            ViewBag.CategoryTypeName = objDB.LocationTypes.ToList();
            var List = ViewBag.CategoryTypeName;
            ViewBag.ParentLocationId = ObjLocation.ParentLocationId;
            return View(ObjLocation);
        }

        //
        // POST: /Location/Create
        [HttpPost]
        public ActionResult Create(Location ObjLocation, int ParentLocationId = 0)
        {
            try
            {

                Location ExistLocation = objDB.Locations.Where(x => x.Id == ObjLocation.Id).FirstOrDefault();

                if (ExistLocation == null)
                {
                    var ObjLocationExit = objDB.Locations.Where(x => x.LocationName == ObjLocation.LocationName && x.LocationTypeId == ObjLocation.LocationTypeId && x.ParentLocationId == ParentLocationId).FirstOrDefault();
                    if (ObjLocationExit == null)
                    {
                        ObjLocation.UpdatedBy = ApplicationSession.CurrentUser.Id;
                        ObjLocation.UpdationDate = System.DateTime.Now;
                        ObjLocation.ParentLocationId = ParentLocationId;
                        objDB.Locations.Add(ObjLocation);
                        objDB.SaveChanges();
                        TempData["Message"] = "Record Creted successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.CategoryTypeName = objDB.LocationTypes.ToList();
                        ViewBag.ParentCategoryId = ParentLocationId;
                        ViewBag.Message = "<div class=\"alert alert-warning\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                              "Info" + "!</strong>" + "Record Alredy Exit" + ".</div>";
                        return View(ObjLocation);
                    }
                }
                else
                {
                    ObjLocation.UpdatedBy = ExistLocation.UpdatedBy;
                    ObjLocation.UpdationDate = System.DateTime.Now;
                    ObjLocation.ParentLocationId = ParentLocationId;
                    objDB.Entry(ExistLocation).CurrentValues.SetValues(ObjLocation);
                    objDB.Entry(ExistLocation).State = EntityState.Modified;
                    objDB.SaveChanges();
                    TempData["Message"] = "Record Update successfully!";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                ViewBag.CategoryTypeName = objDB.LocationTypes.ToList();
                ViewBag.ParentCategoryId = ParentLocationId;
                return View(ObjLocation);
            }
        }

        #region "User Method"
        private List<LocationListView> GetLocationList(int LocationIdS = 0, int LocationTypeId = 0)
        {


            ViewBag.LocationTypeNmae = new SelectList(objDB.LocationTypes.ToList(), "LocationTypeId", "LocationTypeName");



            var ObjLocationList = (from Llist in objDB.Locations
                                   join LType in objDB.LocationTypes on Llist.LocationTypeId equals LType.LocationTypeId

                                   join SubLocation in objDB.Locations on Llist.ParentLocationId equals SubLocation.Id into dt
                                   from d in dt.DefaultIfEmpty()
                                   orderby Llist.LocationTypeId
                                   where (LocationTypeId == 0 || LocationTypeId == 0 || Llist.LocationTypeId == LocationTypeId)
                                   where (LocationIdS == 0 || LocationIdS == 0 || Llist.Id == LocationIdS)


                                   select new LocationListView
                                   {
                                       LocationId = Llist.Id,
                                       LocationName = Llist.LocationName
                                    ,
                                       LocationTypeId = LType.LocationTypeName,
                                       IsActive = Llist.IsActive,
                                       ParentLocationId = (d != null ? d.LocationName : String.Empty),
                                       UpdatedOn = Llist.UpdationDate


                                   }).ToList();

            var TotalLocation = ObjLocationList.Count();
            ViewBag.TotalLocation = TotalLocation;
            return ObjLocationList;
        }
        #endregion
    }
}
