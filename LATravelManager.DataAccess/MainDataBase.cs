using LATravelManager.Models;
using MySql.Data.Entity;
using System;
using System.Data.Entity;

namespace LATravelManager.DataAccess
{
    // Code-Based Configuration and Dependency resolution
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MainDatabase : DbContext
    {
        #region Constructors

        public MainDatabase() : base("LADatabase")
        {
            Configuration.ValidateOnSaveEnabled = false;
        }
        #endregion Constructors

        #region Properties

        public DbSet<BookingInfoPerDay> BookingInfosPerDay { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Bus> Bus { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ExcursionCategory> ExcursionCategories { get; set; }
        public DbSet<Excursion> Excursions { get; set; }
        public DbSet<HotelCategory> HotelCategories { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Leader> Leaders { get; set; }
        public DbSet<OptionalExcursion> OptionalExcursions { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<StartingPlace> StartingPlaces { get; set; }
        public DbSet<User> Users { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>()
                .Configure(s => s.HasMaxLength(200).HasColumnType("varchar"));
            //Database.Connection.Open();
            base.OnModelCreating(modelBuilder);
            // SetExecutionStrategy(MySqlProviderInvariantName.ProviderName, () => new MySqlExecutionStrategy());
        }

        public object GetStartingPlace<T>(Func<object, bool> filter)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}