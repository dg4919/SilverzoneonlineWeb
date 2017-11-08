using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class SeachQusetion
    {
        public int PerPage { get; set; }
        public String FilterBy { get; set; }
        public List<_ViewAllQuestions> ListQuizView { get; set; }
        public SeachQusetion()
        {
            ListQuizView = new List<_ViewAllQuestions>();
        }
    }
    public class QuestionView
    {
        public Int32 QuestionId { get; set; }
        public Int32? QuizId { get; set; }
        public String QuestionText { get; set; }
        public List<QuestionOption> Options { get; set; }
        public long NumberInGroup { get; set; }
        public String QuestionLevel { get; set; }
        public int QuestionLevelId { get; set; }
        public int CategoryId { get; set; }
        public String QuestionImage { get; set; }
        public String CategoryName { get; set; }
        public String ParaName { get; set; }
        public String ChapterName { get; set; }
        public String TopicName{get;set;}
        public List<Question> QuestionSearch { get; set; }
        public Boolean IsPublish { get; set; }
        public String QuizName { get; set; }

    }
}
