using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IRepository<TKey, TEntity> where TEntity : Entity<TKey>
    {
        Task<TEntity?> AddAsync(TEntity input);
        Task<bool> DeleteAsync(TKey id);
        Task<TEntity?> GetAsync(TKey id);
        IQueryable<TEntity> GetQueryable();
        Task<List<TEntity>> ToListAsync();
        Task<TEntity?> UpdateAsync(TEntity input);
        Task<int> SaveChangeAsync();
    }
}
