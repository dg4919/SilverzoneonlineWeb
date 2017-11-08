using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
     [Table("TestPaper")]
    public class TestPaper
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter  Test Paper Name")]
        [Display(Name = "Test Paper Name")]
        public string TestPaperName { get; set; }

        [Display(Name = "Select Paper")]
        public int PaperTypeId { get; set; }

        public virtual PaperType PaperType { get; set; }

        [Display(Name = "Select Class")]
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual StudentClass StudentClass { get; set; }

        [Display(Name = "Select Subjet")]
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

        [Display(Name = "Total Time In Second")]
        public int TotalTimeInSecond { get; set; }

        [Display(Name = "Total Score")]
        public decimal TotalScore { get; set; }

        [Display(Name = "Free Type")]
        public Int32 FreeTypeId { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }

        [Display(Name = "Is Publish")]
        public bool IsPublished { get; set; }
    }
}
