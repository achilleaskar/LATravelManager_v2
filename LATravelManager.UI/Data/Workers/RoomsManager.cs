using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.Plan;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LATravelManager.UI.Data.Workers
{
    public class RoomsManager
    {
        #region Constructors

        public RoomsManager(GenericRepository genericRepository = null)
        {
            Hotels = new List<Hotel>();
            if (genericRepository != null)
            {
                GenericRepository = genericRepository;
            }
            else
            {
                GenericRepository = new GenericRepository();
            }
        }

        #endregion Constructors

        #region Properties

        //public UnitOfWork UOW => ServiceLocator.Current.GetInstance<UnitOfWork>(UnitOfWorkKey);

        private List<Booking> Bookings { get; set; }

        private List<Hotel> Hotels { get; set; }

        private List<NoName> NonamesList { get; set; } = new List<NoName>();

        private List<HotelWrapper> Plan { get; set; } = new List<HotelWrapper>();
        public GenericRepository GenericRepository { get; }

        #endregion Properties

        #region Methods

        //public async Task<List<Hotel>> BuildHotelsInCity(Expression<Func<Hotel, bool>> filter = null,
        //    Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderBy = null)
        //{
        //    // Hotels.AddRange(await UOW.GenericRepository.GetAsync(filter: filter, orderBy: orderBy));
        //    return Hotels;
        //}

        //public bool CheckIfRoomIsFree(DateTime checkIn, DateTime checkout)
        //{
        //    return true;
        //}

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

        public async Task<List<HotelWrapper>> GetAllAvailableRooms(DateTime planStart, DateTime planEnd,
            ExcursionWrapper excursionfilter, bool cropped, RoomType selectedRoomType = null, Hotel selectedHotel = null, int PeopleCount = 0, Booking unSavedBooking = null)
        {
            try
            {
                Plan.Clear();
                NonamesList.Clear();
                Hotels = new List<Hotel>();
                DateTime MinDay = planStart, MaxDay = planEnd, tmpDate;
                MinDay = MinDay.AddDays(-10);
                MaxDay = MaxDay.AddDays(10);
                //mhpws edw na epairna dwmatia?

                Bookings = (await GenericRepository.GetAllBookingInPeriod(MinDay, MaxDay, excursionfilter.Destinations[0])).ToList();

                if (unSavedBooking != null)
                {
                    if (unSavedBooking.Id > 0)
                    {
                        Bookings = Bookings.Where(x => x.Id != unSavedBooking.Id).ToList();
                    }

                    Bookings.Add(unSavedBooking);
                }
                foreach (Booking booking in Bookings)
                {
                    foreach (Reservation reservation in booking.ReservationsInBooking)
                    {
                        if (reservation.ReservationType == ReservationTypeEnum.Noname)
                        {
                            NonamesList.Add(new NoName { Reservation = new ReservationWrapper(reservation) });
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

                List<BookingInfoPerDay> bi = new List<BookingInfoPerDay>();

                List<Room> rooms = (await GenericRepository.GetAllRoomsInCityAsync(MinDay, MaxDay, excursionfilter.Destinations[0].Id));
                foreach (var r in rooms)
                {
                    if (r.RoomType.freeRooms > 0)
                    {
                        r.RoomType.freeRooms = 0;
                    }
                    if (!Hotels.Contains(r.Hotel))
                    {
                        Hotels.Add(r.Hotel);
                    }
                }

                Hotels = Hotels.OrderBy(h => h.Name).ToList();

                HotelWrapper tmpHotelWr;//-------------
                RoomWrapper tmpRoomWr;//-------------
                int counter = 0;//-------------
                                //maybe parallel here

                foreach (Hotel hotel in Hotels)
                {
                    tmpHotelWr = new HotelWrapper//-------------
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
                        tmpRoomWr = new RoomWrapper(room); //new Room { Id = room.Id, RoomType = room.RoomType, Hotel = room.Hotel, Note = room.Note, };

                        if (tmpRoomWr.StartDate > MaxDay || tmpRoomWr.EndDate < MinDay)
                        {
                            continue;
                        }
                        //edw tha mporuse na prosthetei adeia
                        while (tmpRoomWr.DailyBookingInfo[counter].Date < MinDay)
                        {
                            counter++;
                        }

                        while (tmpDate < MaxDay)
                        {
                            if (counter < tmpRoomWr.DailyBookingInfo.Count && tmpRoomWr.DailyBookingInfo[counter].Date == tmpDate)
                            {
                                tmpRoomWr.PlanDailyInfo.Add(new PlanDailyInfo
                                {
                                    Date = tmpRoomWr.DailyBookingInfo[counter].Date,
                                    RoomTypeEnm = tmpRoomWr.DailyBookingInfo[counter].RoomTypeEnm,
                                    RoomState = tmpRoomWr.DailyBookingInfo[counter].RoomTypeEnm == RoomTypeEnum.Allotment ? RoomStateEnum.Allotment : tmpRoomWr.DailyBookingInfo[counter].RoomTypeEnm
                                    == RoomTypeEnum.Booking ? RoomStateEnum.Booking : RoomStateEnum.Available,
                                    MinimumStay = tmpRoomWr.DailyBookingInfo[counter].MinimunStay,
                                    Room = tmpRoomWr
                                });
                                counter++;
                            }
                            else
                            {
                                tmpRoomWr.PlanDailyInfo.Add(new PlanDailyInfo
                                {
                                    Date = tmpDate,
                                    Room = tmpRoomWr,
                                    RoomState = RoomStateEnum.NotAvailable,
                                    CellColor = new SolidColorBrush(System.Windows.Media.Colors.DarkGray)
                                });
                            }
                            tmpDate = tmpDate.AddDays(1);
                        }
                        counter = 0;
                        if (tmpRoomWr.PlanDailyInfo.Count > 0)
                            tmpHotelWr.RoomWrappers.Add(tmpRoomWr);
                    }
                    if (tmpHotelWr.RoomWrappers.Count > 0)
                    {
                        Plan.Add(tmpHotelWr);
                    }
                }

                try
                {
                    foreach (Booking booking in Bookings)
                    {
                        //  if (unSavedBooking != null && booking.Id == unSavedBooking.Id)
                        //  {
                        //      foreach (Reservation reservation in booking.ReservationsInBooking)
                        //      {
                        //          if (reservation.ReservationType == ReservationTypeEnum.Normal)
                        //          {
                        //              //isws n kanei eks arxhs reservation
                        //              (new RoomWrapper(await context.GetByIdAsync<Room>(reservation.Room.Id))).MakeReservation(new ReservationWrapper(reservation));
                        //          }
                        //      }
                        //  }
                        ////  else
                        //{
                        HotelWrapper hw;
                        RoomWrapper rw;
                        foreach (Reservation reservation in booking.ReservationsInBooking)
                        {
                            if (reservation.ReservationType == ReservationTypeEnum.Normal)
                            {
                                if (reservation.Room != null)
                                {
                                    hw = Plan.FirstOrDefault(h => h.Id == reservation.Room.Hotel.Id);
                                    rw = hw.RoomWrappers.FirstOrDefault(r => r.Id == reservation.Room.Id);
                                    rw.MakeReservation(new ReservationWrapper(reservation));
                                }
                                else
                                {
                                    MessageBox.Show($"Πρόβλημα με την κρατηση {reservation.Id}");
                                }
                            }
                        }
                        //}
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
                            foreach (RoomWrapper availableroom in noName.AvailableRooms)
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
                            foreach (RoomWrapper availableroom in noName.AvailableRooms)
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
                            foreach (RoomWrapper availableroom in noName.AvailableRooms)
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
                            foreach (RoomWrapper availableroom in noName.AvailableRooms)
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
                            foreach (RoomWrapper availableroom in noName.AvailableRooms)
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
                            foreach (RoomWrapper availableroom in noName.AvailableRooms)
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
                            foreach (RoomWrapper availableroom in noName.AvailableRooms)
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
                    if (!noName.Handled && noName.Reservation.CheckIn > planStart && noName.Reservation.CheckIn < planEnd)
                    {
                        MessageBox.Show("Η κράτηση " + noName.Reservation.CustomersList[0] + " " + ((noName.Reservation.Room != null) ? noName.Reservation.Room.RoomType.ToString() : "") + " " + noName.Reservation.Dates + " δεν έχει δωμάτιο.", "Λάθος");
                    }
                }

                #endregion nonames

                SetNoNameColors(Plan);

                if (cropped)
                {
                    List<HotelWrapper> PlanCroped = new List<HotelWrapper>();

                    // await GetFreeRooms(Plan);

                    foreach (HotelWrapper hotel in Plan)
                    {
                        if (selectedHotel == null || selectedHotel.Id == hotel.Id)
                        {
                            bool addThis = false;
                            tmpHotelWr = new HotelWrapper { Name = hotel.Name, Id = hotel.Id };
                            foreach (RoomWrapper roomWr in hotel.RoomWrappers)
                            {
                                if ((selectedRoomType == null || roomWr.RoomType.Id == selectedRoomType.Id) && (PeopleCount == 0 || PeopleCount >= roomWr.RoomType.MinCapacity && PeopleCount <= roomWr.RoomType.MaxCapacity))
                                {
                                    addThis = false;
                                    tmpDate = planStart;
                                    tmpRoomWr = roomWr;// new Room { Id = room.Id, RoomType = room.RoomType, Hotel = room.Hotel, Note = room.Note };
                                    List<PlanDailyInfo> newList = new List<PlanDailyInfo>();
                                    while (roomWr.PlanDailyInfo[counter].Date < planStart)
                                    {
                                        counter++;
                                    }
                                    while (tmpDate < planEnd)
                                    {
                                        if (counter < roomWr.PlanDailyInfo.Count)
                                        {
                                            if (!addThis && roomWr.PlanDailyInfo[counter].RoomState != RoomStateEnum.NotAvailable)
                                            {
                                                addThis = true;
                                            }
                                            if (roomWr.PlanDailyInfo[counter].Date == tmpDate)
                                            {
                                                newList.Add(roomWr.PlanDailyInfo[counter]);
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
                                        tmpRoomWr.PlanDailyInfo = newList;
                                        tmpHotelWr.RoomWrappers.Add(tmpRoomWr);
                                    }
                                }
                            }
                            if (tmpHotelWr.RoomWrappers.Count > 0)
                            {
                                PlanCroped.Add(tmpHotelWr);
                            }
                        }
                    }

                    Plan = PlanCroped;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Plan.OrderBy(h => h.Name).ToList(); ;
        }

        private static Cell AddCellWithText(string text)
        {
            Cell c1 = new Cell
            {
                DataType = CellValues.InlineString
            };

            InlineString inlineString = new InlineString();
            Text t = new Text
            {
                Text = text
            };
            inlineString.AppendChild(t);

            c1.AppendChild(inlineString);

            return c1;
        }

        private async Task GetFreeRooms(List<HotelWrapper> plan)
        {
            int counter = 0;
            int size = plan[0].RoomWrappers[0].PlanDailyInfo.Count;
            RoomTypeFrees selected;
            List<RoomTypeFrees> RoomTypes = new List<RoomTypeFrees>();
            foreach (RoomType roomtype in await GenericRepository.GetAllAsync<RoomType>())
            {
                RoomTypes.Add(new RoomTypeFrees { RoomType = roomtype, Quantity = new List<int>(new int[size]) });
            }
            foreach (HotelWrapper hotel in plan)
            {
                foreach (RoomWrapper room in hotel.RoomWrappers)
                {
                    counter = 0;
                    selected = RoomTypes.Where(r => r.RoomType.Id == room.RoomType.Id).FirstOrDefault();
                    foreach (PlanDailyInfo day in room.PlanDailyInfo)
                    {
                        if (day.RoomState == RoomStateEnum.Available)
                        {
                            selected.Quantity[counter]++;
                        }
                        counter++;
                    }
                }
            }

            using (SpreadsheetDocument spreedDoc = SpreadsheetDocument.Create(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Κενα.xlsx",
                DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart wbPart = spreedDoc.WorkbookPart;
                if (wbPart == null)
                {
                    wbPart = spreedDoc.AddWorkbookPart();
                    wbPart.Workbook = new Workbook();
                }

                string sheetName = "Kena";
                WorksheetPart worksheetPart = null;
                worksheetPart = wbPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();

                worksheetPart.Worksheet = new Worksheet(sheetData);

                if (wbPart.Workbook.Sheets == null)
                {
                    wbPart.Workbook.AppendChild(new Sheets());
                }

                var sheet = new Sheet()
                {
                    Id = wbPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = sheetName
                };

                var workingSheet = ((WorksheetPart)wbPart.GetPartById(sheet.Id)).Worksheet;

                int rowindex = 1;
                Row row = new Row
                {
                    RowIndex = (uint)rowindex
                };
                row.AppendChild(AddCellWithText("   "));
                foreach (PlanDailyInfo day in plan[0].RoomWrappers[0].PlanDailyInfo)
                {
                    row.AppendChild(AddCellWithText(day.Date.ToString("dd/MM")));
                }
                sheetData.AppendChild(row);
                rowindex++;
                row.AppendChild(AddCellWithText("   "));
                foreach (PlanDailyInfo day in plan[0].RoomWrappers[0].PlanDailyInfo)
                {
                    row.AppendChild(AddCellWithText(day.Date.ToString("ddd")));
                }
                sheetData.AppendChild(row);
                sheetData.AppendChild(row);
                rowindex++;

                foreach (RoomTypeFrees roomtype in RoomTypes)
                {
                    row = new Row
                    {
                        RowIndex = (uint)rowindex
                    };
                    row.AppendChild(AddCellWithText(roomtype.RoomType.Name));
                    foreach (int item in roomtype.Quantity)
                    {
                        row.AppendChild(AddCellWithText(item.ToString()));
                    }
                    sheetData.AppendChild(row);
                    rowindex++;
                }
                wbPart.Workbook.Sheets.AppendChild(sheet);

                //Set Border
                //wbPark

                wbPart.Workbook.Save();
            }
        }

        private static bool IsOtherFree(ReservationWrapper reservation, List<HotelWrapper> plan)
        {
            foreach (HotelWrapper hotel in plan)
                // if (hotel.Id != 21 && hotel.Id != 22 && hotel.Id != 23 && hotel.Id != 42)
                foreach (RoomWrapper roomWr in hotel.RoomWrappers)
                    if (roomWr.CanFit(reservation))
                        if (roomWr.CanAddReservationToRoom(reservation, false, false))
                            return true;
            return false;
        }

        private static void SetNoNameColors(List<HotelWrapper> plan)
        {
            DateTime tmpdate;
            foreach (HotelWrapper hotel in plan)
            {
                foreach (RoomWrapper room in hotel.RoomWrappers)
                {
                    for (int i = 0; i < room.PlanDailyInfo.Count; i++)
                    {
                        PlanDailyInfo day = room.PlanDailyInfo[i];
                        if (day.DayState == DayStateEnum.FirstDay && day.RoomState == RoomStateEnum.MovableNoName)
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
                                if (tmpdate == day.Reservation.CheckOut && i > 0)
                                    i--;
                            }
                        }
                    }
                }
            }
        }

        private static bool Fits(NoName noName, RoomWrapper availableroomWr)
        {
            try
            {
                int lastNight;
                //int ik = 0;
                //foreach (PlanDailyInfo planDailyInfo in availableroomWr.PlanDailyInfo)
                //{
                //    if (planDailyInfo.Date > noName.Reservation.CheckIn)
                //    {
                //        return false;
                //    }
                //    if (planDailyInfo.Date == noName.Reservation.CheckIn)
                //    {
                //        if (ik == 0)
                //        {
                //            lastNight = noName.Reservation.Nights - 1;
                //            if (lastNight == availableroomWr.PlanDailyInfo.Count - 1)
                //            {
                //                return true;
                //            }
                //            if (availableroomWr.PlanDailyInfo[lastNight + 1].RoomState != RoomStateEnum.Available)
                //            {
                //                return true;
                //            }
                //        }
                //        else if (planDailyInfo.RoomState != RoomStateEnum.Available)
                //        {
                //            lastNight = ik + noName.Reservation.Nights - 1;
                //            if (lastNight == availableroomWr.PlanDailyInfo.Count - 1)
                //            {
                //                return true;
                //            }
                //            if (availableroomWr.PlanDailyInfo[lastNight + 1].RoomState != RoomStateEnum.Available)
                //            {
                //                return true;
                //            }
                //        }
                //    }
                //    ik++;
                //}
                for (int i = 0; i < availableroomWr.PlanDailyInfo.Count; i++)
                {
                    if (availableroomWr.PlanDailyInfo[i].Date > noName.Reservation.CheckIn)
                    {
                        return false;
                    }
                    if (availableroomWr.PlanDailyInfo[i].Date == noName.Reservation.CheckIn)
                    {
                        if (i == 0)
                        {
                            lastNight = noName.Reservation.Nights - 1;
                            if (lastNight == availableroomWr.PlanDailyInfo.Count - 1)
                            {
                                return true;
                            }
                            if (availableroomWr.PlanDailyInfo[lastNight + 1].RoomState != RoomStateEnum.Available)
                            {
                                return true;
                            }
                        }
                        else if (availableroomWr.PlanDailyInfo[i - 1].RoomState != RoomStateEnum.Available)
                        {
                            lastNight = i + noName.Reservation.Nights - 1;
                            if (lastNight == availableroomWr.PlanDailyInfo.Count - 1)
                            {
                                return true;
                            }
                            if (availableroomWr.PlanDailyInfo[lastNight + 1].RoomState != RoomStateEnum.Available)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool FitsAfter(NoName noName, RoomWrapper availableroom)
        {
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

                    if (lastNightI + 1 < availableroom.PlanDailyInfo.Count && availableroom.PlanDailyInfo[lastNightI + 1].RoomState != RoomStateEnum.Available)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FitsBefore(NoName noName, RoomWrapper availableroom)
        {
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
            NonamesList = NonamesList.Where(nn => !nn.Handled).ToList();
            object lockMe = new object();
            Parallel.ForEach(NonamesList, noName =>
            {
                noName.AvailableRooms.Clear();
                Parallel.ForEach(Plan.Where(y => y.HotelCategory.Id == 8), hotel =>
                      {
                          Parallel.ForEach(hotel.RoomWrappers, room =>
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

    public class RoomTypeFrees
    {
        public RoomType RoomType { get; set; }
        public List<int> Quantity { get; set; }
    }
}