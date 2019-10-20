using GalaSoft.MvvmLight.CommandWpf;
using LaTravelManager.ViewModel.Management;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Services;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace LATravelManager.UI.ViewModel.ServiceViewModels
{
    public class Hotel_ViewModel : ServiceViewModel
    {
        public Hotel_ViewModel(NewReservation_Personal_ViewModel parent) : base(parent)
        {
            this.PropertyChanged += Hotel_ViewModel_PropertyChanged;
            Service = new HotelService();
            OpenHotelEditCommand = new RelayCommand(OpenHotelsWindow);
            Refresh();
        }

        private void Hotel_ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Service is HotelService && e.PropertyName == nameof(Service))
            {
                Service.PropertyChanged += Service_PropertyChanged;
                if (HotelsCv != null)
                    HotelsCv.Refresh();
            }
        }

        private void Service_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Service is HotelService && e.PropertyName == "City")
            {
                if (HotelsCv != null)
                    HotelsCv.Refresh();
            }
            if (sender is HotelService hs && e.PropertyName == nameof(hs.TimeGo) && hs.TimeGo.Year > 2000)
            {
                hs.Option = hs.TimeGo.AddDays(-10);
            }
        }

        private void OpenHotelsWindow()
        {
            HotelsManagement_ViewModel vm = new HotelsManagement_ViewModel(Parent.MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new HotelsManagement_Window { DataContext = vm }));
            HotelsList = new ObservableCollection<Hotel>(Parent.MainViewModel.BasicDataManager.Hotels);
        }

        public RelayCommand OpenHotelEditCommand { get; }

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
                HotelsCv = CollectionViewSource.GetDefaultView(HotelsList);
                HotelsCv.Filter = HotelsFilter;
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
                if (_Hotels != null)
                    _Hotels.Refresh();
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<City> _Cities;

        private ICollectionView _HotelsCv;

        public ICollectionView HotelsCv
        {
            get
            {
                return _HotelsCv;
            }

            set
            {
                if (_HotelsCv == value)
                {
                    return;
                }

                _HotelsCv = value;
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
            return Service is HotelService hs && hs.City != null && item is Hotel ho && ho.City.Id == hs.City.Id;
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