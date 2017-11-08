using ApsMind.Olympiad.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ApsMind.Olympiad.Portal
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected  void Application_BeginRequest(object sender, EventArgs e)
        //{
           
        //    string currentUrl = HttpContext.Current.Request.Path.ToLower();
        //    if (currentUrl.StartsWith("http://www.silverzoneonline.com"))
        //    {
        //        Response.Status = "301 Moved Permanently";
        //        Response.AddHeader("Location", currentUrl.Replace("http://www.silverzoneonline.com", "http://silverzoneonline.com"));
        //        Response.End();
        //    }
        //}
        
        //protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        //{
        //    string currentUrl = HttpContext.Current.Request.Path.ToLower();
        //    if (currentUrl.StartsWith("http://www.silverzoneonline.com"))
        //    {
        //        Response.Status = "301 Moved Permanently";
        //        Response.AddHeader("Location", currentUrl.Replace("http://www.silverzoneonline.com", "http://silverzoneonline.com"));
        //        Response.End();

        //    }
        //}


        protected void Application_Error()
         {
            Exception unhandledException = Server.GetLastError();
            HttpException httpException = unhandledException as HttpException;
            if (httpException == null)
            {
                Exception innerException = unhandledException.InnerException;
                httpException = innerException as HttpException;
            }

            if (httpException != null )
            {
                int httpCode = httpException.GetHttpCode();
                switch (httpCode)
                {
                    case (int)HttpStatusCode.NotFound:
                        //Response.Redirect("/Error/global");
                        break;

                    default:
                       
                            Common.LogError(HttpContext.Current.Request.Url.ToString(), httpException);
                            //Response.Redirect("/Error/global");
                        
                        break;
                }
            }
            else
            {
                Common.LogError(HttpContext.Current.Request.Url.ToString(), unhandledException);
                //Response.Redirect("/Error/global");
            }
        }
    }
}
