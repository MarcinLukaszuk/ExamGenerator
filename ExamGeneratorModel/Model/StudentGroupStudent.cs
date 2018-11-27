using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{
    public class StudentGroupStudent : Entity
    {
        public int Id { get; set; }
        [ForeignKey("Student")]
        public int StudentID { get; set; }
        [ForeignKey("StudentGroup")]
        public int StudentGroupID { get; set; }

        public virtual Student Student { get; set; }
        public virtual StudentGroup StudentGroup { get; set; }
    }
}
