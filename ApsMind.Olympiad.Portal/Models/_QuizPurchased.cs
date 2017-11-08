using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApsMind.Olympiad.Portal.Models
{
    public class _QuizPurchased
    {
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int OrderId { get; set; }
    }
}