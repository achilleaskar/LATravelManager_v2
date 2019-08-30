using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Locations
{
    public class City : BaseModel, INamed
    {
        #region Constructors

        public City()
        {
            Excursions = new List<Excursion>();
        }

        #endregion Constructors

        public List<Excursion> Excursions { get; set; }

        #region Properties

        public List<Hotel> Hotels { get; set; }

        [Required(ErrorMessage = "Πρέπει να επιλέξετε χώρα!")]
        public Country Country { get; set; }

        [Required(ErrorMessage = "Το Όνομα Πόλης απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Όνομα Πόλης μπορεί να είναι από 3 έως 20 χαρακτήρες.")]
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