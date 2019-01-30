using LATravelManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LATravelManager.Models
{
    public class Reservation : BaseModel
    {
        //public event EventHandler CustomerValueChanged;
        //public event EventHandler CustomerStartingPlaceChanged;
        //public event EventHandler CustomersChanged;

        #region Constructors

        public Reservation()
        {
            Tittle = "Η κράτηση";
            CustomersList.CollectionChanged += CustomersCollectionChanged;
        }

        //public Reservation(Reservation r, DataBaseManipulation.UnitOfWork uOW)
        //{
        //    Tittle = "Η κράτηση";
        //    foreach (var c in r.CustomersList)
        //    {
        //        CustomersList.Add(new Customer(c, uOW));
        //    }
        //    NoNameRoomType = (r.NoNameRoomType == null) ? null : uOW.GenericRepository.GetById<RoomType>(r.NoNameRoomType.Id);
        //    ReservationType = r.ReservationType;
        //    OnlyStay = r.OnlyStay;
        //    HB = r.HB;
        //    Hotel = (r.Hotel == null) ? null : uOW.GenericRepository.GetById<Hotel>(r.Hotel.Id);
        //    Transfer = r.Transfer;
        //    OnlyStay = r.OnlyStay;
        //    FirstHotel = r.FirstHotel;
        //    Room = (r.Room != null) ? uOW.GenericRepository.GetById<Room>(r.Room.Id) : null;
        //}

        #endregion Constructors

        #region Fields

        public const string BookingPropertyName = nameof(Booking);
        public const string CheckInPropertyName = nameof(CheckIn);
        public const string CheckOutPropertyName = nameof(CheckOut);
        public const string CustomersListPropertyName = nameof(CustomersList);
        public const string FirstHotelPropertyName = nameof(FirstHotel);
        public const string HBPropertyName = nameof(HB);

        public const string HotelPropertyName = nameof(Hotel);
        public const string NoNameRoomPropertyName = nameof(NoNameRoom);
        public const string NoNameRoomTypePropertyName = nameof(NoNameRoomType);
        public const string OnlyStayPropertyName = nameof(OnlyStay);
        public const string ReservationTypePropertyName = nameof(ReservationType);
        public const string RoomPropertyName = nameof(Room);
        public const string TransferPropertyName = nameof(Transfer);
        private Booking _Booking;
        private ObservableCollection<Customer> _CustomersList = new ObservableCollection<Customer>();
        private string _FirstHotel;
        private bool _HB = false;

        private Hotel _Hotel;

        private Room _NoNameRoom;
        private RoomType _NoNameRoomType;

        private bool _OnlyStay = false;

        private ReservationTypeEnum _ReservationType;

        private Room _Room;

        private bool _Transfer = false;

        #endregion Fields

        #region Enums

        public enum ReservationTypeEnum
        {
            Normal,
            Noname,
            Overbooked,
            NoRoom,
            Transfer
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// Sets and gets the Booking property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public Booking Booking
        {
            get
            {
                return _Booking;
            }

            set
            {
                if (_Booking == value)
                {
                    return;
                }

                _Booking = value;
                RaisePropertyChanged(BookingPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the CheckIn property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public DateTime CheckIn
        {
            get
            {
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

        /// <summary>
        /// Sets and gets the CheckOut property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public DateTime CheckOut
        {
            get
            {
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

        /// <summary>
        /// Sets and gets the CustomersList property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public virtual ObservableCollection<Customer> CustomersList
        {
            get
            {
                return _CustomersList;
            }

            set
            {
                if (_CustomersList == value)
                {
                    return;
                }

                _CustomersList = value;
                RaisePropertyChanged(CustomersListPropertyName);
            }
        }

        public string Dates => CheckIn.ToString("dd/MM") + "-" + CheckOut.ToString("dd/MM");
        [NotMapped]
        public int DaysCount
        {
            get { return Nights + 1; }
        }

        /// <summary>
        /// Sets and gets the FirstHotel property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        [StringLength(20)]
        public string FirstHotel
        {
            get
            {
                return _FirstHotel;
            }

            set
            {
                if (_FirstHotel == value)
                {
                    return;
                }
                if (_FirstHotel == null)
                    _FirstHotel = value;
                RaisePropertyChanged(FirstHotelPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the HB property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool HB
        {
            get
            {
                return _HB;
            }

            set
            {
                if (_HB == value)
                {
                    return;
                }

                _HB = value;
                RaisePropertyChanged(HBPropertyName);
            }
        }

        public string HBText => HB ? "HB" : "BB";
        /// <summary>
        /// Sets and gets the Hotel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Hotel Hotel
        {
            get
            {
                return _Hotel;
            }

            set
            {
                if (_Hotel == value)
                {
                    return;
                }

                _Hotel = value;
                RaisePropertyChanged(HotelPropertyName);
                RaisePropertyChanged(nameof(HotelName));
            }
        }

        public string HotelName => GetHotelName();

        public string Locations => GetLocations();

        /// <summary>
        /// Sets and gets the Names property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string Names => GetNames();

        public int Nights
        {
            get { return (int)(CheckOut - CheckIn).TotalDays; }
        }

        /// <summary>
        /// Sets and gets the NoNameRoom property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        [NotMapped]
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
                RaisePropertyChanged(NoNameRoomPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the NoNameRoomType property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public virtual RoomType NoNameRoomType
        {
            get
            {
                return _NoNameRoomType;
            }

            set
            {
                if (_NoNameRoomType == value)
                {
                    return;
                }

                _NoNameRoomType = value;
                RaisePropertyChanged(NoNameRoomTypePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the OnlyStay property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool OnlyStay
        {
            get
            {
                return _OnlyStay;
            }

            set
            {
                if (_OnlyStay == value)
                {
                    return;
                }

                _OnlyStay = value;
                RaisePropertyChanged(OnlyStayPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the ReservationType property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public ReservationTypeEnum ReservationType
        {
            get
            {
                return _ReservationType;
            }

            set
            {
                if (_ReservationType == value)
                {
                    return;
                }

                _ReservationType = value;
                RaisePropertyChanged(ReservationTypePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Room property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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
                RaisePropertyChanged(nameof(RoomTypeName));
                RaisePropertyChanged(nameof(HotelName));
            }
        }

        public string RoomTypeName => GetRoomTypeName();

        public string Tel => GetTel();

        /// <summary>
        /// Sets and gets the Transfer property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool Transfer
        {
            get
            {
                return _Transfer;
            }

            set
            {
                if (_Transfer == value)
                {
                    return;
                }

                _Transfer = value;
                RaisePropertyChanged(TransferPropertyName);
            }
        }

        #endregion Properties

        #region Methods

        public bool Contains(string key)
        {
            key = key.ToUpper();
            if (Booking == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(key))
            {
                return true;
            }
            if (Booking.Comment.ToUpper().Contains(key) || (Booking.IsPartners && Booking.Partner.Name.ToUpper().Contains(key)))
            {
                return true;
            }

            foreach (var c in CustomersList)
            {
                if (c.Name.ToUpper().Contains(key) || c.Surename.ToUpper().Contains(key) || (c.Tel != null && c.Tel.Contains(key)) || c.Comment.ToUpper().Contains(key) || (c.Email != null && c.Email.ToUpper().Contains(key))
                    || c.PassportNum.ToUpper().Contains(key) || c.StartingPlace.ToUpper().Contains(key))
                {
                    return true;
                }
            }
            return false;
        }

        internal void SelectAll()
        {
            foreach (Customer c in CustomersList)
            {
                c.IsSelected = true;
            }
        }

        private void CustomersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Customer customer in e.OldItems)
                {
                    //Removed items
                    customer.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Customer customer in e.NewItems)

                {
                    //Added items
                    customer.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
            RaisePropertyChanged(nameof(Tel));
            RaisePropertyChanged(nameof(Names));
            RaisePropertyChanged(nameof(Locations));
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // CustomerValueChanged?.Invoke(sender, e);
            //if (e.PropertyName==Customer.StartingPlacePropertyName)
            //{
            //    CustomerStartingPlaceChanged(sender, e);
            //}
        }

        private string GetHotelName()
        {
           
            switch (ReservationType)
            {
                case ReservationTypeEnum.Normal:
                    return Room.Hotel.Name;

                case ReservationTypeEnum.Noname:
                    return "NO NAME";

                case ReservationTypeEnum.Overbooked:
                    return Hotel.Name;

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
            //RoomType rt = null;

            //switch (ReservationType)
            //{
            //    case ReservationTypeEnum.Normal:
            //        rt = Room.RoomType;
            //        break;

            //    case ReservationTypeEnum.Noname:
            //        rt = NoNameRoomType;
            //        break;

            //    case ReservationTypeEnum.Overbooked:
            //        rt = NoNameRoomType;
            //        break;

            //    case ReservationTypeEnum.NoRoom:
            //        return "NO ROOM";
            //}

            //if (rt == null || rt.Id == 1)
            //{
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
            //}
            //if (rt != null)
            //    if (HB)
            //    {
            //        return rt.Name + "-HB";
            //    }
            //    else
            //    {
            //        return rt.Name;
            //    }
            //else
            //    return "Error";
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