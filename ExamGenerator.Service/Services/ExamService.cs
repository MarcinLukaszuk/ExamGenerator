
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Services
{
    public class ExamService : Service<Exam>, IExamService
    {
        private readonly IDbContext _context;
        private IAnswerService _answerService;
        private IQuestionService _questionService;
        public ExamService(IDbContext dbContext, IAnswerService answerService, IQuestionService questionService) : base(dbContext)
        {
            _context = dbContext;
            _answerService = answerService;
            _questionService = questionService;
        }

        public void AddQuestionsToExam(int examID, List<Question> questions)
        {
            foreach (var question in questions)
                AddQuestionToExam(examID, question);
        }

        public void AddQuestionsToExam(Exam exam, List<Question> questions)
        {
            foreach (var question in questions)
                AddQuestionToExam(exam, question);
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

        public new void Update(Exam editedExam)
        {
            var questionsToRemove = getQuestionsToRemove(editedExam).ToList();
            var questionsToEdit = getQuestionsToEdit(editedExam).ToList();
            var questionsToAdd = getQuestionsToAdd(editedExam).ToList();
            _context.Questions.RemoveRange(questionsToRemove);

            foreach (var questionToAdd in questionsToAdd)
            {
                AddQuestionToExam(editedExam, questionToAdd);
                foreach (var answerToAdd in questionToAdd.Answers)
                {
                    _questionService.AddAnswerToQuestion(questionToAdd, answerToAdd);
                }
            }

            foreach (var questionToEdit in questionsToEdit)
            {
              

                var answersToRemove = getAnswersToRemove(questionToEdit).ToList();
                var answersToEdit = getAnswersToEdit(questionToEdit).ToList();
                var answersToAdd = getAnswersToAdd(questionToEdit).ToList();
                _context.Answer.RemoveRange(answersToRemove);

                foreach (var answerToAdd in answersToAdd)
                {
                    _questionService.AddAnswerToQuestion(questionToEdit, answerToAdd);
                }

                foreach (var answerToEdit in answersToEdit)
                {
                    _answerService.Update(answerToEdit);
                }
                _questionService.Update(questionToEdit);
            }
            _context.Exams.AddOrUpdate(editedExam);
            _context.SaveChanges();
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

        private IEnumerable<Question> getQuestionsToRemove(Exam exam)
        {
            foreach (var item in Find(exam.Id).Questions)
            {
                var tmp = exam.Questions.FirstOrDefault(x => x.Id == item.Id);
                if (tmp == null)
                {
                    yield return item;
                }
            }
        }
        private IEnumerable<Question> getQuestionsToEdit(Exam exam)
        {
            foreach (var item in Find(exam.Id).Questions)
            {
                var tmp = exam.Questions.FirstOrDefault(x => x.Id == item.Id);
                if (tmp != null)
                {
                    yield return item;
                }
            }

            foreach (var item in exam.Questions)
            {
                var tmp = Find(exam.Id).Questions.FirstOrDefault(x => x.Id == item.Id);
                if (tmp != null)
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<Question> getQuestionsToAdd(Exam exam)
        {
            return exam.Questions.Where(x => x.Id == 0).ToList();
        }

        private IEnumerable<Answer> getAnswersToRemove(Question question)
        {
            foreach (var item in _context.Questions.Find(question.Id).Answers)
            {
                var tmp = question.Answers.FirstOrDefault(x => x.Id == item.Id);
                if (tmp == null)
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<Answer> getAnswersToEdit(Question question)
        {
            foreach (var item in _context.Questions.Find(question.Id).Answers)
            {
                var tmp = question.Answers.FirstOrDefault(x => x.Id == item.Id);
                if (tmp != null)
                {
                    yield return item;
                }
            }

            foreach (var item in question.Answers)
            {
                var tmp = _context.Questions.Find(question.Id).Answers.FirstOrDefault(x => x.Id == item.Id);
                if (tmp != null)
                {
                    yield return item;
                }
            }

        }

        private IEnumerable<Answer> getAnswersToAdd(Question question)
        {
            return question.Answers.Where(x => x.Id == 0).ToList();
        }

    }
}
