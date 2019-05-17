using ComputerInfo.DAL.EF;
using ComputerInfo.DAL.Interfaces;
using ComputerInfo.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace ComputerInfo.DAL.Repositories.Base
{
    public class BaseRepository<T> : IRepository<T>, IDisposable where T : BaseEntity
    {
        protected PCInfoContext _context;
        protected DbSet<T> DbSet { get; }

        protected bool isDisposed = false;

        public BaseRepository(PCInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = _context.Set<T>();
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.AsEnumerable();
        }
        public IQueryable<T> GetAllAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int AddOrUpdate(T item)
        {
            DbSet.AddOrUpdate(item);
            _context.SaveChanges();
            return item.Id;
        }

        public void Delete(int itemId)
        {
            var item = DbSet.Find(itemId);
            DbSet.Remove(item);
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

        public T GetSingle(Expression<Func<T, bool>> expression)
        {
            return DbSet.SingleOrDefault(expression);
        }
    }
}
