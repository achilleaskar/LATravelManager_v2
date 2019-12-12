﻿using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace LATravelManager.Model.Wrapper
{
    public class ReservationWrapper : ModelWrapper<Reservation>
    {
        #region Constructors

        public ReservationWrapper() : this(new Reservation())
        {
        }


        public int Higher => GetHigher();

        public int GetHigher()
        {
            int higher = 0;
            foreach (var c in CustomersList)
            {
                if (c.LeaderDriver > higher)
                    higher = c.LeaderDriver;
            }
            return higher;
        }

        public ReservationWrapper(Reservation model) : base(model)
        {

        }

        #endregion Constructors

        #region Fields

        public DateTime _CheckOut;
        private Room _NoNameRoom;

        private Personal_BookingWrapper _PersonalModel;
        private ThirdParty_Booking_Wrapper _ThirdPartyModel;

        #endregion Fields

        #region Properties

        public Booking Booking
        {
            get { return GetValue<Booking>(); }
            set { SetValue(value); }
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

        public string GetHotelTel()
        {
            string tel = string.Empty;
            if (Room != null && Room.Hotel != null && !string.IsNullOrEmpty(Room.Hotel.Tel) && !Room.Hotel.Tel.StartsWith("000"))
            {
                tel = Room.Hotel.Tel;
            }
            else if (Hotel != null && !string.IsNullOrEmpty(Hotel.Tel))
            {
                tel = Hotel.Tel;
            }

            return tel.StartsWith("000") || tel.StartsWith("123") ? "" : tel;
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

        public List<Customer> CustomersList
        {
            get { return GetValue<List<Customer>>(); }
            set
            {
                SetValue(value);


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

        public string Dates => CheckIn.ToString("dd/MM") + "-" + CheckOut.ToString("dd/MM");

        public int DaysCount
        {
            get { return Nights + 1; }
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

        public ExcursionTypeEnum ExcursionType
        {
            get
            {
                if (Booking != null && Booking.Excursion != null)
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

        public string FirstHotel
        {
            get { return GetValue<string>(); }
            set
            {
                if (FirstHotel == null)
                    SetValue(value);
            }
        }

        public decimal FullPrice { get; set; }

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

        public string HotelDates
        {
            get
            {
                if (Booking != null && Booking.Excursion != null)
                    return (Booking.ExcursionDate != null && Booking.ExcursionDate.NightStart ? CheckIn.AddDays(1).ToString("dd.MM") : CheckIn.ToString("dd.MM")) + "-" + CheckOut.ToString("dd.MM");
                return "";
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

        //public RoomType NoNameRoomType
        //{
        //    get { return GetValue<RoomType>(); }
        //    set { SetValue(value); }
        //}

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
                        return PersonalModel.IsPartners ? PersonalModel.Partner.Name : "";

                    case ExcursionTypeEnum.ThirdParty:
                        return ThirdPartyModel.Partner.Name;

                    default:
                        return "";
                }
            }
        }

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

        public decimal Recieved { get; set; }
        public decimal Remaining { get; set; }

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

        public string RoomTypeNameByNum => GetRoomTypeNameByNum();

        public string Tel => GetTel();

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

        public string To => GetDestination();

        public bool Transfer
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
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

        #endregion Properties

        #region Methods

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
                if (c.Name.ToUpper().StartsWith(key) || c.Surename.ToUpper().StartsWith(key) || (c.Tel != null && c.Tel.StartsWith(key)) || (c.Email != null && c.Email.ToUpper().StartsWith(key))
                    || (!string.IsNullOrEmpty(c.PassportNum) && c.PassportNum.ToUpper().StartsWith(key)) || c.StartingPlace.ToUpper().Contains(key))
                {
                    return true;
                }
            }
            return false;
        }

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
                foreach (Customer customer in CustomersList.OrderBy(cc => cc.Id).ToList())
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
                        if (Room.RoomType.Id!=1)
                        {
                        roomname = Room.RoomType.Name + (Room.RoomType.Id == 1 ? ("(" + CustomersList.Count + ")") : "");

                        }
                        else
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
                    }
                }
                else if (ReservationType == ReservationTypeEnum.Noname)
                {
                    return GetNoNameType();

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
            else if (ReservationType == ReservationTypeEnum.Noname)
            {
                return GetNoNameType();
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

        public string GetNoNameType()
        {
            string roomname = string.Empty;
            switch (CustomersList.Count())
            {
                case 1:
                    roomname = "SINGLE(NN)";
                    break;

                case 2:
                    roomname = "DOUBLE(NN)";
                    break;

                case 3:
                    roomname = "TRIPLE(NN)";
                    break;

                case 4:
                    roomname = "QUAD(NN)";
                    break;

                case 5:
                    roomname = "5BED(NN)";
                    break;

                case 6:
                    roomname = "6BED(NN)";
                    break;
                default:
                    roomname = "VERY BIG";
                    break;
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

        public string GetSurnames()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in CustomersList)
            {
                sb.Append(c.Surename + " - ");
            }
            return sb.ToString().TrimEnd(new[] { ' ', '-' }).TrimEnd();
        }

        #endregion Methods
    }
}