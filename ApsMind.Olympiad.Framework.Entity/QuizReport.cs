using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class QDetail
    {
        public Int32 QuizId { get; set; }
        public String QuizName { get; set; }
        public Int32 DeviceTypeId { get; set; }
        public Int32 UserCount { get; set; }
    }

    public class QReport
    {
        public QReport()
        {
            QDetailList = new List<QDetail>();
        }

        public int iTestPaperCount { get; set; }
        public int iPhoneUserCount { get; set; }
        public int iAndroidUserCount { get; set; }

        public List<QDetail> QDetailList { get; set; }

    }

    public class ScoreCardDetail
    {
        public int UserId { get; set; }
        public String UserName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public String ObtainScore { get; set; }
    }

    public class ScoreCardHeader
    {
        public int QuizId { get; set; }
        public String QuizName { get; set; }
        public String PayType { get; set; }
        public String TotalScore { get; set; }
        public String SubjectName { get; set; }
        public int QuestionCount { get; set; }

        public List<ScoreCardDetail> ScoreCard { get; set; }
    }

}
