using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Interfaces
{
    public interface IAnswerPositionService
    {
        void InsertRange(int examID,List<AnswerPosition> answerPositions);
        List<AnswerPosition> GetAllAnswersPositionsByExamID(int examID);
    }
}
