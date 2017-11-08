using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("LocationType")]
    public class LocationType
    {
        [Key]
        public int LocationTypeId { get; set; }
        public string LocationTypeName { get; set; }
        public bool IsActive { get; set; } 
    }
}
