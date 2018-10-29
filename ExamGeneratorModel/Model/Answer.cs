using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{
    public class Answer : Entity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string TextAnswer { get; set; }
        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        public bool IfCorrect { get; set; }
        public virtual Question Question { get; set; }
    }
}
