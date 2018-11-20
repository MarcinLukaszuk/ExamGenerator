using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string TextAnswer { get; set; }
        public int QuestionID { get; set; }
        public bool IfCorrect { get; set; }
    }
}
