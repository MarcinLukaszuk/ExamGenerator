using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.DTO
{
    public class ResultDTO
    {
        public int GeneratedExamID { get; set; }
        public int StudentID { get; set; }
        public int MaxPoints { get; set; }
        public int Points { get; set; }
    }
}
