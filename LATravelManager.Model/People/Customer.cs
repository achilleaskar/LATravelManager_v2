using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Services;

namespace LATravelManager.Model.People
{
    public class Customer : EditTracker, INamed
    {
        #region Constructors

        public Customer()
        {
            Services = new ObservableCollection<Service>();

            Age = 18;
            //DOB = DateTime.Now.AddYears(-20);
        }

        #endregion Constructors

        #region Fields

        private int _Board;

        private string _HotelName;
        private ObservableCollection<CustomerOptional> _OptionalExcursions;
        private DateTime _PassportExpiration;
        private DateTime _PassportPublish;
        private Brush _RoomColor;
        private int _SeatNum;
        private int _SeatNumRet;

        private string _Surename;
        private bool _IsSelected;

        #endregion Fields



        private bool _Selected;

        [NotMapped]
        public bool Selected
        {
            get
            {
                return _Selected;
            }

            set
            {
                if (_Selected == value)
                {
                    return;
                }

                _Selected = value;
                RaisePropertyChanged();
            }
        }
        #region Properties
        [NotMapped]
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }

            set
            {
                if (_IsSelected == value)
                {
                    return;
                }

                _IsSelected = value;
                RaisePropertyChanged();
            }
        }
        [Range(0, 18)]
        public int Age { get; set; }

        public int Board
        {
            get
            {
                return _Board;
            }

            set
            {
                if (_Board == value)
                {
                    return;
                }

                _Board = value;
                RaisePropertyChanged();
            }
        }

        public Bus BusGo { get; set; }

        public Bus BusReturn { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        [NotMapped]
        public int CId { get; set; }

        public int CustomerHasBusIndex { get; set; }

        public int CustomerHasPlaneIndex { get; set; }

        public int CustomerHasShipIndex { get; set; }

        public int DeserveDiscount { get; set; }

        public DateTime? DOB
        {
            get;
            set;
        }

        [StringLength(50, MinimumLength = 0)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        [EmailAddress(ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        public string Email { get; set; }

        [NotMapped]
        public string HotelName
        {
            get
            {
                if (Reservation != null)
                {
                    _HotelName = GetHotelName();
                }
                return _HotelName;
            }

            set
            {
                if (_HotelName == value)
                {
                    return;
                }

                _HotelName = value;
                RaisePropertyChanged();
            }
        }

        public int LeaderDriver { get; set; }

        [Required(ErrorMessage = "Το όνομα είναι υποχρεωτικό")]
        [RegularExpression(@"^[a-z- A-Z]+$", ErrorMessage = "Παρακαλώ χρησιμοποιήστε μόνο λατινικούς χαρακτήρες")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Το Όνομα μπορεί να είναι από 3 έως 20 χαρακτήρες")]
        public string Name { get; set; }

        public ObservableCollection<CustomerOptional> OptionalExcursions
        {
            get
            {
                return _OptionalExcursions;
            }

            set
            {
                if (_OptionalExcursions == value)
                {
                    return;
                }

                _OptionalExcursions = value;
                RaisePropertyChanged();
            }
        }

        public DateTime PassportExpiration
        {
            get
            {
                return _PassportExpiration;
            }

            set
            {
                if (_PassportExpiration == value)
                {
                    return;
                }

                _PassportExpiration = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(20, ErrorMessage = "Πολύ μεγάλο")]
        public string PassportNum { get; set; }

        public DateTime PassportPublish
        {
            get
            {
                return _PassportPublish;
            }

            set
            {
                if (_PassportPublish == value)
                {
                    return;
                }

                _PassportPublish = value;
                RaisePropertyChanged();
            }
        }

        public decimal Price { get; set; }

        public Reservation Reservation { get; set; }

        [NotMapped]
        public int ReservationId => Reservation != null ? Reservation.Id : 0;

        [StringLength(30, MinimumLength = 0)]
        public string ReturningPlace { get; set; }

        [NotMapped]
        public Brush RoomColor
        {
            get
            {
                return _RoomColor;
            }

            set
            {
                if (_RoomColor == value)
                {
                    return;
                }

                _RoomColor = value;
                RaisePropertyChanged();
            }
        }

        public int SeatNum
        {
            get
            {
                return _SeatNum;
            }

            set
            {
                if (_SeatNum == value)
                {
                    return;
                }

                _SeatNum = value;
                RaisePropertyChanged();
            }
        }

        public int SeatNumRet
        {
            get
            {
                return _SeatNumRet;
            }

            set
            {
                if (_SeatNumRet == value)
                {
                    return;
                }

                _SeatNumRet = value;
                RaisePropertyChanged();
            }
        }

        public ICollection<Service> Services { get; }

        [Required(ErrorMessage = "Επιλέξτε σημέιο αναχώρησης")]
        [StringLength(20)]
        public string StartingPlace { get; set; }

        [Required(ErrorMessage = "Το επίθετο είναι υποχρεωτικό.")]
        [RegularExpression(@"^[a-z- A-Z]+$", ErrorMessage = "Παρακαλώ χρησιμοπποιήστε μόνο λατινικούς χαρακτήρες")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Το Επίθετο μπορεί να είναι από 3 έως 20 χαρακτήρες")]
        public string Surename
        {
            get
            {
                return _Surename;
                //if (LeaderDriver == 0)
                //    return _Surename;
                //if (LeaderDriver == 1)
                //    return "(L)" + _Surename;
                //else if (LeaderDriver == 2)
                //    return "(D)" + _Surename;
                //else
                //    return "(G)" + _Surename;
            }

            set
            {
                if (_Surename == value)
                {
                    return;
                }
                if (LeaderDriver == 0)
                    _Surename = value;
                else if (LeaderDriver == 1)
                    _Surename = value.Replace("(L)", "");
                else if (LeaderDriver == 2)
                    _Surename = value.Replace("(L)", "");
                else
                    _Surename = value.Replace("(L)", "");
                RaisePropertyChanged();
            }
        }

        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες")]
        [Phone(ErrorMessage = "Το τηλέφωνο δν έχει τη σωστή μορφή")]
        public string Tel { get; set; }

        #endregion Properties

        #region Methods

        public string GetHotelName()
        {
            switch (Reservation.ReservationType)
            {
                case ReservationTypeEnum.Noname:
                    return "NO NAME";

                case ReservationTypeEnum.Normal:
                    return (Reservation.Room != null) ? Reservation.Room.Hotel.Name : "ERROR";

                case ReservationTypeEnum.NoRoom:
                    return "NoRoom";

                case ReservationTypeEnum.Overbooked:
                    return (Reservation.Hotel != null) ? Reservation.Hotel.Name : "ERROR";

                case ReservationTypeEnum.Transfer:
                    return "TRANSFER";

                case ReservationTypeEnum.OneDay:
                    return "ONEDAY";
            }
            return "ERROR";
        }

        public override string ToString()
        {
            return Surename + " " + Name;
        }

        #endregion Methods
    }
}