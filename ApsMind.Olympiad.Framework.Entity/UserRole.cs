using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public Int32 RoleId { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String RoleName { get; set; }

        [Required]
        public Boolean IsActive { get; set; }

        [Required]
        public Int32 CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
       
    }
}
