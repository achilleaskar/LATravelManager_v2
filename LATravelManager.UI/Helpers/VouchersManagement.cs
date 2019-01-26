using LATravelManager.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xceed.Wpf.Toolkit;

namespace LATravelManager.UI.Helpers
{
    public class VouchersManagement : IDisposable
    {
        private string folderNameVouchers;

        public UnitOfWork UOW
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UnitOfWork>(Definitions.UnitOfWorkKey);
            }
        }

        public string CreateFolder(DateTime date, string folderName, string city)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + folderName + city + @"\" + DateTime.Now.ToString("MMMM") + @"\" + date.ToString("dd-MM-yy");

            Directory.CreateDirectory(folder);

            return folder;
        }

        public void Dispose()
        {
        }

        public void PrintSingleBookingVoucher(Booking booking)
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
                PrintVoucher(booking);
            }
        }

        public string PrintVoucher(Booking booking)
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

                    CreateWordVoucher(folderNameVouchers + VoucherFilename, res, booking);

                    VoucherDir = folderNameVouchers + VoucherFilename;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return "";
        }

        private void CreateWordVoucher(string saveAs, Reservation reservation, Booking booking)
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
            StartingPlace customerStartingPlace = UOW.GenericRepository.Get<StartingPlace>(filter: x => x.Name == c.StartingPlace).FirstOrDefault();

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
    }
}