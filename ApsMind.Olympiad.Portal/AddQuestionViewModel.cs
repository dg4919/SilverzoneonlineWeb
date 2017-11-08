using ApsMind.Olympiad.Framework.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApsMind.Olympiad.Portal
{
    public class AddQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string ImageUrl { get; set; }
        public int ParaId { get; set; }
        public int ChapterId { get; set; }
        public int TopicId { get; set; }
        public int QuestionLevel { get; set; }
        public int Subjectid { get; set; }
        public int CategoryId { get; set; }
        public double Mark { get; set; }
        public int EstimatedTimeInSecond { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ClassId { get; set; }
        public int PaperTypeId { get; set; }
        public int QuizId { get; set; }

        public IList<Chapter> ChapterList { get; set; }
        public IList<Para> ParagraphList { get; set; }
        public IList<Topic> TopicList { get; set; }
        public IList<Category> CategoryList { get; set; }
        public IList<Keyword> QuestionLevelList { get; set; }
     
        public IList<QuestionOption> Options { get; set; }

        public IList<QuizDetail> QDetails { get; set; }
    }
}