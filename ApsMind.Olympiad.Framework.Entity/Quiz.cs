using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{

    [Table("Quiz")]
    public class Quiz
    {
        [Key]
        public Int32 QuizId { get; set; }

        [Required(ErrorMessage = "Enter  Test Paper Name")]
        [Display(Name = "Test Paper Name")]
        public String QuizName { get; set; }

        [Display(Name = "Select Paper")]
        public Int32 PaperTypeId { get; set; }

         public virtual PaperType PaperType { get; set; }

         [Display(Name = "Select Class")]
         public Int32 ClassId { get; set; }

        [ForeignKey("ClassId")]
         public virtual StudentClass StudentClass { get; set; }

        [Display(Name = "Select Subjet")]
        public Int32 SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
          
        [Display(Name = "Total Time In Second")]
        public Int32 TotalTimeInSecond { get; set; }

        [Display(Name = "Total Score")]
        public Double TotalScore { get; set; }

        [Display(Name = "Free Type")]
        public Int32 FreeTypeId { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
        [Display(Name = "Price")]
        public Decimal? Price { get; set; }

        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int32? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        
        [Display(Name = "Is Publish")]
        public Boolean IsPublish { get; set; }
      
    }
}
