using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("FeedBack")]
    public class FeedBack
    {
        [Key]
        public Int32 FeedBackId { get; set; }
        public Int64 UserID { get; set; }
        public String UserName { get; set; }
        public String EmailId { get; set; }
        public String City { get; set; }
        public Int32 Class { get; set; }
        public Int32 Subject { get; set; }
        public String Message { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
