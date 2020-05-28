﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using LATravelManager.DataAccess.Migrations;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Plan;
using LATravelManager.Model.Services;

namespace LATravelManager.DataAccess
{
    // Code-Based Configuration and Dependency resolution
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MainDatabase : DbContext
    {
        #region Constructors

        public MainDatabase() : base(Properties.Settings.Default.IsTest ? test : normal)
        // public MainDatabase() : base(test)
        {
            Configuration.ValidateOnSaveEnabled = false;

            DbConfiguration.SetConfiguration(new ContextConfiguration());
        }

        #endregion Constructors

        #region Fields

        private const string normal = "Server=server19.cretaforce.gr;Database=readmore_achill2;pooling=true;Uid=readmore_achill2;Pwd=986239787346;Convert Zero Datetime=True; CharSet=utf8; default command timeout=3600;SslMode=none;";
        private const string test = "Server=readmoreachill2.clq6srsguoz6.eu-west-3.rds.amazonaws.com;Database=readmore_achill2;pooling=true;Uid=readmore_achill2;Pwd=986239787346;Convert Zero Datetime=True;  default command timeout=3600;SslMode=none;TreatTinyAsBoolean=true;";

        #endregion Fields

        #region Properties

        public DbSet<Airline> Airlines { get; set; }
        public DbSet<BookingInfoPerDay> BookingInfosPerDay { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<HotelService> HotelServices { get; set; }
        public DbSet<PlaneService> PlaneServices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOptional> CustomerOptionals { get; set; }
        public DbSet<ExcursionCategory> ExcursionCategories { get; set; }
        public DbSet<Excursion> Excursions { get; set; }
        public DbSet<HotelCategory> HotelCategories { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Leader> Leaders { get; set; }
        public DbSet<OptionalExcursion> OptionalExcursions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Personal_Booking> Personal_Bookings { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<StartingPlace> StartingPlaces { get; set; }
        public DbSet<ThirdParty_Booking> ThirdParty_Bookings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<string>()
                .Configure(s => s.HasMaxLength(200).HasColumnType("varchar"));
            modelBuilder.Properties<bool>().Configure(c => c.HasColumnType("bit"));
            //Database.Connection.Open();
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(18, 2));
            base.OnModelCreating(modelBuilder);
            // SetExecutionStrategy(MySqlProviderInvariantName.ProviderName, () => new MySqlExecutionStrategy());
            //modelBuilder.Properties<TimeSpan>().Configure(c => c.HasColumnType("time"));
        }

        public void ToggleTestMode(bool isTest)
        {
            Properties.Settings.Default.IsTest = !isTest;
            Properties.Settings.Default.Save();
        }

        #endregion Methods
    }
}