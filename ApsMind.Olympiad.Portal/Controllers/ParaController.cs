using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize(new string[] { "1", "3" })]
    [ValidateInput(false)]
    public class ParaController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Para/
        public ActionResult Index(String ParaName, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                ViewBag.ParaName = ParaName;
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
                if (ApplicationSession.CurrentUser.RoleId == 1)
                {
                    var objParaList = (from ParaLIst in objDB.Paras

                                       join CreateBy in objDB.UserProfiles on ParaLIst.CreatedBy equals CreateBy.Id
                                       orderby ParaLIst.ParaId
                                       where (ParaLIst.ParaText.Contains(ParaName)
                                         || ParaName == null || ParaName == "")
                                          && (StartDate == null || EndDate == null || (ParaLIst.CreatedOn >= StartDate && ParaLIst.CreatedOn <= EndDate))
                                           
                                       select new
                                       {

                                           ParaId = ParaLIst.ParaId,
                                           ParaText = ParaLIst.ParaText,
                                           ImageUrl = ParaLIst.ImageUrl,
                                           IsActive = ParaLIst.IsActive,
                                           CreatedBy = CreateBy.UserName,
                                           CreatedOn = ParaLIst.CreatedOn
                                        ,

                                       }).ToList();
                    var Count = objParaList.Count();
                    ViewBag.TotalParaGraph = Count;
                    return View(objParaList);
                }
                else
                {
                    var objParaList = (from ParaLIst in objDB.Paras

                                       join CreateBy in objDB.UserProfiles on ParaLIst.CreatedBy equals CreateBy.Id
                                       orderby ParaLIst.ParaId
                                       where (ParaLIst.ParaText.Contains(ParaName)
                                         || ParaName == null || ParaName == "")
                                          && (StartDate == null || EndDate == null || (ParaLIst.CreatedOn >= StartDate && ParaLIst.CreatedOn <= EndDate))
                                           && (ApplicationSession.CurrentUser.Id == ParaLIst.CreatedBy)
                                       select new
                                       {

                                           ParaId = ParaLIst.ParaId,
                                           ParaText = ParaLIst.ParaText,
                                           ImageUrl = ParaLIst.ImageUrl,
                                           IsActive = ParaLIst.IsActive,
                                           CreatedBy = CreateBy.UserName,
                                           CreatedOn = ParaLIst.CreatedOn
                                        ,

                                       }).ToList();
                    var Count = objParaList.Count();
                    ViewBag.TotalParaGraph = Count;
                    return View(objParaList);
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
        // GET: /Para/Create
        public ActionResult Create(int ParaId = 0)
        {


            Para ObjPara = objDB.Paras.Where(x => x.ParaId == ParaId).FirstOrDefault();
            return View(ObjPara);
        }

        //
        // POST: /Para/Create
        [HttpPost]
        public ActionResult Create(Para ObjPara, HttpPostedFileBase ImageUrl)
        {
            try
            {
                if (ObjPara.ParaText != null || ImageUrl != null)
                {
                    Para existing = objDB.Paras.Where(x => x.ParaId == ObjPara.ParaId).FirstOrDefault();
                    string ParaImagePath = "";
                    string ImagePath = "/Images/ParaImage";
                    if (ImageUrl != null && ImageUrl.ContentLength > 0)
                    {
                        ParaImagePath = Path.GetFileName(ImageUrl.FileName);
                        String FileName = ImagePath + "/" + ParaImagePath+Common.Base64Encode(DateTime.Now.ToString())  + Path.GetExtension(ImageUrl.FileName);
                        ImageUrl.SaveAs(Server.MapPath("~" + FileName));
                        ObjPara.ImageUrl = FileName;
                    }

                    if (existing == null)
                    {
                        ObjPara.CreatedBy = ApplicationSession.CurrentUser.Id;
                        ObjPara.CreatedOn = System.DateTime.Now;
                        objDB.Paras.Add(ObjPara);
                        objDB.SaveChanges();
                        TempData["Message"] = "New record created successfully!";
                        //ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                        //            "Success" + "!</strong>" + " New record created successfully" + ".</div>";

                    }
                    else
                    {
                        ObjPara.CreatedBy = existing.CreatedBy;
                        ObjPara.CreatedOn = existing.CreatedOn;
                        ObjPara.ModifiedBy = ApplicationSession.CurrentUser.Id;
                        ObjPara.ModifiedOn = System.DateTime.Now;
                        objDB.Entry(existing).CurrentValues.SetValues(ObjPara);
                        objDB.Entry(existing).State = EntityState.Modified;
                        objDB.SaveChanges();
                        TempData["Message"] = " Record has  been Updated successfully!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                      "Error" + "!</strong>" + "Please Enter Para Graph OR Para Image" + ".</div>";
                    return View(ObjPara);
                }

                



            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View(ObjPara);
            }
        }

      
    }
}
