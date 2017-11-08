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
    public class SubjectController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Subject/
        public ActionResult Index(String SubjectName, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                ViewBag.SubjectName = SubjectName;
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
                if (ApplicationSession.CurrentUser.RoleId == 1)
                {
                    var objSubjectList = (from SubjectLIst in objDB.Subject
                                          orderby SubjectLIst.Id
                                          where (SubjectLIst.SubjectName.Contains(SubjectName)
                                           || SubjectName == null || SubjectName == "")
                                            
                                          select new
                                          {

                                              SubjectId = SubjectLIst.Id,
                                              SubjectName = SubjectLIst.SubjectName,
                                              Class=SubjectLIst.Class.ClassName,
                                              IsActive = SubjectLIst.IsActive,
                                              

                                          }).ToList();
                    var Count = objSubjectList.Count();
                    ViewBag.TotalSubject = Count;
                    return View(objSubjectList);
                }
                else
                {
                    var objSubjectList = (from SubjectLIst in objDB.Subject
                                  
                                          orderby SubjectLIst.Id
                                          where (SubjectLIst.SubjectName.Contains(SubjectName)
                                            || SubjectName == null || SubjectName == "")
                                            
                                          select new
                                          {

                                              SubjectId = SubjectLIst.Id,
                                              SubjectName = SubjectLIst.SubjectName,
                                              ClassId=SubjectLIst.Class.ClassName,
                                              IsActive = SubjectLIst.IsActive,
                                             

                                          }).ToList();
                    var Count = objSubjectList.Count();
                    ViewBag.TotalSubject = Count;
                    return View(objSubjectList);
                }


            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View();
            }
        }

        //
        // GET: /Subject/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Chapter/Create
        public ActionResult Create()
        {
            TempData["StudentClass"] = new SelectList(objDB.StudentClass.Where(x => x.IsActive == true), "Id", "ClassName");
            return View();

        }

        //
        // POST: /Subject/Create
        [HttpPost]
        public ActionResult Create(Subject model)
        {
            try
            {
                
                if (model.SubjectName != null)
                {
                    Subject _subject = new Subject();
                   _subject.SubjectName = model.SubjectName;
                   _subject.ClassId = model.ClassId;
                   _subject.IsActive = true;
                    objDB.Subject.Add(_subject);
                    objDB.SaveChanges();
                    return RedirectToAction("Index","Subject");
                }
                else
                {
                    ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                      "Error" + "!</strong>" + "Please Enter Subject" + ".</div>";
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View(model);
            }
        }

        public ActionResult Edit(int SubjectId)
        {
            Subject objSubject = objDB.Subject.Where(x => x.Id == SubjectId).FirstOrDefault();
            TempData["StudentClass"] = new SelectList(objDB.StudentClass.Where(x => x.IsActive == true), "Id", "ClassName", objSubject.ClassId);
         
            return View(objSubject);
        }

        [HttpPost]
        public ActionResult Edit(Subject model, int SubjectId)
        {
            Subject objSubject = objDB.Subject.Where(x => x.Id == SubjectId).FirstOrDefault();
            
            TempData["StudentClass"] = new SelectList(objDB.StudentClass.Where(x => x.IsActive == true), "Id", "ClassName", objSubject.ClassId);

            objSubject.SubjectName = model.SubjectName;
            objSubject.ClassId = model.ClassId;
            objSubject.IsActive = model.IsActive;
       
            objDB.SaveChanges();
            return RedirectToAction("Index", "Subject");
        }


   }
}