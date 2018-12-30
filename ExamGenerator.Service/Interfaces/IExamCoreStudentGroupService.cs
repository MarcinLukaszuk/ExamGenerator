using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IExamCoreStudentGroupService : IService<ExamCoreStudentGroup>
    {
        bool AssociateExamToStudentGroup(ExamCore examCore, StudentGroup studentGroup);
        void DisassociateExamFromStudentGroup(ExamCore examCore, StudentGroup studentGroup);
        bool CheckIfExamCoreIsGenerated(int examCoreID, int studentGroupID);
        bool CheckIfExamCoreIsValidated(int examCoreID, int studentGroupID);
        string GetGenerategExamArchivePath(int examCoreID, int studentGroupID);
        void SetExamArchivePath(int examCoreID, int studentGroupID,string path);
    }
}
