using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel
{
    public class ExamGeneratorDBContext : DbContext
    { 
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
