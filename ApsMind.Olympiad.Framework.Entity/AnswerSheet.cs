using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("AnswerSheet")]
    public class  AnswerSheet
    {
        [Key]
        public Int32 AnswerSheetId { get; set; }

        public Int32? UserId { get; set; }

        public Int32? QuizId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Boolean? IsSubmitted { get; set; }

        public Double? OptainScore { get; set; }

    }
}
