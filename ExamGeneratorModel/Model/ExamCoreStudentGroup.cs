using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{
   public class ExamCoreStudentGroup : Entity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StudentGroup")]
        public int StudentGroupID { get; set; }
        [ForeignKey("ExamCore")]
        public int ExamCoreID { get; set; }
        public bool? IsGenerated { get; set; }
        public bool? IsValidated { get; set; }
        public string ZIPArchiveName { get; set; }
        public int Version { get; set; }

        public virtual StudentGroup StudentGroup { get; set; }
        public virtual ExamCore ExamCore { get; set; }
    }
}
