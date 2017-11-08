using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("AnswerDetail")]
    public class AnswerDetail
    {
        [Key]
        public Int32 Id { get; set; }

        public Int32 AnswerSheetId { get; set; }

        public Int32 QuestionId { get; set; }

        public Int32 OptionId { get; set; }

        public Int32? CorrectOptionId { get; set; }





    }
}
