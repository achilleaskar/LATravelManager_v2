using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;

namespace LATravelManager.Model.Wrapper
{
    public class BookingWrapper : ModelWrapper<Booking>
    {

        #region Constructors

        public BookingWrapper() : this(new Booking())
        {
        }

        public BookingWrapper(Booking model, bool v = false) : base(model)
        {
            Customers = new ObservableCollection<CustomerWrapper>();
            Customers.CollectionChanged += Customers_CollectionChanged;
            Payments.CollectionChanged += Payments_CollectionChanged;
            ExtraServices.CollectionChanged += ExtraServices_CollectionChanged;
            ReservationsInBooking.CollectionChanged += ReservationsCollectionChanged;

            InitializeBookingWrapper(v);
        }

        #endregion Constructors

        #region Fields

        public bool PhoneMissing;

        private ObservableCollection<CustomerWrapper> _Customers;

        private ICollectionView _CustomersCV;

        private bool _DateChanged = false;

        private string _DatesError;

        private int _Extras;

        private decimal _FIxedCommision;

        private bool _FreeDatesBool;

        private decimal _FullPrice;

        private bool _Loaded;

        private decimal _Recieved;

        private decimal _Remaining;

        private bool Calculating;

        private int IdCounter;

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
                RaisePropertyChanged(nameof(Nights));
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
                RaisePropertyChanged(nameof(Nights));
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
                if (Math.Abs(value - GetValue<decimal>()) >= 0.01m)
                {
                    SetValue(value);
                    NetPrice = Math.Round(FullPrice - (FullPrice - Extras) * Commision / 100, 2);
                    FIxedCommision = FullPrice - NetPrice;
                    CalculateRemainingAmount();
                }
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

        public decimal EPSILON { get; private set; } = 0.001m;

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

        public int Extras
        {
            get
            {
                return _Extras;
            }

            set
            {
                if (_Extras == value)
                {
                    return;
                }
                if (Math.Abs(Extras - value) > 0.01m)
                    _Extras = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ExtraService> ExtraServices
        {
            get { return GetValue<ObservableCollection<ExtraService>>(); }
        }

        public decimal FIxedCommision
        {
            get
            {
                return _FIxedCommision;
            }

            set
            {
                if (_FIxedCommision == value)
                {
                    return;
                }

                if (Math.Abs(value - _FIxedCommision) >= 0.01m)
                {
                    _FIxedCommision = Math.Round(value, 2);
                    Commision = Math.Round(_FIxedCommision * 100 / (FullPrice - Extras), 2);
                }
                RaisePropertyChanged();
            }
        }

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

                if (Math.Abs(FullPrice - value) > 0.01m)
                    _FullPrice = value;
                RaisePropertyChanged();
                //if (IsPartners)
                //{
                //    if (Commision > 0)
                //    {
                //        NetPrice = Math.Round(FullPrice * (1 - (Commision / 100)), 2);
                //    }
                //    else if (Commision == 0)
                //    {
                //        NetPrice = FullPrice;
                //    }
                //    else
                //    {
                //        NetPrice = 0;
                //    }
                //}
                // CalculateRemainingAmount();
            }
        }

        public bool GroupBooking
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool HasManyReservations
        {
            get
            {
                return ReservationsInBooking.Count > 1;
            }
        }

        public bool IsGroup => Excursion != null && Excursion.ExcursionType.Category == ExcursionTypeEnum.Group && Excursion.FixedDates;

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
                    if (FullPrice >= 0 && Customers.Count > 0)
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

        public bool Loaded
        {
            get
            {
                return _Loaded;
            }

            set
            {
                if (_Loaded == value)
                {
                    return;
                }

                _Loaded = value;
                RaisePropertyChanged();
            }
        }

        public string Locations => GetLocations();

        public string Names => GetNames();

        public decimal NetPrice
        {
            get { return GetValue<decimal>(); }
            set
            {
                if (Math.Abs(NetPrice - value) > 0.01m)
                {
                    SetValue(value);
                    if (FullPrice - Extras > 0)
                    {
                        Commision = 100 * (FullPrice - NetPrice) / (FullPrice - Extras);
                        // Commision = 100 * (FullPrice - NetPrice) / FullPrice;
                        //FullPrice = (Commision == 100) ? 0 : Math.Round(value / (1 - (Commision / 100)), 2);
                        CalculateRemainingAmount();
                    }
                }
            }
        }

        public string Nights
        {
            get { return $"{(int)(CheckOut - CheckIn).TotalDays} νύχτες"; }
        }
        public Partner Partner
        {
            get { return GetValue<Partner>(); }
            set
            {
                SetValue(value);
                CalculateRemainingAmount();
            }
        }

        public string PartnerEmail
        {
            get { return GetValue<string>(); }
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

                if (Math.Abs(_Remaining - value) > 0.01m)
                {
                    _Remaining = Math.Round(value, 2);
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Reservation> ReservationsInBooking
        {
            get { return GetValue<ObservableCollection<Reservation>>(); }
        }

        public bool RoomingListIncluded
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
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

        public bool VoucherSent
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        #region Methods

        public bool AreDatesValid()
        {
            return CheckIn.Year > 2010 && CheckOut.Year > 2010 && (ExcursionDate != null || (Excursion != null && (Excursion.ExcursionType.Category != ExcursionTypeEnum.Group || !Excursion.FixedDates)));
        }

        public void CalculateRemainingAmount()
        {
            if (!Loaded)
                return;
            decimal total = 0;
            int extra = 0;

            _Recieved = 0;

            foreach (var p in Payments)
                _Recieved += p.Amount;
            Recieved = _Recieved;

            foreach (var c in Customers)
            {
                if (c.Price > 1)
                    total += c.Price;
            }

            foreach (var e in ExtraServices)
                extra = e.Amount;
            Extras = extra;

            FullPrice = total + Extras;

            if (IsPartners)
            {
                if (FullPrice - Extras > 0)
                {
                    if (Commision > 0)
                        NetPrice = FullPrice - (FullPrice - Extras) * Commision / 100;
                    else
                        NetPrice = FullPrice;
                }
                if (Partner != null && Partner.Person)
                {
                    Remaining = FullPrice - Recieved;
                    if (FullPrice > 0 && Commision > 0 && NetPrice > 0 && FIxedCommision == 0)
                        FIxedCommision = FullPrice - NetPrice;
                }
                else
                    Remaining = NetPrice - Recieved;
                if (Commision > 0 && FullPrice > 0)
                    FIxedCommision = FullPrice - NetPrice;
            }
            else
                Remaining = FullPrice - Recieved;

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

        public string GetPacketDescription()
        {
            if (Excursion != null)
            {
                return $"ΕΚΔΡΟΜΗ ΓΙΑ {Excursion.Destinations[0].Name} / {new ReservationWrapper(ReservationsInBooking[0]).Dates} / {Customers.Count} ΑΤΟΜΑ / {GetHotels()}";
            }
            else
            {
                return "ERROR";
            }
        }

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
                    if (Math.Abs(Customers[Customers.Count - 1].Price) < 0.001m)
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

        private void ExtraServices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateRemainingAmount();
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
                if ((res.ReservationType == ReservationTypeEnum.Noname || res.ReservationType == ReservationTypeEnum.Overbooked) && res.Hotel != null && !hotels.Any(x => x == res.Hotel.Name))
                {
                    hotels.Add(res.Hotel.Name);
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

        private void InitializeBookingWrapper(bool v = false)
        {
            CustomerWrapper c;
            int counter = 0;
            if (v)
                foreach (Reservation res in ReservationsInBooking.OrderByDescending(t => t.Id))
                {
                    counter++;
                    foreach (Customer customer in res.CustomersList.OrderByDescending(t1 => t1.Id))
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
            else
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
            Loaded = true;
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

        #endregion Methods

    }
}