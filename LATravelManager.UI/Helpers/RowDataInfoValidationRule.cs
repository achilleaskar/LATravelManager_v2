using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace LATravelManager.UI.Helpers
{
    public class RowDataInfoValidationRule : ValidationRule
    {
        #region Methods

        public override ValidationResult Validate(object value,
                        CultureInfo cultureInfo)
        {
            var group = (BindingGroup)value;

            StringBuilder error = new StringBuilder();
            foreach (var item in group.Items)
            {
                //if (item is Customer cust)
                //{
                //    if (!cust.IsOk)
                //    {
                //        error.Append("Exume provlima");
                //    }
                //}

                //// aggregate errors
                //if (item is IDataErrorInfo info)
                //{
                //    if (!string.IsNullOrEmpty(info.Error))
                //    {
                //        if (error == null)
                //            error = new StringBuilder();
                //        error.Append((error.Length != 0 ? ", " : "") + info.Error);
                //    }
                //}
            }

            foreach (var item in group.Items)
            {
                // aggregate errors
                if (item is IDataErrorInfo info)
                {
                    if (!string.IsNullOrEmpty(info.Error))
                    {
                        if (error == null)
                            error = new StringBuilder();
                        error.Append((error.Length != 0 ? ", " : "") + info.Error);
                    }
                }
            }

            if (error.Length != 0)
                return new ValidationResult(false, error.ToString());

            return ValidationResult.ValidResult;
        }

        #endregion Methods
    }
}