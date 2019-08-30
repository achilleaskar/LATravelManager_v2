using LATravelManager.Model.Plan;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Hotels
{
    public class Room : EditTracker
    {
        #region Constructors

        public Room()
        {
            Options = new ObservableCollection<Option>();
            DailyBookingInfo = new List<BookingInfoPerDay>();
        }

        #endregion Constructors

        #region Properties

        public List<BookingInfoPerDay> DailyBookingInfo { get; set; }

        [NotMapped]
        public DateTime EndDate { get; set; }

        [Required]
        public Hotel Hotel { get; set; }

        public string Note { get; set; }

        public ObservableCollection<Option> Options { get; set; }

        [Required(ErrorMessage = "Επιλέξτε τύπο δωματίου")]
        public RoomType RoomType { get; set; }

        [NotMapped]
        public DateTime StartDate { get; set; }

        #endregion Properties
    }
}