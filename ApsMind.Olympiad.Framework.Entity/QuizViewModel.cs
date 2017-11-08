using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class QuizViewModel
    {
        public int QuizId { get; set; }
        public int PaperTypeId { get; set; }
        public int SubjectId { get; set; }
        public int TotalTimeInSecond { get; set; }
        public double TotalScore { get; set; }
        public int FreeTypeId { get; set; }
        public Decimal? Price { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsPublish { get; set; }
        public bool IsLive { get; set; }
        public bool IsOn { get; set; }
    }
    public class paper_existViewModel
    {
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int PaperTypeId { get; set; }
    }

    public class paper_SearchViewModel
    {
        public int? classId { get; set; }
        public int? subjectId { get; set; }
        public ICollection<long> PaperTypeId { get; set; }
    }

    public class PackageViewModel
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageImage { get; set; }
        public string Description { get; set; }
        public int PaperTypeId { get; set; }
        public decimal Price { get; set; }
        public decimal BundlePrice { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<int> QuizesId { get; set; }

        public static Package Parse(PackageViewModel model)
        {
            return new Package()
            {
                PackageName = model.PackageName,
                PackageImage=model.PackageImage,
                Description = model.Description,
                PaperTypeId=model.PaperTypeId ,
                Price = model.Price,
                BundlePrice = model.BundlePrice,
                ClassId = model.ClassId,
                IsActive=true,
            };
        }
         public static PackageViewModel Parse(Package model)
             {
                 return new PackageViewModel()
                 {
                     PackageId=model.PackageId,
                     PackageName=model.PackageName,
                     PackageImage=model.PackageImage,
                     Description =model.Description ,
                     PaperTypeId=model.PaperTypeId,
                     Price=model.Price ,
                     BundlePrice =model.BundlePrice,
                     ClassId =model.ClassId,
                     SubjectId =model.SubjectId,
                     IsActive=true
                 };
            }

        public static Package parse(PackageViewModel model,Package myBundle)
         {
             myBundle.PackageName = model.PackageName;
             myBundle.PackageImage = model.PackageImage;
             myBundle.Description = model.Description;
             myBundle.Price = model.Price;
             myBundle.BundlePrice = model.BundlePrice;
             myBundle.ClassId = model.ClassId;
             myBundle.SubjectId = model.SubjectId;
             myBundle.IsActive = true;
             return myBundle;
         }

    }

   
}
