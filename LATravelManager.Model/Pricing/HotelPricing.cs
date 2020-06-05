using LATravelManager.Model.Hotels;

namespace LATravelManager.Model.Pricing
{
    public class HotelPricing : BaseModel
    {
        private Hotel _Hotel;

        public Hotel Hotel
        {
            get
            {
                return _Hotel;
            }

            set
            {
                if (_Hotel == value)
                {
                    return;
                }

                _Hotel = value;
                RaisePropertyChanged();
            }
        }

        public string Name => Hotel != null ? Hotel.Name : "_Transfer";

        private int _N2;

        public int N2
        {
            get
            {
                return _N2;
            }

            set
            {
                if (_N2 == value)
                {
                    return;
                }

                _N2 = value;
                RaisePropertyChanged();
            }
        }

        private int _N3;

        public int N3
        {
            get
            {
                return _N3;
            }

            set
            {
                if (_N3 == value)
                {
                    return;
                }

                _N3 = value;
                RaisePropertyChanged();
            }
        }

        private int _N4;

        public int N4
        {
            get
            {
                return _N4;
            }

            set
            {
                if (_N4 == value)
                {
                    return;
                }

                _N4 = value;
                RaisePropertyChanged();
            }
        }

        private int _N5;

        public int N5
        {
            get
            {
                return _N5;
            }

            set
            {
                if (_N5 == value)
                {
                    return;
                }

                _N5 = value;
                RaisePropertyChanged();
            }
        }

        private int _N6;

        public int N6
        {
            get
            {
                return _N6;
            }

            set
            {
                if (_N6 == value)
                {
                    return;
                }

                _N6 = value;
                RaisePropertyChanged();
            }
        }

        private int _N7;

        public int N7
        {
            get
            {
                return _N7;
            }

            set
            {
                if (_N7 == value)
                {
                    return;
                }

                _N7 = value;
                RaisePropertyChanged();
            }
        }

        private int _N8;

        public int N8
        {
            get
            {
                return _N8;
            }

            set
            {
                if (_N8 == value)
                {
                    return;
                }

                _N8 = value;
                RaisePropertyChanged();
            }
        }
    }
}