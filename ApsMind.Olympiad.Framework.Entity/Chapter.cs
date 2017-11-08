using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Chapter")]
    public class Chapter
    {
        [Key]
        public Int32 ChapterId { get; set; }

        [Display(Name = "Chapter Name")]
        [MaxLength(200, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String ChapterName { get; set; }

        [Display(Name = "Description")]
        [MaxLength(100)]
        public string Description { get; set; }
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual StudentClass Class { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public int PaperTypeId { get; set; }
        [ForeignKey("PaperTypeId")]
        public virtual PaperType PaperType { get; set; }
      [Display(Name = "IsActive")]
        public Boolean IsActive { get; set; }

        public Int32 CreatedBy { get; set; }


        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
