using LATravelManager.Model.Hotels;
using System;
using System.Collections.Generic;

namespace LATravelManager.Model.DTOS
{
    public class ReservationDTO
    {
        public ReservationDTO()
        {

        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<CustomerDTO> CustomersList { get; set; }

        public string FirstHotel { get; set; }

        public bool HB { get; set; }

        public Hotel Hotel { get; set; }

        public RoomType NoNameRoomType { get; set; }

        public bool OnlyStay { get; set; }

        public int BookingId { get; set; }

        public Room Room { get; set; }
        public ReservationTypeEnum ReservationType { get; set; }

        public int? OBHotelID { get; set; }

        public bool Transfer { get; set; }
        public int? HotelID { get; set; }
        public int? RoomTypeId { get; set; }
    }
}