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
            students = new List<Student>();
        }

        public StudentGroup StudentGroup { get; set; }
        public List<Student> students { get; set; }
    }
}
