
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Services
{
    public class AnswerPositionService : Service<AnswerPosition>, IAnswerPositionService
    {
        private readonly IDbContext _context;
        public AnswerPositionService(IDbContext dbContext) : base(dbContext)
        { 
            _context =  dbContext;
        }

        public List<AnswerPosition> GetAllAnswersPositionsByExamID(int examID)
        {
            return _context.AnswersPositions.Where(x => x.Answer.Question.ExamCore.Id == examID).ToList();
        }

        public void InsertRange(int examID, List<AnswerPosition> answerPositions)
        {
            var answersPositions = GetAllAnswersPositionsByExamID(examID);
            if (answersPositions.Any())
            {
                _context.AnswersPositions.RemoveRange(answersPositions);
                _context.SaveChanges();
            }
            _context.AnswersPositions.AddRange(answerPositions);
            _context.SaveChanges();
        }
    }
}
