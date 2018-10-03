using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.DTO
{
    public class QuestionDTO : EntityDTO
    {
        public QuestionDTO()
        {
            Answers = new List<AnswerDTO>();
        }

        public int Id { get; set; }
        public int ExamID { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
}
