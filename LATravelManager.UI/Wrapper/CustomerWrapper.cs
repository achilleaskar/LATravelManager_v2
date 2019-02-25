using LATravelManager.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Windows.Media;

namespace LATravelManager.UI.Wrapper
{
    public class CustomerWrapper : ModelWrapper<Customer>
    {

        #region Constructors

        public CustomerWrapper() : base(new Customer())
        {
        }

        public CustomerWrapper(Customer model) : base(model)
        {
            _PriceString = Price.ToString();
        }

        #endregion Constructors

        #region Fields

        private string _HotelName = "KENO";
        private bool _IsSelected;
        private float _Price;
        private string _PriceString;
        private Brush _RoomColor = new SolidColorBrush(Colors.Green);
        private string _RoomNumber = "OXI";
        private string _RoomTypeName = "KENO";

        #endregion Fields

        #region Properties

        public int Age
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public DateTime CheckIn
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                if (CheckOut < CheckIn)
                {
                    CheckOut = CheckIn;
                }
            }
        }

        public DateTime CheckOut
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                if (CheckOut < CheckIn)
                {
                    CheckIn = CheckOut;
                }
            }
        }

        public string Comment
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public int CustomerHasBusIndex
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int CustomerHasShipIndex
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int DeserveDiscount
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public DateTime? DOB
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool Handled { get; set; }

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

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public OptionalExcursion OptionalExcursion
        {
            get { return GetValue<OptionalExcursion>(); }
            set { SetValue(value); }
        }

        public string PassportNum
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public float Price
        {
            get
            {
                return GetValue<float>();
            }

            set
            {
                if (_Price == value)
                {
                    return;
                }

                if (Math.Abs(value - _Price) > 0.001)
                {
                    _Price = (float)Math.Round(value, 2);
                    SetValue(_Price);
                    PriceString = value.ToString();
                }
                RaisePropertyChanged();
            }
        }

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
                    if (float.TryParse(value.Replace(',', '.'), NumberStyles.Any, new CultureInfo("en-US"), out var tmpFloat))
                    {
                        tmpFloat = (float)Math.Round(tmpFloat, 2);
                        Price = tmpFloat;
                        if (_PriceString[_PriceString.Length - 1] != '.' && _PriceString[_PriceString.Length - 1] != ',')
                        {
                            _PriceString = tmpFloat.ToString();
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
                RaisePropertyChanged();
            }
        }

        public Reservation Reservation
        {
            get { return GetValue<Reservation>(); }
            set { SetValue(value); }
        }

        public string ReturningPlace
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

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
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

        public string StartingPlace
        {
            get { return GetValue<string>(); }
            set {
                value = (value != null) ? value.ToUpper() : value;
                SetValue(value);
            }
        }

        public string Surename
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public string Tel

        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
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
                    return (Reservation.Room != null) ? Reservation.Room.Hotel.Name : "ERROR";

                case Reservation.ReservationTypeEnum.NoRoom:
                    return "NoRoom";

                case Reservation.ReservationTypeEnum.Overbooked:
                    return (Reservation.Hotel != null) ? Reservation.Hotel.Name : "ERROR";

                case Reservation.ReservationTypeEnum.Transfer:
                    return "TRANSFER";
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
                    return (Reservation.Room != null) ? Reservation.Room.RoomType.Name : "ERROR";

                case Reservation.ReservationTypeEnum.NoRoom:
                    return "NoRoom";

                case Reservation.ReservationTypeEnum.Transfer:
                    return "TRANSFER";

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