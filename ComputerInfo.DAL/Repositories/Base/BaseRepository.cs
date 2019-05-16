using ComputerInfo.DAL.EF;
using ComputerInfo.DAL.Interfaces;
using ComputerInfo.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerInfo.DAL.Repositories.Base
{
    public class BaseRepository<T> : IRepository<T>, IDisposable where T : BaseEntity
    {
        protected PCInfoContext _context;
        protected DbSet<T> _dbSet { get; }

        protected bool isDisposed = false;

        public BaseRepository(PCInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }
        public IQueryable<T> GetAllAsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public void Delete(int itemId)
        {
            var item = _dbSet.Find(itemId);
            _dbSet.Remove(item);
            _context.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
