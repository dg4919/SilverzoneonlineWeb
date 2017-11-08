using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Portal.Models;

namespace ApsMind.Olympiad.Portal.Controllers
{
    public class NotificationController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();  //
        // GET: /Notification/
        public ActionResult Index()
        {
            try
            {

                var ObjNotificationList = (from not in objDB.Notifications
                                           join CreateBy in objDB.UserProfiles on not.CreatedBy equals CreateBy.Id
                                           join classs in objDB.Categorys on not.ClassId equals classs.Id
                                           orderby not.NotificationId

                                           select new
                                           {

                                               NotificationId = not.NotificationId,
                                               Class = classs.CategoryName,
                                               Message = not.Message,
                                               CreatedBy = CreateBy.UserName,
                                               CreatedOn = not.CreatedOn
                                            ,

                                           }).ToList();

                return View(ObjNotificationList);




            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View();
            }
        }

        //
        // GET: /Notification/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Notification/Create
        public ActionResult Create()
        {
            ViewBag.Class = objDB.Categorys.Where(x => x.IsActive == true && x.CategoryTypeId == 1).ToList();
            return View();
        }

        //
        // POST: /Notification/Create
        [HttpPost]
        public ActionResult Create(Notification ObjNotification)
        {
            try
            {
                ObjNotification.CreatedBy = ApplicationSession.CurrentUser.Id;
                ObjNotification.CreatedOn = System.DateTime.Now;

                objDB.Notifications.Add(ObjNotification);
                objDB.SaveChanges();

                var ExistClassUser = objDB.UserProfiles.Where(x => x.ClassId == ObjNotification.ClassId).ToList();

                AdminNotification objnotification = new AdminNotification();
                if (ExistClassUser != null)
                {
                    foreach (var item in ExistClassUser)
                    {

                        if (item.MobileNumber != null && item.EmailId != null && item.DeviceTypeId == 1)
                        {
                            objnotification.NotificationByPostIPhone(ObjNotification.Message, item.MobileNumber, item.GCMID);
                        }
                        if (item.MobileNumber != null && item.EmailId != null && item.DeviceTypeId == 2)
                        {
                            objnotification.NotificationByPostAndriod(ObjNotification.Message, item.MobileNumber, item.GCMID);
                        }
                    }
                }
                TempData["Message"] = "Notification Sent successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Notification/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Notification/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Notification/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Notification/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
