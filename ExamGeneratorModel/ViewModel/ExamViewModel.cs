using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class ExamViewModel
    {
        public ExamViewModel()
        {
            Questions = new List<QuestionViewModel>();
        }
        public int Id { get; set; }
        [ DisplayName("Exam Name"), DataType(DataType.MultilineText)]  
        public string Name { get; set; }
        public virtual List<QuestionViewModel> Questions { get; set; }
    }
}
