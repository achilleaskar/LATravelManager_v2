﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.UI.Helpers;

namespace LATravelManager.Model.Wrapper
{
    public class Personal_BookingWrapper : ModelWrapper<Personal_Booking>
    {
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

        public bool CanShowDirections => Partner != null && !string.IsNullOrEmpty(Partner.Note);

        public string CancelReason
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool Disabled
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        } public bool VoucherSent
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }
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

        #region Constructors

        public Personal_BookingWrapper() : this(new Personal_Booking())
        {
        }

        public Personal_BookingWrapper(Personal_Booking model) : base(model)
        {
            CustomerWrappers = new ObservableCollection<CustomerWrapper>(Customers.Select(c => new CustomerWrapper(c)));
            CustomerWrappers.CollectionChanged += Customers_CollectionChanged;
            Payments.CollectionChanged += Payments_CollectionChanged;
            Services.CollectionChanged += Services_CollectionChanged;

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

        #endregion Constructors

        private string _ErrorsInCanSaveBooking;

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

        #region Fields

        private decimal _FullPrice;
        private decimal _NetPrice;
        private decimal _Remaining;

        #endregion Fields

        #region Properties

        public bool Calculating { get; private set; }

        public string Comment
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        private ObservableCollection<CustomerWrapper> _CustomerWrappers;

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

        public ObservableCollection<Customer> Customers
        {
            get { return GetValue<ObservableCollection<Customer>>(); }
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
                ExtraProfit = FullPrice - NetPrice - ServiceProfit;
                RaisePropertyChanged();
            }
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
                sb.Append("Ατομικο πακετο για ");
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
                sb.Append("Αεροπορικο για ");
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
                    sb.Append(" & ακτοπλοικα για ");
                else
                    sb.Append("Aκτοπλοικά για ");
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
                    sb.Append(" & ξεναγηση για ");
                else
                    sb.Append("Ξεναγηση για ");
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
                    sb.Append(" & transfer απο ");
                else
                    sb.Append("Transfer απο ");
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

        public ObservableCollection<Payment> Payments
        {
            get { return GetValue<ObservableCollection<Payment>>(); }
        }

        public ObservableCollection<Service> Services
        {
            get { return GetValue<ObservableCollection<Service>>(); }
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

        public User User
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        #region Methods

        private decimal _ServiceProfit;

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

        public void CalculateRemainingAmount()
        {
            decimal tmpNet = 0, tmpProfit = 0;
            foreach (var s in Services)
            {
                tmpNet += s.NetPrice;
                tmpProfit += s.Profit;
            }

            NetPrice = tmpNet;
            ServiceProfit = tmpProfit;
            TotalProfit = ServiceProfit + ExtraProfit;
            FullPrice = NetPrice + TotalProfit;
            Remaining = FullPrice - Recieved;
        }

        private decimal _TotalProfit;
        private string _PartnerEmail;

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

        public bool PhoneMissing { get; private set; }

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
            if (e.PropertyName == nameof(Customer.Services))
            {
                RaisePropertyChanged(nameof(Customers));
            }
            else if (e.PropertyName == nameof(Service.NetPrice) || e.PropertyName == nameof(Service.Profit))
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
                return "Δέν έχετε ορίσει ΝΕΤ τιμή!";
            }
            if (IsPartners && Partner == null)
            {
                return "Δέν έχετε επιλέξει συνεργάτη";
            }

            return null;
        }

        #endregion Methods
    }
}