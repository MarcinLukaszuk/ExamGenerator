using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class EditStudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public bool IsAssociatedToStudentGroup { get; set; }
    }
}
