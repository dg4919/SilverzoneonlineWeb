using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("QuizPurchaseOrder")]
    public class QuizPurchaseOrder
    {
        [Key]
        [Required]
        public Int32 OrderId { get; set; }

        [Required]
        public Int32 OrderSqNo { get; set; }

        [Required]
        public Int32 ByUserId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The {0} must be maximum {1} characters long")]
        public String Currency { get; set; }

        [Required]
        public Double TotalAmount { get; set; }

        [MaxLength(100, ErrorMessage = "The {0} must be maximum {1} characters long")]
        public String PRN_TrackingId { get; set; }

        [MaxLength(100, ErrorMessage = "The {0} must be maximum {1} characters long")]
        public String BID_BankRefNo { get; set; }

        public DateTime? TXNDATETIME { get; set; }

        [MaxLength(10, ErrorMessage = "The {0} must be maximum {1} characters long")]
        public String TransactionStatus { get; set; }

        [Required]
        public Int32 CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; } 


    }
}
