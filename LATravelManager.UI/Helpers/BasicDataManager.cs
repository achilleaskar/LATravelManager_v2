using GalaSoft.MvvmLight;
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
        public BasicDataManager()
        {
        }






        private ObservableCollection<StartingPlace> _StartingPlaces;


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

        private GenericRepository _Context;

        public GenericRepository Context
        {
            get
            {
                return _Context;
            }

            set
            {
                if (_Context == value)
                {
                    return;
                }

                _Context = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            ExcursionCategories = new ObservableCollection<ExcursionCategory>((await Context.GetAllAsync<ExcursionCategory>()).OrderBy(e => e.IndexNum));
        }

        private ObservableCollection<ExcursionCategory> _ExcursionCategories;

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

        private ObservableCollection<User> _Users;

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

        private ObservableCollection<Excursion> _Excursions;

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

        private ObservableCollection<City> _Cities;

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

        private ObservableCollection<Country> _Countries;

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

        private ObservableCollection<Hotel> _Hotels;

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

        private ObservableCollection<RoomType> _RoomTypes;

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

        public async Task ReloadAsync()
        {
        }
    }
}