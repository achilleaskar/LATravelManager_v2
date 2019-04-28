using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Locations
{
    public class Country : BaseModel, INamed
    {
        #region Constructors

        public Country()
        {
            Continentindex = -1;
        }

        #endregion Constructors

        #region Properties

        [Required]
        [Range(0, 10, ErrorMessage = "Παρακαλώ επιλέξτε Ήπειρο")]
        public int Continentindex { get; set; }

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