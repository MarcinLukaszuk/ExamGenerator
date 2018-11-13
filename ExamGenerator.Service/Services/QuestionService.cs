
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
    public class QuestionService : Service<Question>, IQuestionService
    {
        private readonly IDbContext _context;
        public QuestionService(IDbContext dbContext) : base(dbContext)
        { 
            _context = dbContext;
            DbContext = dbContext;
        }

        public IDbContext DbContext { get; }

        public void AddAnswerToQuestion(Question question, Answer answer)
        {
            if (Find(question.Id) == null)
            {
                return;
            }
            answer.QuestionID = question.Id;
           _context.Answer.Add(answer);
           _context.SaveChanges();

        }
        public void AddAnswerToQuestion(int questionID, Answer answer)
        {
            var question = Find(questionID);
            if (question == null)
            {
                return;
            }
            AddAnswerToQuestion(question, answer);
        }
        public new void Delete(int id)
        {
            this.Delete(Find(id));
        }
        public new void Delete(Question question)
        {
            var returnedQuestion = Find(question?.Id);
            if (returnedQuestion == null)
            {
                return;
            }
            var answersToRemove =_context.Answer.Where(x => x.QuestionID == returnedQuestion.Id).ToList();
            var questionToRemove =_context.Questions.Where(x => x.Id == returnedQuestion.Id).FirstOrDefault();
           _context.Answer.RemoveRange(answersToRemove);
           _context.Questions.Remove(questionToRemove);
           _context.SaveChanges();
        }
    }
}
