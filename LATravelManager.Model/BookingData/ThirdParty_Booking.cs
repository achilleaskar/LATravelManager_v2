using LATravelManager.Model.People;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LATravelManager.Model.BookingData
{
    public class ThirdParty_Booking : EditTracker
    {
        #region Constructors

        public ThirdParty_Booking()
        {
            Payments = new ObservableCollection<Payment>();
            Customers = new ObservableCollection<Customer>();
        }

        #endregion Constructors

        #region Properties

        public CustomFile File { get; set; }
        public string CancelReason { get; set; }

        public ObservableCollection<ChangeInBooking> ChangesInBooking { get; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public string Comment { get; set; }

        public decimal Commision { get; set; }

        public ICollection<Customer> Customers { get; }

        public string Dates => CheckIn.ToString("dd/MM") + "-" + CheckOut.ToString("dd/MM");

        [StringLength(250)]
        public string Description { get; set; }

        public bool Disabled { get; set; }
        public DateTime? DisableDate { get; set; }
        public User DisabledBy { get; set; }
        public string Names => GetNames();
        public decimal NetPrice { get; set; }
        public Partner Partner { get; set; }
        public ICollection<Payment> Payments { get; }
        public bool Reciept { get; set; }
        public string Stay { get; set; }
        public string City { get; set; }

        [Required]
        public User User { get; set; }

        #endregion Properties

        #region Methods

        private string GetNames()
        {
            StringBuilder sb = new StringBuilder();
            if (Customers.Count > 0)
            {
                foreach (Customer customer in Customers)
                {
                    sb.Append(customer.Surename + " " + customer.Name + ", ");
                }
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        #endregion Methods
    }
}