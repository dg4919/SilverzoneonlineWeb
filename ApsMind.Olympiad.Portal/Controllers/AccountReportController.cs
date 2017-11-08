using ApsMind.Olympiad.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using System.Data.Entity;
using ApsMind.Olympiad.Portal.Models;
using System.Web.Helpers;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize(new string[] { "1" })]
    public class AccountReportController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        List<UserProfileView> objUserList = null;
        [HttpGet]
        public ActionResult Index(string UserName, DateTime? StartDate = null, DateTime? EndDate = null, String TEST = null, int deviceType = 2, int ParentCategoryId = 0, String PaperName = null, String EmailId = null)
        {
            ViewBag.UserName = UserName;
            ViewBag.ParentCategoryId = 0;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "PaperName", PaperName);

            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);
            Common.SetSearchConditionValue(Common.searchConditions, "ParentCategoryId", ParentCategoryId);
            Common.SetSearchConditionValue(Common.searchConditions, "DeviceType", deviceType);
            TempData["SearchConditions"] = Common.searchConditions;
            return View(GetAccountList(UserName, StartDate, EndDate, deviceType, 0, ParentCategoryId, PaperName, EmailId));
        }

        [HttpPost]
        public ActionResult Index(string UserName, DateTime? StartDate = null, DateTime? EndDate = null, int deviceType = 2, int Price = 0, int ParentCategoryId = 0, String PaperName = null, String EmailId = null)
        {
            ViewBag.UserName = UserName;
            ViewBag.ParentCategoryId = ParentCategoryId;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.Price = Price;
            ViewBag.PaperName = PaperName;
            ViewBag.EmailId = EmailId;
            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);


            Common.SetSearchConditionValue(Common.searchConditions, "ParentCategoryId", ParentCategoryId);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "Price", Price);
            Common.SetSearchConditionValue(Common.searchConditions, "PaperName", PaperName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);

            Common.SetSearchConditionValue(Common.searchConditions, "DeviceType", deviceType);

            TempData["SearchConditions"] = Common.searchConditions;

            return View(GetAccountList(UserName, StartDate, EndDate, deviceType, Price, ParentCategoryId, PaperName, EmailId));
        }

        [HttpGet]
        public ActionResult IPhoneUser(string UserName, DateTime? StartDate = null, DateTime? EndDate = null, String TEST = null, int deviceType = 1, int ParentCategoryId = 0, String PaperName = null, String EmailId = null)
        {
            ViewBag.UserName = UserName;
            ViewBag.ParentCategoryId = 0;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.PaperName = PaperName;
            ViewBag.EmailId = EmailId;
            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "ParentCategoryId", ParentCategoryId);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "PaperName", PaperName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);
            Common.SetSearchConditionValue(Common.searchConditions, "DeviceType", deviceType);
            TempData["SearchConditions"] = Common.searchConditions;
            return View(GetAccountList(UserName, StartDate, EndDate, deviceType, 0, ParentCategoryId, PaperName, EmailId));
        }

        [HttpPost]
        public ActionResult IPhoneUser(string UserName, DateTime? StartDate = null, DateTime? EndDate = null, int deviceType = 1, int Price = 0, int ParentCategoryId = 0, String PaperName = null, String EmailId = null)
        {
            ViewBag.UserName = UserName;

            ViewBag.ParentCategoryId = ParentCategoryId;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.Price = Price;
            ViewBag.PaperName = PaperName;
            ViewBag.EmailId = EmailId;
            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "ParentCategoryId", ParentCategoryId);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "Price", Price);
            Common.SetSearchConditionValue(Common.searchConditions, "PaperName", PaperName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);
            Common.SetSearchConditionValue(Common.searchConditions, "DeviceType", deviceType);
            TempData["SearchConditions"] = Common.searchConditions;

            return View(GetAccountList(UserName, StartDate, EndDate, deviceType, Price, ParentCategoryId, PaperName, EmailId));
        }

        public ActionResult ExportToExcel()
        {

            DataTable Client = new DataTable("teste");
            DataRow drow = null;

            Client.Columns.Add("User Name", typeof(string));
            Client.Columns.Add("Email Id", typeof(string));
            Client.Columns.Add("Class", typeof(string));
            Client.Columns.Add("Subject", typeof(string));
            Client.Columns.Add("Test Paper Name", typeof(string));
            Client.Columns.Add("Price", typeof(string));
            Client.Columns.Add("Tranjection Date", typeof(string));

            string UserName = (String)Common.GetSearchConditionValue(Common.searchConditions, "UserName");
            string EmailId = (String)Common.GetSearchConditionValue(Common.searchConditions, "EmailId");

            string PaperName = (String)Common.GetSearchConditionValue(Common.searchConditions, "PaperName");

            object tempData1 = Common.GetSearchConditionValue(Common.searchConditions, "StartDate");
            DateTime? StartDate = (tempData1 == null ? new Nullable<DateTime>() : (DateTime?)tempData1);

            object tempData2 = Common.GetSearchConditionValue(Common.searchConditions, "EndDate");
            DateTime? EndDate = (tempData2 == null ? new Nullable<DateTime>() : (DateTime?)tempData2);
            int deviceType = (int)Common.GetSearchConditionValue(Common.searchConditions, "DeviceType");

            int ParentCategoryId = (int)Common.GetSearchConditionValue(Common.searchConditions, "ParentCategoryId");
            List<UserAccountView> objList = GetAccountList(UserName, StartDate, EndDate, deviceType, 0, ParentCategoryId, PaperName, EmailId);
            var Result = objUserList;
            //List<UserProfileView> objList = objUserList.ToList();
            if (objList.Count == 0)
            {
                return null;
            }


            foreach (var list in objList)
            {
                drow = Client.NewRow();
                drow["User Name"] = list.UserName;
                drow["Email Id"] = list.EmailId;
                drow["Class"] = list.ClassName;
                drow["Subject"] = list.SubjectName;
                drow["Test Paper Name"] = list.QuizName;
                drow["Price"] = list.Price;
                drow["Tranjection Date"] = list.TransDate;


                Client.Rows.Add(drow);
            }
            Client.AcceptChanges();

            var grid = new GridView();
            grid.DataSource = Client;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return null;
        }

        [HttpGet]
        public ActionResult WebPayments(string UserName, DateTime? StartDate = null, DateTime? EndDate = null, int deviceType = 3, int ParentCategoryId = 0, String PaperName = null, String EmailId = null)
        {
            ViewBag.UserName = UserName;
            ViewBag.ParentCategoryId = 0;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.PaperName = PaperName;
            ViewBag.EmailId = EmailId;
            
            List<_PaymentHistory> result = (from ph in objDB.QuizPurchaseOrders 
                                            join  users in objDB.UserProfiles on ph.ByUserId equals  users.Id
                                            join c in objDB.Carts on new { a = ph.OrderSqNo, b = ph.ByUserId } equals new { a = c.OrderSqNo, b = c.UserId }
                                            join quiz in objDB.Quizs on c.QuizId equals quiz.QuizId
                                            join SubjectName in objDB.Categorys on quiz.SubjectId equals SubjectName.Id
                                            join ClassName in objDB.Categorys on SubjectName.ParentCategoryId equals ClassName.Id
                                      where users.Id==ph.ByUserId

                         
                              select new _PaymentHistory
                          {
                              OrderId=ph.OrderId,
                              OrderSqId=ph.OrderId,
                              QuizId=c.QuizId,
                              UserId=ph.ByUserId,
                              Amount=ph.TotalAmount,
                              Currency=ph.Currency,
                              BID_BankRefNo=ph.BID_BankRefNo,
                              PRN_TrackingId=ph.PRN_TrackingId,
                              TXNDATETIME=ph.TXNDATETIME,
                              TransactionStatus=ph.TransactionStatus,
                              CategoryId=ClassName.Id,
                              CategoryName=ClassName.CategoryName,
                              Name=users.UserName,
                              EmailId=users.EmailId,
                              QuizName=quiz.QuizName,
                              subjectId=SubjectName.Id,
                              subjectName=SubjectName.CategoryName,

                             
                          }).ToList();

            ViewBag.DeviceTypeAmount = 0;
            ViewBag.TotalAmount = 0;
            ViewBag.TotalIphneAmount = 0;
            ViewBag.totalAndroidAmount = 0;
            ViewBag.totalWebAmount = 0;
            var TotalAnswer = 0;
            ViewBag.TotalAnswer = TotalAnswer;
            return View(result);
        }

        #region "User Method"
        private List<UserAccountView> GetAccountList(string UserName, DateTime? StartDate = null, DateTime? EndDate = null, int deviceType = 0, int Price = 0, int ParentCategoryId = 0, String PaperName = null, String EmailId = null)
        {
            ViewBag.Totaluser = objDB.UserProfiles.Where(x => x.RoleId == 2).ToList();
            ViewBag.UserName = UserName;


            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.PaperName = PaperName;
            ViewBag.EmailId = EmailId;


            ViewBag.UserRole = new SelectList(objDB.UserRoles.Where(x => x.IsActive == true).ToList(), "RoleId", "RoleName");
            ViewBag.ActiveUser = new SelectList(objDB.Filters.Where(x => x.IsActive == true).ToList(), "Id", "FilterName");
            ViewBag.ClassUser = new SelectList(objDB.Categorys.Where(x => x.IsActive == true && x.ParentCategoryId == null).ToList(), "CategoryId", "CategoryName");

            var ObjUserAccountView = (from UseAcc in objDB.UserAccounts
                                      join UName in objDB.UserProfiles on UseAcc.UserId equals UName.Id
                                      join QuizName in objDB.Quizs on UseAcc.QuizId equals QuizName.QuizId
                                      join SubjectName in objDB.Categorys on QuizName.SubjectId equals SubjectName.Id
                                      join ClassName in objDB.Categorys on SubjectName.ParentCategoryId equals ClassName.Id
                                      //join Devicetype in objDB.Keywords on UName.DeviceTypeId equals Devicetype.KeywordValue
                                      where (UName.UserName.Contains(UserName)
                                    || UserName == null || UserName == "")
                                && (QuizName.QuizName.Contains(PaperName)
                                              || PaperName == null || PaperName == "")
                                              && (UName.EmailId.Contains(EmailId)
                                              || EmailId == null || EmailId == "")
                                    && (StartDate == null || EndDate == null || (UseAcc.TransDate >= StartDate && UseAcc.TransDate <= EndDate))
                                           && (ParentCategoryId == 0 || ClassName.Id == ParentCategoryId || SubjectName.Id == ParentCategoryId)
                                      select new UserAccountView
                                      {
                                          UserName = UName.UserName,
                                          QuizName = QuizName.QuizName,
                                          Price = QuizName.Price,
                                          DeviceTypeid = UName.DeviceTypeId,
                                          EmailId = UName.EmailId,
                                          ClassName = ClassName.CategoryName,
                                          SubjectName = SubjectName.CategoryName,
                                          TransDate = UseAcc.TransDate

                                      }




      ).ToList();
            //Device type Amount
            var DeviceTypeAccountList = ObjUserAccountView.Where(x => x.DeviceTypeid == deviceType).ToList();
            decimal DeviceTypeAmount = 0;
            if (DeviceTypeAccountList.Count > 0)
            {
                foreach (var item in DeviceTypeAccountList)
                {
                    if (item.Price == null)
                    {
                        DeviceTypeAmount = DeviceTypeAmount + 0;
                    }
                    else
                    {
                        DeviceTypeAmount = DeviceTypeAmount + (decimal)item.Price;
                    }

                }
            }

            //Andriod Type  Amount
            var AndroidTypeAccountList = ObjUserAccountView.Where(x => x.DeviceTypeid == 2).ToList();
            decimal AndridTypeAmount = 0;
            if (AndroidTypeAccountList.Count > 0)
            {
                foreach (var item in AndroidTypeAccountList)
                {
                    if (item.Price == null)
                    {
                        AndridTypeAmount = AndridTypeAmount + 0;
                    }
                    else
                    {
                        AndridTypeAmount = AndridTypeAmount + (decimal)item.Price;
                    }

                }
            }
            //---------------
            //Iphone Type  Amount
            var IphoneTypeAccountList = ObjUserAccountView.Where(x => x.DeviceTypeid == 1).ToList();
            decimal iphoneamount = 0;
            if (IphoneTypeAccountList.Count > 0)
            {
                foreach (var item in IphoneTypeAccountList)
                {
                    if (item.Price == null)
                    {
                        iphoneamount = iphoneamount + 0;
                    }
                    else
                    {
                        iphoneamount = iphoneamount + (decimal)item.Price;
                    }

                }
            }
            //---------------
            decimal TotalAmount = 0;
            if (ObjUserAccountView.Count > 0)
            {
                foreach (var item in ObjUserAccountView)
                {
                    if (item.Price == null)
                    {
                        TotalAmount = TotalAmount + 0;
                    }
                    else
                    {
                        TotalAmount = TotalAmount + (decimal)item.Price;
                    }

                }
            }
            ViewBag.DeviceTypeAmount = DeviceTypeAmount;
            ViewBag.TotalAmount = TotalAmount;
            ViewBag.TotalIphneAmount = iphoneamount;
            ViewBag.totalAndroidAmount = AndridTypeAmount;
            var TotalAnswer = ObjUserAccountView.Count();
            ViewBag.TotalAnswer = TotalAnswer;

            return DeviceTypeAccountList;
        }
        #endregion



    }
}
