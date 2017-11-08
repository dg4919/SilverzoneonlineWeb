using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using System.IO;
using System.Data.Entity;
using ApsMind.Olympiad.Portal.Models;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize]
    [ValidateInput(false)]
    public class QuestionOptionController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /QuestionOption/
        public ActionResult Index()
        {
            var objQuesOptionList = (from QuesOptionList in objDB.QuestionOptions
                                     join QuestionName in objDB.Questions on QuesOptionList.QuestionId equals QuestionName.QuestionId
                                     select new
                                    {
                                        OptionId = QuesOptionList.OptionId,
                                        QuestionId = QuesOptionList.QuestionId,
                                        QuestionName = QuestionName.QuestionText,
                                        OptionText = QuesOptionList.OptionText,
                                        IsAnswer = QuesOptionList.IsAnswer,
                                        ImageUrl = QuesOptionList.ImageUrl,




                                    }).ToList();


            return View(objQuesOptionList);
        }

        //
        // GET: /QuestionOption/Create
       
        //
        // GET: /QuestionOption/Create
        public ActionResult Create(int OptionId = 0)
        {
            ViewBag.QuestionId = objDB.Questions.Where(x => x.IsActive == true).ToList();
            QuestionOption ObjQuestionOption = objDB.QuestionOptions.Where(x => x.OptionId == OptionId).FirstOrDefault();
            return View(ObjQuestionOption);
        }

        //
        // POST: /QuestionOption/Create
        [HttpPost]
        public ActionResult Create(QuestionOption ObjQuestionOption, HttpPostedFileBase ImageUrl)
        {
            try
            {
                QuestionOption existing = objDB.QuestionOptions.Where(x => x.OptionId == ObjQuestionOption.OptionId).FirstOrDefault();
                string QuesOptionImagePath = "";

                string ImagePath = "/Images/QuestionOptionImage";
                if (ImageUrl != null && ImageUrl.ContentLength > 0)
                {
                    QuesOptionImagePath = Path.GetFileName(ImageUrl.FileName);
                    String FileName = ImagePath + "/" + QuesOptionImagePath + Common.Base64Encode(DateTime.Now.ToString()) +  Path.GetExtension(ImageUrl.FileName);
                    ImageUrl.SaveAs(Server.MapPath("~" + FileName));
                    ObjQuestionOption.ImageUrl = FileName;
                }
                else
                {
                    ObjQuestionOption.ImageUrl = existing.ImageUrl;
                }

                if (existing == null)
                {
                    objDB.QuestionOptions.Add(ObjQuestionOption);
                    objDB.SaveChanges();
                    ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                                "Success" + "!</strong>" + " New record created successfully" + ".</div>";
                }
                else
                {

                    objDB.Entry(existing).CurrentValues.SetValues(ObjQuestionOption);
                    objDB.Entry(existing).State = EntityState.Modified;
                    objDB.SaveChanges();
                    ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                        "Success" + "!</strong>" + " Record has  been Updated successfully" + ".</div>";
                }

                return RedirectToAction("Index");

            }
            catch
            {
                ViewBag.QuestionId = objDB.Questions.Where(x => x.IsActive == true).ToList();
                return View(ObjQuestionOption);
            }
        }

       

       
    }
}
