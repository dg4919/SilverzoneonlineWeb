using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class _CreateCustomeQuizRequest
    {
       
        public string CustomeQuizTitle { get; set; }
        public int SubjectId { get; set; }
        public int UserId { get; set; }

        public Int32 DificultyLevel { get; set; }
        public Int32 NoofQuestions { get; set; }

        public List<Int32> SelectedQuizIds { get; set; }
        public _CreateCustomeQuizRequest()
        {
            SelectedQuizIds = new List<int>();
        }
    }
   
}
