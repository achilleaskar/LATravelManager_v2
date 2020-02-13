using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Lists;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views.Bansko;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class CustomSorter : IComparer
    {
        #region Methods

        public int Compare(object x, object y)
        {
            if (x is CustomerWrapper a && y is CustomerWrapper b)
            {
                bool ab = a.Reservation.Booking.ReservationsInBooking.Any(r => r.CustomersList.Any(c => c.BusGo == null && c.BusReturn == null));
                bool bb = b.Reservation.Booking.ReservationsInBooking.Any(r => r.CustomersList.Any(c => c.BusGo == null && c.BusReturn == null));

                if (ab && !bb)
                {
                    return -1;
                }
                if (bb && !ab)
                {
                    return 1;
                }
                return a.Reservation.Booking.Id.CompareTo(b.Reservation.Booking.Id);
            }
            return 0;
        }

        #endregion Methods
    }

    public class ListManagement_ViewModel : MyViewModelBase
    {
        #region Constructors

        public ListManagement_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            //ShowCustomersCommand = new RelayCommand(async () => { await ShowCustomers(); }, () => SelectedExcursion != null && SelectedDate != null);
            ShowCustomersCommand = new RelayCommand(async () => await ShowCustomers());
            EditBookingCommand = new RelayCommand(async () => await EditBooking(), CanEditBooking);
            AddCustomersToBusCommand = new RelayCommand(AddCustomersToBus, CanAddCustomersToBus);
            UpdateVehiclesCommand = new RelayCommand(async () => await UpdateVehicles());

            SaveBusesCommand = new RelayCommand(async () => await SaveBuses(), Context != null && Context.HasChanges());
            AddBusCommand = new RelayCommand(AddBus, CanAddBus);

            PutAtSeatsCommand = new RelayCommand<object>(PutAtSeats, CanPutAtSeats);
            PutAtSeatsReturnCommand = new RelayCommand<object>(PutAtSeatsEp, CanPutAtSeatsEp);

            RemoveBusCommand = new RelayCommand<object>(RemoveBus, CanRemoveBus);
            MarkAllCommand = new RelayCommand<int>(MarkAll);
            ClearSelCustomersCommand = new RelayCommand<int>(ClearSelCustomers, CanClearSelCustomers);
            LeaderDriverCommand = new RelayCommand<int>(LeaderDriver, SelectedCustomer != null);

            SelectAllCommand = new RelayCommand<int>(SelectAll);

            Load();
        }

        private bool _SecondDepart;

        public bool SecondDepart
        {
            get
            {
                return _SecondDepart;
            }

            set
            {
                if (_SecondDepart == value)
                {
                    return;
                }

                _SecondDepart = value;
                RaisePropertyChanged();
                RecalculateCustomers();
            }
        }

        #endregion Constructors

        #region Fields

        private List<BookingWrapper> _AllBookings;
        private ObservableCollection<Bus> _Buses;
        private DateTime _CheckInDate;
        private bool _CheckOut;
        private ObservableCollection<Counter> _Cities;
        private GenericRepository _Context;
        private ObservableCollection<CustomerWrapper> _Customers;
        private int _DepartmentIndexBookingFilter;
        private ObservableCollection<Excursion> _Excursions;
        private ObservableCollection<Counter> _Hotels;
        private ObservableCollection<Leader> _Leaders;
        private bool _NoBus;

        private int _Remaining;

        private Bus _SelectedBus;

        private CustomerWrapper _SelectedCustomer;

        private int _SelectedCustomers;

        private ExcursionDate _SelectedDates;

        private Excursion _SelectedExcursion;

        private Vehicle _SelectedVehicle;

        private int _StartSeat;

        private int _StartSeatRet;

        private ObservableCollection<Vehicle> _Vehicles;

        #endregion Fields

        #region Properties

        public RelayCommand AddBusCommand { get; }

        public RelayCommand AddCustomersToBusCommand { get; }

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
                Buses.CollectionChanged += Buses_CollectionChanged;
            }
        }

        public DateTime CheckInDate
        {
            get
            {
                return _CheckInDate;
            }

            set
            {
                if (_CheckInDate == value)
                {
                    return;
                }

                _CheckInDate = value;
                RaisePropertyChanged();
            }
        }

        public bool CheckOut
        {
            get
            {
                return _CheckOut;
            }

            set
            {
                if (_CheckOut == value)
                {
                    return;
                }

                _CheckOut = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CheckIn));
            }
        }

        public ObservableCollection<Counter> Cities
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

        public RelayCommand<int> ClearSelCustomersCommand { get; }

        public List<SolidColorBrush> ColorsR { get; set; }

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

        public ObservableCollection<CustomerWrapper> Customers
        {
            get
            {
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                RaisePropertyChanged();
                ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(Customers);
                view.CustomSort = new CustomSorter();
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

        public RelayCommand EditBookingCommand { get; }

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

        public bool HasBus => Buses.Count > 0;

        public ObservableCollection<Counter> Hotels
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

        public RelayCommand<int> LeaderDriverCommand { get; }

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

        public MainViewModel MainViewModel { get; }

        public bool Manual { get; set; }

        public bool ManualAll { get; set; } = false;

        public RelayCommand<int> MarkAllCommand { get; }

        public bool NoBus
        {
            get
            {
                return _NoBus;
            }

            set
            {
                if (_NoBus == value)
                {
                    return;
                }

                _NoBus = value;
                RaisePropertyChanged();
                RecalculateCustomers();
            }
        }

        public RelayCommand<object> PutAtSeatsCommand { get; }

        public RelayCommand<object> PutAtSeatsReturnCommand { get; }

        public int Remaining
        {
            get
            {
                return _Remaining;
            }

            set
            {
                if (_Remaining == value)
                {
                    return;
                }

                _Remaining = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<object> RemoveBusCommand { get; }

        public RelayCommand SaveBusesCommand { get; }

        public RelayCommand<int> SelectAllCommand { get; }

        public Bus SelectedBus
        {
            get
            {
                return _SelectedBus;
            }

            set
            {
                if (_SelectedBus == value)
                {
                    return;
                }

                _SelectedBus = value;
                RaisePropertyChanged();
            }
        }

        public CustomerWrapper SelectedCustomer
        {
            get
            {
                return _SelectedCustomer;
            }

            set
            {
                if (_SelectedCustomer == value)
                {
                    return;
                }

                _SelectedCustomer = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedCustomers
        {
            get
            {
                return _SelectedCustomers;
            }

            set
            {
                if (_SelectedCustomers == value)
                {
                    return;
                }

                _SelectedCustomers = value;
                RaisePropertyChanged();
            }
        }

        public ExcursionDate SelectedDate
        {
            get
            {
                return _SelectedDates;
            }

            set
            {
                if (_SelectedDates == value)
                {
                    return;
                }

                _SelectedDates = value;
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

        public Vehicle SelectedVehicle
        {
            get
            {
                return _SelectedVehicle;
            }

            set
            {
                if (_SelectedVehicle == value)
                {
                    return;
                }

                _SelectedVehicle = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowCustomersCommand { get; set; }

        public int StartSeat
        {
            get
            {
                return _StartSeat;
            }

            set
            {
                if (_StartSeat == value)
                {
                    return;
                }

                _StartSeat = value;
                RaisePropertyChanged();
            }
        }

        public int StartSeatRet
        {
            get
            {
                return _StartSeatRet;
            }

            set
            {
                if (_StartSeatRet == value)
                {
                    return;
                }

                _StartSeatRet = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand UpdateVehiclesCommand { get; }

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

        public void CountBoth()
        {
            Counter tmpHotel;
            Counter tmpCity;

            foreach (var h in Hotels)
            {
                h.Both = 0;
            }
            foreach (var c in Cities)
            {
                c.Both = 0;
            }

            if (!CheckOut)
                foreach (BookingWrapper b in AllBookings)
                {
                    foreach (ReservationWrapper r in b.ReservationsInBooking.Select(r => new ReservationWrapper(r)))
                    {
                        tmpHotel = Hotels.Where(h => h.Name.Equals(r.HotelName.TrimEnd(new[] { '*' }))).FirstOrDefault();
                        foreach (Customer c in r.CustomersList)
                        {
                            tmpCity = Cities.Where(h => h.Name.Equals(c.StartingPlace)).FirstOrDefault();
                            if (Cities.Where(ci => ci.Name == c.StartingPlace).FirstOrDefault().Selected)
                            {
                                tmpHotel.Both += 1;
                            }
                            if (Hotels.Where(ho => ho.Name == c.HotelName).FirstOrDefault().Selected)
                            {
                                tmpCity.Both += 1;
                            }
                        }
                    }
                }
            else
                foreach (BookingWrapper b in AllBookings)
                {
                    foreach (ReservationWrapper r in b.ReservationsInBooking.Select(r => new ReservationWrapper(r)))
                    {
                        tmpHotel = Hotels.Where(h => h.Name.Equals(r.HotelName.TrimEnd(new[] { '*' }))).FirstOrDefault();
                        foreach (Customer c in r.CustomersList)
                        {
                            tmpCity = Cities.Where(h => h.Name.Equals(c.ReturningPlace)).FirstOrDefault();
                            if (Cities.Where(ci => ci.Name == c.ReturningPlace).FirstOrDefault().Selected)
                            {
                                tmpHotel.Both += 1;
                            }
                            if (Hotels.Where(ho => ho.Name == c.HotelName).FirstOrDefault().Selected)
                            {
                                tmpCity.Both += 1;
                            }
                        }
                    }
                }
        }

        public void CountSelected()
        {
            int i = 0;
            int b = 0;
            foreach (var c in Customers)
            {
                if (c.Selected)
                {
                    i++;
                }
                if (c.NoBus)
                {
                    b++;
                }
            }
            SelectedCustomers = i;
            Remaining = b;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                Excursions = new ObservableCollection<Excursion>(MainViewModel.BasicDataManager.Excursions.Where(c => c.Id > 0 && c.ExcursionDates.Any(e => e.CheckOut > DateTime.Now)).OrderBy(e => e.FirstDate));
                Vehicles = new ObservableCollection<Vehicle>(MainViewModel.BasicDataManager.Vehicles.OrderBy(b => b.Name));
                Leaders = new ObservableCollection<Leader>(MainViewModel.BasicDataManager.Leaders.OrderBy(o => o.Name));
                Buses = new ObservableCollection<Bus>();

                Hotels = new ObservableCollection<Counter>();
                Cities = new ObservableCollection<Counter>();
                Customers = new ObservableCollection<CustomerWrapper>();
                AllBookings = new List<BookingWrapper>();
                Hotels.CollectionChanged += Counter_CollectionChanged;
                Cities.CollectionChanged += Counter_CollectionChanged;

                ColorsR = new List<SolidColorBrush>
                {
                    new SolidColorBrush(Colors.Aquamarine),
                    new SolidColorBrush(Colors.LightBlue),
                    new SolidColorBrush(Colors.Yellow),
                    new SolidColorBrush(Colors.LightPink),
                    new SolidColorBrush(Colors.Orange),
                    new SolidColorBrush(Colors.LightCoral),
                    new SolidColorBrush(Colors.LightSkyBlue),
                    new SolidColorBrush(Colors.Khaki),
                    new SolidColorBrush(Colors.Aqua)
                };
                CheckInDate = DateTime.Today;
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

        public void SetColors(ICollectionView customers)
        {
            int coutner = 0;
            //customers = customers.OrderBy(c => c.Reservation.Booking.Id);
            int id = 0;
            foreach (object c1 in customers)
            {
                if (c1 is CustomerWrapper c && c.NoBus)
                {
                    if (c.Reservation.Booking.Id != id)
                    {
                        id = c.Reservation.Booking.Id;
                        coutner++;
                    }
                    c.RoomColor = ColorsR[coutner % 9];
                }
            }
        }

        private void AddBus()
        {
            var b = new Bus
            {
                Customers = new ObservableCollection<Customer>(),
                Excursion = Context.GetById<Excursion>(SelectedExcursion.Id),
                Vehicle = Context.GetById<Vehicle>(SelectedVehicle.Id),
                // TimeGo = SelectedDate.CheckIn
            };
            if (!CheckOut)
            {
                b.TimeGo = CheckInDate;
            }
            else
                b.TimeReturn = CheckInDate;
            Context.Add(b);
            Buses.Add(b);
            RaisePropertyChanged(nameof(HasBus));
        }

        private void AddCustomersToBus()
        {
            SelectedBus.Manual = true;
            ManualAll = true;
            foreach (var c in Customers)
            {
                if (c.NoBus && c.Selected)
                {
                    if (c.LeaderDriver > 0)
                    {
                        if (SelectedBus.Customers.Any(p => p.LeaderDriver == c.LeaderDriver))
                        {
                            MessageBox.Show("Υπάρχει ήδη " + (c.LeaderDriver == 1 ? "συνοδός" : "οδηγός"));
                        }
                        else
                        {
                            SelectedBus.Customers.Add(c.Model);
                            c.Selected = false;
                            if (!CheckOut)
                                c.BusGo = SelectedBus;
                            else
                                c.BusReturn = SelectedBus;
                        }
                    }
                    else
                    {
                        SelectedBus.Customers.Add(c.Model);
                        c.Selected = false;
                        if (!CheckOut)
                            c.BusGo = SelectedBus;
                        else
                            c.BusReturn = SelectedBus;
                    }
                }
            }
            SelectedBus.Manual = false;
            ManualAll = false;

            SelectedBus.RecalculateCustomers();
            CountSelected();
            RecalculateCustomers();

            CollectionViewSource.GetDefaultView(Customers).Refresh();
            SetColors(CollectionViewSource.GetDefaultView(Customers));
        }

        private void B_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BusChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
            {
                if (sender is Bus b && b.Selected)
                {
                    SelectedBus = b;
                }
            }
        }

        private void Buses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Bus c in e.OldItems)
                {
                    //Removed items
                    c.PropertyChanged -= BusChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Bus c in e.NewItems)
                {
                    c.PropertyChanged += BusChanged;
                }
            }
        }

        private void C_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected" && !ManualAll)
            {
                ManualAll = true;
                CountSelected();

                if (sender is CustomerWrapper cmer)
                {
                    if (cmer.Selected)
                    {
                        List<CustomerWrapper> cmers = Customers.Where(c1 => c1.Reservation.Booking.Id == cmer.Reservation.Booking.Id && c1.Id != cmer.Id).ToList();
                        if (!cmers.Any(x => x.Selected))
                        {
                            foreach (var cm1 in cmers)
                            {
                                if (!CheckOut)
                                {
                                    if (cm1.BusGo == null)
                                        cm1.Selected = true;
                                }
                                else
                                    if (cm1.BusReturn == null)
                                    cm1.Selected = true;
                            }
                        }
                    }
                }
                ManualAll = false;
            }
        }

        private bool CanAddBus()
        {
            return SelectedExcursion != null;
        }

        private bool CanAddCustomersToBus()
        {
            return SelectedBus != null && (SelectedBus.RemainingSeats >= SelectedCustomers);
        }

        private bool CanClearSelCustomers(int arg)
        {
            return SelectedBus != null;
        }

        private bool CanEditBooking()
        {
            return true;
        }

        private bool CanPutAtSeats(object customer)
        {
            return StartSeat > 0 && SelectedBus != null && SelectedBus.BusView != null && SelectedBus.BusView.Seires.Count > 0;
        }

        private bool CanPutAtSeatsEp(object arg)
        {
            return StartSeatRet > 0 && SelectedBus != null && SelectedBus.BusViewReturn != null && SelectedBus.BusViewReturn.Seires.Count > 0;
        }

        private bool CanRemoveBus(object arg)
        {
            return (arg is Bus b) && b.Customers != null && b.Customers.Count == 0;
        }

        private void ClearSelCustomers(int obj)
        {
            foreach (var c in SelectedBus.Customers)
            {
                if (!CheckOut)
                    c.BusGo = null;
                else
                    c.BusReturn = null;
            }
            SelectedBus.Customers.Clear();
        }

        private void Counter_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Counter c in e.OldItems)
                {
                    //Removed items
                    c.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Counter c in e.NewItems)
                {
                    c.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }

        private async Task EditBooking()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                if (SelectedCustomer != null)
                {
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedCustomer.Reservation.Booking.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected" && !Manual)
            {
                RecalculateCustomers();
            }
        }

        private Seat GetSeat(int num, bool retur)
        {
            if (!retur)
                foreach (var row in SelectedBus.BusView.Seires)
                {
                    foreach (var seat in row.Seats)
                    {
                        if (seat.Number == num)
                        {
                            return seat;
                        }
                    }
                }
            else
                foreach (var row in SelectedBus.BusViewReturn.Seires)
                {
                    foreach (var seat in row.Seats)
                    {
                        if (seat.Number == num)
                        {
                            return seat;
                        }
                    }
                }

            return null;
        }

        private void LeaderDriver(int obj)
        {
            SelectedCustomer.LeaderDriver = obj;
            RaisePropertyChanged(nameof(Customers));
        }

        private void MarkAll(int obj)
        {
            if (SelectedCustomer != null)
            {
                ManualAll = true;
                foreach (var c in Customers)
                {
                    if (c.Reservation.Booking.Id == SelectedCustomer.Reservation.Booking.Id && c.BusGo == null && c.BusReturn == null)
                    {
                        c.Selected = (obj == 1);
                    }
                }
                ManualAll = false;
            }
            CountSelected();
        }

        private void PutAtSeats(object cust)
        {
            if (cust is CustomerWrapper customer)
            {
                List<Seat> seats = new List<Seat>();
                for (int i = 0; i < (new BookingWrapper(customer.Reservation.Booking)).Customers.Count; i++)
                {
                    Seat seat = GetSeat(StartSeat + i, false);
                    if (seat != null && seat.Customer == null)
                    {
                        seats.Add(seat);
                    }
                    else
                    {
                        MessageBox.Show("Den xwrane");
                        return;
                    }
                }
                int counter = 0;
                foreach (var c in (new BookingWrapper(customer.Reservation.Booking)).Customers)
                {
                    seats[counter].Customer = c;
                    Customers.Where(v => v.Id == c.Id).FirstOrDefault().SeatNum = seats[counter].Number;
                    counter++;
                }

                SelectedBus.RaisePropertyChanged();
                SelectedBus.update();
            }
            RaisePropertyChanged(nameof(Customers));
        }

        private void PutAtSeatsEp(object cust)
        {
            if (cust is CustomerWrapper customer)
            {
                List<Seat> seats = new List<Seat>();
                for (int i = 0; i < (new BookingWrapper(customer.Reservation.Booking)).Customers.Count; i++)
                {
                    Seat seat = GetSeat(StartSeatRet + i, true);
                    if (seat != null && seat.Customer == null)
                    {
                        seats.Add(seat);
                    }
                    else
                    {
                        MessageBox.Show("Den xwrane");
                        return;
                    }
                }
                int counter = 0;
                foreach (var c in (new BookingWrapper(customer.Reservation.Booking)).Customers)
                {
                    seats[counter].Customer = c;
                    // c.SeatNumRet = seats[counter].Number;
                    Customers.Where(v => v.Id == c.Id).FirstOrDefault().SeatNumRet = seats[counter].Number;
                    counter++;
                }

                SelectedBus.RaisePropertyChanged();
                SelectedBus.update();
            }
            RaisePropertyChanged(nameof(Customers));
        }

        public bool CheckIn => !CheckOut;

        private void RecalculateCustomers()
        {
            ManualAll = true;
            Customers.Clear();
            if (!CheckOut)
                foreach (var b in AllBookings)
                {
                    foreach (var r in b.ReservationsInBooking.Select(r => new ReservationWrapper(r)))
                    {
                        foreach (var c in r.CustomersList.Select(k => new CustomerWrapper(k)))
                        {
                            if (Hotels.Where(h => h.Name == r.HotelName.TrimEnd(new[] { '*' })).FirstOrDefault().Selected &&
                                Cities.Where(s => s.Name == c.StartingPlace).FirstOrDefault().Selected &&
                                ((!b.DifferentDates || c.CheckIn == CheckInDate)) && (!NoBus || (c.BusGo == null && c.BusReturn == null)) && b.SecondDepart == SecondDepart && c.CustomerHasBusIndex < 2)
                            {
                                Customers.Add(c);
                                c.PropertyChanged += C_PropertyChanged;
                            }
                            else
                            {
                                c.Selected = false;
                            }
                        }
                    }
                }
            else
                foreach (var b in AllBookings)
                {
                    foreach (var r in b.ReservationsInBooking.Select(r => new ReservationWrapper(r)))
                    {
                        foreach (var c in r.CustomersList.Select(k => new CustomerWrapper(k)))
                        {
                            if (Hotels.Where(h => h.Name == r.HotelName.TrimEnd(new[] { '*' })).FirstOrDefault().Selected &&
                                Cities.Where(s => s.Name == c.ReturningPlace).FirstOrDefault().Selected &&
                                ((!b.DifferentDates || c.CheckOut == CheckInDate)) && (!NoBus || (c.BusGo == null && c.BusReturn == null)) && c.CustomerHasBusIndex % 2 == 0)
                            {
                                Customers.Add(c);
                                c.PropertyChanged += C_PropertyChanged;
                            }
                            else
                            {
                                c.Selected = false;
                            }
                        }
                    }
                }

            ManualAll = false;
            CountSelected();
            CollectionViewSource.GetDefaultView(Customers).Refresh();
            CountBoth();
            SetColors(CollectionViewSource.GetDefaultView(Customers));
        }

        private void RemoveBus(object obj)
        {
            Buses.Remove(obj as Bus);
            Context.Delete(obj as Bus);
        }

        private async Task SaveBuses()
        {
            await Context.SaveAsync();
        }

        private void SelectAll(int par)
        {
            Manual = true;
            ManualAll = true;
            switch (par)
            {
                case 0:
                    foreach (var c in Cities)
                    {
                        c.Selected = true;
                    }
                    break;

                case 1:
                    foreach (var c in Cities)
                    {
                        c.Selected = false;
                    }
                    break;

                case 2:
                    foreach (var c in Hotels)
                    {
                        c.Selected = true;
                    }
                    break;

                case 3:
                    foreach (var c in Hotels)
                    {
                        c.Selected = false;
                    }
                    break;

                case 4:
                    foreach (var c in Customers)
                    {
                        if (c.BusGo == null && c.BusGo == null)
                            c.Selected = true;
                    }
                    break;

                case 5:
                    foreach (var c in Customers)
                    {
                        c.Selected = false;
                    }
                    break;
            }
            ManualAll = false;
            Manual = false;
            if (par < 4)
                RecalculateCustomers();
            else
            {
                CountSelected();
            }
        }

        private async Task ShowCustomers()
        {
            await Task.Delay(0);
            Mouse.OverrideCursor = Cursors.Wait;
            Context = new GenericRepository();
            // AllBookings = (Context.GetAllBookingsForLists(SelectedExcursion.Id, SelectedDate.CheckIn, DepartmentIndexBookingFilter)).Select(b => new BookingWrapper(b)).ToList();
            if (!CheckOut)
                AllBookings = (Context.GetAllBookingsForLists(SelectedExcursion.Id, CheckInDate, DepartmentIndexBookingFilter)).Select(b => new BookingWrapper(b)).ToList();
            else
                AllBookings = (Context.GetAllBookingsForLists(SelectedExcursion.Id, CheckInDate, DepartmentIndexBookingFilter, true)).Select(b => new BookingWrapper(b)).ToList();

            Buses = new ObservableCollection<Bus>((Context.GetAllBuses(SelectedExcursion.Id, CheckInDate, CheckOut)));
            //Buses = new ObservableCollection<Bus>((Context.GetAllBuses(SelectedExcursion.Id, SelectedDate.CheckIn)));
            Leaders = new ObservableCollection<Leader>((await Context.GetAllAsync<Leader>()).OrderBy(o => o.Name));
            Hotels.Clear();
            Cities.Clear();
            Customers.Clear();

            Counter tmpHotel;
            Counter tmpCity;

            foreach (BookingWrapper b in AllBookings)
            {
                foreach (ReservationWrapper r in b.ReservationsInBooking.Select(r => new ReservationWrapper(r)))
                {
                    try
                    {
                        tmpHotel = Hotels.Where(h => h.Name.Equals(r.HotelName.TrimEnd(new[] { '*' }))).FirstOrDefault();
                        if (tmpHotel == null)
                        {
                            tmpHotel = new Counter { Name = r.HotelName.TrimEnd(new[] { '*' }) };
                            Hotels.Add(tmpHotel);
                        }
                        if (!CheckOut)
                        {
                            foreach (Customer c in r.CustomersList)
                            {
                                tmpCity = Cities.Where(h => h.Name.Equals(c.StartingPlace)).FirstOrDefault();
                                if (tmpCity == null)
                                {
                                    tmpCity = new Counter { Name = c.StartingPlace };
                                    Cities.Add(tmpCity);
                                }
                                if (c.BusGo == null && c.BusReturn == null)
                                {
                                    tmpCity.UnHandled += 1;
                                    tmpHotel.UnHandled += 1;
                                }
                                if (Cities.Where(ci => ci.Name == c.StartingPlace).FirstOrDefault().Selected)
                                {
                                    tmpHotel.Both += 1;
                                }
                                if (Hotels.Where(ho => ho.Name == c.HotelName).FirstOrDefault().Selected)
                                {
                                    tmpCity.Both += 1;
                                }

                                tmpCity.Total += 1;
                                tmpHotel.Total += 1;
                            }
                        }
                        else
                        {
                            foreach (Customer c in r.CustomersList)
                            {
                                tmpCity = Cities.Where(h => h.Name.Equals(c.ReturningPlace)).FirstOrDefault();
                                if (tmpCity == null)
                                {
                                    tmpCity = new Counter { Name = c.ReturningPlace };
                                    Cities.Add(tmpCity);
                                }
                                if (c.BusGo == null && c.BusReturn == null)
                                {
                                    tmpCity.UnHandled += 1;
                                    tmpHotel.UnHandled += 1;
                                }
                                if (Cities.Where(ci => ci.Name == c.ReturningPlace).FirstOrDefault().Selected)
                                {
                                    tmpHotel.Both += 1;
                                }
                                if (Hotels.Where(ho => ho.Name == c.HotelName).FirstOrDefault().Selected)
                                {
                                    tmpCity.Both += 1;
                                }

                                tmpCity.Total += 1;
                                tmpHotel.Total += 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                    }
                    CollectionViewSource.GetDefaultView(Cities).SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    CollectionViewSource.GetDefaultView(Hotels).SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    foreach (var b2 in Buses)
                    {
                        b2.SetColors();
                        b2.PropertyChanged += BusChanged;
                    }
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        private async Task UpdateVehicles()
        {
            Vehicles = new ObservableCollection<Vehicle>((await Context.GetAllAsync<Vehicle>()).OrderBy(b => b.Name));
        }

        #endregion Methods
    }
}