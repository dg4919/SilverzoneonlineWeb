using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter User Name")]
        [MaxLength(50, ErrorMessage = "The {0} must be maximum {2} characters long")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string UserImage { get; set; }

        [Required(ErrorMessage = "Enter  Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter Email Id")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Enter Mobile Number")]
        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }
        public GenderType Gender { get; set; }
        public DateTime DOB { get; set; }
        public int ClassId { get; set; }
        public int CountryId { get; set; }
        
        public int StateId { get; set; }
        
        public string City { get; set; }
        
        [DataType(DataType.PostalCode)]
        public string PinCode { get; set; }

        public string Browser { get; set; }
        public string OperatingSystem { get; set; }
        public string IPAddress { get; set; }

        public int DeviceTypeId { get; set; }
        public string GCMID { get; set; }
        public int DeviceId { get; set; }
        public string IMEINumber { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public string FireBaseToken { get; set; }
        public string CurrentVersion { get; set; }
        public string GooglePlayVersion { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdationDate { get; set; }
    }
}
