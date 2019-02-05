using GalaSoft.MvvmLight;
using LATravelManager.Models;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

public class NewReservationHelper : ViewModelBase
{
    #region Fields

    private readonly RoomsManager RoomsManager = new RoomsManager();

    public NewReservationHelper(GenericRepository Context)
    {
        Context = Context;
    }

    public GenericRepository Context { get; }

    #endregion Fields

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

    public void MakeNonameReservation(BookingWrapper booking, RoomType roomType, bool hb, bool all, bool onlystay)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = Reservation.ReservationTypeEnum.Noname,
            FirstHotel = "ΝΟ ΝΑΜΕ",
            HB = hb,
            NoNameRoomType = roomType,
            OnlyStay = onlystay
        };
        foreach (Customer customer in booking.Customers)
        {
            if (customer.Handled)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                continue;
            }
            if (customer.IsSelected || all)
            {
                hasCustomers = true;
                customer.Handled = true;
                customer.RoomNumber = "NN" + (booking.ReservationsInBooking.Count + 1).ToString();
                customer.HotelName = "NO NAME";
                customer.RoomTypeName = roomType.Name;
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
            //check if can addNoName
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

    public async Task OverBookHotelAsync(BookingWrapper booking, Hotel hotel, RoomType roomType, bool all, bool onlyStay, bool hb)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = Reservation.ReservationTypeEnum.Overbooked,
            FirstHotel = hotel.Name,
            Hotel = hotel,
            NoNameRoomType = roomType,
            OnlyStay = onlyStay,
            HB = hb
        };
        foreach (Customer customer in booking.Customers)
        {
            if (customer.Handled)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                continue;
            }
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

    public void PutCustomersInRoomAsync(BookingWrapper booking, Room SelectedRoom, bool all, bool onlyStay, bool hb)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = Reservation.ReservationTypeEnum.Normal,
            OnlyStay = onlyStay,
            Room = SelectedRoom,
            FirstHotel = SelectedRoom.Hotel.Name,
            HB = hb
        };
        foreach (Customer customer in booking.Customers)
        {
            if (customer.Handled)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                continue;
            }
            if (customer.IsSelected || all)
            {
                hasCustomers = true;
                customer.Handled = true;
                customer.RoomNumber = (booking.ReservationsInBooking.Count + 1).ToString();
                customer.HotelName = newRes.Room.Hotel.Name;
                customer.RoomTypeName = newRes.Room.RoomType.Name;
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

    public async Task<BookingWrapper> SaveAsync(Payment Payment, BookingWrapper Booking, BookingWrapper startingBooking)
    {
        //TODO oti na nai exw kanei edw
        //UOW.Reload();
        BookingWrapper tmpBooking;
        if (startingBooking == null)
        {
            Booking.User = await Context.GetByIdAsync<User>(Booking.User.Id);
        }

        if (Payment.Amount > 0)
        {
            Booking.Payments.Add(new Payment { Amount = Payment.Amount, Comment = Payment.Comment, Date = DateTime.Now, PaymentMethod = Payment.PaymentMethod, User = await Context.GetByIdAsync<User>(Booking.User.Id) });
        }

        if (Booking.Id == 0)
        {
            Context.Add(Booking.Model);
            tmpBooking = Booking;
        }
        else
        {
            // CalculateDiferences(startingBooking, Booking);
            tmpBooking = new BookingWrapper(Booking.Model);

            Context.Add(tmpBooking.Model);
            await Context.SaveAsync();
            Context.RemoveById<Booking>(Booking.Id);
            Booking = tmpBooking;
        }
        await Context.SaveAsync();

        return tmpBooking;
    }

    internal void AddTransfer(BookingWrapper booking, bool all)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = Reservation.ReservationTypeEnum.Transfer,
            FirstHotel = "TRANSFER"
        };
        foreach (Customer customer in booking.Customers)
        {
            if (customer.Handled)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                continue;
            }
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

    private async Task CalculateDiferencesAsync(Booking Dbbooking, Booking newBooking)
    {
        var tmpBooking = await Context.GetByIdAsync<Booking>(newBooking.Id);
        Context.UpdateValues(tmpBooking, newBooking);
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