using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ApsMind.Olympiad.Framework.Entity;

namespace ApsMind.Olympiad.Portal.Models
{
    [Serializable()]
    public class ApplicationSession
    {

        private static string key = "Login";

        public static void Login(UserProfile objLogin)
        {
            System.Web.HttpContext.Current.Session[key] = objLogin;
            System.Web.HttpContext.Current.Session.Timeout = 60;

        }

        public static UserProfile CurrentUser
        {
            get
            {
                if ((System.Web.HttpContext.Current.Session != null))
                {
                    return (UserProfile)System.Web.HttpContext.Current.Session[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public static void Logout()
        {
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
        }
    }
}