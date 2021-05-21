using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;

namespace LATravelManager.UI.Helpers
{
public class NewReservationHelper : ViewModelBase
{
    #region Constructors

    public NewReservationHelper()
    {
    }

    #endregion Constructors

    #region Fields

    private readonly RoomsManager RoomsManager = new RoomsManager();

    #endregion Fields

    #region Methods

    public CustomerWrapper CreateRandomCustomer()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string name = new string(Enumerable.Repeat(chars, 5)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        string surename = new string(Enumerable.Repeat(chars, 5)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        string passport = new string(Enumerable.Repeat(chars, 8)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        return new CustomerWrapper() { Age = 12, Name = name, Surename = surename, Price = 35, Tel = "6981001676", StartingPlace = "Αθήνα", PassportNum = passport };
    }

    public void MakeNonameReservation(BookingWrapper booking, bool hb, bool all, bool onlystay)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = ReservationTypeEnum.Noname,
            FirstHotel = "ΝΟ ΝΑΜΕ",
            HB = hb,
            Booking = booking.Model,
            OnlyStay = onlystay
        };
        foreach (CustomerWrapper customer in booking.Customers)
        {
            if (customer.IsSelected || all)
            {
                if (customer.Handled)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                    continue;
                }
                hasCustomers = true;
                customer.Handled = true;
                customer.RoomNumber = "NN" + (booking.ReservationsInBooking.Count + 1).ToString();
                customer.HotelName = "NO NAME";
                if (booking.ReservationsInBooking.Count % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!booking.DifferentDates)
                {
                    customer.CheckIn = booking.CheckIn;
                    customer.CheckOut = booking.CheckOut;
                }
                newRes.CustomersList.Add(customer.Model);
            }
        }
        if (hasCustomers)
        {
            //check if can addNoName
            booking.ReservationsInBooking.Add(newRes);
            RoomsManager.SortCustomers(booking);
            foreach (CustomerWrapper customer in booking.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    internal void AddOneDay(BookingWrapper booking, bool all)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = ReservationTypeEnum.OneDay,
            Booking = booking.Model,
            FirstHotel = "ONEDAY"
        };
        foreach (CustomerWrapper customer in booking.Customers)
        {
            if (customer.IsSelected || all)
            {
                if (customer.Handled)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                    continue;
                }
                hasCustomers = true;
                customer.Handled = true;
                customer.HotelName = "ONEDAY";
                customer.RoomTypeName = "ONEDAY";
                customer.RoomNumber = "OD" + (booking.ReservationsInBooking.Count + 1);
                if (booking.ReservationsInBooking.Count % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!booking.DifferentDates)
                {
                    customer.CheckIn = booking.CheckIn;
                    customer.CheckOut = booking.CheckOut;
                }
                newRes.CustomersList.Add(customer.Model);
            }
        }
        if (hasCustomers)
        {
            booking.ReservationsInBooking.Add(newRes);
            RoomsManager.SortCustomers(booking);
            foreach (CustomerWrapper customer in booking.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    public void OverBookHotelAsync(BookingWrapper bookingWr, Hotel hotel, RoomType roomType, bool all, bool onlyStay, bool hb)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = ReservationTypeEnum.Overbooked,
            FirstHotel = hotel.Name,
            Hotel = hotel,
            Booking = bookingWr.Model,
            OnlyStay = onlyStay,
            HB = hb
        };
        foreach (CustomerWrapper customer in bookingWr.Customers)
        {
            if (customer.IsSelected || all)
            {
                if (customer.Handled)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                    continue;
                }
                hasCustomers = true;
                customer.Handled = true;
                customer.HotelName = newRes.Hotel.Name;
                customer.RoomNumber = "OB" + (bookingWr.ReservationsInBooking.Count + 1).ToString();
                if (bookingWr.ReservationsInBooking.Count % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!bookingWr.DifferentDates)
                {
                    customer.CheckIn = bookingWr.CheckIn;
                    customer.CheckOut = bookingWr.CheckOut;
                }
                newRes.CustomersList.Add(customer.Model);
            }
        }
        if (hasCustomers)
        {
            bookingWr.ReservationsInBooking.Add(newRes);
            RoomsManager.SortCustomers(bookingWr);
            foreach (CustomerWrapper customer in bookingWr.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    public void PutCustomersInRoomAsync(BookingWrapper booking, RoomWrapper SelectedRoom, bool all, bool onlyStay, bool hb)
    {
        bool hasCustomers = false;
        ReservationWrapper newResWr = new ReservationWrapper
        {
            ReservationType = ReservationTypeEnum.Normal,
            OnlyStay = onlyStay,
            Room = SelectedRoom.Model,
            FirstHotel = SelectedRoom.Hotel.Name,
            HB = hb,
            Booking = booking.Model
        };
        foreach (CustomerWrapper customer in booking.Customers)
        {
            if (customer.IsSelected || all)
            {
                if (customer.Handled)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                    continue;
                }
                hasCustomers = true;
                customer.Handled = true;
                customer.RoomNumber = (booking.ReservationsInBooking.Count + 1).ToString();
                customer.HotelName = newResWr.Room.Hotel.Name;
                customer.RoomTypeName = newResWr.Room.RoomType.Name;
                if (booking.ReservationsInBooking.Count % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!booking.DifferentDates)
                {
                    if (booking.ExcursionDate != null)
                    {
                        customer.CheckIn = booking.ExcursionDate.CheckIn;
                        customer.CheckOut = booking.ExcursionDate.CheckOut;
                    }
                    else
                    {
                        customer.CheckIn = booking.CheckIn;
                        customer.CheckOut = booking.CheckOut;
                    }
                }

                newResWr.CustomersList.Add(customer.Model);
            }
        }
        if (hasCustomers)
        {
            if (SelectedRoom.CanAddReservationToRoom(newResWr))
            {
                SelectedRoom.MakeReservation(newResWr);
            }
            booking.ReservationsInBooking.Add(newResWr.Model);

            RoomsManager.SortCustomers(booking);
            foreach (CustomerWrapper customer in booking.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    public async Task<BookingWrapper> SaveAsync(GenericRepository context, Payment Payment, BookingWrapper BookingWr)
    {
        if (Payment != null && BookingWr != null && Payment.Amount > 0)
        {
            BookingWr.Payments.Add(new Payment { Amount = Payment.Amount, Comment = Payment.Comment, Date = DateTime.Now, PaymentMethod = Payment.PaymentMethod, User = await context.GetByIdAsync<User>(StaticResources.User.Id), Checked = (Payment.PaymentMethod == PaymentMethod.Cash || Payment.PaymentMethod == PaymentMethod.Visa) ? (bool?)false : null });
        }
        if (BookingWr != null && BookingWr.IsPartners && BookingWr.Partner != null && !string.IsNullOrEmpty(BookingWr.PartnerEmail))
        {
            if (string.IsNullOrEmpty(BookingWr.Partner.Emails))
            {
                BookingWr.Partner.Emails += BookingWr.PartnerEmail;
            }
            else if (!BookingWr.Partner.Emails.Contains(BookingWr.PartnerEmail))
            {
                BookingWr.Partner.Emails += "," + BookingWr.PartnerEmail;
            }
        }
        if (BookingWr != null && BookingWr.Id == 0)
        {
            context.Add(BookingWr.Model);
        }

        await CaptureChanges(context, BookingWr);

        await context.SaveAsync();

        return BookingWr;
    }

    private async Task CaptureChanges(GenericRepository context, BookingWrapper bookingWr)
    {
        await context.CaptureChanges(bookingWr);
    }

    internal void AddTransfer(BookingWrapper booking, bool all)
    {
        bool hasCustomers = false;
        Reservation newRes = new Reservation
        {
            ReservationType = ReservationTypeEnum.Transfer,
            Booking = booking.Model,
            FirstHotel = "TRANSFER"
        };
        foreach (CustomerWrapper customer in booking.Customers)
        {
            if (customer.IsSelected || all)
            {
                if (customer.Handled)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message($"Ο {customer.Surename} {customer.Name} έχει μπεί ήδη σε άλλη κράτηση"));
                    continue;
                }
                hasCustomers = true;
                customer.Handled = true;
                customer.HotelName = "TRANSFER";
                customer.RoomTypeName = "TRANSFER";
                customer.RoomNumber = "TRNS" + (booking.ReservationsInBooking.Count + 1);
                if (booking.ReservationsInBooking.Count % 2 == 0)
                    customer.RoomColor = new SolidColorBrush(Colors.LightPink);
                else
                    customer.RoomColor = new SolidColorBrush(Colors.LightBlue);
                if (!booking.DifferentDates)
                {
                    customer.CheckIn = booking.CheckIn;
                    customer.CheckOut = booking.CheckOut;
                }
                newRes.CustomersList.Add(customer.Model);
            }
        }
        if (hasCustomers)
        {
            booking.ReservationsInBooking.Add(newRes);
            RoomsManager.SortCustomers(booking);
            foreach (CustomerWrapper customer in booking.Customers)
            {
                customer.IsSelected = false;
            }
        }
        else
        {
            MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
        }
    }

    //private async Task CalculateDiferencesAsync(Booking Dbbooking, Booking newBooking)
    //{
    //    //var tmpBooking = await Context.GetByIdAsync<Booking>(newBooking.Id);
    //Context.UpdateValues(tmpBooking, newBooking);
    //tmpBooking.Payments.Clear();
    //tmpBooking.ReservationsInBooking.Clear();

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
    //}

    #endregion Methods
}
}