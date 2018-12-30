using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class QuestionViewModel
    {
        public QuestionViewModel()
        {
            Answers = new List<AnswerViewModel>();
        }
        public int Id { get; set; }
        public int ExamCoreID { get; set; }
        [DataType(DataType.MultilineText)]
        public string QuestionText { get; set; }
        public virtual List<AnswerViewModel> Answers { get; set; }
    }
}
