using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace LATravelManager.Model.Lists
{
    public class Bus : EditTracker
    {
        #region Constructors

        public Bus()
        {
            Manual = true;
            Cities = new ObservableCollection<Counter>();
            Hotels = new ObservableCollection<Counter>();
            Customers = new ObservableCollection<Customer>();
            ClearBusCommand = new RelayCommand(ClearBus, CanClearBus);
            RemoveCustomerCommand = new RelayCommand<int>(RemoveCustomer, SelectedCustomer != null);
            SetSeatsCommand = new RelayCommand(() => { CreateBusView(false); });
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
            Manual = false;
            BusView = new BusView();
            // RecalculateCustomers();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Counter> _Cities;
        private string _Comment;
        private ObservableCollection<Customer> _Customers = new ObservableCollection<Customer>();
        private string _drivers;

        private Excursion _Excursion;

        private ObservableCollection<Counter> _Hotels;

        private Leader _Leader;

        private bool _OneWay;

        private bool _Selected;

        private Customer _SelectedCustomer;

        private string _StartingPlace = string.Empty;

        private DateTime _TimeGo;

        private DateTime _TimeReturn;

        private Vehicle _Vehicle;

        #endregion Fields

        #region Properties

        [NotMapped]
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
                // CollectionViewSource.GetDefaultView(Cities).SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        [NotMapped]
        public RelayCommand ClearBusCommand { get; set; }

        [NotMapped]
        public List<SolidColorBrush> ColorsR { get; set; }

        public string Comment
        {
            get
            {
                return _Comment;
            }

            set
            {
                if (_Comment == value)
                {
                    return;
                }

                _Comment = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Customer> Customers
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
                Customers.CollectionChanged += Customers_CollectionChanged;
            }
        }

        [NotMapped]
        public string Drivers
        {
            get
            {
                return _drivers;
            }

            set
            {
                if (_drivers == value)
                {
                    return;
                }

                _drivers = value;
                RaisePropertyChanged();
            }
        }

        public string EmptySeats => "Κενές: " + (Vehicle.SeatsPassengers - Customers.Where(g => g.LeaderDriver == 0).ToList().Count) + "/" + Vehicle.SeatsPassengers;

        public Excursion Excursion
        {
            get
            {
                return _Excursion;
            }

            set
            {
                if (_Excursion == value)
                {
                    return;
                }

                _Excursion = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
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
                //CollectionViewSource.GetDefaultView(Hotels).SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        public Leader Leader
        {
            get
            {
                return _Leader;
            }

            set
            {
                if (_Leader == value)
                {
                    return;
                }

                _Leader = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public bool Manual { get; set; }

        public bool OneWay
        {
            get
            {
                return _OneWay;
            }

            set
            {
                if (_OneWay == value)
                {
                    return;
                }

                _OneWay = value;
                RaisePropertyChanged();
            }
        }

        public int RemainingSeats => Vehicle.SeatsPassengers - Customers.Where(y => y.LeaderDriver == 0).ToList().Count;

        public RelayCommand<int> RemoveCustomerCommand { get; }
        public RelayCommand SetSeatsCommand { get; }

        [NotMapped]
        public bool Selected
        {
            get
            {
                return _Selected;
            }

            set
            {
                if (_Selected == value)
                {
                    return;
                }

                _Selected = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public Customer SelectedCustomer
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

        [StringLength(20)]
        public string StartingPlace
        {
            get
            {
                return _StartingPlace;
            }

            set
            {
                if (_StartingPlace == value)
                {
                    return;
                }

                _StartingPlace = value;
                RaisePropertyChanged();
            }
        }

        public DateTime TimeGo
        {
            get
            {
                return _TimeGo;
            }

            set
            {
                if (_TimeGo == value)
                {
                    return;
                }

                _TimeGo = value;
                RaisePropertyChanged();
            }
        }

        public DateTime TimeReturn
        {
            get
            {
                return _TimeReturn;
            }

            set
            {
                if (_TimeReturn == value)
                {
                    return;
                }

                _TimeReturn = value;
                RaisePropertyChanged();
            }
        }

        public Vehicle Vehicle
        {
            get
            {
                return _Vehicle;
            }

            set
            {
                if (_Vehicle == value)
                {
                    return;
                }

                _Vehicle = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public void RecalculateCustomers()
        {
            Cities.Clear();
            Hotels.Clear();

            Counter tmpHotel;
            Counter tmpCity;

            int dr = 0, le = 0, gu = 0;

            foreach (CustomerWrapper b in Customers.Select(c => new CustomerWrapper(c)))
            {
                if (b.LeaderDriver == 0)
                {
                    tmpHotel = Hotels.Where(h => h.Name.Equals(b.HotelName)).FirstOrDefault();
                    if (tmpHotel == null)
                    {
                        tmpHotel = new Counter { Name = b.HotelName };
                        Hotels.Add(tmpHotel);
                    }
                    tmpCity = Cities.Where(h => h.Name.Equals(b.StartingPlace)).FirstOrDefault();
                    if (tmpCity == null)
                    {
                        tmpCity = new Counter { Name = b.StartingPlace };
                        Cities.Add(tmpCity);
                    }
                    tmpCity.Total += 1;
                    tmpHotel.Total += 1;
                }
                else
                {
                    if (b.LeaderDriver == 1)
                    {
                        le += 1;
                    }
                    else if (b.LeaderDriver == 2)
                    {
                        gu += 1;
                    }
                    else if (b.LeaderDriver == 3)
                    {
                        dr += 1;
                    }
                    Drivers = $"Συνοδοί: {le} - Οδηγόι: {dr} - Ξεν.: {gu}";
                }
            }

            SetColors();
            RaisePropertyChanged(nameof(EmptySeats));
        }

        public void SetColors()
        {
            if (Customers.Count > 0)
            {
                int coutner = 0;
                //customers = customers.OrderBy(c => c.Reservation.Booking.Id);
                int id = 0;
                foreach (CustomerWrapper c in Customers.Select(g => new CustomerWrapper(g)))
                {
                    if (c.Reservation == null)
                    {
                        break;
                    }
                    if (c.Reservation.Booking != null && c.Reservation.Booking.Id != id)
                    {
                        id = c.Reservation.Booking.Id;
                        coutner++;
                    }
                    c.RoomColor = ColorsR[coutner % 9];
                }
            }
        }

        private bool CanClearBus()
        {
            return Customers.Count > 0;
        }

        private void ClearBus()
        {
            Manual = true;
            foreach (var c in Customers)
            {
                if (TimeGo.Year > 2000)
                {
                    c.BusGo = null;
                }
                else if (TimeReturn.Year > 2000)
                {
                    c.BusReturn = null;
                }
            }
            Customers.Clear();
            Manual = false;
            RecalculateCustomers();
        }

        private void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!Manual)
                RecalculateCustomers();
            RaisePropertyChanged(nameof(EmptySeats));
        }

        public bool Going => TimeGo.Date.Year > 2000;

        private void RemoveCustomer(int obj)
        {
            if (obj == 0)
            {
                if (Going)
                    SelectedCustomer.BusGo = null;
                else
                    SelectedCustomer.BusReturn = null;

                Customers.Remove(SelectedCustomer);
            }
            else if (obj == 1)
            {
                var toRemove = Customers.Where(v => v.Reservation.Booking.Id == SelectedCustomer.Reservation.Booking.Id).ToList();
                foreach (var c in toRemove)
                {
                    if (Going)
                        c.BusGo = null;
                    else
                        c.BusGo = null;

                    Customers.Remove(c);
                }
            }
        }

        public override string ToString()
        {
            return (Leader != null ? Leader.Name : "" + " " + Vehicle.Name).TrimStart(new[] { ' ' });
        }

        private BusView _BusView;

        [NotMapped]
        public BusView BusView
        {
            get
            {
                return _BusView;
            }

            set
            {
                if (_BusView == value)
                {
                    return;
                }

                _BusView = value;
                RaisePropertyChanged();
            }
        }

        private BusView _BusViewReturn;

        [NotMapped]
        public BusView BusViewReturn
        {
            get
            {
                return _BusViewReturn;
            }

            set
            {
                if (_BusViewReturn == value)
                {
                    return;
                }

                _BusViewReturn = value;
                RaisePropertyChanged();
            }
        }

        public void CreateBusView(bool ret = false)
        {
            if (!ret)
            {
                CreateBusView(true);
            }
            if (ret)
            {
                BusViewReturn = new BusView();
                bool normalBackSeat = Vehicle.DoorSeat % 4 > 2;
                bool doubleDoorPlace = (Vehicle.SeatsPassengers - 5) % 4 == 0;
                int seires = (Vehicle.SeatsPassengers - 5) / 4 + 1 + (doubleDoorPlace ? 1 : 0);
                for (int i = 0; i < seires; i++)
                {
                    BusViewReturn.Seires.Add(new Seira());
                }
                int limit = Vehicle.DoorSeat - (doubleDoorPlace ? 4 : 2) - 1;

                int currentseat = 0;
                int row = 0;
                int counter = 0;
                while (currentseat < limit)
                {
                    if (counter == 5)
                    {
                        row++;
                        counter = 0;
                    }
                    currentseat++;
                    BusViewReturn.Seires[row].Seats[counter].Number = currentseat;
                    BusViewReturn.Seires[row].Seats[counter].Thickness = new Thickness(0, 0, 1, 1);
                    counter++;
                    if ((currentseat + 2) % 4 == 0)
                    {
                        counter++;
                        BusViewReturn.Seires[row].Seats[2].SeatType = SeatType.Road;
                        BusViewReturn.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 0);
                    }
                }
                if (doubleDoorPlace)
                {
                    row++;
                    BusViewReturn.Seires[row].Seats[0].Number = ++currentseat;
                    BusViewReturn.Seires[row].Seats[0].Thickness = new Thickness(0, 0, 1, 1);
                    BusViewReturn.Seires[row].Seats[1].Number = ++currentseat;
                    BusViewReturn.Seires[row].Seats[1].Thickness = new Thickness(0, 0, 1, 1);
                    BusViewReturn.Seires[row].Seats[2].SeatType = SeatType.Road;
                    BusViewReturn.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 0);
                    BusViewReturn.Seires[row].Seats[3].SeatType = SeatType.Door;
                    BusViewReturn.Seires[row].Seats[3].Thickness = new Thickness(0);
                    BusViewReturn.Seires[row].Seats[4].SeatType = SeatType.Door;
                    BusViewReturn.Seires[row].Seats[4].Thickness = new Thickness(0);
                    row++;
                    BusViewReturn.Seires[row].Seats[0].Number = ++currentseat;
                    BusViewReturn.Seires[row].Seats[0].Thickness = new Thickness(0, 0, 1, 1);
                    BusViewReturn.Seires[row].Seats[1].Number = ++currentseat;
                    BusViewReturn.Seires[row].Seats[1].Thickness = new Thickness(0, 0, 1, 1);
                    BusViewReturn.Seires[row].Seats[2].SeatType = SeatType.Road;
                    BusViewReturn.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 0);
                    BusViewReturn.Seires[row].Seats[3].SeatType = SeatType.Door;
                    BusViewReturn.Seires[row].Seats[3].Thickness = new Thickness(0);
                    BusViewReturn.Seires[row].Seats[4].SeatType = SeatType.Door;
                    BusViewReturn.Seires[row].Seats[4].Thickness = new Thickness(0);
                }
                limit = Vehicle.SeatsPassengers - 5;
                row++;
                counter = 0;
                while (currentseat < limit)
                {
                    if (counter == 5)
                    {
                        row++;
                        counter = 0;
                    }
                    currentseat++;
                    BusViewReturn.Seires[row].Seats[counter].Number = currentseat;
                    BusViewReturn.Seires[row].Seats[counter].Thickness = new Thickness(0, 0, 1, 1);
                    counter++;
                    if ((currentseat + 2) % 4 == 0)
                    {
                        counter++;

                        BusViewReturn.Seires[row].Seats[2].SeatType = SeatType.Road;
                        BusViewReturn.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 0);
                    }
                }
                BusViewReturn.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 1);
                row++;
                counter = 0;
                while (currentseat < Vehicle.SeatsPassengers)
                {
                    currentseat++;
                    BusViewReturn.Seires[row].Seats[counter].Number = currentseat;
                    BusViewReturn.Seires[row].Seats[counter].Thickness = new Thickness(0, 0, 1, 1);
                    counter++;
                }
                RaisePropertyChanged(nameof(BusViewReturn));
            }
            else
            {
                BusView = new BusView();
                bool normalBackSeat = Vehicle.DoorSeat % 4 > 2;
                bool doubleDoorPlace = (Vehicle.SeatsPassengers - 5) % 4 == 0;
                int seires = (Vehicle.SeatsPassengers - 5) / 4 + 1 + (doubleDoorPlace ? 1 : 0);
                for (int i = 0; i < seires; i++)
                {
                    BusView.Seires.Add(new Seira());
                }
                int limit = Vehicle.DoorSeat - (doubleDoorPlace ? 4 : 2) - 1;

                int currentseat = 0;
                int row = 0;
                int counter = 0;
                while (currentseat < limit)
                {
                    if (counter == 5)
                    {
                        row++;
                        counter = 0;
                    }
                    currentseat++;
                    BusView.Seires[row].Seats[counter].Number = currentseat;
                    BusView.Seires[row].Seats[counter].Thickness = new Thickness(0, 0, 1, 1);
                    counter++;
                    if ((currentseat + 2) % 4 == 0)
                    {
                        counter++;
                        BusView.Seires[row].Seats[2].SeatType = SeatType.Road;
                        BusView.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 0);
                    }
                }
                if (doubleDoorPlace)
                {
                    row++;
                    BusView.Seires[row].Seats[0].Number = ++currentseat;
                    BusView.Seires[row].Seats[0].Thickness = new Thickness(0, 0, 1, 1);
                    BusView.Seires[row].Seats[1].Number = ++currentseat;
                    BusView.Seires[row].Seats[1].Thickness = new Thickness(0, 0, 1, 1);
                    BusView.Seires[row].Seats[2].SeatType = SeatType.Road;
                    BusView.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 0);
                    BusView.Seires[row].Seats[3].SeatType = SeatType.Door;
                    BusView.Seires[row].Seats[3].Thickness = new Thickness(0);
                    BusView.Seires[row].Seats[4].SeatType = SeatType.Door;
                    BusView.Seires[row].Seats[4].Thickness = new Thickness(0);
                    row++;
                    BusView.Seires[row].Seats[0].Number = ++currentseat;
                    BusView.Seires[row].Seats[0].Thickness = new Thickness(0, 0, 1, 1);
                    BusView.Seires[row].Seats[1].Number = ++currentseat;
                    BusView.Seires[row].Seats[1].Thickness = new Thickness(0, 0, 1, 1);
                    BusView.Seires[row].Seats[2].SeatType = SeatType.Road;
                    BusView.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 0);
                    BusView.Seires[row].Seats[3].SeatType = SeatType.Door;
                    BusView.Seires[row].Seats[3].Thickness = new Thickness(0);
                    BusView.Seires[row].Seats[4].SeatType = SeatType.Door;
                    BusView.Seires[row].Seats[4].Thickness = new Thickness(0);
                }
                limit = Vehicle.SeatsPassengers - 5;
                row++;
                counter = 0;
                while (currentseat < limit)
                {
                    if (counter == 5)
                    {
                        row++;
                        counter = 0;
                    }
                    currentseat++;
                    BusView.Seires[row].Seats[counter].Number = currentseat;
                    BusView.Seires[row].Seats[counter].Thickness = new Thickness(0, 0, 1, 1);
                    counter++;
                    if ((currentseat + 2) % 4 == 0)
                    {
                        counter++;

                        BusView.Seires[row].Seats[2].SeatType = SeatType.Road;
                        BusView.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 0);
                    }
                }
                BusView.Seires[row].Seats[2].Thickness = new Thickness(0, 0, 1, 1);
                row++;
                counter = 0;
                while (currentseat < Vehicle.SeatsPassengers)
                {
                    currentseat++;
                    BusView.Seires[row].Seats[counter].Number = currentseat;
                    BusView.Seires[row].Seats[counter].Thickness = new Thickness(0, 0, 1, 1);
                    counter++;
                }
                RaisePropertyChanged(nameof(BusView));
            }
        }

        public void update()
        {
            RaisePropertyChanged(nameof(BusView));
        }

        #endregion Methods
    }

    public class BusView : BaseModel
    {
        public BusView()
        {
            Seires = new ObservableCollection<Seira>();
        }

        public Bus Bus { get; set; }

        private ObservableCollection<Seira> _Seires;

        public ObservableCollection<Seira> Seires
        {
            get
            {
                return _Seires;
            }

            set
            {
                if (_Seires == value)
                {
                    return;
                }

                _Seires = value;
                RaisePropertyChanged();
            }
        }
    }

    public class Seira : BaseModel
    {
        public Seira()
        {
            Seats = new ObservableCollection<Seat>();
            for (int i = 0; i < 5; i++)
            {
                Seats.Add(new Seat());
            }
        }

        private ObservableCollection<Seat> _Seats;

        public ObservableCollection<Seat> Seats
        {
            get
            {
                return _Seats;
            }

            set
            {
                if (_Seats == value)
                {
                    return;
                }

                _Seats = value;
                Seats.CollectionChanged += Seats_CollectionChanged;
                RaisePropertyChanged();
            }
        }

        private void Seats_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Seat c in e.OldItems)
                {
                    //Removed items
                    c.PropertyChanged -= SeatChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Seat c in e.NewItems)
                {
                    c.PropertyChanged += SeatChanged;
                }
            }
        }

        private void SeatChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Customer")
            {
                if (sender is Seat s)
                {
                    s.RaisePropertyChanged();
                    RaisePropertyChanged("Seats");
                }
            }
        }

        public override string ToString()
        {
            string reply = string.Empty;

            foreach (var seat in Seats)
            {
                reply += seat.Number + " ";
            }

            return reply.Trim();
        }
    }

    public class Seat : BaseModel
    {
        public Thickness Thickness { get; set; }

        public Seat()
        {
            SeatType = SeatType.Normal;
        }

        public int Number { get; set; }

        public SeatType SeatType { get; set; }

        public Brush Color => GetColor();

        private Brush GetColor()
        {
            if (SeatType == SeatType.Normal)
            {
                if (Customer != null)
                {
                    return Customer.RoomColor;
                }
                return new SolidColorBrush(Colors.White);
            }
            if (SeatType == SeatType.Leader || SeatType == SeatType.Driver)
            {
                return new SolidColorBrush(Colors.LightYellow);
            }
            if (SeatType == SeatType.Road)
            {
                return new SolidColorBrush(Colors.LightGray);
            }
            if (SeatType == SeatType.Door)
            {
                return new SolidColorBrush(Colors.DarkGray);
            }
            return new SolidColorBrush(Colors.Red);
        }

        private CustomerWrapper _Customer;

        public CustomerWrapper Customer
        {
            get
            {
                return _Customer;
            }

            set
            {
                if (_Customer == value)
                {
                    return;
                }

                _Customer = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Color));
            }
        }

        private bool _Selected;

        public bool Selected
        {
            get
            {
                return _Selected;
            }

            set
            {
                if (_Selected == value)
                {
                    return;
                }

                _Selected = value;
                RaisePropertyChanged();
            }
        }
    }
}