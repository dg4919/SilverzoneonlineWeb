using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using System.IO;
using ApsMind.Olympiad.Portal.Models;
using System.Data.Entity;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize(new string[] { "1", "3" })]
    [ValidateInput(false)]
    public class InstructionController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Instruction/
        public ActionResult Index(String InstructionName, DateTime? StartDate, DateTime? EndDate, int QuizId = 0)
        {
            try
            {
                ViewBag.QuizeId = objDB.Quizs.Where(x => x.IsActive == true).ToList();
                ViewBag.InstructionName = InstructionName;
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
                var objInstructionList = (from InstructionsList in objDB.Instructions
                                          join QuizeName in objDB.Quizs on InstructionsList.QuizId equals QuizeName.QuizId
                                          join CreateBy in objDB.UserProfiles on InstructionsList.CreatedBy equals CreateBy.Id
                                          orderby InstructionsList.InstructionId
                                          where (InstructionsList.InstructionText.Contains(InstructionName)
                                            || InstructionName == null || InstructionName == "")
                                            && (QuizId == 0 || QuizId == 0 || InstructionsList.QuizId == QuizId)
                                             && (StartDate == null || EndDate == null || (InstructionsList.CreatedOn >= StartDate && InstructionsList.CreatedOn <= EndDate))
                                          select new
                                          {

                                              InstructionId = InstructionsList.InstructionId,
                                              InstructionText = InstructionsList.InstructionText,
                                              QuizeId = QuizeName.QuizName,
                                              ImageUrl = InstructionsList.ImageUrl,
                                              IsActive = InstructionsList.IsActive,
                                              CreatedBy = CreateBy.UserName,
                                              CreatedOn = InstructionsList.CreatedOn
                                           ,


                                          }).ToList();

                var Count = objInstructionList.Count();
                ViewBag.TotalInstruction = Count;
                return View(objInstructionList);
            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                return View();
            }
        }


        //
        // GET: /Instruction/Create
        public ActionResult Create(int InstructionId = 0)
        {
            var RoleID = ApplicationSession.CurrentUser.RoleId;
            if (RoleID == 1)
            {
                ViewBag.QuizeId = (from QuizList in objDB.Quizs

                                   select new
                                   {
                                       QuizId = QuizList.QuizId,
                                       QuizeName = QuizList.QuizName,




                                   }).ToList();
            }
            else
            {
                ViewBag.QuizeId = (from QuizList in objDB.Quizs
                                   join QuizPaperMappingId in objDB.QuizPaperMappings on QuizList.QuizId equals QuizPaperMappingId.QuizId
                                   where ApplicationSession.CurrentUser.Id == QuizPaperMappingId.UserId
                                   select new
                                   {
                                       QuizId = QuizList.QuizId,
                                       QuizeName = QuizList.QuizName,




                                   }).ToList();
            }


            //ViewBag.QuizeId = objDB.Quizs.Where(x => x.IsActive == true).ToList();

            Instruction ObjInstruction = objDB.Instructions.Where(x => x.InstructionId == InstructionId).FirstOrDefault();
            return View(ObjInstruction);
        }

        public void postinstruction()
        {
            var RoleID = ApplicationSession.CurrentUser.RoleId;
            if (RoleID == 1)
            {
                ViewBag.QuizeId = (from QuizList in objDB.Quizs

                                   select new
                                   {
                                       QuizId = QuizList.QuizId,
                                       QuizeName = QuizList.QuizName,




                                   }).ToList();
            }
            else
            {
                ViewBag.QuizeId = (from QuizList in objDB.Quizs
                                   join QuizPaperMappingId in objDB.QuizPaperMappings on QuizList.QuizId equals QuizPaperMappingId.QuizId
                                   where ApplicationSession.CurrentUser.Id == QuizPaperMappingId.UserId
                                   select new
                                   {
                                       QuizId = QuizList.QuizId,
                                       QuizeName = QuizList.QuizName,




                                   }).ToList();
            }


        }
        //
        // POST: /Instruction/Create
        [HttpPost]
        public ActionResult Create(Instruction ObjInstruction, HttpPostedFileBase ImageUrl)
        {
            try
            {

                if (ObjInstruction.InstructionText != null || ImageUrl != null && ObjInstruction.QuizId != 0)
                {

                    Instruction existing = objDB.Instructions.Where(x => x.InstructionId == ObjInstruction.InstructionId).FirstOrDefault();
                    string InstructionImagePath = "";
                    string ImagePath = "/Images/InstructionImage";
                    if (ImageUrl != null && ImageUrl.ContentLength > 0)
                    {
                        InstructionImagePath = Path.GetFileName(ImageUrl.FileName);
                        String FileName = ImagePath + "/" + InstructionImagePath + Common.Base64Encode(DateTime.Now.ToString()) + Path.GetExtension(ImageUrl.FileName);
                        ImageUrl.SaveAs(Server.MapPath("~" + FileName));
                        ObjInstruction.ImageUrl = FileName;
                    }

                    if (existing == null)
                    {
                        ObjInstruction.CreatedBy = ApplicationSession.CurrentUser.Id;
                        ObjInstruction.CreatedOn = System.DateTime.Now;
                        objDB.Instructions.Add(ObjInstruction);
                        objDB.SaveChanges();
                        TempData["Message"] = "New record created successfully!";
                    }
                    else
                    {
                        ObjInstruction.CreatedBy = existing.CreatedBy;
                        ObjInstruction.CreatedOn = existing.CreatedOn;
                        ObjInstruction.ModifiedBy = ApplicationSession.CurrentUser.Id;
                        ObjInstruction.ModifiedOn = System.DateTime.Now;
                        objDB.Entry(existing).CurrentValues.SetValues(ObjInstruction);
                        objDB.Entry(existing).State = EntityState.Modified;
                        objDB.SaveChanges();
                        TempData["Message"] = " Record has  been Updated successfully!";
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                     "Error" + "!</strong>" + "PLease Enter Instruction Text Or Instruction Image!" + ".</div>";
                    postinstruction();
                    return View(ObjInstruction);
                }
            }



            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                postinstruction();

                return View(ObjInstruction);
            }
        }

    }
}
