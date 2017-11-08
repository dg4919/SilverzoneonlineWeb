using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Para")]
    public class Para
    {
        [Key]

        public Int32 ParaId { get; set; }

        [Display(Name = "Para Graph")]
        [MaxLength(4000, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String ParaText { get; set; }

        [Display(Name = "Para Graph Image")]
        [MaxLength(1000, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String ImageUrl { get; set; }

         [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        [Required]
        public Int32 CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

    }
}
