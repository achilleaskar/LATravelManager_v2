using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class Counter : ViewModelBase
    {
        #region Fields

        private int _Handled;
        private string _Name;

        private int _Total;

        #endregion Fields

        #region Properties

        public int Handled
        {
            get
            {
                return _Handled;
            }

            set
            {
                if (_Handled == value)
                {
                    return;
                }

                _Handled = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged();
            }
        }

        public int Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }

    public class ListManagement_ViewModel : MyViewModelBase
    {
        #region Constructors

        public ListManagement_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ShowCustomersCommand = new RelayCommand(async () => { await ShowCustomers(); }, () => SelectedExcursion != null);

            Load();
        }

        #endregion Constructors

        #region Fields

        private List<BookingWrapper> _AllBookings;
        private List<Counter> _Cities;
        private GenericRepository _Context;
        private int _DepartmentIndexBookingFilter;
        private ObservableCollection<Excursion> _Excursions;
        private List<Counter> _Hotels;
        private bool _MyProperty;
        private Excursion _SelectedExcursion;

        #endregion Fields

        #region Properties

        public List<BookingWrapper> AllBookings
        {
            get
            {
                return _AllBookings;
            }

            set
            {
                if (_AllBookings == value)
                {
                    return;
                }

                _AllBookings = value;
                RaisePropertyChanged();
            }
        }

        public List<Counter> Cities
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

        public int DepartmentIndexBookingFilter
        {
            get
            {
                return _DepartmentIndexBookingFilter;
            }

            set
            {
                if (_DepartmentIndexBookingFilter == value)
                {
                    return;
                }

                _DepartmentIndexBookingFilter = value;
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

        public List<Counter> Hotels
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

        public MainViewModel MainViewModel { get; }

        public bool MyProperty
        {
            get
            {
                return _MyProperty;
            }

            set
            {
                if (_MyProperty == value)
                {
                    return;
                }

                _MyProperty = value;
                RaisePropertyChanged();
            }
        }

        public Excursion SelectedExcursion
        {
            get
            {
                return _SelectedExcursion;
            }

            set
            {
                if (_SelectedExcursion == value)
                {
                    return;
                }

                _SelectedExcursion = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowCustomersCommand { get; set; }

        #endregion Properties

        #region Methods

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                Context = new GenericRepository();

                Excursions = new ObservableCollection<Excursion>(MainViewModel.BasicDataManager.Excursions.Where(c => c.Id > 0 && c.ExcursionDates.Any(e => e.CheckOut > DateTime.Now)).OrderBy(e => e.FirstDate));
                Hotels = new List<Counter>();
                Cities = new List<Counter>();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                IsLoaded = true;
            }
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private async Task ShowCustomers()
        {
            AllBookings = (await Context.GetAllBookingsForLists(SelectedExcursion.Id)).Select(b => new BookingWrapper(b)).ToList();
            Cities.Clear();
            Hotels.Clear();

            Counter tmpHotel;
            Counter tmpCity;

            foreach (BookingWrapper b in AllBookings)
            {
                foreach (ReservationWrapper r in b.ReservationsInBooking.Select(r => new ReservationWrapper(r)))
                {
                    tmpHotel = Hotels.Where(h => h.Name.Equals(r.HotelName)).FirstOrDefault();
                    if (tmpHotel == null)
                    {
                        tmpHotel = new Counter { Name = r.HotelName };
                        Hotels.Add(tmpHotel);
                    }
                    foreach (Customer c in r.CustomersList)
                    {
                        tmpCity = Cities.Where(h => h.Name.Equals(c.StartingPlace)).FirstOrDefault();
                        if (tmpCity == null)
                        {
                            tmpCity = new Counter { Name = c.StartingPlace };
                            Cities.Add(tmpCity);
                        }
                        if (c.Bus != null)
                        {
                            tmpCity
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}