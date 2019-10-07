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

        public bool Disabled { get; set; }
        public bool Reciept { get; set; }

        public string CancelReason { get; set; }

        public DateTime? DisableDate { get; set; }

        public User DisabledBy { get; set; }

        #endregion Constructors

        #region Properties

        public string Destination
        {
            get
            {
                return "Atomiko";
            }
        }

        public string Comment { get; set; }

        public ICollection<Customer> Customers { get; }
        public bool IsPartners { get; set; }

        public decimal ExtraProfit { get; set; }

        public Partner Partner { get; set; }

        public ICollection<Payment> Payments { get; }

        public ObservableCollection<Service> Services { get; }

        [Required]
        public User User { get; set; }

        [NotMapped]
        public string Dates => GetDates();

        private string GetDates()
        {
            DateTime min = DateTime.Today, max = DateTime.Today;
            if (Services.Count > 0)
            {
                min = Services[0].TimeGo;
                max = Services[0].TimeReturn;
            }
            if (Services.Count > 1)
            {
                foreach (var s in Services)
                {
                    if (s.TimeGo < min)
                    {
                        min = s.TimeGo;
                    }
                    if (s.TimeReturn > max)
                    {
                        max = s.TimeReturn;
                    }
                }
            }

            return min.ToString("dd/MM") + "-" + max.ToString("dd/MM");
        }

        public string Names => GetNames();

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

        #endregion Properties
    }
}