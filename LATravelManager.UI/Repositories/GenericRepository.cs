using LATravelManager.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public class GenericRepository<TContext> : IGenericRepository
    where TContext : DbContext
    {
        protected readonly TContext Context;

        protected GenericRepository(TContext context)
        {
            this.Context = context;
        }
        public void Add<TEntity>(TEntity model) where TEntity : BaseModel
        {
            Context.Set<TEntity>().Add(model);
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : BaseModel
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : BaseModel
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void Remove<TEntity>(TEntity model) where TEntity : BaseModel
        {
            Context.Set<TEntity>().Remove(model);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
