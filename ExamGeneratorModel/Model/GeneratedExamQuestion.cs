using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{
    public class GeneratedExamQuestion
    {
        [Key]
        public int Id { get; set; }
        public int QuestionID { get; set; }
        public int GeneratedExamID { get; set; }


        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
        [ForeignKey("GeneratedExamID")]
        public virtual GeneratedExam GeneratedExam { get; set; }
    }
}
