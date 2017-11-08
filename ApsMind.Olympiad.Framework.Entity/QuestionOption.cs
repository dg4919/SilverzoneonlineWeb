using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("QuestionOption")]
    public class QuestionOption
    {
        [Key]

        public Int32 OptionId { get; set; }

        public Int32? QuestionId { get; set; }

        [MaxLength(4000, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String OptionText { get; set; }

        [MaxLength(1000, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String ImageUrl { get; set; }

        public Boolean IsAnswer { get; set; }

    }
}
