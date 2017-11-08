using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace ApsMind.Olympiad.Portal.Models
{

    public class Common
    {
        static DatabaseContext objDB = new DatabaseContext();

        public static IDictionary<string, object> searchConditions = new Dictionary<string, object>();

        public static object GetSearchConditionValue(IDictionary<string, object> searchConditions, string key)
        {
            object tempValue = null;

            if (searchConditions != null && searchConditions.Keys.Contains(key))
            {
                searchConditions.TryGetValue(key, out tempValue);
            }
            return tempValue;
        }

        public static bool SetSearchConditionValue(IDictionary<string, object> searchConditions, string key, object tempValue)
        {
            if (searchConditions != null && searchConditions.Keys.Contains(key))
            {
                searchConditions[key] = tempValue;
            }
            else
            {
                searchConditions.Add(new KeyValuePair<string, object>(key, tempValue));
            }
            return true;
        }

        //public static void LogError(String url, String msg)
        //{
        //    try
        //    {
        //        objDB.ErrorLogs.Add(new ErrorLog
        //        {
        //            Error = "Path : " + url + "; Msg : " + msg,
        //            LogType = LogTypes.SystemLog,
        //            LogTime = DateTime.Now

        //        });
        //        objDB.SaveChanges();
        //    }
        //    catch { }
        //    finally { }
        //}

        //public static void LogError(String url, Exception ex)
        //{
        //    try
        //    {
        //        objDB.ErrorLogs.Add(new ErrorLog
        //        {
        //            Error = "Path : " + url + "; Msg : " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""),
        //            LogType = LogTypes.UserLog,
        //            LogTime = DateTime.Now

        //        });
        //        objDB.SaveChanges();
        //    }
        //    catch { }
        //    finally { }
        //}

        public static void LogError1(Exception ex)
        {
            try
            {

                string strPath = HttpContext.Current.Server.MapPath("~/Content");
                string strLogFilePath = strPath + "/log.txt";

                if (!File.Exists(strLogFilePath))
                {
                    File.Create(strLogFilePath).Close();
                }
                using (StreamWriter w = File.AppendText(strLogFilePath))
                {
                    w.WriteLine("\r\nLog: ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error Message:" + ex;
                    w.WriteLine(err);
                    w.WriteLine("Inner Exception");
                    w.WriteLine((ex.InnerException != null ? ex.InnerException.Message : ""));

                    w.WriteLine("");
                    w.Flush();
                    w.Close();
                }



            }
            catch { }
            finally { }
        }
        public static string GetPath(Int32 id = 0)
        {
            string path = "";
            if (id == 0)
            {
                return path;
            }
            path = "";
            List<Category> objCategoryList = objDB.Categorys.ToList();
            Category CurrentCategory = objCategoryList.Where(x => x.Id == id).FirstOrDefault();
            while (true)
            {
                path = "/" + CurrentCategory.CategoryName + path;
                if (CurrentCategory.ParentCategoryId == null) { break; }
                Category ParentCategory = objCategoryList.Where(x => x.Id == CurrentCategory.ParentCategoryId).FirstOrDefault();
                CurrentCategory = ParentCategory;

            }

            return path;

        }
        public static void LogError(String url, Exception ex)
        {
            try
            {
                objDB.ErrorLogs.Add(new ErrorLog
                {
                    Error = "Path : " + url + "; Msg : " + ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""),
                    LogType = LogTypes.UserLog,
                    LogTime = DateTime.Now

                });
                objDB.SaveChanges();
            }
            catch { }
            finally { }
        }
        public static void LogError(String url, String msg)
        {
            try
            {
                objDB.ErrorLogs.Add(new ErrorLog
                {
                    Error = "Path : " + url + "; Msg : " + msg,
                    LogType = LogTypes.SystemLog,
                    LogTime = DateTime.Now

                });
                objDB.SaveChanges();
            }
            catch { }
            finally { }
        }

        public static void TXTErrorLog(Exception ex, string a, string b, string c)
        {
            try
            {

                string strPath = HttpContext.Current.Server.MapPath("~/Content");
                string strLogFilePath = strPath + "/log.txt";

                if (!File.Exists(strLogFilePath))
                {
                    File.Create(strLogFilePath).Close();
                }
                using (StreamWriter w = File.AppendText(strLogFilePath))
                {
                    w.WriteLine("\r\nLog: ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error Message:" + ex;
                    w.WriteLine(err);
                    w.WriteLine("a=" + a);
                    w.WriteLine("b=" + b);
                    w.WriteLine("c=" + c);
                    w.WriteLine("");
                    w.Flush();
                    w.Close();
                }



            }
            catch { }
            finally { }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}