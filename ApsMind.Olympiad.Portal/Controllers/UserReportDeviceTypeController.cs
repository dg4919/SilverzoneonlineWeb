using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Portal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize(new string[] { "1" })]
    public class UserReportDeviceTypeController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        List<UserProfileView> objUserList = null;
        [HttpGet]
        public ActionResult Index(string UserName, string EmailId, int ActiveUser = -1, DateTime? StartDate = null, DateTime? EndDate = null, String TEST = null, int ClassUser = 0, int deviceType = 2)
        {
            ViewBag.UserName = UserName;
            ViewBag.EmailId = EmailId;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);

            Common.SetSearchConditionValue(Common.searchConditions, "ActiveUser", ActiveUser);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "ClassUser", ClassUser);
            Common.SetSearchConditionValue(Common.searchConditions, "DeviceType", deviceType);
            TempData["SearchConditions"] = Common.searchConditions;
            return View(GetUserList(UserName, EmailId, ActiveUser, StartDate, EndDate, ClassUser, deviceType));
        }

        [HttpPost]
        public ActionResult Index(string UserName, string EmailId, int ActiveUser = -1, DateTime? StartDate = null, DateTime? EndDate = null, int ClassUser = 0, int deviceType = 2)
        {
            ViewBag.UserName = UserName;
            ViewBag.EmailId = EmailId;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);

            Common.SetSearchConditionValue(Common.searchConditions, "ActiveUser", ActiveUser);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "ClassUser", ClassUser);
            Common.SetSearchConditionValue(Common.searchConditions, "DeviceType", deviceType);
            TempData["SearchConditions"] = Common.searchConditions;

            return View(GetUserList(UserName, EmailId, ActiveUser, StartDate, EndDate, ClassUser, deviceType));
        }
        public ActionResult ExportToExcel()
        {

            DataTable Client = new DataTable("teste");
            DataRow drow = null;
            Client.Columns.Add("UserId", typeof(int));
            Client.Columns.Add("Name", typeof(string));
            Client.Columns.Add("User Name", typeof(string));
            Client.Columns.Add("Password", typeof(string));
            Client.Columns.Add("Email Id", typeof(string));
            Client.Columns.Add("IsActive", typeof(string));
           
            Client.Columns.Add("Class", typeof(string));


            string UserName = (String)Common.GetSearchConditionValue(Common.searchConditions, "UserName");
            string EmailId = (String)Common.GetSearchConditionValue(Common.searchConditions, "EmailId");


            object tempData1 = Common.GetSearchConditionValue(Common.searchConditions, "StartDate");
            DateTime? StartDate = (tempData1 == null ? new Nullable<DateTime>() : (DateTime?)tempData1);

            object tempData2 = Common.GetSearchConditionValue(Common.searchConditions, "EndDate");
            DateTime? EndDate = (tempData2 == null ? new Nullable<DateTime>() : (DateTime?)tempData2);


            int deviceType = (int)Common.GetSearchConditionValue(Common.searchConditions, "DeviceType");
            int ActiveUser = (int)Common.GetSearchConditionValue(Common.searchConditions, "ActiveUser");
            int ClassUser = (int)Common.GetSearchConditionValue(Common.searchConditions, "ClassUser");

            List<UserProfileView> objList = GetUserList(UserName, EmailId, ActiveUser, StartDate, EndDate, ClassUser,deviceType);
            var Result = objUserList;
            //List<UserProfileView> objList = objUserList.ToList();
            if (objList.Count == 0)
            {
                return null;
            }


            foreach (var list in objList)
            {
                drow = Client.NewRow();
                drow["UserId"] = list.UserId;
                drow["Name"] = list.Name;
                drow["User Name"] = list.UserName;
                drow["Password"] = list.Password;

                drow["Email Id"] = list.EmailId;
                drow["IsActive"] = list.IsActive;
               

                drow["Class"] = list.ClassId;


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
        public ActionResult IPhoneUser(string UserName, string EmailId, int ActiveUser = -1, DateTime? StartDate = null, DateTime? EndDate = null, String TEST = null, int ClassUser = 0, int deviceType = 1)
        {
            ViewBag.UserName = UserName;
            ViewBag.EmailId = EmailId;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);

            Common.SetSearchConditionValue(Common.searchConditions, "ActiveUser", ActiveUser);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "ClassUser", ClassUser);
            Common.SetSearchConditionValue(Common.searchConditions, "DeviceType", deviceType);
            TempData["SearchConditions"] = Common.searchConditions;
            return View(GetUserList(UserName, EmailId, ActiveUser, StartDate, EndDate, ClassUser, deviceType));
        }

        [HttpPost]
        public ActionResult IPhoneUser(string UserName, string EmailId, int ActiveUser = -1, DateTime? StartDate = null, DateTime? EndDate = null, int ClassUser = 0, int deviceType = 1)
        {
            ViewBag.UserName = UserName;
            ViewBag.EmailId = EmailId;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);

            Common.SetSearchConditionValue(Common.searchConditions, "ActiveUser", ActiveUser);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "ClassUser", ClassUser);
            Common.SetSearchConditionValue(Common.searchConditions, "DeviceType", deviceType);
            TempData["SearchConditions"] = Common.searchConditions;

            return View(GetUserList(UserName, EmailId, ActiveUser, StartDate, EndDate, ClassUser, deviceType));
        }

        [HttpGet]
        public ActionResult View(Int32? UserID)
        {
            SerachAnsSheet ObjSeachQuestion = new SerachAnsSheet();

            ObjSeachQuestion.ListQuestionView = GetAnsSheetList(UserID);
            ViewBag.QuestionLevelQuestion = new SelectList(objDB.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList(), "KeywordValue", "KeywordText");
            return View(ObjSeachQuestion);
        }
        [HttpGet]
        public ActionResult UserAccountView(Int32? UserID)
        {

            return View(GetAccountView(UserID));

        }
        #region "User Method"
        private List<UserProfileView> GetUserList(string UserName, string EmailId, int ActiveUser = -1, DateTime? StartDate = null, DateTime? EndDate = null, int ClassUser = 0, int deviceType = 0)
        {
            ViewBag.Totaluser = objDB.UserProfiles.Where(x => x.RoleId == 2).ToList();
            ViewBag.UserName = UserName;
            ViewBag.EmailId = EmailId;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            Boolean Active = true;
            if (ActiveUser == -1)
            {
                Active = true;

            }
            else
            {
                Active = Convert.ToBoolean(ActiveUser);
            }
            ViewBag.UserRole = new SelectList(objDB.UserRoles.Where(x => x.IsActive == true).ToList(), "RoleId", "RoleName");
            ViewBag.ActiveUser = new SelectList(objDB.Filters.Where(x => x.IsActive == true).ToList(), "Id", "FilterName");
            ViewBag.ClassUser = new SelectList(objDB.Categorys.Where(x => x.IsActive == true && x.ParentCategoryId == null).ToList(), "CategoryId", "CategoryName");

            var objUserList = (from Udetails in objDB.UserProfiles
                               join RoleName in objDB.UserRoles on Udetails.RoleId equals RoleName.RoleId
                               join Devicetype in objDB.Keywords on Udetails.DeviceTypeId equals Devicetype.KeywordValue
                               join classs in objDB.Categorys on Udetails.ClassId equals classs.Id into dt
                               from d in dt.DefaultIfEmpty()
                               orderby Udetails.Id
                               where Devicetype.KeywordType == "DeviceType"
                               && (Udetails.RoleId == 2)
                               && (Udetails.UserName.Contains(UserName)
                                || UserName == null || UserName == "")
                               && (Udetails.EmailId.Contains(EmailId)
                               || EmailId == null || EmailId == "")
                                && (ClassUser == 0 || ClassUser == 0 || Udetails.ClassId == ClassUser)
                                && (StartDate == null || EndDate == null || (Udetails.CreationDate >= StartDate && Udetails.CreationDate <= EndDate))
                               && (Udetails.IsActive == Active)
                              select new UserProfileView
                               {

                                   UserId = Udetails.Id,
                                   Name = Udetails.UserName
                                ,
                                   CheckDeviceType = Udetails.DeviceTypeId,
                                   UserName = Udetails.UserName,
                                   Password = Udetails.Password,
                                   Role = RoleName.RoleName,
                                   EmailId = Udetails.EmailId,
                                   IsActive = Udetails.IsActive,
                                   //CreationDate = Udetails.CreationDate,
                                   ClassId =Udetails.ClassId,
                                   MobileNumber = Udetails.MobileNumber,
                                   DeviceId = Udetails.DeviceId,
                                   DeviceTypeId = Devicetype.KeywordText,
                                   IMEINumber = Udetails.IMEINumber
                               }).ToList();

            var UserList = objUserList.Where(x => x.CheckDeviceType == deviceType).ToList();
            ViewBag.TotalUser = objUserList.Count();
            var DeviceTypeTotalUser = UserList.Count();
            ViewBag.IphoneDeviceType = objUserList.Where(x => x.CheckDeviceType == 1).Count();
            ViewBag.AndroidDevice = objUserList.Where(x => x.CheckDeviceType == 2).Count();
            ViewBag.DeviceTotalUser = DeviceTypeTotalUser;

            return UserList;
        }
        #endregion

        #region "User Method"
        private List<AnsSheetDetails> GetAnsSheetList(Int32? UserID)
        {
            var result = objDB.Questions.ToList();


            var objQusetionList = (from Anssheet in objDB.AnswerSheets
                                   join ansdetail in objDB.AnswerDetails on Anssheet.AnswerSheetId equals ansdetail.AnswerSheetId
                                   join UserName in objDB.UserProfiles on Anssheet.UserId equals UserName.Id
                                   join QuizName in objDB.Quizs on Anssheet.QuizId equals QuizName.QuizId
                                   join Subject in objDB.Categorys on QuizName.SubjectId equals Subject.Id
                                   join ClassName in objDB.Categorys on Subject.ParentCategoryId equals ClassName.Id
                                   join FreeType in objDB.Keywords on QuizName.FreeTypeId equals FreeType.KeywordValue
                                   where FreeType.KeywordType == "FreeType"
                                   && Anssheet.UserId == UserID
                                   group ansdetail by new

                                   {
                                       Anssheet.AnswerSheetId,
                                       QuizName.QuizName,
                                       UserName.UserName,
                                       Anssheet.StartDate,
                                       Anssheet.EndDate,
                                       FreeType = FreeType.KeywordText,
                                       Anssheet.OptainScore,
                                       ClassName.CategoryName,


                                   }

                                       into grouped

                                       select new AnsSheetDetails
                                       {
                                           NumberInGroup = grouped.Count(),
                                           AnsOption = grouped.ToList(),
                                           AnswerSheetId = grouped.Key.AnswerSheetId,
                                           StartDate = grouped.Key.StartDate,
                                           EndDate = grouped.Key.EndDate,
                                           OptainScore = grouped.Key.OptainScore,
                                           UserId = grouped.Key.UserName,
                                           FreeType = grouped.Key.FreeType,
                                           QuizId = grouped.Key.QuizName,
                                           ClassName = grouped.Key.CategoryName,

                                       }

                                   //select new QuestionView


       ).ToList();

            var TotalAnswer = objQusetionList.Count();
            ViewBag.TotalAnswer = TotalAnswer;
            return objQusetionList;
        }
        #endregion
        #region "User Method"
        private List<UserAccountView> GetAccountView(Int32? UserID)
        {

            //var Amount = objDB.UserAccounts.Where(x => x.UserId == UserID).ToList();


            var ObjUserAccountView = (from UseAcc in objDB.UserAccounts

                                      join UserName in objDB.UserProfiles on UseAcc.UserId equals UserName.Id
                                      join QuizName in objDB.Quizs on UseAcc.QuizId equals QuizName.QuizId

                                      where UseAcc.UserId == UserID

                                      select new UserAccountView
                                      {
                                          UserName = UserName.UserName,
                                          QuizName = QuizName.QuizName,
                                          Price = QuizName.Price
                                      }

                                   //select new QuestionView


       ).ToList();
            decimal Amount = 0;
            foreach (var item in ObjUserAccountView)
            {
                Amount = Amount + (decimal)item.Price;
            }
            ViewBag.TotalAmount = Amount;
            var TotalAnswer = ObjUserAccountView.Count();
            ViewBag.TotalAnswer = TotalAnswer;
            return ObjUserAccountView;
        }
        #endregion

    }
}
