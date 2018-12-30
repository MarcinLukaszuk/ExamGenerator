using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class StudentGroupViewModel
    {
        public StudentGroupViewModel()
        {
            Students = new List<Student>();
            ExamsCore = new List<ExamCoreStudentGroupViewModel>();
        }

        public StudentGroup StudentGroup { get; set; }
        public List<Student> Students { get; set; }
        public List<ExamCoreStudentGroupViewModel> ExamsCore { get; set; }
    }
}
