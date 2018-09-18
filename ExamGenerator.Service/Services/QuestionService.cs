using ExamGenerator.Service.EF;
using ExamGenerator.Service.Interfaces;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service.Services
{
    public class QuestionService : Service<Question>, IQuestionService
    {
        private readonly IDataModelEF _dataModelEF;
        public QuestionService(IDataModelEF dataModelEF) : base(dataModelEF)
        {
            _dataModelEF = dataModelEF;
        }

        public void AddAnswerToQuestion(Question question, Answer answer)
        {
            using (var db = _dataModelEF.CreateNew())
            {
                if (Find(question.Id) == null)
                {
                    return;
                }
                answer.QuestionID = question.Id;
                db.Answer.Add(answer);
                db.SaveChanges();
            }
        }
        public void AddAnswerToQuestion(int questionID, Answer answer)
        {
            var question = Find(questionID);
            if (question == null)
            {
                return;
            }
            AddAnswerToQuestion(question,answer);
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
            using (var db = _dataModelEF.CreateNew())
            {
                var answersToRemove = db.Answer.Where(x => x.QuestionID == returnedQuestion.Id).ToList();
                var questionToRemove = db.Questions.Where(x => x.Id == returnedQuestion.Id).FirstOrDefault();
                db.Answer.RemoveRange(answersToRemove);
                db.Questions.Remove(questionToRemove);
                db.SaveChanges();
            }
        }
    }
}
