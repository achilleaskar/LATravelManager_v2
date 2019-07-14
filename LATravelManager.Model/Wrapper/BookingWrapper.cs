using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using static LATravelManager.Model.Enums;

namespace LATravelManager.Model.Wrapper
{
    public class BookingWrapper : ModelWrapper<Booking>
    {
        #region Constructors

        public BookingWrapper() : this(new Booking())
        {
        }

        public BookingWrapper(Booking model) : base(model)
        {
            Customers = new ObservableCollection<CustomerWrapper>();
            Customers.CollectionChanged += Customers_CollectionChanged;
            Payments.CollectionChanged += Payments_CollectionChanged;
            ReservationsInBooking.CollectionChanged += ReservationsCollectionChanged;

            InitializeBookingWrapper();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<CustomerWrapper> _Customers;
        private bool _DateChanged = false;
        private decimal _FullPrice;
        private decimal _Recieved;
        private decimal _Remaining;
        private bool Calculating;
        private int IdCounter;

        #endregion Fields

        #region Properties

        private ICollectionView _CustomersCV;

        public ICollectionView CustomersCV
        {
            get
            {
                return _CustomersCV;
            }

            set
            {
                if (_CustomersCV == value)
                {
                    return;
                }

                _CustomersCV = value;
                RaisePropertyChanged();
            }
        }

        private string ValidateToDatetime()
        {
            if (CheckOut < DateTime.Now)
            {
                return "Η επιλεγμένη ημερομηνία επιστροφής έχει παρέλθει!";
            }
            if (CheckOut < CheckIn)
            {
                return "Η επιλεγμένη ημερομηνία επιστροφής είναι νωρίτερα απο την ημερομηνία έναρξης.";
            }
            //if (CheckOut == CheckIn)
            //{
            //    return "Η ημερομηνία επιστροφής δεν΄μπορεί να είναι η ίδια με την ημερομηνία έναρξης.";
            //}
            if ((CheckOut.DayOfWeek == DayOfWeek.Monday || CheckOut.DayOfWeek == DayOfWeek.Tuesday || CheckOut.DayOfWeek == DayOfWeek.Friday) && !FreeDatesBool)
            {
                return "Επιτρεπόμενες μέρες επιστροφής μόνο Τετάρτη, Πέμπτη, Σάββατο και Κυριακή!";
            }
            return null;
        }

        private bool _FreeDatesBool;

        public bool FreeDatesBool
        {
            get { return _FreeDatesBool; }
            set
            {
                if (_FreeDatesBool == value)
                {
                    return;
                }
                _FreeDatesBool = value;
                RaisePropertyChanged();
                ValidateFromDatetime();
                ValidateToDatetime();
            }
        }

        private string ValidateFromDatetime()
        {
            if (CheckIn < DateTime.Now.AddDays(-1) && !FreeDatesBool)
            {
                return "Η επιλεγμένη ημερομηνία έναρξης έχει παρέλθει.";
            }
            if ((CheckIn.DayOfWeek == DayOfWeek.Monday || CheckIn.DayOfWeek == DayOfWeek.Tuesday || CheckIn.DayOfWeek == DayOfWeek.Friday) && !FreeDatesBool)
            {
                return "Επιτρεπόμενες μέρες αναχώρησης μόνο Τετάρτη, Πέμπτη, Σάββατο και Κυριακή!";
            }
            return null;
        }

        public DateTime CheckIn
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                DateChanged = true;
                if (CheckOut < CheckIn)
                {
                    CheckOut = CheckIn.AddDays(3);
                }
                DifferentDates = false;
                foreach (Reservation r in ReservationsInBooking)
                {
                    foreach (Customer c in r.CustomersList)
                    {
                        c.CheckIn = value;
                    }
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
                DateChanged = true;
                if (CheckOut < CheckIn)
                {
                    CheckIn = CheckOut;
                }
                DifferentDates = false;
                foreach (Reservation r in ReservationsInBooking)
                {
                    foreach (Customer c in r.CustomersList)
                    {
                        c.CheckOut = value;
                    }
                }
                DatesError = ValidateToDatetime();
            }
        }

        public string Comment
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public decimal Commision
        {
            get { return GetValue<decimal>(); }
            set
            {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                SetValue(value);
                NetPrice = FullPrice - FullPrice * Commision / 100;
                CalculateRemainingAmount();
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
                CustomersCV = CollectionViewSource.GetDefaultView(Customers);
            }
        }

        public string GetPacketDescription()
        {
            if (Excursion != null)
            {
                return $"ΕΚΔΡΟΜΗ ΓΙΑ {Excursion.Destinations[0].Name.TrimEnd('Σ')} / {new ReservationWrapper(ReservationsInBooking[0]).Dates} / {Customers.Count} ΑΤΟΜΑ / {GetHotels()}";
            }
            else
            {
                return "ERROR";
            }
        }

        private string GetHotels()
        {
            List<string> hotels = new List<string>();
            foreach (var res in ReservationsInBooking)
            {
                if (res.Room != null && !hotels.Any(x => x == res.Room.Hotel.Name))
                {
                    hotels.Add(res.Room.Hotel.Name);
                }
                if (res.ReservationType == ReservationTypeEnum.Noname && !hotels.Any(x => x == "NO NAME"))
                {
                    hotels.Add("NO NAME");
                }
                if (res.ReservationType == ReservationTypeEnum.Transfer && !hotels.Any(x => x == "TRANSFER"))
                {
                    hotels.Add("TRANSFER");
                }
                if (res.ReservationType == ReservationTypeEnum.OneDay && !hotels.Any(x => x == "ONEDAY"))
                {
                    hotels.Add("ONEDAY");
                }
            }
            if (hotels.Count == 1)
            {
                if (hotels[0] == "TRANSFER")
                {
                    return "ΜΟΝΟ ΜΕΤΑΦΟΡΑ";
                }
                else if (hotels[0] == "NO NAME")
                {
                    return "ΧΩΡΙΣ ΕΠΙΛΟΓΗ ΣΥΓΚΕΚΡΙΜΕΝΟΥ ΚΑΤΑΛΥΜΑΤΟΣ";
                }
                else if (hotels[0] == "ONEDAY")
                {
                    return "ΗΜΕΡΗΣΙΑ ΕΚΔΡΟΜΗ";
                }
                else
                {
                    return "ΜΕ ΔΙΑΜΟΝΗ ΣΤΟ ΚΑΤΑΛΥΜΑ " + hotels[0];
                }
            }
            else if (hotels.Count > 1)
            {
                return "ΜΕ ΔΙΑΜΟΝΗ ΣΤA ΚΑΤΑΛΥΜΑTA " + string.Join(" / ", hotels);
            }
            else
            {
                return "ERROR";
            }
        }

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

        public bool DifferentDates
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
                foreach (CustomerWrapper c in Customers)
                {
                    c.CheckIn = CheckIn;
                    c.CheckOut = CheckOut;
                }
            }
        }

        public Excursion Excursion
        {
            get { return GetValue<Excursion>(); }
            set { SetValue(value); }
        }

        public ExcursionDate ExcursionDate
        {
            get { return GetValue<ExcursionDate>(); }
            set
            {
                SetValue(value);
                CheckIn = value.CheckIn;
                CheckOut = value.CheckOut;
            }
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
                if (IsPartners)
                {
                    if (Commision > 0)
                    {
                        NetPrice = FullPrice * (1 - (Commision / 100));
                    }
                    else if (Commision == 0)
                    {
                        NetPrice = FullPrice;
                    }
                    else
                    {
                        NetPrice = 0;
                    }
                }
                CalculateRemainingAmount();
            }
        }

        public bool HasManyReservations
        {
            get
            {
                return ReservationsInBooking.Count > 1;
            }
        }

        public bool IsGroup => Excursion!=null && Excursion.ExcursionType.Category == ExcursionTypeEnum.Group;
        public bool IsNotGroup => !IsGroup;

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
                    if (FullPrice >= 0)
                    {
                        decimal tmpPrice = Math.Round(FullPrice / Customers.Count, 2);
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

        public decimal NetPrice
        {
            get { return GetValue<decimal>(); }
            set
            {
                SetValue(value);
                if (Commision > 0)
                {
                    FullPrice = (Commision == 100) ? 0 : Math.Round(value / (1 - (Commision / 100)), 2);
                }
                CalculateRemainingAmount();
            }
        }

        public Partner Partner
        {
            get { return GetValue<Partner>(); }
            set { SetValue(value); }
        }

        public bool Reciept
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<Payment> Payments
        {
            get { return GetValue<ObservableCollection<Payment>>(); }
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
                    _Remaining = Math.Round(value, 1);
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Reservation> ReservationsInBooking
        {
            get { return GetValue<ObservableCollection<Reservation>>(); }
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

        public string PartnerEmail
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        private string _DatesError;

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

        #region Methods

        public bool AreDatesValid()
        {
            return CheckIn.Year > 2010 && CheckOut.Year > 2010 && (ExcursionDate != null || (Excursion!=null&& Excursion.ExcursionType.Category != ExcursionTypeEnum.Group));
        }

        public void CalculateRemainingAmount()
        {
            decimal total = 0;

            _Recieved = 0;
            foreach (Payment payment in Payments)
                _Recieved += payment.Amount;
            Recieved = _Recieved;

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
            foreach (CustomerWrapper c in Customers)
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
                    if (Math.Abs(Customers[Customers.Count - 1].Price) < 0.00001m)
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
        private string GetNames()
        {
            StringBuilder sb = new StringBuilder();
            foreach (CustomerWrapper customer in Customers)
            {
                sb.Append(customer.Surename + " " + customer.Name + ", ");
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        private void InitializeBookingWrapper()
        {
            CustomerWrapper c;
            int counter = 0;
            foreach (Reservation res in ReservationsInBooking)
            {
                counter++;
                foreach (Customer customer in res.CustomersList)
                {
                    c = new CustomerWrapper(customer);
                    if (res.ReservationType == ReservationTypeEnum.Overbooked)
                    {
                        c.RoomNumber = counter + "-OB";
                    }
                    else if (res.ReservationType == ReservationTypeEnum.Normal)
                    {
                        c.RoomNumber = counter.ToString();
                    }
                    else if (res.ReservationType == ReservationTypeEnum.Noname)
                    {
                        c.RoomNumber = counter + "-NN";
                    }
                    else if (res.ReservationType == ReservationTypeEnum.Transfer)
                    {
                        c.RoomNumber = "TRNS-" + counter;
                    }
                    else if (res.ReservationType == ReservationTypeEnum.OneDay)
                    {
                        c.RoomNumber = "OD-" + counter;
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
            if (IsPartners)
            {
                FullPrice = (Commision == 100) ? 0 : Math.Round(100 * NetPrice / (100 - Commision), 2);
            }
            if (Id == 0)
            {
                Commision = 10;
            }
            CalculateRemainingAmount();
        }

        private void Payments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Recieved));
            CalculateRemainingAmount();
        }

        private void ReservationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ReservationsInBooking.Count > 0)
            {
                ReservationWrapper res0 = new ReservationWrapper(ReservationsInBooking[0]);
                DateTime min = res0.CheckIn;
                DateTime max = res0.CheckOut;
                if (ReservationsInBooking.Count > 1)
                {
                    for (int i = 1; i < ReservationsInBooking.Count; i++)
                    {
                        res0 = new ReservationWrapper(ReservationsInBooking[i]);
                        if (res0.CheckIn < min)
                        {
                            min = res0.CheckIn;
                        }
                        if (res0.CheckIn > max)
                        {
                            max = res0.CheckOut;
                        }
                    }
                }

                if (min.Year > 2000 && max.Year > 2000)
                {
                    if (CheckIn != min || CheckOut != max)
                    {
                        CheckIn = min;
                        CheckOut = max;
                    }
                }
            }
        }

        public bool PhoneMissing;

        public string ValidateBooking()
        {
            PhoneMissing = true;
            foreach (CustomerWrapper customer in Customers)
            {
                if (PhoneMissing)
                {
                    if (IsPartners || (customer.Tel != null && customer.Tel.Length >= 10))
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
            if (PhoneMissing && !IsPartners)
            {
                return "Παρακαλώ προσθέστε έστω έναν αριθμό τηλεφώνου!";
            }
            if (IsPartners && NetPrice <= 0)
            {
                return "Δέν έχετε ορίσει ΝΕΤ τιμή!";
            }
            if (IsPartners && Partner == null)
            {
                return "Δέν έχετε επιλέξει συνεργάτη";
            }
            if (Excursion != null && Excursion.ExcursionType.Category == ExcursionTypeEnum.Group && Excursion.ExcursionDates == null)
            {
                return "Παρακαλώ επιλέξτε ημερομηνίες";
            }

            return null;
        }

        #endregion Methods
    }
}