using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class StudentGroupEditStudentsViewModel
    {
        public StudentGroupEditStudentsViewModel()
        {
            Students = new List<EditStudentViewModel>();
        }

        public StudentGroup StudentGroup { get; set; }
        public List<EditStudentViewModel> Students { get; set; } 
    }
}
