using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class StudentGroupEditExamsViewModel
    {
        public StudentGroupEditExamsViewModel()
        {
            Exams = new List<EditExamViewModel>();
        }

        public StudentGroup StudentGroup { get; set; }
        public List<EditExamViewModel> Exams { get; set; }
    }
}
