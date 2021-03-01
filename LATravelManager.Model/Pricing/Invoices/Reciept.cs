using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Pricing.Invoices
{
    public class Reciept : BaseModel
    {
        public Reciept()
        {
            RecieptItems = new ObservableCollection<RecieptItem>();
        }

        public Company Company { get; set; }

        public int? CompanyId { get; set; }

        public User User { get; set; }



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

        public byte[] Content { get; set; }

        public bool Canceled { get; set; }

        public ObservableCollection<RecieptItem> RecieptItems { get; set; }
        public DateTime Date { get; set; }

        public Booking Booking { get; set; }
        public int? BookingId { get; set; }
        public int? Personal_BookingId { get; set; }
        public int? ThirdParty_BookingId { get; set; }
        public ThirdParty_Booking ThirdParty_Booking { get; set; }
        public Personal_Booking Personal_Booking { get; set; }
    }
}