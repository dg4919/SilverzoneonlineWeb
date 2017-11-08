using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using System.Data.Entity;
using ApsMind.Olympiad.Portal.Models;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize(new string[] { "1"})]
    public class CategoryController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        [HttpGet]
        public ActionResult Index(int? ParentId, String CategoryName, int CategoryTypeId = 0)
        {
            Category objCategory = objDB.Categorys.Where(x => x.Id == ParentId).FirstOrDefault();
            if (objCategory == null)
            {
                objCategory = new Category();
                objCategory.IsActive = true;
            }
            ViewBag.CategoryTypeName = objDB.Keywords.Where(x => x.KeywordType == "CategoryType").ToList();
            CategoryBind(ParentId, CategoryName, CategoryTypeId);
            ViewBag.ParentCategoryId = objCategory.ParentCategoryId;
            return View(objCategory);


        }

        public void CategoryBind(int? ParentId, String CategoryName, int CategoryTypeId = 0)
        {
            int PId = Convert.ToInt32(ParentId);
            ViewBag.ParentCategoryId = ParentId;
            ViewBag.ListCategory = (from cat in objDB.Categorys

                                    join CreateBy in objDB.UserProfiles on cat.CreatedBy equals CreateBy.Id
                                    join CategoryType in objDB.Keywords on cat.CategoryTypeId equals CategoryType.KeywordValue
                                    join subCategoryName in objDB.Categorys on cat.ParentCategoryId equals subCategoryName.Id into dt
                                    from d in dt.DefaultIfEmpty()

                                    //where cat.IsActive == true
                                    where CategoryType.KeywordType == "CategoryType"
                                    select new
                                    {
                                        CategoryId = cat.Id,
                                        CategoryName = cat.CategoryName,
                                        CategoryTypeName = CategoryType.KeywordText,
                                        SubCateGoryName = (d != null ? d.CategoryName : String.Empty),
                                        //SubCateGoryName=cat.ParentCategoryId,
                                        IsActive = cat.IsActive,
                                        CreatedBy = CreateBy.UserName,
                                        CreatedOn = cat.CreationDate
                                    }).ToList();

            var Result = ViewBag.ListCategory;

        }

        [HttpPost]
        public ActionResult Index(Category ObjCategory, String Command)
        {
            try
            {
                if (Command == "Save")
                {
                    Category objcategoryEdit = objDB.Categorys.Where(x => x.Id == ObjCategory.Id && x.CategoryTypeId == ObjCategory.CategoryTypeId).FirstOrDefault();
                    if (objcategoryEdit == null)
                    {
                        Category ObjCategoryExit = objDB.Categorys.Where(x => x.CategoryName == ObjCategory.CategoryName && x.CategoryTypeId == ObjCategory.CategoryTypeId && x.ParentCategoryId == (ObjCategory.ParentCategoryId == 0 ? null : (int?)ObjCategory.ParentCategoryId)).FirstOrDefault();
                        if (ObjCategoryExit == null)
                        {
                            {
                                ObjCategory.ParentCategoryId = ObjCategory.ParentCategoryId == 0 ? null : (int?)ObjCategory.ParentCategoryId;
                            }
                            ObjCategory.CreatedBy = ApplicationSession.CurrentUser.Id;
                            ObjCategory.CreationDate= System.DateTime.Now;
                            objDB.Categorys.Add(ObjCategory);
                            ViewBag.Message = "<div class=\"alert alert-success\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                                "Success" + "!</strong>" + " New record created successfully" + ".</div>";
                        }
                        else
                        {
                            ViewBag.CategoryTypeName = objDB.Keywords.Where(x => x.KeywordType == "CategoryType").ToList();
                            CategoryBind(ObjCategory.ParentCategoryId, ObjCategory.CategoryName, ObjCategory.CategoryTypeId);
                            ViewBag.Message = "<div class=\"alert alert-warning\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                              "Info" + "!</strong>" + "Record Alredy Exit" + ".</div>";
                            return View(ObjCategory);
                        }
                    }

                    else
                    {
                        Category ObjCategoryExit = objDB.Categorys.Where(x => x.CategoryName == ObjCategory.CategoryName && x.CategoryTypeId == ObjCategory.CategoryTypeId && x.ParentCategoryId == ObjCategory.Id).FirstOrDefault();
                        if (ObjCategoryExit == null)
                        {
                            ObjCategory.CreatedBy = objcategoryEdit.CreatedBy;
                            ObjCategory.CreationDate = objcategoryEdit.CreationDate;
                            ObjCategory.ParentCategoryId = ObjCategory.ParentCategoryId;
                            ObjCategory.UpdatedBy = ApplicationSession.CurrentUser.Id;
                            ObjCategory.UpdationDate = System.DateTime.Now;
                            objDB.Entry(objcategoryEdit).CurrentValues.SetValues(ObjCategory);
                            objDB.Entry(objcategoryEdit).State = EntityState.Modified;
                            ViewBag.Message = "<div class=\"alert alert-info\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                                "Success" + "!</strong>" + " Record has  been Updated successfully" + ".</div>";
                        }
                        else
                        {
                            ViewBag.CategoryTypeName = objDB.Keywords.Where(x => x.KeywordType == "CategoryType").ToList();
                            CategoryBind(ObjCategory.ParentCategoryId, ObjCategory.CategoryName, ObjCategory.CategoryTypeId);
                            ViewBag.Message = "<div class=\"alert alert-warning\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                              "Info" + "!</strong>" + "Record Alredy Exit" + ".</div>";
                            return View(ObjCategory);
                        }
                    }
                    objDB.SaveChanges();
                    ViewBag.CategoryTypeName = objDB.Keywords.Where(x => x.KeywordType == "CategoryType").ToList();
                    CategoryBind(ObjCategory.ParentCategoryId, ObjCategory.CategoryName, ObjCategory.CategoryTypeId);

                    //CategoryBind(0,null,0 );
                }

                else if (Command == "Search")
                {

                    ModelState.Clear();

                   
                    ViewBag.ListCategory = (from cat in objDB.Categorys
                                            join CreateBy in objDB.UserProfiles on cat.CreatedBy equals CreateBy.Id
                                            join CategoryType in objDB.Keywords on cat.CategoryTypeId equals CategoryType.KeywordValue
                                            join subCategoryName in objDB.Categorys on cat.ParentCategoryId equals subCategoryName.Id into dt
                                            from d in dt.DefaultIfEmpty()
                                            where (cat.CategoryName.Contains(ObjCategory.CategoryName)
                                                 || ObjCategory.CategoryName == null || ObjCategory.CategoryName == "")
                                            where (ObjCategory.ParentCategoryId == null || ObjCategory.ParentCategoryId == null || cat.Id == ObjCategory.ParentCategoryId)
                                            where (ObjCategory.CategoryTypeId == 0 || ObjCategory.CategoryTypeId == 0 || cat.CategoryTypeId == ObjCategory.CategoryTypeId)
                                                //where cat.IsActive == true
                                             && CategoryType.KeywordType == "CategoryType"
                                            select new
                                            {
                                                CategoryId = cat.Id,
                                                CategoryName = cat.CategoryName,
                                                CategoryTypeName = CategoryType.KeywordText,
                                                SubCateGoryName = (d != null ? d.CategoryName : String.Empty),
                                                IsActive = cat.IsActive,
                                                CreatedBy = CreateBy.UserName,
                                                CreatedOn = cat.CreationDate
                                            }).ToList();
                    var Result = ViewBag.ListCategory;

                    ViewBag.CategoryTypeName = objDB.Keywords.Where(x => x.KeywordType == "CategoryType").ToList();
                    
                }

            }
            catch (Exception ex)
            {
                
                Common.LogError("Error", ex);
                ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                       "Danger" + "!</strong>" + ex.Message + ".</div>";
                ViewBag.CategoryTypeName = objDB.Keywords.Where(x => x.KeywordType == "CategoryType").ToList();
                CategoryBind(ObjCategory.ParentCategoryId, ObjCategory.CategoryName, ObjCategory.CategoryTypeId);
            }

            return View(ObjCategory);
        }

    }
}
