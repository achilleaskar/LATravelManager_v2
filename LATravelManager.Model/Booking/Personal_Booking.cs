using LATravelManager.Model.Services;
using LATravelManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Booking
{
    public class Personal_Booking : EditTracker
    {
        #region Constructors
        public Personal_Booking()
        {
            Payments = new ObservableCollection<Payment>();
            Customers = new ObservableCollection<Customer>();
            Services = new ObservableCollection<Service>();
        }

        #endregion Constructors

        #region Properties

        public string Comment { get; set; }

        public float Commision { get; set; }


        public virtual ICollection<Customer> Customers { get; }
        public bool IsPartners { get; set; }

        public float NetPrice { get; set; }

        public virtual Partner Partner { get; set; }

        public virtual ICollection<Payment> Payments { get; }

        public virtual ICollection<Service> Services { get; }

        [Required]
        public virtual User User { get; set; }

        #endregion Properties
    }
}