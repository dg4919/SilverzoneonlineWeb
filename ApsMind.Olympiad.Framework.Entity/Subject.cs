﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Subject")]
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Subject Name")]
        [MaxLength(200, ErrorMessage = "The {0} must be maximum {2} characters long")]
        public string SubjectName { get; set; }

        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual StudentClass Class { get; set; }
        
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
       
    }
}
