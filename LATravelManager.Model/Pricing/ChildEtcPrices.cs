namespace LATravelManager.Model.Pricing
{
    public class ChildEtcPrices : BaseModel
    {

        public ChildEtcPrices()
        {
            Transfer = new HotelPricing() ;
        }
        private int _Under12;

        public int Under12
        {
            get
            {
                return _Under12;
            }

            set
            {
                if (_Under12 == value)
                {
                    return;
                }

                _Under12 = value;
                RaisePropertyChanged();
            }
        }

        private int _Under18;

        public int Under18
        {
            get
            {
                return _Under18;
            }

            set
            {
                if (_Under18 == value)
                {
                    return;
                }

                _Under18 = value;
                RaisePropertyChanged();
            }
        }

        private int _OnlyStayDiscount;

        public int OnlyStayDiscount
        {
            get
            {
                return _OnlyStayDiscount;
            }

            set
            {
                if (_OnlyStayDiscount == value)
                {
                    return;
                }

                _OnlyStayDiscount = value;
                RaisePropertyChanged();
            }
        }




        private int _Single;


        public int Single
        {
            get
            {
                return _Single;
            }

            set
            {
                if (_Single == value)
                {
                    return;
                }

                _Single = value;
                RaisePropertyChanged();
            }
        }

        private HotelPricing _Transfer;

        public HotelPricing Transfer
        {
            get
            {
                return _Transfer;
            }

            set
            {
                if (_Transfer == value)
                {
                    return;
                }

                _Transfer = value;
                RaisePropertyChanged();
            }
        }
    }
}