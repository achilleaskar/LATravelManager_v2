using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LATravelManager.Model.BookingData
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

        public  ICollection<Customer> Customers { get; }
        public bool IsPartners { get; set; }

        public decimal ExtraProfit { get; set; }

        public  Partner Partner { get; set; }

        public  ICollection<Payment> Payments { get; }

        public  ICollection<Service> Services { get; }

        [Required]
        public  User User { get; set; }
        [NotMapped]
        public string Dates { get; set; }

        #endregion Properties


     
    }
}