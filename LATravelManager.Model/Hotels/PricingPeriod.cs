using LATravelManager.Model.Pricing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Hotels
{
    public class PricingPeriod : BaseModel
    {
        public PricingPeriod()
        {
            From = To = FromB = ToB = DateTime.Today;
            HotelPricings = new List<HotelPricing>();
            ChilndEtcPrices = new ChildEtcPrices();

        }
        private DateTime _From;

        public DateTime From
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




        private bool _Parted;


        public bool Parted
        {
            get
            {
                return _Parted;
            }

            set
            {
                if (_Parted == value)
                {
                    return;
                }

                _Parted = value;
                RaisePropertyChanged();
            }
        }




        private DateTime _FromB;


        public DateTime FromB
        {
            get
            {
                return _FromB;
            }

            set
            {
                if (_FromB == value)
                {
                    return;
                }
                if (ToB<value)
                {
                    ToB = value;
                }
                _FromB = value;
                RaisePropertyChanged();
            }
        }



        private DateTime _ToB;


        public DateTime ToB
        {
            get
            {
                return _ToB;
            }

            set
            {
                if (_ToB == value)
                {
                    return;
                }

                _ToB = value;
                RaisePropertyChanged();
            }
        }




        private string _Name;


        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged();
            }
        }

        private ChildEtcPrices _ChilndEtcPrices;


        public ChildEtcPrices ChilndEtcPrices
        {
            get
            {
                return _ChilndEtcPrices;
            }

            set
            {
                if (_ChilndEtcPrices == value)
                {
                    return;
                }

                _ChilndEtcPrices = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _To;

        public DateTime To
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

                if (FromB<value)
                {

                FromB = value;
                }
                RaisePropertyChanged();
            }
        }

        private List<HotelPricing> _HotelPricings;

        public List<HotelPricing> HotelPricings
        {
            get
            {
                return _HotelPricings;
            }

            set
            {
                if (_HotelPricings == value)
                {
                    return;
                }

                _HotelPricings = value;
                RaisePropertyChanged();
            }
        }
    }
}