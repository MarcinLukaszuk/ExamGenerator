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
    public class StudentGroupStudentService : Service<StudentGroupStudent>, IStudentGroupStudentService
    {
        private readonly IDbContext _context;
        public StudentGroupStudentService(IDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public bool AssociateStudentToStudentGroup(Student student, StudentGroup studentGroup)
        {
            if (student != null && studentGroup != null)
            {
                var existingAssociate = _context.StudentGroupStudents.Where(x => x.StudentGroupID == studentGroup.Id && x.StudentID == student.Id).FirstOrDefault();
                if (existingAssociate == null)
                {
                    StudentGroupStudent sgs = new StudentGroupStudent() { StudentID = student.Id, StudentGroupID = studentGroup.Id };
                    Insert(sgs);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        public void DisassociateStudentFromStudentGroup(Student student, StudentGroup studentGroup)
        {
            if (student != null && studentGroup != null)
            {
                var existingAssociate = _context.StudentGroupStudents.Where(x => x.StudentGroupID == studentGroup.Id && x.StudentID == student.Id).FirstOrDefault();
                if (existingAssociate != null)
                {
                    Delete(existingAssociate);
                }
            }
        }
    }
}
