using AutoFixture;
using AutoFixture.Xunit2;
using ExamGenerator.Service.Services;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExamGenerator.UnitTests
{
    public class ExamCoreServiceTest
    {
        [Fact]
        public void Insert_ExamCore()
        {
            ServiceTestsHelper helper = new ServiceTestsHelper();
            ExamCoreService service = helper.examCoreService;

            var exam = new ExamCore()
            {
                Id = 1,
                Name = "name"
            };
            var exam2 = new ExamCore()
            {
                Id = 2,
                Name = "name"
            };
            var exam3 = new ExamCore()
            {
                Id = 3,
                Name = "name"
            };

            service.Insert(exam);
            service.Insert(exam2);
            service.Insert(exam3);

            var tmp = service.GetAll().ToList();
            Assert.Equal(tmp.Count, 3);
        }

        [Fact]
        public void Update_AddQuestionToExam()
        {
            ServiceTestsHelper helper = new ServiceTestsHelper();
            ExamCoreService examCoreService = helper.examCoreService;
            QuestionService questionService = helper.questionService;

            var exam = new ExamCore()
            {
                Id = 1,
                Name = "name"
            };
            var question = new Question()
            {
                Id = 1,
                QuestionText = "Question1"
            };
            examCoreService.Insert(exam);

            examCoreService.AddQuestionToExam(exam, question);
            var addedQuestion = questionService.GetAll().Where(x => x.ExamCoreID == exam.Id).FirstOrDefault();
            Assert.NotNull(addedQuestion);
            Assert.Equal(addedQuestion.QuestionText, "Question1");
        }

        [Fact]
        public void Update_AddQuestionsToExam()
        {
            ServiceTestsHelper helper = new ServiceTestsHelper();
            ExamCoreService examCoreService = helper.examCoreService;
            QuestionService questionService = helper.questionService;

            var exam = new ExamCore()
            {
                Id = 1,
                Name = "name"
            };
            var question = new Question()
            {
                Id = 1,
                QuestionText = "Question1"
            };
            var question2 = new Question()
            {
                Id = 2,
                QuestionText = "Question2"
            };
            var question3 = new Question()
            {
                Id = 3,
                QuestionText = "Question3"
            };
            examCoreService.Insert(exam);

            examCoreService.AddQuestionsToExam(exam, new List<Question>() { question, question2, question3 });

            var addedQuestion = questionService.GetAll().Where(x => x.ExamCoreID == exam.Id).ToList();
            Assert.Equal(addedQuestion.Count, 3);
        }
        [Fact]
        public void Update_GetAllQuestionOfExam()
        {
            ServiceTestsHelper helper = new ServiceTestsHelper();
            ExamCoreService examCoreService = helper.examCoreService;
            QuestionService questionService = helper.questionService;

            var exam = new ExamCore()
            {
                Id = 1,
                Name = "name"
            };
            var question = new Question()
            {
                Id = 1,
                QuestionText = "Question1"
            };
            var question2 = new Question()
            {
                Id = 2,
                QuestionText = "Question2"
            };
            var question3 = new Question()
            {
                Id = 3,
                QuestionText = "Question3"
            };
            examCoreService.Insert(exam);

            examCoreService.AddQuestionsToExam(exam, new List<Question>() { question, question2, question3 });
            var addedQuestion = examCoreService.GetAllQuestionOfExam(exam);
            Assert.Equal(addedQuestion.Count, 3);
        }
    }
}
