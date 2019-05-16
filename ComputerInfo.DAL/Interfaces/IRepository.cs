using ComputerInfo.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerInfo.DAL.Interfaces
{
    public interface IRepository<T> : IDisposable where T : BaseEntity
    {
        void Create(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllAsQueryable();
        void Update(T item);
        void Delete(int itemId);
    }
}
