using LATravelManager.Model;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Windows.Media;

namespace LATravelManager.Models
{
    public class Customer : BaseModel,INamed
    {
        #region Fields + Constructors

        private int _Age = 18;
        private DateTime _CheckIn;
        private DateTime _CheckOut;
        private string _Comment = string.Empty;
        private int _CustomerHasBusIndex;
        private int _CustomerHasPlaneIndex;
        private int _CustomerHasShipIndex;
        private int _DeserveDiscount;
        private DateTime _DOB = DateTime.Now.AddYears(-20);
        private string _Email = string.Empty;
        private string _HotelName = "KENO";
        private bool _IsSelected = false;
        private string _Name = string.Empty;
        private OptionalExcursion _OptionalExcursion;
        private string _PassportNum = string.Empty;
        private float _Price;
        private string _PriceString = string.Empty;
        private Reservation _Reservation;
        private string _ReturningPlace = string.Empty;
        private Room _Room;
        private Brush _RoomColor = new SolidColorBrush(Colors.Green);
        private string _RoomNumber = "OXI";
        private string _RoomTypeName = "KENO";
        private string _StartingPlace = string.Empty;
        private string _Surename = string.Empty;
        private string _Tel = string.Empty;

        public Customer()
        {
            Tittle = "Ο πελάτης";
            ValidationProperties = new string[] { NamePropertyName, SurenamePropertyName, EmailPropertyName, TelPropertyName, AgePropertyName, AgePropertyName, StartingPlacePropertyName, PassportNumPropertyName };
        }

        public string AgeText => Age < 18 ? Age.ToString() : "";
        //public Customer(Customer customer, DataBaseManipulation.UnitOfWork uOW) : this()
        //{
        //    Age = customer.Age;
        //    CheckIn = customer.CheckIn;
        //    CheckOut = customer.CheckOut;
        //    Comment = customer.Comment;
        //    CustomerHasBusIndex = customer.CustomerHasBusIndex;
        //    CustomerHasPlaneIndex = customer.CustomerHasPlaneIndex;
        //    CustomerHasShipIndex = customer.CustomerHasShipIndex;
        //    DeserveDiscount = customer.DeserveDiscount;

        //    DOB = customer.DOB;
        //    Email = customer.Email;
        //    Handled = customer.Handled;
        //    Name = customer.Name;
        //    Surename = customer.Surename;
        //    PassportNum = customer.PassportNum;
        //    Price = customer.Price;
        //    StartingPlace = customer.StartingPlace;
        //    Tel = customer.Tel;
        //    RoomNumber = customer.RoomNumber;
        //    RoomColor = customer.RoomColor;
        //    Room = (customer.Room != null) ? uOW.GenericRepository.GetById<Room>(customer.Room.Id) : null;
        //}

        public const string AgePropertyName = nameof(Age);

        public const string CheckInPropertyName = nameof(CheckIn);

        public const string CheckOutPropertyName = nameof(CheckOut);

        public const string CommentPropertyName = nameof(Comment);

        public const string CustomerHasBusIndexPropertyName = nameof(CustomerHasBusIndex);

        public const string CustomerHasPlaneIndexPropertyName = nameof(CustomerHasPlaneIndex);

        public const string CustomerHasShipIndexPropertyName = nameof(CustomerHasShipIndex);

        public const string DeserveDiscountPropertyName = nameof(DeserveDiscount);

        public const string DOBPropertyName = nameof(DOB);

        public const string EmailPropertyName = nameof(Email);

        public const string HotelNamePropertyName = nameof(HotelName);

        public const string IsSelectedPropertyName = nameof(IsSelected);

        public const string NamePropertyName = nameof(Name);

        public const string OptionalExcursionPropertyName = nameof(OptionalExcursion);

        public const string PassportNumPropertyName = nameof(PassportNum);

        public const string PricePropertyName = nameof(Price);

        public const string PriceStringPropertyName = nameof(PriceString);

        public const string ReservationPropertyName = nameof(Reservation);

        public const string ReturningPlacePropertyName = nameof(ReturningPlace);

        public const string RoomColorPropertyName = nameof(RoomColor);

        public const string RoomNumberPropertyName = nameof(RoomNumber);

        public const string RoomPropertyName = nameof(Room);

        public const string RoomTypeNamePropertyName = nameof(RoomTypeName);

        public const string StartingPlacePropertyName = nameof(StartingPlace);

        public const string SurenamePropertyName = nameof(Surename);

        public const string TelPropertyName = nameof(Tel);

        #endregion Fields + Constructors

        #region Properties

        /// <summary>
        /// Sets and gets the Age property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Range(0, 18)]
        [DisplayName("1234")]
        public int Age
        {
            get
            {
                return _Age;
            }

            set
            {
                if (_Age == value)
                {
                    return;
                }

                _Age = value;
                RaisePropertyChanged(AgePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the CheckIn property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }

                _CheckIn = value;
                RaisePropertyChanged(CheckInPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the CheckOut property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime CheckOut
        {
            get
            {
                return _CheckOut;
            }

            set
            {
                if (_CheckOut == value)
                {
                    return;
                }

                _CheckOut = value;
                RaisePropertyChanged(CheckOutPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Comment property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(200, ErrorMessage = "Πολύ μεγάλο")]
        public string Comment
        {
            get
            {
                return _Comment;
            }

            set
            {
                if (_Comment == value)
                {
                    return;
                }

                _Comment = value;
                RaisePropertyChanged(CommentPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the CustomerHasBusIndex property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public int CustomerHasBusIndex
        {
            get
            {
                return _CustomerHasBusIndex;
            }

            set
            {
                if (_CustomerHasBusIndex == value)
                {
                    return;
                }

                _CustomerHasBusIndex = value;
                RaisePropertyChanged(CustomerHasBusIndexPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the CustomerHasPlaneIndex property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public int CustomerHasPlaneIndex
        {
            get
            {
                return _CustomerHasPlaneIndex;
            }

            set
            {
                if (_CustomerHasPlaneIndex == value)
                {
                    return;
                }

                _CustomerHasPlaneIndex = value;
                RaisePropertyChanged(CustomerHasPlaneIndexPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the CustomerHasShipIndex property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public int CustomerHasShipIndex
        {
            get
            {
                return _CustomerHasShipIndex;
            }

            set
            {
                if (_CustomerHasShipIndex == value)
                {
                    return;
                }

                _CustomerHasShipIndex = value;
                RaisePropertyChanged(CustomerHasShipIndexPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the DeserveDiscount property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int DeserveDiscount
        {
            get
            {
                return _DeserveDiscount;
            }

            set
            {
                if (_DeserveDiscount == value)
                {
                    return;
                }

                _DeserveDiscount = value;
                RaisePropertyChanged(DeserveDiscountPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the DOB property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime DOB
        {
            get
            {
                return _DOB;
            }

            set
            {
                if (_DOB == value)
                {
                    return;
                }

                _DOB = value;
                RaisePropertyChanged(DOBPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Email property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(30, MinimumLength = 0)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        [EmailAddress(ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        public string Email
        {
            get
            {
                if (_Email == string.Empty)
                {
                    return null;
                }
                return _Email;
            }

            set
            {
                if (_Email == value)
                {
                    return;
                }

                _Email = value;
                RaisePropertyChanged(EmailPropertyName);
            }
        }

        [NotMapped]
        public double EPSILON { get; private set; } = 0.00001;

        [NotMapped]
        public bool Handled { get; set; }

        /// <summary>
        /// Sets and gets the HotelName property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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
                RaisePropertyChanged(HotelNamePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the IsSelected property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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
                RaisePropertyChanged(IsSelectedPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Name property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Το όνομα είναι υποχρεωτικό")]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Παρακαλώ χρησιμοποιήστε μόνο λατινικούς χαρακτήρες")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Όνομα μπορεί να είναι απο 3 έως 20 χαρακτήρες")]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                value = value.ToUpper();
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the OptionalExcursion property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public OptionalExcursion OptionalExcursion
        {
            get
            {
                return _OptionalExcursion;
            }

            set
            {
                if (_OptionalExcursion == value)
                {
                    return;
                }

                _OptionalExcursion = value;
                RaisePropertyChanged(OptionalExcursionPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the PassportNum property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(20, ErrorMessage = "Πολύ μεγάλο")]
        public string PassportNum
        {
            get
            {
                return _PassportNum;
            }

            set
            {
                value = value.ToUpper();
                if (_PassportNum == value)
                {
                    return;
                }

                _PassportNum = value;
                RaisePropertyChanged(PassportNumPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Price property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public float Price
        {
            get
            {
                return _Price;
            }

            set
            {
                if (_Price == value)
                {
                    return;
                }

                if (Math.Abs(value - _Price) > EPSILON)
                {
                    _Price = (float)Math.Round(value, 2);
                    PriceString = value.ToString();
                }
                RaisePropertyChanged(PricePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the PriceString property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public string PriceString
        {
            get
            {
                return _PriceString;
            }

            set
            {
                if (_PriceString == value)
                {
                    return;
                }

                _PriceString = value;

                if (!string.IsNullOrEmpty(_PriceString))
                {
                    if (float.TryParse(value.Replace(',', '.'), NumberStyles.Any, new CultureInfo("en-US"), out var tmpDouble))
                    {
                        tmpDouble = (float)Math.Round(tmpDouble, 2);
                        Price = tmpDouble;
                        RaisePropertyChanged(PricePropertyName);
                        if (_PriceString[_PriceString.Length - 1] != '.' && _PriceString[_PriceString.Length - 1] != ',')
                        {
                            _PriceString = tmpDouble.ToString();
                        }
                    }
                    else
                    {
                        _PriceString = Price.ToString();
                    }
                }
                else
                {
                    _PriceString = "0";
                    Price = 0;
                }
                _PriceString = value;
                RaisePropertyChanged(PriceStringPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Reservation property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Reservation Reservation
        {
            get
            {
                return _Reservation;
            }

            set
            {
                if (_Reservation == value)
                {
                    return;
                }

                _Reservation = value;
                RaisePropertyChanged(ReservationPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the ReturningPlace property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string ReturningPlace
        {
            get
            {
                return _ReturningPlace;
            }

            set
            {
                if (_ReturningPlace == value)
                {
                    return;
                }

                _ReturningPlace = value;
                RaisePropertyChanged(ReturningPlacePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Room property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(20)]
        public virtual Room Room
        {
            get
            {
                return _Room;
            }

            set
            {
                if (_Room == value)
                {
                    return;
                }

                _Room = value;
                RaisePropertyChanged(RoomPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the RoomColor property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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
                RaisePropertyChanged(RoomColorPropertyName);
            }
        }

        [NotMapped]
        public string RoomNumber
        {
            get
            {
                return _RoomNumber;
            }
            set
            {
                if (_RoomNumber == value)
                {
                    return;
                }

                _RoomNumber = value;
                RaisePropertyChanged(RoomNumberPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the RoomTypeName property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public string RoomTypeName
        {
            get
            {
                if (Reservation != null)
                {
                    _RoomTypeName = GetRoomType();
                }
                return _RoomTypeName;
            }

            set
            {
                if (_RoomTypeName == value)
                {
                    return;
                }

                _RoomTypeName = value;
                RaisePropertyChanged(RoomTypeNamePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the StartingPlace property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Επιλέξτε σημέιο αναχώρησης")]
        [StringLength(20)]
        public string StartingPlace
        {
            get
            {
                return _StartingPlace;
            }

            set
            {
                if (_StartingPlace == value)
                {
                    return;
                }

                _StartingPlace = value;
                //if (string.IsNullOrEmpty(ReturningPlace))
                //{
                //    ReturningPlace = StartingPlace;
                //}
                RaisePropertyChanged(StartingPlacePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the RoomNumber property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        /// <summary>
        /// Sets and gets the Surename property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Το επίθετο είναι υποχρεωτικό.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Παρακαλώ χρησιμοπποιήστε μόνο λατινικούς χαρακτήρες")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Επίθετο μπορεί να είναι απο 3 έως 20 χαρακτήρες")]
        public string Surename
        {
            get
            {
                return _Surename;
            }

            set
            {
                value = value.ToUpper();
                if (_Surename == value)
                {
                    return;
                }

                _Surename = value;
                RaisePropertyChanged(SurenamePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Tel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(18, MinimumLength = 3, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες")]
        [Phone(ErrorMessage = "Το τηλέφωνο δν έχει τη σωστή μορφή")]
        public string Tel
        {
            get => string.IsNullOrEmpty(_Tel) ? null : _Tel;

            set
            {
                if (_Tel == value)
                {
                    return;
                }

                _Tel = value;
                RaisePropertyChanged(TelPropertyName);
            }
        }

        #endregion Properties

        #region Methods

        public string GetHotelName()
        {
            switch (Reservation.ReservationType)
            {
                case Reservation.ReservationTypeEnum.Noname:
                    return "NO NAME";

                case Reservation.ReservationTypeEnum.Normal:
                    return (Room != null) ? Room.Hotel.Name : "ERROR";

                case Reservation.ReservationTypeEnum.NoRoom:
                    return "NoRoom";

                case Reservation.ReservationTypeEnum.Overbooked:
                    return (Reservation.Hotel != null) ? Reservation.Hotel.Name : "ERROR";

                case Reservation.ReservationTypeEnum.Transfer:
                    return (Reservation.Hotel != null) ? Reservation.Hotel.Name : "TRANSFER";
            }
            return "ERROR";
        }

        public string GetRoomType()
        {
            switch (Reservation.ReservationType)
            {
                case Reservation.ReservationTypeEnum.Noname:
                    return Reservation.NoNameRoomType.Name;

                case Reservation.ReservationTypeEnum.Normal:
                    return (Room != null) ? Room.RoomType.Name : "ERROR";

                case Reservation.ReservationTypeEnum.NoRoom:
                    return "NoRoom";

                case Reservation.ReservationTypeEnum.Transfer:
                    return (Reservation.Hotel != null) ? Reservation.Hotel.Name : "TRANSFER";

                case Reservation.ReservationTypeEnum.Overbooked:
                    return (Reservation.NoNameRoomType != null) ? Reservation.NoNameRoomType.Name : "ERROR";
            }
            return "ERROR";
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}