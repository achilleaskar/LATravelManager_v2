using LATravelManager.DataAccess;
using LATravelManager.Model;
using LATravelManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public class GenericRepository : IGenericRepository,IDisposable
    {
        protected readonly MainDatabase Context;

        public GenericRepository()
        {
            this.Context = new MainDatabase();
        }

        public async Task<IEnumerable<Booking>> GetAllBookingInPeriod(DateTime minDay, DateTime maxDay, int excursionId)
        {
            return await Context.Bookings.Where(c => c.Excursion.Id == excursionId && c.CheckIn <= maxDay && c.CheckOut > minDay)
                .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .ToListAsync(); ;
        }
        public void Add<TEntity>(TEntity model) where TEntity : BaseModel
        {
            Context.Set<TEntity>().Add(model);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            return await Context.Set<TEntity>().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<TEntity> GetByNameAsync<TEntity>(string name) where TEntity : BaseModel, INamed
        {
            return await Context.Set<TEntity>().Where(x => x.Name == name).FirstOrDefaultAsync();
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

        public virtual void Delete<TEntity>(TEntity entity)
           where TEntity : BaseModel
        {
            var dbSet = Context.Set<TEntity>();
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public void RemoveById<TEntity>(int id) where TEntity : BaseModel
        {
            TEntity entity = Context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public virtual void UpdateValues<TEntity>(TEntity entity, TEntity newEntity)
           where TEntity : BaseModel
        {
            entity.ModifiedDate = DateTime.Now;
            Context.Entry(entity).CurrentValues.SetValues(newEntity);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}