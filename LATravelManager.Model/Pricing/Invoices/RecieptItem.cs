using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Pricing.Invoices
{
    public class RecieptItem : BaseModel
    {
        private string _Description;

        [StringLength(100)]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description == value)
                {
                    return;
                }

                _Description = value;
                RaisePropertyChanged();
            }
        }

        private string _Dates;

        [StringLength(60)]
        public string Dates
        {
            get
            {
                return _Dates;
            }

            set
            {
                if (_Dates == value)
                {
                    return;
                }

                _Dates = value;
                RaisePropertyChanged();
            }
        }




        private decimal _Amount;


        public decimal Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (_Amount == value)
                {
                    return;
                }

                _Amount = value;
                RaisePropertyChanged();
            }
        }

        private int _ReservationId;

        public int ReservationId
        {
            get
            {
                return _ReservationId;
            }

            set
            {
                if (_ReservationId == value)
                {
                    return;
                }

                _ReservationId = value;
                RaisePropertyChanged();
            }
        }

        private int _Pax;

        public int Pax
        {
            get
            {
                return _Pax;
            }

            set
            {
                if (_Pax == value)
                {
                    return;
                }

                _Pax = value;
                RaisePropertyChanged();
            }
        }

        private string _Extra;

        [StringLength(100)]
        public string Extra
        {
            get
            {
                return _Extra;
            }

            set
            {
                if (_Extra == value)
                {
                    return;
                }

                _Extra = value;
                RaisePropertyChanged();
            }
        }

        private string _Names;

        [StringLength(100)]
        public string Names
        {
            get
            {
                return _Names;
            }

            set
            {
                if (_Names == value)
                {
                    return;
                }

                _Names = value;
                RaisePropertyChanged();
            }
        }
    }
}