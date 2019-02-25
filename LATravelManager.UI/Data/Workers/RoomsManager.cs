using LATravelManager.Models;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.Data.Workers
{
    public class RoomsManager
    {
        #region Constructors

        public RoomsManager()
        {
            Hotels = new List<Hotel>();
        }

        #endregion Constructors

        #region Properties

        //public UnitOfWork UOW => ServiceLocator.Current.GetInstance<UnitOfWork>(UnitOfWorkKey);

        private List<Booking> Bookings { get; set; }

        private List<Hotel> Hotels { get; set; }

        private List<NoName> NonamesList { get; set; } = new List<NoName>();

        private List<Hotel> Plan { get; set; } = new List<Hotel>();

        #endregion Properties

        #region Methods

        //public async Task<List<Hotel>> BuildHotelsInCity(Expression<Func<Hotel, bool>> filter = null,
        //    Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderBy = null)
        //{
        //    // Hotels.AddRange(await UOW.GenericRepository.GetAsync(filter: filter, orderBy: orderBy));
        //    return Hotels;
        //}

        public bool CheckIfRoomIsFree(DateTime checkIn, DateTime checkout)
        {
            return true;
        }

        //public async Task<List<Hotel>> GetFreeRooms(DateTime checkIn, DateTime checkout, Excursion excursion)
        //{
        //    Hotels.Clear();
        //    int CityId = -1;
        //    for (int i = 0; i < excursion.Destinations.Count; i++)
        //    {
        //        CityId = excursion.Destinations[i].Id;
        //       // await BuildHotelsInCity(filter: x => x.City.Id == CityId);
        //    }

        //    Parallel.ForEach(Hotels, hotel =>
        //    {
        //        hotel.FreeRooms = hotel.ReservedRooms = hotel.AllotmentRooms = 0;
        //        Parallel.ForEach(hotel.Rooms, room =>
        //        {
        //            if (room.DailyBookingInfo.Count > 0 && room.DailyBookingInfo[0].Date <= checkIn && room.DailyBookingInfo[room.DailyBookingInfo.Count - 1].Date >= checkout)
        //            {
        //                room.DailyBookingInfo = room.DailyBookingInfo.OrderBy(o => o.Date).ToList();

        //                DateTime CurrentDayChecking = checkIn;
        //                bool isAllotment = false;
        //                for (int i = 0; i < room.DailyBookingInfo.Count; i++)
        //                {
        //                    if (room.DailyBookingInfo[i].Date == checkIn)
        //                    {
        //                        for (int j = i; j < room.DailyBookingInfo.Count; i++)
        //                        {
        //                            if (CurrentDayChecking == room.DailyBookingInfo[j].Date)
        //                            {
        //                                if (room.DailyBookingInfo[j].GetRoomState() == RoomStateEnum.Booked || room.DailyBookingInfo[j].GetRoomState() == RoomStateEnum.NotMovableNoName)
        //                                {
        //                                    hotel.ReservedRooms += 1;
        //                                    break;
        //                                }
        //                                else
        //                                {
        //                                    if (room.DailyBookingInfo[j].GetRoomState() == RoomStateEnum.Available || room.DailyBookingInfo[j].GetRoomState() == RoomStateEnum.MovaBleNoName)
        //                                    {
        //                                        hotel.FreeRooms += 1;
        //                                    }
        //                                    else if (room.DailyBookingInfo[j].GetRoomState() == RoomStateEnum.Allotment)
        //                                    {
        //                                        isAllotment = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        ///big error
        //                                    }
        //                                }
        //                                if (CurrentDayChecking == checkout)
        //                                {
        //                                    if (isAllotment)
        //                                    {
        //                                        hotel.AllotmentRooms += 1;
        //                                    }
        //                                    else
        //                                    {
        //                                        hotel.FreeRooms += 1;
        //                                    }
        //                                    // room.IsAllotment = isAllotment;
        //                                    hotel.AvailableRooms.Add(room);
        //                                    break;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                break;
        //                            }
        //                            CurrentDayChecking = CurrentDayChecking.AddDays(1);
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //        });
        //    });

        //    return Hotels;
        //}

        internal void SortCustomers(BookingWrapper booking)
        {
            booking.Customers.OrderBy(c => c.Reservation);
            //foreach (CustomerWrapper customer in booking.Customers)
            //{
            //    if (!customer.Handled)
            //    {
            //        booking.Customers.Insert(0, customer);
            //    }
            //}
            //foreach (Reservation reservation in booking.ReservationsInBooking)
            //{
            //    foreach (Customer customer in reservation.CustomersList)
            //    {
            //        booking.Customers.Add(new CustomerWrapper(customer));
            //    }
            //}
        }

        public async Task<List<Hotel>> GetAllAvailableRooms(
            GenericRepository context, DateTime planStart, DateTime planEnd,
            ExcursionWrapper excursionfilter,
             Booking unSavedBooking = null)
        {
            try
            {
                Plan.Clear();
                NonamesList.Clear();
                DateTime MinDay = planStart, MaxDay = planEnd, tmpDate;
                MinDay = MinDay.AddDays(-10);
                MaxDay = MaxDay.AddDays(10);
                Hotels = (await context.GetAllHotelsWithRoomsInCityAsync(MinDay, MaxDay,excursionfilter.Destinations[0].Id));
                Bookings = (await context.GetAllBookingInPeriod(MinDay, MaxDay, excursionfilter.Id)).ToList();

                if (unSavedBooking.Id > 0)
                {
                    Bookings = Bookings.Where(x => x.Id != unSavedBooking.Id).ToList();
                }

                Bookings.Add(unSavedBooking);
                foreach (Booking booking in Bookings)
                {
                    foreach (Reservation reservation in booking.ReservationsInBooking)
                    {
                        if (reservation.ReservationType == Reservation.ReservationTypeEnum.Noname)
                        {
                            NonamesList.Add(new NoName { Reservation = reservation });
                        }

                        if (booking.CheckIn < MinDay)
                        {
                            MinDay = booking.CheckIn;
                        }
                        if (booking.CheckOut > MaxDay)
                        {
                            MaxDay = booking.CheckOut;
                        }
                    }
                }

                Hotel tmpHotel;//-------------
                Room tmpRoom;//-------------
                int counter = 0;//-------------
                                //maybe parallel here

                foreach (Hotel hotel in Hotels)
                {
                    tmpHotel = new Hotel//-------------
                    {
                        Name = hotel.Name,
                        Id = hotel.Id,
                        City = hotel.City,
                        HotelCategory = hotel.HotelCategory,
                        Tel = hotel.Tel
                    };
                    foreach (Room room in hotel.Rooms)
                    {
                        room.DailyBookingInfo.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
                        tmpDate = MinDay;
                        tmpRoom = room; //new Room { Id = room.Id, RoomType = room.RoomType, Hotel = room.Hotel, Note = room.Note, };

                        if (room.StartDate > MaxDay || room.EndDate < MinDay)
                        {
                            continue;
                        }
                        while (room.DailyBookingInfo[counter].Date < MinDay)
                        {
                            counter++;
                        }

                        while (tmpDate < MaxDay)
                        {
                            if (tmpRoom.Id == 1472)
                            {
                            }
                            if (counter < room.DailyBookingInfo.Count && room.DailyBookingInfo[counter].Date == tmpDate)
                            {
                                tmpRoom.PlanDailyInfo.Add(new PlanDailyInfo
                                {
                                    Date = room.DailyBookingInfo[counter].Date,
                                    IsAllotment = room.DailyBookingInfo[counter].IsAllotment,
                                    // Reservation = room.DailyBookingInfo[d].Reservation,
                                    RoomState = room.DailyBookingInfo[counter].IsAllotment ? RoomStateEnum.Allotment : RoomStateEnum.Available
                                });
                                counter++;
                            }
                            else
                            {
                                tmpRoom.PlanDailyInfo.Add(new PlanDailyInfo { Date = tmpDate, RoomState = RoomStateEnum.NotAvailable });
                            }
                            tmpDate = tmpDate.AddDays(1);
                        }
                        counter = 0;
                        if (tmpRoom.PlanDailyInfo.Count > 0)
                            tmpHotel.Rooms.Add(tmpRoom);
                    }
                    if (tmpHotel.Rooms.Count > 0)
                    {
                        Plan.Add(tmpHotel);
                    }
                }

                try
                {
                    foreach (Booking booking in Bookings)
                    {
                        if (booking.Id == unSavedBooking.Id)
                        {
                            foreach (Reservation reservation in booking.ReservationsInBooking)
                            {
                                if (reservation.ReservationType == Reservation.ReservationTypeEnum.Normal)
                                {
                                    (await context.GetByIdAsync<Room>(reservation.Room.Id)).MakeReservation(reservation);
                                }
                            }
                        }
                        else
                        {
                            foreach (Reservation reservation in booking.ReservationsInBooking)
                            {
                                if (reservation.ReservationType == Reservation.ReservationTypeEnum.Normal)
                                {
                                    reservation.Room.MakeReservation(reservation);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                UpdateNoNames();

                #region nonames

                NonamesList = NonamesList.OrderBy(o => o.Reservation.Nights).ToList();
                bool changed = false;
                do
                {
                    //step 1 - put only choice
                    changed = false;
                    //maybe can be done in paralel
                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled && noName.AvailableRooms.Count == 1)
                        {
                            noName.AvailableRooms[0].MakeNoNameReservation(noName.Reservation);
                            noName.Handled = true;
                            changed = true;
                            goto end;
                        }
                    }
                end:
                    if (changed)
                    {
                        UpdateNoNames();
                    }
                } while (changed);

                do
                {
                    //step 2 - put what fits
                    changed = false;
                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled && noName.AvailableRooms.Count == 1)
                        {
                            noName.AvailableRooms[0].MakeNoNameReservation(noName.Reservation);
                            noName.Handled = true;
                            changed = true;
                            goto end;
                        }
                    }
                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled)
                        {
                            foreach (Room availableroom in noName.AvailableRooms)
                            {
                                if (Fits(noName, availableroom))
                                {
                                    availableroom.MakeNoNameReservation(noName.Reservation);
                                    noName.Handled = true;
                                    changed = true;
                                    goto end;
                                }
                            }
                        }
                    }

                end:
                    if (changed)
                    {
                        UpdateNoNames();
                    }
                } while (changed);

                do
                {
                    //step 3 - put those who fit onse side
                    changed = false;
                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled && noName.AvailableRooms.Count == 1)
                        {
                            noName.AvailableRooms[0].MakeNoNameReservation(noName.Reservation);
                            noName.Handled = true;
                            changed = true;
                            goto end;
                        }
                    }

                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled)
                        {
                            foreach (Room availableroom in noName.AvailableRooms)
                            {
                                if (Fits(noName, availableroom))
                                {
                                    availableroom.MakeNoNameReservation(noName.Reservation);
                                    noName.Handled = true;
                                    changed = true;
                                    goto end;
                                }
                            }
                        }
                    }

                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled)
                        {
                            foreach (Room availableroom in noName.AvailableRooms)
                            {
                                if (FitsBefore(noName, availableroom))
                                {
                                    availableroom.MakeNoNameReservation(noName.Reservation);
                                    noName.Handled = true;
                                    changed = true;
                                    goto end;
                                }
                            }
                        }
                    }

                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled)
                        {
                            foreach (Room availableroom in noName.AvailableRooms)
                            {
                                if (FitsAfter(noName, availableroom))
                                {
                                    availableroom.MakeNoNameReservation(noName.Reservation);
                                    noName.Handled = true;
                                    changed = true;
                                    goto end;
                                }
                            }
                        }
                    }

                end:
                    if (changed)
                    {
                        UpdateNoNames();
                    }
                } while (changed);

                do
                {
                    //step 3 - put everything else
                    changed = false;
                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled && noName.AvailableRooms.Count == 1)
                        {
                            noName.AvailableRooms[0].MakeNoNameReservation(noName.Reservation);
                            noName.Handled = true;
                            changed = true;
                            goto end;
                        }
                    }

                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled)
                        {
                            foreach (Room availableroom in noName.AvailableRooms)
                            {
                                if (Fits(noName, availableroom))
                                {
                                    availableroom.MakeNoNameReservation(noName.Reservation);
                                    noName.Handled = true;
                                    changed = true;
                                    goto end;
                                }
                            }
                        }
                    }

                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled)
                        {
                            foreach (Room availableroom in noName.AvailableRooms)
                            {
                                if (FitsBefore(noName, availableroom))
                                {
                                    availableroom.MakeNoNameReservation(noName.Reservation);
                                    noName.Handled = true;
                                    changed = true;
                                    goto end;
                                }
                            }
                        }
                    }

                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled)
                        {
                            foreach (Room availableroom in noName.AvailableRooms)
                            {
                                if (FitsAfter(noName, availableroom))
                                {
                                    availableroom.MakeNoNameReservation(noName.Reservation);
                                    noName.Handled = true;
                                    changed = true;
                                    goto end;
                                }
                            }
                        }
                    }

                    foreach (NoName noName in NonamesList)
                    {
                        if (!noName.Handled)
                        {
                            if (noName.AvailableRooms.Count > 0)
                            {
                                noName.AvailableRooms[0].MakeNoNameReservation(noName.Reservation);
                                noName.Handled = true;
                                changed = true;
                                goto end;
                            }
                        }
                    }

                end:
                    if (changed)
                    {
                        UpdateNoNames();
                    }
                } while (changed);

                foreach (NoName noName in NonamesList)
                {
                    if (!noName.Handled)
                    {
                        MessageBox.Show("Η κράτηση " + noName.Reservation.CustomersList[0] + " " + ((noName.Reservation.Room != null) ? noName.Reservation.Room.RoomType.ToString() : "") + " " + noName.Reservation.Dates + " δεν έχει δωμάτιο.", "Λάθος");
                    }
                }

                #endregion nonames

                SetNoNameColors(Plan);

                List<Hotel> PlanCroped = new List<Hotel>();

                foreach (Hotel hotel in Plan)
                {
                    bool addThis = false;
                    tmpHotel = new Hotel { Name = hotel.Name, Id = hotel.Id };
                    foreach (Room room in hotel.Rooms)
                    {
                        addThis = false;
                        tmpDate = planStart;
                        tmpRoom = room;// new Room { Id = room.Id, RoomType = room.RoomType, Hotel = room.Hotel, Note = room.Note };
                        List<PlanDailyInfo> newList = new List<PlanDailyInfo>();
                        while (room.PlanDailyInfo[counter].Date < planStart)
                        {
                            counter++;
                        }
                        while (tmpDate < planEnd)
                        {
                            if (counter < room.PlanDailyInfo.Count)
                            {
                                if (!addThis && room.PlanDailyInfo[counter].RoomState != RoomStateEnum.NotAvailable)
                                {
                                    addThis = true;
                                }
                                if (room.PlanDailyInfo[counter].Date == tmpDate)
                                {
                                    newList.Add(room.PlanDailyInfo[counter]);
                                    counter++;
                                }
                                else
                                {
                                    newList.Add(new PlanDailyInfo { Date = tmpDate });
                                }
                            }
                            else
                            {
                                newList.Add(new PlanDailyInfo { Date = tmpDate });
                            }

                            tmpDate = tmpDate.AddDays(1);
                        }
                        counter = 0;
                        if (addThis)
                        {
                            tmpRoom.PlanDailyInfo = newList;
                            tmpHotel.Rooms.Add(tmpRoom);
                        }
                    }
                    if (tmpHotel.Rooms.Count > 0)
                    {
                        PlanCroped.Add(tmpHotel);
                    }
                }
                Plan = PlanCroped;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Plan;
        }

        private static bool IsOtherFree(Reservation reservation, List<Hotel> plan)
        {
            foreach (Hotel hotel in plan)
                if (hotel.Id != 21 && hotel.Id != 22 && hotel.Id != 23 && hotel.Id != 42)
                    foreach (Room room in hotel.Rooms)
                        if (room.RoomType == reservation.NoNameRoom.RoomType)
                            if (room.CanAddReservationToRoom(reservation, false, false))
                                return true;
            return false;
        }

        private static void SetNoNameColors(List<Hotel> plan)
        {
            DateTime tmpdate;
            foreach (Hotel hotel in plan)
            {
                foreach (Room room in hotel.Rooms)
                {
                    for (var i = 0; i < room.PlanDailyInfo.Count; i++)
                    {
                        PlanDailyInfo day = room.PlanDailyInfo[i];
                        if (day.DayState == DayStateEnum.FirstDay && day.RoomState == RoomStateEnum.MovaBleNoName)
                        {
                            if (!IsOtherFree(day.Reservation, plan))
                            {
                                tmpdate = day.Reservation.CheckIn;
                                while (tmpdate < day.Reservation.CheckOut)
                                {
                                    room.PlanDailyInfo[i].RoomState = RoomStateEnum.NotMovableNoName;
                                    tmpdate = tmpdate.AddDays(1);
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool Fits(NoName noName, Room availableroom)
        {
            DateTime tmpdate = noName.Reservation.CheckIn;
            int lastNight;
            for (int i = 0; i < availableroom.PlanDailyInfo.Count; i++)
            {
                if (availableroom.PlanDailyInfo[i].Date > noName.Reservation.CheckIn)
                {
                    return false;
                }
                if (availableroom.PlanDailyInfo[i].Date == noName.Reservation.CheckIn)
                {
                    if (i == 0)
                    {
                        lastNight = i + noName.Reservation.Nights - 1;
                        if (lastNight == availableroom.PlanDailyInfo.Count)
                        {
                            return true;
                        }
                        if (availableroom.PlanDailyInfo[lastNight + 1].RoomState != RoomStateEnum.Available)
                        {
                            return true;
                        }
                    }
                    else if (availableroom.PlanDailyInfo[i - 1].RoomState != RoomStateEnum.Available)
                    {
                        lastNight = i + noName.Reservation.Nights - 1;
                        if (lastNight == availableroom.PlanDailyInfo.Count)
                        {
                            return true;
                        }
                        if (availableroom.PlanDailyInfo[lastNight + 1].RoomState != RoomStateEnum.Available)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool FitsAfter(NoName noName, Room availableroom)
        {
            DateTime tmpdate = noName.Reservation.CheckIn;
            int lastNightI;
            for (int i = 0; i < availableroom.PlanDailyInfo.Count; i++)
            {
                if (availableroom.PlanDailyInfo[i].Date > noName.Reservation.CheckIn)
                {
                    return false;
                }

                if (availableroom.PlanDailyInfo[i].Date == noName.Reservation.CheckIn)
                {
                    if (i == 0 || availableroom.PlanDailyInfo[i - 1].RoomState != RoomStateEnum.Available)
                    {
                        return true;
                    }

                    lastNightI = i + noName.Reservation.Nights - 1;
                    if (lastNightI == availableroom.PlanDailyInfo.Count)
                    {
                        return true;
                    }

                    if (availableroom.PlanDailyInfo[lastNightI + 1].RoomState != RoomStateEnum.Available)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FitsBefore(NoName noName, Room availableroom)
        {
            DateTime tmpdate = noName.Reservation.CheckIn;
            for (int i = 0; i < availableroom.PlanDailyInfo.Count; i++)
            {
                if (availableroom.PlanDailyInfo[i].Date > noName.Reservation.CheckIn)
                {
                    return false;
                }

                if (availableroom.PlanDailyInfo[i].Date == noName.Reservation.CheckIn)
                {
                    if (i == 0 || availableroom.PlanDailyInfo[i - 1].RoomState != RoomStateEnum.Available)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void UpdateNoNames()
        {
            Object lockMe = new Object();
            Parallel.ForEach(NonamesList, noName =>
            {
                noName.AvailableRooms.Clear();
                Parallel.ForEach(Plan, hotel =>
                  {
                      Parallel.ForEach(hotel.Rooms, room =>
                      {
                          if (room.CanFit(noName.Reservation) && noName.AvailableRooms.Count < NonamesList.Count + 10)
                              if (room.CanAddReservationToRoom(noName.Reservation))
                              {
                                  lock (lockMe)
                                  {
                                      noName.AvailableRooms.Add(room);
                                  }
                              }
                      });
                  });
            });
        }

        #endregion Methods
    }
}