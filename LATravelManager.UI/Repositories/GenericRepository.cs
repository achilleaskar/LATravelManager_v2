using LATravelManager.DataAccess;
using LATravelManager.Model;
using LATravelManager.Model.Booking;
using LATravelManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public class GenericRepository : IGenericRepository, IDisposable
    {
        protected readonly MainDatabase Context;

        public GenericRepository()
        {
            this.Context = new MainDatabase();
            Context.Database.Log = Console.Write;
            IsContextAvailable = true;
        }

        public bool IsTaskOk => LastTask == null || LastTask.IsCompleted;

        public Task LastTask => lastTask;
        private volatile Task lastTask;

        private async Task<T> RunTask<T>(Func<Task<T>> task)
        {
            try
            {
                if (!IsContextAvailable)
                {
                    await lastTask;
                }
                IsContextAvailable = false;
                if (LastTask != null && !LastTask.IsCompleted)
                {
                    await lastTask;
                }
                var t = Task.Run(task);
                lastTask = t;
                return await t;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsContextAvailable = true;
            }
        }

        public bool IsContextAvailable { get; set; }

        public async Task<List<Hotel>> GetAllHotelsInCityAsync(int cityId)
        {
            return await RunTask(Context.Hotels.Where(x => x.City.Id == cityId).OrderBy(h => h.Name).AsNoTracking().ToListAsync);
        }

        public async Task<List<Hotel>> GetAllHotelsWithRoomsInCityAsync(DateTime minDay, DateTime maxDay, int cityId)
        {
            return await RunTask(Context.Hotels.Where(x => x.City.Id == cityId)
                .Include(x => x.City)
                .Include(x => x.HotelCategory)
                .Include(x => x.Rooms.Select(r => r.DailyBookingInfo))
                .Include(x => x.Rooms.Select(r => r.RoomType))
                .Where(x => x.Rooms.Any(z => z.StartDate <= maxDay && z.EndDate > minDay))
                .ToListAsync);
        }

        public async Task<User> FindUserAsync(string userName)
        {
            return await RunTask(Context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingInPeriod(DateTime minDay, DateTime maxDay, int excursionId)
        {
            return await RunTask(Context.Bookings.Where(c => c.Excursion.Id == excursionId && c.CheckIn <= maxDay && c.CheckOut > minDay)
                .Include(f => f.Partner)
                .Include(f => f.ExcursionDate)
                .Include(f => f.User)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .ToListAsync);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await RunTask(Context.Bookings
                .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.ExcursionDate)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .ToListAsync);
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsByCreationDate(DateTime afterThisDay, int excursionId)
        {
            return await RunTask(Context.Reservations.Where(c => c.Booking.Excursion.Id == excursionId && c.CreatedDate >= afterThisDay)
                .Include(f => f.Booking)
                .Include(f => f.Booking.User)
                .Include(f => f.Booking.ExcursionDate)
                .Include(f => f.Booking.Partner)
                .Include(f => f.Booking.Payments)
                .Include(f => f.Room)
                .Include(f => f.Room.Hotel)
                .Include(f => f.Room.RoomType)
                .Include(f => f.CustomersList)
                .Include(f => f.Hotel)
                .Include(f => f.NoNameRoomType)
                .ToListAsync);
        }

        public async Task<IEnumerable<Reservation>> GetAllGroupReservationsByCreationDate(DateTime afterThisDay)
        {
            return await RunTask(Context.Reservations
                .Where(c => c.Booking.Excursion.ExcursionType.Category == Enums.ExcursionTypeEnum.Group && c.CreatedDate >= afterThisDay)
                .Include(f => f.Booking)
                .Include(f => f.Booking.User)
                .Include(f => f.Booking.ExcursionDate)
                .Include(f => f.Booking.Partner)
                .Include(f => f.Booking.Payments)
                .Include(f => f.Room)
                .Include(f => f.Room.Hotel)
                .Include(f => f.Room.RoomType)
                .Include(f => f.CustomersList)
                .Include(f => f.Hotel)
                .Include(f => f.NoNameRoomType)
                .ToListAsync);
        }

        public void Add<TEntity>(TEntity model) where TEntity : BaseModel
        {
            Context.Set<TEntity>().Add(model);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            return await RunTask(Context.Set<TEntity>().OrderBy(x => x.Name).ToListAsync);
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsyncSortedByName()
        {
            return await RunTask(Context.Set<City>().Include(c => c.Country).OrderBy(x => x.Name).ToListAsync);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsyncSortedByUserName()
        {
            return await RunTask(Context.Set<User>().OrderBy(x => x.UserName).ToListAsync);
        }

        public async Task<IEnumerable<Excursion>> GetAllUpcomingGroupExcursionsAsync()
        {
            return await RunTask(Context.Set<Excursion>().Where(c => c.ExcursionType.Category == Enums.ExcursionTypeEnum.Group && c.ExcursionDates.Any(d => d.CheckIn >= DateTime.Today))
            .Include(c => c.Destinations.Select(d => d.Country))
            .Include(c => c.ExcursionType)
            .Include(c => c.ExcursionDates)
            .ToListAsync);
        }

        public async Task<Excursion> GetExcursionByIdAsync(int id)
        {
            return await RunTask(Context.Set<Excursion>().Where(c => c.Id == id)
            .Include(c => c.Destinations.Select(d => d.Country))
            .Include(c => c.ExcursionType)
            .Include(c => c.ExcursionDates)
            .FirstOrDefaultAsync);
        }

        public IEnumerable<TEntity> GetAllSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            try
            {
                return Context.Set<TEntity>().OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
            }
        }

        public async Task<TEntity> GetByNameAsync<TEntity>(string name) where TEntity : BaseModel, INamed
        {
            return await RunTask(Context.Set<TEntity>().Where(x => x.Name == name).FirstOrDefaultAsync);
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : BaseModel
        {
            return await RunTask(() => Context.Set<TEntity>().FindAsync(id));
        }

        public virtual TEntity GetById<TEntity>(int id) where TEntity : BaseModel
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual async Task<Booking> GetFullBookingByIdAsync(int id)
        {
            return await RunTask(Context.Bookings.Where(b => b.Id == id)
            .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.ExcursionDate)
                .Include(f => f.Payments.Select(p => p.User))
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .FirstOrDefaultAsync);
        }

        public virtual async Task<Personal_Booking> GetFullPersonalBookingByIdAsync(int id)
        {
            return await RunTask(Context.Personal_Bookings.Where(b => b.Id == id)
            .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.Payments.Select(p => p.User))
                .FirstOrDefaultAsync);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : BaseModel
        {
            if (filter == null)
            {
                return await RunTask(Context.Set<TEntity>().ToListAsync);
            }
            else
            {
                return await RunTask(Context.Set<TEntity>().Where(filter).ToListAsync);
            }
        }

        public void RejectChanges()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
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
            var AddedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                if (E.CurrentValues.PropertyNames.Contains("CreatedDate"))
                {
                    E.Property("CreatedDate").CurrentValue = DateTime.Now;
                }
            });

            var EditedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                if (E.OriginalValues.PropertyNames.Contains("ModifiedDate"))
                {
                    //   E.Property("ModifiedDate").CurrentValue = DateTime.Now;
                }
            });

            var changes = from e in Context.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;

            foreach (var change in changes)
            {
                if (change.State == EntityState.Added)
                {
                    // Log Added
                }
                else if (change.State == EntityState.Modified)
                {
                    // Log Modified
                    var item = change.Entity;
                    var originalValues = Context.Entry(item).OriginalValues;
                    var currentValues = Context.Entry(item).CurrentValues;

                    foreach (string propertyName in originalValues.PropertyNames)
                    {
                        var original = originalValues[propertyName];
                        var current = currentValues[propertyName];

                        Console.WriteLine("Property {0} changed from {1} to {2}",
                     propertyName,
                     originalValues[propertyName],
                     currentValues[propertyName]);
                    }
                }
                else if (change.State == EntityState.Deleted)
                {
                    // log deleted
                }
            }
            await RunTask(Context.SaveChangesAsync);
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

        public async Task<IEnumerable<Booking>> GetAllBookinsFromCustomers(DateTime checkIn)
        {
            return await RunTask(Context.Bookings.Where(b => b.DifferentDates && b.ReservationsInBooking.Any(r => r.CustomersList.Any(c => c.CheckIn == checkIn)))
           .Include(f => f.Partner)
               .Include(f => f.User)
               .Include(f => f.ExcursionDate)
               .Include(f => f.Payments.Select(p => p.User))
               .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
               .Include(f => f.ReservationsInBooking.Select(i => i.Room))
               .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
               .Include(f => f.ReservationsInBooking.Select(i => i.Hotel)).ToListAsync);
        }

        internal async Task<List<Reservation>> GetAllReservationsFiltered(int excursionId, int userId, bool completed, int category, DateTime dateLimit)
        {
            return await RunTask(Context.Reservations
               .Where(c =>
               c.Booking.CreatedDate >= dateLimit &&
               (category >= 0 ? (int)c.Booking.Excursion.ExcursionType.Category == category : true) &&
               (excursionId == 0 || c.Booking.Excursion.Id == excursionId) &&
               (userId == 0 || c.Booking.User.Id == userId) &&
               (completed || c.Booking.CheckIn >= DateTime.Today))
               .Include(f => f.Booking.User)
               .Include(f => f.Booking.ExcursionDate)
               .Include(f => f.Booking.Partner)
              .Include(f => f.Booking.Payments)
               .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList))
               .Include(f => f.Room.Hotel)
               .Include(f => f.Room.RoomType)
               .Include(f => f.CustomersList)
               .Include(f => f.Hotel)
               .Include(f => f.NoNameRoomType)
               .ToListAsync);
        }
    }
}