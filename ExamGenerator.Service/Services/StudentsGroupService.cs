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

        public List<ExamCore> GetExamsCoreByStudentGroup(int studentGroupID)
        {
            var associatedExamsCore = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroupID).ToList();
            List<ExamCore> examsCoreList = new List<ExamCore>();
            foreach (var examCoreAssociated in associatedExamsCore)
            {
                examsCoreList.Add(_context.Exams.Find(examCoreAssociated.ExamCoreID));
            }
            return examsCoreList.OrderBy(x => x.Name).ToList();
        }

        public List<ExamCore> GetExamsCoreNotInStudentGroup(int studentGroupID)
        {
            var associatedExamsCore = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroupID).ToList();
            List<ExamCore> examsCoreList = _context.Exams.ToList();
            foreach (var examCoreAssociated in associatedExamsCore)
            {
                examsCoreList.Remove(_context.Exams.Find(examCoreAssociated.ExamCoreID));
            }
            return examsCoreList.OrderBy(x => x.Name).ToList();
        }

        public List<Student> GetStudentsByStudentGroup(int? studentGroupID)
        {
            var associatedStudents = _context.StudentGroupStudents.Where(x => x.StudentGroupID == studentGroupID).ToList();
            List<Student> studentList = new List<Student>();
            foreach (var studentAssociated in associatedStudents)
            {
                studentList.Add(_context.Students.Find(studentAssociated.StudentID));
            }
            return studentList.OrderBy(x => x.Email).ToList();
        }

        public List<Student> GetStudentNotInStudentGroup(int? studentGroupID)
        {
            var associatedStudents = _context.StudentGroupStudents.Where(x => x.StudentGroupID == studentGroupID).ToList();
            List<Student> studentList = _context.Students.ToList();
            foreach (var studentAssociated in associatedStudents)
            {
                studentList.Remove(_context.Students.Find(studentAssociated.StudentID));
            }
            return studentList.OrderBy(x => x.Email).ToList();
        }

        public List<int> GetStudentsGroupStudentID(int? studentGroupID)
        {
            return _context.StudentGroupStudents.Where(x => x.StudentGroupID == studentGroupID).Select(x=>x.Id).ToList();
        }
    }
}
