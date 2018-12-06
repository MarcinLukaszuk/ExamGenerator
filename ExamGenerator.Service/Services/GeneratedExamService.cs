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
    public class GeneratedExamService : Service<GeneratedExam>, IGeneratedExamService
    {
        private readonly IDbContext _context;
        public GeneratedExamService(IDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public void AssociateQuestionsToGeneratedExam(GeneratedExam generatedExam, List<Question> questions)
        {
            if (generatedExam?.Id != null && questions?.Any() != null)
            {
                foreach (var question in questions)
                {
                    var returnedGeneratedExamQuestion = _context.GeneratedExamQuestions.Where(x => x.QuestionID == question.Id && x.GeneratedExamID == generatedExam.Id).FirstOrDefault();
                    if (returnedGeneratedExamQuestion == null)
                    {
                        var generatedExamQuestion = new GeneratedExamQuestion()
                        {
                            QuestionID = question.Id,
                            GeneratedExamID = generatedExam.Id
                        };
                        _context.GeneratedExamQuestions.Add(generatedExamQuestion);
                        _context.SaveChanges();
                    }
                }
            }
        }

        public void DisAssociateQuestionsFromGeneratedExam(GeneratedExam generatedExam, List<Question> questions)
        {
            if (generatedExam?.Id != null && questions?.Any() != null)
            {
                foreach (var question in questions)
                {
                    var returnedGeneratedExamQuestion = _context.GeneratedExamQuestions.Where(x => x.QuestionID == question.Id && x.GeneratedExamID == generatedExam.Id).FirstOrDefault();
                    if (returnedGeneratedExamQuestion != null)
                        _context.GeneratedExamQuestions.Remove(returnedGeneratedExamQuestion);
                    _context.SaveChanges();
                }
            }
        }

        public List<Question> GetQuestionsByGeneratedExamID(int generatedExamID)
        {
            var questionsID = _context.GeneratedExamQuestions.Where(x => x.GeneratedExamID == generatedExamID).Select(x => x.QuestionID).ToList();
            var questions = new List<Question>();
            foreach (var questionID in questionsID)


                questions.Add(_context.Questions.Find(questionID));
            return questions;
        }

        public Student GetStudentByGeneratedExamID(int? studentGroupStudentID)
        {
            var studentGroupStudent = _context.StudentGroupStudents.Where(x => x.Id == studentGroupStudentID).FirstOrDefault();
            return _context.Students.Find(studentGroupStudent.StudentID);
        }
    }
}
