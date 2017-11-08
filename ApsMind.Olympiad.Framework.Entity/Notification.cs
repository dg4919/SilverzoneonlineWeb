using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public Int64 NotificationId { get; set; }
        [Display(Name = "Select Class")]
        [Required(ErrorMessage = "Select Class")]
        public Int64 ClassId { get; set; }
        public String Body { get; set; }
        [Display(Name = "Message")]
        public String Message { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }
}
