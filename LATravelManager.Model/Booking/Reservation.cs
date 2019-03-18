using LATravelManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LATravelManager.Model.Enums;

namespace LATravelManager.Models
{
    public class Reservation : EditTracker
    {
        public Reservation()
        {
            CustomersList = new List<Customer>();
        }
        #region Properties

        [Required]
        public Booking Booking { get; set; }
       
        public virtual List<Customer> CustomersList { get; set; }

        [StringLength(20)]
        public string FirstHotel { get; set; }

        public bool HB { get; set; }

        public Hotel Hotel { get; set; }

        public virtual RoomType NoNameRoomType { get; set; }

        public bool OnlyStay { get; set; }

        [Required]
        public ReservationTypeEnum ReservationType { get; set; }

        public virtual Room Room { get; set; }

        public bool Transfer { get; set; }

        #endregion Properties
    }
}