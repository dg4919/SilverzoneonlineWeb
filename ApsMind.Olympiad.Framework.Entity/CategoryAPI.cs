using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{

    public class CategoryAPI
    {

        public Int32 CategoryId { get; set; }


        public String CategoryName { get; set; }

        public Int32 ParentCategoryId { get; set; }
        public string CategoryType { get; set; }

        public Int32 CategoryTypeId { get; set; }


        public Boolean IsActive { get; set; }


        public String CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

    }
}
