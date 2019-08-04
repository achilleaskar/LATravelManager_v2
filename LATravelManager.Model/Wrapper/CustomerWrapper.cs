using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.Model.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.Helpers
{
    public class CustomerWrapper : ModelWrapper<Customer>
    {
        #region Constructors

        public CustomerWrapper() : this(new Customer())
        {
        }

        public CustomerWrapper(Customer model) : base(model)
        {
            Services.CollectionChanged += Services_CollectionChanged;
        }

        private void Services_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(NumOfServices));
            RaisePropertyChanged(nameof(Services));

        }

        #endregion Constructors

        #region Fields

        private string _HotelName = "KENO";
        private bool _IsSelected;
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






        public int NumOfServices => Services.Count;

        public DateTime CheckIn
        {
            get
            {
                if (Reservation == null)
                {
                    return DateTime.Today;
                }
                if (Reservation.Booking.DifferentDates)
                {
                    return GetValue<DateTime>();
                }
                else
                {
                    return new ReservationWrapper(Reservation).CheckIn;
                }
            }

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
            get
            {
                if (Reservation == null)
                {
                    return DateTime.Today;
                }
                if (Reservation.Booking.DifferentDates)
                {
                    return GetValue<DateTime>();
                }
                else
                {
                    return new ReservationWrapper(Reservation).CheckOut;
                }
            }
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
            set { SetValue(value.ToUpper()); }
        }

        public decimal Price
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
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
            set
            {
                value = (value != null) ? value.ToUpper() : value;
                SetValue(value);
            }
        }

        public ObservableCollection<Service> Services
        {
            get { return GetValue<ObservableCollection<Service>>(); }
        }

        public string Surename
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public string Tel

        {
            get { return GetValue<string>(); }
            set { SetValue(string.IsNullOrEmpty(value)?null:value); }
        }

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

        public string GetRoomType()
        {
            switch (Reservation.ReservationType)
            {
                case ReservationTypeEnum.Noname:
                    return Reservation.NoNameRoomType.Name;

                case ReservationTypeEnum.Normal:
                    return (Reservation.Room != null) ? Reservation.Room.RoomType.Name : "ERROR";

                case ReservationTypeEnum.NoRoom:
                    return "NoRoom";

                case ReservationTypeEnum.Transfer:
                    return "TRANSFER";

                case ReservationTypeEnum.OneDay:
                    return "ONEDAY";
                case ReservationTypeEnum.Overbooked:
                    return (Reservation.NoNameRoomType != null) ? Reservation.NoNameRoomType.Name : "ERROR";
            }
            return "ERROR";
        }

        public override string ToString()
        {
            return Name;
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            //switch (propertyName)
            //{
            //    default:
            //        yield return "";
            //    //case nameof(ExcursionDates):
            //    //    if (ExcursionDates.Count == 0)
            //    //    {
            //    //        yield return "Παρακαλώ επιλέξτε Ημερομηνίες";
            //    //    }
            //    //    break;
            //}
            return null;
        }

        #endregion Methods
    }
}