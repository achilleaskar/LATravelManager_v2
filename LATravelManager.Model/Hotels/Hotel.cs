using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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




        private bool _NoName;


        public bool NoName
        {
            get
            {
                return _NoName;
            }

            set
            {
                if (_NoName == value)
                {
                    return;
                }

                _NoName = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(50, MinimumLength = 0)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        [EmailAddress(ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Η πόλη στην οποία βρίσκεται το ξενοδοχείο απαιτείται!")]
        public City City { get; set; }

        [Required(ErrorMessage = "Η κατηγορία του ξενοδοχείου απαιτείται!")]
        public HotelCategory HotelCategory { get; set; }

        public HotelCategories HotelCategoryEnum { get; set; }

        [Required(ErrorMessage = "Το Όνομα ξενοδοχείου απαιτείται!")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Το Όνομα ξενοδοχείου μπορεί να είναι από 3 έως 25 χαρακτήρες.")]
        public string Name { get; set; }

        public List<Room> Rooms { get; set; }

        [Phone]
        [Required(ErrorMessage = "Το Τηλέφωνο απαιτείται!")]
        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες.")]
        public string Tel { get; set; }

        #endregion Properties




        private bool _IsExpanded;

        [NotMapped]
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }

            set
            {
                if (_IsExpanded == value)
                {
                    return;
                }

                _IsExpanded = value;
                RaisePropertyChanged();
            }
        }

        #region Methods

        public override string ToString() => Name;

        #endregion Methods
    }
}