using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.DTO
{
    public class ExamDTO: EntityDTO
    {
        public ExamDTO()
        {
            Questions = new List<QuestionDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}
