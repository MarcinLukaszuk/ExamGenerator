using ExamGeneratorModel.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{
    public class AnswerPosition:Entity
    {
        [Key]
        public int Id { get; set; }
        public int AnswerID { get; set; }

        public int PageNumber { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
   
        public virtual Answer Answer { get; set; }
    }
}
