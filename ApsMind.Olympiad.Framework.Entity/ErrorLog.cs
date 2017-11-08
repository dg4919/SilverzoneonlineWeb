using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{

    [Table("ErrorLog")]
    public class ErrorLog
    {

        [Key]
        public Int64 Id { get; set; }
        public String Error { get; set; }
        public int? LoginId { get; set; }
        public DateTime LogTime { get; set; }
        public LogTypes LogType { get; set; }
    }

}
