using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Token")]
    public class Token
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string TokenCode { get; set; }

        public int RoleId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpireOn { get; set; }
    }
}
