
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
    public class ExamService : Service<Exam>, IExamService
    {
         private readonly IDbContext _context;
        public ExamService(IDbContext dbContext) : base(dbContext)
        { 
            _context =  dbContext;
        }
        public void AddQuestionToExam(int examID, Question question)
        {
            var exam = Find(examID);
            if (exam == null || question == null)
            {
                return;
            }
            AddQuestionToExam(exam, question);
        }
        public void AddQuestionToExam(Exam exam, Question question)
        {
            if (Find(exam?.Id) == null || question == null)
            {
                return;
            }
            question.ExamID = exam.Id;
            _context.Questions.Add(question);
            _context.SaveChanges();
        }
        public List<Question> GetAllQuestionOfExam(int examID)
        {
            var returnedExam = Find(examID);
            if (returnedExam == null)
            {
                return null;
            }
            return GetAllQuestionOfExam(returnedExam);
        }
        public List<Question> GetAllQuestionOfExam(Exam exam)
        {
            var returnedExam = Find(exam.Id);
            if (returnedExam == null)
            {
                return null;
            }
            return returnedExam.Questions.ToList();
        }
        public new void Delete(int id)
        {
            this.Delete(Find(id));
        }
        public new void Delete(Exam exam)
        {
            var returnedExam = Find(exam.Id);
            if (returnedExam == null)
            {
                return;
            }

            var questionsToRemove = _context.Questions.Where(x => x.ExamID == returnedExam.Id).ToList();
            var examToRemove = _context.Exams.Where(x => x.Id == returnedExam.Id).FirstOrDefault();
            _context.Questions.RemoveRange(questionsToRemove);
            _context.Exams.Remove(examToRemove);
            _context.SaveChanges();
        }
    }
}
