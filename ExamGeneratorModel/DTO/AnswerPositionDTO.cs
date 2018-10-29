using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.DTO
{
   public class AnswerPositionDTO
    {
        public int Id { get; set; }
        public int AnswerID { get; set; }
        public int PageNumber { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public virtual AnswerDTO AnswerDTO { get; set; }
    }
}
