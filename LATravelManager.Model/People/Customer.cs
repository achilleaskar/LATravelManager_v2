﻿using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.People
{
    public class Customer : EditTracker, INamed
    {
        #region Constructors

        public Customer()
        {
            Services = new ObservableCollection<Service>();

            Age = 18;
            //DOB = DateTime.Now.AddYears(-20);
        }

        #endregion Constructors
        public Bus Bus{ get; set; }
        #region Fields

        private DateTime _PassportExpiration;

        private DateTime _PassportPublish;

        #endregion Fields

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
        [RegularExpression(@"^[a-z- A-Z]+$", ErrorMessage = "Παρακαλώ χρησιμοποιήστε μόνο λατινικούς χαρακτήρες")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Όνομα μπορεί να είναι από 3 έως 20 χαρακτήρες")]
        public string Name { get; set; }

        public OptionalExcursion OptionalExcursion { get; set; }

        public DateTime PassportExpiration
        {
            get
            {
                return _PassportExpiration;
            }

            set
            {
                if (_PassportExpiration == value)
                {
                    return;
                }

                _PassportExpiration = value;
                RaisePropertyChanged();
            }
        }
        [StringLength(20, ErrorMessage = "Πολύ μεγάλο")]
        public string PassportNum { get; set; }

        public DateTime PassportPublish
        {
            get
            {
                return _PassportPublish;
            }

            set
            {
                if (_PassportPublish == value)
                {
                    return;
                }

                _PassportPublish = value;
                RaisePropertyChanged();
            }
        }

        public decimal Price { get; set; }
        public Reservation Reservation { get; set; }

        [StringLength(30, MinimumLength = 0)]
        public string ReturningPlace { get; set; }
        public ICollection<Service> Services { get; }
        [Required(ErrorMessage = "Επιλέξτε σημέιο αναχώρησης")]
        [StringLength(20)]
        public string StartingPlace { get; set; }

        [Required(ErrorMessage = "Το επίθετο είναι υποχρεωτικό.")]
        [RegularExpression(@"^[a-z- A-Z]+$", ErrorMessage = "Παρακαλώ χρησιμοπποιήστε μόνο λατινικούς χαρακτήρες")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Επίθετο μπορεί να είναι από 3 έως 20 χαρακτήρες")]
        public string Surename { get; set; }

        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες")]
        [Phone(ErrorMessage = "Το τηλέφωνο δν έχει τη σωστή μορφή")]
        public string Tel { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Surename + " " + Name;
        }

        #endregion Methods
    }
}