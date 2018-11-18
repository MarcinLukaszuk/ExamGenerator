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
        DbSet<Exam> Exams { get; set; }
        DbSet<Answer> Answer { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<AnswerPosition> AnswersPositions { get; set; }
        Task<int> SaveChangesAsync();
        void SaveChanges();
    }
    public class ExamGeneratorDBContext : DbContext, IDbContext
    {
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<AnswerPosition> AnswersPositions { get; set; }

         void IDbContext.SaveChanges(){}
    }
}
