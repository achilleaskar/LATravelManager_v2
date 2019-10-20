using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Lists
{
    public class Bus : EditTracker
    {
        #region Fields

        private ObservableCollection<Customer> _Customers = new ObservableCollection<Customer>();
        private Excursion _Excursion;
        private Leader _Leader;
        private bool _OneWay;
        private string _StartingPlace = string.Empty;

        private DateTime _TimeGo;

        private DateTime _TimeReturn;

        private Vehicle _Vehicle;

        #endregion Fields

        #region Properties

        public ObservableCollection<Customer> Customers
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

        public Excursion Excursion
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
                RaisePropertyChanged();
            }
        }

        public Leader Leader
        {
            get
            {
                return _Leader;
            }

            set
            {
                if (_Leader == value)
                {
                    return;
                }

                _Leader = value;
                RaisePropertyChanged();
            }
        }

        public bool OneWay
        {
            get
            {
                return _OneWay;
            }

            set
            {
                if (_OneWay == value)
                {
                    return;
                }

                _OneWay = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(20)]
        public string StartingPlace
        {
            get
            {
                return _StartingPlace;
            }

            set
            {
                if (_StartingPlace == value)
                {
                    return;
                }

                _StartingPlace = value;
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
                RaisePropertyChanged();
            }
        }

        public Vehicle Vehicle
        {
            get
            {
                return _Vehicle;
            }

            set
            {
                if (_Vehicle == value)
                {
                    return;
                }

                _Vehicle = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}