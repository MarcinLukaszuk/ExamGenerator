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
    public class StudentGroupService : Service<StudentGroup>, IStudentGroupService
    {
        private readonly IDbContext _context;
        public StudentGroupService(IDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public List<Student> GetStudentByStudentGroup(int studentGroupID)
        {
            var associatedStudents = _context.StudentGroupStudents.Where(x => x.StudentGroupID == studentGroupID).ToList();
            List<Student> studentList = new List<Student>();
            foreach (var studentAssociated in associatedStudents)
            {
                studentList.Add(_context.Students.Find(studentAssociated.StudentID));
            }
            return studentList;
        }
    }
}
