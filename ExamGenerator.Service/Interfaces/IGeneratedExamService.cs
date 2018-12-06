using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IGeneratedExamService : IService<GeneratedExam>
    {
        void AssociateQuestionsToGeneratedExam(GeneratedExam generatedExam, List<Question> questions);
        List<Question> GetQuestionsByGeneratedExamID(int generatedExamID);
        Student GetStudentByGeneratedExamID(int? studentGroupStudentID);
        void DisAssociateQuestionsFromGeneratedExam(GeneratedExam generatedExam, List<Question> questions);
    }
}
