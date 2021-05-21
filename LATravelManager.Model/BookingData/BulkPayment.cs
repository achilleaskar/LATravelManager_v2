using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using LATravelManager.Model.People;

namespace LATravelManager.Model.BookingData
{
    public class BulkPayment : BaseModel
    {
        public BulkPayment()
        {
            Payments = new ObservableCollection<Payment>();
        }
        #region Fields

        private decimal _Amount;

        private string _Comment;

        private DateTime _Date;

        private Partner _Partner;

        private PaymentMethod _PaymentMethod;

        private ObservableCollection<Payment> _Payments;

        private bool _Selected;

        #endregion Fields

        #region Properties

        public int? PartnerId { get; set; }

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

        public decimal Remaining => GetRemainign();

        private decimal GetRemainign()
        {
            decimal sum = 0;
            foreach (var p in Payments)
            {
                sum += p.Amount;
            }
            return Amount - sum;
        }

        #endregion Properties
    }
}