using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamGeneratorModel.Model
{
    public class Question : Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Exam")]
        public int ExamID { get; set; } 

        [StringLength(500)] 
        public string QuestionText { get; set; }
        public virtual Exam Exam { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
