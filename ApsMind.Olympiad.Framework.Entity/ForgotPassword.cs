using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("ForgotPWD")]
    public class ForgotPassword
    {
        [Key]
        public Int64 ForgotPWDID { get; set; }
        public Int64 UserID { get; set; }
        public String Password { get; set; }
        public String EmailId { get; set; }
    }
}
