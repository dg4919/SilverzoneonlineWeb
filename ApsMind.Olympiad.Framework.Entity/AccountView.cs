using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{

    public class UserAccountView
    {
        public String UserName { get; set; }
        public String QuizName { get; set; }
        public Decimal? Price { get; set; }
        public Int32 DeviceTypeid { get; set; }
        public String EmailId { get; set; }
        public String ClassName { get; set; }
        public String SubjectName { get; set; }
        public DateTime? TransDate { get; set; }

    }

}
