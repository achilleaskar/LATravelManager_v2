using LATravelManager.Model;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class City : BaseModel, INamed
    {

        #region Constructors

        public City()
        {
        }

        #endregion Constructors

        #region Fields

        public const string CountryPropertyName = nameof(Country);

        public const string NamePropertyName = nameof(Name);
        private Country _Country = null;

        private string _Name = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Country property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Πρέπει να επιλέξετε χώρα!")]
        public virtual Country Country
        {
            get
            {
                return _Country;
            }

            set
            {
                if (_Country == value)
                {
                    return;
                }

                _Country = value;
                RaisePropertyChanged(CountryPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Name property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Το Όνομα Πόλης απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Όνομα Πόλης μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
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

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods

    }
}