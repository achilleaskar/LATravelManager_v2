using LATravelManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.UI.Helpers
{
    public static class StaticResources
    {
        #region Properties

        public static string[] AgesList { get; set; } = { "<1", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18+" };

        public static string[] Airports { get; set; } = { "Thessaloniki(SKG)", "Athens(ATH)" };
        public static ObservableCollection<StartingPlace> StartingPlaces { get; set; }
        public static Booking TmpBooking { get; set; }
        public static User User { get; set; }

        #endregion Properties
    }

    public class RequiredIfAttribute : ValidationAttribute
    {
        #region Constructors

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            this._dependentProperty = dependentProperty;
            this._targetValue = targetValue;
        }

        #endregion Constructors

        #region Fields

        private readonly RequiredAttribute _innerAttribute = new RequiredAttribute();

        #endregion Fields

        #region Properties

        public string _dependentProperty { get; set; }
        public object _targetValue { get; set; }

        #endregion Properties

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = validationContext.ObjectType.GetProperty(_dependentProperty);
            if (field != null)
            {
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                if ((dependentValue == null && _targetValue == null) || (dependentValue.Equals(_targetValue)))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string name = validationContext.DisplayName;
                        return new ValidationResult(ErrorMessage = name + " Is required.");
                    }
                }
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(_dependentProperty));
            }
        }

        #endregion Methods
    }
}