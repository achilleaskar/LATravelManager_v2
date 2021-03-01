using LATravelManager.Model.People;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.BookingData
{
    public class BulkPayment : BaseModel
    {
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

                if (Math.Abs(value - _Amount) > 0.001m)
                {
                    _Amount = Math.Round(value, 2);
                }
                RaisePropertyChanged();
            }
        }

        private DateTime _Date;

        public DateTime Date
        {
            get
            {
                return _Date;
            }

            set
            {
                if (_Date == value)
                {
                    return;
                }

                _Date = value;
                RaisePropertyChanged();
            }
        }

        private string _Comment;

        public string Comment
        {
            get
            {
                return _Comment;
            }

            set
            {
                if (_Comment == value)
                {
                    return;
                }

                _Comment = value;
                RaisePropertyChanged();
            }
        }

        public PaymentMethod PaymentMethod
        {
            get
            {
                return _PaymentMethod;
            }

            set
            {
                if (_PaymentMethod == value)
                {
                    return;
                }

                _PaymentMethod = value;
                RaisePropertyChanged();
            }
        }

        private Partner _Partner;

        public Partner Partner
        {
            get
            {
                return _Partner;
            }

            set
            {
                if (_Partner == value)
                {
                    return;
                }

                _Partner = value;
                RaisePropertyChanged();
            }
        }

        private PaymentMethod _PaymentMethod;


        private bool _Selected;

        [NotMapped]
        public bool Selected
        {
            get
            {
                return _Selected;
            }

            set
            {
                if (_Selected == value)
                {
                    return;
                }

                _Selected = value;
                RaisePropertyChanged();
            }
        }




        private ObservableCollection<Payment> _Payments;


        public ObservableCollection<Payment> Payments
        {
            get
            {
                return _Payments;
            }

            set
            {
                if (_Payments == value)
                {
                    return;
                }

                _Payments = value;
                RaisePropertyChanged();
            }
        }


    }
}