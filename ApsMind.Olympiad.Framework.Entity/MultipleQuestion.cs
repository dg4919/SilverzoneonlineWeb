using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class MultipleQuestion
    {
        public Int32? QuestionId { get; set; }

        public Int32? OptionId { get; set; }

        public Int32? CorrectOptionId { get; set; }
    }
}
