using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApsMind.Olympiad.Framework.Entity;

namespace ApsMind.Olympiad.Portal.Models
{

    public class _PaymentHistory
    {
        public int OrderId { get; set; }
       //User detail
        public Int32 UserId { get; set; }
        public String Name { get; set; }
        public String EmailId { get; set; }

        //purchase histroy
        
        public Int32 OrderSqId { get; set; }

        public String Currency { get; set; }

        public Double Amount { get; set; }

        public String PRN_TrackingId { get; set; }

        public String BID_BankRefNo { get; set; }

        public DateTime? TXNDATETIME { get; set; }

        public String TransactionStatus { get; set; }
        //class detail
        
        public Int32 CategoryId { get; set; }
        public String CategoryName { get; set; }
        public string subjectName { set; get; }
        public Int32 subjectId { set; get; }
        //quiz detail
       
        public Int32 QuizId { get; set; }
        public String QuizName { get; set; }



    }
}