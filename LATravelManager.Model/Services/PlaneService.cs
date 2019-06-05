using LATravelManager.Model;
using LATravelManager.Model.Services;
using System;

namespace LATravelManager.Model.Services
{
    public class PlaneService : Service
    {
        #region Constructors

        public PlaneService()
        {
            TimeGo = DateTime.Now.AddDays(7);
            Tittle = "Αεροπορικό";
            Allerretour = true;
            To = From = "";
            WaitingTime = new DateTime(1, 1, 1, 0, 0, 0);
        }

        #endregion Constructors

        #region Fields

        private Airline _Airline;

        private string _PNR;

        private bool _Stop = false;

        private string _StopPlace;

        private DateTime? _WaitingTime;

        #endregion Fields

        #region Properties

        public  Airline Airline
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

        #endregion Properties
    }
}