using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Package")]
    public class Package
    {
        [Key]
        public Int32 PackageId { get; set; }

        [Display(Name = "Package Name")]
        [MaxLength(200, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public string PackageName { get; set; }
        public string PackageImage { get; set; }
        public int PaperTypeId { get; set; }

        [ForeignKey("PaperTypeId")]
        public virtual PaperType PaperType { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal BundlePrice { get; set; }


        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual StudentClass StudentClass { get; set; }

        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
              
        public Int32 CreatedBy { get; set; }


        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public byte[] RowVersion { get; set; }

        [Display(Name = "IsActive")]
        public Boolean IsActive { get; set; }

        public virtual ICollection<PackageDetails> packageDetails { get; set; }

    }
}
