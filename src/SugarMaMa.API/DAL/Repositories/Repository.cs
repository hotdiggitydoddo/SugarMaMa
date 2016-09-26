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

        public async Task<T> AddAsync(T t)
        {
            _db.Set<T>().Add(t);
            await _db.SaveChangesAsync();
            return t;
        }

        public async Task<IEnumerable<T>> GetAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();

            foreach (var property in includes)
                query = query.Include(property);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();

            foreach (var property in includes)
                query.Include(property);

            return await query.Where(where).ToListAsync();
        }

        public async Task<T> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();

            foreach (var property in includes)
                query.Include(property);

            return await query.SingleOrDefaultAsync(x => Equals(x.Id, id));
        }
    }
}
