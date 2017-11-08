using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("QuizePaperMapping")]
    public class QuizPaperMapping
    {
        [Key]
        
        public Int32 Id { get; set; }

        
        [Display(Name = "Test Paper")]
        [Required(ErrorMessage = "Select Test Paper Name")]
        [Range(1, int.MaxValue, ErrorMessage = "Select Test Paper Name")]
        public Int32 QuizId { get; set; }
        
        [Required(ErrorMessage = "Select User")]
        [Range(1, int.MaxValue, ErrorMessage = "Select User")]
        [Display(Name = "User Name")]
       
        public Int32 UserId { get; set; }

        public Int32 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
