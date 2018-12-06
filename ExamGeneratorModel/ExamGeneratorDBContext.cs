using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<ExamCore> Exams { get; set; }
        DbSet<Answer> Answer { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<AnswerPosition> AnswersPositions { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<StudentGroup> StudentGroups { get; set; }
        DbSet<StudentGroupStudent> StudentGroupStudents { get; set; }
        DbSet<ExamCoreStudentGroup> ExamCoreStudentGroups { get; set; }
        DbSet<GeneratedExam> GeneratedExams { get; set; }
        DbSet<GeneratedExamQuestion> GeneratedExamQuestions { get; set; }
        
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
    public class ExamGeneratorDBContext : DbContext, IDbContext
    {
        public ExamGeneratorDBContext() : base("Name=DefaultConnection") { }
        public virtual DbSet<ExamCore> Exams { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<AnswerPosition> AnswersPositions { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentGroup> StudentGroups { get; set; }
        public virtual DbSet<StudentGroupStudent> StudentGroupStudents { get; set; }
        public virtual DbSet<ExamCoreStudentGroup> ExamCoreStudentGroups { get; set; }
        public virtual DbSet<GeneratedExam> GeneratedExams { get; set; }
        public DbSet<GeneratedExamQuestion> GeneratedExamQuestions { get; set; }
    }
}
