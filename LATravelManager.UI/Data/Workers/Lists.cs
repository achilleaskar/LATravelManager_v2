using GalaSoft.MvvmLight.Command;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Lists;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.People;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.Data.Workers
{
    public class Lists : MyViewModelBase
    {
        #region Constructors

        public Lists(Excursion excursion, DateTime from, DateTime to)
        {
            //ShowCustomersCommand = new RelayCommand(ShowCustomers, CanShowCustomers);
            SelectedDate = DateTime.Today;
            Departments.CollectionChanged += Departments_CollectionChanged;
            //ShowRoomingListCommand = new RelayCommand(ShowRoomingList);
            PrintAllRoomingListsCommand = new RelayCommand(async () => { await PrintAllRoomingLists(); });
            //PrintListCommand = new RelayCommand(() => PrintList(false));
            //PrintListBackCommand = new RelayCommand(() => PrintList(true));
            //RoomingList.CollectionChanged += RoomingListChanged;
            RoomListCheckInFrom = DateTime.Today;
            RoomListCheckInTo = DateTime.Today.AddDays(3);
            Excursion = excursion;
            From = from;
            To = to;
        }

        #endregion Constructors

        #region Fields

        public const string DepartmentsPropertyName = nameof(Departments);
        public const string FilteredBookingsPropertyName = nameof(FilteredBookings);
        public const string FilteredCustomersPropertyName = nameof(FilteredCustomers);
        public const string HotelsPropertyName = nameof(Hotels);
        public const string RoomingListPropertyName = nameof(RoomingList);
        public const string RoomListCheckInFromPropertyName = nameof(RoomListCheckInFrom);
        public const string RoomListCheckOutFromPropertyName = nameof(RoomListCheckInTo);
        public const string SecondDepartsPropertyName = nameof(SecondDeparts);

        public const string SelectedDatePropertyName = nameof(SelectedDate);
        public const string SelectedHotelIndexPropertyName = nameof(SelectedHotelIndex);
        private ObservableCollection<Department> _Departments = new ObservableCollection<Department>();
        private ObservableCollection<Booking> _FilteredBookings;
        private ObservableCollection<Customer> _FilteredCustomers;
        private ObservableCollection<Hotel> _Hotels = new ObservableCollection<Hotel>();
        private ObservableCollection<Reservation> _RoomingList = new ObservableCollection<Reservation>();
        private DateTime _RoomListCheckInFrom;
        private DateTime _RoomListCheckInTo;
        private bool _SecondDeparts = false;

        private DateTime _SelectedDate;

        private int _SelectedHotelIndex;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Departments property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<Department> Departments
        {
            get
            {
                return _Departments;
            }

            set
            {
                if (_Departments == value)
                {
                    return;
                }

                _Departments = value;
                RaisePropertyChanged(DepartmentsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the FilteredBookings property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<Booking> FilteredBookings
        {
            get
            {
                return _FilteredBookings;
            }

            set
            {
                if (_FilteredBookings == value)
                {
                    return;
                }

                _FilteredBookings = value;
                RaisePropertyChanged(FilteredBookingsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the FilteredCustomers property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<Customer> FilteredCustomers
        {
            get
            {
                return _FilteredCustomers;
            }

            set
            {
                if (_FilteredCustomers == value)
                {
                    return;
                }

                _FilteredCustomers = value;
                RaisePropertyChanged(FilteredCustomersPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Hotels property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ObservableCollection<Hotel> Hotels
        {
            get
            {
                return _Hotels;
            }

            set
            {
                if (_Hotels == value)
                {
                    return;
                }

                _Hotels = value;
                RaisePropertyChanged(HotelsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the IsVisible property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsVisible => Departments.Count > 0;

        public RelayCommand PrintAllRoomingListsCommand { get; set; }

        public RelayCommand PrintListBackCommand { get; set; }

        public RelayCommand PrintListCommand { get; set; }

        /// <summary>
        /// Sets and gets the RoomingList property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ObservableCollection<Reservation> RoomingList
        {
            get
            {
                return _RoomingList;
            }

            set
            {
                if (_RoomingList == value)
                {
                    return;
                }

                _RoomingList = value;
                RaisePropertyChanged(RoomingListPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the RoomListCheckInFrom property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public DateTime RoomListCheckInFrom
        {
            get
            {
                return _RoomListCheckInFrom;
            }

            set
            {
                if (_RoomListCheckInFrom == value)
                {
                    return;
                }

                _RoomListCheckInFrom = value;
                RaisePropertyChanged(RoomListCheckInFromPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the RoomListCheckOutFrom property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public DateTime RoomListCheckInTo
        {
            get
            {
                return _RoomListCheckInTo;
            }

            set
            {
                if (_RoomListCheckInTo == value)
                {
                    return;
                }

                _RoomListCheckInTo = value;
                RaisePropertyChanged(RoomListCheckOutFromPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the SecondDeparts property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool SecondDeparts
        {
            get
            {
                return _SecondDeparts;
            }

            set
            {
                if (_SecondDeparts == value)
                {
                    return;
                }

                _SecondDeparts = value;
                RaisePropertyChanged(SecondDepartsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the SelectedDate property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                return _SelectedDate;
            }

            set
            {
                if (_SelectedDate == value)
                {
                    return;
                }

                _SelectedDate = value;
                RaisePropertyChanged(SelectedDatePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the SelectedHotelIndex property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int SelectedHotelIndex
        {
            get
            {
                return _SelectedHotelIndex;
            }

            set
            {
                if (_SelectedHotelIndex == value)
                {
                    return;
                }

                _SelectedHotelIndex = value;
                RaisePropertyChanged(SelectedHotelIndexPropertyName);
            }
        }

        public RelayCommand ShowCustomersCommand { get; set; }

        public RelayCommand ShowRoomingListCommand { get; set; }

        #endregion Properties

        #region Methods

        public static string GetPath(string fileName)
        {
            int i = 1;
            string resultPath;
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Λίστες";
            Directory.CreateDirectory(folder);

            resultPath = folder + fileName + ".xlsx";
            string fileExtension = ".xlsx";
            while (File.Exists(resultPath))
            {
                i++;
                resultPath = folder + fileName + "(" + i + ")" + fileExtension;
            }
            return resultPath;
        }

        public void CreateRoomingList(List<RoomingList> roomingLists)
        {
            int lineNum;
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RoomingLists");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RoomingLists\" + $"Rooming List {From.ToString("dd.MM")}-{To.ToString("dd.MM")}" + ".xlsx";

            FileInfo fileInfo = new FileInfo(path);
            ExcelPackage p = new ExcelPackage();
            int pageCounter = 0;
            foreach (RoomingList roomingList in roomingLists)
            {
                pageCounter++;
                p.Workbook.Worksheets.Add($"{roomingList.Hotel.Name} {roomingList.Reservations[0].HotelDates}");
                ExcelWorksheet myWorksheet = p.Workbook.Worksheets[pageCounter];
                myWorksheet.Cells["A1"].Value = roomingList.Hotel.Name;
                myWorksheet.Cells["A1:B1"].Merge = true;
                lineNum = 5;
                int counter = 0;
                foreach (ReservationWrapper reservation in roomingList.Reservations)
                {
                    myWorksheet.Column(1).Width = 3;
                    myWorksheet.Column(2).Width = 12;
                    myWorksheet.Column(3).Width = 16;
                    myWorksheet.Column(4).Width = 16;
                    myWorksheet.Column(5).Width = 6;
                    counter++;
                    myWorksheet.Cells["A" + lineNum].Value = counter;
                    myWorksheet.Cells["A" + lineNum + ":A" + (lineNum + reservation.CustomersList.Count - 1)].Merge = true;
                    myWorksheet.Cells["B" + lineNum].Value = reservation.HotelDates;
                    myWorksheet.Cells["B" + lineNum + ":B" + (lineNum + reservation.CustomersList.Count - 1)].Merge = true;
                    myWorksheet.Cells["G" + lineNum].Value = reservation.HB ? "HB" : "BB";
                    myWorksheet.Cells["G" + lineNum + ":G" + (lineNum + reservation.CustomersList.Count - 1)].Merge = true;
                    myWorksheet.Cells["F" + lineNum].Value = reservation.RoomTypeName;
                    myWorksheet.Cells["F" + lineNum + ":F" + (lineNum + reservation.CustomersList.Count - 1)].Merge = true;
                    foreach (Customer customer in reservation.CustomersList)
                    {
                        myWorksheet.Cells["C" + lineNum].Value = customer.Name;
                        myWorksheet.Cells["D" + lineNum].Value = customer.Surename;
                        myWorksheet.Cells["E" + lineNum].Value = customer.Age < 18 ? customer.Age.ToString() + "yo" : "";
                        lineNum++;
                    }
                }
                myWorksheet.Cells["A4:G" + lineNum].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            //fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
            p.SaveAs(fileInfo);
            // Process.Start(path);
        }

        //public async Task PrintList(bool isback)
        //{
        //    string PhoneNumbers = "";
        //    string wbPath = null;
        //    var sampleFile = AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo_leoforeia.xlsx";
        //    //List<string> selectedCities = new List<string>();
        //    ExcelRange modelTable;
        //    int lineNum;
        //    Thread.CurrentThread.CurrentCulture = culture;

        //    var fileInfo = new FileInfo(sampleFile);
        //    var p = new ExcelPackage(fileInfo);
        //    ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];
        //    int counter = 0;
        //    int customersCount, secondline;
        //    var reservationsThisDay = new List<Reservation>();
        //    var RestReservations = new List<Reservation>();

        //    //foreach (CityDeptartureInfo city in PerCityDepartureList)
        //    //    if (city.IsChecked)
        //    //    {
        //    //        selectedCities.Add(city.City);
        //    //        foreach (Reservation rese in CollectionsManager.ReservationsCollection)
        //    //            if (rese.From == Date)
        //    //                foreach (Customer cust in rese.Customers)
        //    //                    if (cust.Location == city.City && cust.CustomerHasBusIndex < 2)
        //    //                    {
        //    //                        if (!reservationsThisDay.Contains(rese))
        //    //                            reservationsThisDay.Add(rese);
        //    //                        break;
        //    //                    }
        //    //}

        //    List<BookingWrapper> AllRBookings = new List<BookingWrapper>();

        //    if (!isback)
        //    {
        //        AllRBookings = (await GenericRepository.GetAllBookingInPeriod(From, To, Excursion.Id)).Select(b=>new BookingWrapper(b)).ToList();
        //        foreach (BookingWrapper booking in AllRBookings)
        //        {
        //            foreach (var r in booking.ReservationsInBooking)
        //            {
        //                if (r.CheckIn == SelectedDate)
        //                {
        //                    if (r.OnlyStay || (r.Booking.SecondDepart && !SecondDeparts) || (!r.Booking.SecondDepart && SecondDeparts))
        //                    {
        //                        RestReservations.Add(r);
        //                    }
        //                    else
        //                    {
        //                        reservationsThisDay.Add(r);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        AllRBookings = UOW.GenericRepository.Get<Booking>(filter: x => x.CheckOut == SelectedDate).ToList();
        //        foreach (Booking booking in AllRBookings)
        //        {
        //            foreach (var r in booking.ReservationsInBooking)
        //            {
        //                if (r.CheckOut == SelectedDate)
        //                {
        //                    if (r.OnlyStay)
        //                    {
        //                        // RestReservations.Add(r);
        //                    }
        //                    else
        //                    {
        //                        reservationsThisDay.Add(r);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    reservationsThisDay = reservationsThisDay.OrderBy(x => x.CustomersList[0].StartingPlace).ToList();
        //    reservationsThisDay = reservationsThisDay.OrderBy(x => x.HotelName).ToList();

        //    wbPath = GetPath("\\Λίστα Λεωφορείου για Bansko " + SelectedDate.ToString("dd_MM_yy"));

        //    myWorksheet.Cells["A1"].Value = "ΑΝΑΧΩΡΗΣΗ - " + SelectedDate.ToString("dd/MM/yyyy");
        //    lineNum = 5;
        //    myWorksheet.Cells["A2"].Value = "Αρ. Δωματίων:" + reservationsThisDay.Count;
        //    myWorksheet.Cells["E2"].Value = "Συνοδός:";

        //    if (reservationsThisDay.Count > 0)
        //    {
        //        foreach (Reservation reservation in reservationsThisDay)
        //        {
        //            customersCount = -1;
        //            foreach (Customer customer in reservation.CustomersList)
        //            {
        //                if ((!isback && customer.CustomerHasBusIndex < 2) || (isback && (customer.CustomerHasBusIndex == 0 || customer.CustomerHasBusIndex == 2)))
        //                {
        //                    //if (selectedCities.Contains(customer.Location))
        //                    {
        //                        counter++;
        //                        myWorksheet.InsertRow(lineNum, 1);
        //                        customersCount++;

        //                        myWorksheet.Cells["A" + lineNum].Value = counter;
        //                        myWorksheet.Cells["C" + lineNum].Value = customer.Name;
        //                        myWorksheet.Cells["D" + lineNum].Value = customer.Surename;
        //                        if (customersCount == 0)
        //                        {
        //                            myWorksheet.Cells["B" + lineNum].Value = reservation.CheckIn.Day + "-" + reservation.CheckOut.ToString("dd/MM");
        //                            myWorksheet.Cells["E" + lineNum].Value = reservation.HotelName;
        //                            if (reservation.Booking.IsPartners)
        //                                myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Partner.ToString().Length > 11) ? reservation.Booking.Partner.ToString().Substring(0, 11) : reservation.Booking.Partner.ToString();
        //                            else if (reservation.Booking.Remaining > 0)
        //                                myWorksheet.Cells["H" + lineNum].Value = reservation.Booking.Remaining + "€";
        //                        }
        //                        //else if (customersCount == 1)
        //                        //    myWorksheet.Cells["E" + lineNum].Value = reservation.RoomTypeName;
        //                        myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;
        //                        if (customer.Tel != null && (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal) || (customer.Tel.StartsWith("00", StringComparison.Ordinal) && customer.Tel.Length > 10 && customer.Tel.Length < 16)))
        //                        {
        //                            myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
        //                            PhoneNumbers += customer.Tel + ",";
        //                        }
        //                        else
        //                            myWorksheet.Cells["G" + lineNum].Value = " ";
        //                    }
        //                    //else
        //                    //{
        //                    //    myWorksheet.InsertRow(lineNum, 1);
        //                    //    customersCount++;

        //                    //    myWorksheet.Cells["A" + lineNum].Value = "X";
        //                    //    myWorksheet.Cells["C" + lineNum].Value = customer.Name;
        //                    //    myWorksheet.Cells["C" + lineNum].Style.Font.Strike = true;
        //                    //    myWorksheet.Cells["D" + lineNum].Value = customer.SureName;
        //                    //    myWorksheet.Cells["D" + lineNum].Style.Font.Strike = true;
        //                    //    if (customersCount == 0)
        //                    //    {
        //                    //        myWorksheet.Cells["B" + lineNum].Value = reservation.From.Day + "-" + reservation.To.ToString("dd/MM");
        //                    //        myWorksheet.Cells["E" + lineNum].Value = reservation.Room.Hotel;
        //                    //        if (reservation.IsPartner)
        //                    //            myWorksheet.Cells["H" + lineNum].Value = (reservation.Partner.ToString().Length > 7) ? reservation.Partner.ToString().Substring(0, 7) : reservation.Partner.ToString();
        //                    //        else if (reservation.Ypoloipo > 0)
        //                    //            myWorksheet.Cells["H" + lineNum].Value = reservation.Ypoloipo + "€";
        //                    //    }
        //                    //    else if (customersCount == 1)
        //                    //        myWorksheet.Cells["E" + lineNum].Value = reservation.Room.RoomType;

        //                    //    myWorksheet.Cells["F" + lineNum].Value = customer.Location;
        //                    //    myWorksheet.Cells["F" + lineNum].Style.Font.Strike = true;
        //                    //    if (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal))
        //                    //    {
        //                    //        myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
        //                    //        myWorksheet.Cells["G" + lineNum].Style.Font.Strike = true;
        //                    //    }
        //                    //    else
        //                    //        myWorksheet.Cells["G" + lineNum].Value = " ";
        //                    //}
        //                    lineNum++;
        //                }
        //            }
        //            myWorksheet.Cells["E" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            myWorksheet.Cells["B" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            //if (reservation.Room.Id >= 0)
        //            //{
        //            //    PrintVoucher(reservation, out string dir);
        //            //}
        //        }
        //        PhoneNumbers = PhoneNumbers.Trim(',');
        //        Clipboard.SetText(PhoneNumbers);
        //        lineNum--;
        //        modelTable = myWorksheet.Cells["A5:A" + (lineNum)];
        //        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //        modelTable = myWorksheet.Cells["C5:D" + (lineNum)];
        //        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //        modelTable = myWorksheet.Cells["F5:H" + (lineNum)];
        //        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //        if (RestReservations.Count > 0)
        //        {
        //            int secondCounter = 1;
        //            lineNum += 3;
        //            myWorksheet.Cells["A" + lineNum + ":H" + lineNum].Merge = true;
        //            myWorksheet.Cells["A" + lineNum].Value = "Εκτός Λεωφορείου";
        //            myWorksheet.Cells["A" + lineNum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            lineNum += 2;
        //            secondline = lineNum;
        //            foreach (Reservation reservation in RestReservations)
        //            {
        //                customersCount = -1;
        //                foreach (Customer customer in reservation.CustomersList)
        //                {
        //                    myWorksheet.InsertRow(lineNum, 1);
        //                    customersCount++;

        //                    myWorksheet.Cells["A" + lineNum].Value = secondCounter;
        //                    myWorksheet.Cells["C" + lineNum].Value = customer.Name;
        //                    myWorksheet.Cells["D" + lineNum].Value = customer.Surename;
        //                    if (customersCount == 0)
        //                    {
        //                        myWorksheet.Cells["B" + lineNum].Value = reservation.CheckIn.Day + "-" + reservation.CheckOut.ToString("dd/MM");
        //                        myWorksheet.Cells["E" + lineNum].Value = reservation.HotelName;
        //                        if (reservation.Booking.IsPartners)
        //                            myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Partner.ToString().Length > 11) ? reservation.Booking.Partner.ToString().Substring(0, 11) : reservation.Booking.Partner.ToString();
        //                        else if (reservation.Booking.Remaining > 0)
        //                            myWorksheet.Cells["H" + lineNum].Value = reservation.Booking.Remaining + "€";
        //                    }
        //                    //else if (customersCount == 1)
        //                    //    myWorksheet.Cells["E" + lineNum].Value = reservation.RoomTypeName;

        //                    myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;
        //                    if (customer.Tel != null && (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal) || (customer.Tel.StartsWith("00", StringComparison.Ordinal) && customer.Tel.Length > 10 && customer.Tel.Length < 16)))
        //                        myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
        //                    else
        //                        myWorksheet.Cells["G" + lineNum].Value = " ";

        //                    lineNum++;
        //                    secondCounter++;
        //                }
        //                myWorksheet.Cells["E" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            }
        //            lineNum--;
        //            modelTable = myWorksheet.Cells["A" + secondline + ":H" + (lineNum)];
        //            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //        }

        //        fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
        //        p.SaveAs(fileInfo);
        //        Process.Start(wbPath);
        //    }
        //    else
        //        MessageBox.Show("Δέν υπάρχουν άτομα που να πηγαίνουν τις συγκεκριμένες ημέρες απο τις επιλεγμένες πόλεις", "Σφάλμα");
        //}

        //private bool CanShowCustomers()
        //{
        //    return SelectedDate.Year >= 2018;
        //}

        private void DepartmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectCustomers();
        }

        private void Departments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Department item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= DepartmentModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Department item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += DepartmentModelPropertyChanged;
                }
            }
        }

        public async Task PrintAllRoomingLists()
        {
            List<RoomingList> rl = new List<RoomingList>();
            IEnumerable<Booking> tmplist = null;
            List<Reservation> tmpress = new List<Reservation>();
            tmplist = (await GenericRepository.GetAllBookingInPeriod(From, To, Excursion.Id));
            
            foreach (Booking b in tmplist)
            {
                tmpress.AddRange(b.ReservationsInBooking);
            }
            List<IGrouping<(Hotel Hotel, DateTime CheckIn, DateTime CheckOut), ReservationWrapper>> wrers =
                tmpress.Where(r=>r.ReservationType==ReservationTypeEnum.Normal || r.ReservationType == ReservationTypeEnum.Overbooked).Select(x => new ReservationWrapper(x)).GroupBy(r => (r.Hotel, r.CheckIn, r.CheckOut)).ToList()
             .OrderBy(t => t.Key.Hotel.Name).ThenBy(ti => ti.Key.CheckIn).ThenBy(to => to.Key.CheckOut).ToList();
            RoomingList rmList;
            RoomingList nonameroominglist = new RoomingList { Hotel = new HotelWrapper { Name = "NO NAME" } };

            foreach (IGrouping<(Hotel Hotel, DateTime CheckIn, DateTime CheckOut), ReservationWrapper> group in wrers)
            {
                rmList = new RoomingList { Hotel = new HotelWrapper(group.Key.Hotel) };
                foreach (ReservationWrapper res in group.OrderBy(r => r.Booking.Id))
                {
                    if (res.ReservationType == ReservationTypeEnum.Noname)
                        nonameroominglist.Reservations.Add(res);
                    else
                        rmList.Reservations.Add(res);
                }
                rl.Add(rmList);
            }
            if (nonameroominglist.Reservations.Count > 0)
            {
                rl.Add(nonameroominglist);
            }

            //foreach (var h in Hotels.Select(h => new HotelWrapper(h)))
            //{
            //    int hotelid = h.Id;
            //    if (tmplist.Count() > 0)
            //    {
            //        rl.Add(new RoomingList { Hotel = h });
            //        int count = 1;
            //        foreach (var b in tmplist)
            //        {
            //            foreach (var r in b.ReservationsInBooking.Select(r => new ReservationWrapper(r)))
            //            {
            //                if (r.CheckIn >= From && r.CheckIn <= To && ((r.Room != null && r.Room.Hotel.Id == hotelid) || (r.Hotel != null && r.Hotel.Id == hotelid)))
            //                {
            //                    r.Number = count;
            //                    rl.Last().Reservations.Add(r);
            //                    count++;
            //                }

            //            }
            //        }
            //        rl.Add(nonameroominglist);
            //    }
            //}
            //    rl = rl.OrderBy(r => r.Hotel).ThenBy(r=>r.Reservations[0].CheckIn).ToList();
            CreateRoomingList(rl);
        }

        private void RoomingListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    foreach (Reservation r in e.OldItems)
            //    {
            //    }
            //}
            //else
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ReservationWrapper r in e.NewItems)
                {
                    r.Number = RoomingList.Count;
                }
            }
        }

        private void SelectCustomers()
        {
            foreach (Booking b in FilteredBookings)
            {
                foreach (Reservation r in b.ReservationsInBooking)
                {
                    foreach (Customer c in r.CustomersList)
                    {
                        if (Departments.Any(x => x.StartingPlace.Equals(c.StartingPlace) && x.IsChecked))
                        {
                            // c.IsSelected = true;
                        }
                        else
                        {
                            // c.IsSelected = false;
                        }
                    }
                }
            }
        }

        //private void ShowCustomers()
        //{
        //    try
        //    {
        //        int bookingno = 0, resno = 0, customerno = 0;
        //        Departments.Clear();
        //        UOW.Reload();
        //        var tmpBookingsList = UOW.GenericRepository.Get<Booking>(includeProperties: "Excursion, Partner, Payments, ReservationsInBooking, ReservationsInBooking.Hotel, Payments.User, ReservationsInBooking.CustomersList, ReservationsInBooking.Room",
        //                filter: x => x.CheckIn == SelectedDate && x.Excursion.Id == SelectedExcursion.Id);
        //        FilteredBookings = new ObservableCollection<Booking>(tmpBookingsList);
        //        List<Department> departmentsTmpList = new List<Department>();
        //        int IndexOfStatingPlace = -1;
        //        foreach (var b in tmpBookingsList)
        //        {
        //            bookingno++;
        //            b.Number = bookingno;
        //            foreach (var r in b.ReservationsInBooking)
        //            {
        //                resno++;
        //                r.Number = resno;
        //                foreach (var c in r.CustomersList)
        //                {
        //                    customerno++;
        //                    c.Number = customerno;
        //                    IndexOfStatingPlace = departmentsTmpList.IndexOf(departmentsTmpList.Where(p => p.StartingPlace.Equals(c.StartingPlace)).FirstOrDefault());
        //                    if (IndexOfStatingPlace == -1)
        //                    {
        //                        departmentsTmpList.Add(new Department { StartingPlace = c.StartingPlace });
        //                    }
        //                    else
        //                    {
        //                        departmentsTmpList[IndexOfStatingPlace].Quantity++;
        //                    }
        //                }
        //            }
        //        }
        //        foreach (Department d in departmentsTmpList)
        //        {
        //            Departments.Add(d);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
        //    }
        //}

        //private void ShowRoomingList()
        //{
        //    UOW.Reload();
        //    Mouse.OverrideCursor = Cursors.Wait;
        //    int hotelid = 0;
        //    //if (SelectedHotelIndex == 0)
        //    //{
        //    //    tmplist = UOW.GenericRepository.Get<Reservation>(filter: x => x.Booking.Excursion.Id == SelectedExcursion.Id);
        //    //}
        //    //else
        //    //{
        //    hotelid = Hotels[SelectedHotelIndex].Id;
        //    // x.Hotel.Id == hotelid &&
        //    IEnumerable<Booking> tmplist = null;
        //    tmplist = UOW.GenericRepository.Get<Booking>(filter: x => x.Excursion.Id == SelectedExcursion.Id && x.CheckIn >= RoomListCheckInFrom && x.CheckIn <= RoomListCheckInTo,
        //        includeProperties: "Excursion, ReservationsInBooking.CustomersList");
        //    // }

        //    List<Reservation> tmproomingList = new List<Reservation>();

        //    int count = 1;
        //    foreach (var b in tmplist)
        //    {
        //        foreach (var r in b.ReservationsInBooking)
        //        {
        //            if (r.CheckIn >= RoomListCheckInFrom && r.CheckIn <= RoomListCheckInTo && ((r.Room != null && r.Room.Hotel.Id == hotelid) || (r.Hotel != null && r.Hotel.Id == hotelid)))
        //            {
        //                r.Number = count;
        //                tmproomingList.Add(r);
        //                count++;
        //            }
        //        }
        //    }

        //    RoomingList = new ObservableCollection<Reservation>(tmproomingList);
        //    Mouse.OverrideCursor = Cursors.Arrow;
        //}

        public GenericRepository GenericRepository { get; set; }
        public Excursion Excursion { get; }
        public DateTime From { get; }
        public DateTime To { get; }

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            GenericRepository = new GenericRepository();
            Hotels = new ObservableCollection<Hotel>(await GenericRepository.GetAllHotelsInCityAsync(Excursion.Destinations[0].Id));
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}