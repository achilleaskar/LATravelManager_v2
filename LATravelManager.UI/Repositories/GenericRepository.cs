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
            tasks = new List<Task>();
            
        }

        List<Task> tasks;

        public bool IsContextAvailable { get; set; }

        public async Task<IEnumerable<Hotel>> GetAllHotelsInCityAsync(int cityId)
        {
            IsContextAvailable = false;
            IsContextAvailable = true;
            Task<List<Hotel>> result =  Task.Run(() => Context.Hotels.Where(x => x.City.Id == cityId).OrderBy(h => h.Name).AsNoTracking().ToListAsync());
            tasks.Add(result);
            await Task.WhenAll(tasks);
            return await result;
        }

        public async Task<List<Hotel>> GetAllHotelsWithRoomsInCityAsync(DateTime minDay, DateTime maxDay, int cityId)
        {
            IsContextAvailable = false;
            var result = await Task.Run(() => Context.Hotels.Where(x => x.City.Id == cityId)
                .Include(x => x.City)
                .Include(x => x.HotelCategory)
                .Include(x => x.Rooms.Select(r => r.DailyBookingInfo))
                .Include(x => x.Rooms.Select(r => r.RoomType))
                .Where(x => x.Rooms.Any(z => z.StartDate <= maxDay && z.EndDate > minDay))
                .ToListAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<User> FindUserAsync(string userName)
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingInPeriod(DateTime minDay, DateTime maxDay, int excursionId)
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Bookings.Where(c => c.Excursion.Id == excursionId && c.CheckIn <= maxDay && c.CheckOut > minDay)
                .Include(f => f.Partner)
                .Include(f => f.ExcursionDate)
                .Include(f => f.User)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .ToListAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Bookings
                .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.ExcursionDate)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .ToListAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsByCreationDate(DateTime afterThisDay, int excursionId)
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Reservations.Where(c => c.Booking.Excursion.Id == excursionId && c.CreatedDate >= afterThisDay)
                .Include(f => f.Booking)
                .Include(f => f.Booking.User)
                .Include(f => f.Booking.ExcursionDate)
                .Include(f => f.Booking.Partner)
                .Include(f => f.Room)
                .Include(f => f.Room.Hotel)
                .Include(f => f.Room.RoomType)
                .Include(f => f.CustomersList)
                .Include(f => f.Hotel)
                .Include(f => f.NoNameRoomType)
                .ToListAsync());
            IsContextAvailable = true;
            return result;
        }

        public void Add<TEntity>(TEntity model) where TEntity : BaseModel
        {
            Context.Set<TEntity>().Add(model);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Set<TEntity>().OrderBy(x => x.Name).ToListAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsyncSortedByName()
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Set<City>().Include(c => c.Country).OrderBy(x => x.Name).ToListAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsyncSortedByUserName()
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Set<User>().OrderBy(x => x.UserName).ToListAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<IEnumerable<Excursion>> GetAllUpcomingGroupExcursionsAsync()
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Set<Excursion>().Where(c => c.ExcursionType.Category == Enums.ExcursionTypeEnum.Group && c.ExcursionDates.Any(d=>d.CheckIn>=DateTime.Today))
            .Include(c => c.Destinations.Select(d => d.Country))
            .Include(c => c.ExcursionType)
            .Include(c => c.ExcursionDates)
            .ToListAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<Excursion> GetExcursionByIdAsync(int id)
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Set<Excursion>().Where(c => c.Id==id)
            .Include(c => c.Destinations.Select(d => d.Country))
            .Include(c => c.ExcursionType)
            .Include(c => c.ExcursionDates)
            .FirstOrDefaultAsync());
            IsContextAvailable = true;
            return result;
        }

        public IEnumerable<TEntity> GetAllSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            return Context.Set<TEntity>().OrderBy(x => x.Name).ToList();
        }

        public async Task<TEntity> GetByNameAsync<TEntity>(string name) where TEntity : BaseModel, INamed
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Set<TEntity>().Where(x => x.Name == name).FirstOrDefaultAsync());
            IsContextAvailable = true;
            return result;
        }

        public virtual async Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : BaseModel
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Set<TEntity>().FindAsync(id));
            IsContextAvailable = true;
            return result;
        }

        public virtual TEntity GetById<TEntity>(int id) where TEntity : BaseModel
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual async Task<Booking> GetFullBookingByIdAsync(int id)
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Bookings.Where(b => b.Id == id)
            .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.ExcursionDate)
                .Include(f => f.Payments.Select(p => p.User))
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .FirstOrDefaultAsync());
            IsContextAvailable = true;
            return result;


        }
            public virtual async Task<Personal_Booking> GetFullPersonalBookingByIdAsync(int id)
        {
            IsContextAvailable = false;
            var result = await Task.Run(async () => await Context.Personal_Bookings.Where(b => b.Id == id)
            .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.Payments.Select(p => p.User))
                .FirstOrDefaultAsync());
            IsContextAvailable = true;
            return result;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : BaseModel
        {
            IsContextAvailable = false;
            if (filter == null)
            {
                var result = await Task.Run(async () => await Context.Set<TEntity>().ToListAsync());
                IsContextAvailable = true;
                return result;
            }
            else
            {
                var result = await Task.Run(async () => await Context.Set<TEntity>().Where(filter).ToListAsync());
                IsContextAvailable = true;
                return result;
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
            IsContextAvailable = false;

            await Task.Run(async () => await Context.SaveChangesAsync());
            IsContextAvailable = true;
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