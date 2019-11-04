using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
                    if (c.Reservation.Booking.Id != id)
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
                c.Bus = null;
            }
            Customers.Clear();
            Manual = false;
            RecalculateCustomers();
        }

        private void Customers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!Manual)
                RecalculateCustomers();
            RaisePropertyChanged(nameof(EmptySeats));
        }

        private void RemoveCustomer(int obj)
        {
            if (obj == 0)
            {
                SelectedCustomer.Bus = null;
                Customers.Remove(SelectedCustomer);
            }
            else if (obj == 1)
            {
                var toRemove = Customers.Where(v => v.Reservation.Booking.Id == SelectedCustomer.Reservation.Booking.Id).ToList();
                foreach (var c in toRemove)
                {
                    c.Bus = null;
                    Customers.Remove(c);
                }
            }
        }

        public override string ToString()
        {
            return Leader.Name + " " + Vehicle.Name;
        }

        #endregion Methods
    }
}