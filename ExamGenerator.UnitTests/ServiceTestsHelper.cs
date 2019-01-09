using ExamGenerator.Service.Services;
using ExamGeneratorModel;
using ExamGeneratorModel.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace ExamGenerator.UnitTests
{

    public abstract class MockableDbSetWithExtensions<T> : DbSet<T> where T : class
    {
        public abstract void AddOrUpdate(params T[] entities);
        public abstract void AddOrUpdate(Expression<Func<T, object>>
             identifierExpression, params T[] entities);
    }





    public class ServiceTestsHelper
    {

        public Mock<IDbContext> ctx { get; set; }
        public IDbContext fakectx { get; set; }
        public ExamCoreService examCoreService { get; set; }
        public QuestionService questionService { get; set; }
        public AnswerService answerService { get; set; }
        public StudentService studentService { get; set; }
        public StudentGroupService studentGroupService { get; set; }
        public ResultService resultService { get; set; }

        public ServiceTestsHelper()
        {
            ctx = new Mock<IDbContext>();
            fakectx = ctx.Object;
            Initialize();
        }

        private void Initialize()
        {
            var exams = new List<ExamCore>();
            var questions = new List<Question>();
            var answers = new List<Answer>();
            var students = new List<Student>();
            var studentGropus = new List<StudentGroup>();
            var results = new List<Result>();

            var mockDbSetExamCore = GetMockDbSet<ExamCore>(exams);
            var mockDbSetQuestion = GetMockDbSet<Question>(questions);
            var mockDbSetAnswer = GetMockDbSet<Answer>(answers);
            var mockDbSetStudent = GetMockDbSet<Student>(students);
            var mockDbSetStudentGroup = GetMockDbSet<StudentGroup>(studentGropus);
            var mockDbSetResult = GetMockDbSet<Result>(results);

            mockDbSetAnswer.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => answers.FirstOrDefault(d => d.Id == (int)ids[0]));
            mockDbSetQuestion.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => questions.FirstOrDefault(d => d.Id == (int)ids[0]));
            mockDbSetExamCore.Setup(m => m.Find(It.IsAny<object[]>()))
               .Returns<object[]>(ids => exams.FirstOrDefault(d => d.Id == (int)ids[0]));
            mockDbSetStudent.Setup(m => m.Find(It.IsAny<object[]>()))
              .Returns<object[]>(ids => students.FirstOrDefault(d => d.Id == (int)ids[0]));
            mockDbSetStudentGroup.Setup(m => m.Find(It.IsAny<object[]>()))
              .Returns<object[]>(ids => studentGropus.FirstOrDefault(d => d.Id == (int)ids[0]));
            mockDbSetResult.Setup(m => m.Find(It.IsAny<object[]>()))
              .Returns<object[]>(ids => results.FirstOrDefault(d => d.Id == (int)ids[0]));

            ctx.Setup(c => c.Set<ExamCore>()).Returns(mockDbSetExamCore.Object);
            ctx.Setup(c => c.Set<Question>()).Returns(mockDbSetQuestion.Object);
            ctx.Setup(c => c.Set<Answer>()).Returns(mockDbSetAnswer.Object);
            ctx.Setup(c => c.Set<Student>()).Returns(mockDbSetStudent.Object);
            ctx.Setup(c => c.Set<StudentGroup>()).Returns(mockDbSetStudentGroup.Object);
            ctx.Setup(c => c.Set<Result>()).Returns(mockDbSetResult.Object);

            ctx.Setup(c => c.Exams).Returns(mockDbSetExamCore.Object);
            ctx.Setup(c => c.Questions).Returns(mockDbSetQuestion.Object);
            ctx.Setup(c => c.Answer).Returns(mockDbSetAnswer.Object);
            ctx.Setup(c => c.Students).Returns(mockDbSetStudent.Object);
            ctx.Setup(c => c.StudentGroups).Returns(mockDbSetStudentGroup.Object);
            ctx.Setup(c => c.Results).Returns(mockDbSetResult.Object);



            answerService = new AnswerService(fakectx);
            questionService = new QuestionService(fakectx);
            examCoreService = new ExamCoreService(fakectx, answerService, questionService);
            studentService = new StudentService(fakectx);
            studentGroupService = new StudentGroupService(fakectx);
            resultService = new ResultService(fakectx, studentGroupService);
        }


        internal Mock<MockableDbSetWithExtensions<T>> GetMockDbSet<T>(ICollection<T> entities) where T : class
        {
            var entitiesAsQueryable = entities.AsQueryable();
            var mockSet = new Mock<MockableDbSetWithExtensions<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entitiesAsQueryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entitiesAsQueryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entitiesAsQueryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => entitiesAsQueryable.GetEnumerator());
            
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>((entit) => entities.Add(entit));
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>((entit) => entities.Remove(entit));
            return mockSet;
        }
    }
}
