using SugarMaMa.API.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SugarMaMa.API.DAL.Repositories
{
    public interface IRepository<T, TKey> where T: SMEntity<TKey>
    {
        Task<IEnumerable<T>> GetAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T t);
    }
}
