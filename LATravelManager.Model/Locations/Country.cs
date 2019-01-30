using LATravelManager.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Models
{
    public class Country : BaseModel, INamed
    {
        #region Constructors

        public Country()
        {
            Tittle = "Η χώρα";
        }

        #endregion Constructors

        #region Fields

        public const string ContinentindexPropertyName = nameof(Continentindex);

        public const string ContinentPropertyName = nameof(Continent);

        private string _Continent;

        private int _Continentindex = -1;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Continent property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public string Continent
        {
            get
            {
                return _Continent;
            }

            set
            {
                if (_Continent == value)
                {
                    return;
                }

                _Continent = value;
                RaisePropertyChanged(ContinentPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Continentindex property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        [Range(0, 10, ErrorMessage = "Παρακαλώ επιλέξτε Ήπειρο")]
        public int Continentindex
        {
            get
            {
                return _Continentindex;
            }

            set
            {
                if (_Continentindex == value)
                {
                    return;
                }
                if (value >= 0 && value < Definitions.Continents.Count)
                    Continent = Definitions.Continents[value];
                _Continentindex = value;
                RaisePropertyChanged(ContinentindexPropertyName);
            }
        }

        [Required(ErrorMessage = "Το Όνομα Χώρας απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Όνομα Χώρας μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
        public string Name { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}