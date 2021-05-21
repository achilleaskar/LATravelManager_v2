using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using System;
using System.Collections.Generic;

namespace LATravelManager.Model.DTOS
{
    public class BookingDTO : EditTracker
    {
        public BookingDTO()
        {

        }
        public ICollection<TransactionDTO> Transactions { get; set; }

        #region Properties

        public bool GroupBooking { get; set; }
        public string CancelReason { get; set; }

        public string Comment { get; set; }
        public decimal Commision { get; set; }
        public bool DifferentDates { get; set; }
        public bool Disabled { get; set; }
        public DateTime? DisableDate { get; set; }

        public Excursion Excursion { get; set; }

        public ExcursionDate ExcursionDate { get; set; }

        public bool IsPartners { get; set; }

        public decimal NetPrice { get; set; }

        public Partner Partner { get; set; }

        public ICollection<PaymentDTO> Payments { get; set; }

        public ICollection<ExtraServiceDTO> ExtraServices { get; set; }

        public bool Reciept { get; set; }

        public ICollection<ReservationDTO> ReservationsInBooking { get; set; }

        public bool SecondDepart { get; set; }

        public User User { get; set; }
        public int? UserId { get; set; }
        public ExcursionCategory ExcursionType { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int? PartnerId { get; set; }

        #endregion Properties
    }
}