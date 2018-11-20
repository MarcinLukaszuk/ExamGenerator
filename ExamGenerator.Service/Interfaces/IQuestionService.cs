using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IQuestionService : IService<Question>
    {
        void AddAnswerToQuestion(Question question, Answer answer);
        void AddAnswerToQuestion(int questionID, Answer answer);
    }
}
