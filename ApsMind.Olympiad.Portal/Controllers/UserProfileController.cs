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
using System.Net.Mail;
using System.Web.Configuration;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize(new string[] { "1" })]
    //[CustomAuthorize(Roles = "Admin")]
    public class UserProfileController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        List<UserProfileView> objUserList = null;


        [HttpGet]
        public ActionResult Index(string UserName, string EmailId, int RoleId = 0, int ActiveUser = -1, DateTime? StartDate = null, DateTime? EndDate = null, String TEST = null, int ClassUser = 0)
        {
            ViewBag.UserName = UserName;
            ViewBag.EmailId = EmailId;
            ViewBag.RolId = RoleId;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);
            Common.SetSearchConditionValue(Common.searchConditions, "RoleId", RoleId);
            Common.SetSearchConditionValue(Common.searchConditions, "ActiveUser", ActiveUser);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "ClassUser", ClassUser);
            TempData["SearchConditions"] = Common.searchConditions;
            return View(GetUserList(UserName, EmailId, RoleId, ActiveUser, StartDate, EndDate, ClassUser));
        }

        [HttpPost]
        public ActionResult Index(string UserName, string EmailId, int RoleId = 0, int ActiveUser = -1, DateTime? StartDate = null, DateTime? EndDate = null, int ClassUser = 0)
        {
            ViewBag.UserName = UserName;
            ViewBag.EmailId = EmailId;
            ViewBag.RolId = RoleId;
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            Common.SetSearchConditionValue(Common.searchConditions, "UserName", UserName);
            Common.SetSearchConditionValue(Common.searchConditions, "EmailId", EmailId);
            Common.SetSearchConditionValue(Common.searchConditions, "RoleId", RoleId);
            Common.SetSearchConditionValue(Common.searchConditions, "ActiveUser", ActiveUser);
            Common.SetSearchConditionValue(Common.searchConditions, "StartDate", StartDate);
            Common.SetSearchConditionValue(Common.searchConditions, "EndDate", EndDate);
            Common.SetSearchConditionValue(Common.searchConditions, "ClassUser", ClassUser);
            TempData["SearchConditions"] = Common.searchConditions;

            return View(GetUserList(UserName, EmailId, RoleId, ActiveUser, StartDate, EndDate, ClassUser));
        }


        public ActionResult ExportToExcel()
        {

            DataTable Client = new DataTable("teste");
            DataRow drow = null;
            Client.Columns.Add("UserId", typeof(int));
            Client.Columns.Add("Name", typeof(string));
            Client.Columns.Add("User Name", typeof(string));
            Client.Columns.Add("Password", typeof(string));
            Client.Columns.Add("Role", typeof(string));
            Client.Columns.Add("Email Id", typeof(string));
            Client.Columns.Add("IsActive", typeof(string));
            Client.Columns.Add("Class", typeof(string));
            Client.Columns.Add("MobileNumber", typeof(string));


            string UserName = (String)Common.GetSearchConditionValue(Common.searchConditions, "UserName");
            string EmailId = (String)Common.GetSearchConditionValue(Common.searchConditions, "EmailId");
            int RoleId = (int)Common.GetSearchConditionValue(Common.searchConditions, "RoleId");

            object tempData1 = Common.GetSearchConditionValue(Common.searchConditions, "StartDate");
            DateTime? StartDate = (tempData1 == null ? new Nullable<DateTime>() : (DateTime?)tempData1);

            object tempData2 = Common.GetSearchConditionValue(Common.searchConditions, "EndDate");
            DateTime? EndDate = (tempData2 == null ? new Nullable<DateTime>() : (DateTime?)tempData2);



            int ActiveUser = (int)Common.GetSearchConditionValue(Common.searchConditions, "ActiveUser");
            int ClassUser = (int)Common.GetSearchConditionValue(Common.searchConditions, "ClassUser");

            List<UserProfileView> objList = GetUserList(UserName, EmailId, RoleId, ActiveUser, StartDate, EndDate, ClassUser);
            var Result = objUserList;
           
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
                drow["Role"] = list.Role;
                drow["Email Id"] = list.EmailId;
                drow["IsActive"] = list.IsActive;
                drow["Class"] = list.ClassId;
                drow["MobileNumber"] = list.MobileNumber;
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

        public ActionResult Create()
        {
            ViewBag.RoleName = objDB.UserRoles.Where(x => x.IsActive == true && x.RoleId == 1 || x.RoleId == 3).ToList();
            ViewBag.Class = objDB.Categorys.Where(x => x.IsActive == true && x.CategoryTypeId == 1).ToList();
            ViewBag.CountryList = objDB.Locations.Where(x => x.LocationTypeId == 1).ToList();
            ViewBag.stateEdit = objDB.Locations.Where(X => X.ParentLocationId == -1).ToList(); ;
            return View();
        }

        public void BindPostData(Int64 CountryID = 0)
        {
            ViewBag.RoleName = objDB.UserRoles.Where(x => x.IsActive == true).ToList();
            ViewBag.RoleName = objDB.UserRoles.Where(x => x.IsActive == true).ToList();
            ViewBag.Class = objDB.Categorys.Where(x => x.IsActive == true && x.CategoryTypeId == 1).ToList();
            ViewBag.CountryList = objDB.Locations.Where(x => x.LocationTypeId == 1).ToList();
            ViewBag.stateEdit = objDB.Locations.Where(X => X.ParentLocationId == CountryID).ToList();
        }

        [HttpPost]
        public ActionResult Create(UserProfile ObjUserProfile)
        {
            try
            {
                var ExitEmailID = objDB.UserProfiles.Where(x => x.EmailId == ObjUserProfile.EmailId).FirstOrDefault();
                if (ExitEmailID != null)
                {
                    ObjUserProfile.Password = "";
                    ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                       "Error" + "!</strong>" + "Weak Password re enter you password." + ".</div>";
                }
                else
                {
                    UserProfile ObjCheckuserprofile = objDB.UserProfiles.Where(x => x.UserName == ObjUserProfile.UserName && x.Password == ObjUserProfile.Password).FirstOrDefault();
                    if (ObjCheckuserprofile != null)
                    {

                        ObjUserProfile.Password = "";
                        ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                           "Error" + "!</strong>" + "Weak Password re enter you password." + ".</div>";

                    }
                    else
                    {
                        if (ObjUserProfile.CountryId == 0)
                        {
                            ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                           "Error" + "!</strong>" + "Select Country" + ".</div>";
                            BindPostData(ObjUserProfile.CountryId);
                            return View(ObjUserProfile);
                        }
                        else if (ObjUserProfile.StateId == 0)
                        {
                            ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                                                       "Error" + "!</strong>" + "Select State" + ".</div>";
                            BindPostData(ObjUserProfile.CountryId);
                            return View(ObjUserProfile);
                        }
                        else if (ObjUserProfile.RoleId == 0)
                        {
                            ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                                                       "Error" + "!</strong>" + "Select Role" + ".</div>";
                            BindPostData(ObjUserProfile.CountryId);
                            return View(ObjUserProfile);
                        }
                        else
                        {
                            ObjUserProfile.CreatedBy = ApplicationSession.CurrentUser.Id;
                            ObjUserProfile.CreationDate = System.DateTime.Now;
                            ObjUserProfile.RoleId = ObjUserProfile.RoleId;
                            objDB.UserProfiles.Add(ObjUserProfile);
                            objDB.SaveChanges();
                            //
                            string body = "";
                            //string body = string.Empty;
                            using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/emailTemplate/silverzone.html")))
                            {
                                string readFile = reader.ReadToEnd();
                                body = readFile;

                            }
                            body = body.Replace("{{Username}}", ObjUserProfile.UserName);
                            body = body.Replace("{{Password}}", ObjUserProfile.Password);
                            body = body.Replace("{{UserName}}", ObjUserProfile.UserName);
                            Send(ObjUserProfile.EmailId, "", "Welcome to Silver Zone ", body, true);
                            //
                            TempData["Message"] = "New record created successfully!";
                            return RedirectToAction("Index");
                        }

                    }
                }


                BindPostData(ObjUserProfile.CountryId);
                return View(ObjUserProfile);

            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                BindPostData(ObjUserProfile.CountryId);
                return View(ObjUserProfile);
            }
        }

        //
        // GET: /UserProfile/Edit/5
        public ActionResult Edit(int UserId = 0, int CountryId = 0)
        {


            UserProfile ObjUser = objDB.UserProfiles.Where(x => x.Id == UserId).FirstOrDefault();

            ViewBag.RoleName = objDB.UserRoles.Where(x => x.IsActive == true).ToList();
            ViewBag.ClassEdit = objDB.Categorys.Where(x => x.IsActive == true && x.CategoryTypeId == 1).ToList();
            var Result = ViewBag.Class;
            ViewBag.CountryList = objDB.Locations.Where(x => x.LocationTypeId == 1).ToList();
            ViewBag.stateEdit = objDB.Locations.Where(x => x.ParentLocationId == ObjUser.CountryId).ToList();
            return View(ObjUser);

        }

        public void BindPOstDataEdit(Int64 CountryId = 0)
        {
            ViewBag.CountryList = objDB.Locations.Where(x => x.LocationTypeId == 1).ToList();
            ViewBag.stateEdit = objDB.Locations.Where(x => x.ParentLocationId == CountryId).ToList();
            ViewBag.ClassEdit = objDB.Categorys.Where(x => x.IsActive == true && x.CategoryTypeId == 1).ToList();
            ViewBag.RoleName = objDB.UserRoles.Where(x => x.IsActive == true).ToList();
            ViewBag.RoleName = objDB.UserRoles.Where(x => x.IsActive == true).ToList();
        }
        //
        // POST: /UserProfile/Edit/5
        [HttpPost]
        public ActionResult Edit(UserProfile ObjUserProfile)
        {
            try
            {
                UserProfile Existing = objDB.UserProfiles.Where(x => x.Id == ObjUserProfile.Id).FirstOrDefault();
                if (Existing != null)
                {
                    if (ObjUserProfile.RoleId == 2)
                    {
                        if (ObjUserProfile.ClassId == 0)
                        {
                            ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                  "Error" + "!</strong>" + "Select Class!" + ".</div>";
                            BindPOstDataEdit(ObjUserProfile.CountryId);
                            return View(ObjUserProfile);
                        }
                        else
                        {
                            ObjUserProfile.ClassId = ObjUserProfile.ClassId;
                        }

                    }
                    else
                    {
                        ObjUserProfile.ClassId = 0;
                    }
                    if (ObjUserProfile.CountryId == 0)
                    {
                        ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                       "Error" + "!</strong>" + "Select Country" + ".</div>";
                        BindPOstDataEdit(ObjUserProfile.CountryId);
                        return View(ObjUserProfile);
                    }
                    else if (ObjUserProfile.StateId == 0)
                    {
                        ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                                                   "Error" + "!</strong>" + "Select State" + ".</div>";
                        BindPOstDataEdit(ObjUserProfile.CountryId);
                        return View(ObjUserProfile);
                    }
                    else if (ObjUserProfile.RoleId == 0)
                    {
                        ViewBag.Message = "<div class=\"alert alert-danger\"><a href=\"#\" class=\"close\" data-dismiss=\"alert\" aria-label=\"close\">&times;</a> <strong>" +
                                                   "Error" + "!</strong>" + "Select Role" + ".</div>";
                        BindPOstDataEdit(ObjUserProfile.CountryId);
                        return View(ObjUserProfile);
                    }
                    ObjUserProfile.CreatedBy = Existing.CreatedBy;
                    ObjUserProfile.CreationDate = Existing.CreationDate;
                    ObjUserProfile.RoleId = ObjUserProfile.RoleId;
                    ObjUserProfile.EmailId = ObjUserProfile.EmailId;
                    ObjUserProfile.UserName = ObjUserProfile.UserName;
                    ObjUserProfile.UpdatedBy = ApplicationSession.CurrentUser.Id;
                    ObjUserProfile.UpdationDate = System.DateTime.Now;
                    objDB.Entry(Existing).CurrentValues.SetValues(ObjUserProfile);
                    objDB.Entry(Existing).State = EntityState.Modified;
                    objDB.SaveChanges();
                    TempData["Message"] = " Record has  been Updated successfully!";

                    if (ApplicationSession.CurrentUser.RoleId == 1)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.RoleName = objDB.UserRoles.Where(x => x.IsActive == true).ToList();
                        return View(ObjUserProfile);
                    }


                }
                BindPOstDataEdit(ObjUserProfile.CountryId);
                return View(ObjUserProfile);
            }
            catch (Exception ex)
            {

                Common.LogError("Error", ex);
                BindPOstDataEdit(ObjUserProfile.CountryId);
                return View(ObjUserProfile);
            }
        }

        public static bool Send(string toemail = "", string ccmail = "", string subjecttxt = "", string bodytext = "", bool ishtmlcontent = false)
        {

            //mailMessage.Body = body;
            //mailMessage.IsBodyHtml = true;

            bool ismailsend = false;
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(WebConfigurationManager.AppSettings["fromemaildisplayas"] + "<" + WebConfigurationManager.AppSettings["fromemail"] + ">");

            try
            {
                mail.To.Add((toemail.Length <= 0) ? WebConfigurationManager.AppSettings["sendto"].ToString() : toemail);
                string bccemail = WebConfigurationManager.AppSettings["bccemail"].ToString().Trim();

                if (bccemail.Length > 0)
                {
                    mail.Bcc.Add(bccemail);
                }
                if (ccmail.Length > 0)
                {
                    mail.CC.Add(ccmail);
                }
                mail.Subject = subjecttxt;
                mail.IsBodyHtml = ishtmlcontent;
                mail.Body = bodytext.ToString();


                SmtpClient SmtpServer = new SmtpClient(WebConfigurationManager.AppSettings["smtpserver"]);
                SmtpServer.Port = int.Parse(WebConfigurationManager.AppSettings["port"].ToString());
                SmtpServer.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["fromemail"], WebConfigurationManager.AppSettings["pwd"]);
                SmtpServer.EnableSsl = bool.Parse(WebConfigurationManager.AppSettings["EnableSsl "].ToString().Trim());

                SmtpServer.Send(mail);
                ismailsend = true;

            }
            catch (Exception ex)
            {
                Common.LogError("Error", ex);
                ismailsend = false;

            }
            return ismailsend;
        }
        #region "User Method"
        private List<UserProfileView> GetUserList(string UserName, string EmailId, int RoleId = 0, int ActiveUser = -1, DateTime? StartDate = null, DateTime? EndDate = null, int ClassUser = 0)
        {
            ViewBag.UserName = UserName;
            ViewBag.EmailId = EmailId;
            ViewBag.RolId = RoleId;
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

            objUserList = (from Udetails in objDB.UserProfiles
                           join RoleName in objDB.UserRoles on Udetails.RoleId equals RoleName.RoleId
                           join classs in objDB.Categorys on Udetails.ClassId equals classs.Id into dt
                           from d in dt.DefaultIfEmpty()
                           orderby Udetails.Id
                           where (Udetails.RoleId != 1)
                           && (Udetails.UserName.Contains(UserName)
                                     || UserName == null || UserName == "")
                           && (Udetails.EmailId.Contains(EmailId)
                           || EmailId == null || EmailId == "")
                           && (RoleId == 0 || RoleId == 0 || Udetails.RoleId == RoleId)
                             && (ClassUser == 0 || ClassUser == 0 || Udetails.ClassId == ClassUser)
                            && (StartDate == null || EndDate == null || (Udetails.CreationDate >= StartDate && Udetails.CreationDate <= EndDate))
                           && (Udetails.IsActive == Active)
                           select new UserProfileView
                           {

                               UserId = Udetails.Id,
                               Name = Udetails.UserName
                            ,

                               UserName = Udetails.UserName,
                               Password = Udetails.Password,
                               Role = RoleName.RoleName,
                               EmailId = Udetails.EmailId,
                               IsActive = Udetails.IsActive,
                               CreationDate = Udetails.CreationDate,
                               ClassId =Udetails.ClassId,
                               MobileNumber = Udetails.MobileNumber

                           }).ToList();

            var TotalUser = objUserList.Count();
            ViewBag.TotalUser = TotalUser;
            return objUserList;
        }
        #endregion


    }
}
