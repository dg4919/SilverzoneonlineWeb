using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class PackageDetails
    {
        [Key]
        public Int32 Id { get; set; }

        public int PackageId { get; set; }

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }
        
        public int TestPaperId { get; set; }

        [ForeignKey("TestPaperId")]
        public virtual Quiz Quiz { get; set; }
        
    }
}
