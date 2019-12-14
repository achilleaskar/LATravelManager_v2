using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using TEST = DocumentFormat.OpenXml.Drawing;

namespace LATravelManager.UI.Helpers
{
    public partial class DocumentsManagement : IDisposable
    {
        #region Constructors

        public DocumentsManagement(GenericRepository context)
        {
            Context = context;
        }
        public void PrintOptionals(List<CustomerOptional> sofia)
        {
            Thread.CurrentThread.CurrentCulture = culture;

            using (GenericRepository Context = new GenericRepository())
            {

                if (sofia.Count > 0)
                {

                    FileInfo fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo kratiseon.xlsx");
                    ExcelPackage p = new ExcelPackage(fileInfo);
                    ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];

                    string wbPath = GetPath("Test_Phones", @"\proairetika\");
                    int lineNum = 4;


                    foreach (var c in sofia)
                    {
                        myWorksheet.InsertRow(lineNum, 1);

                        myWorksheet.Cells["A" + lineNum].Value = lineNum - 3;
                        myWorksheet.Cells["B" + lineNum].Value = c.Customer.Name;
                        myWorksheet.Cells["C" + lineNum].Value = c.Customer.Surename;
                        myWorksheet.Cells["D" + lineNum].Value = !string.IsNullOrEmpty(c.Customer.Tel) && !c.Customer.Tel.StartsWith("000") ? c.Customer.Tel : "";
                        myWorksheet.Cells["E" + lineNum].Value = c.Leader.Name.Length > 12 ? c.Leader.Name.Substring(0, 12) : c.Leader.Name;
                        myWorksheet.Cells["F" + lineNum].Value = c.Customer.Reservation.Room.Hotel.Name;

                        lineNum++;
                    }

                    fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
                    p.SaveAs(fileInfo);
                }
            }
        }
        #endregion Constructors

        #region Fields

        private readonly CultureInfo culture = new CultureInfo("el-GR");
        private string folderNameContracts;
        private string folderNameVouchers;

        #endregion Fields

        #region Properties

        public GenericRepository Context { get; }

        public bool Nonamemess { get; set; }

        public bool Tranmess { get; set; }

        #endregion Properties

        #region Methods

        public static string CreateFolder(DateTime date, string folderName, string city)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + folderName + city + @"\" + date.ToString("MMMM") + @"\" + date.ToString("dd-MM-yy") + @"\";

            Directory.CreateDirectory(folder);

            return folder;
        }

        public static string GetPath(string fileName, string folderpath)
        {
            int i = 1;
            string resultPath;
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + folderpath;
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

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        public async void PrintAllBookings()
        {
            string wbPath = null;
            string sampleFile = AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo kratiseon.xlsx";
            //List<string> selectedCities = new List<string>();
            ExcelRange modelTable;
            int lineNum = 0;
            Thread.CurrentThread.CurrentCulture = culture;

            int counter;
            int customersCount;
            using (GenericRepository Context = new GenericRepository())
            {
                List<Booking> AllBookings = (await Context.GetAllBookingInPeriod(new DateTime(2018, 09, 01), new DateTime(2019, 09, 01), new City { Id = 15 })).ToList();

                if (AllBookings.Count > 0)
                {
                    AllBookings = AllBookings.OrderBy(o => o.CheckIn).ToList();

                    DateTime date = AllBookings[0].CheckIn;
                    List<BookingsPerDay> bookingsPerDays = new List<BookingsPerDay>();
                    BookingsPerDay CurrentDayBookings = new BookingsPerDay(date);
                    foreach (Booking b in AllBookings)
                    {
                        if (b.CheckIn == date)
                        {
                            CurrentDayBookings.Bookings.Add(b);
                        }
                        else
                        {
                            date = b.CheckIn;
                            bookingsPerDays.Add(CurrentDayBookings);
                            CurrentDayBookings = new BookingsPerDay(date);
                            CurrentDayBookings.Bookings.Add(b);
                        }
                    }
                    bool first = true;
                    foreach (BookingsPerDay day in bookingsPerDays)
                    {
                        FileInfo fileInfo = new FileInfo(sampleFile);
                        ExcelPackage p = new ExcelPackage(fileInfo);
                        ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];

                        counter = 0;
                        wbPath = GetPath(day.Date.ToString("dd-MM-yy"), @"\Κρατήσεις ανα ημέρα\" + day.Date.ToString("MMMM") + @"\");
                        customersCount = 0;
                        lineNum = 4;

                        foreach (Booking booking in day.Bookings)
                        {
                            first = true;
                            foreach (ReservationWrapper reservation in booking.ReservationsInBooking.Select(r => new ReservationWrapper(r)))
                            {
                                customersCount = -1;
                                foreach (Customer customer in reservation.CustomersList)
                                {
                                    counter++;
                                    customersCount++;
                                    myWorksheet.InsertRow(lineNum, 1);

                                    myWorksheet.Cells["B" + lineNum].Value = customer.Name;
                                    myWorksheet.Cells["C" + lineNum].Value = customer.Surename;
                                    myWorksheet.Cells["E" + lineNum].Value = string.IsNullOrEmpty(customer.Tel) ? "" : customer.Tel;
                                    myWorksheet.Cells["G" + lineNum].Value = (customer.StartingPlace.Length > 9) ? customer.StartingPlace.Substring(0, 9) : customer.StartingPlace;
                                    if (!booking.IsPartners)
                                    {
                                        myWorksheet.Cells["I" + lineNum].Value = customer.Price;
                                    }
                                    if (customersCount == 0)
                                    {
                                        myWorksheet.Cells["A" + lineNum].Value = booking.CheckIn.ToString("dd/MM") + "-" + booking.CheckOut.ToString("dd/MM");
                                        if (booking.IsPartners && first)
                                        {
                                            first = false;
                                            myWorksheet.Cells["I" + lineNum].Value = booking.NetPrice;
                                            myWorksheet.Cells["H" + lineNum].Value = booking.Partner.Name;
                                        }
                                        if (reservation.ReservationType == ReservationTypeEnum.Noname)
                                        {
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.GetNoNameType();
                                            myWorksheet.Cells["F" + lineNum].Value = "NO NAME";
                                        }
                                        else if (reservation.ReservationType == ReservationTypeEnum.Normal)
                                        {
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.Room.RoomType;
                                            myWorksheet.Cells["F" + lineNum].Value = reservation.Room.Hotel.Name;
                                        }
                                        else if (reservation.ReservationType == ReservationTypeEnum.Overbooked)
                                        {
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.GetNoNameType();
                                            myWorksheet.Cells["F" + lineNum].Value = reservation.Hotel.Name;
                                        }
                                        else if (reservation.ReservationType == ReservationTypeEnum.Transfer)
                                        {
                                            myWorksheet.Cells["F" + lineNum].Value = "TRANSFER";
                                        }
                                        else if (reservation.ReservationType == ReservationTypeEnum.OneDay)
                                        {
                                            myWorksheet.Cells["F" + lineNum].Value = "ONEDAY";
                                        }
                                        if (booking.IsPartners)
                                        {
                                            myWorksheet.Cells["H" + lineNum].Value = booking.Partner.Name;
                                        }
                                    }
                                    lineNum++;
                                    if (!booking.IsPartners)
                                    {
                                        myWorksheet.Cells["I" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                                        myWorksheet.Cells["H" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                                    }
                                }
                                myWorksheet.Cells["D" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                                myWorksheet.Cells["F" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                                myWorksheet.Cells["A" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            }
                            myWorksheet.Cells["B" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            if (booking.IsPartners)
                            {
                                myWorksheet.Cells["I" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                                myWorksheet.Cells["H" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            }
                        }
                        //if (reservation.Room.Id >= 0)
                        //{
                        //    PrintVoucher(reservation, out string dir);
                        //}
                        lineNum--;
                        modelTable = myWorksheet.Cells["C3:C" + (lineNum)];
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        modelTable = myWorksheet.Cells["E4:E" + (lineNum)];
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        modelTable = myWorksheet.Cells["G4:G" + (lineNum)];
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
                        p.SaveAs(fileInfo);
                    }
                }
                else
                    MessageBox.Show("Δέν υπάρχουν άτομα που να πηγαίνουν τις συγκεκριμένες ημέρες από τις επιλεγμένες πόλεις", "Σφάλμα");
            }
        }

        public async Task PrintAllPhones()
        {
            string wbPath = null;
            string sampleFile = AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo kratiseon.xlsx";
            //List<string> selectedCities = new List<string>();
            int lineNum = 0;
            Thread.CurrentThread.CurrentCulture = culture;

            using (GenericRepository Context = new GenericRepository())
            {
                List<Customer> AllBookings = (await Context.GetAllCustomersWithPhone()).ToList();

                if (AllBookings.Count > 0)
                {
                    AllBookings = AllBookings.OrderBy(o => o.StartingPlace).ToList();

                    FileInfo fileInfo = new FileInfo(sampleFile);
                    ExcelPackage p = new ExcelPackage(fileInfo);
                    ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];

                    wbPath = GetPath("Test_Phones", @"\Thlefona\");
                    lineNum = 4;

                    foreach (Customer customer in AllBookings)
                    {
                        myWorksheet.InsertRow(lineNum, 1);

                        myWorksheet.Cells["B" + lineNum].Value = customer.Name;
                        myWorksheet.Cells["C" + lineNum].Value = customer.Surename;
                        myWorksheet.Cells["E" + lineNum].Value = string.IsNullOrEmpty(customer.Tel) ? "" : customer.Tel;
                        myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;

                        lineNum++;
                    }

                    fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
                    p.SaveAs(fileInfo);
                }
            }
        }

        public void PrintContract(BookingWrapper booking)
        {
            string outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ReservationWrapper resWrapper;
            foreach (Reservation res in booking.ReservationsInBooking)
            {
                resWrapper = new ReservationWrapper(res);
                if (resWrapper.ReservationType == ReservationTypeEnum.Noname)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                }
                else if (resWrapper.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}

                try
                {
                    string ContractFilename;
                    if (booking.IsPartners)
                    {
                        ContractFilename = string.Format(@"\{0}_{1}_{2}_{3}_Contract.docx", resWrapper.CustomersList[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeNameByNum, booking.Partner.Name);
                    }
                    else
                    {
                        ContractFilename = string.Format(@"\{0}_{1}_{2}_Contract.docx", resWrapper.CustomersList[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeNameByNum);
                    }
                    Directory.CreateDirectory(outputpath + @"\Vouchers");
                    folderNameContracts = CreateFolder(resWrapper.CheckIn, @"\Contracts\", booking.Excursion.Name);

                    CreateWordContract(folderNameContracts + ContractFilename, resWrapper, booking);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void PrintLetter(BookingWrapper booking)
        {
            string outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            ReservationWrapper resWrapper;
            foreach (Reservation res in booking.ReservationsInBooking)
            {
                resWrapper = new ReservationWrapper(res);
                if (resWrapper.ReservationType == ReservationTypeEnum.Noname)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                }
                else if (resWrapper.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                else if (resWrapper.ReservationType == ReservationTypeEnum.Transfer)
                {
                    continue;
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}

                try
                {
                    string LetterFilename = string.Format(@"\{0}_{1}_{2}_{3}_Letter.docx", (resWrapper.CustomersList.OrderBy(cc => cc.Id)).ToList()[0].Surename, resWrapper.HotelName.TrimEnd(new[] { '*' }), resWrapper.RoomTypeNameByNum, resWrapper.Id);
                    Directory.CreateDirectory(outputpath + @"\Letters");
                    folderNameVouchers = CreateFolder(resWrapper.CheckIn, @"\Letters\", booking.Excursion.Name);

                    CreateWordLetter(folderNameVouchers + LetterFilename, resWrapper, booking);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void PrintSingleBookingContract(BookingWrapper booking)
        {
            foreach (Reservation res in booking.ReservationsInBooking)
            {
                if (res.ReservationType == ReservationTypeEnum.Noname && !Nonamemess)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                    Nonamemess = true;
                    break;
                }
                else if (res.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}
                else if (res.ReservationType == ReservationTypeEnum.Transfer && !Tranmess)
                {
                    MessageBox.Show("Η κράτηση είναι TRANSFER");
                    Tranmess = true;
                    break;
                }
            }
            PrintContract(booking);
        }

        public async Task PrintSingleBookingVoucher(BookingWrapper booking)
        {
            foreach (Reservation res in booking.ReservationsInBooking)
            {
                if (res.ReservationType == ReservationTypeEnum.Noname && !Nonamemess)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                    Nonamemess = true;
                }
                else if (res.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}
                else if (res.ReservationType == ReservationTypeEnum.Transfer && !Tranmess)
                {
                    MessageBox.Show("Η κράτηση είναι TRANSFER");
                    Tranmess = true;
                }
            }
            await PrintVoucher(booking);
        }

        public async Task PrintVoucher(BookingWrapper booking)
        {
            string outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            ReservationWrapper resWrapper;
            foreach (Reservation res in booking.ReservationsInBooking)
            {
                resWrapper = new ReservationWrapper(res);
                if (res.ReservationType == ReservationTypeEnum.Noname)
                {
                    // MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                    continue;
                }
                else if (res.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    // MessageBox.Show("Error");
                    continue;
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}
                else if (res.ReservationType == ReservationTypeEnum.Transfer)
                {
                    // MessageBox.Show("Η κράτηση είναι TRANSFER");
                    continue;
                }
                else if (res.ReservationType == ReservationTypeEnum.OneDay)
                {
                    // MessageBox.Show("Η κράτηση είναι TRANSFER");
                    continue;
                }

                try
                {
                    string VoucherFilename;
                    if (booking.IsPartners)
                    {
                        VoucherFilename = string.Format(@"\{0}_{1}_{2}_{3}_{4}_Voucher.docx", (resWrapper.CustomersList.OrderBy(cc => cc.Id)).ToList()[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeNameByNum, booking.Partner.Name, resWrapper.Id);
                    }
                    else
                    {
                        VoucherFilename = string.Format(@"\{0}_{1}_{2}_{3}_Voucher.docx", (resWrapper.CustomersList.OrderBy(cc => cc.Id)).ToList()[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeNameByNum, resWrapper.Id);
                    }
                    Directory.CreateDirectory(outputpath + @"\Vouchers");
                    folderNameVouchers = CreateFolder(resWrapper.CheckIn, @"\Vouchers\", booking.Excursion.Name);

                    Regex rgx = new Regex("[^a-zA-Z0-9\\ ._-]");
                    VoucherFilename = rgx.Replace(VoucherFilename, "");
                    await CreateWordVoucher(folderNameVouchers + VoucherFilename, resWrapper, booking);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        internal async Task PrintList(List<Booking> bookings, DateTime checkIn, Bus bus = null, bool borderonly = false)
        {
            IEnumerable<Booking> more = await Context.GetAllBookinsFromCustomers(checkIn);

            foreach (Booking b in more)
            {
                if (bookings.Count > 0 && b.Excursion.Id == bookings[0].Excursion.Id && !bookings.Any(h => h.Id == b.Id))
                {
                    bookings.Add(b);
                }
            }

            if (bookings.Count == 0)
            {
                return;
            }
            string PhoneNumbers = "";
            string wbPath = null;
            string sampleFile = AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo_leoforeia.xlsx";
            //List<string> selectedCities = new List<string>();
            ExcelRange modelTable;
            int lineNum;
            Thread.CurrentThread.CurrentCulture = culture;

            FileInfo fileInfo = new FileInfo(sampleFile);
            ExcelPackage p = new ExcelPackage(fileInfo);
            ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];
            int counter = 0;
            int customersCount, secondline;
            List<ReservationWrapper> reservationsThisDay = new List<ReservationWrapper>();
            List<ReservationWrapper> RestReservations = new List<ReservationWrapper>();

            //foreach (CityDeptartureInfo city in PerCityDepartureList)
            //    if (city.IsChecked)
            //    {
            //        selectedCities.Add(city.City);
            //        foreach (Reservation rese in CollectionsManager.ReservationsCollection)
            //            if (rese.From == Date)
            //                foreach (Customer cust in rese.Customers)
            //                    if (cust.Location == city.City && cust.CustomerHasBusIndex < 2)
            //                    {
            //                        if (!reservationsThisDay.Contains(rese))
            //                            reservationsThisDay.Add(rese);
            //                        break;
            //                    }
            //}

            foreach (Booking booking in bookings)
            {
                foreach (Reservation r in booking.ReservationsInBooking)
                {
                    if (r.OnlyStay || r.Booking.SecondDepart)
                    {
                        RestReservations.Add(new ReservationWrapper(r));
                    }
                    else //if ((!booking.DifferentDates || r.CustomersList.Any(c => c.CheckIn == checkIn) && r.CustomersList.Any(b => b.CustomerHasBusIndex < 2 && b.CheckIn == checkIn)))
                    {
                        reservationsThisDay.Add(new ReservationWrapper(r));
                    }
                }
            }

            reservationsThisDay = reservationsThisDay.OrderBy(x1 => x1.CustomersList[0].StartingPlace).ThenBy(x => x.HotelName).ToList();
            if (bus != null)
            {
                if (!borderonly)
                    wbPath = GetPath($"\\Λίστα Λεωφορείου για {reservationsThisDay[0].Booking.Excursion.Destinations[0].Name} " + bus.TimeGo.ToString("dd_MM_yy_") + bus.Vehicle.Name.Trim(new[] { '-' }));
                else
                    wbPath = GetPath($"\\Λίστα Συνόρων για {reservationsThisDay[0].Booking.Excursion.Destinations[0].Name} " + bus.TimeGo.ToString("dd_MM_yy_") + bus.Vehicle.Name.Trim(new[] { '-' }));
            }
            else
            {
                if (!borderonly)
                    wbPath = GetPath($"\\Λίστα Λεωφορείου για {reservationsThisDay[0].Booking.Excursion.Destinations[0].Name} " + checkIn.ToString("dd_MM_yy"));
                else
                    wbPath = GetPath($"\\Λίστα Συνόρων για {reservationsThisDay[0].Booking.Excursion.Destinations[0].Name} " + checkIn.ToString("dd_MM_yy"));
            }

            myWorksheet.Cells["A1"].Value = "ΑΝΑΧΩΡΗΣΗ - " + (bus == null ? checkIn.ToString("dd/MM/yyyy") : bus.TimeGo.ToString("dd/MM/yyyy"));
            lineNum = 5;
            myWorksheet.Cells["A2"].Value = "Αρ. Δωματίων:" + reservationsThisDay.Count;
            myWorksheet.Cells["F2"].Value = "Συνοδός:" + ((bus != null && bus.Leader != null && !string.IsNullOrEmpty(bus.Leader.Name)) ? bus.Leader.Name : "");
            myWorksheet.Cells["D2"].Value = "Λεωφορείο:" + ((bus != null && bus.Vehicle != null && !string.IsNullOrEmpty(bus.Vehicle.Name)) ? bus.Vehicle.Name : "");

            reservationsThisDay = reservationsThisDay.OrderBy(t => t.Higher).ToList();

            if (borderonly)
            {
                myWorksheet.Cells["E4"].Value = "Passport";
                myWorksheet.Cells["F4"].Value = "Ημ. Γεν";
                myWorksheet.Cells["G4"].Value = "";
                myWorksheet.Cells["H4"].Value = "";
                myWorksheet.Cells["I4"].Value = "";
                myWorksheet.Cells["J4"].Value = "";
                modelTable = myWorksheet.Cells["G4:JA" + (lineNum)];
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.None;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.None;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.None;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.None;
            }

            if (reservationsThisDay.Count > 0)
            {
                foreach (ReservationWrapper reservation in reservationsThisDay)
                {
                    customersCount = -1;
                    foreach (Customer customer in reservation.CustomersList)
                    {
                        if ((bus == null || (customer.Bus != null && customer.Bus.Id == bus.Id)) && customer.CustomerHasBusIndex < 2 && (!reservation.Booking.DifferentDates || customer.CheckIn == checkIn))
                        {
                            counter++;
                            myWorksheet.InsertRow(lineNum, 1);
                            customersCount++;

                            myWorksheet.Cells["A" + lineNum].Value = counter;
                            myWorksheet.Cells["C" + lineNum].Value = customer.Name;
                            myWorksheet.Cells["D" + lineNum].Value = customer.Surename;
                            if (customersCount == 0)
                            {
                                myWorksheet.Cells["B" + lineNum].Value = reservation.CheckIn.Day + "-" + reservation.CheckOut.ToString("dd/MM");
                                if (!borderonly)
                                {
                                    myWorksheet.Cells["E" + lineNum].Value = reservation.HotelName.TrimEnd(new[] { '*' });
                                    if (reservation.Booking.IsPartners)
                                        myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Partner.Name.Length > 11) ? reservation.Booking.Partner.Name.Substring(0, 11) : reservation.Booking.Partner.Name;
                                    // else if (!string.IsNullOrEmpty(reservation.Booking.Comment))
                                    // myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Comment.Length > 12) ? reservation.Booking.Comment.Substring(0, 11) : reservation.Booking.Comment;
                                }
                            }
                            //else if (customersCount == 1)
                            //    myWorksheet.Cells["E" + lineNum].Value = reservation.RoomTypeName;
                            if (!borderonly)
                            {
                                myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;
                                if (customer.Tel != null && (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal) || (customer.Tel.StartsWith("00", StringComparison.Ordinal) && customer.Tel.Length > 10 && customer.Tel.Length < 16)))
                                {
                                    myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
                                    PhoneNumbers += customer.Tel + ",";
                                }
                                else
                                    myWorksheet.Cells["G" + lineNum].Value = " ";
                            }
                            if (borderonly)
                            {
                                myWorksheet.Cells["E" + lineNum].Value = customer.PassportNum ?? "";
                                myWorksheet.Cells["F" + lineNum].Value = customer.DOB != null && customer.DOB.Value.Year > 1800 ? customer.DOB.Value.ToString("dd/MM/yy") : "";
                            }
                            lineNum++;

                        }
                        //else if (bus != null)
                        //{
                        //    myWorksheet.InsertRow(lineNum, 1);
                        //    customersCount++;

                        //    myWorksheet.Cells["A" + lineNum].Value = "X";
                        //    myWorksheet.Cells["C" + lineNum].Value = customer.Name;
                        //    myWorksheet.Cells["C" + lineNum].Style.Font.Strike = true;
                        //    myWorksheet.Cells["D" + lineNum].Value = customer.Surename;
                        //    myWorksheet.Cells["D" + lineNum].Style.Font.Strike = true;
                        //    if (customersCount == 0)
                        //    {
                        //        myWorksheet.Cells["B" + lineNum].Value = reservation.CheckIn.Day + "-" + reservation.CheckOut.ToString("dd/MM");
                        //        if (!borderonly)
                        //        {
                        //            myWorksheet.Cells["E" + lineNum].Value = reservation.HotelName.TrimEnd(new[] { '*' });
                        //            if (reservation.Booking.IsPartners)
                        //                myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Partner.ToString().Length > 11) ? reservation.Booking.Partner.ToString().Substring(0, 11) : reservation.Booking.Partner.ToString();
                        //            else if (!string.IsNullOrEmpty(reservation.Booking.Comment))
                        //                myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Comment.Length > 12) ? reservation.Booking.Comment.Substring(0, 11) : reservation.Booking.Comment;
                        //        }
                        //    }
                        //    //else if (customersCount == 1)
                        //    //    myWorksheet.Cells["E" + lineNum].Value = reservation.Room.RoomType;

                        //    if (!borderonly)
                        //    {
                        //        myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;
                        //        myWorksheet.Cells["F" + lineNum].Style.Font.Strike = true;
                        //        if (customer.Tel != null && (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal) || (customer.Tel.StartsWith("00", StringComparison.Ordinal) && customer.Tel.Length > 10 && customer.Tel.Length < 16)))
                        //        {
                        //            myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
                        //            myWorksheet.Cells["G" + lineNum].Style.Font.Strike = true;
                        //            PhoneNumbers += customer.Tel + ",";
                        //        }
                        //        else
                        //            myWorksheet.Cells["G" + lineNum].Value = " ";
                        //    }
                        //    if (borderonly)
                        //    {
                        //        myWorksheet.Cells["I" + lineNum].Value = customer.PassportNum ?? "";
                        //        myWorksheet.Cells["J" + lineNum].Value = customer.DOB != null && customer.DOB.Value.Year > 1800 ? customer.DOB.Value.ToString("dd/MM/yy") : "";
                        //        myWorksheet.Cells["I" + lineNum].Style.Font.Strike = true;
                        //        myWorksheet.Cells["J" + lineNum].Style.Font.Strike = true;
                        //    }
                        //}
                    }
                    myWorksheet.Cells["E" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    myWorksheet.Cells["B" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    //if (reservation.Room.Id >= 0)
                    //{
                    //    PrintVoucher(reservation, out string dir);
                    //}
                }
                PhoneNumbers = PhoneNumbers.Trim(',');
                Clipboard.SetText(PhoneNumbers);
                lineNum--;
                modelTable = myWorksheet.Cells["A5:A" + (lineNum)];
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                modelTable = myWorksheet.Cells["C5:D" + (lineNum)];
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                if (!borderonly)
                {
                    modelTable = myWorksheet.Cells["F5:H" + (lineNum)];
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                else
                {
                    modelTable = myWorksheet.Cells["E5:F" + (lineNum)];
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                //if (RestReservations.Count > 0)
                //{
                //    int secondCounter = 1;
                //    lineNum += 3;
                //    myWorksheet.Cells["A" + lineNum + ":H" + lineNum].Merge = true;
                //    myWorksheet.Cells["A" + lineNum].Value = "Εκτός Λεωφορείου";
                //    myWorksheet.Cells["A" + lineNum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    lineNum += 2;
                //    secondline = lineNum;
                //    foreach (ReservationWrapper reservation in RestReservations)
                //    {
                //        customersCount = -1;
                //        foreach (Customer customer in reservation.CustomersList)
                //        {
                //            myWorksheet.InsertRow(lineNum, 1);
                //            customersCount++;

                //            myWorksheet.Cells["A" + lineNum].Value = secondCounter;
                //            myWorksheet.Cells["C" + lineNum].Value = customer.Name;
                //            myWorksheet.Cells["D" + lineNum].Value = customer.Surename;
                //            if (customersCount == 0)
                //            {
                //                myWorksheet.Cells["B" + lineNum].Value = reservation.CheckIn.Day + "-" + reservation.CheckOut.ToString("dd/MM");
                //                myWorksheet.Cells["E" + lineNum].Value = reservation.HotelName.TrimEnd(new[] { '*' });
                //                if (!borderonly)
                //                {
                //                    if (reservation.Booking.IsPartners)
                //                        myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Partner.ToString().Length > 11) ? reservation.Booking.Partner.ToString().Substring(0, 11) : reservation.Booking.Partner.ToString();
                //                }
                //                //else if (reservation.Booking.Remaining > 0)
                //                //    myWorksheet.Cells["H" + lineNum].Value = reservation.Booking.Remaining + "€";
                //            }
                //            //else if (customersCount == 1)
                //            //    myWorksheet.Cells["E" + lineNum].Value = reservation.RoomTypeName;
                //            if (!borderonly)
                //            {
                //                myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;
                //                if (customer.Tel != null && (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal) || (customer.Tel.StartsWith("00", StringComparison.Ordinal) && customer.Tel.Length > 10 && customer.Tel.Length < 16)))
                //                    myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
                //                else
                //                    myWorksheet.Cells["G" + lineNum].Value = " ";
                //            }
                //            lineNum++;
                //            secondCounter++;
                //        }
                //        myWorksheet.Cells["E" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //    }
                //    lineNum--;
                //    modelTable = myWorksheet.Cells["A" + secondline + ":H" + (lineNum)];
                //    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //}

                fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
                p.SaveAs(fileInfo);
                // Process.Start(wbPath);
                p.Dispose();
            }
            else
                MessageBox.Show("Δέν υπάρχουν άτομα που να πηγαίνουν τις συγκεκριμένες ημέρες από τις επιλεγμένες πόλεις", "Σφάλμα");
            //if (!borderonly && bus != null)
            //{
            //    sampleFile = AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo_theseis.xlsx";
            //    //List<string> selectedCities = new List<string>();
            //    lineNum = 2;
            //    fileInfo = new FileInfo(sampleFile);
            //    p = new ExcelPackage(fileInfo);
            //    myWorksheet = p.Workbook.Worksheets[1];
            //    counter = 0;

            //    if (reservationsThisDay.Count > 0)
            //    {

            //        foreach (ReservationWrapper reservation in reservationsThisDay)
            //        {
            //            if (50 - ((lineNum - 1) % 50) < 4)
            //            {
            //                lineNum += 50 - ((lineNum - 1) % 50);
            //            }

            //            foreach (var c in reservation.CustomersList)
            //            {
            //                if (c.SeatNum == 0 || c.SeatNumRet == 0)
            //                {
            //                    //  MessageBox.Show($"O pelatis {c} den exei thesi");
            //                }
            //            }

            //            myWorksheet.Cells["A" + lineNum].Value = "Επώνυμα: " + reservation.GetSurnames();
            //            myWorksheet.Cells["A" + lineNum].Style.Font.Bold = true;

            //            myWorksheet.Cells["A" + ++lineNum].Value = "Αναχώρηση : Ταξίδι προς " + reservation.Booking.Excursion.Destinations[0].Name + ":";
            //            myWorksheet.Cells["A" + ++lineNum].Value = "Επιστροφή : Ταξίδι από " + reservation.Booking.Excursion.Destinations[0].Name + ":";

            //            lineNum--;

            //            if (reservation.CustomersList.Count > 0)
            //            {
            //                myWorksheet.Cells["B" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNum).ToList()[0].SeatNum;
            //            }
            //            if (reservation.CustomersList.Count > 1)
            //            {
            //                myWorksheet.Cells["C" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNum).ToList()[1].SeatNum;
            //            }
            //            if (reservation.CustomersList.Count > 2)
            //            {
            //                myWorksheet.Cells["D" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNum).ToList()[2].SeatNum;
            //            }
            //            if (reservation.CustomersList.Count > 3)
            //            {
            //                myWorksheet.Cells["E" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNum).ToList()[3].SeatNum;
            //            }
            //            if (reservation.CustomersList.Count > 4)
            //            {
            //                myWorksheet.Cells["F" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNum).ToList()[4].SeatNum;
            //            }
            //            lineNum++;
            //            if (reservation.CustomersList.Count > 0)
            //            {
            //                myWorksheet.Cells["B" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNumRet).ToList()[0].SeatNumRet;
            //            }
            //            if (reservation.CustomersList.Count > 1)
            //            {
            //                myWorksheet.Cells["C" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNumRet).ToList()[1].SeatNumRet;
            //            }
            //            if (reservation.CustomersList.Count > 2)
            //            {
            //                myWorksheet.Cells["D" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNumRet).ToList()[2].SeatNumRet;
            //            }
            //            if (reservation.CustomersList.Count > 3)
            //            {
            //                myWorksheet.Cells["E" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNumRet).ToList()[3].SeatNumRet;
            //            }
            //            if (reservation.CustomersList.Count > 4)
            //            {
            //                myWorksheet.Cells["F" + lineNum].Value = reservation.CustomersList.OrderBy(t => t.SeatNumRet).ToList()[4].SeatNumRet;
            //            }
            //            lineNum++;
            //            lineNum++;
            //        }

            //        wbPath = GetPath($"\\Θέσεις Λεωφορείου για {reservationsThisDay[0].Booking.Excursion.Destinations[0].Name} " + bus.TimeGo.ToString("dd_MM_yy"));

            //        fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
            //        p.SaveAs(fileInfo);
            //        // Process.Start(wbPath);
            //        p.Dispose();
            //    }
            //}
        }

        internal void PrintAllRooms(List<HotelContainer> hotelContainers)
        {
            string wbPath = null;
            string sampleFile = AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo kratiseon.xlsx";
            //List<string> selectedCities = new List<string>();
            int lineNum = 0;
            Thread.CurrentThread.CurrentCulture = culture;
            FileInfo fileInfo = new FileInfo(sampleFile);
            ExcelPackage p = new ExcelPackage(fileInfo);
            ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];

            wbPath = GetPath("Rooms", @"\Λίστες Δωματίων\");
            lineNum = 4;

            foreach (var hotel in hotelContainers)
            {
                myWorksheet.Cells["A" + lineNum].Value = hotel.Hotel.Name;
                lineNum++;
                foreach (var period in hotel.Periods)
                {
                    myWorksheet.Cells["B" + lineNum].Value = period.Period.ToString();
                    lineNum++;
                    foreach (var roomtype in period.Roomtypes)
                    {
                        myWorksheet.Cells["C" + lineNum].Value = roomtype.Rommtype.Name;
                        myWorksheet.Cells["E" + lineNum].Value = roomtype.Count.ToString();
                        lineNum++;
                    }
                }
            }

            fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
            p.SaveAs(fileInfo);
        }

        internal void PrintPaymentsReciept(Payment selectedPayment, Customer selectedCustomer)
        {
            string outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            Directory.CreateDirectory(outputpath + @"\Payments");
            if (selectedPayment == null)
            {
                return;
            }
            if (selectedPayment.Booking != null)
            {
                string RecieptFilename = string.Format(@"\Πληρωμή {0}_{1}.docx", selectedCustomer.Surename, selectedPayment.Booking.Excursion.Destinations[0].Name);

                CreateWordReciept(outputpath + @"\Payments\" + RecieptFilename, selectedPayment, selectedCustomer);
            }
            else if (selectedPayment.Personal_Booking != null)
            {
                string RecieptFilename = string.Format(@"\Πληρωμή {0}.docx", selectedCustomer.Surename);

                CreateWordReciept(outputpath + @"\Payments\" + RecieptFilename, selectedPayment, selectedCustomer);
            }
            else if (selectedPayment.ThirdParty_Booking != null)
            {
                string RecieptFilename = string.Format(@"\Πληρωμή {0}.docx", selectedCustomer.Surename);

                CreateWordReciept(outputpath + @"\Payments\" + RecieptFilename, selectedPayment, selectedCustomer);
            }
        }

        internal void PrintSingleBookingLetter(BookingWrapper booking)
        {
            foreach (Reservation res in booking.ReservationsInBooking)
            {
                if (res.ReservationType == ReservationTypeEnum.Noname && !Nonamemess)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                    Nonamemess = true;
                    return;
                }
                else if (res.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}
                else if (res.ReservationType == ReservationTypeEnum.Transfer && !Tranmess)
                {
                    MessageBox.Show("Η κράτηση είναι TRANSFER");
                    Tranmess = true;
                    return;
                }
            }
            PrintLetter(booking);
        }

        protected virtual void Dispose(bool b)
        {
        }

        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            long LCX = (long)(7.35 * 914400L);
            long LCY = (long)(4.17 * 914400L);
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                        new DW.Extent() { Cx = LCX, Cy = LCY },
                     new DW.EffectExtent()
                     {
                         LeftEdge = 0L,
                         TopEdge = 0L,
                         RightEdge = 0L,
                         BottomEdge = 0L
                     },
                         new DW.DocProperties()
                         {
                             Id = 1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new TEST.GraphicFrameLocks() { NoChangeAspect = true }),
                         new TEST.Graphic(
                             new TEST.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = 0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new TEST.Blip(
                                             new TEST.BlipExtensionList(
                                                 new TEST.BlipExtension()
                                                 {
                                                     Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState = TEST.BlipCompressionValues.Print
                                         },
                                         new TEST.Stretch(
                                             new TEST.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new TEST.Transform2D(
                                             new TEST.Offset() { X = 0L, Y = 0L },
                                             new TEST.Extents() { Cx = LCX, Cy = LCY }),
                                         new TEST.PresetGeometry(
                                             new TEST.AdjustValueList()
                                         )
                                         { Preset = TEST.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = 0U,
                         DistanceFromBottom = 0U,
                         DistanceFromLeft = 0U,
                         DistanceFromRight = 0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }

        private static void CreateWordReciept(string saveAs, Payment selectedPayment, Customer SelectedCustomer)
        {
            if (SelectedCustomer == null)
            {
                SelectedCustomer = selectedPayment.Booking.ReservationsInBooking[0].CustomersList[0];
            }
            string fileName;
            if (StaticResources.User.BaseLocation == 1)
            {
                fileName = @"Sources\reciept.docx";
            }
            else if (StaticResources.User.BaseLocation == 2)
            {
                fileName = @"Sources\reciept_larisa.docx";
            }
            else
            {
                return;
            }

            File.Copy(fileName, saveAs, true);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(saveAs, true))
            {
                try
                {
                    string docText = null;
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }

                    if (true)
                    {
                    }
                    int id = selectedPayment.Booking != null ? selectedPayment.Booking.Id : selectedPayment.Personal_Booking != null ? selectedPayment.Personal_Booking.Id : selectedPayment.ThirdParty_Booking.Id;
                    decimal remaining = selectedPayment.Booking != null ? new BookingWrapper(selectedPayment.Booking).Remaining : selectedPayment.Personal_Booking != null ? new Personal_BookingWrapper(selectedPayment.Personal_Booking).Remaining : new ThirdParty_Booking_Wrapper(selectedPayment.ThirdParty_Booking).Remaining;
                    decimal total = selectedPayment.Booking != null ? new BookingWrapper(selectedPayment.Booking).FullPrice : selectedPayment.Personal_Booking != null ? new Personal_BookingWrapper(selectedPayment.Personal_Booking).FullPrice : new ThirdParty_Booking_Wrapper(selectedPayment.ThirdParty_Booking).FullPrice;
                    string Description = selectedPayment.Booking != null ? new BookingWrapper(selectedPayment.Booking).GetPacketDescription() : selectedPayment.Personal_Booking != null ? new Personal_BookingWrapper(selectedPayment.Personal_Booking).GetPacketDescription() : new ThirdParty_Booking_Wrapper(selectedPayment.ThirdParty_Booking).GetPacketDescription();

                    Regex regexText = new Regex("regexcustomerid");
                    docText = regexText.Replace(docText, SelectedCustomer.Id.ToString());
                    regexText = new Regex("regexcustomername");
                    docText = regexText.Replace(docText, SelectedCustomer.ToString());
                    regexText = new Regex("regexpaymentmethod");
                    docText = regexText.Replace(docText, selectedPayment.GetPaymentMethod());
                    regexText = new Regex("regexpaymentid");
                    docText = regexText.Replace(docText, selectedPayment.Id.ToString());
                    regexText = new Regex("regexdatetoday");
                    docText = regexText.Replace(docText, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    regexText = new Regex("regexbookingid");
                    docText = regexText.Replace(docText, id.ToString());
                    regexText = new Regex("regexremainingamount");
                    docText = regexText.Replace(docText, remaining.ToString("c2"));
                    regexText = new Regex("regexdescription");
                    docText = regexText.Replace(docText, Description);
                    regexText = new Regex("regexfullwordamount");
                    docText = regexText.Replace(docText, StaticResources.ConvertAmountToWords(selectedPayment.Amount));
                    regexText = new Regex("regextotalamount");
                    docText = regexText.Replace(docText, total.ToString("c2"));
                    regexText = new Regex("regexamount");
                    docText = regexText.Replace(docText, selectedPayment.Amount.ToString("c2"));
                    regexText = new Regex("regexusername");
                    docText = regexText.Replace(docText, StaticResources.User.FullName.ToUpper());

                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }

                    wordDoc.Close();
                    Process.Start(saveAs);
                }
                catch (Exception)
                {
                }
            }
        }

        private void CreateWordContract(string saveAs, ReservationWrapper reservationWr, BookingWrapper booking)
        {
            string fileName;

            //if (booking.IsPartners)
            //    fileName = @"Sources\Voucher_afirmo.docx";
            //else if (booking.User.BaseLocation == 2)
            //    fileName = @"Sources\Voucher_enfirmo_larissas.docx";
            //else if (booking.User.BaseLocation == 1)
            //    fileName = @"Sources\Voucher_enfirmo_thess.docx";
            //else
            fileName = @"Sources\protypo_symvasis.docx";
            File.Copy(fileName, saveAs, true);
            Customer c = reservationWr.CustomersList[0];

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(saveAs, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                // var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"VoucherImages\";
                Regex regexText = new Regex("todaydate");
                docText = regexText.Replace(docText, DateTime.Today.ToString("dd/MM/yyyy"));
                regexText = new Regex("customername");
                docText = regexText.Replace(docText, c.Surename + " " + c.Name);
                regexText = new Regex("excursion");
                docText = regexText.Replace(docText, $"{reservationWr.DaysCount}ήμερη εκδρομή για {booking.Excursion.Destinations[0].Name}");
                regexText = new Regex("duration");
                docText = regexText.Replace(docText, reservationWr.DaysCount > 1 ? reservationWr.DaysCount + " ημέρες" : "1 ημέρα ");
                regexText = new Regex("startdate");
                docText = regexText.Replace(docText, reservationWr.CheckIn.ToString("dd/MM/yyyy"));
                regexText = new Regex("allcustomers");
                docText = regexText.Replace(docText, reservationWr.Names);
                regexText = new Regex("minyes");
                docText = regexText.Replace(docText, "....");
                regexText = new Regex("minno");
                docText = regexText.Replace(docText, " X ");
                regexText = new Regex("location");
                docText = regexText.Replace(docText, booking.User.BaseLocation == 2 ? "Λάρισα" : "Θεσσαλονίκη");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                wordDoc.Close();
                //Process.Start(saveAs);
            }
        }

        private void CreateWordLetter(string saveAs, ReservationWrapper reservationWr, BookingWrapper booking)
        {
            string fileName = @"Sources\letter.docx";

            File.Copy(fileName, saveAs, true);
            Customer c = (reservationWr.CustomersList.OrderBy(cc => cc.Id)).ToList()[0];

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(saveAs, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                // var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"VoucherImages\";
                Regex regexText = new Regex("fullname");
                docText = regexText.Replace(docText, c.Surename + " " + c.Name);
                regexText = new Regex("fulldate");
                docText = regexText.Replace(docText, reservationWr.Dates + " " + (reservationWr.Booking.ExcursionDate != null && reservationWr.Booking.ExcursionDate.NightStart ? reservationWr.DaysCount - 2 : reservationWr.DaysCount - 1) + "νυχτο");
                regexText = new Regex("destination");
                docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Name);
                regexText = new Regex("hotelnroomtype");
                string tel = reservationWr.GetHotelTel();
                docText = regexText.Replace(docText, reservationWr.HotelName + "-" + reservationWr.RoomTypeNameByNum + (!string.IsNullOrEmpty(tel) ? "(" + tel + ")" : ""));

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                wordDoc.Close();
                //Process.Start(saveAs);
            }
        }

        private async Task CreateWordVoucher(string saveAs, ReservationWrapper reservationWr, BookingWrapper booking)
        {
            string fileName = "";

            // if (booking.Excursion.Destinations[0].Id != 2)
            //{
            if (booking.IsPartners)
            {
                if (booking.User.BaseLocation == 2)
                    fileName = @"Sources\group\Voucher_afirmo_larissa.docx";
                else if (booking.User.BaseLocation == 1)
                    fileName = @"Sources\group\Voucher_afirmo_thess.docx";
            }
            else if (booking.User.BaseLocation == 2)
                fileName = @"Sources\group\Voucher_enfirmo_larissas.docx";
            else if (booking.User.BaseLocation == 1)
                fileName = @"Sources\group\Voucher_enfirmo_thess.docx";
            else
                fileName = @"Sources\group\Voucher_enfirmo_thess.docx";
            //}
            //else
            //{
            //    if (booking.IsPartners)
            //        fileName = @"Sources\Voucher_afirmo.docx";
            //    else if (booking.User.BaseLocation == 2)
            //        fileName = @"Sources\Voucher_enfirmo_larissas.docx";
            //    else if (booking.User.BaseLocation == 1)
            //        fileName = @"Sources\Voucher_enfirmo_thess.docx";
            //    else
            //        fileName = @"Sources\Voucher_enfirmo_thess.docx";
            //}
            StartingPlace customerStartingPlace = null;
            File.Copy(fileName, saveAs, true);
            Customer c = reservationWr.CustomersList[0];
            var place = booking.Excursion.Destinations[0].ExcursionTimes.FirstOrDefault(e => e.StartingPlace.Name == c.StartingPlace);
            if (place != null)
            {
                customerStartingPlace = place.StartingPlace;

            }
            Bus bus;
            bus = c.Bus ?? new Bus { Leader = new Leader { Name = "", Tel = "" } };

            if (customerStartingPlace == null || customerStartingPlace.Id == 19)
            {

                customerStartingPlace = new StartingPlace
                {
                    Details = reservationWr.CustomersList[0].StartingPlace,
                    Name = reservationWr.CustomersList[0].StartingPlace,
                    Id = -1
                };
                if (customerStartingPlace == null)
                {
                    MessageBox.Show($"Το σημείο {customerStartingPlace.Details} δεν είναι καταχωρημένο");
                }
            }
            if (string.IsNullOrEmpty(customerStartingPlace.Details))
            {
                customerStartingPlace.Details = "";
            }
            if (string.IsNullOrEmpty(customerStartingPlace.ReturnTime))
            {
                customerStartingPlace.ReturnTime = "";
            }
            if (string.IsNullOrEmpty(customerStartingPlace.StartTime))
            {
                customerStartingPlace.StartTime = "";
            }

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(saveAs, true))
            {
                try
                {
                    string docText = null;
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }
                    DateTime startDate = reservationWr.CheckIn;
                    if (booking.Excursion.Id == 2)
                    {
                        List<int> athinaika = new List<int> { 1, 4, 9, 20, 29 };
                        if (athinaika.Any(a => a == customerStartingPlace.Id))
                        {
                            startDate = startDate.AddDays(-1);
                        }
                    }
                    var time = booking.Excursion.Destinations[0].ExcursionTimes.Any(s => s.StartingPlace.Id == customerStartingPlace.Id) ? booking.Excursion.Destinations[0].ExcursionTimes.Where(s => s.StartingPlace.Id == customerStartingPlace.Id).FirstOrDefault().Time : new TimeSpan(999);
                    bool after12 = time.Ticks != 999 && booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Group && booking.ExcursionDate != null && booking.ExcursionDate.NightStart && time.Hours < 5;
                    // var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"VoucherImages\";
                    Regex regexText = new Regex("regexorderno");
                    docText = regexText.Replace(docText, reservationWr.Booking.Id.ToString());
                    regexText = new Regex("regexresno");
                    docText = regexText.Replace(docText, reservationWr.Id.ToString());
                    regexText = new Regex("regexdate");
                    docText = regexText.Replace(docText, DateTime.Today.ToString("dd/MM/yyyy"));
                    regexText = new Regex("regexhotel");
                    docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Id == 19 ? "Tiara(Προύσα), Mustafa(Προκόπι), Lion(Κωνσταντινούπολη)" : reservationWr.HotelName);
                    regexText = new Regex("regexaddress");
                    docText = regexText.Replace(docText, reservationWr.Room == null || string.IsNullOrEmpty(reservationWr.Room.Hotel.Address) ? reservationWr.Hotel != null ? reservationWr.Hotel.Address ?? "" : "" : reservationWr.Room.Hotel.Address);
                    regexText = new Regex("regexagency");
                    docText = regexText.Replace(docText, !booking.IsPartners ? "LA TRAVEL" : booking.Partner.Name);
                    regexText = new Regex("regexcity");
                    docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Name);
                    regexText = new Regex("regextel");
                    docText = regexText.Replace(docText, (reservationWr.GetHotelTel()));
                    regexText = new Regex("regexnames");
                    docText = regexText.Replace(docText, reservationWr.Names);
                    regexText = new Regex("regexcheckin");
                    docText = regexText.Replace(docText, booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Group && booking.ExcursionDate != null && booking.ExcursionDate.NightStart ? reservationWr.CheckIn.AddDays(1).ToString("dd/MM/yyyy") : reservationWr.CheckIn.ToString("dd/MM/yyyy"));
                    regexText = new Regex("regexcheckout");
                    docText = regexText.Replace(docText, reservationWr.CheckOut.ToString("dd/MM/yyyy"));
                    regexText = new Regex("regexroomtype");
                    docText = regexText.Replace(docText, reservationWr.RoomTypeNameByNum);
                    regexText = new Regex("regexnotes");
                    docText = regexText.Replace(docText, "");
                    regexText = new Regex("zdayscount");
                    docText = regexText.Replace(docText, reservationWr.DaysCount + ((reservationWr.DaysCount == 1) ? " Hμέρα" : " Hμέρες"));
                    regexText = new Regex("zdates");
                    docText = regexText.Replace(docText, reservationWr.Dates);
                    regexText = new Regex("regexstart");//
                    docText = regexText.Replace(docText, after12 ? reservationWr.CheckIn.AddDays(1).ToString("dd/MM/yyyy") : startDate.ToString("dd/MM/yyyy"));
                    regexText = new Regex("zreturntime");
                    docText = regexText.Replace(docText, reservationWr.ExcursionType == ExcursionTypeEnum.Skiathos ? "16:00" : booking.Excursion.Destinations[0].Id == 9 ? "09:00" : "11:00");
                    regexText = new Regex("zdate");//
                    docText = regexText.Replace(docText, after12 ? reservationWr.CheckIn.AddDays(1).ToString("dd/MM") : startDate.ToString("dd/MM"));
                    regexText = new Regex("zlocation");
                    docText = regexText.Replace(docText, reservationWr.CustomersList[0].StartingPlace);
                    regexText = new Regex("ztime");
                    docText = regexText.Replace(docText, time.Ticks != 999 ? time.ToString(@"hh\:mm") : "");
                    regexText = new Regex("zplace");
                    docText = regexText.Replace(docText, customerStartingPlace.Id == 14 && booking.Excursion.Destinations[0].Id == 5 ? "Εθνική Τράπεζα" : customerStartingPlace.Details);
                    regexText = new Regex("zsynodos");
                    docText = regexText.Replace(docText, booking.Excursion.Id == 29 ? "Αθανασία 6981189869" : bus != null && bus.Leader != null ? bus?.Leader.ToString() : booking.Excursion.Destinations[0].Id == 2 ? "ΣΤΡΑΤΟΣ: +30 6988558275" : "");
                    regexText = new Regex("zcity");
                    docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Name);
                    regexText = new Regex("zgostart");
                    docText = regexText.Replace(docText, booking.Excursion.Destinations[0].ExcursionTimes.Any(s => s.StartingPlace.Id == customerStartingPlace.Id) ? booking.Excursion.Destinations[0].ExcursionTimes.Where(s => s.StartingPlace.Id == customerStartingPlace.Id).FirstOrDefault().Time.ToString(@"hh\:mm") : "");
                    regexText = new Regex("zreturnstart");
                    docText = regexText.Replace(docText, customerStartingPlace.Id == 19 ? "" : (booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Group) ? "10:00" : "15:30");
                    regexText = new Regex("zreturnplacee");
                    docText = regexText.Replace(docText, booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Skiathos ? "Παλιό λιμάνι – γωνία - Μπούρτζι" : "");
                    regexText = new Regex("regexhotel");
                    docText = regexText.Replace(docText, reservationWr.HotelName);
                    regexText = new Regex("regexroomtype");
                    docText = regexText.Replace(docText, reservationWr.RoomTypeNameByNum);
                    regexText = new Regex("todaydate");
                    docText = regexText.Replace(docText, DateTime.Today.ToString("dd/MM/yyyy"));
                    regexText = new Regex("customername");
                    docText = regexText.Replace(docText, c.Surename + " " + c.Name);
                    regexText = new Regex("excursion");
                    docText = regexText.Replace(docText, $"Eκδρομή για {booking.Excursion.Destinations[0].Name}");
                    regexText = new Regex("duration");
                    docText = regexText.Replace(docText, reservationWr.DaysCount > 1 ? reservationWr.DaysCount + " ημέρες" : "1 ημέρα ");
                    regexText = new Regex("startdate");
                    docText = regexText.Replace(docText, startDate.ToString("dd/MM/yyyy"));
                    regexText = new Regex("allcustomers");
                    docText = regexText.Replace(docText, reservationWr.Names);
                    regexText = new Regex("minyes");
                    docText = regexText.Replace(docText, "....");
                    regexText = new Regex("minno");
                    docText = regexText.Replace(docText, " X ");
                    regexText = new Regex("location");
                    docText = regexText.Replace(docText, booking.User.BaseLocation == 2 ? "Λάρισα" : "Θεσσαλονίκη");
                    regexText = new Regex("regextax");
                    docText = regexText.Replace(docText, reservationWr.ExcursionType == ExcursionTypeEnum.Skiathos ?
                        "- Ο φόρος διαμονής καταβάλλεται επι τόπου στο κατάλυμα: Pension έως ξενοδοχεία 2* 0,5€ το δωμάτιο / βραδιά, ξενοδοχεία 3* 1,5€ το δωμάτιο ανά διανυκτέρευση." :
                        booking.Excursion.Destinations[0].Id == 9 ? "- Ο φόρος διαμονής καταβάλλεται πάνω στο λεωφορείο στον συνοδό και ανέρχεται σε 1,5€ το άτομο ανά διανυκτέρευση" : "");

                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }
                    MainDocumentPart mainPart = wordDoc.MainDocumentPart;

                    ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                    string imageName = @"Sources\VoucherImages\" + (booking.Excursion.Destinations[0].Id == 2 ? "bansko_general" : reservationWr.Room.Hotel.Id.ToString()) + ".jpg";
                    if (File.Exists(imageName))
                    {
                        using (FileStream stream = new FileStream(imageName, FileMode.Open))
                        {
                            imagePart.FeedData(stream);
                        }

                        AddImageToBody(wordDoc, mainPart.GetIdOfPart(imagePart));
                    }
                    wordDoc.Close();
                    //Process.Start(saveAs);
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion Methods
    }
}