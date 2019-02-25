using LATravelManager.Model;
using System.Collections.Generic;
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



        #region Properties

        public List<Hotel> Hotels { get; set; }

        [Required(ErrorMessage = "Πρέπει να επιλέξετε χώρα!")]
        public virtual Country Country { get; set; }

        [Required(ErrorMessage = "Το Όνομα Πόλης απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Όνομα Πόλης μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
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