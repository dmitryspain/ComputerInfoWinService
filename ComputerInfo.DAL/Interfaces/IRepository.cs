using ComputerInfo.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ComputerInfo.DAL.Interfaces
{
    public interface IRepository<T> : IDisposable where T : BaseEntity
    {
        int AddOrUpdate(T item);
        T GetById(int id);
        T GetSingle(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllAsQueryable();
        void Update(T item);
        void Delete(int itemId);
    }
}
