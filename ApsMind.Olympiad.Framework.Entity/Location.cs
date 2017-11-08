using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Location")]
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int LocationTypeId { get; set; }
        public int? ParentLocationId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public bool IsActive { get; set; } 
    }
}
