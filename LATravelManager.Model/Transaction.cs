using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using System;

namespace LATravelManager.Model
{
    public class Transaction : EditTracker
    {
        #region Constructors

        public Transaction()
        {
            Date = DateTime.Today;
        }

        #endregion Constructors

        #region Fields

        private decimal _Amount;
        private Booking _Booking;
        private DateTime _CheckIn;
        private DateTime _CheckOut;

        private DateTime _Date;

        private string _Description;

        private ExpenseBaseCategories _ExpenseBaseCategory;
        private GroupExpenseCategories _GroupExpenseCategory;
        private IncomeBaseCategories _IncomeBaseCategory;
        private User _PaymentTo;

        private StandardExpenseCategories _StandardExpenseCategory;
        private TaxExpenseCategories _TaxExpenseCategory;
        private TransactionType _TransactionType;
        private User _User;

        #endregion Fields

        #region Properties

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

        public Booking Booking
        {
            get
            {
                return _Booking;
            }

            set
            {
                if (_Booking == value)
                {
                    return;
                }

                _Booking = value;
                RaisePropertyChanged();
            }
        }

        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }

                _CheckIn = value;
                RaisePropertyChanged();
            }
        }

        public DateTime CheckOut
        {
            get
            {
                return _CheckOut;
            }

            set
            {
                if (_CheckOut == value)
                {
                    return;
                }

                _CheckOut = value;
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

        public ExpenseBaseCategories ExpenseBaseCategory
        {
            get
            {
                return _ExpenseBaseCategory;
            }

            set
            {
                if (_ExpenseBaseCategory == value)
                {
                    return;
                }

                _ExpenseBaseCategory = value;
                RaisePropertyChanged();
                GroupExpenseCategory = GroupExpenseCategories.None;
                IncomeBaseCategory = IncomeBaseCategories.None;
                StandardExpenseCategory = StandardExpenseCategories.None;
                TaxExpenseCategory = TaxExpenseCategories.None;
            }
        }

        public GroupExpenseCategories GroupExpenseCategory
        {
            get
            {
                return _GroupExpenseCategory;
            }

            set
            {
                if (_GroupExpenseCategory == value)
                {
                    return;
                }

                _GroupExpenseCategory = value;
                RaisePropertyChanged();
                IncomeBaseCategory = IncomeBaseCategories.None;
                StandardExpenseCategory = StandardExpenseCategories.None;
                TaxExpenseCategory = TaxExpenseCategories.None;
            }
        }

        public IncomeBaseCategories IncomeBaseCategory
        {
            get
            {
                return _IncomeBaseCategory;
            }

            set
            {
                if (_IncomeBaseCategory == value)
                {
                    return;
                }

                _IncomeBaseCategory = value;
                RaisePropertyChanged();
                GroupExpenseCategory = GroupExpenseCategories.None;
                ExpenseBaseCategory = ExpenseBaseCategories.None;
                StandardExpenseCategory = StandardExpenseCategories.None;
                TaxExpenseCategory = TaxExpenseCategories.None;
            }
        }

        public User PaymentTo
        {
            get
            {
                return _PaymentTo;
            }

            set
            {
                if (_PaymentTo == value)
                {
                    return;
                }

                _PaymentTo = value;
                RaisePropertyChanged();
            }
        }

        public StandardExpenseCategories StandardExpenseCategory
        {
            get
            {
                return _StandardExpenseCategory;
            }

            set
            {
                if (_StandardExpenseCategory == value)
                {
                    return;
                }

                _StandardExpenseCategory = value;
                RaisePropertyChanged();
                GroupExpenseCategory = GroupExpenseCategories.None;
                IncomeBaseCategory = IncomeBaseCategories.None;
                TaxExpenseCategory = TaxExpenseCategories.None;
            }
        }

        public TaxExpenseCategories TaxExpenseCategory
        {
            get
            {
                return _TaxExpenseCategory;
            }

            set
            {
                if (_TaxExpenseCategory == value)
                {
                    return;
                }

                _TaxExpenseCategory = value;
                RaisePropertyChanged();
                GroupExpenseCategory = GroupExpenseCategories.None;
                IncomeBaseCategory = IncomeBaseCategories.None;
                StandardExpenseCategory = StandardExpenseCategories.None;
            }
        }

        public TransactionType TransactionType
        {
            get
            {
                return _TransactionType;
            }

            set
            {
                if (_TransactionType == value)
                {
                    return;
                }

                _TransactionType = value;
                RaisePropertyChanged();
                GroupExpenseCategory = GroupExpenseCategories.None;
                IncomeBaseCategory = IncomeBaseCategories.None;
                ExpenseBaseCategory = ExpenseBaseCategories.None;
                StandardExpenseCategory = StandardExpenseCategories.None;
                TaxExpenseCategory = TaxExpenseCategories.None;
            }
        }

        public User User
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
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}