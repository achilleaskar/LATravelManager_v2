using LATravelManager.BaseTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace LATravelManager.Models
{
    public class Booking : BaseModel
    {
        #region Constructors

        public Booking()
        {
            ReservationsInBooking = new ObservableCollection<Reservation>();
            Payments.CollectionChanged += PaymentCollectionChanged;
            Customers.CollectionChanged += Customers_CollectionChanged;
            ReservationsInBooking.CollectionChanged += ReservationsCollectionChanged;
            CheckIn = DateTime.Today;
        }

        //public Booking(Booking tmpBooking, UnitOfWork uOW)
        //{
        //    Id = tmpBooking.Id;
        //    CheckIn = tmpBooking.CheckIn;
        //    CheckOut = tmpBooking.CheckOut;
        //    Comment = tmpBooking.Comment;
        //    IsPartners = tmpBooking.IsPartners;
        //    FullPrice = tmpBooking.FullPrice;
        //    Commision = tmpBooking.Commision;
        //    SecondDepart = tmpBooking.SecondDepart;
        //    DifferentDates = tmpBooking.DifferentDates;
        //    Partner = (tmpBooking.Partner != null) ? uOW.GenericRepository.GetById<Partner>(tmpBooking.Partner.Id) : null;
        //    User = uOW.GenericRepository.GetById<User>(tmpBooking.User.Id);

        //    Customers.CollectionChanged += Customers_CollectionChanged;
        //    foreach (Customer customer in tmpBooking.Customers)
        //    {
        //        Customers.Add(new Customer(customer, uOW));
        //    }
        //    ReservationsInBooking = new ObservableCollection<Reservation>();
        //    ReservationsInBooking.CollectionChanged += ReservationsCollectionChanged;
        //    foreach (Reservation r in tmpBooking.ReservationsInBooking)
        //    {
        //        ReservationsInBooking.Add(new Reservation(r, uOW));
        //    }
        //    Payments.CollectionChanged += PaymentCollectionChanged;
        //    foreach (var p in tmpBooking.Payments)
        //    {
        //        Payments.Add(new Payment(p));
        //    }
        //    Excursion = uOW.GenericRepository.GetById<Excursion>(tmpBooking.Excursion.Id);
        //    //FullPrice = tmpBooking.FullPrice;
        //    CalculateRemainingAmount();
        //}

        #endregion Constructors

        #region Fields

        public const string CheckInPropertyName = nameof(CheckIn);

        //private void CustomersPropertyChanged(object sender, EventArgs e)
        //{
        public const string CheckOutPropertyName = nameof(CheckOut);

        public const string CommentPropertyName = nameof(Comment);
        public const string CommisionPropertyName = nameof(Commision);
        public const string CommisionStringPropertyName = nameof(CommisionString);
        public const string CustomersPropertyName = nameof(Customers);
        public const string DifferentDatesPropertyName = nameof(DifferentDates);
        public const string ExcursionPropertyName = nameof(Excursion);
        public const string FullPricePropertyName = nameof(FullPrice);
        public const string FullPriceStringPropertyName = nameof(FullPriceString);
        public const string HasManyReservationsPropertyName = nameof(HasManyReservations);
        public const string IsPartnersPropertyName = nameof(IsPartners);
        public const string NetPricePropertyName = nameof(NetPrice);
        public const string NetPriceStringPropertyName = nameof(NetPriceString);
        public const string PartnerPropertyName = nameof(Partner);
        public const string PaymentsPropertyName = nameof(Payments);
        public const string RecievedPropertyName = nameof(Recieved);
        public const string RemainingPropertyName = nameof(Remaining);
        public const string ReservationsInBookingPropertyName = nameof(ReservationsInBooking);
        public const string SecondDepartPropertyName = nameof(SecondDepart);
        public const string UserPropertyName = nameof(User);
        private DateTime _CheckIn;

        private DateTime _CheckOut;

        private string _Comment = string.Empty;

        private float _Commision = 0;

        private string _CommisionString = string.Empty;

        private ObservableCollection<Customer> _Customers = new ObservableCollection<Customer>();

        private bool _DifferentDates = false;

        private Excursion _Excursion;

        private float _FullPrice = 0;

        private string _FullPriceString = string.Empty;

        private bool _IsPartners = false;

        private float _NetPrice = 0;

        private string _NetPriceString = string.Empty;

        private Partner _Partner;

        private ObservableCollection<Payment> _Payments = new ObservableCollection<Payment>();

        private float _Remaining;

        private ObservableCollection<Reservation> _ReservationsInBooking;

        private bool _SecondDepart = false;

        private User _User;

        private bool Calculating;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the CheckIn property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }
                _CheckIn = value;
                if (CheckOut < CheckIn)
                {
                    CheckOut = CheckIn.AddDays(3);
                }
                foreach (var c in Customers)
                {
                    if (!c.Handled)
                    {
                        c.CheckIn = value;
                    }
                }
                RaisePropertyChanged(CheckInPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Checkout property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime CheckOut
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
                if (CheckIn > CheckOut)
                {
                    CheckIn = CheckOut.AddDays(-3);
                }
                foreach (var c in Customers)
                {
                    if (!c.Handled)
                    {
                        c.CheckOut = value;
                    }
                }

                RaisePropertyChanged(CheckOutPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Comment property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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
                RaisePropertyChanged(CommentPropertyName);
            }
        }

        public float Commision
        {
            get
            {
                return _Commision;
            }

            set
            {
                if (_Commision == value)
                {
                    return;
                }
                _Commision = (float)Math.Round(value, 2);
                CommisionString = Commision.ToString();
                if (IsPartners && Commision > 0 && Commision < 100)
                {
                    NetPrice = FullPrice - FullPrice * Commision / 100;
                }
                else if (Commision == 0)
                {
                    NetPrice = FullPrice;
                }

                RaisePropertyChanged(CommisionPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Commision property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        /// <summary>
        /// Sets and gets the CommisionString property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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
                        RaisePropertyChanged(CommisionPropertyName);
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

                RaisePropertyChanged(CommisionStringPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Customers property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public ObservableCollection<Customer> Customers
        {
            get
            {
                //if (_Customers == null)
                //{
                //    _Customers = new ObservableCollection<Customer>();
                //}
                //if (_Customers.Count == 0 && ReservationsInBooking.Count > 0)
                //    foreach (Reservation reservation in ReservationsInBooking)
                //        foreach (Customer customer in reservation.CustomersList)
                //            _Customers.Add(customer);
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                RaisePropertyChanged(CustomersPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the DifferentDates property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool DifferentDates
        {
            get
            {
                return _DifferentDates;
            }

            set
            {
                if (_DifferentDates == value)
                {
                    return;
                }

                _DifferentDates = value;
                RaisePropertyChanged(DifferentDatesPropertyName);
            }
        }

        [NotMapped]
        public double EPSILON { get; private set; } = 0.00001;

        /// <summary>
        /// Sets and gets the Excursion property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public virtual Excursion Excursion
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
                RaisePropertyChanged(ExcursionPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the FullPrice property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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
                FullPriceString = FullPrice.ToString();
                if (IsPartners)
                {
                    if (Commision > 0 && Commision < 100 && value > 0)
                    {
                        NetPrice = FullPrice - FullPrice * Commision / 100;
                    }
                    else if (Commision == 0)
                    {
                        NetPrice = FullPrice;
                    }
                }
                Calculating = true;
                if (IsPartners && FullPrice > 0)
                {
                    float tmpPrice = FullPrice / Customers.Count;
                    foreach (Customer customer in Customers)
                        customer.Price = tmpPrice;
                }
                Calculating = false;

                RaisePropertyChanged(FullPricePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the FullPriceString property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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
                        RaisePropertyChanged(FullPricePropertyName);
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

                RaisePropertyChanged(FullPriceStringPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the HasManyReservations property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        /// [
        [NotMapped]
        public bool HasManyReservations
        {
            get
            {
                return ReservationsInBooking.Count > 1;
            }
        }

        public bool IsNotPartners => !IsPartners;

        /// <summary>
        /// Sets and gets the IsPartners property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsPartners
        {
            get
            {
                return _IsPartners;
            }

            set
            {
                if (_IsPartners == value)
                {
                    return;
                }
                _IsPartners = value;

                if (!value)
                {
                    Partner = null;
                    Calculating = true;
                    if (FullPrice > 0)
                    {
                        float tmpPrice = FullPrice / Customers.Count;
                        foreach (Customer customer in Customers)
                            customer.Price = tmpPrice;
                    }
                    Calculating = false;
                }

                CalculateRemainingAmount();
                RaisePropertyChanged(IsPartnersPropertyName);
                RaisePropertyChanged(nameof(IsNotPartners));
            }
        }

        /// <summary>
        /// Sets and gets the Locations property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string Locations => GetLocations();

        /// <summary>
        /// Sets and gets the Names property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string Names => GetNames();

        /// <summary>
        /// Sets and gets the NeetPrice property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public float NetPrice
        {
            get
            {
                return _NetPrice;
            }

            set
            {
                if (_NetPrice == value)
                {
                    return;
                }

                _NetPrice = (float)Math.Round(value, 2);
                RaisePropertyChanged(NetPricePropertyName);
                NetPriceString = NetPrice.ToString();
                Remaining = NetPrice - Recieved;
                FullPrice = (Commision == 100) ? 0 : (float)Math.Round(100 * NetPrice / (100 - Commision), 2);
                CalculateRemainingAmount();
                RaisePropertyChanged(ReservationsInBookingPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the NetPriceString property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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
                        RaisePropertyChanged(NetPricePropertyName);
                        if (_NetPriceString[_NetPriceString.Length - 1] != '.' && _NetPriceString[_NetPriceString.Length - 1] != ',')
                        {
                            _NetPriceString = tmpFloat.ToString();
                        }
                    }
                    else
                    {
                        _NetPriceString = NetPrice.ToString();
                    }
                }
                else
                {
                    _NetPriceString = "0";
                    NetPrice = 0;
                }
                RaisePropertyChanged(NetPriceStringPropertyName);
            }
        }

        public int NumberOfCustomers
        {
            get
            {
                return Customers.Count;
            }
        }

        /// <summary>
        /// Sets and gets the Partner property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public virtual Partner Partner
        {
            get
            {
                return _Partner;
            }

            set
            {
                if (_Partner == value)
                {
                    return;
                }

                _Partner = value;
                RaisePropertyChanged(PartnerPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Payments property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public virtual ObservableCollection<Payment> Payments
        {
            get
            {
                return _Payments;
            }

            set
            {
                if (_Payments == value)
                {
                    return;
                }

                _Payments = value;
                RaisePropertyChanged(PaymentsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Recieved property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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

        /// <summary>
        /// Sets and gets the Remaining property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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

                if (Math.Abs(_Remaining - value) > EPSILON)
                {
                    _Remaining = (float)Math.Round(value, 1);
                    RaisePropertyChanged(RemainingPropertyName);
                }
            }
        }

        /// <summary>
        /// Sets and gets the ReservationsInBooking property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public virtual ObservableCollection<Reservation> ReservationsInBooking
        {
            get
            {
                return _ReservationsInBooking;
            }

            set
            {
                if (_ReservationsInBooking == value)
                {
                    return;
                }

                _ReservationsInBooking = value;
                RaisePropertyChanged(ReservationsInBookingPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the SecondDepart property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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
                RaisePropertyChanged(SecondDepartPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the User property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public virtual User User
        {
            get
            {
                return _User;
            }

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
                RaisePropertyChanged(UserPropertyName);
            }
        }

        #endregion Properties

        #region Methods

        public void CalculateRemainingAmount()
        {
            float total = 0;
            if (IsNotPartners)
            {
                foreach (Customer customer in Customers)
                {
                    total += customer.Price;
                }
                FullPrice = total;
                Remaining = total - Recieved;
                RaisePropertyChanged(RemainingPropertyName);
            }
            else
            {
                Remaining = NetPrice - Recieved;
                RaisePropertyChanged(RemainingPropertyName);
            }
        }

        public bool Contains(string key)
        {
            key = key.ToUpper();
            if (string.IsNullOrEmpty(key))
            {
                return true;
            }
            if (Comment.ToUpper().Contains(key) || (IsPartners && Partner.Name.ToUpper().Contains(key)))
            {
                return true;
            }
            foreach (var c in Customers)
            {
                if (c.Name.ToUpper().Contains(key) || c.Surename.ToUpper().Contains(key) || (c.Tel != null && c.Tel.Contains(key)) || c.Comment.ToUpper().Contains(key) || (c.Email != null && c.Email.ToUpper().Contains(key))
                    || c.PassportNum.ToUpper().Contains(key) || c.StartingPlace.ToUpper().Contains(key))
                {
                    return true;
                }
            }
            return false;
        }

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Customer.PriceStringPropertyName && !Calculating)
                CalculateRemainingAmount();
            if (e.PropertyName == Customer.StartingPlacePropertyName)
                if (Customers.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Customers[0].StartingPlace))
                    {
                        foreach (Customer customer in Customers)
                        {
                            if (string.IsNullOrEmpty(customer.StartingPlace))
                            {
                                customer.StartingPlace = Customers[0].StartingPlace;
                            }
                        }
                    }
                }
        }

        public void PaymentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Payment item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Payment item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
            CalculateRemainingAmount();
        }

        internal bool AreDatesValid()
        {
            return CheckIn.Year > 2010 && CheckOut.Year > 2010;
        }

        private void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Customer customer in e.OldItems)
                {
                    foreach (Reservation r in ReservationsInBooking)
                    {
                        if (r.CustomersList.Contains(customer))
                        {
                            r.CustomersList.Remove(customer);
                            if (r.CustomersList.Count == 0)
                            {
                                ReservationsInBooking.Remove(r);
                                break;
                            }
                        }
                    }
                    //Removed items
                    customer.PropertyChanged -= EntityViewModelPropertyChanged;
                    CalculateRemainingAmount();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (Customers.Count > 1)
                {
                    if (Math.Abs(Customers[Customers.Count - 1].Price) < EPSILON)
                    {
                        Customers[Customers.Count - 1].Price = Customers[0].Price;
                    }
                    if (string.IsNullOrEmpty(Customers[Customers.Count - 1].StartingPlace))
                    {
                        Customers[Customers.Count - 1].StartingPlace = Customers[0].StartingPlace;
                    }
                }

                foreach (Customer customer in e.NewItems)

                {
                    //customer.CheckIn = CheckIn;
                    //customer.CheckOut = CheckOut;
                    //Added items
                    customer.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }

            if (IsPartners && FullPrice > 0 && Customers.Count > 0)
            {
                var tmpPrice = FullPrice / Customers.Count;
                foreach (Customer customer in Customers)
                {
                    customer.Price = tmpPrice;
                }
            }
            CalculateRemainingAmount();
        }

        private void CustomersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Customer c in e.OldItems)
                {
                    foreach (Reservation reservation in ReservationsInBooking)
                    {
                        if (reservation.CustomersList.Contains(c))
                        {
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Customer c in e.NewItems)
                {
                    if (c != null && !Customers.Contains(c))
                        Customers.Add(c);
                }
            }
        }

        private string GetLocations()
        {
            List<string> locations = new List<string>();
            foreach (Customer customer in Customers)
            {
                if (!locations.Contains(customer.StartingPlace))
                    locations.Add(customer.StartingPlace);
            }
            return string.Join(", ", locations);
        }

        private string GetNames()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Customer customer in Customers)
            {
                sb.Append(customer.Surename + " " + customer.Name + ", ");
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        private void ReservationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Reservation r in e.OldItems)
                {
                    r.CustomersList.CollectionChanged -= CustomersChanged;

                    // r.CustomerValueChanged -= CustomersPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Reservation r in e.NewItems)

                {
                    r.CustomersList.CollectionChanged += CustomersChanged;

                    //Added items
                    //r.CustomerValueChanged += CustomersPropertyChanged;
                }
            }
        }

        #endregion Methods
    }
}