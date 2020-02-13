using System;
using LATravelManager.Model.Locations;

namespace LATravelManager.Model.Excursions
{
    public class ExcursionTime : BaseModel
    {
        public ExcursionTime()
        {
        }

        private TimeSpan _Time;

        public TimeSpan Time
        {
            get
            {
                return _Time;
            }

            set
            {
                if (_Time == value)
                {
                    return;
                }

                _Time = value;
                RaisePropertyChanged();
            }
        }

        private City _From;

        public City From
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

        private StartingPlace _StartingPlace;

        public StartingPlace StartingPlace
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
    }
}