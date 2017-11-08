using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApsMind.Olympiad.Framework.Entity;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApsMind.Olympiad.Framework.Entity
{
     [Table("InfoZone")]
    public class InfoZone
    {
         public Int64 InfozoneId { get; set; }
         public String FileName { get; set; }
         public DateTime UpdateOn { get; set; }
    }
}
