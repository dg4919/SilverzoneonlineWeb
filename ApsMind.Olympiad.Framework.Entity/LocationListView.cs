using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class LocationListView
    {
        public Int64 LocationId { get; set; }
        public String LocationName { get; set; }
        public String LocationTypeId { get; set; }
        public string ParentLocationId { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Boolean IsActive { get; set; }
    }
}
