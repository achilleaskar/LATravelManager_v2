using LATravelManager.Model.People;
using System;
using System.Collections.ObjectModel;

namespace LATravelManager.Model.Services
{
    public class PlaneService : Service
    {
        public PlaneService()
        {
            FlightGo = DateTime.Now.AddDays(7);
            Tittle = "Αεροπορικό";
            Allerretour = true;
        }

        private string _FlyFrom;

        private string _PNR;

        private DateTime? _WaitingTime;

        public DateTime? WaitingTime
        {
            get
            {
                return _WaitingTime;
            }

            set
            {
                if (_WaitingTime == value)
                {
                    return;
                }

                _WaitingTime = value;
                RaisePropertyChanged();
            }
        }

        public string PNR
        {
            get
            {
                return _PNR;
            }

            set
            {
                if (_PNR == value)
                {
                    return;
                }

                _PNR = value;
                RaisePropertyChanged();
            }
        }

        public string FlyFrom
        {
            get
            {
                return _FlyFrom;
            }

            set
            {
                if (_FlyFrom == value)
                {
                    return;
                }

                _FlyFrom = value;
                RaisePropertyChanged();
            }
        }

        private string _FlyTo;

        private Airline _Airline;

        public Airline Airline
        {
            get
            {
                return _Airline;
            }

            set
            {
                if (_Airline == value)
                {
                    return;
                }

                _Airline = value;
                RaisePropertyChanged();
            }
        }

        public string FlyTo
        {
            get
            {
                return _FlyTo;
            }

            set
            {
                if (_FlyTo == value)
                {
                    return;
                }

                _FlyTo = value;
                RaisePropertyChanged();
            }
        }

        private bool _Stop = false;

        public bool Stop
        {
            get
            {
                return _Stop;
            }

            set
            {
                if (_Stop == value)
                {
                    return;
                }

                _Stop = value;
                RaisePropertyChanged();
            }
        }

        private bool _Allerretour = false;

        private ObservableCollection<Customer> _Customers;

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

        private DateTime _FlightReturn;

        public DateTime FlightReturn
        {
            get
            {
                return _FlightReturn;
            }

            set
            {
                if (_FlightReturn == value)
                {
                    return;
                }

                _FlightReturn = value;
                if (_FlightReturn < FlightGo)
                {
                    FlightGo = FlightReturn.AddDays(-3);
                }
                RaisePropertyChanged();
            }
        }

        private DateTime _FlightGo;

        public DateTime FlightGo
        {
            get
            {
                return _FlightGo;
            }

            set
            {
                if (_FlightGo == value)
                {
                    return;
                }

                _FlightGo = value;
                if (_FlightGo > FlightReturn)
                {
                    FlightReturn = _FlightGo.AddDays(3);
                }
                RaisePropertyChanged();
            }
        }

        private string _StopPlace;

        public string StopPlace
        {
            get
            {
                return _StopPlace;
            }

            set
            {
                if (_StopPlace == value)
                {
                    return;
                }

                _StopPlace = value;
                RaisePropertyChanged();
            }
        }
    }
}