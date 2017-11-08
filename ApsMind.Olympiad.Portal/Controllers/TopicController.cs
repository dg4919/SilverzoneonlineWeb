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
    public class TopicController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Para/
        public ActionResult Index(String TopicName, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                ViewBag.TopicName = TopicName;
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
                if (ApplicationSession.CurrentUser.RoleId == 1)
                {
                    var objTopicList = (from TopicLIst in objDB.Topic

                                       join CreateBy in objDB.UserProfiles on TopicLIst.CreatedBy equals CreateBy.Id
                                       orderby TopicLIst.TopicId
                                        where (TopicLIst.TopicName.Contains(TopicName)
                                         || TopicName == null || TopicName == "")
                                          && (StartDate == null || EndDate == null || (TopicLIst.CreatedOn >= StartDate && TopicLIst.CreatedOn <= EndDate))

                                       select new
                                       {

                                           TopicId = TopicLIst.TopicId,
                                           TopicName = TopicLIst.TopicName,
                                           ChapterId=TopicLIst.ChapterId,
                                           IsActive = TopicLIst.IsActive,
                                           CreatedBy = CreateBy.UserName,
                                           CreatedOn = TopicLIst.CreatedOn
                                        ,

                                       }).ToList();
                    var Count = objTopicList.Count();
                    ViewBag.TotalTopic = Count;
                    return View(objTopicList);
                }
                else
                {
                    var objTopicList = (from TopicLIst in objDB.Topic

                                       join CreateBy in objDB.UserProfiles on TopicLIst.CreatedBy equals CreateBy.Id
                                       orderby TopicLIst.TopicId
                                       where (TopicLIst.TopicName.Contains(TopicName)
                                         || TopicName == null || TopicName == "")
                                          && (StartDate == null || EndDate == null || (TopicLIst.CreatedOn >= StartDate && TopicLIst.CreatedOn <= EndDate))
                                           && (ApplicationSession.CurrentUser.Id == TopicLIst.CreatedBy)
                                       select new
                                       {

                                           TopicId = TopicLIst.TopicId,
                                           TopicName = TopicLIst.TopicName,
                                           ChapterId=TopicLIst.ChapterId,
                                           IsActive = TopicLIst.IsActive,
                                           CreatedBy = CreateBy.UserName,
                                           CreatedOn = TopicLIst.CreatedOn
                                        ,

                                       }).ToList();
                    var Count = objTopicList.Count();
                    ViewBag.TotalTopic = Count;
                    return View(objTopicList);
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

        //
        // GET: /Topic/Create
        public ActionResult Create(int TopicId = 0)
        {
            ViewBag.ChapterList = objDB.Chapter.Where(x => x.IsActive == true).ToList();
            Topic ObjTopic = objDB.Topic.Where(x => x.TopicId == TopicId).FirstOrDefault();
            return View(ObjTopic);
        }

        //
        // POST: /Topic/Create
        [HttpPost]
        public ActionResult Create(Topic ObjTopic)
        {
            try
            {
                if (ObjTopic.TopicName!= null )
                {
                    Topic existing = objDB.Topic.Where(x =>x.TopicId  == ObjTopic.TopicId).FirstOrDefault();
                    
                    
                    if (existing == null)
                    {
                        ObjTopic.CreatedBy = ApplicationSession.CurrentUser.Id;
                        ObjTopic.CreatedOn = System.DateTime.Now;
                        objDB.Topic.Add(ObjTopic);
                        objDB.SaveChanges();
                        TempData["Message"] = "New record created successfully!";
                        //ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                        //            "Success" + "!</strong>" + " New record created successfully" + ".</div>";

                    }
                    else
                    {
                        ObjTopic.CreatedBy = existing.CreatedBy;
                        ObjTopic.CreatedOn = existing.CreatedOn;
                        ObjTopic.ModifiedBy = ApplicationSession.CurrentUser.Id;
                        ObjTopic.ModifiedOn = System.DateTime.Now;
                        objDB.Entry(existing).CurrentValues.SetValues(ObjTopic);
                        objDB.Entry(existing).State = EntityState.Modified;
                        objDB.SaveChanges();
                        TempData["Message"] = " Record has  been Updated successfully!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                      "Error" + "!</strong>" + "Please Enter Topic" + ".</div>";
                    return View(ObjTopic);
                }





            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View(ObjTopic);
            }
        }


    }
}