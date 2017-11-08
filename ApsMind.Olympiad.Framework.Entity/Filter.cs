using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Filter")]
    public class Filter
    {
        [Key]
        public int Id { get; set; }

        public String FilterName { get; set; }

        public Boolean IsActive { get; set; }

        public Boolean FilterType { get; set; }
    }
}
