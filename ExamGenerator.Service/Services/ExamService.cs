using ExamGenerator.Service.EF;
using ExamGenerator.Service.Interfaces;
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
        private readonly IDataModelEF _dataModelEF;

        public ExamService(IDataModelEF dataModelEF) : base(dataModelEF)
        {
            _dataModelEF = dataModelEF;
        }
        public void AddQuestionToExam(int examID, Question question)
        {
            using (var db = _dataModelEF.CreateNew())
            {
                var exam = Find(examID);
                if (exam == null)
                {
                    return;
                }
                AddQuestionToExam(exam, question);
            }
        }
        public void AddQuestionToExam(Exam exam, Question question)
        {
            using (var db = _dataModelEF.CreateNew())
            {
                if (Find(exam?.Id) == null)
                {
                    return;
                }
                question.ExamID = exam.Id;
                db.Questions.Add(question);
                db.SaveChanges();
            }
        }
        public List<Question> GetAllQuestionOfExam(int examID)
        {
            using (var db = _dataModelEF.CreateNew())
            {
                var returnedExam = Find(examID);
                if (returnedExam == null)
                {
                    return null;
                }
                return GetAllQuestionOfExam(returnedExam);
            }
        }
        public List<Question> GetAllQuestionOfExam(Exam exam)
        {
            using (var db = _dataModelEF.CreateNew())
            {
                var returnedExam = Find(exam.Id);
                if (returnedExam == null)
                {
                    return null;
                }
                return returnedExam.Questions.ToList();
            }
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
            using (var db = _dataModelEF.CreateNew())
            {
                var questionsToRemove = db.Questions.Where(x => x.ExamID == returnedExam.Id).ToList();
                var examToRemove = db.Exams.Where(x => x.Id == returnedExam.Id).FirstOrDefault();
                db.Questions.RemoveRange(questionsToRemove);
                db.Exams.Remove(examToRemove);
                db.SaveChanges();
            }
        }
    }
}
