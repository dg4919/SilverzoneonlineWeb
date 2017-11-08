using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{

    public class ViewQuestion
    {
        public ViewQuestion()
        {
            Options = new List<QuestionOption>();
        }
        public int QuestionId { get; set; }
        public String QuestionText { get; set; }
        public String ImageUrl { get; set; }
        public double Mark { get; set; }
        public int? ParaId { get; set; }
        public String ParaText { get; set; }
        public String ParaImage { get; set; }
        public List<QuestionOption> Options { get; set; }

        public bool IsPublished { get; set; }
        public int QuizId { get; set; }
        public int CategoryId { get; set; }
        public String CategoryText { get; set; }
        
    }

    public class ViewQuizDetails
    {
        public ViewQuizDetails()
        {
            QuestionList = new List<ViewQuestion>();
        }
        public int QuizId { get; set; }
        public String QuizName { get; set; }
        public String CategoryName { get; set; }
        public String Instruction { get; set; }
        public List<ViewQuestion> QuestionList { get; set; }

    }


    public class _ViewAllQuestions
    {
        public Int32 QuizId { get; set; }
        public Boolean IsPublish { get; set; }
        public String QuizName { get; set; }
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        public List<_Paragraph> paragraph{get;set;}
        public _ViewAllQuestions()
        {
            paragraph = new List<_Paragraph>();
        }

    }
    public class _Paragraph
    {
        public string paratext{get;set;}
        
        public List<_Question> questionList{get;set;}
        public _Paragraph()
        {
            questionList = new List<_Question>();
        }
        
    }
    public class _Question
    {
        public Int32 QuestionId { get; set; }
        public String QuestionText { get; set; }
        public String QuestionImage { get; set; }
        public int QuestionLevelId { get; set; }

        public List<QuestionOption> Options { get; set; }
        public _Question()
        {
            Options = new List<QuestionOption>();
        }
    }
}


//public Int32 QuestionId { get; set; }
//        public Int32? QuizId { get; set; }
//        public String QuestionText { get; set; }
//        public List<QuestionOption> Options { get; set; }
//        public long NumberInGroup { get; set; }
//        public String QuestionLevel { get; set; }
//        public int QuestionLevelId { get; set; }
//        public int CategoryId { get; set; }
//        public String QuestionImage { get; set; }
//        public String CategoryName { get; set; }
//        public String ParaName { get; set; }
//        public List<Question> QuestionSearch { get; set; }
//        public Boolean IsPublish { get; set; }
//        public String QuizName { get; set; }
       