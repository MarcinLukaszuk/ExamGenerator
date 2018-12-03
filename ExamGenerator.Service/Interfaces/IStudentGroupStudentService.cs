using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IStudentGroupStudentService : IService<StudentGroupStudent>
    {
        bool AssociateStudentToStudentGroup(Student student, StudentGroup studentGroup);
        void DisassociateStudentFromStudentGroup(Student student, StudentGroup studentGroup);

    }
}
