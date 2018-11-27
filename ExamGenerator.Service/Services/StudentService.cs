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

        public Student GetStudentByEmail(string email)
        {
            return _context.Students.Where(x => x.Email == email).FirstOrDefault();
        }
    }
}
