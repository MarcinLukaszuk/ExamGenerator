using ExamGenerator.Service.Services;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ExamGenerator.UnitTests
{
    public class StudentServiceTest
    {
        [Fact]
        public void Insert_Student()
        {
            ServiceTestsHelper helper = new ServiceTestsHelper();
            StudentService studentService = helper.studentService;

            var student = new Student()
            {
                Id = 1,
                Name = "name"
            };

            studentService.Insert(student);
            var tmp = studentService.GetAll();
            Assert.Equal(tmp.Count, 1);
        }

        [Fact]
        public void Insert_GetStudentByEmail()
        {
            ServiceTestsHelper helper = new ServiceTestsHelper();
            StudentService studentService = helper.studentService;

            var student = new Student()
            {
                Id = 1,
                Name = "name",
                Email="tmp@tmp.tmp"
            };

            studentService.Insert(student);
            var tmp = studentService.GetStudentByEmail("tmp@tmp.tmp");
            Assert.NotNull(tmp);
            Assert.Equal(tmp.Email, "tmp@tmp.tmp");
        }
    }
}
