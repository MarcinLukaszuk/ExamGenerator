using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{
   public class Result : Entity
    {
        [Key]
        public int Id { get; set; }      
        public int GeneratedExamID { get; set; }      
        public int StudentID { get; set; }
        public int Points { get; set; }
        public int MaxPoints { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }
        [ForeignKey("GeneratedExamID")]
        public virtual GeneratedExam GeneratedExam { get; set; }
    }
}
