using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class EditExamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Associate Exam Pools")]
        public bool IsAssociatedToStudentGroup { get; set; }
        public string Owner { get; set; }
        public bool IsDeleted { get; set; }
    }
}
