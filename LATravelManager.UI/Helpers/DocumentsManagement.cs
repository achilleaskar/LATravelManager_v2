using DocumentFormat.OpenXml.Packaging;
using LATravelManager.Models;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace LATravelManager.UI.Helpers
{
    public class DocumentsManagement : IDisposable
    {
        private string folderNameVouchers;

        public DocumentsManagement(GenericRepository context)
        {
            Context = context;
        }

        public GenericRepository Context { get; }
        private readonly CultureInfo culture = new CultureInfo("el-GR");

        public string CreateFolder(DateTime date, string folderName, string city)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + folderName + city + @"\" + DateTime.Now.ToString("MMMM") + @"\" + date.ToString("dd-MM-yy");

            Directory.CreateDirectory(folder);

            return folder;
        }

        public void Dispose()
        {
        }

        public async Task PrintSingleBookingVoucher(BookingWrapper booking)
        {
            foreach (var res in booking.ReservationsInBooking)
            {
                if (res.ReservationType == Reservation.ReservationTypeEnum.Noname)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                }
                else if (res.ReservationType == Reservation.ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}
                else if (res.ReservationType == Reservation.ReservationTypeEnum.Transfer)
                {
                    MessageBox.Show("Η κράτηση είναι TRANSFER");
                }
            }
           await PrintVoucher(booking);
        }

        public async Task<string> PrintVoucher(BookingWrapper booking)
        {
            var VoucherFilename = string.Empty;
            var inputPath = AppDomain.CurrentDomain.BaseDirectory;
            var outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string VoucherDir = string.Empty;

            foreach (var res in booking.ReservationsInBooking)
            {
                if (res.ReservationType == Reservation.ReservationTypeEnum.Noname)
                {
                    MessageBox.Show("Παρακαλώ τοποθετήστε τα NO NAME");
                }
                else if (res.ReservationType == Reservation.ReservationTypeEnum.NoRoom)
                {
                    MessageBox.Show("Error");
                }
                //else if (res.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                //{
                //    MessageBox.Show("Παρακαλώ τοποθετήστε τα OVER");
                //}

                try
                {
                    if (booking.IsPartners)
                    {
                        VoucherFilename = string.Format(@"\{0}_{1}_{2}_{3}_Voucher.docx", res.CustomersList[0].Surename, res.HotelName, res.RoomTypeName, booking.Partner.Name);
                    }
                    else
                    {
                        VoucherFilename = string.Format(@"\{0}_{1}_{2}_Voucher.docx", res.CustomersList[0].Surename, res.HotelName, res.RoomTypeName);
                    }
                    Directory.CreateDirectory(outputpath + @"\Vouchers");
                    folderNameVouchers = CreateFolder(res.CheckIn, @"\Vouchers\", booking.Excursion.Name);

                    await CreateWordVoucher(folderNameVouchers + VoucherFilename, res, booking);

                    VoucherDir = folderNameVouchers + VoucherFilename;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return "";
        }

        private async Task CreateWordVoucher(string saveAs, Reservation reservation, BookingWrapper booking)
        {
            string fileName;

            if (booking.IsPartners)
                fileName = @"Sources\Voucher_afirmo.docx";
            else if (booking.User.BaseLocation == User.GrafeiaXriston.Larisas)
                fileName = @"Sources\Voucher_enfirmo_larissas.docx";
            else if (booking.User.BaseLocation == User.GrafeiaXriston.Thessalonikis)
                fileName = @"Sources\Voucher_enfirmo_thess.docx";
            else
                fileName = @"Sources\Voucher_enfirmo_thess.docx";
            File.Copy(fileName, saveAs, true);
            var c = reservation.CustomersList[0];
            StartingPlace customerStartingPlace = await Context.GetByNameAsync<StartingPlace>(c.StartingPlace);

            if (customerStartingPlace == null || customerStartingPlace.Id == 19)
            {
                customerStartingPlace = new StartingPlace
                {
                    Details = reservation.CustomersList[0].StartingPlace,
                    Name = reservation.CustomersList[0].StartingPlace,
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
                // var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"VoucherImages\";
                var regexText = new Regex("regexorderno");
                docText = regexText.Replace(docText, reservation.Id.ToString());
                regexText = new Regex("regexresno");
                docText = regexText.Replace(docText, reservation.Id.ToString());
                regexText = new Regex("regexdate");
                docText = regexText.Replace(docText, DateTime.Today.ToString("dd/MM/yyyy"));
                regexText = new Regex("regexhotel");
                docText = regexText.Replace(docText, reservation.HotelName);
                regexText = new Regex("regexaddress");
                docText = regexText.Replace(docText, reservation.Room == null || string.IsNullOrEmpty(reservation.Room.Hotel.Address) ? "" : reservation.Room.Hotel.Address);
                regexText = new Regex("regexagency");
                docText = regexText.Replace(docText, !booking.IsPartners ? "LA TRAVEL" : booking.Partner.Name);
                regexText = new Regex("regexcity");
                docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Name);
                regexText = new Regex("regextel");
                docText = regexText.Replace(docText, (reservation.Room == null || reservation.Room.Hotel.Tel == "0" || reservation.Room.Hotel.Tel == null) ? "" : reservation.Room.Hotel.Tel);
                regexText = new Regex("regexnames");
                docText = regexText.Replace(docText, reservation.Names);
                regexText = new Regex("regexcheckin");
                docText = regexText.Replace(docText, reservation.CheckIn.ToString("dd/MM/yyyy"));
                regexText = new Regex("regexcheckout");
                docText = regexText.Replace(docText, reservation.CheckOut.ToString("dd/MM/yyyy"));
                regexText = new Regex("regexroomtype");
                docText = regexText.Replace(docText, reservation.RoomTypeName);
                //regexText = new Regex("regexpayment");
                //docText = regexText.Replace(docText, reservation.HotelName);
                regexText = new Regex("regexnotes");
                docText = regexText.Replace(docText, "");
                regexText = new Regex("zdayscount");
                docText = regexText.Replace(docText, reservation.DaysCount + ((reservation.DaysCount == 1) ? " Hμέρα" : " Hμέρες"));
                regexText = new Regex("zdates");
                docText = regexText.Replace(docText, reservation.Dates);
                regexText = new Regex("zdate");
                docText = regexText.Replace(docText, reservation.CheckIn.ToString("dd/MM"));
                regexText = new Regex("zlocation");
                docText = regexText.Replace(docText, reservation.CustomersList[0].StartingPlace);
                regexText = new Regex("ztime");
                docText = regexText.Replace(docText, (customerStartingPlace.Id < 0) ? "" : customerStartingPlace.StartTime);
                regexText = new Regex("zplace");
                docText = regexText.Replace(docText, customerStartingPlace.Details);
                regexText = new Regex("zsynodos");
                docText = regexText.Replace(docText, "");
                regexText = new Regex("zcity");
                docText = regexText.Replace(docText, booking.Excursion.Destinations[0].Name);
                regexText = new Regex("zgostart");
                docText = regexText.Replace(docText, customerStartingPlace.StartTime);
                regexText = new Regex("zreturnstart");
                docText = regexText.Replace(docText, customerStartingPlace.Id == 19 ? "" : "13:00");
                regexText = new Regex("regexhotel");
                docText = regexText.Replace(docText, reservation.HotelName);
                regexText = new Regex("regexroomtype");
                docText = regexText.Replace(docText, reservation.RoomTypeName);

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                wordDoc.Close();
                //Process.Start(saveAs);
            }
        }

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
                                        if (reservation.ReservationType == Reservation.ReservationTypeEnum.Noname)
                                        {
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.NoNameRoomType.Name;
                                            myWorksheet.Cells["F" + lineNum].Value = "NO NAME";
                                        }
                                        else if (reservation.ReservationType == Reservation.ReservationTypeEnum.Normal)
                                        {
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.Room.RoomType;
                                            myWorksheet.Cells["F" + lineNum].Value = reservation.Room.Hotel.Name;
                                        }
                                        else if (reservation.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                                        {
                                            myWorksheet.Cells["D" + lineNum].Value = reservation.NoNameRoomType.Name;
                                            myWorksheet.Cells["F" + lineNum].Value = reservation.Hotel.Name;
                                        }
                                        else if (reservation.ReservationType == Reservation.ReservationTypeEnum.Transfer)
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
    }

    public class BookingsPerDay
    {
        public BookingsPerDay(DateTime dateTime)
        {
            Bookings = new List<Booking>();
            Date = dateTime;
        }

        public List<Booking> Bookings;
        public DateTime Date;
    }
}