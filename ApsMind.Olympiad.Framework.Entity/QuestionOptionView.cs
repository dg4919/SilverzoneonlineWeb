using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{

    public class QuestionOptionView
    {
       

        public Int32 OptionId { get; set; }

        public Int32 QuestionId { get; set; }

       
        public String OptionText { get; set; }
        public String ImageUrl { get; set; }

        public Boolean IsAnswer { get; set; }

    }
}
