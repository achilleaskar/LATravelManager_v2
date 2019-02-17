using LATravelManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace LATravelManager.UI.Wrapper
{
    public class BookingWrapper : ModelWrapper<Booking>
    {
        #region Constructors

        public BookingWrapper(Booking model) : base(model)
        {
            Customers = new ObservableCollection<CustomerWrapper>();
            Customers.CollectionChanged += Customers_CollectionChanged;
            Payments.CollectionChanged += Payments_CollectionChanged;
            InitializeBookingWrapper();
        }

        private void Payments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateRemainingAmount();

        }

        private void InitializeBookingWrapper()
        {
            CustomerWrapper c;
            int counter = 0;
            foreach (var res in ReservationsInBooking)
            {
                counter++;
                foreach (var customer in res.CustomersList)
                {
                    c = new CustomerWrapper(customer);
                    if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                    {
                        c.RoomNumber = counter + "-OB";
                    }
                    else if (res.ReservationType == Reservation.ReservationTypeEnum.Normal)
                    {
                        c.RoomNumber = counter.ToString();
                    }
                    else if (res.ReservationType == Reservation.ReservationTypeEnum.Noname)
                    {
                        c.RoomNumber = counter + "-NN";
                    }
                    else if (res.ReservationType == Reservation.ReservationTypeEnum.Transfer)
                    {
                        c.RoomNumber = "TRNS-" + counter;
                    }
                    if (counter % 2 == 0)
                    {
                        c.RoomColor = new SolidColorBrush(Colors.LightPink);
                    }
                    else
                    {
                        c.RoomColor = new SolidColorBrush(Colors.LightBlue);
                    }
                    c.Handled = true;
                    Customers.Add(c);
                }
            }
            if (IsPartners && string.IsNullOrEmpty(FullPriceString))
            {
                CommisionString = Commision.ToString();
                FullPriceString = FullPrice.ToString();
                FullPrice = (Commision == 100) ? 0 : (float)Math.Round(100 * NetPrice / (100 - Commision), 2);

            }
            if (Id == 0)
            {
                Commision = 10;
            }
            CalculateRemainingAmount();
        }

        #endregion Constructors

        #region Fields

        private float _Commision;
        private string _CommisionString;
        private float _FullPrice;
        private string _FullPriceString;
        private float _NetPrice;
        private string _NetPriceString;
        private float _Remaining;
        private bool Calculating;

        #endregion Fields

        #region Properties

        public DateTime CheckIn
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                DateChanged = true;
                if (CheckOut < CheckIn)
                {
                    CheckOut = CheckIn;
                }
            }
        }

        public DateTime CheckOut
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                DateChanged = true;
                if (CheckOut < CheckIn)
                {
                    CheckIn = CheckOut;
                }
            }
        }

        public string Comment
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public Excursion Excursion
        {
            get { return GetValue<Excursion>(); }
            set { SetValue(value); }
        }

        public float Commision
        {
            get
            {
                return GetValue<float>();
            }

            set
            {
                if (_Commision == value)
                {
                    return;
                }
                _Commision = (float)Math.Round(value, 2);
                SetValue(_Commision);
                CommisionString = _Commision.ToString();
                _FullPrice = FullPrice;
                if (IsPartners && _Commision > 0 && _Commision < 100)
                {
                    NetPrice = _FullPrice - _FullPrice * _Commision / 100;
                }
                else if (_Commision == 0)
                {
                    NetPrice = _FullPrice;
                }
            }
        }

        public string CommisionString
        {
            get
            {
                return _CommisionString;
            }

            set
            {
                if (_CommisionString == value)
                {
                    return;
                }
                _CommisionString = value;
                if (!string.IsNullOrEmpty(_CommisionString))
                {
                    if (float.TryParse(value.Replace(',', '.'), NumberStyles.Any, new CultureInfo("en-US"), out float tmpFloat))
                    {
                        tmpFloat = (float)Math.Round(tmpFloat, 2);
                        Commision = tmpFloat;
                        if (_CommisionString[_CommisionString.Length - 1] != '.' && _CommisionString[_CommisionString.Length - 1] != ',')
                        {
                            _CommisionString = tmpFloat.ToString();
                        }
                    }
                    else
                    {
                        _CommisionString = Commision.ToString();
                    }
                }
                else
                {
                    _CommisionString = "0";
                    Commision = 0;
                }
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<CustomerWrapper> _Customers;

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
            }
        }

        public bool DifferentDates
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public float FullPrice
        {
            get
            {
                return _FullPrice;
            }

            set
            {
                if (_FullPrice == value)
                {
                    return;
                }

                _FullPrice = (float)Math.Round(value, 2);
                _Commision = Commision;
                FullPriceString = _FullPrice.ToString();
                if (IsPartners)
                {
                    if (_Commision > 0 && _Commision < 100 && value > 0)
                    {
                        NetPrice = _FullPrice * (1 - _Commision / 100);
                    }
                    else if (_Commision == 0)
                    {
                        NetPrice = _FullPrice;
                    }
                }
            }
        }

        public string FullPriceString
        {
            get
            {
                return _FullPriceString;
            }

            set
            {
                if (_FullPriceString == value)
                {
                    return;
                }
                _FullPriceString = value;
                if (!string.IsNullOrEmpty(_FullPriceString))
                {
                    if (float.TryParse(value.Replace(',', '.'), NumberStyles.Any, new CultureInfo("en-US"), out float tmpFloat))
                    {
                        tmpFloat = (float)Math.Round(tmpFloat, 2);
                        FullPrice = tmpFloat;
                        if (_FullPriceString[_FullPriceString.Length - 1] != '.' && _FullPriceString[_FullPriceString.Length - 1] != ',')
                        {
                            _FullPriceString = tmpFloat.ToString();
                        }
                    }
                    else
                    {
                        _FullPriceString = FullPrice.ToString();
                    }
                }
                else
                {
                    _FullPriceString = "0";
                    FullPrice = 0;
                }
                RaisePropertyChanged();
            }
        }




        private bool _DateChanged = false;

        public bool DateChanged
        {
            get
            {
                return _DateChanged;
            }

            set
            {
                if (_DateChanged == value)
                {
                    return;
                }

                _DateChanged = value;
                RaisePropertyChanged();
            }
        }

        public bool HasManyReservations
        {
            get
            {
                return ReservationsInBooking.Count > 1;
            }
        }

        public bool IsNotPartners => !IsPartners;

        public bool IsPartners
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue(value);

                if (!value)
                {
                    Partner = null;

                    Calculating = true;
                    if (FullPrice > 0)
                    {
                        float tmpPrice = FullPrice / Customers.Count;
                        foreach (CustomerWrapper customer in Customers)
                            customer.Price = tmpPrice;
                    }
                    Calculating = false;
                }

                CalculateRemainingAmount();
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsNotPartners));
            }
        }

        public string Locations => GetLocations();

        public string Names => GetNames();

        public float NetPrice
        {
            get
            {
                return GetValue<float>();
            }

            set
            {
                if (_NetPrice == value)
                {
                    return;
                }

                _NetPrice = (float)Math.Round(value, 2);
                SetValue(_NetPrice);
                NetPriceString = _NetPrice.ToString();
                Remaining = _NetPrice - Recieved;
                FullPrice = (Commision == 100) ? 0 : (float)Math.Round(100 * _NetPrice / (100 - Commision), 2);
                CalculateRemainingAmount();
            }
        }

        public string NetPriceString
        {
            get
            {
                return _NetPriceString;
            }

            set
            {
                if (_NetPriceString == value)
                {
                    return;
                }
                _NetPriceString = value;
                if (!string.IsNullOrEmpty(_NetPriceString))
                {
                    if (float.TryParse(value.Replace(',', '.'), NumberStyles.Any, new CultureInfo("en-US"), out float tmpFloat))
                    {
                        tmpFloat = (float)Math.Round(tmpFloat, 2);
                        NetPrice = tmpFloat;
                        if (_NetPriceString[_NetPriceString.Length - 1] != '.' && _NetPriceString[_NetPriceString.Length - 1] != ',')
                        {
                            _NetPriceString = tmpFloat.ToString();
                        }
                    }
                    else
                    {
                        _NetPriceString = _NetPrice.ToString();
                    }
                }
                else
                {
                    _NetPriceString = "0";
                    NetPrice = 0;
                }
                RaisePropertyChanged();
            }
        }

        public Partner Partner
        {
            get { return GetValue<Partner>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<Payment> Payments
        {
            get { return GetValue<ObservableCollection<Payment>>(); }
        }

        public float Recieved
        {
            get
            {
                float recieved = 0;
                foreach (Payment payment in Payments)
                    recieved += payment.Amount;
                return recieved;
            }
        }

        public float Remaining
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

                if (Math.Abs(_Remaining - value) > 0.001)
                {
                    _Remaining = (float)Math.Round(value, 1);
                    RaisePropertyChanged();
                }
            }
        }

        public List<Reservation> ReservationsInBooking
        {
            get { return GetValue<List<Reservation>>(); }
        }

        public bool SecondDepart
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public User User
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        #region Methods

        public bool AreDatesValid()
        {
            return CheckIn.Year > 2010 && CheckOut.Year > 2010;
        }

        public void CalculateRemainingAmount()
        {
            float total = 0;
            if (IsNotPartners)
            {
                foreach (CustomerWrapper customer in Customers)
                {
                    total += customer.Price;
                }
                FullPrice = total;
                Remaining = total - Recieved;
            }
            else
            {
                if (NetPrice != FullPrice - FullPrice * Commision / 100)
                {
                    FullPrice = (Commision == 100) ? 0 : (float)Math.Round(100 * FullPrice / (100 - Commision), 2);
                }
                Remaining = NetPrice - Recieved;

            }
        }

        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return true;
            }
            key = key.ToLower();
            if (Comment.ToLower().Contains(key) || (IsPartners && Partner.Name.ToLower().Contains(key)))
            {
                return true;
            }
            key = key.ToUpper();
            foreach (var c in Customers)
            {
                if (c.Name.ToUpper().StartsWith(key) || c.Surename.ToUpper().StartsWith(key) || (c.Tel != null && c.Tel.StartsWith(key)) || c.Comment.ToUpper().Contains(key) || (c.Email != null && c.Email.ToUpper().StartsWith(key))
                    || c.PassportNum.ToUpper().StartsWith(key) || c.StartingPlace.ToUpper().StartsWith(key))
                {
                    return true;
                }
            }
            return false;
        }

        int IdCounter;

        //ΤΟΔΟ 
        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomerWrapper.Price) && !Calculating)
                CalculateRemainingAmount();

            //afto einai gia otan vazw atoma apo arxeio thelw vazontas topothesia ston prwto na paei se olus
            if (e.PropertyName == nameof(CustomerWrapper.StartingPlace))
                if (Customers.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Customers[0].StartingPlace))
                    {
                        foreach (CustomerWrapper customer in Customers)
                        {
                            if (string.IsNullOrEmpty(customer.StartingPlace))
                            {
                                customer.StartingPlace = Customers[0].StartingPlace;
                            }
                        }
                    }
                }
            RaisePropertyChanged(nameof(Customers));
        }

        //public void PaymentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (Payment item in e.OldItems)
        //        {
        //            //Removed items
        //            item.PropertyChanged -= EntityViewModelPropertyChanged;
        //        }
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        foreach (Payment item in e.NewItems)
        //        {
        //            //Added items
        //            item.PropertyChanged += EntityViewModelPropertyChanged;
        //        }
        //    }
        //    CalculateRemainingAmount();
        //}

        private void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (CustomerWrapper customer in e.OldItems)
                {
                    //Removed items
                    customer.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (Customers.Count > 1)
                {
                    if (Math.Abs(Customers[Customers.Count - 1].Price) < 0.01)
                    {
                        Customers[Customers.Count - 1].Price = Customers[0].Price;
                    }
                    if (string.IsNullOrEmpty(Customers[Customers.Count - 1].StartingPlace))
                    {
                        Customers[Customers.Count - 1].StartingPlace = Customers[0].StartingPlace;
                    }
                }

                foreach (CustomerWrapper customer in e.NewItems)
                {
                    if (customer.Id == 0)
                    {
                        IdCounter--;
                        customer.Id = --IdCounter;
                    }
                    customer.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
            CalculateRemainingAmount();

            //if (IsPartners && FullPrice > 0 && Customers.Count > 0)
            //{
            //    var tmpPrice = FullPrice / Customers.Count;
            //    foreach (Customer customer in Customers)
            //    {
            //        customer.Price = tmpPrice;
            //    }
            //}
        }

        //private void CustomersChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (Customer c in e.OldItems)
        //        {
        //            foreach (Reservation reservation in ReservationsInBooking)
        //            {
        //                if (reservation.CustomersList.Contains(c))
        //                {
        //                }
        //            }
        //        }
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        foreach (Customer c in e.NewItems)
        //        {
        //            if (c != null && !Customers.Contains(c))
        //                Customers.Add(c);
        //        }
        //    }
        //}

        private string GetLocations()
        {
            List<string> locations = new List<string>();
            foreach (CustomerWrapper customer in Customers)
            {
                if (!locations.Contains(customer.StartingPlace))
                    locations.Add(customer.StartingPlace);
            }
            return string.Join(", ", locations);
        }

        private string GetNames()
        {
            StringBuilder sb = new StringBuilder();
            foreach (CustomerWrapper customer in Customers)
            {
                sb.Append(customer.Surename + " " + customer.Name + ", ");
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        //private void ReservationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (Reservation r in e.OldItems)
        //        {
        //            r.CustomersList.CollectionChanged -= CustomersChanged;

        //            // r.CustomerValueChanged -= CustomersPropertyChanged;
        //        }
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        foreach (Reservation r in e.NewItems)

        //        {
        //            r.CustomersList.CollectionChanged += CustomersChanged;

        //            //Added items
        //            //r.CustomerValueChanged += CustomersPropertyChanged;
        //        }
        //    }
        //}

        #endregion Methods
    }
}