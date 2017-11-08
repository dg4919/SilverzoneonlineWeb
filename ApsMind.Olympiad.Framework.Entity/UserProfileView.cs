using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class UserProfileView
    {

        public int UserId { get; set; }
        public int CheckDeviceType { get; set; }
        public String Name { get; set; }
        public String UserName { get; set; }
        public String Role { get; set; }
        public string Password { get; set; }
        public String MobileNumber { get; set; }
        public String EmailId { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public int CountryId { get; set; }

        public int StateId { get; set; }
        public string City { get; set; }
        public int ClassId { get; set; }
        public String DeviceTypeId { get; set; }
        public Int64 DeviceId { get; set; }
        public String IMEINumber { get; set; }

    }
}
