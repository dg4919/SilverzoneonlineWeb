using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Portal.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApsMind.Olympiad.Portal.Controllers
{
      [CustomAuthorize(new string[] { "1"})]
    public class QuizReportController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        public ActionResult Index(String TestPaperName = null, int deviceType = 2)
        {
            ViewBag.TestPaperName = TestPaperName;
            ViewBag.DeviceType = deviceType;

            return View(GetQuizReport(deviceType, TestPaperName));
        }

        [HttpPost]
        public ActionResult Index(int deviceType = 2, string TestPaperName = null)
        {
            ViewBag.TestPaperName = TestPaperName;
            ViewBag.DeviceType = deviceType;
            
            return View(GetQuizReport(deviceType, TestPaperName));
        }

        [HttpGet]
        public ActionResult View(Int32 QuizID)
        {
            return View(GetScoreCard(QuizID));
        }

        #region "User Method"

        private QReport GetQuizReport(int deviceType = 0, String TestPaperName = null)
        {
            QReport objQReport = new QReport();
            var varQDetail = (from Q in objDB.Quizs
                              join AS in objDB.AnswerSheets on Q.QuizId equals AS.QuizId
                              join UP in objDB.UserProfiles on AS.UserId equals UP.Id
                              where (String.IsNullOrEmpty(TestPaperName) || Q.QuizName.Contains(TestPaperName))
                              group Q by new { Q.QuizId, Q.QuizName, UP.DeviceTypeId } into g

                              select new QDetail
                              {
                                  QuizId = g.Key.QuizId,
                                  QuizName = g.Key.QuizName,
                                  DeviceTypeId = g.Key.DeviceTypeId,
                                  UserCount = g.Count()

                              });

            try
            {
                objQReport.iPhoneUserCount = varQDetail.Where(y => y.DeviceTypeId == 1).Sum(x => x.UserCount);
            }
            catch { }
            try
            {
                objQReport.iAndroidUserCount = varQDetail.Where(y => y.DeviceTypeId == 2).Sum(x => x.UserCount);
            }
            catch { }

            objQReport.QDetailList = varQDetail.Where(x => x.DeviceTypeId == deviceType).ToList();
            objQReport.iTestPaperCount = objQReport.iPhoneUserCount + objQReport.iAndroidUserCount;

            return objQReport;
        }

        private ScoreCardHeader GetScoreCard(Int32 QuizId)
        {
            ScoreCardHeader objQReportHeader;
            objQReportHeader = (from q in objDB.Quizs
                                join sc in objDB.Categorys on q.SubjectId equals sc.Id
                                join cc in objDB.Categorys on sc.ParentCategoryId equals cc.Id
                                join ft in objDB.Keywords on q.FreeTypeId equals ft.KeywordValue
                                where q.QuizId == QuizId
                                    && ft.KeywordType == "FreeType"
                                group q by new {q.QuizId
                                    , q.QuizName
                                    , Class= cc.CategoryName
                                    , Subject = sc.CategoryName
                                    , ft.KeywordText
                                    , q.TotalScore
                                } into g
                                select new ScoreCardHeader
                                {
                                    QuizId = g.Key.QuizId,
                                    QuizName = g.Key.QuizName,
                                    PayType = g.Key.KeywordText,
                                    TotalScore = g.Key.TotalScore.ToString(),
                                    SubjectName = g.Key.Class + "/" + g.Key.Subject,
                                    QuestionCount = g.Count(),
                                }).FirstOrDefault();

            objQReportHeader.QuestionCount = objDB.QuizDetails.Count(x => x.QuizId == QuizId);
            objQReportHeader.ScoreCard = (from AS in objDB.AnswerSheets
                                   join UN in objDB.UserProfiles on AS.UserId equals UN.Id
                                   where AS.QuizId == QuizId
                                   group AS by new
                                   {
                                       UserId = UN.Id,
                                       UN.UserName,
                                       AS.StartDate,
                                       AS.EndDate,
                                       AS.OptainScore,
                                   } into g

                                   select new ScoreCardDetail
                                   {
                                       UserId = g.Key.UserId,
                                       UserName = g.Key.UserName,
                                       StartTime = g.Key.StartDate,
                                       EndTime = g.Key.EndDate,
                                       ObtainScore = g.Key.OptainScore.ToString(),
                                   }).ToList();

            return objQReportHeader;
        }

        #endregion
    }
}
