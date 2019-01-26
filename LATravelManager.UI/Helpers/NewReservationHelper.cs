using GalaSoft.MvvmLight;
using LATravelManager.Models;
using System;
using System.Linq;
using System.Windows.Media;

public class NewReservationHelper : ViewModelBase
{
    #region Fields

    private readonly RoomsManager RoomsManager = new RoomsManager();

    #endregion Fields

    #region Properties

    public UnitOfWork UOW
    {
        get
        {
            return ServiceLocator.Current.GetInstance<UnitOfWork>(Definitions.UnitOfWorkKey);
        }
    }

    #endregion Properties

    #region Methods

    public Customer CreateRandomCustomer()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string name = new string(Enumerable.Repeat(chars, 5)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        string surename = new string(Enumerable.Repeat(chars, 5)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        string passport = new string(Enumerable.Repeat(chars, 8)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        return new Customer { Age = 12, Name = name, Surename = surename, Price = 35, Tel = "6981001676", StartingPlace = "Αθήνα", PassportNum = passport };
    }

    public void MakeNonameReservation(Booking booking, RoomType roomType, bool hb, bool all, bool onlystay)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation { ReservationType = Reservation.ReservationTypeEnum.Noname, FirstHotel = "ΝΟ ΝΑΜΕ", HB = hb, NoNameRoomType = roomType, OnlyStay = onlystay };
        foreach (Customer customer in booking.Customers)
        {
            if (customer.IsSelected || all)
            {
                hasCustomers = true;
                customer.RoomNumber = "NN" + (booking.ReservationsInBooking.Count + 1).ToString();
                customer.HotelName = "NO NAME";
                customer.Handled = true;
                customer.RoomTypeName = roomType.Name;
                newRes.CustomersList.Add(customer);
                if (booking.ReservationsInBooking.Count() % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!booking.DifferentDates)
                {
                    customer.CheckIn = booking.CheckIn;
                    customer.CheckOut = booking.CheckOut;
                }
            }
        }
        if (hasCustomers)
        {
            //check if can addNoName
            booking.ReservationsInBooking.Add(newRes);
            booking.Customers.OrderBy(x => x.RoomNumber);
            foreach (Customer customer in booking.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    public void OverBookHotel(Booking booking, Hotel hotel, RoomType roomType, bool all, bool onlyStay, bool hb)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = Reservation.ReservationTypeEnum.Overbooked,
            FirstHotel = hotel.Name,
            Hotel = UOW.GenericRepository.GetById<Hotel>(hotel.Id),
            NoNameRoomType = UOW.GenericRepository.GetById<RoomType>(roomType.Id),
            OnlyStay = onlyStay,
            HB = hb
        };
        foreach (Customer customer in booking.Customers)
        {
            if (customer.IsSelected || all)
            {
                hasCustomers = true;
                customer.Handled = true;
                customer.HotelName = newRes.Hotel.Name;
                customer.RoomTypeName = newRes.NoNameRoomType.Name;
                customer.RoomNumber = "OB" + (booking.ReservationsInBooking.Count + 1).ToString();
                if (booking.ReservationsInBooking.Count() % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!booking.DifferentDates)
                {
                    customer.CheckIn = booking.CheckIn;
                    customer.CheckOut = booking.CheckOut;
                }
                newRes.CustomersList.Add(customer);
            }
        }
        if (hasCustomers)
        {
            booking.ReservationsInBooking.Add(newRes);
            booking.Customers.OrderBy(x => x.RoomNumber);
            foreach (Customer customer in booking.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    public void PutCustomersInRoom(Booking booking, Room SelectedRoom, bool all, bool onlyStay, bool hb)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = Reservation.ReservationTypeEnum.Normal,
            OnlyStay = onlyStay,
            Room = UOW.GenericRepository.GetById<Room>(SelectedRoom.Id),
            FirstHotel = SelectedRoom.Hotel.Name,
            HB = hb
        };
        foreach (Customer customer in booking.Customers)
        {
            if (customer.IsSelected || all)
            {
                hasCustomers = true;
                customer.Handled = true;
                customer.RoomNumber = (booking.ReservationsInBooking.Count + 1).ToString();
                if (booking.ReservationsInBooking.Count() % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!booking.DifferentDates)
                {
                    customer.CheckIn = booking.CheckIn;
                    customer.CheckOut = booking.CheckOut;
                }
                customer.Room = SelectedRoom;
                customer.HotelName = newRes.Room.Hotel.Name;
                customer.RoomTypeName = newRes.Room.RoomType.Name;

                newRes.CustomersList.Add(customer);
            }
        }
        if (hasCustomers)
        {
            if (SelectedRoom.CanAddReservationToRoom(newRes))
            {
                SelectedRoom.MakeReservation(newRes);
            }
            booking.ReservationsInBooking.Add(newRes);
            RoomsManager.SortCustomers(booking);
            foreach (Customer customer in booking.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    public Booking Save(Payment Payment, Booking Booking, Booking startingBooking)
    {
        //UOW.Reload();
        Booking tmpBooking;
        if (startingBooking == null)
        {
            Booking.User = UOW.GenericRepository.GetById<User>(Booking.User.Id);
        }

        if (Payment.Amount > 0)
        {
            Booking.Payments.Add(new Payment { Amount = Payment.Amount, Comment = Payment.Comment, Date = DateTime.Now, PaymentMethod = Payment.PaymentMethod, User = UOW.GenericRepository.GetById<User>(Booking.User.Id) });
        }

        if (Booking.Id == 0)
        {
            UOW.GenericRepository.Create(Booking);
            tmpBooking = Booking;
        }
        else
        {
            // CalculateDiferences(startingBooking, Booking);
            tmpBooking = new Booking(Booking, UOW);

            UOW.GenericRepository.Create(tmpBooking);
            UOW.GenericRepository.Save();
            UOW.GenericRepository.Delete<Booking>(Booking.Id);
            Booking = tmpBooking;
        }
        UOW.Complete();

        Payment = new Payment();
        return tmpBooking;
    }

    internal void AddTransfer(Booking booking, bool all)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation { ReservationType = Reservation.ReservationTypeEnum.Transfer, FirstHotel = "TRANSFER" };
        foreach (Customer customer in booking.Customers)
        {
            if (customer.IsSelected || all)
            {
                hasCustomers = true;
                customer.Handled = true;
                customer.HotelName = "TRANSFER";
                customer.RoomTypeName = "TRANSFER";
                customer.RoomNumber = "TRNS" + (booking.ReservationsInBooking.Count + 1);
                customer.CustomerHasBusIndex = 0;
                if (booking.ReservationsInBooking.Count() % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!booking.DifferentDates)
                {
                    customer.CheckIn = booking.CheckIn;
                    customer.CheckOut = booking.CheckOut;
                }
                newRes.CustomersList.Add(customer);
            }
        }
        if (hasCustomers)
        {
            booking.ReservationsInBooking.Add(newRes);
            RoomsManager.SortCustomers(booking);
            foreach (Customer customer in booking.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    private void CalculateDiferences(Booking Dbbooking, Booking newBooking)
    {
        var tmpBooking = UOW.GenericRepository.GetById<Booking>(newBooking.Id);
        UOW.GenericRepository.UpdateValues(tmpBooking, newBooking);
        tmpBooking.Payments.Clear();
        tmpBooking.ReservationsInBooking.Clear();

        //UOW.ObjectContextAdapter.ObjectContext.ObjectStateManager.TryGetObjectStateEntry(tmpBooking, out ObjectStateEntry myObjectState);
        //IEnumerable<string> modifiedProperties = myObjectState.GetModifiedProperties();
        //foreach (var propName in modifiedProperties)
        //{
        //    Console.WriteLine("Property {0} changed from {1} to {2}",
        //         propName,
        //         myObjectState.OriginalValues[propName],
        //         myObjectState.CurrentValues[propName]);
        //}

        //var myObjectState = UOW myContext.ObjectStateManager.GetObjectStateEntry(myObject);
        //var modifiedProperties = myObjectState.GetModifiedProperties();
        //foreach (var propName in modifiedProperties)
        //{
        //    Console.WriteLine("Property {0} changed from {1} to {2}",
        //         propName,
        //         myObjectState.OriginalValues[propName],
        //         myObjectState.CurrentValues[propName]);
        //}
    }

    #endregion Methods
}
