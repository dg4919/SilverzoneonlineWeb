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
    public class StudentClassController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Para/
        public ActionResult Index(String ClassName, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                ViewBag.ClassName = ClassName;
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
                if (ApplicationSession.CurrentUser.RoleId == 1)
                {
                    var objClassList = (from ClassLIst in objDB.StudentClass
                                        
                                        orderby ClassLIst.Id
                                        where (ClassLIst.ClassName.Contains(ClassName)
                                         || ClassName == null || ClassName == "")
                                          
                                        select new
                                        {

                                            StudentClassId = ClassLIst.Id,
                                            ClassName = ClassLIst.ClassName,
                                            IsActive = ClassLIst.IsActive,
                                           

                                        }).ToList();
                    var Count = objClassList.Count();
                    ViewBag.TotalClass = Count;
                    return View(objClassList);
                }
                else
                {
                    var objClassList = (from ClassLIst in objDB.StudentClass

                                        orderby ClassLIst.Id
                                        where (ClassLIst.ClassName.Contains(ClassName)
                                          || ClassName == null || ClassName == "")
                                           
                                        select new
                                        {

                                            StudentClassId =ClassLIst.Id,
                                           ClassName = ClassLIst.ClassName,
                                           IsActive = ClassLIst.IsActive,
                                          
                                        }).ToList();
                    var Count = objClassList.Count();
                    ViewBag.TotalClass = Count;
                    return View(objClassList);
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
        // GET: /StudentClass/Create
        public ActionResult Create(int StudentClassId = 0)
        {
            ViewBag.ChapterList = objDB.Chapter.Where(x => x.IsActive == true).ToList();
            StudentClass ObjStudentClass = objDB.StudentClass.Where(x => x.Id == StudentClassId).FirstOrDefault();
            TempData["Subject"] = new SelectList(objDB.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
            return View(ObjStudentClass);
        }

        //
        // POST: /StudentClass/Create
        [HttpPost]
        public ActionResult Create(StudentClass ObjStudentClass)
        {
            try
            {
                if (ObjStudentClass.ClassName != null)
                {
                    StudentClass existing = objDB.StudentClass.Where(x => x.Id == ObjStudentClass.Id).FirstOrDefault();

                    TempData["Subject"] = new SelectList(objDB.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName");
                    if (existing == null)
                    {
                       
                        objDB.StudentClass.Add(ObjStudentClass);
                        objDB.SaveChanges();
                        TempData["Message"] = "New record created successfully!";
                        //ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                        //            "Success" + "!</strong>" + " New record created successfully" + ".</div>";

                    }
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                      "Error" + "!</strong>" + "Please Enter StudentClass" + ".</div>";
                    return View(ObjStudentClass);
                }





            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View(ObjStudentClass);
            }
        }

        public ActionResult Edit(int StudentClassId = 0)
        {
            
            ViewBag.ChapterList = objDB.Chapter.Where(x => x.IsActive == true).ToList();
            StudentClass ObjStudentClass = objDB.StudentClass.Where(x => x.Id == StudentClassId).FirstOrDefault();
            //TempData["Subject"] = new SelectList(objDB.Subject.Where(x => x.IsActive == true), "SubjectId", "SubjectName",ObjStudentClass.SubjectId);
            return View(ObjStudentClass);
        }

        [HttpPost]
        public ActionResult Edit(StudentClass model, int StudentClassId)
        {
            try
            {
                if (StudentClassId != 0)
                {
                    var existing = objDB.StudentClass.Find(StudentClassId);
                    existing.ClassName = model.ClassName;
                    existing.IsActive = model.IsActive;
                    
                    objDB.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "No record found");
                }
               
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Error occured in exception",ex.Message);
            }
            return RedirectToAction("Index", "StudentClass");
        }
    }
}