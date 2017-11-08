using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace ApsMind.Olympiad.Portal.Models
{
    public class NotesSummary
    {
        public string title { set; get; }
        public string comment {set;get;}
        public int quizid { set; get; }
        public int userId { set; get; }


    }
}