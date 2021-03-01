using GalaSoft.MvvmLight;
using LATravelManager.Model;
using LATravelManager.Model.DTOS;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LATravelManager.UI.Helpers
{
    public class BasicDataManager : ViewModelBase
    {
        #region Constructors

        public BasicDataManager(GenericRepository genericRepository)
        {
            Context = genericRepository;
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Airline> _Airlines;

        private ObservableCollection<Bus> _Buses;

        private ObservableCollection<City> _Cities;

        private ObservableCollection<Country> _Countries;

        private ObservableCollection<ExcursionCategory> _ExcursionCategories;

        private ObservableCollection<Excursion> _Excursions;

        private ObservableCollection<HotelCategory> _HotelCategories;

        private ObservableCollection<Hotel> _Hotels;

        private ObservableCollection<Leader> _Leaders;

        private ObservableCollection<Partner> _Partners;

        private ObservableCollection<RoomType> _RoomTypes;

        private ObservableCollection<StartingPlace> _StartingPlaces;

        private ObservableCollection<User> _Users;

        private ObservableCollection<Vehicle> _Vehicles;

        #endregion Fields

        #region Properties

        public ObservableCollection<Airline> Airlines
        {
            get
            {
                return _Airlines;
            }

            set
            {
                if (_Airlines == value)
                {
                    return;
                }

                _Airlines = value;
                RaisePropertyChanged();
            }
        }

        internal async Task GetAllPrices()
        {
            await Context.UpdatePrices();
        }

        public ObservableCollection<Bus> Buses
        {
            get
            {
                return _Buses;
            }

            set
            {
                if (_Buses == value)
                {
                    return;
                }

                _Buses = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<City> Cities
        {
            get
            {
                return _Cities;
            }

            set
            {
                if (_Cities == value)
                {
                    return;
                }

                _Cities = value;
                RaisePropertyChanged();
            }
        }

        public GenericRepository Context
        {
            get;
            set;
        }

        public ObservableCollection<Country> Countries
        {
            get
            {
                return _Countries;
            }

            set
            {
                if (_Countries == value)
                {
                    return;
                }

                _Countries = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ExcursionCategory> ExcursionCategories
        {
            get
            {
                return _ExcursionCategories;
            }

            set
            {
                if (_ExcursionCategories == value)
                {
                    return;
                }

                _ExcursionCategories = value;
                RaisePropertyChanged();
            }
        }

        internal async Task ToggleTestMode(bool isTest)
        {
            Context.Context.ToggleTestMode(isTest);
            Properties.Settings.Default.isTest = !isTest;
            Properties.Settings.Default.Save();
            await Refresh();
        }

        public ObservableCollection<Excursion> Excursions
        {
            get
            {
                return _Excursions;
            }

            set
            {
                if (_Excursions == value)
                {
                    return;
                }

                _Excursions = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Excursion> GroupExcursions { get; set; }

        public ObservableCollection<HotelCategory> HotelCategories
        {
            get
            {
                return _HotelCategories;
            }

            set
            {
                if (_HotelCategories == value)
                {
                    return;
                }

                _HotelCategories = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Hotel> Hotels
        {
            get
            {
                return _Hotels;
            }

            set
            {
                if (_Hotels == value)
                {
                    return;
                }

                _Hotels = value;
                RaisePropertyChanged();
            }
        }

        public bool IsContextAvailable => Context.IsContextAvailable;

        public ObservableCollection<Leader> Leaders
        {
            get
            {
                return _Leaders;
            }

            set
            {
                if (_Leaders == value)
                {
                    return;
                }

                _Leaders = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Partner> Partners
        {
            get
            {
                return _Partners;
            }

            set
            {
                if (_Partners == value)
                {
                    return;
                }

                _Partners = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<RoomType> RoomTypes
        {
            get
            {
                return _RoomTypes;
            }

            set
            {
                if (_RoomTypes == value)
                {
                    return;
                }

                _RoomTypes = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StartingPlace> StartingPlaces
        {
            get
            {
                return _StartingPlaces;
            }

            set
            {
                if (_StartingPlaces == value)
                {
                    return;
                }

                _StartingPlaces = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<User> Users
        {
            get
            {
                return _Users;
            }

            set
            {
                if (_Users == value)
                {
                    return;
                }

                _Users = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get
            {
                return _Vehicles;
            }

            set
            {
                if (_Vehicles == value)
                {
                    return;
                }

                _Vehicles = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public bool HasChanges()
        {
            return Context.HasChanges();
        }

        private ObservableCollection<OptionalExcursion> _OptionalExcursions;

        public ObservableCollection<OptionalExcursion> OptionalExcursions
        {
            get
            {
                return _OptionalExcursions;
            }

            set
            {
                if (_OptionalExcursions == value)
                {
                    return;
                }

                _OptionalExcursions = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Hotels = new ObservableCollection<Hotel>(await Context.GetAllHotelsAsync<Hotel>());
            Countries = new ObservableCollection<Country>(await Context.GetAllAsyncSortedByName<Country>());
            Cities = new ObservableCollection<City>(await Context.GetAllCitiesAsyncSortedByName());
            RoomTypes = new ObservableCollection<RoomType>((await Context.GetAllAsync<RoomType>()).OrderBy(r => r.MaxCapacity));
            StartingPlaces = new ObservableCollection<StartingPlace>(await Context.GetAllAsyncSortedByName<StartingPlace>());
            StaticResources.StartingPlaces = StartingPlaces;
            Users = new ObservableCollection<User>(await Context.GetAllUsersAsyncSortedByUserName());
            ExcursionCategories = new ObservableCollection<ExcursionCategory>((await Context.GetAllAsync<ExcursionCategory>()).OrderBy(e => e.IndexNum));
            Excursions = new ObservableCollection<Excursion>((await Context.GetAllExcursionsAsync()).OrderBy(ex => ex, new ExcursionComparer()));
            foreach (Excursion excursion in Excursions.Where(e => e.ExcursionDates.Any(ed => ed.CheckOut >= DateTime.Today)))
            {
                excursion.ExcursionDates = new ObservableCollection<ExcursionDate>(excursion.ExcursionDates.OrderBy(t => t.CheckIn));
            }
            Partners = new ObservableCollection<Partner>(await Context.GetAllAsyncSortedByName<Partner>());
            HotelCategories = new ObservableCollection<HotelCategory>(await Context.GetAllAsync<HotelCategory>());
            GroupExcursions = new ObservableCollection<Excursion>(Excursions.Where(e => e.ExcursionType.Category == ExcursionTypeEnum.Group));
            Airlines = new ObservableCollection<Airline>(await Context.GetAllAsyncSortedByName<Airline>());
            Vehicles = new ObservableCollection<Vehicle>(await Context.GetAllAsync<Vehicle>());
            Leaders = new ObservableCollection<Leader>(await Context.GetAllAsync<Leader>());
            OptionalExcursions = new ObservableCollection<OptionalExcursion>(await Context.GetAllAsync<OptionalExcursion>(o => o.Date >= DateTime.Today));
            //List<CompanyDto> t = await Context.Context.Companies.Where(c => !c.Disabled).Select(c => new CompanyDto { CompanyName = c.CompanyName, Id = c.Id }).ToListAsync();

            await Context.SaveAsync();

            Mouse.OverrideCursor = Cursors.Arrow;
            //DateTime from = new DateTime(2019, 12, 18);
            //DateTime to = new DateTime(2020, 01, 1);

            //var t = Context.Context.BookingInfosPerDay.Where(i => i.Room.Hotel.Id == 24 && i.Room.RoomType.Id == 1 && i.Date >= from && i.Date <= to);

            //foreach (BookingInfoPerDay item in t)
            //{
            //    Context.Delete(item);

            //}
            //await Context.SaveAsync();

            //int counter = 0;
            //IEnumerable<Booking> boo = await Context.GetAllBookingsAsync();
            //foreach (var item in boo)
            //{
            //    if (item.Id == 4370)
            //    {
            //        foreach (var r in item.ReservationsInBooking)
            //        {
            //            foreach (var c in r.CustomersList)
            //            {
            //                if (counter < 28)
            //                {
            //                    c.Price = 0.1m;
            //                    counter++;
            //                }
            //                else
            //                    c.Price = 60;
            //            }
            //        }
            //    }
            //}
            ////foreach (var b in boo)
            //{
            //    foreach (var r in b.ReservationsInBooking)
            //    {
            //        if (r.HB || (r.Room != null && r.Room.Hotel.Id == 39))
            //        {
            //            r.HB = true;
            //            foreach (var c in r.CustomersList)
            //            {
            //                if (c.Board == 0)
            //                {
            //                c.Board = 1;
            //                }

            //            }

            //        }
            //    }
            //}
            //await Context.SaveAsync();

            //booking await DoTempStuff();

            //foreach (var e in Excursions)
            //{
            //    if (e.ExcursionType.Category != ExcursionTypeEnum.Personal && e.ExcursionType.Category != ExcursionTypeEnum.ThirdParty && !e.IncludesPlane && e.ExcursionDates.Any(ed => ed.CheckOut > DateTime.Today))
            //    {
            //        foreach (var s in StartingPlaces)
            //        {
            //            e.ExcursionTimes.Add(new ExcursionTime { StartingPlace = s });
            //        }
            //    }
            //}
        }

        //private async Task DoTempStuff()
        //{
        //    var allpartnersBookings = await Context.GetAllBookingsAsyncP();
        //    var custs = allpartnersBookings.Select(r => new BookingWrapper(r)).Sum(t => t.Customers.Count);
        //    foreach (var b in allpartnersBookings.Select(r => new BookingWrapper(r)))
        //    {
        //        b.CalculateRemainingAmount();
        //        b.Calculating = true;
        //        if (b.FullPrice >= 0 && b.Customers.Count > 0 && Math.Abs(b.FullPrice - b.Customers.Sum(i => i.Price)) > 0.30m)
        //        {
        //            decimal tmpPrice = Math.Round(b.FullPrice / b.Customers.Count, 2);
        //            foreach (CustomerWrapper customer in b.Customers)
        //                customer.Price = tmpPrice;
        //        }
        //        b.Calculating = false;
        //    }

        //    await Context.SaveAsync();
        //}

        public async Task LoadPersonal()
        {
            Hotels = new ObservableCollection<Hotel>(await Context.GetAllHotelsAsync<Hotel>());
            Countries = new ObservableCollection<Country>(await Context.GetAllAsyncSortedByName<Country>());
            Cities = new ObservableCollection<City>(await Context.GetAllCitiesAsyncSortedByName());
            RoomTypes = new ObservableCollection<RoomType>((await Context.GetAllAsync<RoomType>()).OrderBy(r => r.MaxCapacity));
            StartingPlaces = new ObservableCollection<StartingPlace>(await Context.GetAllAsyncSortedByName<StartingPlace>());
            StaticResources.StartingPlaces = StartingPlaces;
            Users = new ObservableCollection<User>(await Context.GetAllUsersAsyncSortedByUserName());
            Partners = new ObservableCollection<Partner>(await Context.GetAllAsyncSortedByName<Partner>());
            HotelCategories = new ObservableCollection<HotelCategory>(await Context.GetAllAsync<HotelCategory>());
            Airlines = new ObservableCollection<Airline>(await Context.GetAllAsyncSortedByName<Airline>());
        }

        public async Task Refresh()
        {
            Context = new GenericRepository();
            await LoadAsync();
        }

        internal void Add<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            Context.Add(model);
            if (model is Hotel h)
            {
                Hotels.Add(h);
                Hotels = new ObservableCollection<Hotel>(Hotels.OrderBy(m => m.Name));
            }
            else if (model is Partner p)
            {
                Partners.Add(p);
                Partners = new ObservableCollection<Partner>(Partners.OrderBy(m => m.Name));
            }
            else if (model is City c)
            {
                Cities.Add(c);
                Cities = new ObservableCollection<City>(Cities.OrderBy(m => m.Name));
            }
            else if (model is Country co)
            {
                Countries.Add(co);
                Countries = new ObservableCollection<Country>(Countries.OrderBy(m => m.Name));
            }
            else if (model is Excursion e)
            {
                Excursions.Add(e);
                if (e.ExcursionType.Category == ExcursionTypeEnum.Group)
                {
                    GroupExcursions.Add(e);
                    GroupExcursions = new ObservableCollection<Excursion>(GroupExcursions.OrderBy(m => m.Name));
                }
                Excursions = new ObservableCollection<Excursion>(Excursions.OrderBy(m => m.Name));
            }
            else if (model is User u)
            {
                Users.Add(u);
                Users = new ObservableCollection<User>(Users.OrderBy(m => m.Name));
            }
        }

        internal void Delete<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            Context.Delete(model);
            if (model is Hotel h)
            {
                Hotels.Remove(h);
            }
            else if (model is Partner p)
            {
                Partners.Remove(p);
            }
            else if (model is City c)
            {
                Cities.Remove(c);
            }
            else if (model is Country co)
            {
                Countries.Remove(co);
            }
            else if (model is Excursion e)
            {
                Excursions.Remove(e);
            }
            else if (model is User u)
            {
                Users.Remove(u);
            }
        }

        internal async Task SaveAsync()
        {
            await Context.SaveAsync();
        }

        #endregion Methods
    }
}