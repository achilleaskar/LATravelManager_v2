﻿using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
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

        private ObservableCollection<Reciept> _Reciepts;

        public ObservableCollection<Reciept> Reciepts
        {
            get
            {
                return _Reciepts;
            }

            set
            {
                if (_Reciepts == value)
                {
                    return;
                }

                _Reciepts = value;
                RaisePropertyChanged();
            }
        }

        public Personal_Booking()
        {
            Payments = new ObservableCollection<Payment>();
            Customers = new ObservableCollection<Customer>();
            Services = new ObservableCollection<Service>();
        }

        public DisabledInfo DisabledInfo { get; set; }
        public PartnerInfo PartnerInfo { get; set; }

        public bool Disabled { get; set; }
        public bool Reciept { get; set; }

        public string CancelReason { get; set; }

        public DateTime? DisableDate { get; set; }

        public User DisabledBy { get; set; }

        #endregion Constructors

        private bool _Group;

        public bool Group
        {
            get
            {
                return _Group;
            }

            set
            {
                if (_Group == value)
                {
                    return;
                }

                _Group = value;
                RaisePropertyChanged();
            }
        }

        public bool VoucherSent { get; set; }
        public bool ProformaSent { get; set; }

        #region Properties

        public static string Destination => "Atomiko";

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