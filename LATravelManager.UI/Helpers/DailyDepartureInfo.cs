using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Repositories;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace LATravelManager.UI.Helpers
{
    public class DailyDepartureInfo : INotifyPropertyChanged
    {
        #region Constructors

        public DailyDepartureInfo(GenericRepository context, int excursionId)
        {
            // PrintListCommand = new RelayCommand<int>(async (obj) => { await PrintListAsync(obj); }, CanPrintList);
            Context = context;
            ExcursionId = excursionId;
        }



        public bool SecondDepart { get; set; }
        //private bool CanPrintList(int arg)
        //{
        //    switch (arg)
        //    {
        //        case 1:
        //            return SelectedGo > 0;

        //        case 4:
        //            return SelectedReturn > 0;

        //        default:
        //            return false;
        //    }
        //}

        //private async Task PrintListAsync(int parameter)
        //{
        //   // //string wbPath = null;
        //// List<string> selectedCities = new List<string>();
        // var sampleFile = AppDomain.CurrentDomain.BaseDirectory;
        // await Task.Delay(0);
        // FileInfo fileInfo = new FileInfo(sampleFile);
        // ExcelPackage p = new ExcelPackage(fileInfo);
        // ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];
        //// int counter = 0;
        //// int customersCount, secondline;
        // List<ReservationWrapper> reservationsThisDay = new List<ReservationWrapper>();
        // List<ReservationWrapper> RestReservations = new List<ReservationWrapper>();

        // switch (parameter)
        // {
        //     case 0:
        //         sampleFile += @"Sources\protypo_aktoploikon.xlsx";
        //         break;

        //     case 1:
        //         sampleFile += @"Sources\protypo_leoforeia.xlsx";
        //         break;

        //     case 3:
        //         sampleFile += @"Sources\protypo_aktoploikon.xlsx";
        //         break;

        //     case 4:
        //         sampleFile += @"Sources\protypo_leoforeia.xlsx";
        //         break;

        //     default:
        //         sampleFile += @"Sources\protypo_leoforeia.xlsx";
        //         break;
        // }

        // switch (parameter)
        // {
        //     case 1:
        //         // IEnumerable<Booking> bookings = await Context.GetAllBookingInPeriod(Date, Date, ExcursionId);

        //         //foreach (CityDepartureInfo city in PerCityDepartureList)
        //         //    if (city.IsChecked)
        //         //    {
        //         //        selectedCities.Add(city.City);
        //         //        foreach (Reservation rese in CollectionsManager.ReservationsCollection)
        //         //            if (rese.From == Date)
        //         //                foreach (Customer cust in rese.Customers)
        //         //                    if (cust.Location == city.City && cust.CustomerHasBusIndex < 2)
        //         //                    {
        //         //                        if (!reservationsThisDay.Contains(rese))
        //         //                            reservationsThisDay.Add(rese);
        //         //                        break;
        //         //                    }
        //         //    }

        //         //foreach (Reservation res in CollectionsManager.ReservationsCollection)
        //         //    if (res.From == Date)
        //         //        if (!reservationsThisDay.Contains(res))
        //         //            RestReservations.Add(res);

        //         //folderNameVouchers = CreateFolder(Date, @"\Vouchers\");
        //         //folderNameInfo = CreateFolder(Date, @"\Ενημερωτικά\");
        //         //di = new DirectoryInfo(folderNameVouchers);
        //         //foreach (FileInfo file in di.GetFiles())
        //         //    file.Delete();
        //         //di = new DirectoryInfo(folderNameInfo);
        //         //foreach (FileInfo file in di.GetFiles())
        //         //    file.Delete();
        //         break;

        //     case 4:
        //         break;

        //     default:
        //         break;
        // }
        // }

        #endregion Constructors

        #region Fields

        //private static readonly string[] ValidatePrintProperties =
        //{
        //    nameof(PerCityDepartureList)
        //};

        private DateTime _Date;

        private ObservableCollection<CityDepartureInfo> _PerCityDepartureList;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public DateTime Date
        {
            get => _Date;
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        //public bool CanPrintLists => ArePrintDataValid;
        public string Dates => ExcursionDate != null ? ExcursionDate.CheckIn.ToString("dd") + "-" + ExcursionDate.CheckOut.ToString("dd/MM") : (Date.ToString("dddd dd/MM") + (SecondDepart ? " Δέυτ." : ""));

        public int SelectedGo, SelectedReturn, TotalGo, TotalReturn;

        public void CalculateTotals()
        {
            SelectedGo = SelectedReturn = TotalGo = TotalReturn = 0;
            foreach (var city in PerCityDepartureList)
            {
                if (city.IsChecked)
                {
                    SelectedGo += city.Going + city.OneDayGo + city.OnlyBusGo + city.OnlyShipGo + city.OnlyStayGo;
                    SelectedReturn += city.Returning + city.OneDayReturn + city.OnlyBusReturn + city.OnlyShipReturn + city.OnlyStayReturn;
                }
                TotalGo += city.Going + city.OneDayGo + city.OnlyBusGo + city.OnlyShipGo + city.OnlyStayGo;
                TotalReturn += city.Returning + city.OneDayReturn + city.OnlyBusReturn + city.OnlyShipReturn + city.OnlyStayReturn;
            }
            OnPropertyChanged(nameof(GoTotals));
            OnPropertyChanged(nameof(ReturnTotals));
        }

        public RelayCommand<int> PrintListCommand { get; set; }
        public string GoTotals => SelectedGo + "/" + TotalGo;
        public string ReturnTotals => SelectedReturn + "/" + TotalReturn;

        private ExcursionDate _ExcursionDate;

        public ExcursionDate ExcursionDate
        {
            get
            {
                return _ExcursionDate;
            }

            set
            {
                if (_ExcursionDate == value)
                {
                    return;
                }
                Date = value.CheckIn;
                _ExcursionDate = value;
            }
        }

        //public bool ArePrintDataValid
        //{
        //    get
        //    {
        //        var tmpError = "";
        //        foreach (var property in ValidatePrintProperties)
        //        {
        //            tmpError = GetPrintError(property);
        //            if (tmpError != null)
        //            {
        //                //ShowDepartureInfoError = tmpError;
        //                return false;
        //            }
        //        }

        //        //ShowDepartureInfoError = "";
        //        return true;
        //    }
        //}
        public ObservableCollection<CityDepartureInfo> PerCityDepartureList
        {
            get => _PerCityDepartureList;
            set
            {
                if (_PerCityDepartureList != value)
                {
                    _PerCityDepartureList = value;
                    OnPropertyChanged(nameof(PerCityDepartureList));
                    PerCityDepartureList.CollectionChanged += PerCityDepartureList_CollectionChanged;
                }
            }
        }

        public GenericRepository Context { get; }
        public int ExcursionId { get; }

        private void PerCityDepartureList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (CityDepartureInfo city in e.OldItems)
                {
                    //Removed items
                    city.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (CityDepartureInfo city in e.NewItems)
                {
                    city.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CityDepartureInfo.IsChecked))
            {
                CalculateTotals();
            }
        }

        #endregion Properties

        //public ICommand PrintListCommand
        //{
        //    get;
        //    private set;
        //}

        //public string GetPath(string fileName)
        //{
        //    var i = 1;
        //    string resultPath;
        //    var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Λίστες";
        //    Directory.CreateDirectory(folder);

        //    resultPath = folder + fileName + ".xlsx";
        //    var fileExtension = ".xlsx";
        //    while (File.Exists(resultPath))
        //    {
        //        i++;
        //        resultPath = folder + fileName + "(" + i + ")" + fileExtension;
        //    }
        //    return resultPath;
        //}

        //public string CreateFolder(DateTime date)
        //{
        //    var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Vouchers\" + StaticResources.City + @"\" + DateTime.Now.ToString("MMMM") + @"\" + date.ToString("dd-MM-yy");

        //    Directory.CreateDirectory(folder);

        //    return folder;
        //}

        //private string _PhoneNumbers;

        //public string PhoneNumbers
        //{
        //    get => _PhoneNumbers;
        //    set
        //    {
        //        if (_PhoneNumbers != value)
        //        {
        //            _PhoneNumbers = value;
        //            OnPropertyChanged(nameof(PhoneNumbers));
        //        }
        //    }
        //}

        //internal void PrintList(int parameter)
        //{
        //    string wbPath = null;
        //    var sampleFile = AppDomain.CurrentDomain.BaseDirectory;

        //    switch (parameter)
        //    {
        //        case 0:
        //            sampleFile += @"Sources\protypo_aktoploikon.xlsx";
        //            break;

        //        case 1:
        //            sampleFile += @"Sources\protypo_leoforeia.xlsx";
        //            break;

        //        case 3:
        //            sampleFile += @"Sources\protypo_aktoploikon.xlsx";
        //            break;

        //        case 4:
        //            sampleFile += @"Sources\protypo_leoforeia.xlsx";
        //            break;
        //    }

        //    ExcelRange modelTable;
        //    int lineNum;
        //    Thread.CurrentThread.CurrentCulture = culture;

        //    var fileInfo = new FileInfo(sampleFile);
        //    var p = new ExcelPackage(fileInfo);
        //    ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];
        //    var ticketsList = new List<Customer>();
        //    int rowsNum;
        //    int customersCount;
        //    var reservationsThisDay = new List<Reservation>();
        //    PhoneNumbers = "";

        //    switch (parameter)
        //    {
        //        case 0:
        //            foreach (Reservation reservation in CollectionsManager.ReservationsCollection)
        //                if (reservation.From == Date)
        //                    reservationsThisDay.Add(reservation);

        //            foreach (CityDeptartureInfo city in PerCityDepartureList)
        //                if (city.IsChecked)
        //                    foreach (Reservation rese in reservationsThisDay)

        //                        foreach (Customer customer in rese.Customers)
        //                            if (customer.Location == city.City && customer.CustomerHasShipIndex < 3)
        //                                ticketsList.Add(customer);
        //            wbPath = GetPath("\\Λίστα αναχώρησης για " + Date.ToString("dd_MM_yy"));

        //            myWorksheet.Cells["A2"].Value = "Αναχώρηση - " + Date.ToString("dd/MM/yyyy");
        //            lineNum = 5;
        //            rowsNum = ticketsList.Count;
        //            myWorksheet.InsertRow(lineNum, rowsNum);

        //            modelTable = myWorksheet.Cells["B5:G" + (rowsNum + 4)];
        //            modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            foreach (Customer customer in ticketsList)
        //            {
        //                myWorksheet.Cells["B" + lineNum].Value = lineNum - 4;
        //                myWorksheet.Cells["B" + lineNum].Style.Border.Left.Style = ExcelBorderStyle.Thin;

        //                myWorksheet.Cells["C" + lineNum].Value = customer;

        //                if (customer.CustomerHasBusIndex < 2)
        //                    myWorksheet.Cells["D" + lineNum].Value = Date.ToShortDateString();
        //                if (customer.CustomerHasBusIndex < 2)
        //                    foreach (Reservation reservation in CollectionsManager.ReservationsCollection)
        //                        foreach (Customer cust in reservation.Customers)
        //                            if (cust.Id == customer.Id)
        //                                myWorksheet.Cells["E" + lineNum].Value = reservation.To.ToShortDateString();

        //                myWorksheet.Cells["F" + lineNum].Value = customer.CustomerPaso == 0 ? "" : customer.CustomerPaso == 1 ? "Φοιτ." : "Πολυτ.";
        //                myWorksheet.Cells["G" + lineNum].Value = customer.Age < 12 ? customer.Age.ToString() : "";
        //                myWorksheet.Cells["G" + lineNum].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                lineNum++;
        //            }

        //            fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
        //            p.SaveAs(fileInfo);
        //            Process.Start(wbPath);
        //            break;

        //        case 1:
        //            foreach (CityDeptartureInfo city in PerCityDepartureList)
        //                if (city.IsChecked)
        //                    foreach (Reservation rese in CollectionsManager.ReservationsCollection)
        //                        if (rese.From == Date)
        //                            foreach (Customer cust in rese.Customers)
        //                                if (cust.Location == city.City && cust.CustomerHasBusIndex < 2)
        //                                {
        //                                    reservationsThisDay.Add(rese);
        //                                    break;
        //                                }

        //            wbPath = GetPath("\\Λίστα Λεωφορείου για Σκιάθο " + Date.ToString("dd_MM_yy"));

        //            myWorksheet.Cells["A2"].Value = "Αναχώρηση - " + Date.ToString("dd/MM/yyyy");
        //            lineNum = 5;
        //            if (reservationsThisDay.Count > 0)
        //            {
        //                foreach (Reservation reservation in reservationsThisDay)
        //                {
        //                    customersCount = -1;
        //                    foreach (Customer customer in reservation.Customers)
        //                    {
        //                        if (customer.CustomerHasBusIndex < 2)
        //                        {
        //                            myWorksheet.InsertRow(lineNum, 1);
        //                            customersCount++;

        //                            myWorksheet.Cells["A" + lineNum].Value = lineNum - 4;
        //                            myWorksheet.Cells["B" + lineNum].Value = reservation.From.Day + "-" + reservation.To.ToString("dd/MM");
        //                            myWorksheet.Cells["C" + lineNum].Value = customer.Name;
        //                            myWorksheet.Cells["D" + lineNum].Value = customer.SureName;
        //                            if (customersCount == 0)
        //                            {
        //                                myWorksheet.Cells["E" + lineNum].Value = reservation.Room.Hotel;
        //                                if (reservation.IsPartner)
        //                                    myWorksheet.Cells["H" + lineNum].Value = (reservation.Partner.ToString().Length > 7) ? reservation.Partner.ToString().Substring(0, 7) : reservation.Partner.ToString();
        //                                else if (reservation.Ypoloipo > 0)
        //                                    myWorksheet.Cells["H" + lineNum].Value = reservation.Ypoloipo + "€";
        //                            }
        //                            else if (customersCount == 1)
        //                                myWorksheet.Cells["E" + lineNum].Value = reservation.Room.RoomType;
        //                            myWorksheet.Cells["E" + lineNum].Style.Border.Bottom.Style = ExcelBorderStyle.None;

        //                            myWorksheet.Cells["F" + lineNum].Value = customer.Location;
        //                            if (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal))
        //                            {
        //                                myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
        //                                PhoneNumbers += customer.Tel + ",";
        //                            }
        //                            else
        //                                myWorksheet.Cells["G" + lineNum].Value = " ";

        //                            lineNum++;
        //                        }
        //                    }
        //                    myWorksheet.Cells["E" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                    if (reservation.Room.Id >= 0)
        //                    {
        //                        PrintVoucher(reservation);
        //                    }
        //                }
        //                PhoneNumbers = PhoneNumbers.Trim(',');
        //                Clipboard.SetText(PhoneNumbers);
        //                lineNum--;
        //                modelTable = myWorksheet.Cells["A5:D" + (lineNum)];
        //                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //                modelTable = myWorksheet.Cells["F5:H" + (lineNum)];
        //                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //                fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
        //                p.SaveAs(fileInfo);
        //                Process.Start(wbPath);
        //            }
        //            else
        //                MessageBox.Show("Δέν υπάρχουν άτομα που να πηγαίνουν τις συγκεκριμένες ημέρες από τις επιλεγμένες πόλεις", "Σφάλμα");
        //            break;

        //        case 3:
        //            foreach (Reservation reservation in CollectionsManager.ReservationsCollection)
        //                if (reservation.To == Date)
        //                    foreach (CityDeptartureInfo city in PerCityDepartureList)
        //                        if (city.IsChecked)
        //                            foreach (Reservation rese in reservationsThisDay)
        //                                foreach (Customer customer in rese.Customers)
        //                                    if (customer.Location == city.City && customer.CustomerHasShipIndex == 0 || customer.CustomerHasShipIndex == 2)
        //                                        ticketsList.Add(customer);

        //            wbPath = GetPath("\\Λίστα Επιστροφής για " + Date.ToString("dd_MM_yy"));

        //            myWorksheet.Cells["A2"].Value = "Επιστροφή - " + Date.ToString("dd/MM/yyyy");
        //            lineNum = 5;
        //            rowsNum = ticketsList.Count;
        //            if (rowsNum > 0)
        //            {
        //                myWorksheet.InsertRow(lineNum, rowsNum);
        //                modelTable = myWorksheet.Cells["B5:G" + (rowsNum + 4)];
        //                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //                foreach (Customer customer in ticketsList)
        //                {
        //                    myWorksheet.Cells["B" + lineNum].Value = lineNum - 4;
        //                    myWorksheet.Cells["B" + lineNum].Style.Border.Left.Style = ExcelBorderStyle.Thin;

        //                    myWorksheet.Cells["C" + lineNum].Value = customer;

        //                    if (customer.CustomerHasShipIndex == 0 || customer.CustomerHasShipIndex == 2)
        //                        myWorksheet.Cells["D" + lineNum].Value = Date.ToShortDateString();

        //                    myWorksheet.Cells["F" + lineNum].Value = customer.CustomerPaso == 0 ? "" : customer.CustomerPaso == 1 ? "Φοιτ." : "Πολυτ.";
        //                    myWorksheet.Cells["G" + lineNum].Value = customer.Age < 12 ? customer.Age.ToString() : "";
        //                    myWorksheet.Cells["G" + lineNum].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                    lineNum++;
        //                }

        //                fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
        //                p.SaveAs(fileInfo);
        //                Process.Start(wbPath);
        //            }
        //            else
        //                MessageBox.Show("Δέν υπάρχουν άτομα που να επιστρέφουν τις συγκεκριμένες ημέρες από Σκιάθο", "Σφάλμα");
        //            break;

        //        case 4:
        //            foreach (CityDeptartureInfo city in PerCityDepartureList)
        //                if (city.IsChecked)
        //                    foreach (Reservation rese in CollectionsManager.ReservationsCollection)
        //                        if (rese.To == Date)
        //                            foreach (Customer cust in rese.Customers)
        //                                if ((cust.Location == city.City || cust.CustomerHasBusIndex == 2) && cust.CustomerHasBusIndex == 0)
        //                                {
        //                                    reservationsThisDay.Add(rese);
        //                                    break;
        //                                }

        //            wbPath = GetPath("\\Λίστα Λεωφορείου επιστροφής από Σκιάθο " + Date.ToString("dd_MM_yy"));

        //            myWorksheet.Cells["A2"].Value = "Επιστροφή - " + Date.ToString("dd/MM/yyyy");
        //            lineNum = 5;
        //            if (reservationsThisDay.Count > 0)
        //            {
        //                foreach (Reservation reservation in reservationsThisDay)
        //                {
        //                    customersCount = -1;
        //                    foreach (Customer customer in reservation.Customers)
        //                    {
        //                        if (customer.CustomerHasBusIndex == 2 || customer.CustomerHasBusIndex == 0)
        //                        {
        //                            myWorksheet.InsertRow(lineNum, 1);
        //                            customersCount++;

        //                            myWorksheet.Cells["A" + lineNum].Value = lineNum - 4;
        //                            myWorksheet.Cells["B" + lineNum].Value = reservation.From.Day + "-" + reservation.To.ToString("dd/MM");
        //                            myWorksheet.Cells["C" + lineNum].Value = customer.Name;
        //                            myWorksheet.Cells["D" + lineNum].Value = customer.SureName;
        //                            if (customersCount == 0)
        //                            {
        //                                myWorksheet.Cells["E" + lineNum].Value = reservation.Room.Hotel;
        //                                if (reservation.IsPartner)
        //                                    myWorksheet.Cells["H" + lineNum].Value = (reservation.Partner.ToString().Length > 7) ? reservation.Partner.ToString().Substring(0, 7) : reservation.Partner.ToString();
        //                            }
        //                            else if (customersCount == 1)
        //                                myWorksheet.Cells["E" + lineNum].Value = reservation.Room.RoomType;
        //                            myWorksheet.Cells["E" + lineNum].Style.Border.Bottom.Style = ExcelBorderStyle.None;

        //                            myWorksheet.Cells["F" + lineNum].Value = customer.Location;
        //                            if (customer.Tel.StartsWith("69", StringComparison.Ordinal) || customer.Tel.StartsWith("+", StringComparison.Ordinal))
        //                                myWorksheet.Cells["G" + lineNum].Value = customer.Tel;
        //                            else
        //                                myWorksheet.Cells["G" + lineNum].Value = " ";

        //                            lineNum++;
        //                        }
        //                    }
        //                    myWorksheet.Cells["E" + (lineNum - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                }
        //                lineNum--;
        //                modelTable = myWorksheet.Cells["A5:D" + (lineNum)];
        //                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //                modelTable = myWorksheet.Cells["F5:H" + (lineNum)];
        //                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //                fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
        //                p.SaveAs(fileInfo);
        //                Process.Start(wbPath);
        //            }
        //            else
        //                MessageBox.Show("Δέν υπάρχουν άτομα που να επιστρέφουν τις συγκεκριμένες ημέρες από Σκιάθο", "Σφάλμα");
        //            break;
        //    }
        //}

        //private string GetPrintError(string propertyName)
        //{
        //    string error = null;
        //    switch (propertyName)
        //    {
        //        case nameof(PerCityDepartureList):
        //            error = ValidatePrintInfo();
        //            break;
        //    }
        //    return error;
        //}

        #region Methods

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion Methods

        //private string ValidatePrintInfo()
        //{
        //    foreach (CityDepartureInfo city in PerCityDepartureList)
        //    {
        //        if (city.IsChecked)
        //        {
        //            return null;
        //        }
        //    }
        //    return "Επιλέξτε τουλάχιστον ένα σημείο αναχώρησης.";
        //}

        //public void PrintVoucher(Reservation res)
        //{
        //    try
        //    {
        //        var filename = string.Format(@"\{0}_{1}_{2}_{3}.docx", res.Room.Hotel, res.Room.RoomType, res.Customers[0].SureName, res.Id);
        //        var inputPath = AppDomain.CurrentDomain.BaseDirectory;
        //        var outputpath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        //        Directory.CreateDirectory(outputpath + @"\Vouchers");
        //        var folderName = CreateFolder(res.From);
        //        CreateWordDocument(inputPath + @"VoucherImages\voucher-" + res.Room.Hotel.Id + ".docx",
        //                            folderName + filename, res);
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Error in process.", "Internal Error");
        //    }
        //}

        //ImagePart GetImagePart(WordprocessingDocument document, string imageName)
        //{
        //    return document.MainDocumentPart.ImageParts
        //        .Where(p => p.Uri.ToString().Contains(imageName)) // or EndsWith
        //        .First();
        //}

        //private void ReplaceImage(string tagName, string imagePath, WordprocessingDocument wordDoc)
        //{
        //    ImagePart imagePart = GetImagePart(wordDoc, "12.png");
        //    var newImageBytes = File.ReadAllBytes(imagePath);// however the image is generated or obtained

        //    using(var writer = new BinaryWriter(imagePart.GetStream()))
        //    {
        //        writer.Write(newImageBytes);
        //    }
        //}

        //private void CreateWordDocument(string fileName, string saveAs, Reservation reservation)
        //{
        //    File.Copy(fileName, saveAs, true);

        //    using (var wordDoc = WordprocessingDocument.Open(saveAs, true))
        //    {
        //        string docText = null;
        //        using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
        //        {
        //            docText = sr.ReadToEnd();
        //        }
        //        // var imagePath = AppDomain.CurrentDomain.BaseDirectory + @"VoucherImages\";
        //        var regexText = new Regex("regexorderno");
        //        docText = regexText.Replace(docText, reservation.Id.ToString());
        //        regexText = new Regex("regexresno");
        //        docText = regexText.Replace(docText, reservation.Id.ToString());
        //        regexText = new Regex("regexissue");
        //        docText = regexText.Replace(docText, DateTime.Today.ToString("dd/MM/yyyy"));
        //        regexText = new Regex("regexhotel");
        //        docText = regexText.Replace(docText, reservation.Room.Hotel.Name);
        //        regexText = new Regex("regexaddress");
        //        docText = regexText.Replace(docText, reservation.Room.Hotel.Comment);
        //        regexText = new Regex("regexbooked");
        //        docText = regexText.Replace(docText, string.IsNullOrEmpty(reservation.Room.Option.OptionNote) ? "LA TRAVEL" : reservation.Room.Option.OptionNote);
        //        regexText = new Regex("regexcity");
        //        docText = regexText.Replace(docText, StaticResources.City);
        //        regexText = new Regex("regextel");
        //        docText = regexText.Replace(docText, reservation.Room.Hotel.Tel == "0" ? "" : reservation.Room.Hotel.Tel);
        //        regexText = new Regex("regexname");
        //        docText = regexText.Replace(docText, reservation.Names);
        //        regexText = new Regex("regexcheckin");
        //        docText = regexText.Replace(docText, reservation.From.ToString("dd/MM/yyyy"));
        //        regexText = new Regex("regexcheckout");
        //        docText = regexText.Replace(docText, reservation.To.ToString("dd/MM/yyyy"));
        //        regexText = new Regex("regexroomtypes");
        //        docText = regexText.Replace(docText, reservation.Room.RoomType.Name);
        //        regexText = new Regex("regexcomment");
        //        docText = regexText.Replace(docText, "");

        //        // imagePath += reservation.Room.Hotel.Id + ".png";

        //        using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
        //        {
        //            sw.Write(docText);
        //        }
        //        wordDoc.Close();

        //        //ReplaceImage("map", imagePath, wordDoc);
        //    }

        //    //MessageBox.Show("Το Αρχείο Δημιουργήθηκε Στην Επιφάνεια Εργασίας.");
        //}
    }
}