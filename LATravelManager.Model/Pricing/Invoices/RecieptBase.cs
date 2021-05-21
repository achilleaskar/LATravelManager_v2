using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;

namespace LATravelManager.Model.Pricing.Invoices
{
    public class RecieptBase : BaseModel
    {
        public RecieptBase()
        {
            RecieptItems = new ObservableCollection<RecieptItem>();
        }

        public Company Company { get; set; }

        public int? CompanyId { get; set; }

        public User User { get; set; }




        private decimal _Discount;

        [NotMapped]
        public decimal Discount
        {
            get
            {
                return _Discount;
            }

            set
            {
                if (_Discount == value)
                {
                    return;
                }

                _Discount = value;
                RaisePropertyChanged();
            }
        }




        private decimal _FinalAmount;

        [NotMapped]
        public decimal FinalAmount
        {
            get
            {
                return _FinalAmount;
            }

            set
            {
                if (_FinalAmount == value)
                {
                    return;
                }

                _FinalAmount = value;
                RaisePropertyChanged();
            }
        }

        private decimal _Total;


        public decimal Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        public RecieptTypeEnum RecieptType { get; set; }

        [Required]
        public RecieptSeries Series { get; set; }

        [Required]
        public int RecieptNumber { get; set; }

        public string RecieptDescription { get; set; }

        private string _FileName;

        [StringLength(100)]
        public string FileName
        {
            get
            {
                return _FileName;
            }

            set
            {
                if (_FileName == value)
                {
                    return;
                }

                _FileName = value;
                RaisePropertyChanged();
            }
        }


        public bool Canceled { get; set; }

        public ObservableCollection<RecieptItem> RecieptItems { get; set; }
        public DateTime Date { get; set; }

        public Booking Booking { get; set; }
        public int? BookingId { get; set; }
        public int? Personal_BookingId { get; set; }
        public int? ThirdParty_BookingId { get; set; }
        public ThirdParty_Booking ThirdParty_Booking { get; set; }
        public Personal_Booking Personal_Booking { get; set; }
        public string Dates => GetDates();

        private string GetDates()
        {
            if (Booking != null)
            {
                return Booking.Dates;
            }
            else if (Personal_Booking != null)
            {
                return Personal_Booking.Dates;
            }
            else if (ThirdParty_Booking != null)
            {
                return ThirdParty_Booking.Dates;
            }
            return "";
        }

        public string GetNumber()
        {
            switch (RecieptType)
            {
                case RecieptTypeEnum.ServiceReciept:
                    return "ΑΠΥ" + RecieptNumber;
                case RecieptTypeEnum.ServiceInvoice:
                    return "ΤΠΥ" + RecieptNumber;
                case RecieptTypeEnum.AirTicketsReciept:
                    return "ΑΠΕ" + RecieptNumber;
                case RecieptTypeEnum.FerryTicketsReciept:
                    return "ΑΠΑ" + RecieptNumber;
                case RecieptTypeEnum.CancelationInvoice:
                    return "ΑΤ" + RecieptNumber;
                case RecieptTypeEnum.CreditInvoice:
                    return "ΠΤ" + RecieptNumber;
                default:
                    return "Error";
                   
            }
        }
    }
}
