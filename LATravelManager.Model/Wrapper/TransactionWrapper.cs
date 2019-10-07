using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using System;

namespace LATravelManager.Model.Wrapper
{
    public class TransactionWrapper : ModelWrapper<Transaction>
    {
        #region Constructors

        public TransactionWrapper(Transaction model) : base(model)
        {
        }

        #endregion Constructors

        #region Properties

        public decimal Amount
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

        public ExpenseBaseCategories ExpenseBaseCategory
        {
            get { return GetValue<ExpenseBaseCategories>(); }
            set { SetValue(value); }
        }

        public GroupExpenseCategories GroupExpenseCategory
        {
            get { return GetValue<GroupExpenseCategories>(); }
            set { SetValue(value); }
        }

        public IncomeBaseCategories IncomeBaseCategory
        {
            get { return GetValue<IncomeBaseCategories>(); }
            set { SetValue(value); }
        }

        public User PaymentTo
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        public StandardExpenseCategories StandardExpenseCategory
        {
            get { return GetValue<StandardExpenseCategories>(); }
            set { SetValue(value); }
        }

        public TaxExpenseCategories TaxExpenseCategory
        {
            get { return GetValue<TaxExpenseCategories>(); }
            set { SetValue(value); }
        }

        public TransactionType TransactionType
        {
            get { return GetValue<TransactionType>(); }
            set { SetValue(value); }
        }

        public User User
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        //internal void FullToCode()
        //{
        //    int code = 0;

        //    if (TransactionType > 0)
        //    {
        //        code += (int)TransactionType;
        //        if (code == 1)
        //        {
        //            if (IncomeBaseCategory > 0)
        //            {
        //                code += (int)IncomeBaseCategory * 10;
        //            }
        //        }
        //        else if (code ==2)
        //        {
        //            if (ExpenseBaseCategory > 0)
        //            {
        //                code += (int)ExpenseBaseCategory * 10;
        //            }
        //        }
        //    }
        //}
    }
}