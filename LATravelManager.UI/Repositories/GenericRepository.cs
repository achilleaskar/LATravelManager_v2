using LATravelManager.DataAccess;
using LATravelManager.Model;
using LATravelManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public class GenericRepository : IGenericRepository, IDisposable
    {
        protected readonly MainDatabase Context;

        public GenericRepository()
        {
            this.Context = new MainDatabase();
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsInCityAsync(int cityId)
        {
            return await Task.Run(() => Context.Hotels.Where(x => x.City.Id == cityId).OrderBy(h => h.Name).AsNoTracking().ToListAsync());
        }



        public async Task<List<Hotel>> GetAllHotelsWithRoomsInCityAsync(DateTime minDay, DateTime maxDay,int cityId)
        {
            return await Task.Run(() => Context.Hotels.Where(x => x.City.Id == cityId)
                .Include(x => x.City)
                .Include(x => x.HotelCategory)
                .Include(x => x.Rooms.Select(r => r.DailyBookingInfo))
                .Include(x => x.Rooms.Select(r => r.RoomType))
                .Where(x => x.Rooms.Any(z => z.StartDate <= maxDay && z.EndDate > minDay))
                .ToListAsync());
        }
        public async Task<User> FindUserAsync(string userName)
        {
            return await Task.Run(async () => await Context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<Booking>> GetAllBookingInPeriod(DateTime minDay, DateTime maxDay, int excursionId)
        {
            return await Task.Run(async () => await Context.Bookings.Where(c => c.Excursion.Id == excursionId && c.CheckIn <= maxDay && c.CheckOut > minDay)
                .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .ToListAsync());
        }


        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await Task.Run(async () => await Context.Bookings
                .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .ToListAsync());
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsByCreationDate(DateTime afterThisDay, int excursionId)
        {
            return await Task.Run(async () => await Context.Reservations.Where(c => c.Booking.Excursion.Id == excursionId && c.CreatedDate >= afterThisDay)
                .Include(f => f.Booking)
                .Include(f => f.Booking.User)
                .Include(f => f.Booking.Partner)
                .Include(f => f.Room)
                .Include(f => f.Room.Hotel)
                .Include(f => f.Room.RoomType)
                .Include(f => f.CustomersList)
                .Include(f => f.Hotel)
                .Include(f => f.NoNameRoomType)
                .ToListAsync());
        }

        public void Add<TEntity>(TEntity model) where TEntity : BaseModel
        {
            Context.Set<TEntity>().Add(model);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            return await Task.Run(async () => await Context.Set<TEntity>().OrderBy(x => x.Name).ToListAsync());
        }

        public IEnumerable<TEntity> GetAllSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            return Context.Set<TEntity>().OrderBy(x => x.Name).ToList();
        }

        public async Task<TEntity> GetByNameAsync<TEntity>(string name) where TEntity : BaseModel, INamed
        {
            return await Task.Run(async () => await Context.Set<TEntity>().Where(x => x.Name == name).FirstOrDefaultAsync());
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : BaseModel
        {
            return await Task.Run(async () => await Context.Set<TEntity>().FindAsync(id));
        }

        public virtual TEntity GetById<TEntity>(int id) where TEntity : BaseModel
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual async Task<Booking> GetFullBookingByIdAsync<TEntity>(int id) where TEntity : BaseModel
        {
            return await Task.Run(async () => await Context.Bookings.Where(b => b.Id == id)
            .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.Payments.Select(p => p.User))
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : BaseModel
        {
            return await Task.Run(async () => await Context.Set<TEntity>().ToListAsync());
        }

        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }

        private object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var modifiedEntities = Context.ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Modified).ToList();
            var now = DateTime.UtcNow;

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop].ToString();
                    var currentValue = change.CurrentValues[prop].ToString();
                    if (originalValue != currentValue)
                    {
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = primaryKey.ToString(),
                            PropertyName = prop,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            DateChanged = now
                        };
                        ChangeLogs.Add(log);
                    }
                }
            }
            var objectStateEntry = ((IObjectContextAdapter)Context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
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
            var AddedEntities =Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                E.Property("CreatedDate").CurrentValue = DateTime.Now;
            });

            var EditedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property("ModifiedDate").CurrentValue = DateTime.Now;
            });

            await Task.Run(async () => await Context.SaveChangesAsync());
        }

        public virtual void UpdateValues<TEntity>(TEntity entity, TEntity newEntity)
           where TEntity : EditTracker
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