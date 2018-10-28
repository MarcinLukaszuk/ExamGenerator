using ExamGenerator.Service.EF;
using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service
{

    public abstract class Service<TEntity> : IService<TEntity> where TEntity : Entity
    {
        private readonly IDataModelEF _dataModelEF;
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
    
        public Service(IDataModelEF dataModelEF)
        {
            _dataModelEF = dataModelEF;
            _context = _dataModelEF.GetContext();
            _dbSet = _context.Set<TEntity>();
        }

        public void Delete(int id)
        {
            var element = Find(id);
            if (element != null)
            {
                _dbSet.Remove(element);
                _context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public List<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public TEntity GetByID(int ID)
        {
            return Find(ID);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _dbSet.AddOrUpdate(entity);
            _context.SaveChanges();
        }
    }
}
