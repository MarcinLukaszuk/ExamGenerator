using ExamGenerator.Service.Services;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ExamGenerator.UnitTests
{
   public  class StudentsGroupService
    {
        [Fact]
        public void Insert_ExamCore()
        {
            ServiceTestsHelper helper = new ServiceTestsHelper();
            StudentGroupService studentGroupService = helper.studentGroupService;

            var studentGroup = new StudentGroup()
            {
                Id = 1,
                Name = "StudentGroup"
            };
            var studentGroup2 = new StudentGroup()
            {
                Id = 1,
                Name = "StudentGroup2"
            };

            studentGroupService.Insert(studentGroup);
            studentGroupService.Insert(studentGroup2);


            var tmp = studentGroupService.GetAll();
            Assert.Equal(tmp.Count, 1);
        }
    }
}
