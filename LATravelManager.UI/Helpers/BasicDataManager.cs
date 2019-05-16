using GalaSoft.MvvmLight;
using LATravelManager.Model;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.UI.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.Helpers
{
    public class BasicDataManager : ViewModelBase
    {
        #region Constructors

        public BasicDataManager(GenericRepository genericRepository)
        {
            Context = genericRepository;
        }

        public async Task Refresh()
        {
            Context = new GenericRepository();
            await LoadAsync();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<City> _Cities;
        private ObservableCollection<Country> _Countries;
        private ObservableCollection<ExcursionCategory> _ExcursionCategories;
        private ObservableCollection<Excursion> _Excursions;
        private ObservableCollection<HotelCategory> _HotelCategories;
        private ObservableCollection<Hotel> _Hotels;
        private ObservableCollection<Partner> _Partners;

        private ObservableCollection<RoomType> _RoomTypes;

        private ObservableCollection<StartingPlace> _StartingPlaces;

        private ObservableCollection<User> _Users;

        public GenericRepository Context { get; set; }

        #endregion Fields

        #region Properties

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

        #endregion Properties

        #region Methods

        public bool HasChanges()
        {
            return Context.HasChanges();
        }

        public async Task LoadAsync()
        {
            Cities = new ObservableCollection<City>(await Context.GetAllCitiesAsyncSortedByName());
            Countries = new ObservableCollection<Country>(await Context.GetAllAsyncSortedByName<Country>());
            RoomTypes = new ObservableCollection<RoomType>(await Context.GetAllAsync<RoomType>());
            StartingPlaces = new ObservableCollection<StartingPlace>(await Context.GetAllAsyncSortedByName<StartingPlace>());
            Users = new ObservableCollection<User>(await Context.GetAllUsersAsyncSortedByUserName());
            ExcursionCategories = new ObservableCollection<ExcursionCategory>((await Context.GetAllAsync<ExcursionCategory>()).OrderBy(e => e.IndexNum));
            Excursions = new ObservableCollection<Excursion>((await Context.GetAllGroupExcursionsAsync()).OrderBy(ex => ex, new ExcursionComparer()));
            Partners = new ObservableCollection<Partner>(await Context.GetAllAsyncSortedByName<Partner>());
            HotelCategories = new ObservableCollection<HotelCategory>(await Context.GetAllAsync<HotelCategory>());
            Hotels = new ObservableCollection<Hotel>(await Context.GetAllAsyncSortedByName<Hotel>());
            GroupExcursions = new ObservableCollection<Excursion>(Excursions.Where(e => e.ExcursionType.Category == Enums.ExcursionTypeEnum.Group));
        }

        internal void Add<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            Context.Add(model);
        }

        internal void Delete<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            Context.Delete(model);
        }

        internal async Task SaveAsync()
        {
            await Context.SaveAsync();
        }

        #endregion Methods
    }
}