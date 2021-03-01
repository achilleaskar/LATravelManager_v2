using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.Model.Services;
using LATravelManager.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LATravelManager.Model.Wrapper
{
    public class Personal_BookingWrapper : ModelWrapper<Personal_Booking>
    {
        #region Constructors

        public Personal_BookingWrapper() : this(new Personal_Booking())
        {
        }

        public Personal_BookingWrapper(Personal_Booking model) : base(model)
        {
            CustomerWrappers = new ObservableCollection<CustomerWrapper>(Customers.Select(c => new CustomerWrapper(c)));
            foreach (CustomerWrapper customer in CustomerWrappers)
            {
                customer.PropertyChanged += EntityViewModelPropertyChanged;
            }
            CustomerWrappers.CollectionChanged += Customers_CollectionChanged;
            Payments.CollectionChanged += Payments_CollectionChanged;
            Services.CollectionChanged += Services_CollectionChanged;

            CalculateRemainingAmount();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<CustomerWrapper> _CustomerWrappers;

        private string _ErrorsInCanSaveBooking;

        private decimal _FullPrice;

        private decimal _NetPrice;

        private string _PartnerEmail;

        private decimal _Remaining;

        private decimal _ServiceProfit;

        private decimal _TotalProfit;

        #endregion Fields

        #region Properties

        public ObservableCollection<Reciept> Reciepts
        {
            get { return GetValue<ObservableCollection<Reciept>>(); }
            set { SetValue(value); }
        }


        public bool Calculating { get; private set; }

        public string CancelReason
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool CanShowDirections => Partner != null && !string.IsNullOrEmpty(Partner.Note);

        public string Comment
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<Customer> Customers
        {
            get { return GetValue<ObservableCollection<Customer>>(); }
            set { SetValue(value); }
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

        public string ErrorsInCanSaveBooking
        {
            get
            {
                return _ErrorsInCanSaveBooking;
            }

            set
            {
                if (_ErrorsInCanSaveBooking == value)
                {
                    return;
                }

                _ErrorsInCanSaveBooking = value;
                RaisePropertyChanged();
            }
        }

        public decimal ExtraProfit
        {
            get
            {
                return GetValue<decimal>();
            }

            set
            {
                SetValue(value);
                CalculateRemainingAmount();
                RaisePropertyChanged();
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
                ExtraProfit = FullPrice - NetPrice - ServiceProfit;
                RaisePropertyChanged();
            }
        }

        public int IdCounter { get; set; }

        public bool IsNotPartners => !IsPartners;

        public bool IsPartners
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue(value);
                RaisePropertyChanged(nameof(IsNotPartners));
            }
        }

        public string Names => GetNames();

        public decimal NetPrice
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

                _NetPrice = value;
                RaisePropertyChanged();
            }
        }

        public Partner Partner
        {
            get { return GetValue<Partner>(); }
            set
            {
                SetValue(value);
                RaisePropertyChanged("CanShowDirections");
            }
        }

        [NotMapped]
        public string PartnerEmail
        {
            get
            {
                return _PartnerEmail;
            }

            set
            {
                if (_PartnerEmail == value)
                {
                    return;
                }

                _PartnerEmail = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Payment> Payments
        {
            get { return GetValue<ObservableCollection<Payment>>(); }
        }

        public bool PhoneMissing { get; private set; }

        public bool ProformaSent
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool Reciept
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool Group
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public decimal Recieved
        {
            get
            {
                decimal recieved = 0;
                foreach (Payment payment in Payments)
                    recieved += payment.Amount;
                return recieved;
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
                    _Remaining = Math.Round(value, 1);
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ServiceProfit
        {
            get
            {
                return _ServiceProfit;
            }

            set
            {
                if (_ServiceProfit == value)
                {
                    return;
                }

                _ServiceProfit = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Service> Services
        {
            get { return GetValue<ObservableCollection<Service>>(); }
        }

        public decimal TotalProfit
        {
            get
            {
                return _TotalProfit;
            }

            set
            {
                if (_TotalProfit == value)
                {
                    return;
                }

                _TotalProfit = value;
                RaisePropertyChanged();
            }
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

        public DateTime Start => GetStart();

        private DateTime GetStart()
        {
            if (Services == null || Services.Count == 0)
            {
                return CreatedDate;
            }
            else
            {
                return Services.OrderBy(s => s.TimeGo).First().TimeGo;
            }
        }

        #endregion Properties

        #region Methods

        public void CalculateRemainingAmount()
        {
            if (Calculating)
            {
                return;
            }
            decimal tmpNet = 0, tmpProfit = 0;
            foreach (var s in Services)
            {
                tmpNet += s.NetPrice;
                tmpProfit += s.Profit;
            }

            NetPrice = tmpNet;
            ServiceProfit = tmpProfit;
            if (Group)
            {
                Calculating = true;
                if (Customers.Count == 0)
                {
                    ExtraProfit = 0 - NetPrice;
                }
                else
                {
                    ExtraProfit = Customers.Where(r => r.Price > 0).Sum(c => c.Price) - NetPrice - ServiceProfit;
                }
                Calculating = false;
            }
            TotalProfit = ServiceProfit + ExtraProfit;
            FullPrice = NetPrice + TotalProfit;
            Remaining = FullPrice - Recieved;
        }

        public string GetPacketDescription()
        {
            StringBuilder sb = new StringBuilder();
            string hotels = string.Empty, planes = string.Empty, ferrys = string.Empty, guides = string.Empty, transfers = string.Empty, optionals = string.Empty;
            string tmp;
            foreach (var s in Services)
            {
                if (s is HotelService hs)
                {
                    tmp = hs.City != null ? hs.City.Name : "";
                    if (tmp.Length > 1 && !hotels.Contains(tmp))
                        hotels += tmp + "-";
                }
            }
            hotels = hotels.TrimEnd('-');
            if (!string.IsNullOrEmpty(hotels))
            {
                sb.Append("Ατομικό πακέτο για ");
                sb.Append(hotels);
                sb.Append(".");
                return sb.ToString().ToUpper();
            }
            foreach (var s in Services)
            {
                if (s is PlaneService ps)
                {
                    tmp = ps.To ?? "";
                    if (tmp.Length > 1 && !planes.Contains(tmp))
                        planes += tmp + "-";
                }
            }
            planes = planes.TrimEnd('-');
            if (!string.IsNullOrEmpty(planes))
            {
                sb.Append("Αεροπορικό για ");
                sb.Append(planes);
            }

            foreach (var s in Services)
            {
                if (s is FerryService ps)
                {
                    tmp = ps.To ?? "";
                    if (tmp.Length > 1 && !ferrys.Contains(tmp))
                        ferrys += tmp + "-";
                }
            }
            ferrys = ferrys.TrimEnd('-');
            if (!string.IsNullOrEmpty(ferrys))
            {
                if (sb.Length > 1)
                    sb.Append(" & ακτοπλοϊκά για ");
                else
                    sb.Append("Ακτοπλοϊκά για ");
                sb.Append(ferrys);
            }
            foreach (var s in Services)
            {
                if (s is GuideService ps)
                {
                    tmp = ps.From ?? "";
                    if (tmp.Length > 1 && !guides.Contains(tmp))
                        guides += tmp + "-";
                }
            }
            guides = guides.TrimEnd('-');
            if (!string.IsNullOrEmpty(guides))
            {
                if (sb.Length > 1)
                    sb.Append(" & ξενάγηση για ");
                else
                    sb.Append("Ξενάγηση για ");
                sb.Append(guides);
            }
            foreach (var s in Services)
            {
                if (s is TransferService ps)
                {
                    tmp = ps.To ?? "";
                    if (tmp.Length > 1 && !transfers.Contains(tmp))
                        transfers += tmp + "-";
                }
            }
            transfers = transfers.TrimEnd('-');
            if (!string.IsNullOrEmpty(transfers))
            {
                if (sb.Length > 1)
                    sb.Append(" & transfer από ");
                else
                    sb.Append("Transfer από ");
                sb.Append(transfers);
            }
            foreach (var s in Services)
            {
                if (s is OptionalService ps)
                {
                    tmp = ps.CompanyInfo ?? "";
                    if (tmp.Length > 1 && !optionals.Contains(tmp))
                        optionals += tmp + "-";
                }
            }
            optionals = optionals.TrimEnd('-');
            if (!string.IsNullOrEmpty(optionals))
            {
                if (sb.Length > 1)
                    sb.Append(" & ");
                else
                    sb.Append("");
                sb.Append(optionals);
            }
            if (sb.Length > 1)
            {
                sb.Append(".");
            }
            else
                return "";
            return sb.ToString().ToUpper(); ;
        }

        public string ValidatePersonalBooking()
        {
            PhoneMissing = true;
            foreach (CustomerWrapper customer in CustomerWrappers)
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
                return "Δεν έχετε ορίσει ΝΕΤ τιμή!";
            }
            if (IsPartners && Partner == null)
            {
                return "Δεν έχετε επιλέξει συνεργάτη";
            }

            return null;
        }

        internal string GetDestinations()
        {
            StringBuilder sb = new StringBuilder();
            string hotels = string.Empty, planes = string.Empty, ferrys = string.Empty, guides = string.Empty, transfers = string.Empty, optionals = string.Empty;
            string tmp;
            foreach (var s in Services)
            {
                if (s is HotelService hs)
                {
                    tmp = hs.City != null ? hs.City.Name : "";
                    if (tmp.Length > 1 && !hotels.Contains(tmp))
                        hotels += tmp + "-";
                }
            }
            hotels = hotels.TrimEnd('-');
            if (!string.IsNullOrEmpty(hotels))
            {
                sb.Append(hotels);
                return sb.ToString().ToUpper();
            }
            foreach (var s in Services)
            {
                if (s is PlaneService ps)
                {
                    tmp = ps.To ?? "";
                    if (tmp.Length > 1 && !planes.Contains(tmp))
                        planes += tmp + "-";
                }
            }
            planes = planes.TrimEnd('-');
            if (!string.IsNullOrEmpty(planes))
            {
                sb.Append(planes);
            }

            foreach (var s in Services)
            {
                if (s is FerryService ps)
                {
                    tmp = ps.To ?? "";
                    if (tmp.Length > 1 && !ferrys.Contains(tmp))
                        ferrys += tmp + "-";
                }
            }
            ferrys = ferrys.TrimEnd('-');
            if (!string.IsNullOrEmpty(ferrys))
            {
                if (sb.Length > 1)
                    sb.Append("-");
                sb.Append(ferrys);
            }
            foreach (var s in Services)
            {
                if (s is GuideService ps)
                {
                    tmp = ps.From ?? "";
                    if (tmp.Length > 1 && !guides.Contains(tmp))
                        guides += tmp + "-";
                }
            }
            guides = guides.TrimEnd('-');
            if (!string.IsNullOrEmpty(guides))
            {
                if (sb.Length > 1)
                    sb.Append("-");
                sb.Append(guides);
            }
            foreach (var s in Services)
            {
                if (s is TransferService ps)
                {
                    tmp = ps.To ?? "";
                    if (tmp.Length > 1 && !transfers.Contains(tmp))
                        transfers += tmp + "-";
                }
            }
            transfers = transfers.TrimEnd('-');
            if (!string.IsNullOrEmpty(transfers))
            {
                if (sb.Length > 1)
                    sb.Append("-");
                sb.Append(transfers);
            }
            foreach (var s in Services)
            {
                if (s is OptionalService ps)
                {
                    tmp = ps.From ?? "";
                    if (tmp.Length > 1 && !optionals.Contains(tmp))
                        optionals += tmp + "-";
                }
            }
            optionals = optionals.TrimEnd('-');
            if (!string.IsNullOrEmpty(optionals))
            {
                if (sb.Length > 1)
                    sb.Append("-");
                sb.Append(optionals);
            }
            if (sb.Length > 1)
            {
                sb.Append(".");
            }
            else
                return "";
            return sb.ToString().ToUpper(); ;
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
                    customer.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Customers));
            if (e.PropertyName == nameof(Service.NetPrice) || e.PropertyName == nameof(Service.Profit) || e.PropertyName == nameof(Customer.Price))
            {
                CalculateRemainingAmount();
            }
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

        private void Payments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Recieved));
            CalculateRemainingAmount();
        }

        private void Services_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    foreach (Service service in e.OldItems)
            //    {
            //        //Removed items
            //        service.PropertyChanged -= EntityViewModelPropertyChanged;
            //    }
            //}
            //else if (e.Action == NotifyCollectionChangedAction.Add)
            //{
            //    foreach (Service service in e.NewItems)
            //    {
            //        if (service.Id == 0)
            //        {
            //            IdCounter--;
            //            service.Id = --IdCounter;
            //        }
            //        service.PropertyChanged += EntityViewModelPropertyChanged;
            //    }
            //}
            CalculateRemainingAmount();
        }

        #endregion Methods
    }
}