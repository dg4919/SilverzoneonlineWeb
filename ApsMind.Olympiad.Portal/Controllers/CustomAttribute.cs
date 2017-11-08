using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ApsMind.Olympiad.Portal.Models;
using ApsMind.Olympiad.Framework;

namespace ApsMind.Olympiad.Portal.Controllers
{
    //public class CustomAuthorizeAttribute : AuthorizeAttribute
    //{
    //    DatabaseContext objDB = new DatabaseContext();
    //    private readonly String[] allowedroles;

    //    public CustomAuthorizeAttribute(params String[] Roles)
    //    {
    //        this.allowedroles = Roles;
    //    }
    //    public override void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) &&
    //            !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
    //        {
    //            if (ApplicationSession.CurrentUser == null)
    //            {
    //                filterContext.Result = new RedirectToRouteResult(new
    //                        RouteValueDictionary(new { controller = "Account", action = "Login" }));
    //                return;
    //            }

    //            if (Roles == null || Roles == "" || Roles.Contains(ApplicationSession.CurrentUser.RoleId.ToString()))
    //            {
    //                bool authorize = false;
    //                Int64 GetUser = ApplicationSession.CurrentUser.UserId;
    //                int[] RoleId = allowedroles.Select(int.Parse).ToArray();
    //                foreach (var role in allowedroles)
    //                {
    //                    var Name = (from UProfile in objDB.UserProfiles
    //                                join Role in objDB.UserProfiles on UProfile.UserId equals Role.UserId
    //                                where UProfile.UserId == GetUser
    //                                select new
    //                                {
    //                                    UProfile
    //                                }).FirstOrDefault();

    //                    int RoleId1 = Name.UProfile.RoleId;
    //                    var user = (from Exit in objDB.UserProfiles
    //                                join rolid in RoleId on Exit.RoleId equals rolid
    //                                where Exit.RoleId == RoleId1
    //                                select new
    //                                {
    //                                    Exit
    //                                }).FirstOrDefault();

    //                    if (user != null)
    //                    {

    //                        authorize = true; /* return true if Entity has current user(active) with specific role */
    //                    }
    //                    else
    //                    {
    //                        filterContext.Result = new RedirectToRouteResult(new
    //                         RouteValueDictionary(new { controller = "Error", action = "CustomError" }));

    //                    }

    //                }

    //            }
    //            else
    //            {
    //                filterContext.Result = new RedirectToRouteResult(new
    //                 RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
    //            }
    //        }



    //    }
    //    protected override bool AuthorizeCore(HttpContextBase httpContext)
    //    {
    //        bool authorize = false;
    //        Int64 GetUser = ApplicationSession.CurrentUser.UserId;
    //        foreach (var role in allowedroles)
    //        {

    //            var Name = (from UProfile in objDB.UserProfiles
    //                        join Role in objDB.UserProfiles on UProfile.UserId equals Role.UserId
    //                        where UProfile.UserId == GetUser
    //                        select new
    //                        {
    //                            UProfile
    //                        }).FirstOrDefault();

    //            int RoleId = Name.UProfile.RoleId;
    //            var user = objDB.UserProfiles.Where(m => m.UserId == GetUser/* getting user form current context */ && m.RoleId == RoleId &&
    //            m.IsActive == true); // checking active users with allowed roles.  
    //            if (user.Count() > 0)
    //            {
    //                authorize = true; /* return true if Entity has current user(active) with specific role */
    //            }
    //        }
    //        return authorize;
    //    }
    //    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        filterContext.Result = new HttpUnauthorizedResult();
    //    }


    //}



    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        string[] Roles;
        string ReturnUrl;
        public CustomAuthorizeAttribute()
        {
            this.Roles = null;
        }
        public CustomAuthorizeAttribute(String[] roles)
        {
            this.Roles = roles;
        }
        public CustomAuthorizeAttribute(String[] roles, string returnUrl)
        {
            this.Roles = roles;
            this.ReturnUrl = returnUrl;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                if (ApplicationSession.CurrentUser == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = this.ReturnUrl }));

                    return;
                }

                if (this.Roles == null|| this.Roles.Length==0 || Roles.Contains(ApplicationSession.CurrentUser.RoleId.ToString()))
                {

                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Error", action = "CustomError" }));
                }
            }


        }
    }

}