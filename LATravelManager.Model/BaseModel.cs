using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GalaSoft.MvvmLight;

namespace LATravelManager.Model
{
    public abstract class BaseModel : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty
        /// string ("").
        ///// </returns>
        //public string Error
        //{
        //    get
        //    {
        //        if (ValidationProperties == null)
        //        {
        //            return "";
        //        }
        //        var error = new StringBuilder();

        //        // iterate over all of the properties of this object - aggregating any validation errors
        //        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
        //        foreach (var propName in ValidationProperties)
        //        {
        //            var propertyError = this[propName];
        //            if (!string.IsNullOrEmpty(propertyError))
        //            {
        //                error.Append((error.Length != 0 ? "," + Environment.NewLine : "") + propertyError);
        //            }
        //        }

        //        return error.ToString();
        //    }
        //}

        ///// <summary>
        ///// Indicates whether this instance has any errors.
        ///// </summary>
        //public bool HasErrors => Errors.Any();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[NotMapped]
        //public int Number
        //{
        //    get
        //    {
        //        return _Number;
        //    }

        //    set
        //    {
        //        if (_Number == value)
        //        {
        //            return;
        //        }

        //        _Number = value;
        //        RaisePropertyChanged();
        //    }
        ////}
        //[NotMapped]
        //public string Tittle { get; set; } = string.Empty;

        //public string[] ValidationProperties { get; set; }

        ///// <summary>
        ///// Retrieves a list of all properties with attributes required for <see
        ///// cref="IDataErrorInfo"/> automation.
        ///// </summary>
        //protected List<PropertyInfo> PropertyInfos => _propertyInfos
        //               ?? (_propertyInfos =
        //                   GetType()
        //                       .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //                       .Where(prop => prop.IsDefined(typeof(RequiredAttribute), true) || prop.IsDefined(typeof(MaxLengthAttribute), true))
        //                       .ToList());

        ///// <summary>
        ///// A dictionary of current errors with the name of the error-field as the key and the error
        ///// text as the value.
        ///// </summary>
        //private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        #endregion Properties

        #region Indexers

        ///// <summary>
        ///// Gets the error message for the property with the given name.
        ///// </summary>
        ///// <returns>The error message for the property. The default is an empty string ("").</returns>
        ///// <param name="columnName">The name of the property whose error message to get.</param>
        //public string this[string columnName]
        //{
        //    get
        //    {
        //        return OnValidate(columnName);
        //    }
        //}

        #endregion Indexers

        //#region Methods

        //public bool HasValues()
        //{
        //    foreach (PropertyInfo pi in GetType().GetProperties())
        //    {
        //        if (pi.PropertyType == typeof(string))
        //        {
        //            string value = (string)pi.GetValue(this);
        //            if (!string.IsNullOrEmpty(value))
        //            {
        //                return true;
        //            }
        //        }
        //        else if (pi.PropertyType == typeof(int))
        //        {
        //            int value = (int)pi.GetValue(this);
        //            if (value != 0)
        //            {
        //                return true;
        //            }
        //        }
        //        else if (pi.PropertyType == typeof(double))
        //        {
        //            int value = (int)pi.GetValue(this);
        //            if (value != 0)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //protected object GetValue(string propName)
        //{
        //    return GetType().GetProperty(propName).GetValue(this);
        //}

        //protected string OnValidate(string propertyName)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(propertyName))
        //        {
        //            throw new ArgumentException("Invalid property name", propertyName);
        //        }
        //        if (propertyName == "Item")
        //        {
        //            return null;
        //        }

        //        string error = string.Empty;
        //        var value = GetValue(propertyName);
        //        var results = new List<ValidationResult>(1);
        //        var result = Validator.TryValidateProperty(
        //            value,
        //            new ValidationContext(this, null, null)
        //            {
        //                MemberName = propertyName
        //            },
        //            results);

        //        if (!result)
        //        {
        //            var validationResult = results.First();
        //            error = validationResult.ErrorMessage;
        //        }
        //        Errors.Remove(propertyName);

        //        if (!string.IsNullOrEmpty(error))
        //        {
        //            Errors.Add(propertyName, error);
        //            RaisePropertyChanged(nameof(HasErrors));
        //            RaisePropertyChanged(nameof(IsOk));
        //        }

        //        return error;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    return null;
        //}

        //#endregion Methods
    }
}