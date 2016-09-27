using Microsoft.EntityFrameworkCore;
using SugarMaMa.API.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SugarMaMa.API.DAL.Repositories
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T: SMEntity<TKey>
    {
        private readonly SMDbContext _db;
        public Repository(SMDbContext context)
        {
            _db = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();

            foreach (var property in includes)
                query = query.Include(property);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();

            foreach (var property in includes)
                query.Include(property);

            return await query.Where(where).ToListAsync();
        }

        public async Task<T> GetByIdAsync(TKey key, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();

            foreach (var property in includes)
                query.Include(property);

            return await query.SingleOrDefaultAsync(x => Equals(x.Id, key));
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
                return false;

            var existing = await _db.Set<T>().SingleOrDefaultAsync((x => Equals(x.Id, entity.Id)));

            if (existing != null)
            {
               _db.Update(entity);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
                return;

            var existing = await _db.Set<T>().SingleOrDefaultAsync((x => Equals(x.Id, entity.Id)));

            if (existing != null)
            {
                _db.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}
