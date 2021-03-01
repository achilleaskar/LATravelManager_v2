using System;

namespace LATravelManager.Model.Services
{
    public class PlaneService : Service
    {
        #region Constructors

        public PlaneService()
        {
            TimeGo = DateTime.Now.AddDays(7);
            Title = "Αεροπορικό";
            Allerretour = true;
            To = From = "";
            StopArrive = new TimeSpan(0);
            StopLeave = new TimeSpan(0);
        }

        #endregion Constructors

        #region Fields

        private Airline _Airline;

        private string _PNR;

        private bool _Stop = false;

        private string _StopPlace;

        #endregion Fields

        #region Properties

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

        private TimeSpan _StopArrive;

        public TimeSpan StopArrive
        {
            get
            {
                return _StopArrive;
            }

            set
            {
                if (_StopArrive == value)
                {
                    return;
                }

                _StopArrive = value;
                RaisePropertyChanged();
            }
        }

        private TimeSpan _StopLeave;

        public TimeSpan StopLeave
        {
            get
            {
                return _StopLeave;
            }

            set
            {
                if (_StopLeave == value)
                {
                    return;
                }

                _StopLeave = value;
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

                _PNR = value.ToUpper();
                RaisePropertyChanged();
            }
        }

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

                _StopPlace = value.ToUpper();
                RaisePropertyChanged();
            }
        }

        public override string GetDescription()
        {
            return $"Αεροπορικό εισιτήριο{(!string.IsNullOrWhiteSpace(From) ? " από " + From : "") +" "+ (!string.IsNullOrWhiteSpace(To) ? " για " + To : "")}";
        }

        #endregion Properties
    }
}