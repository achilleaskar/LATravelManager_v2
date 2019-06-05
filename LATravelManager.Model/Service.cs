using LATravelManager.Model.People;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Services
{
    public abstract class Service : EditTracker
    {
        public Service()
        {
            TimeGo = DateTime.Now;
        }
        private string _From;




        private decimal _Profit;


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

        private bool _Allerretour;

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
        [NotMapped]
        public string Tittle { get; set; }

        private DateTime _TimeReturn;
        private DateTime _TimeGo;

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

        private string _CompanyInfo;

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
                if (_TimeReturn < TimeGo)
                {
                    TimeGo = TimeReturn.AddDays(-3);
                }
                RaisePropertyChanged();
            }
        }

        public decimal PricePerPerson => Customers == null || Customers.Count == 0 || NetPrice == 0 ? 0 : NetPrice / Customers.Count;

     



        private decimal _NetPrice;
        private string _To;

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

        public List<Customer> Customers { get; set; }

        public override string ToString()
        {
            return Tittle;
        }
    }

}