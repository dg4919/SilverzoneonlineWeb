using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    public class SerachAnsSheet
    {
        public int PerPage { get; set; }
        public String FilterBy { get; set; }
        public List<AnsSheetDetails> ListQuestionView { get; set; }
        public SerachAnsSheet()
        {
            ListQuestionView = new List<AnsSheetDetails>();
        }
    }
    public class AnsSheetDetails
    {
        public Int32 AnswerSheetId { get; set; }

        public String UserId { get; set; }

        public String QuizId { get; set; }

        public String FreeType { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Boolean? IsSubmitted { get; set; }

        public Double? OptainScore { get; set; }
        public List<AnswerDetail> AnsOption { get; set; }
        public long NumberInGroup { get; set; }
       
        public String ClassName { get; set; }

        
    }
}
