using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using System;
using System.Collections.Generic;
using System.Text;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.Wrapper
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

        public float Recieved { get; set; }
        public float Remaining { get; set; }
        public float FullPrice { get; set; }

        public void CalculateAmounts()
        {
            float total = 0;

            Recieved = 0;
            foreach (Payment payment in Booking.Payments)
                Recieved += payment.Amount;

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
                Remaining = total - Recieved;
            }
            else
            {
                Remaining = Booking.NetPrice - Recieved;
            }
        }

        public Booking Booking
        {
            get { return GetValue<Booking>(); }
            set { SetValue(value); }
        }

        public DateTime CheckIn
        {
            get
            {
                if (Booking != null && !Booking.DifferentDates)
                {
                    return Booking.ExcursionDate == null ? Booking.CheckIn : Booking.ExcursionDate.CheckIn;
                }
                DateTime minValue = new DateTime();
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
                }
                return minValue;
            }
        }

        public DateTime CheckOut
        {
            get
            {
                if (Booking != null && !Booking.DifferentDates)
                {
                    return Booking.ExcursionDate == null ? Booking.CheckOut : Booking.ExcursionDate.CheckOut;
                }

                DateTime maxValue = new DateTime();
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
                return maxValue;
            }
        }

        public List<Customer> CustomersList
        {
            get { return GetValue<List<Customer>>(); }
            set { SetValue(value); }
        }

        public string Dates => CheckIn.ToString("dd/MM") + "-" + CheckOut.ToString("dd/MM");
        public string HotelDates => (Booking.Excursion.NightStart ? CheckIn.AddDays(1).ToString("dd.MM") : CheckIn.ToString("dd.MM")) + "-" + CheckOut.ToString("dd.MM");

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
            set { SetValue(value); }
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

        #endregion Properties

        #region Methods

        public bool Contains(string key)
        {
            key = key.ToUpper();
            if (string.IsNullOrEmpty(key))
            {
                return true;
            }
            if (Booking == null)
            {
                return false;
            }
            if ((!string.IsNullOrEmpty(Booking.Comment) && Booking.Comment.ToUpper().Contains(key)) || (Booking.IsPartners && Booking.Partner.Name.ToUpper().Contains(key)))
            {
                return true;
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
            switch (ReservationType)
            {
                case ReservationTypeEnum.Normal:
                    return Room != null ? Room.Hotel.Name : "";

                case ReservationTypeEnum.Noname:
                    return "NO NAME";

                case ReservationTypeEnum.Overbooked:
                    return Hotel != null ? Hotel.Name : "";

                case ReservationTypeEnum.NoRoom:
                    return "NO ROOM";

                case ReservationTypeEnum.Transfer:
                    return "TRANSFER";
            }
            return "Error";
        }

        private string GetLocations()
        {
            List<string> locations = new List<string>();
            foreach (Customer customer in CustomersList)
            {
                if (!locations.Contains(customer.StartingPlace))
                    locations.Add(customer.StartingPlace);
            }
            return string.Join(", ", locations);
        }

        private string GetNames()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Customer customer in CustomersList)
            {
                sb.Append(customer.Surename + " " + customer.Name + ", ");
            }
            return sb.ToString().TrimEnd(',', ' ');
        }

        private string GetRoomTypeName()
        {
            string roomname = "";
            switch (CustomersList.Count)
            {
                case 1:
                    roomname = "SINGLE";
                    break;

                case 2:
                    roomname = "DOUBLE";
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

            if (HB)
            {
                roomname += "-HB";
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