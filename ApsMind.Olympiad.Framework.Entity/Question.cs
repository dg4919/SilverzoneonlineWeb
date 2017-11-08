using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Question")]
    public class Question
    {
        [Key]
        public Int32 QuestionId { get; set; }

        
        [Display(Name = "Question Text")]
        [MaxLength(4000, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String QuestionText { get; set; }

        [MaxLength(1000, ErrorMessage = "The {0} must be maximum {2} characters long")]
        [Display(Name = "Select Image")]
        public String ImageUrl { get; set; }

        [Display(Name = "Select para Graph")]
        public Int32? ParaId { get; set; }

        [Display(Name = "Select Chapter")]
        public Int32? ChapterId { get; set; }

        [Display(Name = "Select Topic")]
        public Int32? TopicId { get; set; }

         [Required(ErrorMessage = "Select Question Level")]
        [Display(Name = "Select Question Level")]
        public Int32 QuestionLevel { get; set; }

        [Display(Name = "Select Subject")]
        public Int32 CategoryId { get; set; }
        
        
        [Display(Name = "Mark")]
        public double Mark { get; set; }
       
       
        [Display(Name = "Estimated Time In Second")]
        public Int32 EstimatedTimeInSecond { get; set; }

         [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }


        public Int32 CreatedBy { get; set; }


        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
