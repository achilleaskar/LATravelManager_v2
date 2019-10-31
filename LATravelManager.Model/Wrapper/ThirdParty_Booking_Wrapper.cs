using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace LATravelManager.Model.Wrapper
{
    public class ThirdParty_Booking_Wrapper : ModelWrapper<ThirdParty_Booking>
    {
        #region Constructors

        public ThirdParty_Booking_Wrapper() : this(new ThirdParty_Booking())
        {
        }

        public ThirdParty_Booking_Wrapper(ThirdParty_Booking model) : base(model)
        {
            CustomerWrappers = new ObservableCollection<CustomerWrapper>(Customers.Select(c => new CustomerWrapper(c)));
            CustomerWrappers.CollectionChanged += Customers_CollectionChanged;
            Payments.CollectionChanged += Payments_CollectionChanged;
            InitializeBookingWrapper();
        }

        public ObservableCollection<CustomerWrapper> CustomerWrappers
        {
            get
            {
                return _CustomerWrappers;
            }

            set
            {
                if (_CustomerWrappers == value)
                {
                    return;
                }

                _CustomerWrappers = value;
                RaisePropertyChanged();
            }
        }

        #endregion Constructors

        #region Fields

        public bool PhoneMissing;
        private string _DatesError;
        private decimal _FullPrice;
        private decimal _Recieved;
        private decimal _Remaining;
        private int IdCounter;
        private ObservableCollection<CustomerWrapper> _CustomerWrappers;

        #endregion Fields

        #region Properties

        public string CancelReason
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<ChangeInBooking> ChangesInBooking
        {
            get { return GetValue<ObservableCollection<ChangeInBooking>>(); }
        }

        public DateTime CheckIn
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                if (CheckOut < CheckIn)
                {
                    CheckOut = CheckIn.AddDays(3);
                }

                DatesError = ValidateFromDatetime();
            }
        }

        public DateTime CheckOut
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                if (CheckOut < CheckIn)
                {
                    CheckIn = CheckOut;
                }
                DatesError = ValidateToDatetime();
            }
        }

        public string Comment
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public CustomFile File
        {
            get { return GetValue<CustomFile>(); }
            set { SetValue(value); }
        }

        public decimal Commision
        {
            get { return GetValue<decimal>(); }
            set
            {
                if (value == GetValue<decimal>())
                {
                    return;
                }
                SetValue(value);
                // if (FullPrice > 0)
                NetPrice = FullPrice - Commision;
                //else if (NetPrice > 0)
                //{
                //    FullPrice = NetPrice + Commision;
                //}
                CalculateRemainingAmount();
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get { return GetValue<ObservableCollection<Customer>>(); }
            set { SetValue(value); }
        }

        public string DatesError
        {
            get { return _DatesError; }
            set
            {
                if (_DatesError == value)
                {
                    return;
                }
                _DatesError = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool Disabled
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public DateTime? DisableDate
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public User DisabledBy
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        public decimal FullPrice
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

                _FullPrice = value;
                RaisePropertyChanged();
                if (value > 0 && NetPrice > 0)
                    Commision = FullPrice - NetPrice;
                CalculateRemainingAmount();
            }
        }

        public string Names => GetValue<string>();

        public decimal NetPrice
        {
            get { return GetValue<decimal>(); }
            set
            {
                if (value == GetValue<decimal>())
                {
                    return;
                }
                SetValue(value);
                //if (Commision > 0)
                //{
                //    FullPrice = NetPrice + Commision;
                //}
                //else if (FullPrice > 0)
                //{
                Commision = FullPrice - NetPrice;
                //}
                CalculateRemainingAmount();
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

        public bool Reciept
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public decimal Recieved
        {
            set
            {
                if (value != _Recieved)
                {
                    _Recieved = value;
                }
                RaisePropertyChanged();
            }

            get
            {
                return _Recieved;
            }
        }

        public decimal Remaining
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

                if (Math.Abs(_Remaining - value) > 0.0001m)
                {
                    _Remaining = Math.Round(value, 2);
                    RaisePropertyChanged();
                }
            }
        }

        private decimal _NetRemaining;

        public decimal NetRemaining
        {
            get
            {
                return _NetRemaining;
            }

            set
            {
                if (_NetRemaining == value)
                {
                    return;
                }

                if (Math.Abs(_NetRemaining - value) > 0.0001m)
                {
                    _NetRemaining = Math.Round(value, 2);
                    RaisePropertyChanged();
                }
            }
        }

        public string Stay
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public string City
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
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
            _Recieved = 0;
            _NetRemaining = NetPrice;
            foreach (Payment payment in Payments)
                if (payment.Outgoing)
                    _NetRemaining -= payment.Amount;
                else
                    _Recieved += payment.Amount;
            Recieved = _Recieved;

            Remaining = FullPrice - Recieved;
            RaisePropertyChanged(nameof(NetRemaining));
        }

        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return true;
            }
            key = key.ToLower();
            if (Comment.ToLower().Contains(key) || Description.ToLower().Contains(key) || Stay.ToLower().Contains(key) || City.ToLower().Contains(key) || Partner.Name.ToLower().Contains(key))
            {
                return true;
            }
            key = key.ToUpper();
            foreach (CustomerWrapper c in CustomerWrappers)
            {
                if (c.Name.ToUpper().StartsWith(key) || c.Surename.ToUpper().StartsWith(key) || (c.Tel != null && c.Tel.StartsWith(key)) || c.Comment.ToUpper().Contains(key) || (c.Email != null && c.Email.ToUpper().StartsWith(key))
                    || c.PassportNum.ToUpper().StartsWith(key) || c.StartingPlace.ToUpper().StartsWith(key))
                {
                    return true;
                }
            }
            return false;
        }

        //ΤΟΔΟ
        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomerWrapper.Price))
                CalculateRemainingAmount();

            //afto einai gia otan vazw atoma apo arxeio thelw vazontas topothesia ston prwto na paei se olus
            if (e.PropertyName == nameof(CustomerWrapper.StartingPlace))
                if (Customers.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Customers[0].StartingPlace))
                    {
                        foreach (CustomerWrapper customer in CustomerWrappers)
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

        public string GetPacketDescription()
        {
            if (!string.IsNullOrEmpty(Description))
            {
                return Description;
            }
            else
            {
                return "ERROR";
            }
        }

        public string ValidateThirdPartyBooking()
        {
            PhoneMissing = true;
            foreach (CustomerWrapper customer in CustomerWrappers)
            {
                if (PhoneMissing)
                {
                    if (customer.Tel != null && customer.Tel.Length >= 10)
                    {
                        PhoneMissing = false;
                    }
                }
                if (customer.HasErrors)
                {
                    return customer.GetFirstError();
                }
            }
            if (Customers.Count <= 0)
            {
                return "Προσθέστε Πελάτες!";
            }
            if (PhoneMissing)
            {
                return "Παρακαλώ προσθέστε έστω έναν αριθμό τηλεφώνου!";
            }
            if (NetPrice <= 0)
            {
                return "Δέν έχετε ορίσει ΝΕΤ τιμή!";
            }
            if (Partner == null)
            {
                return "Δέν έχετε επιλέξει συνεργάτη";
            }
            if (string.IsNullOrEmpty(Description) || Description.Length < 10)
                return "Προσθέστε επαρκή επεξήγηση";
            if (string.IsNullOrEmpty(Stay) || Stay.Length < 3)
                return "Προσθέστε πόλη διαμονής";

            return null;
        }

        private void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (CustomerWrapper customer in e.OldItems)
                {
                    //Removed items
                    customer.PropertyChanged -= EntityViewModelPropertyChanged;
                    Customers.Remove(customer.Model);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (CustomerWrapper customer in e.NewItems)
                {
                    if (customer.Id == 0)
                    {
                        IdCounter--;
                        customer.Id = --IdCounter;
                        Customers.Add(customer.Model);
                    }
                    if (Customers.Count > 1)
                    {
                        if (string.IsNullOrEmpty(customer.StartingPlace))
                        {
                            customer.StartingPlace = Customers[0].StartingPlace;
                        }
                    }
                    customer.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
            CalculateRemainingAmount();
        }

        //private string GetHotels()
        //{
        //    if (!string.IsNullOrEmpty(Stay))
        //    {
        //        return Description;
        //    }
        //    else
        //    {
        //        return "ERROR";
        //    }
        //}

        private void InitializeBookingWrapper()
        {
            CalculateRemainingAmount();
        }

        private void Payments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Recieved));
            CalculateRemainingAmount();
        }

        private string ValidateFromDatetime()
        {
            if (CheckIn < DateTime.Now.AddDays(-1))
            {
                return "Η επιλεγμένη ημερομηνία έναρξης έχει παρέλθει.";
            }
            return null;
        }

        private string ValidateToDatetime()
        {
            if (CheckOut < DateTime.Now)
            {
                return "Η επιλεγμένη ημερομηνία επιστροφής έχει παρέλθει!";
            }
            if (CheckOut < CheckIn)
            {
                return "Η επιλεγμένη ημερομηνία επιστροφής είναι νωρίτερα από την ημερομηνία έναρξης.";
            }
            return null;
        }

        #endregion Methods
    }
}