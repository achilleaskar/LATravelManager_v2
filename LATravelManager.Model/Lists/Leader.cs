using LATravelManager.Model;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class Leader : BaseModel,INamed
    {
        #region Fields + Constructors

        private string _Name = string.Empty;
        private string _Tel;
        public const string NamePropertyName = nameof(Name);
        public const string TelPropertyName = nameof(Tel);

        #endregion Fields + Constructors

        #region Properties

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        [StringLength(20)]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Tel property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        [StringLength(20)]
        public string Tel
        {
            get
            {
                return _Tel;
            }

            set
            {
                if (_Tel == value)
                {
                    return;
                }

                _Tel = value;
                RaisePropertyChanged(TelPropertyName);
            }
        }

        #endregion Properties
    }
}