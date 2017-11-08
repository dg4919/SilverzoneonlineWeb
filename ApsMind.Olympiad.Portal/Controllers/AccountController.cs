using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ApsMind.Olympiad.Portal.Models;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Portal.Controllers;
using System.Threading.Tasks;
using ApsMind.Olympiad.Portal.Results;
using System.Net;
using Microsoft.Web.WebPages.OAuth;
using DotNetOpenAuth.AspNet;
using WebMatrix.WebData;
namespace ApsMind.Olympiad.Portal.Controllers
{
    
    public class AccountController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult UserLogin(string returnUrl)
        {
            
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Message = "";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Message = "";
            return View();
        }
        public ActionResult Logout()
        {
            ApplicationSession.Logout();
            return RedirectToAction("Login");
        }
        public ActionResult UserLogOff()
        {
            ApplicationSession.Logout();

            return RedirectToAction("UserLogin");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.StudentClass= objDB.StudentClass.Where(x => x.IsActive == true).ToList();
            ViewBag.CountryList = objDB.Country.Where(x => x.IsActive == true).ToList();

            IEnumerable<GenderType> genderType=Enum.GetValues(typeof(GenderType)).Cast<GenderType>();
            ViewBag.Gender = from gender in genderType
                             select new SelectListItem
                             {
                                 Text = gender.ToString(),
                                 Value = ((int)gender).ToString()
                             };
            //var glist = ViewBag.Gender;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserProfile model,FormCollection fc)
        {
            ViewBag.StudentClass = objDB.StudentClass.Where(x => x.IsActive == true).ToList();
            ViewBag.CountryList = objDB.Country.Where(x => x.IsActive == true).ToList();
            IEnumerable<GenderType> genderType = Enum.GetValues(typeof(GenderType)).Cast<GenderType>();
            ViewBag.Gender = from gender in genderType
                             select new SelectListItem
                             {
                                 Text = gender.ToString(),
                                 Value = ((int)gender).ToString()
                             };
           
            int stateId =Convert.ToInt32(fc["State"]);
            if (ModelState.IsValid)
            {
                using (var transaction = objDB.Database.BeginTransaction())
                {
                    try
                    {
                    var user = objDB.UserProfiles.Where(x => x.EmailId == model.EmailId).FirstOrDefault();
                        if(user==null)
                        {
                            UserProfile _user = new UserProfile();
                            _user.UserName = model.UserName;
                            _user.Password = model.Password;
                            _user.MobileNumber = model.MobileNumber;
                            _user.Gender = model.Gender;
                            _user.ClassId = model.ClassId;
                            _user.CountryId = model.CountryId;
                            _user.StateId = stateId;
                            _user.City = model.City;
                            _user.EmailId = model.EmailId;
                            _user.DeviceTypeId = 3;
                            _user.IsActive = true;
                            _user.RoleId = 2;
                            objDB.UserProfiles.Add(_user);
                            objDB.SaveChanges();
                            
                            return RedirectToAction("UserLogin", "Account");
                        }
                        else
                        {
                            ModelState.AddModelError("","Email Id already exists,please try with another email id");
                        }
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("Error occured", ex.Message);
                    }
                }
            }
            return RedirectToAction("UserLogin", "Account");
           
        }
       
        public JsonResult GetStateList(int id)
        {
            var stateList = objDB.State.Where(x => x.CountryId == id).ToList();
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserProfile UserLogIn)
        {
            UserProfile UserLogin = objDB.UserProfiles.FirstOrDefault(x => x.UserName == UserLogIn.UserName && x.Password == UserLogIn.Password);
            if (UserLogin == null)
            {
                //ViewBag.Message = "Invalid User Name Or Password.";
                TempData["Message"] = "Invalid User Name Or Password.";
                return View("Login");
            }
            else
            {
                ApplicationSession.Login(UserLogin);
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            ApplicationSession.Logout();

            return RedirectToAction("Login");
        }


        /* external login section */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
       public ActionResult _ExternalLogin(string provider,string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (DatabaseContext db = new DatabaseContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
