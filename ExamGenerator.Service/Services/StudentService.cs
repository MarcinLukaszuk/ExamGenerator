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

    public class StudentService : Service<Student>, IStudentService
    {
        private readonly IDbContext _context;
        public StudentService(IDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public new List<Student> GetAll()
        {
            return _context.Students.Where(x => x.IsDeleted == false).ToList();
        }

        public new void Insert(Student student)
        {
            student.IsDeleted = false;
            _context.Students.Add(student);
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

        public new void Delete(Student student)
        {
            student.IsDeleted = true;
            _context.SaveChanges();
        }
        public Student GetStudentByEmail(string email)
        {
            return _context.Students.Where(x => x.Email == email).FirstOrDefault();
        }
    }
}
