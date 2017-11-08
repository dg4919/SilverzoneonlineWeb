using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Keyword")]
    public class Keyword
    {


        [Key]
        public Int64 KeywordId { get; set; }

        public Int32 KeywordValue { get; set; }
         
        [Required]
        [MaxLength(400, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String KeywordText { get; set; }

        [Required]
        [MaxLength(400, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String KeywordType { get; set; }

        [Required]
        public Int32 CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; } 

    }
}
