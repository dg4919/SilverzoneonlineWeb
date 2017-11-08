using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApsMind.Olympiad.Framework.Entity
{
    [Table("Note")]
  public class Note
    {
      [Key]
      public int NotesId { get; set; }
      public int QuizId { get; set; }
      public int UserId { get; set; }
      public int ClassId { get; set; }
      public int QuestionId { get; set; }
      public int SubjectId { get; set; }
      public string Title { set; get; }
      public string Comment { get; set; }
      public int CreatedBy { get; set; }
      public DateTime CreatedOn { get; set; }
      public int? ModifiedBy { set; get; }
      public DateTime? ModifiedOn { get; set; }
      




    }
}
