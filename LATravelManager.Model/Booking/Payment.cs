using LATravelManager.BaseTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace LATravelManager.Models
{
    public class Payment : BaseModel
    {
        #region Fields + Constructors

        private float _Amount;
        private string _AmountString;

        private string _Comment = string.Empty;

        private DateTime _Date;

        private int _PaymentMethod;

        private User _User;

        public Payment()
        {

        }
        public Payment(Payment p)
        {
            Amount = p.Amount;
            Comment = p.Comment;
            Date = p.Date;
            PaymentMethod = p.PaymentMethod;
            User = p.User;
        }

        public const string AmountPropertyName = nameof(Amount);
        public const string AmountStringPropertyName = nameof(AmountString);

        public const string CommentPropertyName = nameof(Comment);
        public const string DatePropertyName = nameof(Date);
        public const string PaymentMethodPropertyName = nameof(PaymentMethod);
        public const string UserPropertyName = nameof(User);

        #endregion Fields + Constructors

        #region Properties

        [NotMapped]
        public double EPSILON { get; private set; } = 0.00001;

        /// <summary>
        /// Sets and gets the Amount property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        /// <summary>
        /// Sets and gets the Price property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public float Amount
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

                if (Math.Abs(value - _Amount) > EPSILON)
                {
                    _Amount = (float)Math.Round(value, 2);
                    AmountString = value.ToString();
                }
                RaisePropertyChanged(AmountPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the PriceString property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public string AmountString
        {
            get
            {
                return _AmountString;
            }

            set
            {
                if (_AmountString == value)
                {
                    return;
                }

                _AmountString = value;

                if (!string.IsNullOrEmpty(_AmountString))
                {
                    if (float.TryParse(value.Replace(',', '.'), NumberStyles.Any, new CultureInfo("en-US"), out var tmpDouble))
                    {
                        tmpDouble = (float)Math.Round(tmpDouble, 2);
                        Amount = tmpDouble;
                        RaisePropertyChanged(AmountStringPropertyName);
                        if (_AmountString[_AmountString.Length - 1] != '.' && _AmountString[_AmountString.Length - 1] != ',')
                        {
                            _AmountString = tmpDouble.ToString();
                        }
                    }
                    else
                    {
                        _AmountString = Amount.ToString();
                    }
                }
                else
                {
                    _AmountString = "0";
                    Amount = 0;
                }
                _AmountString = value;
                RaisePropertyChanged(AmountStringPropertyName);
            }
        }


        /// <summary>
        /// Sets and gets the Comment property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(20)]
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
                RaisePropertyChanged(CommentPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Date property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
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
                RaisePropertyChanged(DatePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the PaymentMethod property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public int PaymentMethod
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
                RaisePropertyChanged(PaymentMethodPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the User property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public virtual User User
        {
            get
            {
                return _User;
            }

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
                RaisePropertyChanged(UserPropertyName);
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            switch (PaymentMethod)
            {
                case 0:
                    return "Μετρητά";

                case 1:
                    return "Τράπεζα";

                case 2:
                    return "VISA";
            }
            return base.ToString();
        }

        #endregion Methods

    }
}