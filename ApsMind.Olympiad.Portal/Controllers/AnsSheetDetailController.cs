using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;

namespace ApsMind.Olympiad.Portal.Controllers
{
    [CustomAuthorize]
    public class AnsSheetDetailController : Controller
    {
        DatabaseContext objDB = new DatabaseContext();
        //
        // GET: /AnsSheetDetail/
        public ActionResult Index()
        {
            SerachAnsSheet ObjSeachQuestion = new SerachAnsSheet();

            ObjSeachQuestion.ListQuestionView = GetAnsSheetList();
            ViewBag.QuestionLevelQuestion = new SelectList(objDB.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList(), "KeywordValue", "KeywordText");
            return View(ObjSeachQuestion);
        }
        [HttpPost]
        public ActionResult Index(String name = null)
        {

            SerachAnsSheet ObjSeachQuestion = new SerachAnsSheet();
            ObjSeachQuestion.ListQuestionView = GetAnsSheetList();
          
            ViewBag.QuestionLevelQuestion = new SelectList(objDB.Keywords.Where(x => x.KeywordType == "QuestionLevel").ToList(), "KeywordValue", "KeywordText");
            return View(ObjSeachQuestion);
        }
        #region "User Method"
        private List<AnsSheetDetails> GetAnsSheetList()
        {
            var result = objDB.Questions.ToList();


            var objQusetionList = (from Anssheet in objDB.AnswerSheets
                                   join ansdetail in objDB.AnswerDetails on Anssheet.AnswerSheetId equals ansdetail.AnswerSheetId
                                   join UserName in objDB.UserProfiles on Anssheet.UserId equals UserName.Id
                                   join QuizName in objDB.Quizs on Anssheet.QuizId equals QuizName.QuizId
                                   group ansdetail by new

                                {
                                    Anssheet.AnswerSheetId,
                                    QuizName.QuizName,
                                    UserName.UserName,
                                    Anssheet.StartDate,
                                    Anssheet.EndDate,
                                    Anssheet.IsSubmitted,
                                    Anssheet.OptainScore
                                }

                                       into grouped

                                       select new AnsSheetDetails
                                       {
                                           NumberInGroup = grouped.Count(),
                                           AnsOption = grouped.ToList(),

                                           AnswerSheetId = grouped.Key.AnswerSheetId,
                                           StartDate = grouped.Key.StartDate,
                                           EndDate = grouped.Key.EndDate,
                                           IsSubmitted = grouped.Key.IsSubmitted,
                                           OptainScore = grouped.Key.OptainScore,
                                           UserId = grouped.Key.UserName,

                                           QuizId = grouped.Key.QuizName
                                       }

                                   //select new QuestionView


       ).ToList();
            
            var TotalQuestion = objQusetionList.Count();
            ViewBag.TotalQuestion = TotalQuestion;
            return objQusetionList;
        }
    }
        #endregion
}

