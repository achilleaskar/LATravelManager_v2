using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LATravelManager.Model.Locations;

namespace LATravelManager.Model.Hotels
{
    public class Hotel : EditTracker, INamed
    {
        #region Constructors

        public Hotel()
        {
            Rooms = new List<Room>();
        }

        #endregion Constructors

        #region Properties

        public string Address
        {
            get;
            set;
        }

        [StringLength(50, MinimumLength = 0)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        [EmailAddress(ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Η πόλη στην οποία βρίσκεται το ξενοδοχείο απαιτείται!")]
        public City City { get; set; }

        [Required(ErrorMessage = "Η κατηγορία του ξενοδοχείου απαιτείται!")]
        public HotelCategory HotelCategory { get; set; }

        [Required(ErrorMessage = "Το Όνομα ξενοδοχείου απαιτείται!")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Το Όνομα ξενοδοχείου μπορεί να είναι από 3 έως 25 χαρακτήρες.")]
        public string Name { get; set; }

        public List<Room> Rooms { get; set; }

        [Phone]
        [Required(ErrorMessage = "Το Τηλέφωνο απαιτείται!")]
        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες.")]
        public string Tel { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString() => Name;

        #endregion Methods
    }
}