using ExamGeneratorModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.Service
{
    public interface IService<TEntity> where TEntity : Entity
    {
        List<TEntity> GetAll();
        TEntity Find(params object[] keyValues);
        TEntity GetByID(int ID);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entity);   
    }
}
