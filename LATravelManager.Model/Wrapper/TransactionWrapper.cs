using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using System;
using static LATravelManager.Model.Enums;

namespace LATravelManager.Model.Wrapper
{
    public class TransactionWrapper : ModelWrapper<Transaction>
    {
        #region Constructors

        public TransactionWrapper(Transaction model) : base(model)
        {
        }

        #endregion Constructors

        private ExpenseBaseCategories _ExpenseBaseCategory;

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
            }
        }

        private GroupExpenseCategories _GroupExpenseCategory;

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
            }
        }

        private StandardExpenseCategories _StandardExpenseCategory;

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
            }
        }

        private TaxExpenseCategories _TaxExpenseCategory;

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
            }
        }

        private TransactionType _TransactionType;

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
            }
        }

        #region Properties

        public decimal AgeAmount
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }

        public Booking Booking
        {
            get { return GetValue<Booking>(); }
            set { SetValue(value); }
        }

        public DateTime CheckIn
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public DateTime CheckOut
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public int Code
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public DateTime Date
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public User PaymentTo
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        public User User
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        #endregion Properties
    }
}