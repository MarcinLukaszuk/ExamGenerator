using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.DTO
{
    public class ExamDTO : EntityDTO
    {
        public ExamDTO()
        {
            QuestionsDTO = new List<QuestionDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string StudentFullName { get; set; }

        public List<QuestionDTO> QuestionsDTO { get; set; }
    }
}
