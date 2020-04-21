using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IRepository
{
    public interface IBaseRepository
    {
    }
    public interface IBaseRepository<T> : IBaseRepository
        where T : class
    {
        IBaseRepository<T> AsQueryable();
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);

        IEnumerable<T> GetList(Expression<Func<T, bool>> filter);
        T Single(Func<T, bool> predicate);
        T SingleOrDefault(Func<T, bool> predicate);
        T First(Func<T, bool> predicate);
        T FirstOrDefault(Func<T, bool> predicate);
        void Create(T entity);

        void SaveList(IEnumerable<T> entityList);

        void Delete(T entity);
        void Attach(T entity);

        void Update(T entity);
    }
