using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Wrapper;

namespace LATravelManager.Model.LocalModels
{
    public class NoName
    {
        #region Properties

        public List<RoomWrapper> AvailableRooms { get; set; } = new List<RoomWrapper>();

        public bool Handled { get; set; }

        [Required]
        public ReservationWrapper Reservation { get; set; }

        #endregion Properties
    }
}