using LATravelManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class Booking : EditTracker
    {
        public Booking()
        {
            Payments = new ObservableCollection<Payment>();
            ReservationsInBooking = new List<Reservation>();
            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(3);

        }

        //public Booking()
        //{
        //    ReservationsInBooking = new ObservableCollection<Reservation>();
        //    Payments.CollectionChanged += PaymentCollectionChanged;
        //    Customers.CollectionChanged += Customers_CollectionChanged;
        //    ReservationsInBooking.CollectionChanged += ReservationsCollectionChanged;
        //    CheckIn = DateTime.Today;
        //}

        //public Booking(Booking booking, object uOW)
        //{
        //    //this.booking = booking;
        //    //this.uOW = uOW;
        //}

        //public Booking(Booking tmpBooking, UnitOfWork uOW)
        //{
        //    Id = tmpBooking.Id;
        //    CheckIn = tmpBooking.CheckIn;
        //    CheckOut = tmpBooking.CheckOut;
        //    Comment = tmpBooking.Comment;
        //    IsPartners = tmpBooking.IsPartners;
        //    FullPrice = tmpBooking.FullPrice;
        //    Commision = tmpBooking.Commision;
        //    SecondDepart = tmpBooking.SecondDepart;
        //    DifferentDates = tmpBooking.DifferentDates;
        //    Partner = (tmpBooking.Partner != null) ? uOW.GenericRepository.GetById<Partner>(tmpBooking.Partner.Id) : null;
        //    User = uOW.GenericRepository.GetById<User>(tmpBooking.User.Id);

        //    Customers.CollectionChanged += Customers_CollectionChanged;
        //    foreach (Customer customer in tmpBooking.Customers)
        //    {
        //        Customers.Add(new Customer(customer, uOW));
        //    }
        //    ReservationsInBooking = new ObservableCollection<Reservation>();
        //    ReservationsInBooking.CollectionChanged += ReservationsCollectionChanged;
        //    foreach (Reservation r in tmpBooking.ReservationsInBooking)
        //    {
        //        ReservationsInBooking.Add(new Reservation(r, uOW));
        //    }
        //    Payments.CollectionChanged += PaymentCollectionChanged;
        //    foreach (var p in tmpBooking.Payments)
        //    {
        //        Payments.Add(new Payment(p));
        //    }
        //    Excursion = uOW.GenericRepository.GetById<Excursion>(tmpBooking.Excursion.Id);
        //    //FullPrice = tmpBooking.FullPrice;
        //    CalculateRemainingAmount();
        //}

        #region Properties

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public ExcursionDate ExcursionDate { get; set; }

        public string Comment { get; set; }

        public float Commision { get; set; }

        public bool DifferentDates { get; set; }

        [Required]
        public virtual Excursion Excursion { get; set; }

        public bool IsPartners { get; set; }

        public float NetPrice { get; set; }

        public virtual Partner Partner { get; set; }

        public virtual ICollection<Payment> Payments { get; }

        public virtual ICollection<Reservation> ReservationsInBooking { get; }

        public bool SecondDepart { get; set; }

        [Required]
        public virtual User User { get; set; }

        #endregion Properties
    }
}