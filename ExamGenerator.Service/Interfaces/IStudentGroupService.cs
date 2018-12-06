using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IStudentGroupService : IService<StudentGroup>
    {
        List<int> GetStudentsGroupStudentID(int? studentGroupID);
        List<Student> GetStudentsByStudentGroup(int? studentGroupID);
        List<Student> GetStudentNotInStudentGroup(int? studentGroupID);

        List<ExamCore> GetExamsCoreByStudentGroup(int studentGroupID);
        List<ExamCore> GetExamsCoreNotInStudentGroup(int studentGroupID);

    }
}
