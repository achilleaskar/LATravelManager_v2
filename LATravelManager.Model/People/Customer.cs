using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.People
{
    public class Customer : EditTracker, INamed
    {
        public Customer()
        {
            Services = new ObservableCollection<Service>();

            Age = 18;
            //DOB = DateTime.Now.AddYears(-20);
        }

        public ICollection<Service> Services { get; }

        #region Properties

        [Range(0, 18)]
        public int Age { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        [StringLength(200, ErrorMessage = "Πολύ μεγάλο")]
        public string Comment { get; set; }

        public int CustomerHasBusIndex { get; set; }

        public int CustomerHasPlaneIndex { get; set; }

        public int CustomerHasShipIndex { get; set; }

        public int DeserveDiscount { get; set; }

        public DateTime? DOB { get; set; }

        [StringLength(30, MinimumLength = 0)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        [EmailAddress(ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Το όνομα είναι υποχρεωτικό")]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Παρακαλώ χρησιμοποιήστε μόνο λατινικούς χαρακτήρες")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Όνομα μπορεί να είναι από 3 έως 20 χαρακτήρες")]
        public string Name { get; set; }

        public OptionalExcursion OptionalExcursion { get; set; }

        [StringLength(20, ErrorMessage = "Πολύ μεγάλο")]
        public string PassportNum { get; set; }

        public decimal Price { get; set; }

        public Reservation Reservation { get; set; }

        public string ReturningPlace { get; set; }

        [Required(ErrorMessage = "Επιλέξτε σημέιο αναχώρησης")]
        [StringLength(20)]
        public string StartingPlace { get; set; }

        [Required(ErrorMessage = "Το επίθετο είναι υποχρεωτικό.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Παρακαλώ χρησιμοπποιήστε μόνο λατινικούς χαρακτήρες")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Επίθετο μπορεί να είναι από 3 έως 20 χαρακτήρες")]
        public string Surename { get; set; }

        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες")]
        [Phone(ErrorMessage = "Το τηλέφωνο δν έχει τη σωστή μορφή")]
        public string Tel { get; set; }

        #endregion Properties

        public override string ToString()
        {
            return Surename + " " + Name;
        }
    }
}