using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{
    public class GeneratedExam : Entity
    {
        [Key]
        public int Id { get; set; }       
        public int StudentGroupStudentID { get; set; }
        public int ExamCoreID { get; set; }
        public int ExamCoreStudentGroupID { get; set; }

        [ForeignKey("ExamCoreStudentGroupID")]
        public virtual ExamCoreStudentGroup ExamCoreStudentGroup { get; set; }
        [ForeignKey("StudentGroupStudentID")]
        public virtual StudentGroupStudent StudentGroupStudent { get; set; }
        [ForeignKey("ExamCoreID")]
        public virtual ExamCore ExamCore { get; set; }
    }
}
