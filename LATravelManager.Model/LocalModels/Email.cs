using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LATravelManager.Model.LocalModels
{
    public class Email : BaseModel, IDataErrorInfo
    {
        public Email(string email)
        {
            EValue = email;
        }

        private string _EValue;

        [EmailAddress]
        public string EValue
        {
            get
            {
                return _EValue;
            }

            set
            {
                if (_EValue == value)
                {
                    return;
                }
                _EValue = value;
                ValidateThis();
                RaisePropertyChanged();
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string error;

        private void ValidateThis([CallerMemberName] string columnName = "")
        {
            List<ValidationResult> results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(this) { MemberName = columnName };
            Validator.TryValidateProperty(EValue, context, results);
            if (results.Count > 0)
            {
                error = results[0].ErrorMessage;
            }
            else
            {
                error = null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                return error;
            }
        }
    }
}