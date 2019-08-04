using LATravelManager.DataAccess;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public static class DbContextExtensions
    {
        #region Methods

        public static IEnumerable<Tuple<object, object, EntityState>> GetAddedRelationships(
            this DbContext context)
        {
            return GetRelationships(context, EntityState.Added, (e, i) => e.CurrentValues[i]);
        }

        public static IEnumerable<Tuple<object, object, EntityState>> GetDeletedRelationships(
            this DbContext context)
        {
            return GetRelationships(context, EntityState.Deleted, (e, i) => e.OriginalValues[i]);
        }

        public static IEnumerable<Tuple<object, object, EntityState>> GetRelationships(
                            this DbContext context)
        {
            return GetAddedRelationships(context)
                    .Union(GetDeletedRelationships(context));
        }

        private static IEnumerable<Tuple<object, object, EntityState>> GetRelationships(
            this DbContext context,
            EntityState relationshipState,
            Func<ObjectStateEntry, int, object> getValue)
        {
            context.ChangeTracker.DetectChanges();
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.ObjectStateManager
                                .GetObjectStateEntries(relationshipState)
                                .Where(e => e.IsRelationship)
                                .Select(
                                        e => Tuple.Create(
                                                objectContext.GetObjectByKey((EntityKey)getValue(e, 0)),
                                                objectContext.GetObjectByKey((EntityKey)getValue(e, 1)),
                                                relationshipState));
        }

        #endregion Methods
    }

    public class GenericRepository : IGenericRepository, IDisposable
    {
        #region Constructors

        public GenericRepository()
        {
            Context = new MainDatabase();
            //Context.Database.Log = Console.Write;
            IsContextAvailable = true;
        }

        #endregion Constructors

        #region Fields

        protected readonly MainDatabase Context;
        private volatile Task lastTask;

        #endregion Fields

        #region Properties

        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }
        public bool IsContextAvailable { get; set; }
        public bool IsTaskOk => LastTask == null || LastTask.IsCompleted;

        public Task LastTask => lastTask;

        #endregion Properties

        #region Methods

        public void Add<TEntity>(TEntity model) where TEntity : BaseModel
        {
            Context.Set<TEntity>().Add(model);
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

        public void Dispose()
        {
            Context.Dispose();
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

        public List<TEntity> GetAllAsyncLocal<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : BaseModel
        {
            return Context.Set<TEntity>().Local.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            return await RunTask(Context.Set<TEntity>().OrderBy(x => x.Name).ToListAsync);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingInPeriod(DateTime minDay, DateTime maxDay, int excursionId, bool canceled = false)
        {
            return await RunTask(Context.Bookings.Where(c => c.Excursion.Id == excursionId && ((c.CheckIn <= maxDay && c.CheckIn >= minDay) || (c.CheckOut <= maxDay && c.CheckOut >= minDay)))
                .Include(f => f.Partner)
                .Include(f => f.ExcursionDate)
                .Include(f => f.User)
                .Include(f => f.Excursion)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room).Select(r => r.RoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .Where(r => r.Disabled == canceled)
                .ToListAsync);
        }

        //public async Task<User> FindUserAsync(string userName)
        //{
        //    return await RunTask(Context.Users.Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync);
        //}
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync(bool canceled = false)
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
                .Where(r => r.Disabled == canceled)
                .ToListAsync);
        }

        public async Task<IEnumerable<Booking>> GetAllBookinsFromCustomers(DateTime checkIn, bool canceled = false)
        {
            return await RunTask(Context.Bookings.Where(b => b.DifferentDates && b.ReservationsInBooking.Any(r => r.CustomersList.Any(c => c.CheckIn == checkIn)))
           .Include(f => f.Partner)
               .Include(f => f.User)
               .Include(f => f.Excursion)
               .Include(f => f.ExcursionDate)
               .Include(f => f.Payments.Select(p => p.User))
               .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
               .Include(f => f.ReservationsInBooking.Select(i => i.Room))
               .Include(f => f.ReservationsInBooking.Select(i => i.Room).Select(r => r.RoomType))
               .Include(f => f.ReservationsInBooking.Select(i => i.Room).Select(r => r.Hotel))
               .Include(f => f.ReservationsInBooking.Select(i => i.NoNameRoomType))
               .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
               .Where(b => b.Disabled == false)
               .ToListAsync);
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsyncSortedByName()
        {
            return await RunTask(Context.Set<City>().Include(c => c.Country).OrderBy(x => x.Name).ToListAsync);
        }

        public async Task<IEnumerable<Excursion>> GetAllExcursionsAsync()
        {
            return await RunTask(Context.Set<Excursion>()
            .Include(c => c.Destinations.Select(d => d.Country))
            .Include(c => c.ExcursionType)
            .Include(c => c.ExcursionDates)
            .ToListAsync);
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

        public async Task<IEnumerable<Reservation>> GetAllGroupReservationsByCreationDate(DateTime afterThisDay, bool canceled = false)
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
                .Where(r => r.Booking.Disabled == canceled)
                .ToListAsync);
        }

        public async Task<List<Hotel>> GetAllHotelsInCityAsync(int cityId)
        {
            return await RunTask(Context.Hotels.Where(x => x.City.Id == cityId).OrderBy(h => h.Name).ToListAsync);
        }

        public async Task<List<Option>> GetAllPendingOptions()
        {
            var limit = DateTime.Today.AddDays(2);
            return await RunTask(Context.Options.Where(ο => ο.Date <= limit)
                .Include(x => x.Room.Hotel)
                .ToListAsync);
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsByCreationDate(DateTime afterThisDay, int excursionId, bool canceled = false)
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
                .Where(r => r.Booking.Disabled == false)
                .ToListAsync);
        }

        public async Task<List<Room>> GetAllRoomsInCityAsync(DateTime minDay, DateTime maxDay, int cityId, int selectedRoomTypeId = 0)
        {
            return await RunTask(Context.Rooms.Where(x => x.Hotel.City.Id == cityId
            && (selectedRoomTypeId == 0 || x.RoomType.Id == selectedRoomTypeId)
            && x.DailyBookingInfo.Any(d => d.Date <= maxDay) && x.DailyBookingInfo.Any(d => d.Date >= minDay))
                .Include(x => x.Hotel.HotelCategory)
                .Include(x => x.DailyBookingInfo)
                .Include(x => x.RoomType)
                .ToListAsync);
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

        public async Task<IEnumerable<User>> GetAllUsersAsyncSortedByUserName()
        {
            return await RunTask(Context.Set<User>().OrderBy(x => x.UserName).ToListAsync);
        }

        public virtual TEntity GetById<TEntity>(int id) where TEntity : BaseModel
        {
            return Context.Set<TEntity>().Find(id);
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

        public async Task<TEntity> GetByNameAsync<TEntity>(string name) where TEntity : BaseModel, INamed
        {
            return await RunTask(Context.Set<TEntity>().Where(x => x.Name == name).FirstOrDefaultAsync);
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
                .Include(f => f.ChangesInBooking.Select(i => i.User))
                .FirstOrDefaultAsync);
        }

        public virtual async Task<Personal_Booking> GetFullPersonalBookingByIdAsync(int id)
        {
            return await RunTask(Context.Personal_Bookings.Where(b => b.Id == id)
                .Include(f => f.User)
                .Include(f => f.Payments)
                .Include(f => f.Customers.Select(s => s.Services))
                .Include(f => f.Services)
                .Include(f => f.Partner)
                .Include(f => f.User)
                .Include(f => f.Customers)
                .FirstOrDefaultAsync);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
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

        internal async Task CaptureChanges(BookingWrapper bookingWr)
        {
            var relatioships = DbContextExtensions.GetRelationships(Context).ToList();
            Context.ChangeTracker.DetectChanges();
            var changes = from e in Context.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;
            ObjectStateManager osm = ((IObjectContextAdapter)Context).ObjectContext.ObjectStateManager;
            var addedRelations = osm.GetObjectStateEntries(EntityState.Added).Where(e => e.IsRelationship).Select(r => new
            {
                EntityKeyInfo = r.CurrentValues[0],
                CollectionMemberKeyInfo = r.CurrentValues[1],
                r.State
            });
            var deletedRelations = osm.GetObjectStateEntries(EntityState.Deleted)
                                               .Where(e => e.IsRelationship).Select(r => new
                                               {
                                                   EntityKeyInfo = r.CurrentValues[0],
                                                   CollectionMemberKeyInfo = r.CurrentValues[1],
                                                   r.State
                                               });
            var modifiedRelations = osm.GetObjectStateEntries(EntityState.Modified)
                                                .Where(e => e.IsRelationship).Select(r => new
                                                {
                                                    EntityKeyInfo = r.CurrentValues[0],
                                                    CollectionMemberKeyInfo = r.CurrentValues[1],
                                                    r.State
                                                });

            if (changes.Count() > 0 || relatioships.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                if (relatioships.Count > 0)
                {
                    var ru = relatioships.Where(r => r.Item1 is Booking && r.Item2 is User).ToList();
                    if (ru.Count == 2)
                    {
                        sb.Append($"Ο πωλητής άλλαξε από '{(ru[1].Item2 as User).UserName}' σε '{(ru[0].Item2 as User).UserName}', ");
                    }

                    var par = relatioships.Where(r => r.Item1 is Partner && r.Item2 is Booking).ToList();
                    if (par.Count == 2)
                    {
                        sb.Append($"Ο Συνεργάτης άλλαξε από '{(par[1].Item1 as Partner).Name}' σε '{(par[0].Item1 as Partner).Name}', ");
                    }

                    var cust = relatioships.Where(r => r.Item1 is Customer && r.Item2 is Reservation).ToList();
                    foreach (var c in cust)
                    {
                        if (!cust.Any(c1 => c1.Item1 is Customer cus && cus.Id == (c.Item1 as Customer).Id))
                        {
                            var reswr = new ReservationWrapper(c.Item2 as Reservation);
                            if (c.Item3 == EntityState.Deleted)
                                sb.Append($"Διαγράφηκε ο πελάτης'{c.Item1 as Customer}' απο '{reswr.RoomTypeName}' στο '{reswr.HotelName}', ");
                            else if (c.Item3 == EntityState.Added)
                                sb.Append($"Προστέθηκε ο πελάτης'{c.Item1 as Customer}', ");
                        }
                    }

                    var res = relatioships.Where(r => r.Item1 is Reservation && r.Item2 is Booking).ToList();
                    foreach (var r in res)
                    {
                        var reswr = new ReservationWrapper(r.Item1 as Reservation);
                        if (r.Item3 == EntityState.Deleted)
                            sb.Append($"Διαγράφηκε '{reswr.Model.LastRoomtype}' δωμάτιο στο '{reswr.Model.LastHotel}' με τους πελάτες '{reswr.Model.LastCustomers}', ");
                        else if (r.Item3 == EntityState.Added)
                            sb.Append($"Προστέθηκε '{reswr.RoomTypeName}' δωμάτιο στο '{reswr.HotelName}' με τους πελάτες '{reswr.Names}', ");
                    }
                }

                foreach (DbEntityEntry change in changes)
                {
                    switch (change.Entity)
                    {
                        case Booking b:
                            if (change.State == EntityState.Modified)
                                sb.Append(GetBookingChanges(change, relatioships));
                            break;

                        case Customer c:
                            if (change.State == EntityState.Modified)
                                sb.Append(GetCustomerChanges(change, relatioships));
                            break;

                        default:
                            break;
                    }

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
                if (sb.Length > 0)
                {
                    bookingWr.ChangesInBooking.Add(new ChangeInBooking { Date = DateTime.Now, Description = sb.ToString().TrimEnd(' ', ','), User = await GetByIdAsync<User>(StaticResources.User.Id) });
                }
            }
        }

        internal void DeletePayments(int bookingId)
        {
            List<Payment> toDelete = Context.Payments.Where(p => p.Booking.Id == bookingId).ToList();
            Context.Payments.RemoveRange(toDelete);
        }

        internal async Task<IEnumerable<Booking>> GetAllBookingsFiltered(int excursionId, int excursionCategory, int grafeio, int userId, int partnerId, DateTime From, DateTime To, bool canceled = false)
        {
            return await RunTask(Context.Bookings.Where(c =>
            (excursionCategory == -1 || (int)c.Excursion.ExcursionType.Category == excursionCategory) &&
            (excursionId == -1 || c.Excursion.Id == excursionId) &&
            (grafeio == 0 || c.User.BaseLocation == grafeio) &&
            (partnerId == -1 || c.IsPartners && c.Partner.Id == partnerId) &&
            (userId == -1 || c.User.Id == userId) &&
            c.CheckIn >= From && c.CheckIn <= To)
                .Include(f => f.Partner)
                .Include(f => f.ExcursionDate)
                .Include(f => f.User)
                .Include(f => f.Excursion)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Where(r => r.Disabled == canceled)
                .ToListAsync);
        }

        internal async Task<List<Hotel>> GetAllHotelsAsync<T>()
        {
            return await RunTask(Context.Hotels.OrderBy(h => h.Name)
                .Include(h => h.HotelCategory)
                .Include(h => h.City)
                .ToListAsync);
        }

        internal async Task<IEnumerable<Payment>> GetAllPaymentsFiltered(int excursionId, int userId, DateTime dateLimit, bool enableDatesFilter, DateTime from, DateTime to, bool canceled = false)
        {
            return await RunTask(Context.Payments
               .Where(c => (
               (!enableDatesFilter && c.Date >= dateLimit) ||
               (enableDatesFilter && c.Date >= from && c.Date <= to)) &&
               (excursionId == 0 || c.Booking.Excursion.Id == excursionId) &&
               (userId == 0 || c.User.Id == userId) &&
               c.User.BaseLocation == StaticResources.User.BaseLocation)
               .Include(f => f.User)
               .Include(f => f.Booking)
               .Include(f => f.Booking.Excursion)
               .Include(f => f.Booking.ExcursionDate)
               .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList))
               .Include(f => f.Personal_Booking)
               .Include(f => f.Personal_Booking.Services)
               .Include(f => f.Personal_Booking.Customers)
               .Include(f => f.ThirdParty_Booking)
               .Include(f => f.ThirdParty_Booking.Customers)
               .Where(f => (!f.Outgoing&& (f.Booking != null && f.Booking.Disabled == canceled)) || (f.Personal_Booking != null && f.Personal_Booking.Disabled == canceled) || (f.ThirdParty_Booking != null && f.ThirdParty_Booking.Disabled == canceled))
               .ToListAsync);
        }

        internal async Task<List<Personal_Booking>> GetAllPersonalBookingsFiltered(int userId, bool completed, DateTime dateLimit, bool canceled = false)
        {
            var x = await RunTask(Context.Personal_Bookings
                .Where(c =>
                c.CreatedDate >= dateLimit &&
                (userId == 0 || c.User.Id == userId) &&
                (completed || c.Services.Any(s => s.TimeReturn >= DateTime.Today)))
                .Include(f => f.User)
                .Include(f => f.Services)
                .Include(f => f.Partner)
                .Include(f => f.Payments)
                .Include(f => f.Customers)
                .Where(b => b.Disabled == canceled)
                .ToListAsync);
            return x;
        }

        internal async Task<List<Reservation>> GetAllReservationsDimitri(Expression<Func<Reservation, bool>> p, bool canceled = false)
        {
            return await RunTask(Context.Reservations
                  .Where(p)
                  .Include(f => f.Booking.Excursion)
                  .Include(f => f.Booking.Excursion.ExcursionType)
                  .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList))
                  .Include(f => f.CustomersList)
                  .Where(b => b.Booking.Disabled == canceled)
                  .ToListAsync);
        }

        internal async Task<List<Reservation>> GetAllReservationsFiltered(int excursionId, int userId, bool completed, int category, DateTime dateLimit, bool canceled = false)
        {
            return await RunTask(Context.Reservations
                  .Where(c =>
                  c.Booking.ReservationsInBooking.Any(r => r.CreatedDate >= dateLimit) &&
                  (category >= 0 ? (int)c.Booking.Excursion.ExcursionType.Category == category : true) &&
                  (excursionId == 0 || c.Booking.Excursion.Id == excursionId) &&
                  (userId == 0 || c.Booking.User.Id == userId) &&
                  (completed || c.Booking.CheckOut >= DateTime.Today))
                  .Include(f => f.Booking.User)
                  .Include(f => f.Booking.ExcursionDate)
                  .Include(f => f.Booking.Partner)
                  .Include(f => f.Booking.Excursion)
                  .Include(f => f.Booking.Excursion.ExcursionType)
                  .Include(f => f.Booking.Payments)
                  .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList))
                  .Include(f => f.Room.Hotel)
                  .Include(f => f.Room.RoomType)
                  .Include(f => f.CustomersList)
                  .Include(f => f.Hotel)
                  .Include(f => f.NoNameRoomType)
                  .Where(b => b.Booking.Disabled == canceled)
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

        internal async Task<List<ThirdParty_Booking>> GetAllThirdPartyBookingsFiltered(int userId, bool completed, DateTime dateLimit, bool canceled = false)
        {
            var x = await RunTask(Context.ThirdParty_Bookings
                .Where(c =>
                c.CreatedDate >= dateLimit &&
                (userId == 0 || c.User.Id == userId) &&
                (completed || c.CheckIn >= DateTime.Today))
                 .Include(f => f.User)
                 .Include(f => f.Payments)
                 .Include(f => f.Customers)
                 .Include(f => f.Partner)
                 .Include(f => f.File)
                 .Where(b => b.Disabled == canceled)
                 .ToListAsync);
            return x;
        }

        internal async Task<ThirdParty_Booking> GetFullThirdPartyBookingByIdAsync(int id)
        {
            return await RunTask(Context.ThirdParty_Bookings.Where(b => b.Id == id)
               .Include(f => f.User)
               .Include(f => f.Payments)
               .Include(f => f.Customers)
               .Include(f => f.Partner)
               .Include(f => f.File)
               .FirstOrDefaultAsync);
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

        internal async Task LoadBasic()
        {
            await Context.RoomTypes.LoadAsync();
            await Context.Countries.LoadAsync();
            await Context.Hotels.LoadAsync();
        }

        private string GetBookingChanges(DbEntityEntry change, List<Tuple<object, object, EntityState>> relatioships)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string propertyName in change.OriginalValues.PropertyNames)
            {
                var original = change.OriginalValues[propertyName];
                var current = change.CurrentValues[propertyName];

                if (!Equals(original, current) && change.Entity is Booking b)
                {
                    switch (propertyName)
                    {
                        case nameof(Booking.CancelReason):
                            if (original is string s1 && !string.IsNullOrEmpty(s1))
                                sb.Append($"Λόγος ακύρωσης από '{original.ToString()}' σε '{current.ToString()}', ");
                            break;

                        case nameof(Booking.CheckIn):
                            sb.Append($"CheckIn από '{((DateTime)original).ToString("dd/MM/yyyy")}' σε '{b.CheckIn.ToString("dd/MM/yyyy")}', ");
                            break;

                        case nameof(Booking.CheckOut):
                            sb.Append($"CheckOut από '{((DateTime)original).ToString("dd/MM/yyyy")}' σε '{b.CheckOut.ToString("dd/MM/yyyy")}', ");
                            break;

                        case nameof(Booking.Comment):
                            if (original is string s2 && !string.IsNullOrEmpty(s2))
                                sb.Append($"Σχόλιο κράτησης από '{original.ToString()}' σε '{current.ToString()}', ");
                            break;

                        case nameof(Booking.Commision):
                            if (original is decimal d1 && b.IsPartners && (bool)change.OriginalValues[nameof(Booking.IsPartners)])
                                sb.Append($"Προμήθεια συνεργάτη από '{d1.ToString("0.##")}' σε '{b.Commision.ToString("0.##")}', ");
                            break;

                        case nameof(Booking.DifferentDates):
                            if (current is bool d)
                                if (d)
                                    sb.Append($"Πλέον οι πελάτες ΕΧΟΥΝ διαφορετικές ημερομηνίες, ");
                                else
                                    sb.Append($"Πλέον οι πελάτες ΔΕΝ ΕΧΟΥΝ διαφορετικές ημερομηνίες, ");
                            break;

                        case nameof(Booking.Disabled):
                            if (current is bool dis)
                                if (dis)
                                    sb.Append($"Η κράτηση ακυρώθηκε με αιτιολογία '{b.CancelReason}', ");
                                else
                                    sb.Append($"Η κράτηση επανενεργοποιήθηκε, ");
                            break;

                        case nameof(Booking.ExcursionDate):
                            if (original is ExcursionDate ed)
                                sb.Append($"Ημέρομηνίες Εκδρομής από '{ed.Name}' σε '{b.ExcursionDate.Name}', ");
                            break;

                        case nameof(Booking.IsPartners):
                            if (current is bool ip)
                                if (ip)
                                    sb.Append($"Η κράτηση είναι πλέον συνεργάτη, ");
                                else if (relatioships.Any(r => r.Item1 is Partner && r.Item3 == EntityState.Deleted))
                                    sb.Append($"Η κράτηση ΔΕΝ είναι πλέον συνεργάτη. Ηταν '{(relatioships.Where(r => r.Item1 is Partner && r.Item3 == EntityState.Deleted).FirstOrDefault().Item1 as Partner).Name}' " +
                                        $"με προμήθεια '{((decimal)change.OriginalValues[nameof(Booking.Commision)]).ToString("0.##\\%")}'" +
                                        $" και ΝΕΤ τιμή'{((decimal)change.OriginalValues[nameof(Booking.NetPrice)]).ToString("0.##€")}', ");
                            break;

                        case nameof(Booking.NetPrice):
                            if (original is decimal d2 && ((bool)change.OriginalValues[nameof(Booking.IsPartners)]) && b.IsPartners)
                                sb.Append($"Net τιμή από '{d2.ToString("0.##€")}' σε '{b.NetPrice.ToString("0.##€")}', ");
                            break;

                        case nameof(Booking.PartnerEmail):
                            if (b.IsPartners && (bool)change.OriginalValues[nameof(Booking.IsPartners)] && original is string pe)
                                sb.Append($"Email συνεργάτη από '{pe}' σε '{b.PartnerEmail}', ");
                            break;

                        case nameof(Booking.Reciept):
                            if (current is bool rec)
                                if (rec)
                                    sb.Append($"Βγήκε απόδειξη, ");
                                else
                                    sb.Append($"Τελικά δεν βγήκε απόδειξη, ");
                            break;

                        case nameof(Booking.SecondDepart):
                            if (current is bool sd)
                                if (sd)
                                    sb.Append($"Είναι πλέον δέυτερη αναχώρηση, ");
                                else
                                    sb.Append($"Δέν είναι πλέον δέυτερη αναχώρηση, ");
                            break;

                        default:
                            break;
                    }
                }
            }

            return sb.ToString();
        }

        private string GetCustomerChanges(DbEntityEntry change, List<Tuple<object, object, EntityState>> relatioships)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string propertyName in change.OriginalValues.PropertyNames)
            {
                var original = change.OriginalValues[propertyName];
                var current = change.CurrentValues[propertyName];

                if (!Equals(original, current) && change.Entity is Customer c)
                {
                    switch (propertyName)
                    {
                        case nameof(Customer.Name):
                        case nameof(Customer.Surename):
                            sb.Append($"Πελάτης από '{change.OriginalValues[nameof(Customer.Surename)].ToString() + " " + change.OriginalValues[nameof(Customer.Name)].ToString()}' σε '{c}', ");
                            break;

                        case nameof(Customer.Age):
                            sb.Append($"Ηλικία  του '{c}' από '{((int)original)}' σε '{c.Age}', ");
                            break;

                        case nameof(Customer.CheckIn):
                            if (c.Reservation.Booking.DifferentDates)
                                sb.Append($"CheckIn  του '{c}' από '{((DateTime)original)}' σε '{c.CheckIn}', ");
                            break;

                        case nameof(Customer.CheckOut):
                            if (c.Reservation.Booking.DifferentDates)
                                sb.Append($"CheckIn  του '{c}' από '{((DateTime)original)}' σε '{c.CheckOut}', ");
                            break;

                        case nameof(Customer.CustomerHasBusIndex):
                            sb.Append($"Λεωφορείο  του '{c}' από '{((int)original)}' σε '{c.CustomerHasBusIndex}', ");
                            break;

                        case nameof(Customer.CustomerHasPlaneIndex):
                            sb.Append($"Αεροπλάνο  του '{c}' από '{((int)original)}' σε '{c.CustomerHasPlaneIndex}', ");
                            break;

                        case nameof(Customer.CustomerHasShipIndex):
                            sb.Append($"Πλοίο  του '{c}' από '{((int)original)}' σε '{c.CustomerHasShipIndex}', ");
                            break;

                        case nameof(Customer.DeserveDiscount):
                            sb.Append($"Πάσο  του '{c}' από '{((int)original)}' σε '{c.DeserveDiscount}', ");
                            break;

                        case nameof(Customer.DOB):
                            sb.Append($"Ημερ. γεν.  του '{c}' από '{(((DateTime?)original).HasValue ? ((DateTime?)original).Value.ToString("dd/MM/yyyy") : "κενό")}' σε" +
                                $" '{(c.DOB.HasValue ? c.DOB.Value.ToString("dd/MM/yyyy") : "κενό")}', ");
                            break;

                        case nameof(Customer.Email):
                            sb.Append($"Email  του '{c}' από '{((string)original)}' σε '{c.Email}', ");
                            break;

                        case nameof(Customer.PassportNum):
                            sb.Append($"Αρ.Ταυτότητας  του '{c}' από '{((string)original)}' σε '{c.PassportNum}', ");
                            break;

                        case nameof(Customer.Price):
                            if (!c.Reservation.Booking.IsPartners)
                                sb.Append($"Τιμή  του '{c}' από '{((decimal)original).ToString("0.##€")}' σε '{c.Price.ToString("0.##€")}', ");
                            break;

                        case nameof(Customer.ReturningPlace):
                            sb.Append($"Σημείο επιστροφής του '{c}' από '{((string)original)}' σε '{c.ReturningPlace}', ");
                            break;

                        case nameof(Customer.StartingPlace):
                            sb.Append($"Σημείο αναχώρησης του '{c}' από '{((string)original)}' σε '{c.StartingPlace}', ");
                            break;

                        case nameof(Customer.Tel):
                            sb.Append($"Τηλέφωνο του '{c}' από '{((string)original)}' σε '{(c.Tel ?? "")}', ");
                            break;
                    }
                }
            }

            return sb.ToString();
        }

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

        #endregion Methods
    }
}