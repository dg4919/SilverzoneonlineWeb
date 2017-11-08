using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Topic")]
    public class Topic
    {
        [Key]
        public Int32 TopicId { get; set; }


        [Display(Name = "Topic Name")]
        [MaxLength(200, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public String TopicName { get; set; }

        public int ChapterId { get; set; }

        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        public Int32 CreatedBy { get; set; }


        public DateTime CreatedOn { get; set; }

        public Int32? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
