using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.ViewModel
{
    public class ExamCoreStudentGroupViewModel
    {
        public int Id { get; set; }
        public ExamCore ExamCore { get; set; }
        public int Version { get; set; }
        public bool IsGenerated { get; set; }
        public bool IsValidated { get; set; }
        public string ZIPArchiveName { get; set; }
    }
}
