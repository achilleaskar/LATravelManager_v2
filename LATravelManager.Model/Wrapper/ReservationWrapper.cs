using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LATravelManager.Model.Wrapper
{
    public class ReservationWrapper : ModelWrapper<Reservation>
    {
        #region Constructors

        public ReservationWrapper() : this(new Reservation())
        {
        }

        public ReservationWrapper(Reservation model) : base(model)
        {
        }

        #endregion Constructors

        #region Fields

        private Room _NoNameRoom;

        #endregion Fields

        #region Properties

        public decimal Recieved { get; set; }
        public decimal Remaining { get; set; }
        public decimal FullPrice { get; set; }

        public void CalculateAmounts()
        {
            decimal total = 0;
            if (ExcursionType != ExcursionTypeEnum.Personal && ExcursionType != ExcursionTypeEnum.ThirdParty)
            {
                Recieved = 0;
                foreach (Payment payment in Booking.Payments)
                {
                    Recieved += payment.Amount;
                }

                if (!Booking.IsPartners)
                {
                    foreach (Reservation r in Booking.ReservationsInBooking)
                    {
                        foreach (Customer c in r.CustomersList)
                        {
                            total += c.Price;
                        }
                    }
                    FullPrice = total;
                    Remaining = (total - Recieved) >= 1 ? total - Recieved : 0;
                }
                else
                {
                    Remaining = Booking.NetPrice <= Recieved ? 0 : Booking.NetPrice - Recieved;
                }
            }
            else if (ExcursionType == ExcursionTypeEnum.Personal)
            {
                PersonalModel.CalculateRemainingAmount();
                Recieved = PersonalModel.Recieved;
                Remaining = PersonalModel.Remaining;
            }
            else if (ExcursionType == ExcursionTypeEnum.ThirdParty)
            {
                ThirdPartyModel.FullPrice = ThirdPartyModel.NetPrice + ThirdPartyModel.Commision;
                ThirdPartyModel.CalculateRemainingAmount();
                Recieved = ThirdPartyModel.Recieved;
                Remaining = ThirdPartyModel.Remaining;
            }
            //else if (ExcursionType==ExcursionTypeEnum.Personal)
            //{
            //    PersonalModel.CalculateRemainingAmount();
            //}
        }

        public ExcursionTypeEnum ExcursionType
        {
            get
            {
                if (Booking != null)
                    if (Booking.Excursion.Id == 2)
                    {
                        return ExcursionTypeEnum.Bansko;
                    }
                    else if (Booking.Excursion.Id == 29)
                    {
                        return ExcursionTypeEnum.Skiathos;
                    }
                    else
                    {
                        return ExcursionTypeEnum.Group;
                    }
                else if (PersonalModel != null)
                {
                    return ExcursionTypeEnum.Personal;
                }
                else if (ThirdPartyModel != null)
                {
                    return ExcursionTypeEnum.ThirdParty;
                }
                else
                {
                    return ExcursionTypeEnum.Group;
                }
            }
        }



        public Booking Booking
        {
            get { return GetValue<Booking>(); }
            set { SetValue(value); }
        }

        private ThirdParty_Booking_Wrapper _ThirdPartyModel;

        public ThirdParty_Booking_Wrapper ThirdPartyModel
        {
            get
            {
                return _ThirdPartyModel;
            }

            set
            {
                if (_ThirdPartyModel == value)
                {
                    return;
                }

                _ThirdPartyModel = value;
                RaisePropertyChanged();
            }
        }

        private Personal_BookingWrapper _PersonalModel;

        public Personal_BookingWrapper PersonalModel
        {
            get
            {
                return _PersonalModel;
            }

            set
            {
                if (_PersonalModel == value)
                {
                    return;
                }

                _PersonalModel = value;
                RaisePropertyChanged();
            }
        }

        public string User
        {
            get
            {
                switch (ExcursionType)
                {
                    case ExcursionTypeEnum.Bansko:
                    case ExcursionTypeEnum.Skiathos:
                    case ExcursionTypeEnum.Group:
                        return Booking.User.ToString();

                    case ExcursionTypeEnum.Personal:
                        return PersonalModel.User != null ? PersonalModel.User.ToString() : "";

                    case ExcursionTypeEnum.ThirdParty:
                        return ThirdPartyModel.User != null ? ThirdPartyModel.User.ToString() : "";

                    default:
                        return "";
                }
            }
        }
        public string Comment
        {
            get
            {
                switch (ExcursionType)
                {
                    case ExcursionTypeEnum.Bansko:
                    case ExcursionTypeEnum.Skiathos:
                    case ExcursionTypeEnum.Group:
                        return Booking.Comment;

                    case ExcursionTypeEnum.Personal:
                        return PersonalModel.Comment;

                    case ExcursionTypeEnum.ThirdParty:
                        return ThirdPartyModel.Comment;

                    default:
                        return "";
                }
            }
        }

        public bool Reciept
        {
            get
            {
                switch (ExcursionType)
                {
                    case ExcursionTypeEnum.Bansko:
                    case ExcursionTypeEnum.Skiathos:
                    case ExcursionTypeEnum.Group:
                        return Booking.Reciept;

                    case ExcursionTypeEnum.Personal:
                        return PersonalModel.Reciept;

                    case ExcursionTypeEnum.ThirdParty:
                        return ThirdPartyModel.Reciept;

                    default:
                        return false;
                }
            }
        }

        public string Partner
        {
            get
            {
                switch (ExcursionType)
                {
                    case ExcursionTypeEnum.Bansko:
                    case ExcursionTypeEnum.Skiathos:
                    case ExcursionTypeEnum.Group:
                        return Booking.IsPartners ? Booking.Partner.Name : "";

                    case ExcursionTypeEnum.Personal:
                        return "";

                    case ExcursionTypeEnum.ThirdParty:
                        return ThirdPartyModel.Partner.Name;

                    default:
                        return "";
                }
            }
        }


        public DateTime DateOfCreate
        {
            get
            {
                switch (ExcursionType)
                {
                    case ExcursionTypeEnum.Bansko:
                    case ExcursionTypeEnum.Skiathos:
                    case ExcursionTypeEnum.Group:
                        return Booking.CreatedDate;

                    case ExcursionTypeEnum.Personal:
                        return PersonalModel.CreatedDate;

                    case ExcursionTypeEnum.ThirdParty:
                        return ThirdPartyModel.CreatedDate;

                    default:
                        return new DateTime();
                }
            }
        }



        public string To => GetDestination();

        private string GetDestination()
        {
            switch (ExcursionType)
            {
                case ExcursionTypeEnum.Bansko:
                case ExcursionTypeEnum.Skiathos:
                case ExcursionTypeEnum.Group:
                    return Booking.Excursion.Name;

                case ExcursionTypeEnum.Personal:
                    return PersonalModel.GetDestinations();

                case ExcursionTypeEnum.ThirdParty:
                    return ThirdPartyModel.City;

                default:
                    return "";
            }
        }

        public DateTime CheckIn
        {
            get
            {
                DateTime minValue = new DateTime();
                DateTime maxValue = new DateTime();

                if (ExcursionType == ExcursionTypeEnum.Personal && PersonalModel != null)
                {
                    foreach (var s in PersonalModel.Services)
                    {
                        if (minValue.Year < 2000)
                        {
                            minValue = s.TimeGo;
                        }
                        else if (s.TimeGo < minValue)
                        {
                            minValue = s.TimeGo;
                        }

                        if (maxValue.Year < 2000)
                        {
                            maxValue = s.TimeReturn;
                        }
                        else if (s.TimeReturn > maxValue)
                        {
                            maxValue = s.TimeReturn;
                        }
                    }
                }
                else if (ExcursionType == ExcursionTypeEnum.ThirdParty && ThirdPartyModel != null)
                {
                    CheckOut = ThirdPartyModel.CheckOut;
                    return ThirdPartyModel.CheckIn;
                }
                else
                {
                    if (Booking != null && !Booking.DifferentDates)
                    {
                        return Booking.ExcursionDate == null ? Booking.CheckIn : Booking.ExcursionDate.CheckIn;
                    }
                    foreach (Customer customer in CustomersList)
                    {
                        if (minValue.Year < 2000)
                        {
                            minValue = customer.CheckIn;
                        }
                        else if (customer.CheckIn < minValue)
                        {
                            minValue = customer.CheckIn;
                        }

                        if (maxValue.Year < 2000)
                        {
                            maxValue = customer.CheckOut;
                        }
                        else if (customer.CheckOut > maxValue)
                        {
                            maxValue = customer.CheckOut;
                        }
                    }
                }
                CheckOut = maxValue;
                return minValue;
            }
        }

        public DateTime _CheckOut;

        public DateTime CheckOut
        {
            set { _CheckOut = value; }
            get
            {
                if (_CheckOut == null || _CheckOut.Year < 2000)
                {
                    DateTime maxValue = new DateTime();
                    if (ExcursionType == ExcursionTypeEnum.Personal && PersonalModel != null)
                    {
                        foreach (var s in PersonalModel.Services)
                        {
                            if (maxValue.Year < 2000)
                            {
                                maxValue = s.TimeReturn;
                            }
                            else if (s.TimeReturn > maxValue)
                            {
                                maxValue = s.TimeReturn;
                            }
                        }
                        _CheckOut = maxValue;
                    }
                    else if (ExcursionType == ExcursionTypeEnum.ThirdParty && ThirdPartyModel != null)
                    {
                        _CheckOut = ThirdPartyModel.CheckOut;
                    }
                    else
                    {
                        if (Booking != null && !Booking.DifferentDates)
                        {
                            return Booking.ExcursionDate == null ? Booking.CheckOut : Booking.ExcursionDate.CheckOut;
                        }

                        foreach (Customer customer in CustomersList)
                        {
                            if (maxValue.Year < 2000)
                            {
                                maxValue = customer.CheckOut;
                            }
                            else if (customer.CheckOut > maxValue)
                            {
                                maxValue = customer.CheckOut;
                            }
                        }
                        _CheckOut = maxValue;
                    }
                }
                return _CheckOut;
            }
        }

        public List<Customer> CustomersList
        {
            get { return GetValue<List<Customer>>(); }
            set { SetValue(value); }
        }

        public string Dates => CheckIn.ToString("dd/MM") + "-" + CheckOut.ToString("dd/MM");

        public string HotelDates
        {
            get
            {
                if (Booking != null && Booking.Excursion != null)
                    return (Booking.Excursion.NightStart ? CheckIn.AddDays(1).ToString("dd.MM") : CheckIn.ToString("dd.MM")) + "-" + CheckOut.ToString("dd.MM");
                return "";
            }
        }

        public int DaysCount
        {
            get { return Nights + 1; }
        }

        public string FirstHotel
        {
            get { return GetValue<string>(); }
            set
            {
                if (FirstHotel == null)
                    SetValue(value);
            }
        }

        public bool HB
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string HBText => HB ? "HB" : "BB";

        public Hotel Hotel
        {
            get { return GetValue<Hotel>(); }
            set
            {
                SetValue(value);
                RaisePropertyChanged(nameof(HotelName));
            }
        }

        public string HotelName => GetHotelName();

        public string Locations => GetLocations();

        public string Names => GetNames();

        public int Nights
        {
            get { return (int)(CheckOut - CheckIn).TotalDays; }
        }

        public Room NoNameRoom
        {
            get
            {
                return _NoNameRoom;
            }

            set
            {
                if (_NoNameRoom == value)
                {
                    return;
                }

                _NoNameRoom = value;
                RaisePropertyChanged();
            }
        }

        public RoomType NoNameRoomType
        {
            get { return GetValue<RoomType>(); }
            set { SetValue(value); }
        }

        public int Number { get; set; }

        public bool OnlyStay
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
                foreach (var c in CustomersList)
                {
                    c.StartingPlace = "ONLY STAY";
                    c.CustomerHasBusIndex = 3;
                    c.CustomerHasShipIndex = 3;
                    c.CustomerHasPlaneIndex = 3;
                }
            }
        }

        public ReservationTypeEnum ReservationType
        {
            get { return GetValue<ReservationTypeEnum>(); }
            set { SetValue(value); }
        }

        public Room Room
        {
            get { return GetValue<Room>(); }
            set { SetValue(value); }
        }

        public string RoomTypeName => GetRoomTypeName();

        public string Tel => GetTel();

        public bool Transfer
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string RoomTypeNameByNum => GetRoomTypeNameByNum();

        public User UserWr
        {
            get
            {
                switch (ExcursionType)
                {
                    case ExcursionTypeEnum.Bansko:
                    case ExcursionTypeEnum.Skiathos:
                    case ExcursionTypeEnum.Group:
                        return Booking.User;

                    case ExcursionTypeEnum.Personal:
                        return PersonalModel.User;

                    case ExcursionTypeEnum.ThirdParty:
                        return ThirdPartyModel.User;

                    default:
                        return null;
                }
            }
        }

        public DateTime DisableDate
        {
            get
            {
                if (PersonalModel != null && PersonalModel.DisableDate != null)
                {
                    return PersonalModel.DisableDate.Value;
                }
                else if (ThirdPartyModel != null && ThirdPartyModel.DisableDate != null)
                {
                    return ThirdPartyModel.DisableDate.Value;
                }
                else if (Booking != null && Booking.DisableDate != null)
                {
                    return Booking.DisableDate.Value;
                }
                return new DateTime();
            }
        }

        public string DisableReason
        {
            get
            {
                if (PersonalModel != null && PersonalModel.CancelReason != null)
                {
                    return PersonalModel.CancelReason;
                }
                else if (ThirdPartyModel != null && ThirdPartyModel.CancelReason != null)
                {
                    return ThirdPartyModel.CancelReason;
                }
                else if (Booking != null && Booking.DisableDate != null)
                {
                    return Booking.CancelReason;
                }
                return "";
            }
        }

        private string GetRoomTypeNameByNum()
        {
            bool handled = false;
            string roomname = "";
            if (Room != null)
            {
                if (CustomersList.Count == 2 && (Room.RoomType.Id == 2 || Room.RoomType.Id == 3))
                {
                    roomname = Room.RoomType.Name;
                    handled = true;
                }
            }
            else if (NoNameRoomType != null)
            {
                if (CustomersList.Count == 2 && (NoNameRoomType.Id == 2 || NoNameRoomType.Id == 3))
                {
                    roomname = NoNameRoomType.Name;
                    handled = true;
                }
            }
            else if (ReservationType == ReservationTypeEnum.Transfer)
            {
                return "TRANSFER";
            }
            else if (ReservationType == ReservationTypeEnum.OneDay)
            {
                return "ONEDAY";
            }
            if (!handled)
            {
                switch (CustomersList.Count)
                {
                    case 1:
                        roomname = "SINGLE";
                        break;

                    case 2:
                        roomname = "TWIN";
                        break;

                    case 3:
                        roomname = "TRIPLE";
                        break;

                    case 4:
                        roomname = "QUAD";
                        break;

                    case 5:
                        roomname = "5BED";
                        break;

                    case 6:
                        roomname = "6BED";
                        break;
                }
            }

            if (HB)
            {
                roomname += "-HB";
            }

            return roomname;
        }

        #endregion Properties

        #region Methods

        public bool Contains(string key)
        {
            key = key.ToUpper();
            if (string.IsNullOrEmpty(key))
            {
                return true;
            }
            if (ExcursionType != ExcursionTypeEnum.Personal && ExcursionType != ExcursionTypeEnum.ThirdParty)
            {
                if (Booking == null)
                {
                    return false;
                }
                if ((!string.IsNullOrEmpty(Booking.Comment) && Booking.Comment.ToUpper().Contains(key)) || (!string.IsNullOrEmpty(HotelName) && HotelName.ToUpper().Contains(key)) || (Booking.IsPartners && Booking.Partner.Name.ToUpper().Contains(key)))
                {
                    return true;
                }
            }
            else if (ExcursionType == ExcursionTypeEnum.Personal)
            {
                if (PersonalModel == null)
                {
                    return false;
                }
                if ((!string.IsNullOrEmpty(PersonalModel.Comment) && PersonalModel.Comment.ToUpper().Contains(key)) || (PersonalModel.IsPartners && PersonalModel.Partner.Name.ToUpper().Contains(key)))
                {
                    return true;
                }
            }
            else if (ExcursionType == ExcursionTypeEnum.ThirdParty)
            {
                if (ThirdPartyModel == null)
                {
                    return false;
                }
                if ((!string.IsNullOrEmpty(ThirdPartyModel.Comment) && ThirdPartyModel.Comment.ToUpper().Contains(key)) || ThirdPartyModel.Partner.Name.ToUpper().Contains(key)
                    || ThirdPartyModel.Stay.ToUpper().Contains(key) || ThirdPartyModel.Description.ToUpper().Contains(key))
                {
                    return true;
                }
            }
            foreach (Customer c in CustomersList)
            {
                if (c.Name.ToUpper().StartsWith(key) || c.Surename.ToUpper().StartsWith(key) || (c.Tel != null && c.Tel.StartsWith(key)) || (!string.IsNullOrEmpty(c.Comment) && c.Comment.ToUpper().Contains(key)) || (c.Email != null && c.Email.ToUpper().StartsWith(key))
                    || (!string.IsNullOrEmpty(c.PassportNum) && c.PassportNum.ToUpper().StartsWith(key)) || c.StartingPlace.ToUpper().Contains(key))
                {
                    return true;
                }
            }
            return false;
        }

        private string GetHotelName()
        {
            if (PersonalModel != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Service s in PersonalModel.Services.Where(se => se is HotelService hs && hs.Hotel != null))
                {
                    sb.Append((s as HotelService).Hotel.Name + ", ");
                }
                return sb.ToString().TrimEnd(',', ' ');
            }
            else if (ThirdPartyModel != null)
            {
                return ThirdPartyModel.Stay;
            }
            else
            {
                switch (ReservationType)
                {
                    case ReservationTypeEnum.Normal:
                        return Room != null ? Room.Hotel.Name : "";

                    case ReservationTypeEnum.Noname:
                        return "NO NAME";

                    case ReservationTypeEnum.Overbooked:
                        return Hotel != null ? Hotel.Name + "*" : "";

                    case ReservationTypeEnum.NoRoom:
                        return "NO ROOM";

                    case ReservationTypeEnum.Transfer:
                        return "TRANSFER";

                    case ReservationTypeEnum.OneDay:
                        return "ONEDAY";
                }
            }
            return "Error";
        }

        private string GetLocations()
        {
            List<string> locations = new List<string>();
            StringBuilder sb = new StringBuilder();
            if (PersonalModel != null)
            {
                foreach (Customer customer in PersonalModel.Customers)
                {
                    if (!locations.Contains(customer.StartingPlace))
                        locations.Add(customer.StartingPlace);
                }
            }
            else if (ThirdPartyModel != null)
            {
                foreach (Customer customer in ThirdPartyModel.Customers)
                {
                    if (!locations.Contains(customer.StartingPlace))
                        locations.Add(customer.StartingPlace);
                }
            }
            else
            {
                foreach (Customer customer in CustomersList)
                {
                    if (!locations.Contains(customer.StartingPlace))
                        locations.Add(customer.StartingPlace);
                }
            }
            return string.Join(", ", locations);
        }

        private string GetNames()
        {
            StringBuilder sb = new StringBuilder();
            if (PersonalModel != null)
            {
                return PersonalModel.Names;
            }
            else if (ThirdPartyModel != null)
            {
                return ThirdPartyModel.Names;
            }
            else
            {
                foreach (Customer customer in CustomersList)
                {
                    sb.Append(customer.Surename + " " + customer.Name + ", ");
                }
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        private string GetRoomTypeName()
        {
            string roomname = "";

            if (PersonalModel != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Service s in PersonalModel.Services.Where(se => se is HotelService hs && hs.RoomType != null))
                {
                    if (!sb.ToString().Contains((s as HotelService).RoomType.Name))
                        sb.Append((s as HotelService).RoomType.Name + ", ");
                }
                return sb.ToString().TrimEnd(',', ' ');
            }
            else if (ThirdPartyModel != null)
            {
            }
            else
            {
                if (Room != null)
                {
                    if (Room.RoomType != null)
                    {
                        roomname = Room.RoomType.Name;
                    }
                }
                else if (NoNameRoomType != null)
                {
                    roomname = NoNameRoomType.Name;
                }
                else if (ReservationType == ReservationTypeEnum.Transfer)
                {
                    return "TRANSFER";
                }
                else if (ReservationType == ReservationTypeEnum.OneDay)
                {
                    return "ONEDAY";
                }
                else
                {
                    switch (CustomersList.Count)
                    {
                        case 1:
                            roomname = "SINGLE_";
                            break;

                        case 2:
                            roomname = "DOUBLE_";
                            break;

                        case 3:
                            roomname = "TRIPLE_";
                            break;

                        case 4:
                            roomname = "QUAD_";
                            break;

                        case 5:
                            roomname = "5BED_";
                            break;

                        case 6:
                            roomname = "6BED_";
                            break;
                    }
                }

                if (HB)
                {
                    roomname += "-HB";
                }
            }
            return roomname;
        }

        private string GetTel()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Customer customer in CustomersList)
            {
                if (!string.IsNullOrEmpty(customer.Tel))
                {
                    sb.Append(customer.Tel + ", ");
                }
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        #endregion Methods
    }
}