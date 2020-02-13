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
using System.Windows.Input;
using LATravelManager.DataAccess;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;

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

        public GenericRepository(bool Nochanges = false)
        {
            Context = new MainDatabase();
            if (Nochanges == true)
                Context.Configuration.AutoDetectChangesEnabled = false;
            //Context.Database.Log = Console.Write;
            IsContextAvailable = true;
        }

        #endregion Constructors

        #region Fields

        public MainDatabase Context;

        private readonly DateTime limit = DateTime.Today.AddDays(2);

        private volatile Task lastTask;

        #endregion Fields

        #region Properties

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
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
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

        public List<TEntity> GetAllAsyncLocal<TEntity>() where TEntity : BaseModel
        {
            return Context.Set<TEntity>().Local.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncSortedByName<TEntity>() where TEntity : BaseModel, INamed
        {
            return await RunTask(Context.Set<TEntity>().OrderBy(x => x.Name).ToListAsync);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingInPeriod(DateTime minDay, DateTime maxDay, City city = null, int excursionId = -1, bool canceled = false)
        {
            int cityId = city != null ? city.Id : -1;
            return await RunTask(Context.Bookings.Where(c => ((cityId > 0 && c.Excursion.Destinations.FirstOrDefault().Id == cityId) || (excursionId > 0 && c.Excursion.Id == excursionId)) && ((c.CheckIn <= maxDay && c.CheckIn >= minDay) || (c.CheckOut <= maxDay && c.CheckOut >= minDay)))
                .Include(f => f.Partner)
                .Include(f => f.ExcursionDate)
                .Include(f => f.User)
                .Include(f => f.Excursion)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room).Select(r => r.RoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .Where(r => r.Disabled == canceled)
                .ToListAsync);
        }

        //public async Task<User> FindUserAsync(string userName)
        //{
        //    return await RunTask(Context.Users.Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync);
        //}
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync(int excursionId, int excursionCategory, int grafeio, int partnerId, DateTime From, DateTime To, bool byCheckIn = false, bool canceled = false, bool onlypartners = false)
        {
            return await RunTask(Context.Bookings.Where(c =>
            c.Disabled == canceled &&
            (excursionCategory == -1 || (int)c.Excursion.ExcursionType.Category == excursionCategory) &&
            (excursionId == -1 || c.Excursion.Id == excursionId) &&
            (grafeio == 0 || c.User.BaseLocation == grafeio) &&
            (!onlypartners || c.IsPartners) &&
            ((partnerId == -1 && c.Partner.Id != 219) || ((c.Partner.Id == partnerId) && (StaticResources.User.BaseLocation == 1 || c.Partner.Id != 219))) &&
            ((byCheckIn && c.CheckIn >= From && c.CheckIn <= To) || (!byCheckIn && c.CreatedDate >= From && c.CreatedDate <= To)))
                .Include(f => f.Partner)
                .Include(f => f.ExcursionDate)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.Payments)
                .Include(f => f.User)
                .Include(f => f.Excursion)
                .Include(f => f.Excursion.ExcursionType)
                .Include(f => f.ReservationsInBooking.Select(i => i.Room.Hotel))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room.RoomType))
                .Include(f => f.ExtraServices)
                .ToListAsync);
        }

        public IEnumerable<Booking> GetAllBookingsForLists(int excursionId, DateTime checkIn, int dep, bool checkout = false)
        {
            if (!checkout)
            {
                return Context.Bookings.Where(b => b.Excursion.Id == excursionId && b.Disabled == false &&
                ((!b.DifferentDates && b.CheckIn == checkIn) || (b.DifferentDates && b.ReservationsInBooking.Any(tc => tc.CustomersList.Any(r => r.CheckIn == checkIn)))) &&
                (dep == 0 || b.User.BaseLocation == dep))
                   .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                   .Include(f => f.ReservationsInBooking.Select(i => i.Room).Select(r => r.Hotel))
                   .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                   .Include(f => f.Excursion)
                   .ToList();
            }

            return Context.Bookings.Where(b => b.Excursion.Id == excursionId && b.Disabled == false &&
            ((!b.DifferentDates && b.CheckOut == checkIn) || (b.DifferentDates && b.ReservationsInBooking.Any(tc => tc.CustomersList.Any(r => r.CheckOut == checkIn)))) &&
            (dep == 0 || b.User.BaseLocation == dep))
               .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
               .Include(f => f.ReservationsInBooking.Select(i => i.Room).Select(r => r.Hotel))
               .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
               .Include(f => f.Excursion)
               .ToList();
        }

        public async Task<IEnumerable<Booking>> GetAllBookinsFromCustomers(DateTime checkIn, DateTime checkout)
        {
            return await RunTask(Context.Bookings.Where(b => b.DifferentDates && b.ReservationsInBooking.Any(r => r.CustomersList.Any(c => (checkout.Year < 2000 && c.CheckIn == checkIn) || (checkout.Year > 2000 && c.CheckOut == checkout))))
           .Include(f => f.Partner)
               .Include(f => f.User)
               .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
               .Include(f => f.Excursion)
               .Include(f => f.ExcursionDate)
               .Include(f => f.Payments.Select(p => p.User))
               .Include(f => f.ReservationsInBooking.Select(i => i.Room))
               .Include(f => f.ReservationsInBooking.Select(i => i.Room).Select(r => r.RoomType))
               .Include(f => f.ReservationsInBooking.Select(i => i.Room).Select(r => r.Hotel))
               .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
               .Include(f => f.ExtraServices)
               .Where(b => b.Disabled == false)
               .ToListAsync);
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsyncSortedByName()
        {
            return await RunTask(Context.Set<City>()
                .Include(c => c.Country)
                .Include(c => c.ExcursionTimes.Select(p => p.StartingPlace))
                .OrderBy(x => x.Name)
                .ToListAsync);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(DateTime date)
        {
            return await RunTask(Context.Customers.Where(c => (!c.Reservation.Booking.DifferentDates && c.Reservation.Booking.CheckIn <= date && c.Reservation.Booking.CheckOut >= date) || (c.Reservation.Booking.DifferentDates && c.CheckIn <= date && c.CheckOut >= date) && c.Reservation.Booking.Excursion.Destinations.Any(t => t.Id == 2))
                .Include(r => r.Reservation)
                .Include(r => r.Reservation.Booking)
                .Include(r => r.Reservation.Hotel)
                .Include(r => r.BusGo)
                .Include(r => r.OptionalExcursions.Select(o => o.OptionalExcursion))
                .Include(r => r.Reservation.Room.Hotel)
                .Where(r => r.Reservation.Booking.Disabled == false)
                .ToListAsync);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersWithPhone()
        {
            DateTime limit = new DateTime(2019, 10, 1);
            return await RunTask(Context.Customers.Where(c => c.Tel.Length > 9 && !c.Tel.StartsWith("000") && c.Reservation != null && c.Reservation.Booking.Partner == null && c.Reservation.Booking.User.BaseLocation == 1 && c.Reservation.Booking.CreatedDate > limit)
                .Where(r => r.Reservation.Booking.Disabled == false)
                .ToListAsync);
        }

        public async Task<IEnumerable<Excursion>> GetAllExcursionsAsync()
        {
            DateTime t = DateTime.Today.AddYears(-1);
            return await RunTask(Context.Set<Excursion>()
                .Where(e => e.ExcursionDates.Any(c => c.CheckOut > t))
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
                .Where(c => c.ExcursionType.Category == ExcursionTypeEnum.Group)
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
                .Where(c => c.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Group && c.CreatedDate >= afterThisDay)
                .Include(f => f.Booking)
                .Include(f => f.CustomersList)
                .Include(f => f.Booking.User)
                .Include(f => f.Booking.ExcursionDate)
                .Include(f => f.Booking.Partner)
                .Include(f => f.Booking.Payments)
                .Include(f => f.Room)
                .Include(f => f.Room.Hotel)
                .Include(f => f.Room.RoomType)
                .Include(f => f.Hotel)
                .Include(f => f.Booking.ExtraServices)
                .Where(r => r.Booking.Disabled == canceled)
                .ToListAsync);
        }

        public async Task<List<Hotel>> GetAllHotelsInCityAsync(int cityId)
        {
            return await RunTask(Context.Hotels.Where(x => x.City.Id == cityId).OrderBy(h => h.Name).ToListAsync);
        }

        public async Task<List<Booking>> GetAllNonPayersGroup()
        {
            try
            {
                return await RunTask(Context.Bookings
                    .Where(o => (o.CheckIn >= DateTime.Today && (StaticResources.User.Level < 3 || o.User.Id == StaticResources.User.Id) &&
                    o.User.BaseLocation == StaticResources.User.BaseLocation &&
                    ((DbFunctions.DiffDays(DateTime.Today, o.CreatedDate) >= 5 && !o.Payments.Any(p => p.Amount > 0)) ||
                       DbFunctions.DiffDays(o.CheckIn, o.CreatedDate) <= 5)) && o.Disabled == false)
                           .Include(x => x.ReservationsInBooking.Select(t => t.CustomersList))
                           .Include(x => x.Payments)
                           .Include(x => x.ExtraServices)
                           .Include(x => x.Partner)
                           .Include(x => x.User)
                           .Include(x => x.Excursion.Destinations)
                           .ToListAsync);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Personal_Booking>> GetAllNonPayersPersonal()
        {
            return await RunTask(Context.Personal_Bookings
                .Where(o => o.Services.Any(y => y.TimeGo >= DateTime.Today) &&
                o.User.BaseLocation == StaticResources.User.BaseLocation &&
                (StaticResources.User.Level < 3 || o.User.Id == StaticResources.User.Id) &&
                ((DbFunctions.DiffDays(DateTime.Today, o.CreatedDate) >= 5 && !o.Payments.Any(p => p.Amount > 0)) ||
                 o.Services.Any(r => DbFunctions.DiffDays(r.TimeGo, o.CreatedDate) <= 5)) && o.Disabled == false)
                .Include(x => x.Customers)
                .Include(x => x.Partner)
                .Include(x => x.Payments)
                .ToListAsync);
        }

        public async Task<List<ThirdParty_Booking>> GetAllNonPayersThirdparty()
        {
            return await RunTask(Context.ThirdParty_Bookings
                .Where(o => (StaticResources.User.Level < 3 || o.User.Id == StaticResources.User.Id) &&
                o.User.BaseLocation == StaticResources.User.BaseLocation &&
                o.CheckIn >= DateTime.Today &&
                ((DbFunctions.DiffDays(DateTime.Today, o.CreatedDate) >= 5 && !o.Payments.Any(p => p.Amount > 0))
                || DbFunctions.DiffDays(o.CheckIn, o.CreatedDate) <= 5) && o.Disabled == false)
                .Include(x => x.Customers)
                .Include(x => x.Payments)
                .Include(x => x.Partner)
                .Include(x => x.User)
                .ToListAsync);
        }

        public async Task<List<Option>> GetAllPendingOptions(bool ok = false)
        {
            return await RunTask(Context.Options
                .Where(o => o.Room.User.BaseLocation == StaticResources.User.BaseLocation &&
                ((ok && o.NotifStatus != null && o.NotifStatus.IsOk) || (!ok && (o.NotifStatus == null || !o.NotifStatus.IsOk))) &&
                (StaticResources.User.Level < 3 || o.Room.User.Id == StaticResources.User.Id) &&
                o.Date >= DateTime.Today && o.Date <= limit)
                .Include(x => x.Room.Hotel)
                .Include(x => x.Room.User)
                .ToListAsync);
        }

        public async Task<List<HotelService>> GetAllPersonalOptions(bool ok = false)
        {
            return await RunTask(Context.HotelServices
                .Where(o => o.Personal_Booking.User.BaseLocation == StaticResources.User.BaseLocation &&
                ((ok && o.NotifStatus != null && o.NotifStatus.IsOk) || (!ok && (o.NotifStatus == null || !o.NotifStatus.IsOk))) &&
                (StaticResources.User.Level < 2 || o.Personal_Booking.User.Id == StaticResources.User.Id) &&
                 o.Option >= DateTime.Today && o.Option <= limit && o.Personal_Booking.Disabled == false)
                .Include(x => x.Personal_Booking.Customers)
                .Include(x => x.Personal_Booking.Partner)
                .ToListAsync);
        }

        public async Task<List<PlaneService>> GetAllPlaneOptions(bool ok = false)
        {
            var planelimit = DateTime.Today.AddDays(4);
            return await RunTask(Context.PlaneServices
                .Where(o => o.Personal_Booking.User.BaseLocation == StaticResources.User.BaseLocation &&
                ((ok && o.NotifStatus != null && o.NotifStatus.IsOk) || (!ok && (o.NotifStatus == null || !o.NotifStatus.IsOk))) &&
                (StaticResources.User.Level < 2 || o.Personal_Booking.User.Id == StaticResources.User.Id) &&
                (o.TimeGo >= DateTime.Today || o.TimeReturn >= DateTime.Today) &&
                (o.TimeGo <= planelimit || o.TimeReturn <= planelimit) && o.Personal_Booking.Disabled == false)
                .Include(x => x.Personal_Booking.Customers)
                .Include(x => x.Airline)
                .ToListAsync);
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsByCreationDate(DateTime afterThisDay, int excursionId, bool canceled = false)
        {
            var limit = DateTime.Today.AddDays(2);
            return await RunTask(Context.Reservations.Where(c => c.Booking.Excursion.Id == excursionId && c.CreatedDate >= afterThisDay)
                .Include(f => f.Booking)
                .Include(f => f.Booking.User)
                .Include(f => f.Booking.ExcursionDate)
                .Include(f => f.Booking.Partner)
                .Include(f => f.Booking.Payments)
                .Include(f => f.Booking.ExtraServices)
                .Include(f => f.Room)
                .Include(f => f.Room.Hotel)
                .Include(f => f.Room.RoomType)
                .Include(f => f.CustomersList)
                .Include(f => f.Hotel)
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

        public string GetBoard(int b)
        {
            switch (b)
            {
                case 0:
                    return "BB";

                case 1:
                    return "HB";

                case 2:
                    return "FB";
            }
            return "ERR";
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
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList.Select(j => j.BusGo.Vehicle)))
                .Include(f => f.Excursion)
                .Include(f => f.ExcursionDate)
                .Include(f => f.Payments.Select(p => p.User))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room.RoomType))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room.Hotel))
                .Include(f => f.ReservationsInBooking.Select(i => i.Hotel))
                .Include(f => f.ChangesInBooking.Select(i => i.User))
                .Include(f => f.ExtraServices)
                .FirstOrDefaultAsync);
        }

        public virtual async Task<Personal_Booking> GetFullPersonalBookingByIdAsync(int id)
        {
            return await RunTask(Context.Personal_Bookings.Where(b => b.Id == id)
                .Include(f => f.User)
                .Include(f => f.Payments)
                .Include(f => f.Customers.Select(s => s.Services))
                .Include(f => f.Partner)
                .Include(f => f.User)
                .FirstOrDefaultAsync);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public bool IsAnyInRoom(int roomId)
        {
            return (Context.Set<Reservation>().Where(re => re.Room.Id == roomId).ToList()).Count > 0;
        }

        public void RemoveById<TEntity>(int id) where TEntity : BaseModel
        {
            TEntity entity = Context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public void RollBack()
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
            RejectNavigationChanges();
        }

        public async Task SaveAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
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

            //IEnumerable<DbEntityEntry> changes = from e in Context.ChangeTracker.Entries()
            //                                     where e.State != EntityState.Unchanged
            //                                     select e;

            //foreach (DbEntityEntry change in changes)
            //{
            //    if (change.State == EntityState.Added)
            //    {
            //        // Log Added
            //    }
            //    else if (change.State == EntityState.Modified)
            //    {
            //        // Log Modified
            //        object item = change.Entity;
            //        DbPropertyValues originalValues = Context.Entry(item).OriginalValues;
            //        DbPropertyValues currentValues = Context.Entry(item).CurrentValues;

            //        foreach (string propertyName in originalValues.PropertyNames)
            //        {
            //            object original = originalValues[propertyName];
            //            object current = currentValues[propertyName];

            //            Console.WriteLine("Property {0} changed from {1} to {2}",
            //         propertyName,
            //         originalValues[propertyName],
            //         currentValues[propertyName]);
            //        }
            //    }
            //    else if (change.State == EntityState.Deleted)
            //    {
            //        // log deleted
            //    }
            //}
            await RunTask(Context.SaveChangesAsync);
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public virtual void UpdateValues<TEntity>(TEntity entity, TEntity newEntity)
           where TEntity : EditTracker
        {
            // entity.ModifiedDate = DateTime.Now;
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
            ((partnerId == -1 && c.Partner.Id != 219) || ((c.Partner.Id == partnerId) && (StaticResources.User.BaseLocation == 1 || c.Partner.Id != 219))) &&
            (userId == -1 || c.User.Id == userId) &&
            c.CheckIn >= From && c.CheckIn <= To)
                .Include(f => f.Partner)
                .Include(f => f.ExcursionDate)
                .Include(f => f.User)
                .Include(f => f.Excursion)
                .Include(f => f.ReservationsInBooking.Select(i => i.CustomersList))
                .Include(f => f.ReservationsInBooking.Select(i => i.Room.Hotel))
                .Where(r => r.Disabled == canceled)
                .ToListAsync);
        }

        internal IEnumerable<Bus> GetAllBuses(int id = 0, DateTime checkIn = default, bool checkout = false)
        {
            if (!checkout)
            {
                return Context.Set<Bus>().Where(b => (id == 0 && b.TimeGo >= DateTime.Today) || (b.Excursion.Id == id && b.TimeGo == checkIn))
                 .Include(f => f.Excursion)
                 .Include(f => f.Customers)
                 .Include(f => f.Leader)
                 .Include(f => f.Vehicle)
                 .ToList();
            }
            return Context.Set<Bus>().Where(b => (id == 0 && b.TimeReturn >= DateTime.Today) || (b.Excursion.Id == id && b.TimeReturn == checkIn))
                 .Include(f => f.Excursion)
                 .Include(f => f.Customers)
                 .Include(f => f.Leader)
                 .Include(f => f.Vehicle)
                 .ToList();
        }

        internal async Task<IEnumerable<Bus>> GetAllBusesAsync(int id = 0, DateTime checkIn = default, DateTime checkout = default, bool strict = false)
        {
            if (checkIn.Year < 2000)
            {
                checkIn = DateTime.Today.AddYears(-200);
            }
            DateTime from = checkIn.AddDays(-5);
            DateTime to = checkIn.AddDays(5);
            if (!strict)
            {
                return await RunTask(Context.Set<Bus>().Where(b => ((id == 0 || b.Excursion.Id == id) && (((b.TimeGo >= from && b.TimeGo <= to)) || ((b.TimeReturn >= from && b.TimeReturn <= to)))))
                  .Include(f => f.Excursion)
                  //.Include(f => f.Customers)
                  .Include(f => f.Leader)
                  .Include(f => f.Vehicle)
                  .ToListAsync);
            }
            else
            {
                return await RunTask(Context.Set<Bus>().Where(b => ((id == 0 || b.Excursion.Id == id) && ((checkIn.Year > 2000 && b.TimeGo == checkIn) || (checkout.Year > 2000 && b.TimeReturn == checkout))))
                  .Include(f => f.Excursion)
                  //.Include(f => f.Customers)
                  .Include(f => f.Leader)
                  .Include(f => f.Vehicle)
                  .ToListAsync);
            }
        }

        internal async Task<List<Hotel>> GetAllHotelsAsync<T>()
        {
            return await RunTask(Context.Hotels.OrderBy(h => h.Name)
                .Include(h => h.HotelCategory)
                .Include(h => h.City)
                .ToListAsync);
        }

        internal async Task<IEnumerable<OptionalExcursion>> GetAllOptionalExcursionsAsync(DateTime checkIn, bool track = true)
        {
            DateTime from = checkIn.AddDays(-6);
            DateTime to = checkIn.AddDays(6);
            if (!track)
            {
                return await RunTask(Context.OptionalExcursions
                .Where(o => o.Date == checkIn)
                .Include(o => o.Excursion)
                .AsNoTracking()
                .ToListAsync);
            }
            return await RunTask(Context.OptionalExcursions
                 .Where(o => o.Date == checkIn)
                 .Include(o => o.Excursion)
                 .ToListAsync);
        }

        internal async Task<List<CustomerOptional>> GetAllOptionalsAsync(int id)
        {
            return await Context.CustomerOptionals.Where(o => o.OptionalExcursion.Id == id)
                 .Include(o => o.Customer)
                 .Include(o => o.Customer.Reservation)
                 .Include(o => o.Customer.Reservation.Room.Hotel)
                 .Include(o => o.Leader)
                 .Include(o => o.OptionalExcursion)
                 .ToListAsync();
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
               .Where(f => (!f.Outgoing && f.Booking != null && f.Booking.Disabled == canceled) || (f.Personal_Booking != null && f.Personal_Booking.Disabled == canceled) || (f.ThirdParty_Booking != null && f.ThirdParty_Booking.Disabled == canceled))
               .ToListAsync);
        }

        internal async Task<List<Personal_Booking>> GetAllPersonalBookingsFiltered(int userId, bool completed, DateTime dateLimit, int partnerId = 0, int remainingPar = 0, DateTime checkin = new DateTime(), DateTime checkout = new DateTime(), bool canceled = false, string keyword = null, int id = 0, bool common = true, bool byCheckIn = true, bool onlyPartners = false)
        {
            var x = await RunTask(Context.Personal_Bookings
           .Where(c =>
           c.CreatedDate >= dateLimit &&
           c.Disabled == canceled &&
            (!onlyPartners || c.IsPartners) &&
           (userId == 0 || c.User.Id == userId) &&
           (partnerId == 0 || c.Partner.Id == partnerId) &&
           (remainingPar == 0 || (byCheckIn && c.Services.Any(s => s.TimeGo >= checkin) && c.Services.Any(s => s.TimeGo <= checkout) || (!byCheckIn && c.CreatedDate >= checkin && c.CreatedDate <= checkout))) &&
           (completed || c.Services.Any(s => s.TimeReturn >= DateTime.Today)) &&
           ((id > 0 && c.Id == id) || (id == 0 && (common || (c.IsPartners && c.Partner.Name.StartsWith(keyword)) || c.Customers.Any(p => p.Name.StartsWith(keyword) || p.Surename.StartsWith(keyword) || p.Tel.StartsWith(keyword))))))
           .Include(f => f.User)
           .Include(f => f.Services)
           .Include(f => f.Services)
           .Include(f => f.Partner)
           .Include(f => f.Payments)
           .Include(f => f.Customers)
           .ToListAsync);
            return x;
        }

        internal async Task<List<Reservation>> GetAllRemainingReservationsFiltered(int excursionId, int userId, int category, int par = 0, DateTime checkin = new DateTime(), DateTime checkout = new DateTime())
        {
            DateTime limit = new DateTime(2019, 04, 01);
            return await RunTask(Context.Reservations.Where(
                c => (category >= 0 ? (int)c.Booking.Excursion.ExcursionType.Category == category : true) &&
                  (excursionId == 0 || c.Booking.Excursion.Id == excursionId) &&
                  (userId == 0 || c.Booking.User.Id == userId) &&
                  (par == 0 || c.Booking.CheckIn >= checkin && c.Booking.CheckIn <= checkout) &&
                  (c.CreatedDate >= limit) &&
                  (StaticResources.User.BaseLocation == 1 || c.Booking.Partner.Id != 219) &&
                  !c.Booking.Disabled)
                  .Include(f => f.CustomersList)
                  .Include(f => f.Booking.User)
                  .Include(f => f.Booking.ExcursionDate)
                  .Include(f => f.Booking.Partner)
                  .Include(f => f.Booking.Payments)
                  .Include(f => f.Booking.Excursion)
                  .Include(f => f.Booking.Excursion.ExcursionType)
                  .Include(f => f.Booking.ExtraServices)
                  .ToListAsync);
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

        internal async Task<List<Reservation>> GetAllReservationsFiltered(int excursionId, int userId, bool completed, int category, DateTime dateLimit, bool checkInb, bool checkoutb, DateTime checkin, DateTime checkout, bool fromto, bool checkinout, bool canceled = false)
        {
            return await RunTask(Context.Reservations
                  .Where(c =>
                  c.Booking.ReservationsInBooking.Any(r => r.CreatedDate >= dateLimit) &&
                  (category >= 0 ? (int)c.Booking.Excursion.ExcursionType.Category == category : true) &&
                  (excursionId == 0 || c.Booking.Excursion.Id == excursionId) &&
                  (userId == 0 || c.Booking.User.Id == userId) &&
                  (!checkinout || ((!checkInb || c.Booking.CheckIn == checkin) && (!checkoutb || c.Booking.CheckOut == checkout))) &&
                  (!fromto || ((!checkInb || c.Booking.CheckIn >= checkin) && (!checkoutb || c.Booking.CheckIn <= checkout))) &&
                  (StaticResources.User.BaseLocation == 1 || c.Booking.Partner.Id != 219) &&
                  (completed || c.Booking.CheckOut >= DateTime.Today) &&
                  c.Booking.Disabled == canceled)
                  .Include(f => f.Booking)
                  .Include(f => f.CustomersList)
                  .Include(f => f.Booking.User)
                  .Include(f => f.Booking.Partner)
                  .Include(f => f.Booking.Excursion)
                  .Include(f => f.Booking.Excursion.ExcursionType)
                  .Include(f => f.Booking.Payments)
                  .Include(f => f.Booking.ExcursionDate)
                  .Include(f => f.Room.Hotel)
                  .Include(f => f.Room.RoomType)
                  .Include(f => f.Hotel)
                  .Include(f => f.Booking.ExtraServices)
                  .ToListAsync, 50);

            //return await RunTask(Context.Reservations
            //     .Where(c =>
            //     c.Booking.ReservationsInBooking.Any(r => r.CreatedDate >= dateLimit) &&
            //     (category >= 0 ? (int)c.Booking.Excursion.ExcursionType.Category == category : true) &&
            //     (excursionId == 0 || c.Booking.Excursion.Id == excursionId) &&
            //     (userId == 0 || c.Booking.User.Id == userId) &&
            //     (!checkinout || ((!checkInb || c.Booking.CheckIn == checkin) && (!checkoutb || c.Booking.CheckOut == checkout))) &&
            //     (!fromto || ((!checkInb || c.Booking.CheckIn >= checkin) && (!checkoutb || c.Booking.CheckIn <= checkout))) &&
            //     (StaticResources.User.BaseLocation == 1 || c.Booking.Partner.Id != 219) &&
            //     (completed || c.Booking.CheckOut >= DateTime.Today))
            //     .Include(f => f.Booking)
            //     .Include(f => f.Booking.User)
            //     .Include(f => f.Booking.ExcursionDate)
            //     .Include(f => f.Booking.Partner)
            //     .Include(f => f.Booking.Excursion)
            //     .Include(f => f.Booking.Excursion.ExcursionType)
            //     .Include(f => f.Booking.Payments)
            //     .Include(f => f.CustomersList.Select(c => new CustomerNeededValues
            //     {
            //         Name = c.Name,
            //         CheckIn = c.CheckIn,
            //         CheckOut = c.CheckOut,
            //         Price = c.Price,
            //         ReturnPlace = c.ReturningPlace,
            //         StartPlace = c.StartingPlace,
            //         Surename = c.Surename,
            //         Tel = c.Tel
            //     }))
            //     .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList.Select(c => new CustomerNeededValues
            //     {
            //         Name = c.Name,
            //         CheckIn = c.CheckIn,
            //         CheckOut = c.CheckOut,
            //         Price = c.Price,
            //         ReturnPlace = c.ReturningPlace,
            //         StartPlace = c.StartingPlace,
            //         Surename = c.Surename,
            //         Tel = c.Tel
            //     })))
            //     .Include(f => f.Room.Hotel)
            //     .Include(f => f.Room.RoomType)
            //     .Include(f => f.Hotel)
            //     .Where(b => b.Booking.Disabled == canceled)
            //     .ToListAsync, 50);
        }

        internal async Task<List<Reservation>> GetAllReservationsFilteredWithName(int excursionId, int userId, bool completed, int category, DateTime dateLimit, bool checkInb, bool checkoutb, DateTime checkin, DateTime checkout, bool fromto, bool checkinout, bool canceled = false)
        {
            return await RunTask(Context.Reservations
                  .Where(c =>
                  c.Booking.ReservationsInBooking.Any(r => r.CreatedDate >= dateLimit) &&
                  (category >= 0 ? (int)c.Booking.Excursion.ExcursionType.Category == category : true) &&
                  (excursionId == 0 || c.Booking.Excursion.Id == excursionId) &&
                  (userId == 0 || c.Booking.User.Id == userId) &&
                  (!checkinout || ((!checkInb || c.Booking.CheckIn == checkin) && (!checkoutb || c.Booking.CheckOut == checkout))) &&
                  (!fromto || ((!checkInb || c.Booking.CheckIn >= checkin) && (!checkoutb || c.Booking.CheckIn <= checkout))) &&
                  (StaticResources.User.BaseLocation == 1 || c.Booking.Partner.Id != 219) &&
                  (completed || c.Booking.CheckOut >= DateTime.Today))
                  .Include(f => f.Booking)
                  .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList))
                  .Include(f => f.Booking.User)
                  .Include(f => f.Booking.ExcursionDate)
                  .Include(f => f.Booking.Partner)
                  .Include(f => f.Booking.Excursion)
                  .Include(f => f.Booking.Excursion.ExcursionType)
                  .Include(f => f.Booking.Payments)
                  .Include(f => f.Room.Hotel)
                  .Include(f => f.Room.RoomType)
                  .Include(f => f.CustomersList)
                  .Include(f => f.Hotel)
                  .Include(f => f.Booking.ExtraServices)
                  .Where(b => b.Booking.Disabled == canceled)
                  .ToListAsync);
        }

        internal async Task<IEnumerable<Reservation>> GetAllReservationsForAddIncome(int grafeio, string keyword, DateTime datelimit, int id = 0, bool canceled = false)
        {
            return await RunTask(Context.Reservations.Where(c =>
                  c.Booking.ReservationsInBooking.Any(r => r.CreatedDate >= datelimit) &&
                  c.Booking.Disabled == canceled &&
                 (grafeio == 0 || c.Booking.User.BaseLocation == grafeio) &&
                ((id > 0 && c.Booking.Id == id) || (id == 0 && ((c.Booking.IsPartners && c.Booking.Partner.Name.StartsWith(keyword)) || c.CustomersList.Any(p => p.Name.StartsWith(keyword) || p.Surename.StartsWith(keyword) || p.Tel.StartsWith(keyword))))))
                  .Include(f => f.Booking.User)
                  .Include(f => f.Booking.ExcursionDate)
                  .Include(f => f.Booking.Partner)
                  .Include(f => f.Booking.Excursion)
                  .Include(f => f.Booking.Excursion.ExcursionType)
                  .Include(f => f.Booking.ReservationsInBooking.Select(i => i.CustomersList))
                  .Include(f => f.Room.Hotel)
                  .Include(f => f.Room.RoomType)
                  .Include(f => f.CustomersList)
                  .Include(f => f.Hotel)
                  .ToListAsync);
        }

        internal async Task<IEnumerable<Room>> GetAllRoomsFiltered(int cityId = -1, int hotelId = -1, int roomTypeId = -1, DateTime checkIn = default, DateTime checkOut = default)
        {
            return await RunTask(Context.Rooms
               .Where(r =>
               (cityId == -1 || r.Hotel.City.Id == cityId) &&
               (hotelId == -1 || r.Hotel.Id == hotelId) &&
               (roomTypeId == -1 || r.RoomType.Id == roomTypeId) &&
               r.DailyBookingInfo.Any(t => t.Date >= checkIn && t.Date <= checkOut))
               .Include(f => f.Hotel)
               .Include(f => f.Hotel.HotelCategory)
               .Include(f => f.Hotel.City)
               .Include(f => f.DailyBookingInfo)
               .Include(f => f.RoomType)
               .ToListAsync);
        }

        internal async Task<List<ThirdParty_Booking>> GetAllThirdPartyBookingsFiltered(int userId, bool completed, DateTime dateLimit, int par = 0, DateTime checkin = new DateTime(), DateTime checkout = new DateTime(), bool canceled = false, string keyword = null, int id = 0, bool common = true, bool byCheckIn = true)
        {
            var x = await RunTask(Context.ThirdParty_Bookings
                .Where(c =>
                c.CreatedDate >= dateLimit &&
                (userId == 0 || c.User.Id == userId) &&
                (par == 0 || (byCheckIn && c.CheckIn >= checkin && c.CheckIn <= checkout) || (!byCheckIn && c.CreatedDate >= checkin && c.CreatedDate <= checkout)) &&
                (completed || c.CheckIn >= DateTime.Today) &&
                ((id > 0 && c.Id == id) || (id == 0 && (common || c.Partner.Name.StartsWith(keyword) || c.Customers.Any(p => p.Name.StartsWith(keyword) || p.Surename.StartsWith(keyword) || p.Tel.StartsWith(keyword))))) && c.Disabled == canceled)
                .Include(f => f.Customers)
                .Include(f => f.User)
                .Include(f => f.Payments)
                .Include(f => f.Partner)
                .Include(f => f.File)
                .ToListAsync);
            return x;
        }

        internal async Task<List<Transaction>> GetAllTransactionsFiltered(Transaction transaction = null, Excursion excursion = null, Bus bus = null, User user = null, DateTime? from = null, DateTime? to = null)
        {
            bool tranfilter = transaction != null;
            int busid = bus != null ? bus.Id : 0;
            int excid = excursion != null ? excursion.Id : 0;
            int userid = user != null ? user.Id : 0;

            try
            {
                if (tranfilter)
                {
                    return await RunTask(Context.Transactions
                    .Where(t =>
                    t.TransactionType == transaction.TransactionType &&
                    (transaction.IncomeBaseCategory == IncomeBaseCategories.None || t.IncomeBaseCategory == transaction.IncomeBaseCategory) &&
                    (transaction.ExpenseBaseCategory == ExpenseBaseCategories.None || t.ExpenseBaseCategory == transaction.ExpenseBaseCategory) &&
                    (transaction.GroupExpenseCategory == GroupExpenseCategories.None || t.GroupExpenseCategory == transaction.GroupExpenseCategory) &&
                    (transaction.ExcursionExpenseCategory == ExcursionExpenseCategories.Total || t.ExcursionExpenseCategory == transaction.ExcursionExpenseCategory) &&
                    (transaction.GeneralOrDatesExpenseCategory == GeneralOrDatesExpenseCategories.Total || t.GeneralOrDatesExpenseCategory == transaction.GeneralOrDatesExpenseCategory) &&
                    (transaction.StandardExpenseCategory == StandardExpenseCategories.None || t.StandardExpenseCategory == transaction.StandardExpenseCategory) &&
                    (transaction.TaxExpenseCategory == TaxExpenseCategories.None || t.TaxExpenseCategory == transaction.TaxExpenseCategory) &&
                    (busid == 0 || t.SelectedBus.Id == busid) &&
                    (excid == 0 || t.Excursion.Id == excid) &&
                    (userid == 0 || t.User.Id == userid) &&
                    (from == null || t.Date >= from) &&
                    (to == null || t.Date <= to))
                    .Include(t => t.User)
                    .Include(t => t.Booking)
                    .Include(t => t.Booking.ReservationsInBooking.Select(r => r.CustomersList))
                    .Include(t => t.PersonalBooking)
                    .Include(t => t.PersonalBooking.Customers)
                    .Include(t => t.ThirdPartyBooking.Customers)
                    .Include(t => t.Excursion)
                    .Include(t => t.PaymentTo)
                    .Include(t => t.SelectedBus.Vehicle)
                    .Include(t => t.SelectedBus.Leader)
                    .Include(t => t.ThirdPartyBooking)
                    .ToListAsync);
                }
                else
                {
                    var x = await RunTask(Context.Transactions
                         .Where(t =>
                         (busid == 0 || t.SelectedBus.Id == busid) &&
                         (excid == 0 || t.Excursion.Id == excid) &&
                         (userid == 0 || t.User.Id == userid) &&
                         (from == null || t.Date >= from) &&
                         (to == null || t.Date <= to))
                         .Include(t => t.User)
                         .Include(t => t.Booking)
                         .Include(t => t.Booking.ReservationsInBooking.Select(r => r.CustomersList))
                         .Include(t => t.PersonalBooking)
                         .Include(t => t.PersonalBooking.Customers)
                         .Include(t => t.ThirdPartyBooking.Customers)
                         .Include(t => t.Excursion)
                         .Include(t => t.PaymentTo)
                         .Include(t => t.SelectedBus.Vehicle)
                         .Include(t => t.SelectedBus.Leader)
                         .Include(t => t.ThirdPartyBooking)
                         .ToListAsync);
                    return x;
                }
            }
            catch (Exception ex)
            {
                // MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            return null;
        }

        internal async Task<ThirdParty_Booking> GetFullThirdPartyBookingByIdAsync(int id)
        {
            return await RunTask(Context.ThirdParty_Bookings.Where(b => b.Id == id)
               .Include(f => f.User)
               .Include(f => f.Payments)
               .Include(f => f.Payments.Select(p => p.User))
               .Include(f => f.Customers)
               .Include(f => f.Partner)
               .Include(f => f.File)
               .FirstOrDefaultAsync);
        }

        internal async Task<List<string>> GetNotifications()
        {
            //DateTime limit = DateTime.Today;
            //return await RunTask(Context.Customers.Where(c => c.Tel.Length > 9 && !c.Tel.StartsWith("000") && c.Reservation != null && c.Reservation.Booking.Partner == null && c.Reservation.Booking.User.BaseLocation == 1 && c.Reservation.Booking.CreatedDate > limit)
            //     .Where(r => r.Reservation.Booking.Disabled == false)
            //     .ToListAsync);
            return null;
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

        internal List<Bus> LoadBuses()
        {
            return Context.Set<Bus>()
                 .Include(b => b.Customers)
                 .Include(b => b.Excursion)
                 .Include(b => b.Leader)
                 .Include(b => b.Vehicle)
                 .ToList();
        }

        internal void Save()
        {
            Context.SaveChanges();
        }

        protected virtual void Dispose(bool b)
        {
            if (b)
            {
                Context.Dispose();
                GC.SuppressFinalize(this);
                return;
            }
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
                            else if (original is string s3 && string.IsNullOrEmpty(s3))
                            {
                                sb.Append($"Νέο σχόλιο κράτησης '{current.ToString()}', ");
                            }
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

                        case nameof(Customer.Board):
                            sb.Append($"Διατροφή  του '{c}' από '{GetBoard((int)original)}' σε '{GetBoard(c.Board)}', ");
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
                    }
                }
            }
            ObjectStateEntry objectStateEntry = ((IObjectContextAdapter)Context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }

        private void RejectNavigationChanges()
        {
            ObjectContext objectContext = ((IObjectContextAdapter)Context).ObjectContext;
            IEnumerable<ObjectStateEntry> deletedRelationships = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).Where(e => e.IsRelationship && !RelationshipContainsKeyEntry(e));
            IEnumerable<ObjectStateEntry> addedRelationships = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(e => e.IsRelationship);

            foreach (ObjectStateEntry relationship in addedRelationships)
                relationship.Delete();

            foreach (ObjectStateEntry relationship in deletedRelationships)
                relationship.ChangeState(EntityState.Unchanged);
        }

        private bool RelationshipContainsKeyEntry(ObjectStateEntry stateEntry)
        {
            //prevent exception: "Cannot change state of a relationship if one of the ends of the relationship is a KeyEntry"
            //I haven't been able to find the conditions under which this happens, but it sometimes does.
            ObjectContext objectContext = ((IObjectContextAdapter)Context).ObjectContext;
            object[] keys = new[] { stateEntry.OriginalValues[0], stateEntry.OriginalValues[1] };
            return keys.Any(key => objectContext.ObjectStateManager.GetObjectStateEntry(key).Entity == null);
        }

        private async Task<T> RunTask<T>(Func<Task<T>> task, int limit = 0)
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
                if (limit > 0)
                {
                    t.Wait(limit * 1000);
                }
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