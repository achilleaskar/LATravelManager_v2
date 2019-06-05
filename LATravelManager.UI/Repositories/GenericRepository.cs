using LATravelManager.DataAccess;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
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
            Context = new MainDatabase();
            //Context.Database.Log = Console.Write;
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
                Task<T> t = Task.Run(task);
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

        internal void DeletePayments(int bookingId)
        {
            List<Payment> toDelete = Context.Payments.Where(p => p.Booking.Id == bookingId).ToList();
            Context.Payments.RemoveRange(toDelete);
        }

        public bool IsContextAvailable { get; set; }

        public async Task<List<Hotel>> GetAllHotelsInCityAsync(int cityId)
        {
            return await RunTask(Context.Hotels.Where(x => x.City.Id == cityId).OrderBy(h => h.Name).ToListAsync);
        }

        public async Task<List<Room>> GetAllRoomsInCityAsync(DateTime minDay, DateTime maxDay, int cityId)
        {
            return await RunTask(Context.Rooms.Where(x => x.Hotel.City.Id == cityId
            && x.DailyBookingInfo.Any(d => d.Date <= maxDay) && x.DailyBookingInfo.Any(d => d.Date >= minDay))
                .Include(x => x.Hotel.HotelCategory)
                .Include(x => x.DailyBookingInfo)
                .Include(x => x.RoomType)
                .ToListAsync);
        }
        public async Task<List<Option>> GetAllPendingOptions()
        {
            var limit = DateTime.Today.AddDays(2);
            return await RunTask(Context.Options.Where(ο => ο.Date <= limit)
                .Include(x => x.Room.Hotel)
                .ToListAsync);
        }

        //public async Task<User> FindUserAsync(string userName)
        //{
        //    return await RunTask(Context.Users.Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync);
        //}

        public async Task<IEnumerable<Booking>> GetAllBookingInPeriodNoTracking(DateTime minDay, DateTime maxDay, int excursionId)
        {
            return await RunTask(Context.Bookings.Where(c => c.Excursion.Id == excursionId && c.CheckIn <= maxDay && c.CheckOut > minDay)
                .Include(f => f.Partner)
                .Include(f => f.ExcursionDate)
                .Include(f => f.User)
                .Include(f => f.Excursion)
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
                .Include(f => f.Excursion)
                .Include(f => f.ExcursionDate)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .ToListAsync);
        }

        internal async Task LoadBasic()
        {
            await Context.RoomTypes.LoadAsync();
            await Context.Countries.LoadAsync();
            await Context.Hotels.LoadAsync();
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

        public async Task<IEnumerable<Excursion>> GetAllGroupExcursionsAsync(bool showFinished = false)
        {
            try
            {
                return await RunTask(Context.Set<Excursion>()
                .Where(c => c.ExcursionType.Category == Enums.ExcursionTypeEnum.Group)
                .Include(c => c.Destinations.Select(d => d.Country))
                .Include(c => c.ExcursionType)
                .Include(c => c.ExcursionDates)
                .ToListAsync);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<List<Hotel>> GetAllHotelsAsync<T>()
        {
            return await RunTask(Context.Hotels.OrderBy(h => h.Name)
                .Include(h => h.HotelCategory)
                .Include(h => h.City)
                .ToListAsync);
        }

        public async Task<IEnumerable<Excursion>> GetAllExcursionsAsync()
        {
            return await RunTask(Context.Set<Excursion>()
            .Include(c => c.Destinations.Select(d => d.Country))
            .Include(c => c.ExcursionType)
            .Include(c => c.ExcursionDates)
            .ToListAsync);
        }

        public async Task<Excursion> GetExcursionByIdAsync(int id, bool local = false)
        {
            if (!local)
            {
                return await RunTask(Context.Set<Excursion>().Where(c => c.Id == id)
                .Include(c => c.Destinations.Select(d => d.Country))
                .Include(c => c.ExcursionType)
                .Include(c => c.ExcursionDates)
                .FirstOrDefaultAsync);
            }
            else
            {
                return Context.Set<Excursion>().Local.Where(c => c.Id == id).FirstOrDefault();
            }
        }

        public IEnumerable<TEntity> GetAllSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            try
            {
                return Context.Set<TEntity>().OrderBy(x => x.Name).ToList();
            }
            catch (Exception)
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

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int id, bool local = false) where TEntity : BaseModel
        {
            if (!local)
            {
                var x = await RunTask(() => Context.Set<TEntity>().FindAsync(id));
                return x;
            }
            else
            {
                return Context.Set<TEntity>().Local.Where(x => x.Id == id).FirstOrDefault();
            }
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
                .Include(f => f.Excursion)
                .Include(f => f.ExcursionDate)
                .Include(f => f.Payments.Select(p => p.User))
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room.RoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room.Hotel))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .FirstOrDefaultAsync);
        }

        public virtual async Task<Personal_Booking> GetFullPersonalBookingByIdAsync(int id)
        {
            return await RunTask(Context.Personal_Bookings.Where(b => b.Id == id)
                .Include(f => f.User)
                .Include(f => f.Customers)
                .Include(f => f.Customers.Select(s => s.Services))
                .Include(f => f.Partner)
                .Include(f => f.Services)
                .Include(f => f.Payments)
                .FirstOrDefaultAsync);
        }

        public List<TEntity> GetAllAsyncLocal<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : BaseModel
        {
            return Context.Set<TEntity>().Local.ToList();
        }

        public async Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : BaseModel
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

        internal async Task<IEnumerable<Payment>> GetAllPaymentsFiltered(int excursionId, int userId, DateTime dateLimit, bool enableDatesFilter, DateTime from, DateTime to)
        {
            return await RunTask(Context.Payments
               .Where(c =>
             (!enableDatesFilter && c.Date >= dateLimit) ||
               (enableDatesFilter && (c.Date >= from && c.Date <= to)) &&
               (excursionId == 0 || c.Booking.Excursion.Id == excursionId) &&
               (userId == 0 || c.User.Id == userId))
               .Include(f => f.User)
               .Include(f => f.Booking)
               .Include(f => f.Booking.ExcursionDate)
               .Include(f => f.Booking.Partner)
               .Include(f => f.Booking.Payments)
               .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList))
               .ToListAsync);
        }

        internal async Task<IEnumerable<Room>> GetAllRoomsFiltered(int cityId = -1, int hotelId = -1, int roomTypeId = -1)
        {
            return await RunTask(Context.Rooms
               .Where(r =>
               (cityId == -1 || r.Hotel.City.Id == cityId) &&
               (hotelId == -1 || r.Hotel.Id == hotelId) &&
               (roomTypeId == -1 || r.RoomType.Id == roomTypeId))
               .Include(f => f.Hotel)
               .Include(f => f.Hotel.HotelCategory)
               .Include(f => f.Hotel.City)
               .Include(f => f.DailyBookingInfo)
               .Include(f => f.RoomType)
               .ToListAsync);
        }

        public void RejectChanges()
        {
            foreach (DbEntityEntry entry in Context.ChangeTracker.Entries())
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
            List<DbEntityEntry> modifiedEntities = Context.ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Modified).ToList();
            DateTime now = DateTime.UtcNow;

            foreach (DbEntityEntry change in modifiedEntities)
            {
                string entityName = change.Entity.GetType().Name;
                object primaryKey = GetPrimaryKeyValue(change);

                foreach (string prop in change.OriginalValues.PropertyNames)
                {
                    string originalValue = change.OriginalValues[prop].ToString();
                    string currentValue = change.CurrentValues[prop].ToString();
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
            System.Data.Entity.Core.Objects.ObjectStateEntry objectStateEntry = ((IObjectContextAdapter)Context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }

        internal async Task<Room> GetRoomById(int roomId)
        {
            return await RunTask(Context.Rooms
                .Where(r => r.Id == roomId)
                .Include(f => f.Hotel)
                .Include(f => f.Hotel.HotelCategory)
                .Include(f => f.Hotel.City)
                .Include(f => f.DailyBookingInfo)
                .Include(f => f.RoomType)
                .FirstOrDefaultAsync);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public virtual void Delete<TEntity>(TEntity entity)
           where TEntity : BaseModel
        {
            DbSet<TEntity> dbSet = Context.Set<TEntity>();
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
            List<DbEntityEntry> AddedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                if (E.CurrentValues.PropertyNames.Contains("CreatedDate"))
                {
                    E.Property("CreatedDate").CurrentValue = DateTime.Now;
                }
            });

            List<DbEntityEntry> EditedEntities = Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                if (E.OriginalValues.PropertyNames.Contains("ModifiedDate"))
                {
                    //   E.Property("ModifiedDate").CurrentValue = DateTime.Now;
                }
            });

            IEnumerable<DbEntityEntry> changes = from e in Context.ChangeTracker.Entries()
                                                 where e.State != EntityState.Unchanged
                                                 select e;

            foreach (DbEntityEntry change in changes)
            {
                if (change.State == EntityState.Added)
                {
                    // Log Added
                }
                else if (change.State == EntityState.Modified)
                {
                    // Log Modified
                    object item = change.Entity;
                    DbPropertyValues originalValues = Context.Entry(item).OriginalValues;
                    DbPropertyValues currentValues = Context.Entry(item).CurrentValues;

                    foreach (string propertyName in originalValues.PropertyNames)
                    {
                        object original = originalValues[propertyName];
                        object current = currentValues[propertyName];

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
                  .Include(f => f.Booking.Excursion)
                  .Include(f => f.Booking.Payments)
                  .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList))
                  .Include(f => f.Room.Hotel)
                  .Include(f => f.Room.RoomType)
                  .Include(f => f.CustomersList)
                  .Include(f => f.Hotel)
                  .Include(f => f.NoNameRoomType)
                  .ToListAsync);
        }

        internal async Task<List<Personal_Booking>> GetAllPersonalBookingsFiltered(int userId, bool completed, DateTime dateLimit)
        {
            return await RunTask(Context.Personal_Bookings
                .Where(c =>
                c.CreatedDate >= dateLimit &&
                (userId == 0 || c.User.Id == userId) &&
                (completed || c.Services.Any(s => s.TimeReturn >= DateTime.Today)))
                .Include(f => f.User)
                .Include(f => f.Customers)
                .Include(f => f.Partner)
                .Include(f => f.Services)
                .Include(f => f.Payments)
                .ToListAsync);
        }
    }
}