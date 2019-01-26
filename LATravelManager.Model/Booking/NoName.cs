using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class NoName
    {
        #region Properties

        public List<Room> AvailableRooms { get; set; } = new List<Room>();

        public bool Handled { get; set; }

        [Required]
        public Reservation Reservation { get; set; }

        #endregion Properties
    }
}