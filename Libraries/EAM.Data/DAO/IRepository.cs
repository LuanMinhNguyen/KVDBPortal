using System;
using System.Linq;
using System.Linq.Expressions;

namespace EAM.Data.DAO
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T GetByID(int Id);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
    }
}
