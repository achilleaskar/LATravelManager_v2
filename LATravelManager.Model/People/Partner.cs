using LATravelManager.Model.BookingData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.People
{
    public class Partner : EditTracker, INamed
    {
        [MaxLength(400)]
        public string Emails { get; set; }

        #region Properties

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        public bool Person { get; set; }

        public List<Booking> Bookings { get; set; }
        public string Note { get; set; }

        [Required]
        [StringLength(20)]
        [Phone]
        public string Tel { get; set; }

        public Company CompanyInfo { get; set; }
        public int? CompanyInfoId { get; set; }

        #endregion Properties
    }
}