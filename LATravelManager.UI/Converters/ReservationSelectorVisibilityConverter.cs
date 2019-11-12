using LATravelManager.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    public class ReservationSelectorVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Transaction tr)
                if (tr != null)
                    if (parameter is string bookingselector && bookingselector.Equals("0"))
                    {
                        if (tr.IncomeBaseCategory == IncomeBaseCategories.OptionalActivities ||
                            (tr.TransactionType == TransactionType.Income && tr.IncomeBaseCategory == IncomeBaseCategories.None) ||
                            (tr.ExpenseBaseCategory == ExpenseBaseCategories.GroupExpense && tr.ExcursionExpenseCategory != ExcursionExpenseCategories.Total && tr.ExpenseBaseCategory != ExpenseBaseCategories.GroupExpense) ||
                            tr.ExpenseBaseCategory == ExpenseBaseCategories.PersonelExpense ||
                            tr.ExcursionExpenseCategory == ExcursionExpenseCategories.Booking)
                        {
                            return Visibility.Visible;
                        }
                    }
                    else if (parameter is string bookingselector1 && bookingselector1.Equals("1"))
                    {
                        if (tr.ExcursionExpenseCategory == ExcursionExpenseCategories.Dates ||
                            tr.GeneralOrDatesExpenseCategory == GeneralOrDatesExpenseCategories.Dates)
                        {
                            return Visibility.Visible;
                        }
                    }
                    else if (parameter is string excursionSelect && excursionSelect.Equals("2"))
                    {
                        if (!tr.FiltersEnabled || tr.ExpenseBaseCategory == ExpenseBaseCategories.GroupExpense ||
                            (tr.TransactionType == TransactionType.Income && tr.IncomeBaseCategory == IncomeBaseCategories.OptionalActivities))
                        {
                            return Visibility.Visible;
                        }
                    }
                    else if (parameter is string bookingselector3 && bookingselector3.Equals("3"))
                    {
                        if (tr.ExpenseBaseCategory == ExpenseBaseCategories.StandardExpense ||
                            tr.ExpenseBaseCategory == ExpenseBaseCategories.TaxExpense ||
                            (tr.TransactionType == TransactionType.Expense && tr.ExpenseBaseCategory == ExpenseBaseCategories.None))
                        {
                            return Visibility.Visible;
                        }
                    }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}