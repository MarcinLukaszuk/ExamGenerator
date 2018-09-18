using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IExamService
    {
        void AddQuestionToExam(Exam exam, Question question);
        void AddQuestionToExam(int examID, Question question);
        List<Question> GetAllQuestionOfExam(Exam exam);
        List<Question> GetAllQuestionOfExam(int examID);
    }
}
