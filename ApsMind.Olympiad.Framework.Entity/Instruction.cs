using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Instruction")]
    public class Instruction
    {
        [Key]
        public Int32 InstructionId { get; set; }

       [Required(ErrorMessage = "Select Test Paper")]
        [Display(Name = "Test Paper")]
        public Int32 QuizId { get; set; }

         [Display(Name = "Instruction Text")]
        [MaxLength(4000, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String InstructionText { get; set; }

         [Display(Name = "Instruction Image")]
        [MaxLength(1000, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String ImageUrl { get; set; }

         [Display(Name = "Is Active")]
        [Required]
        public Boolean IsActive { get; set; }

       
        public Int32 CreatedBy { get; set; }

        
        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }


    }
}
