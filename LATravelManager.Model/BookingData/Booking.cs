using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;

namespace LATravelManager.Model.BookingData
{
    public class Booking : EditTracker
    {
        #region Constructors

        public Booking()
        {
            Payments = new ObservableCollection<Payment>();
            ExtraServices = new ObservableCollection<ExtraService>();
            ReservationsInBooking = new ObservableCollection<Reservation>();
            ChangesInBooking = new ObservableCollection<ChangeInBooking>();
            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(3);
        }

        #endregion Constructors

        #region Properties

        // public int LeaderDriver => GetHigher(); ReservationsInBooking.OrderByDescending(r=> r.CustomersList.OrderByDescending(c=>c.LeaderDriver).First().LeaderDriver).Select;

        public bool GroupBooking { get; set; }
        public bool VoucherSent { get; set; }
        public bool ProformaSent { get; set; }
        public bool RoomingListIncluded { get; set; }

        public void GetHigher()
        {
            int higher = 0;
            foreach (var r in ReservationsInBooking)
            {
                foreach (var c in r.CustomersList)
                {
                    if (c.LeaderDriver > higher)
                    {
                        higher = c.LeaderDriver;
                    }
                }
            }
        }

        [NotMapped]
        public int CustomId { get; set; }

        public string CancelReason { get; set; }
        public ObservableCollection<ChangeInBooking> ChangesInBooking { get; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

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

        public string Comment { get; set; }
        public decimal Commision { get; set; }
        public string Dates => CheckIn.ToString("dd/MM") + "-" + CheckOut.ToString("dd/MM");
        public bool DifferentDates { get; set; }
        public bool Disabled { get; set; }
        public DateTime? DisableDate { get; set; }
        public User DisabledBy { get; set; }

        [Required]
        public Excursion Excursion { get; set; }

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
        public ExcursionDate ExcursionDate { get; set; }

        public bool IsPartners { get; set; }
        public string Names => GetNames();

        [NotMapped]
        public decimal NetPrice { get; set; }

        public Partner Partner { get; set; }

        //public Booking(Booking booking, object uOW)
        //{
        //    //this.booking = booking;
        //    //this.uOW = uOW;
        //}

        public string PartnerEmail { get; set; }

        //public Booking()
        //{
        //    ReservationsInBooking = new ObservableCollection<Reservation>();
        //    Payments.CollectionChanged += PaymentCollectionChanged;
        //    Customers.CollectionChanged += Customers_CollectionChanged;
        //    ReservationsInBooking.CollectionChanged += ReservationsCollectionChanged;
        //    CheckIn = DateTime.Today;
        //}
        public ICollection<Payment> Payments { get; }

        public ICollection<ExtraService> ExtraServices { get; }

        public bool Reciept { get; set; }
        public ObservableCollection<Reservation> ReservationsInBooking { get; }

        public bool SecondDepart { get; set; }

        [Required]
        public User User { get; set; }

        #endregion Properties

        #region Methods

        private string GetNames()
        {
            StringBuilder sb = new StringBuilder();
            if (ReservationsInBooking.Count > 0)
            {
                foreach (Customer customer in ReservationsInBooking[0].CustomersList)
                {
                    sb.Append(customer.Surename + " " + customer.Name + ", ");
                }
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        #endregion Methods
    }
}