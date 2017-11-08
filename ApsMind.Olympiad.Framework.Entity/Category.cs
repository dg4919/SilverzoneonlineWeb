using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Category Name")]
        [MaxLength(400, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public string CategoryName { get; set; }

        public int? ParentCategoryId { get; set; }
        [Required(ErrorMessage = "Select Category Types")]
        [Range(1, int.MaxValue, ErrorMessage = "Select Category Type")]
        public int CategoryTypeId { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdationDate { get; set; }

    }
}
