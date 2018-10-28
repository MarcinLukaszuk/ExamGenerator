using ExamGenerator.Service.EF;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Services
{
    public class AnswerPositionService : Service<AnswerPosition>, IAnswerPositionService
    {
        private readonly IDataModelEF _dataModelEF;
        private readonly ExamGeneratorDBContext _context;
        public AnswerPositionService(IDataModelEF dataModelEF) : base(dataModelEF)
        {
            _dataModelEF = dataModelEF;
            _context = _dataModelEF.GetContext();
        }

        public List<AnswerPosition> GetAllAnswersPositionsByExamID(int examID)
        {
            return _context.AnswersPositions.Where(x => x.Answer.Question.Exam.Id == examID).ToList();
        }

        public void InsertRange(List<AnswerPosition> answerPositions)
        {
            _context.AnswersPositions.AddRange(answerPositions);
            _context.SaveChanges();
        }
    }
}
