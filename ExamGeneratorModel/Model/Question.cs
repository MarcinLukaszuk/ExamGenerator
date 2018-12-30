using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamGeneratorModel.Model
{
    public class Question : Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ExamCore")]
        public int ExamCoreID { get; set; } 
        
        public string QuestionText { get; set; }

        public virtual ExamCore ExamCore { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
