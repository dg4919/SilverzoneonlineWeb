using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Cart")]
    public class Cart
    {
        [Key,Column(Order=0)]
        [Required]
        public Int32 UserId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public Int32 QuizId { get; set; }

        [Required]
        public Int32 OrderSqNo { get; set; }

        [Required]
        public Double Price { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Int32 CreatedBy { get; set; } 

    }
}
