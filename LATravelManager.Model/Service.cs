using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Notifications;
using LATravelManager.Model.People;

namespace LATravelManager.Model.Services
{
    public abstract class Service : EditTracker
    {
        #region Fields

        private bool _Allerretour;
        private string _CompanyInfo;
        private string _From;

        private decimal _NetPrice;
        private decimal _Profit;

        private DateTime _TimeGo = DateTime.Now;

        private DateTime _TimeReturn;

        private string _To;

        #endregion Fields

        public bool Reciept { get; set; }

        private NotifStatus _NotifStatus;

        public NotifStatus NotifStatus
        {
            get
            {
                return _NotifStatus;
            }

            set
            {
                if (_NotifStatus == value)
                {
                    return;
                }

                _NotifStatus = value;
                RaisePropertyChanged();
            }
        }

        #region Properties

        public bool Allerretour
        {
            get
            {
                return _Allerretour;
            }

            set
            {
                if (_Allerretour == value)
                {
                    return;
                }

                _Allerretour = value;
                RaisePropertyChanged();
            }
        }
        [MaxLength(50)]
        public string CompanyInfo
        {
            get
            {
                return _CompanyInfo;
            }

            set
            {
                if (_CompanyInfo == value)
                {
                    return;
                }

                _CompanyInfo = value;
                RaisePropertyChanged();
            }
        }

        public List<Customer> Customers { get; set; }
        [MaxLength(50)]
        public string From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                _From = value;
                RaisePropertyChanged();
            }
        }

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

        public Personal_Booking Personal_Booking { get; set; }
        public decimal PricePerPerson => Customers == null || Customers.Count == 0 || NetPrice == 0 ? 0 : NetPrice / Customers.Count;

        public decimal Profit
        {
            get
            {
                return _Profit;
            }

            set
            {
                if (_Profit == value)
                {
                    return;
                }

                _Profit = value;
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
                if (_TimeGo > TimeReturn)
                {
                    TimeReturn = _TimeGo.AddDays(3);
                }
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
                if (_TimeReturn < TimeGo && TimeGo.Year > 100 && TimeReturn.Year > 100)
                {
                    TimeGo = TimeReturn.AddDays(-3);
                }
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public string Title { get; set; }
        [MaxLength(50)]
        public string To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }

                _To = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Title;
        }


        public abstract string GetDescription();
       
        
        
        public virtual string GetDates()
        {
            return TimeGo.ToString("dd/MM") + (Allerretour ? ("-" + TimeReturn.ToString("dd/MM/yy")) : TimeGo.ToString("/yy"));
        }
        #endregion Methods
    }
}