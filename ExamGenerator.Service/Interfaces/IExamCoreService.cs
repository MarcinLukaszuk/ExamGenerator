using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IExamCoreService: IService<ExamCore>
    {
        void AddQuestionToExam(ExamCore exam, Question question);
        void AddQuestionToExam(int examID, Question question);
        void AddQuestionsToExam(int examID, List<Question> questions);
        void AddQuestionsToExam(ExamCore exam, List<Question> questions);
        List<Question> GetAllQuestionOfExam(ExamCore exam);
        List<Question> GetAllQuestionOfExam(int examID);
    }
}
