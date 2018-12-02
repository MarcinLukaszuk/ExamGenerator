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
    public class UnitTest1
    {
        [Theory, AutoData]
        public void Test1(string aaa)
        {
            string username = "testUser1";
            var ctx = new Mock<IDbContext>();

            var exams = new List<ExamCore>();
            var questions = new List<Question>();
            var mockDbSet1 = ServiceTestsHelper.GetMockDbSet<ExamCore>(exams);
            var mockDbSet2 = ServiceTestsHelper.GetMockDbSet<Question>(questions);
            mockDbSet1.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => exams.FirstOrDefault(d => d.Id == (int)ids[0]));

            mockDbSet2.Setup(m => m.Find(It.IsAny<object[]>()))
              .Returns<object[]>(ids => questions.FirstOrDefault(d => d.Id == (int)ids[0]));
            
            ctx.Setup(c => c.Set<ExamCore>()).Returns(mockDbSet1.Object);
            ctx.Setup(c => c.Set<Question>()).Returns(mockDbSet2.Object);

            ctx.Setup(c => c.Exams).Returns(mockDbSet1.Object);
            ctx.Setup(c => c.Questions).Returns(mockDbSet2.Object);

            var fakectx = ctx.Object;
           // ExamService service = new ExamService(fakectx);
           // QuestionService qservice = new QuestionService(fakectx);
               var examTmp = new ExamCore()
            {
                Id = 1,
                Name = username
            };

            var question = new Question()
            {Id =1,
                QuestionText = "JakiesPytanie"
            };

           // service.Insert(examTmp);
          
         ///   service.AddQuestionToExam(1, question);



          //   var tmp = service.GetAll().FirstOrDefault();
          //  var tmp2 = qservice.GetAll().FirstOrDefault();
        }
    }
    static class ServiceTestsHelper
    {
        internal static Mock<DbSet<T>> GetMockDbSet<T>(ICollection<T> entities) where T : class
        {
            var entitiesAsQueryable = entities.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entitiesAsQueryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entitiesAsQueryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entitiesAsQueryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => entitiesAsQueryable.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(entities.Add);
            //mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>((entit) => entities.Remove(entit));
          
            return mockSet;
        }
    }


}
