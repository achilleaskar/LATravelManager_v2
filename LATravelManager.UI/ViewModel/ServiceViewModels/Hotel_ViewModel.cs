using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Services;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace LATravelManager.UI.ViewModel.ServiceViewModels
{
    public class Hotel_ViewModel : ServiceViewModel
    {
        public Hotel_ViewModel(NewReservation_Personal_ViewModel parent) : base(parent)
        {
            Service = new HotelService();
            Refresh();
        }

        public override void Refresh()
        {
            Cities = Parent.BasicDataManager.Cities;
            RoomTypes = Parent.BasicDataManager.RoomTypes;
            HotelsList = new ObservableCollection<Hotel>(Parent.BasicDataManager.Hotels);
            Hotels = (CollectionView)CollectionViewSource.GetDefaultView(HotelsList);
        }

        private ObservableCollection<Hotel> _HotelsList;

        public ObservableCollection<Hotel> HotelsList
        {
            get
            {
                return _HotelsList;
            }

            set
            {
                if (_HotelsList == value)
                {
                    return;
                }

                _HotelsList = value;
                RaisePropertyChanged();
            }
        }

        private ICollectionView _Hotels;

        public ICollectionView Hotels
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
                _Hotels.Filter = HotelsFilter;
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

        private City _City;
        private ObservableCollection<RoomType> _RoomTypes;

        public City City
        {
            get
            {
                return _City;
            }

            set
            {
                if (_City == value)
                {
                    return;
                }

                _City = value;
                (Service as HotelService).City = value;

                Hotels.Refresh();
                RaisePropertyChanged();
            }
        }

        public bool HotelsFilter(object item)
        {
            if (City == null)
                return true;
            return (item as Hotel).City.Id == City.Id;
        }

        public ObservableCollection<RoomType> RoomTypes
        {
            get => _RoomTypes;

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
    }
}