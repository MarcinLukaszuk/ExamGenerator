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

        public new List<StudentGroup> GetAll()
        {
            return _context.StudentGroups.Where(x => x.IsDeleted == false).ToList();
        }

        public new void Insert(StudentGroup studentGroup)
        {
            studentGroup.IsDeleted = false;
            _context.StudentGroups.Add(studentGroup);
            _context.SaveChanges();
        }
        public new void Delete(int id)
        {
            var element = Find(id);
            if (element == null)
            {
                return;
            }
            this.Delete(element);
        }

        public new void Delete(StudentGroup studentGroup)
        {
            studentGroup.IsDeleted = true;
            _context.SaveChanges();
        }
        public List<ExamCore> GetExamsCoreByStudentGroup(int studentGroupID)
        {
            var associatedExamsCore = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroupID).ToList();
            List<ExamCore> examsCoreList = new List<ExamCore>();
            foreach (var examCoreAssociated in associatedExamsCore)
            {
                var exam = _context.Exams.Find(examCoreAssociated.ExamCoreID);
                if (exam.IsDeleted == false)
                {
                    examsCoreList.Add(exam);
                }
            }
            return examsCoreList.OrderBy(x => x.Name).Distinct().ToList();
        }

        public List<ExamCore> GetExamsCoreNotInStudentGroup(int studentGroupID)
        {
            var associatedExamsCore = _context.ExamCoreStudentGroups.Where(x => x.StudentGroupID == studentGroupID).ToList();
            List<ExamCore> examsCoreList = _context.Exams.Where(x=>x.IsDeleted==false).ToList();
            foreach (var examCoreAssociated in associatedExamsCore)
            {
                var exam = _context.Exams.Find(examCoreAssociated.ExamCoreID);             
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
            return _context.StudentGroupStudents.Where(x => x.StudentGroupID == studentGroupID).Select(x => x.Id).ToList();
        }
    }
}
