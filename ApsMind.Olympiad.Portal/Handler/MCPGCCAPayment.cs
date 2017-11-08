using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MCPG.CCA.Payment
{
	public class MCPG
	{
	}
    public static class CCAConfig
    {

        public static bool IsTestPayment { get; set; }
        public static string TestURL { get; set; }
        public static string LiveURL { get; set; }
        public static string merchant_id { get; set; }
        public static string access_code { get; set; }
        public static string workingKey { get; set; }
        public static string redirect_url { get; set; }
        public static string cancel_url { get; set; }
        public static string validator { get; set; }
        static CCAConfig()
        {
            IsTestPayment = Convert.ToBoolean(WebConfigurationManager.AppSettings["CCAIsTestPayment"].ToString());
            TestURL = WebConfigurationManager.AppSettings["CCATestURL"].ToString();
            LiveURL = WebConfigurationManager.AppSettings["CCALiveURL"].ToString();
            merchant_id = WebConfigurationManager.AppSettings["CCAmerchant_id"].ToString();
            access_code = WebConfigurationManager.AppSettings["CCAaccess_code"].ToString();
            workingKey = WebConfigurationManager.AppSettings["CCAworkingKey"].ToString();
            redirect_url = WebConfigurationManager.AppSettings["CCAredirect_url"].ToString();
            cancel_url = WebConfigurationManager.AppSettings["CCAcancel_url"].ToString();
            validator = WebConfigurationManager.AppSettings["CCAValidator"].ToString();
        }
    }

    public class CCARemotePost
    {


        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


        public string Url = CCAConfig.IsTestPayment ? CCAConfig.TestURL : CCAConfig.LiveURL;
        public string Method = "post";
        public string FormName = "form1";

        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();

            System.Web.HttpContext.Current.Response.Write("<html><head>");

            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");

            System.Web.HttpContext.Current.Response.End();
        }
    }
}