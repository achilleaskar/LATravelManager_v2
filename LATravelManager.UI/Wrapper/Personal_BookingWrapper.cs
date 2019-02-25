using LATravelManager.Model.Booking;
using LATravelManager.Model.Services;
using LATravelManager.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace LATravelManager.UI.Wrapper
{
    public class Personal_BookingWrapper : ModelWrapper<Personal_Booking>
    {
        #region Constructors

        public Personal_BookingWrapper() : base(new Personal_Booking())
        {
        }

        public Personal_BookingWrapper(Personal_Booking model) : base(model)
        {
            Customers.CollectionChanged += Customers_CollectionChanged;
            Payments.CollectionChanged += Payments_CollectionChanged;
            Services.CollectionChanged += Services_CollectionChanged;
        }
        public int IdCounter { get; set; }



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
                foreach (Customer customer in e.NewItems)
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
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        private void Payments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Services_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           
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

        #endregion Fields

        #region Properties

        public bool Calculating { get; private set; }

        public string Comment
        {
            get { return GetValue<string>(); }
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
                    NetPrice = _FullPrice - (_FullPrice * _Commision / 100);
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

        public ObservableCollection<Customer> Customers
        {
            get { return GetValue<ObservableCollection<Customer>>(); }
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
                        foreach (Customer customer in Customers)
                            customer.Price = tmpPrice;
                    }
                    Calculating = false;
                }
                CommisionString = Commision.ToString();

                CalculateRemainingAmount();
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsNotPartners));
            }
        }

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

        public ObservableCollection<Service> Services
        {
            get { return GetValue<ObservableCollection<Service>>(); }
        }

        public User User
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        private void CalculateRemainingAmount()
        {
            throw new NotImplementedException();
        }
        private string GetNames()
        {
            throw new NotImplementedException();
        }
        #endregion Properties
    }
}