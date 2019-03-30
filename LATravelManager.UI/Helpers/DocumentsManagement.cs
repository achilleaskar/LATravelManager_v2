using DocumentFormat.OpenXml.Packaging;
using LATravelManager.Models;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
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
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.Helpers
{
    public class DocumentsManagement : IDisposable
    {
        #region Constructors

        public DocumentsManagement(GenericRepository context)
        {
            Context = context;
        }

        #endregion Constructors

        #region Fields

        private readonly CultureInfo culture = new CultureInfo("el-GR");
        private string folderNameContracts;
        private string folderNameVouchers;

        #endregion Fields

        #region Properties

        public GenericRepository Context { get; }

        #endregion Properties

        #region Methods

        public static string GetPath(string fileName, string folderpath)
        {
            var i = 1;
            string resultPath;
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + folderpath;
            Directory.CreateDirectory(folder);

            resultPath = folder + fileName + ".xlsx";
            var fileExtension = ".xlsx";
            while (File.Exists(resultPath))
            {
                i++;
                resultPath = folder + fileName + "(" + i + ")" + fileExtension;
            }
            return resultPath;
        }

        public string CreateFolder(DateTime date, string folderName, string city)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + folderName + city + @"\" + date.ToString("MMMM") + @"\" + date.ToString("dd-MM-yy");

            Directory.CreateDirectory(folder);

            return folder;
        }

        public void Dispose()
        {
        }

        public async void PrintAllBookings()
        {
            string wbPath = null;
            var sampleFile = AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo kratiseon.xlsx";
            //List<string> selectedCities = new List<string>();
            ExcelRange modelTable;
            int lineNum = 0;
            Thread.CurrentThread.CurrentCulture = culture;

            int counter;
            int customersCount;
            using (GenericRepository Context = new GenericRepository())
            {
                List<Booking> AllBookings = (await Context.GetAllBookingInPeriod(new DateTime(2018, 09, 01), new DateTime(2019, 09, 01), 2)).ToList();

                if (AllBookings.Count > 0)
                {
                    AllBookings = AllBookings.OrderBy(o => o.CheckIn).ToList();

                    DateTime date = AllBookings[0].CheckIn;
                    List<BookingsPerDay> bookingsPerDays = new List<BookingsPerDay>();
                    BookingsPerDay CurrentDayBookings = new BookingsPerDay(date);
                    foreach (var b in AllBookings)
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
                    foreach (var day in bookingsPerDays)
                    {
                        var fileInfo = new FileInfo(sampleFile);
                        var p = new ExcelPackage(fileInfo);
                        ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];

                        counter = 0;
                        wbPath = GetPath(day.Date.ToString("dd-MM-yy"), @"\Κρατήσεις ανα ημέρα\" + day.Date.ToString("MMMM") + @"\");
                        customersCount = 0;
                        lineNum = 4;

                        foreach (Booking booking in day.Bookings)
                        {
                            first = true;
                            foreach (Reservation reservation in booking.ReservationsInBooking)
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
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.NoNameRoomType.Name;
                                            myWorksheet.Cells["F" + lineNum].Value = "NO NAME";
                                        }
                                        else if (reservation.ReservationType == ReservationTypeEnum.Normal)
                                        {
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.Room.RoomType;
                                            myWorksheet.Cells["F" + lineNum].Value = reservation.Room.Hotel.Name;
                                        }
                                        else if (reservation.ReservationType == ReservationTypeEnum.Overbooked)
                                        {
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.NoNameRoomType.Name;
                                            myWorksheet.Cells["F" + lineNum].Value = reservation.Hotel.Name;
                                        }
                                        else if (reservation.ReservationType == ReservationTypeEnum.Transfer)
                                        {
                                            myWorksheet.Cells["F" + lineNum].Value = "TRANSFER";
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
                    MessageBox.Show("Δέν υπάρχουν άτομα που να πηγαίνουν τις συγκεκριμένες ημέρες απο τις επιλεγμένες πόλεις", "Σφάλμα");
            }
        }

        public void PrintContract(BookingWrapper booking)
        {
            var outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ReservationWrapper resWrapper;
            foreach (var res in booking.ReservationsInBooking)
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
                        ContractFilename = string.Format(@"\{0}_{1}_{2}_{3}_Contract.docx", resWrapper.CustomersList[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeName, booking.Partner.Name);
                    }
                    else
                    {
                        ContractFilename = string.Format(@"\{0}_{1}_{2}_Contract.docx", resWrapper.CustomersList[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeName);
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
            var outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            ReservationWrapper resWrapper;
            foreach (var res in booking.ReservationsInBooking)
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
                    string VoucherFilename = string.Format(@"\{0}_{1}_{2}_{3}_Letter.docx", resWrapper.CustomersList[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeName, resWrapper.Id);
                    Directory.CreateDirectory(outputpath + @"\Letters");
                    folderNameVouchers = CreateFolder(resWrapper.CheckIn, @"\Letters\", booking.Excursion.Name);

                    CreateWordLetter(folderNameVouchers + VoucherFilename, resWrapper, booking);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void PrintSingleBookingContract(BookingWrapper booking)
        {
            foreach (var res in booking.ReservationsInBooking)
            {
                if (res.ReservationType == ReservationTypeEnum.Noname)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                }
                else if (res.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}
                else if (res.ReservationType == ReservationTypeEnum.Transfer)
                {
                    MessageBox.Show("Η κράτηση είναι TRANSFER");
                }
            }
            PrintContract(booking);
        }

        public async Task PrintSingleBookingVoucher(BookingWrapper booking)
        {
            foreach (var res in booking.ReservationsInBooking)
            {
                if (res.ReservationType == ReservationTypeEnum.Noname)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                }
                else if (res.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}
                else if (res.ReservationType == ReservationTypeEnum.Transfer)
                {
                    MessageBox.Show("Η κράτηση είναι TRANSFER");
                }
            }
            await PrintVoucher(booking);
        }

        public async Task PrintVoucher(BookingWrapper booking)
        {
            var outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            ReservationWrapper resWrapper;
            foreach (var res in booking.ReservationsInBooking)
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
                    string VoucherFilename;
                    if (booking.IsPartners)
                    {
                        VoucherFilename = string.Format(@"\{0}_{1}_{2}_{3}_{4}_Voucher.docx", resWrapper.CustomersList[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeName, booking.Partner.Name, resWrapper.Id);
                    }
                    else
                    {
                        VoucherFilename = string.Format(@"\{0}_{1}_{2}_{3}_Voucher.docx", resWrapper.CustomersList[0].Surename, resWrapper.HotelName, resWrapper.RoomTypeName, resWrapper.Id);
                    }
                    Directory.CreateDirectory(outputpath + @"\Vouchers");
                    folderNameVouchers = CreateFolder(resWrapper.CheckIn, @"\Vouchers\", booking.Excursion.Name);

                    await CreateWordVoucher(folderNameVouchers + VoucherFilename, resWrapper, booking);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static string GetPath(string fileName)
        {
            var i = 1;
            string resultPath;
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Λίστες";
            Directory.CreateDirectory(folder);

            resultPath = folder + fileName + ".xlsx";
            var fileExtension = ".xlsx";
            while (File.Exists(resultPath))
            {
                i++;
                resultPath = folder + fileName + "(" + i + ")" + fileExtension;
            }
            return resultPath;
        }

        internal async Task PrintList(List<Booking> bookings, DateTime checkIn)
        {
            var more = await Context.GetAllBookinsFromCustomers(checkIn);

            foreach (var b in more)
            {
                if (!bookings.Any(h => h.Id == b.Id))
                {
                    bookings.Add(b);
                }
            }
            string PhoneNumbers = "";
            string wbPath = null;
            var sampleFile = AppDomain.CurrentDomain.BaseDirectory + @"Sources\protypo_leoforeia.xlsx";
            //List<string> selectedCities = new List<string>();
            ExcelRange modelTable;
            int lineNum;
            Thread.CurrentThread.CurrentCulture = culture;

            var fileInfo = new FileInfo(sampleFile);
            var p = new ExcelPackage(fileInfo);
            ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];
            int counter = 0;
            int customersCount, secondline;
            var reservationsThisDay = new List<ReservationWrapper>();
            var RestReservations = new List<ReservationWrapper>();

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
                foreach (var r in booking.ReservationsInBooking)
                {
                    if (r.OnlyStay || r.Booking.SecondDepart)
                    {
                        RestReservations.Add(new ReservationWrapper( r));
                    }
                    else //if ((!booking.DifferentDates || r.CustomersList.Any(c => c.CheckIn == checkIn) && r.CustomersList.Any(b => b.CustomerHasBusIndex < 2 && b.CheckIn == checkIn)))
                    {
                        reservationsThisDay.Add(new ReservationWrapper(r));
                    }
                }
            }

            reservationsThisDay = reservationsThisDay.OrderBy(x => x.CustomersList[0].StartingPlace).ToList();
            reservationsThisDay = reservationsThisDay.OrderBy(x => x.HotelName).ToList();

            wbPath = GetPath($"\\Λίστα Λεωφορείου για {reservationsThisDay[0].Booking.Excursion.Destinations[0].Name} " + checkIn.ToString("dd_MM_yy"));

            myWorksheet.Cells["A1"].Value = "ΑΝΑΧΩΡΗΣΗ - " + checkIn.ToString("dd/MM/yyyy");
            lineNum = 5;
            myWorksheet.Cells["A2"].Value = "Αρ. Δωματίων:" + reservationsThisDay.Count;
            myWorksheet.Cells["E2"].Value = "Συνοδός:";

            if (reservationsThisDay.Count > 0)
            {
                foreach (ReservationWrapper reservation in reservationsThisDay)
                {
                    customersCount = -1;
                    foreach (Customer customer in reservation.CustomersList)
                    {
                        if (customer.CustomerHasBusIndex < 2 && (!reservation.Booking.DifferentDates || customer.CheckIn == checkIn))
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
                                myWorksheet.Cells["E" + lineNum].Value = reservation.HotelName;
                                if (reservation.Booking.IsPartners)
                                    myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Partner.Name.Length > 11) ? reservation.Booking.Partner.Name.Substring(0, 11) : reservation.Booking.Partner.Name;
                                else if (!string.IsNullOrEmpty(reservation.Booking.Comment))
                                    myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Comment.Length > 12) ? reservation.Booking.Comment.Substring(0, 11) : reservation.Booking.Comment;
                            }
                            //else if (customersCount == 1)
                            //    myWorksheet.Cells["E" + lineNum].Value = reservation.RoomTypeName;
                            myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;
                            if (customer.Tel != null && (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal) || (customer.Tel.StartsWith("00", StringComparison.Ordinal) && customer.Tel.Length > 10 && customer.Tel.Length < 16)))
                            {
                                myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
                                PhoneNumbers += customer.Tel + ",";
                            }
                            else
                                myWorksheet.Cells["G" + lineNum].Value = " ";
                            myWorksheet.Cells["I" + lineNum].Value = customer.PassportNum ?? "";
                            myWorksheet.Cells["J" + lineNum].Value = customer.DOB != null && customer.DOB.Value.Year > 1800 ? customer.DOB.Value.ToString("dd/MM/yy") : "";
                        }
                        else
                        {
                            myWorksheet.InsertRow(lineNum, 1);
                            customersCount++;

                            myWorksheet.Cells["A" + lineNum].Value = "X";
                            myWorksheet.Cells["C" + lineNum].Value = customer.Name;
                            myWorksheet.Cells["C" + lineNum].Style.Font.Strike = true;
                            myWorksheet.Cells["D" + lineNum].Value = customer.Surename;
                            myWorksheet.Cells["D" + lineNum].Style.Font.Strike = true;
                            if (customersCount == 0)
                            {
                                myWorksheet.Cells["B" + lineNum].Value = reservation.CheckIn.Day + "-" + reservation.CheckOut.ToString("dd/MM");
                                myWorksheet.Cells["E" + lineNum].Value = reservation.HotelName;
                                if (reservation.Booking.IsPartners)
                                    myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Partner.ToString().Length > 11) ? reservation.Booking.Partner.ToString().Substring(0, 11) : reservation.Booking.Partner.ToString();
                                else if (reservation.Booking.Comment.Length > 0)
                                    myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Comment.Length > 12) ? reservation.Booking.Comment.Substring(0, 11) : reservation.Booking.Comment;
                            }
                            //else if (customersCount == 1)
                            //    myWorksheet.Cells["E" + lineNum].Value = reservation.Room.RoomType;

                            myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;
                            myWorksheet.Cells["F" + lineNum].Style.Font.Strike = true;
                            if (customer.Tel != null && (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal) || (customer.Tel.StartsWith("00", StringComparison.Ordinal) && customer.Tel.Length > 10 && customer.Tel.Length < 16)))
                            {
                                myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
                                myWorksheet.Cells["G" + lineNum].Style.Font.Strike = true;
                                PhoneNumbers += customer.Tel + ",";
                            }
                            else
                                myWorksheet.Cells["G" + lineNum].Value = " ";
                            myWorksheet.Cells["I" + lineNum].Value = customer.PassportNum ?? "";
                            myWorksheet.Cells["J" + lineNum].Value = customer.DOB != null && customer.DOB.Value.Year > 1800 ? customer.DOB.Value.ToString("dd/MM/yy") : "";
                            myWorksheet.Cells["I" + lineNum].Style.Font.Strike = true;
                            myWorksheet.Cells["J" + lineNum].Style.Font.Strike = true;
                        }
                        lineNum++;
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

                modelTable = myWorksheet.Cells["F5:J" + (lineNum)];
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                if (RestReservations.Count > 0)
                {
                    int secondCounter = 1;
                    lineNum += 3;
                    myWorksheet.Cells["A" + lineNum + ":H" + lineNum].Merge = true;
                    myWorksheet.Cells["A" + lineNum].Value = "Εκτός Λεωφορείου";
                    myWorksheet.Cells["A" + lineNum].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    lineNum += 2;
                    secondline = lineNum;
                    foreach (ReservationWrapper reservation in RestReservations)
                    {
                        customersCount = -1;
                        foreach (Customer customer in reservation.CustomersList)
                        {
                            myWorksheet.InsertRow(lineNum, 1);
                            customersCount++;

                            myWorksheet.Cells["A" + lineNum].Value = secondCounter;
                            myWorksheet.Cells["C" + lineNum].Value = customer.Name;
                            myWorksheet.Cells["D" + lineNum].Value = customer.Surename;
                            if (customersCount == 0)
                            {
                                myWorksheet.Cells["B" + lineNum].Value = reservation.CheckIn.Day + "-" + reservation.CheckOut.ToString("dd/MM");
                                myWorksheet.Cells["E" + lineNum].Value = reservation.HotelName;
                                if (reservation.Booking.IsPartners)
                                    myWorksheet.Cells["H" + lineNum].Value = (reservation.Booking.Partner.ToString().Length > 11) ? reservation.Booking.Partner.ToString().Substring(0, 11) : reservation.Booking.Partner.ToString();
                                //else if (reservation.Booking.Remaining > 0)
                                //    myWorksheet.Cells["H" + lineNum].Value = reservation.Booking.Remaining + "€";
                            }
                            //else if (customersCount == 1)
                            //    myWorksheet.Cells["E" + lineNum].Value = reservation.RoomTypeName;

                            myWorksheet.Cells["F" + lineNum].Value = customer.StartingPlace;
                            if (customer.Tel != null && (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal) || (customer.Tel.StartsWith("00", StringComparison.Ordinal) && customer.Tel.Length > 10 && customer.Tel.Length < 16)))
                                myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
                            else
                                myWorksheet.Cells["G" + lineNum].Value = " ";

                            lineNum++;
                            secondCounter++;
                        }
                        myWorksheet.Cells["E" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    lineNum--;
                    modelTable = myWorksheet.Cells["A" + secondline + ":H" + (lineNum)];
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
                p.SaveAs(fileInfo);
                Process.Start(wbPath);
            }
            else
                System.Windows.MessageBox.Show("Δέν υπάρχουν άτομα που να πηγαίνουν τις συγκεκριμένες ημέρες απο τις επιλεγμένες πόλεις", "Σφάλμα");
        }

        internal void PrintSingleBookingLetter(BookingWrapper booking)
        {
            foreach (var res in booking.ReservationsInBooking)
            {
                if (res.ReservationType == ReservationTypeEnum.Noname)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                }
                else if (res.ReservationType == ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}
                else if (res.ReservationType == ReservationTypeEnum.Transfer)
                {
                    MessageBox.Show("Η κράτηση είναι TRANSFER");
                }
            }
            PrintLetter(booking);
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
            var c = reservationWr.CustomersList[0];

            using (var wordDoc = WordprocessingDocument.Open(saveAs, true))
            {
                string docText = null;
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                // var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"VoucherImages\";
                var regexText = new Regex("todaydate");
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
                docText = regexText.Replace(docText, "Θεσσαλονίκη");

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
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
            var c = reservationWr.CustomersList[0];

            using (var wordDoc = WordprocessingDocument.Open(saveAs, true))
            {
                string docText = null;
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                // var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"VoucherImages\";
                var regexText = new Regex("fullname");
                docText = regexText.Replace(docText, c.Surename + " " + c.Name);
                regexText = new Regex("fulldate");
                docText = regexText.Replace(docText, reservationWr.Dates + " " + (reservationWr.Booking.Excursion.NightStart? reservationWr.DaysCount-2:reservationWr.DaysCount-1) + "νυχτο");
                regexText = new Regex("destination");
                docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Name);
                regexText = new Regex("hotelnroomtype");
                docText = regexText.Replace(docText, reservationWr.HotelName + "-" + reservationWr.RoomTypeName);

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                wordDoc.Close();
                //Process.Start(saveAs);
            }
        }

        private async Task CreateWordVoucher(string saveAs, ReservationWrapper reservationWr, BookingWrapper booking)
        {
            string fileName;

            if (booking.Excursion.ExcursionType.Category == Model.Enums.ExcursionTypeEnum.Group)
            {
                if (booking.IsPartners)
                    fileName = @"Sources\group\Voucher_afirmo.docx";
                else if (booking.User.BaseLocation == 2)
                    fileName = @"Sources\group\Voucher_enfirmo_larissas.docx";
                else if (booking.User.BaseLocation == 1)
                    fileName = @"Sources\group\Voucher_enfirmo_thess.docx";
                else
                    fileName = @"Sources\group\Voucher_enfirmo_thess.docx";
            }
            else
            {
                if (booking.IsPartners)
                    fileName = @"Sources\Voucher_afirmo.docx";
                else if (booking.User.BaseLocation == 2)
                    fileName = @"Sources\Voucher_enfirmo_larissas.docx";
                else if (booking.User.BaseLocation == 1)
                    fileName = @"Sources\Voucher_enfirmo_thess.docx";
                else
                    fileName = @"Sources\Voucher_enfirmo_thess.docx";
            }
            File.Copy(fileName, saveAs, true);
            var c = reservationWr.CustomersList[0];
            StartingPlace customerStartingPlace = await Context.GetByNameAsync<StartingPlace>(c.StartingPlace);

            if (customerStartingPlace == null || customerStartingPlace.Id == 19)
            {
                customerStartingPlace = new StartingPlace
                {
                    Details = reservationWr.CustomersList[0].StartingPlace,
                    Name = reservationWr.CustomersList[0].StartingPlace,
                    Id = -1
                };
            }

            using (var wordDoc = WordprocessingDocument.Open(saveAs, true))
            {
                string docText = null;
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                bool after12 = (!string.IsNullOrEmpty(customerStartingPlace.ReturnTime) ? int.Parse(customerStartingPlace.ReturnTime.Substring(0, 2)) < 16 : false) && booking.Excursion.ExcursionType.Category == Model.Enums.ExcursionTypeEnum.Group;
                // var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"VoucherImages\";
                var regexText = new Regex("regexorderno");
                docText = regexText.Replace(docText, reservationWr.Booking.Id.ToString());
                regexText = new Regex("regexresno");
                docText = regexText.Replace(docText, reservationWr.Id.ToString());
                regexText = new Regex("regexdate");
                docText = regexText.Replace(docText, DateTime.Today.ToString("dd/MM/yyyy"));
                regexText = new Regex("regexhotel");
                docText = regexText.Replace(docText, reservationWr.HotelName);
                regexText = new Regex("regexaddress");
                docText = regexText.Replace(docText, reservationWr.Room == null || string.IsNullOrEmpty(reservationWr.Room.Hotel.Address) ? "" : reservationWr.Room.Hotel.Address);
                regexText = new Regex("regexagency");
                docText = regexText.Replace(docText, !booking.IsPartners ? "LA TRAVEL" : booking.Partner.Name);
                regexText = new Regex("regexcity");
                docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Name);
                regexText = new Regex("regextel");
                docText = regexText.Replace(docText, (reservationWr.Room == null || reservationWr.Room.Hotel.Tel == "0" || reservationWr.Room.Hotel.Tel == null) ? "" : reservationWr.Room.Hotel.Tel);
                regexText = new Regex("regexnames");
                docText = regexText.Replace(docText, reservationWr.Names);
                regexText = new Regex("regexcheckin");
                docText = regexText.Replace(docText, booking.Excursion.ExcursionType.Category == Model.Enums.ExcursionTypeEnum.Group ? reservationWr.CheckIn.AddDays(1).ToString("dd/MM/yyyy") : reservationWr.CheckIn.ToString("dd/MM/yyyy"));
                regexText = new Regex("regexcheckout");
                docText = regexText.Replace(docText, reservationWr.CheckOut.ToString("dd/MM/yyyy"));
                regexText = new Regex("regexroomtype");
                docText = regexText.Replace(docText, reservationWr.RoomTypeName);
                //regexText = new Regex("regexpayment");
                //docText = regexText.Replace(docText, reservation.HotelName);
                regexText = new Regex("regexnotes");
                docText = regexText.Replace(docText, "");
                regexText = new Regex("zdayscount");
                docText = regexText.Replace(docText, reservationWr.DaysCount + ((reservationWr.DaysCount == 1) ? " Hμέρα" : " Hμέρες"));
                regexText = new Regex("zdates");
                docText = regexText.Replace(docText, reservationWr.Dates);
                regexText = new Regex("regexstart");
                docText = regexText.Replace(docText, after12 ? reservationWr.CheckIn.AddDays(1).ToString("dd/MM/yyyy") : reservationWr.CheckIn.ToString("dd/MM/yyyy"));
                regexText = new Regex("zdate");
                docText = regexText.Replace(docText, after12 ? reservationWr.CheckIn.AddDays(1).ToString("dd/MM") : reservationWr.CheckIn.ToString("dd/MM"));
                regexText = new Regex("zlocation");
                docText = regexText.Replace(docText, reservationWr.CustomersList[0].StartingPlace);
                regexText = new Regex("ztime");
                docText = regexText.Replace(docText, (booking.Excursion.ExcursionType.Category == Model.Enums.ExcursionTypeEnum.Group) ? ((customerStartingPlace.Id < 0) ? "" : customerStartingPlace.ReturnTime) : ((customerStartingPlace.Id < 0) ? "" : customerStartingPlace.StartTime));
                regexText = new Regex("zplace");
                docText = regexText.Replace(docText,  customerStartingPlace.Id==14&&booking.Excursion.Destinations[0].Id==5?"Εθνική Τράπεζα": customerStartingPlace.Details);
                regexText = new Regex("zsynodos");
                docText = regexText.Replace(docText, "");
                regexText = new Regex("zcity");
                docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Name);
                regexText = new Regex("zgostart");
                docText = regexText.Replace(docText, (booking.Excursion.ExcursionType.Category == Model.Enums.ExcursionTypeEnum.Group) ? ((customerStartingPlace.Id < 0) ? "" : customerStartingPlace.ReturnTime) : ((customerStartingPlace.Id < 0) ? "" : customerStartingPlace.StartTime));
                regexText = new Regex("zreturnstart");
                docText = regexText.Replace(docText, customerStartingPlace.Id == 19 ? "" : (booking.Excursion.ExcursionType.Category == Model.Enums.ExcursionTypeEnum.Group) ? "10:00" : "13:00");
                regexText = new Regex("regexhotel");
                docText = regexText.Replace(docText, reservationWr.HotelName);
                regexText = new Regex("regexroomtype");
                docText = regexText.Replace(docText, reservationWr.RoomTypeName);
                regexText = new Regex("todaydate");
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
                docText = regexText.Replace(docText, "Θεσσαλονίκη");

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                wordDoc.Close();
                //Process.Start(saveAs);
            }
        }

        #endregion Methods

        #region Classes

        public class BookingsPerDay
        {
            #region Constructors

            public BookingsPerDay(DateTime dateTime)
            {
                Bookings = new List<Booking>();
                Date = dateTime;
            }

            #endregion Constructors

            #region Fields

            public List<Booking> Bookings;
            public DateTime Date;

            #endregion Fields
        }

        #endregion Classes
    }
}