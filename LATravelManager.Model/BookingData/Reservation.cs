using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;

namespace LATravelManager.Model.BookingData
{
    public class Reservation : EditTracker
    {
        #region Constructors

        [NotMapped]
        public string LastHotel { get; set; }

        [NotMapped]
        public string LastCustomers { get; set; }

        [NotMapped]
        public string LastRoomtype { get; set; }

        public Reservation()
        {
            CustomersList = new List<Customer>();
        }

        #endregion Constructors

        #region Properties

        public Booking Booking { get; set; }

        public List<Customer> CustomersList { get; set; }

        [StringLength(20)]
        public string FirstHotel { get; set; }

        public bool HB { get; set; }

        public Hotel Hotel { get; set; }

        public RoomType NoNameRoomType { get; set; }

        public bool OnlyStay { get; set; }

        [Required]
        public ReservationTypeEnum ReservationType { get; set; }

        public Room Room { get; set; }

        public bool Transfer { get; set; }

        #endregion Properties
    }
}