using LATravelManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
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

        public virtual List<BookingInfoPerDay> DailyBookingInfo { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public virtual Hotel Hotel { get; set; }

        public string Note { get; set; }

        public virtual ObservableCollection<Option> Options { get; set; }

        [Required(ErrorMessage = "Επιλέξτε τύπο δωματίου")]
        public virtual RoomType RoomType { get; set; }

        public DateTime StartDate { get; set; }

        #endregion Properties
    }
}