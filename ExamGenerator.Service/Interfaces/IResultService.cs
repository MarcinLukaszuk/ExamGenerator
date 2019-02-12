using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IResultService : IService<Result>
    {
        List<Result> GetResultsByStudentGroupAndExam(int? studentGroupID, int? examCoreID);
        List<Result> GetResultsByStudentGroupAndExam2(int? examCoreStudentGroupID);
        int? GetStudentIDByExamID(int? examID);
        void DeletePreviousResults(int? examID);
        void SetIsValidatetFlagByExamID(int? examID);

        int? GetExamCoreIDByExamID(int? examID);
        int? GetStudentGroupIDByExamID(int? examID);
    }
}
