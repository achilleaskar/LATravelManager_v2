﻿using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight.Command;
using LATravelManager.Models;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko
{
    public class NewReservation_Bansko_ViewModel : BanskoChilds_BaseViewModel
    {
        #region Constructors

        public NewReservation_Bansko_ViewModel()
        {
            BanskoRepository = new BanskoRepository();
            NewReservationHelper = new NewReservationHelper(RefreshableContext);
            RoomsManager = new RoomsManager();
            //Commands
            ShowFilteredRoomsCommand = new RelayCommand(async () => { await ShowFilteredRoomsAsync(true); }, CanShowFilteredRooms);
            ClearBookingCommand = new RelayCommand(ClearBooking);
            ReadNextLineCommand = new RelayCommand(ReadNextLine);
            AddCustomerCommand = new RelayCommand(AddRandomCustomer);
            AddFromFileCommand = new RelayCommand(AddFromFile);
            BookRoomNoNameCommand = new RelayCommand(MakeNonameReservation, CanMakeNoNameReservation);
            OverBookHotelCommand = new RelayCommand(async () => { await OverBookHotelAsync(); }, CanOverBookHotel);
            PutCustomersInRoomCommand = new RelayCommand(async () => { await PutCustomersInRoomAsync(); }, CanPutCustomersInRoom);
            SaveCommand = new RelayCommand(async () => { await SaveAsync(); }, CanSave);
            CheckOutCommand = new RelayCommand(CheckOut, CanCheckOut);
            AddTransferCommand = new RelayCommand(AddTransfer, CanAddTransfer);
            PrintVoucherCommand = new RelayCommand(PrintVoucher, CanPrintVoucher);
            Payment = new Payment();

            FilteredRoomList = new ObservableCollection<Room>();
        }

        public NewReservation_Bansko_ViewModel(Booking booking)
        {
            //StartingBooking = new Booking(booking);
            //Booking = booking;
            //ReloadCustomers();
            //Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Constructors

        #region Fields

        private readonly string[] ValidateRoomsProperties =
                {
            nameof(BookingWr),nameof(Payment)
        };

        private bool _All = true;

        private ObservableCollection<Hotel> _AvailableHotels;

        private string _BookedMessage = string.Empty;

        private BookingWrapper _Booking;

        private string _ErrorsInCanAddReservationToRoom = string.Empty;

        private string _ErrorsInDatagrid = string.Empty;

        private ObservableCollection<Room> _FilteredRoomList;

        private bool _HB = false;

        private ObservableCollection<Hotel> _Hotels;

        private int _Line;

        private bool _OnlyStay = false;

        private Payment _Payment = new Payment();

        private ObservableCollection<RoomType> _RoomTypes;

        private Customer _SelectedCustomer;

        private int _SelectedHotelIndex;

        private Payment _SelectedPayment;

        private Room _SelectedRoom;

        private int _SelectedRoomType;

        private ObservableCollection<StartingPlace> _StartingPlaces;
        private ObservableCollection<User> _Users;
        private object Id;

        #endregion Fields

        #region Properties

        public RelayCommand AddCustomerCommand { get; set; }

        public RelayCommand AddFromFileCommand { get; set; }

        public RelayCommand AddTransferCommand { get; set; }

        public bool All
        {
            get => _All;

            set
            {
                if (_All == value)
                {
                    return;
                }

                _All = value;
                RaisePropertyChanged();
            }
        }

        public bool AreBookingDataValid
        {
            get
            {
                foreach (string property in ValidateRoomsProperties)
                {
                    ErrorsInDatagrid = GetBookingDataValidationError(property);
                    if (ErrorsInDatagrid != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public ObservableCollection<Hotel> AvailableHotels
        {
            get => _AvailableHotels;

            set
            {
                if (_AvailableHotels == value)
                {
                    return;
                }

                _AvailableHotels = value;
                RaisePropertyChanged();
            }
        }

        public BanskoRepository BanskoRepository { get; set; }

        public string BookedMessage
        {
            get
            {
                return _BookedMessage;
            }

            set
            {
                if (_BookedMessage == value)
                {
                    return;
                }

                _BookedMessage = value;
                RaisePropertyChanged();
            }
        }

        public BookingWrapper BookingWr
        {
            get => _Booking;

            set
            {
                if (_Booking == value)
                {
                    return;
                }

                _Booking = value;
                _Booking.PropertyChanged += Booking_PropertyChanged;
                RaisePropertyChanged();
            }
        }

        public RelayCommand BookRoomNoNameCommand { get; set; }

        public RelayCommand CheckOutCommand { get; set; }

        public RelayCommand ClearBookingCommand { get; set; }

        public string ErrorsInCanAddReservationToRoom
        {
            get
            {
                return _ErrorsInCanAddReservationToRoom;
            }

            set
            {
                if (_ErrorsInCanAddReservationToRoom == value)
                {
                    return;
                }

                _ErrorsInCanAddReservationToRoom = value;
                RaisePropertyChanged();
            }
        }

        public string ErrorsInDatagrid
        {
            get => _ErrorsInDatagrid;

            set
            {
                if (_ErrorsInDatagrid == value)
                {
                    return;
                }

                _ErrorsInDatagrid = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Room> FilteredRoomList
        {
            get => _FilteredRoomList;

            set
            {
                if (_FilteredRoomList == value)
                {
                    return;
                }

                _FilteredRoomList = value;
                RaisePropertyChanged();
            }
        }

        public bool HB
        {
            get
            {
                return _HB;
            }

            set
            {
                if (_HB == value)
                {
                    return;
                }

                _HB = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Hotel> Hotels
        {
            get => _Hotels;

            set
            {
                if (_Hotels == value)
                {
                    return;
                }

                _Hotels = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAnyReservationResSelected => SelectedCustomer != null && SelectedCustomer.Reservation != null;

        public int Line
        {
            get => _Line;

            set
            {
                if (_Line == value)
                {
                    return;
                }

                _Line = value;
                RaisePropertyChanged();
            }
        }

        public NewReservationHelper NewReservationHelper { get; set; }

        private int NumOfSelectedCustomers;

        public bool AreCustomersLeft
        {
            get
            {
                bool areAnyUnhandled = false;
                foreach (Customer c in BookingWr.Customers)
                {
                    if (!c.Handled && !areAnyUnhandled)
                    {
                        areAnyUnhandled = true;
                    }
                    if (c.IsSelected)
                    {
                        NumOfSelectedCustomers++;
                    }
                }
                return areAnyUnhandled && NumOfSelectedCustomers == 0;
            }
        }

        public bool OnlyStay
        {
            get
            {
                return _OnlyStay;
            }

            set
            {
                if (_OnlyStay == value)
                {
                    return;
                }

                _OnlyStay = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OverBookHotelCommand { get; set; }
        public ObservableCollection<Partner> Partners { get; set; }

        public Payment Payment
        {
            get => _Payment;

            set
            {
                if (_Payment == value)
                {
                    return;
                }

                _Payment = value;
                RaisePropertyChanged();
            }
        }

        public bool PhoneMissing
        {
            get
            {
                foreach (Customer customer in BookingWr.Customers)
                {
                    if (customer.Tel != null && (customer.Tel.Length > 0 || BookingWr.IsPartners))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public RelayCommand PrintVoucherCommand { get; set; }
        public RelayCommand PutCustomersInRoomCommand { get; set; }
        public RelayCommand ReadNextLineCommand { get; set; }
        public BanskoRepository RefreshableContext { get; set; }
        public RoomsManager RoomsManager { get; set; }

        public ObservableCollection<RoomType> RoomTypes
        {
            get => _RoomTypes;

            set
            {
                if (_RoomTypes == value)
                {
                    return;
                }

                _RoomTypes = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; set; }

        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;

            set
            {
                if (_SelectedCustomer == value)
                {
                    return;
                }

                _SelectedCustomer = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsAnyReservationResSelected));
            }
        }

        public Excursion SelectedExcursion { get; set; }

        public int SelectedHotelIndex
        {
            get => _SelectedHotelIndex;

            set
            {
                if (_SelectedHotelIndex == value)
                {
                    return;
                }

                _SelectedHotelIndex = value;
                RaisePropertyChanged();
            }
        }

        public Payment SelectedPayment
        {
            get => _SelectedPayment;

            set
            {
                if (_SelectedPayment == value)
                {
                    return;
                }

                _SelectedPayment = value;
                RaisePropertyChanged();
            }
        }

        public Room SelectedRoom
        {
            get => _SelectedRoom;

            set
            {
                if (_SelectedRoom == value)
                {
                    return;
                }

                _SelectedRoom = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedRoomType
        {
            get => _SelectedRoomType;

            set
            {
                if (_SelectedRoomType == value)
                {
                    return;
                }

                _SelectedRoomType = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowFilteredRoomsCommand { get; set; }

        public BookingWrapper StartingBooking { get; set; }

        public ObservableCollection<StartingPlace> StartingPlaces
        {
            get
            {
                return _StartingPlaces;
            }

            set
            {
                if (_StartingPlaces == value)
                {
                    return;
                }

                _StartingPlaces = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<User> Users
        {
            get => _Users;

            set
            {
                if (_Users == value)
                {
                    return;
                }

                _Users = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id)
        {
            try
            {
                var booking = id > 0
                    ? await BanskoRepository.GetByIdAsync<Booking>(id)
                    : CreateNewBooking();

                Id = id;

                InitializeBooking(booking);

                SelectedExcursion = await BanskoRepository.GetByIdAsync<Excursion>(2);

                await ResetAllRefreshableDataASync();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                IsLoaded = true;
            }
        }

        public override async Task ReloadAsync()
        {
            await ResetAllRefreshableDataASync();
        }

        public void ReloadCustomers()// kalo tha itan na fygei
        {
            int counter = 0;

            foreach (Reservation reservation in BookingWr.ReservationsInBooking)
            {
                foreach (Customer customer in reservation.CustomersList)
                {
                    if (!BookingWr.Customers.Contains(customer))
                    {
                        BookingWr.Customers.Add(customer);
                    }
                }
            }
            foreach (Reservation reservation in BookingWr.ReservationsInBooking)
            {
                counter++;

                foreach (Customer customer in reservation.CustomersList)
                {
                    customer.Handled = true;
                    if (reservation.ReservationType == Reservation.ReservationTypeEnum.Overbooked)
                    {
                        customer.RoomNumber = counter + "-OB";
                    }
                    else if (reservation.ReservationType == Reservation.ReservationTypeEnum.Normal)
                    {
                        customer.RoomNumber = counter.ToString();
                    }
                    else if (reservation.ReservationType == Reservation.ReservationTypeEnum.Noname)
                    {
                        customer.RoomNumber = counter + "-NN";
                    }
                    else if (reservation.ReservationType == Reservation.ReservationTypeEnum.Transfer)
                    {
                        customer.RoomNumber = "TRNS-" + counter;
                    }
                    if (counter % 2 == 0)
                    {
                        customer.RoomColor = new SolidColorBrush(System.Windows.Media.Colors.LightPink);
                    }
                    else
                    {
                        customer.RoomColor = new SolidColorBrush(System.Windows.Media.Colors.LightBlue);
                    }
                }
            }
            if (BookingWr.ReservationsInBooking.Count == 1 && BookingWr.ReservationsInBooking[0].CustomersList.Count > 0)
            {
                SelectedCustomer = BookingWr.ReservationsInBooking[0].CustomersList[0];
            }
        }

        public async Task ShowFilteredRoomsAsync(bool reset = true)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                if (reset)
                {
                    await ResetAllRefreshableDataASync();
                }

                AvailableHotels = await RoomsManager.GetAllAvailableRooms(RefreshableContext, BookingWr.CheckIn, BookingWr.CheckOut, SelectedExcursion, unSavedBooking: BookingWr.Model);
                FilteredRoomList = new ObservableCollection<Room>();
                int allotmentDays = 0;
                bool addThis = false;

                foreach (Hotel hotel in AvailableHotels)
                {
                    if (SelectedHotelIndex == 0 || hotel.Id == Hotels[SelectedHotelIndex - 1].Id)
                    {
                        foreach (Room room in hotel.Rooms)
                        {
                            allotmentDays = 0;
                            addThis = true;
                            for (int i = 0; i < room.PlanDailyInfo.Count; i++)
                            {
                                addThis &= room.PlanDailyInfo[i].RoomState == RoomStateEnum.Available || room.PlanDailyInfo[i].RoomState == RoomStateEnum.MovaBleNoName || room.PlanDailyInfo[i].RoomState == RoomStateEnum.Allotment;
                                if (room.PlanDailyInfo[i].IsAllotment)
                                {
                                    allotmentDays++;
                                }
                            }
                            //todelete
                            if (addThis)
                            {
                                if (allotmentDays > 0)
                                {
                                    if (room.PlanDailyInfo.Count != allotmentDays)
                                    {
                                        //room.IsAllotment = true;
                                        room.LocalNote = "Allotment" + ((allotmentDays == 1) ? " η 1 μέρα" : (" οι " + allotmentDays + " ημέρες."));
                                    }
                                    else
                                    {
                                        // room.IsAllotment = true;
                                        room.LocalNote = "Allotment";
                                    }
                                }
                                else
                                {
                                    // room.IsAllotment = false;
                                    room.LocalNote = "";
                                }
                                FilteredRoomList.Add(room);
                            }
                            else
                            {
                            }
                        }
                    }
                }
                //TODO an mporei na fygei
                RaisePropertyChanged(nameof(FilteredRoomList));

                FilteredRoomList = new ObservableCollection<Room>(FilteredRoomList.OrderBy(f => f.RoomType.MinCapacity).ToList());
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilteredRoomList);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription(nameof(Hotel));
                view.GroupDescriptions.Add(groupDescription);
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
            }
        }

        private void AddFromFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Document", // Default file name
                DefaultExt = ".txt", // Default file extension
                Filter = "Excel Documents (.xlsx)|*.xlsx" // Filter files by extension
            };
            string localError = string.Empty;
            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string fileName = dlg.FileName;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                    {
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringTable sst = sstpart.SharedStringTable;

                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        Worksheet sheet = worksheetPart.Worksheet;

                        IEnumerable<Cell> cells = sheet.Descendants<Cell>();
                        IEnumerable<Row> rows = sheet.Descendants<Row>();

                        Console.WriteLine("Row count = {0}", rows.LongCount());
                        Console.WriteLine("Cell count = {0}", cells.LongCount());

                        int i = 0;
                        int rowNum = 0;
                        Customer tmpCustomer;
                        // Or... via each row
                        foreach (Row row in rows)
                        {
                            rowNum++;
                            i = 0;
                            bool addHim = true;
                            tmpCustomer = new Customer { StartingPlace = "Θεσσαλονίκη", Price = 70 };
                            foreach (Cell c in row.Elements<Cell>())
                            {
                                i++;
                                switch (i)
                                {
                                    case 1:
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            tmpCustomer.Name = sst.ChildElements[ssid].InnerText;
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case 2:
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            tmpCustomer.Surename = sst.ChildElements[ssid].InnerText;
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case 3:
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            DateTime.TryParse(sst.ChildElements[ssid].InnerText, out DateTime resultDate);
                                            tmpCustomer.DOB = resultDate;
                                        }
                                        else if (c.CellValue != null && int.TryParse(c.CellValue.InnerText, out int tmp))
                                        {
                                            tmpCustomer.DOB = DateTime.FromOADate(tmp);
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case 4:
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            tmpCustomer.PassportNum = sst.ChildElements[ssid].InnerText;
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;
                                }
                            }

                            //  localError = tmpCustomer.Error;

                            if (!string.IsNullOrEmpty(localError))
                            {
                                addHim = false;
                            }

                            if (SelectedExcursion.DOBNeeded && (tmpCustomer.DOB == null || tmpCustomer.DOB.Year < 1918))
                            {
                                addHim = false;
                            }
                            if (SelectedExcursion.PassportNeeded && tmpCustomer.PassportNum.Length < 7)
                            {
                                addHim = false;
                            }
                            if (addHim)
                            {
                                BookingWr.Customers.Add(tmpCustomer);
                            }
                            else
                            {
                                Console.Write("Failed To ADD ROW {0}", rowNum);
                            }
                        }
                    }
                }
            }
        }

        private void AddRandomCustomer()
        {
            BookingWr.Customers.Add(NewReservationHelper.CreateRandomCustomer());
        }

        private void AddTransfer()
        {
            NewReservationHelper.AddTransfer(BookingWr, All);
            if (All)
            {
                FilteredRoomList.Clear();
            }
            else
            {
                ShowFilteredRoomsCommand.Execute(null);
            }
        }

        private void Booking_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //TODO mporei na mporei na ginei kalytera vazontas to mesa sto checkin
            //episis na to kanw na vazei ws epistrofi thn epomenh kyriakh ektos apo xristugenna
            if (e.PropertyName == nameof(BookingWr.CheckIn) || e.PropertyName == nameof(BookingWr.CheckOut))
            {
                FilteredRoomList.Clear();
            }
        }

        private bool CanAddTransfer()
        {//TODO xrizei veltiwshs
            if (!BookingWr.AreDatesValid())
            {
                ErrorsInCanAddReservationToRoom = "Παρακαλώ επιλέξτε Ημερομηνίες";
                return false;
            }
            if (AreCustomersLeft && !All)
            {
                ErrorsInCanAddReservationToRoom = "Παρακαλώ επιλέξτε Πελάτες";
                return false;
            }

            if (ValidateBooking() != null)
            {
                return false;
            }

            return true;
        }

        private bool CanCheckOut()
        {
            return SelectedCustomer != null && SelectedCustomer.Handled;
        }

        private bool CanMakeNoNameReservation()
        {//TODO tsekare to opws kai ola ta TODO
            if (SelectedRoomType <= 0 || (SelectedCustomer == null && !All) || FilteredRoomList.Count == 0 || !AreBookingDataValid)
            {
                return false;
            }
            return true;
        }

        private bool CanOverBookHotel()
        {
            return SelectedHotelIndex > 0 && SelectedRoomType > 0 && AreBookingDataValid && (SelectedCustomer != null || All);
        }

        private bool CanPrintVoucher()
        {
            return BookingWr.Id > 0;
        }

        private bool CanPutCustomersInRoom()
        {
            if (!BookingWr.AreDatesValid())
            {
                ErrorsInCanAddReservationToRoom = "Παρακαλώ επιλέξτε Ημερομηνίες";
                return false;
            }
            if (AreCustomersLeft && !All)
            {
                ErrorsInCanAddReservationToRoom = "Παρακαλώ επιλέξτε Πελάτες";
                return false;
            }
            if (SelectedRoom == null)
            {
                ErrorsInCanAddReservationToRoom = "Παρακαλώ επιλέξτε δωμάτιο";
                return false;
            }
            if (NumOfSelectedCustomers > SelectedRoom.RoomType.MaxCapacity)
            {
                ErrorsInCanAddReservationToRoom = "Οι πελάτες δεν χωράνε στο επιλεγμένο δωμάτιο";
                return false;
            }
            if (ValidateBooking() != null)
            {
                return false;
            }
            return true;
        }

        private bool CanSave()
        {
            return AreBookingDataValid && ValidateReservations() == null;
        }

        private bool CanShowFilteredRooms()
        {
            return SelectedHotelIndex >= 0 && SelectedRoomType >= 0 && BookingWr.AreDatesValid();
        }

        private void CheckOut()//να το δω
        {//thelei veltiwsh
            List<Reservation> toRemove = new List<Reservation>();
            foreach (Customer c in BookingWr.Customers)
            {
                if (c.IsSelected && c.Handled)
                {
                    foreach (Reservation r in BookingWr.ReservationsInBooking)
                    {
                        r.CustomersList.Remove(r.CustomersList.Single(x => x.Id == c.Id));
                    }
                    c.Handled = false;
                    c.RoomColor = new SolidColorBrush(System.Windows.Media.Colors.Green);
                    c.Reservation = null;
                    c.HotelName = "KENO";
                    c.RoomTypeName = "KENO";
                    c.RoomNumber = "OXI";
                }
            }
            foreach (var r in BookingWr.ReservationsInBooking)
            {
                if (r.CustomersList.Count == 0)
                {
                    toRemove.Add(r);
                }
            }
            foreach (Reservation r in toRemove)
            {
                BookingWr.ReservationsInBooking.Remove(r);
            }
        }

        private void ClearBooking()
        {
            DocumentsManagement dm = new DocumentsManagement(RefreshableContext);

            dm.PrintAllBookings();

            BookingWr = new BookingWrapper(CreateNewBooking());
            FilteredRoomList.Clear();
            Payment = new Payment();
            StartingBooking = null;
            BookedMessage = string.Empty;
            HB = false;
            OnlyStay = false;
        }

        private Booking CreateNewBooking()
        {
            return new Booking { Excursion = SelectedExcursion, CheckIn = new DateTime(2018, 12, 28), CheckOut = new DateTime(2018, 12, 30), User = StaticResources.User };
        }

        private string GetBookingDataValidationError(string propertyName)
        {
            string error = null;

            //Reservation.OnPropertyChanged("CanAddRows");
            switch (propertyName)
            {
                case nameof(BookingWr):
                    error = ValidateBooking();
                    break;

                case nameof(Payment):
                    error = ValidatePayment();
                    break;

                case nameof(BookingWr.ReservationsInBooking):
                    error = ValidateReservations();
                    break;
            }
            return error;
        }

        private void InitializeBooking(Booking booking)
        {
            BookingWr = new BookingWrapper(booking);
            BookingWr.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = BanskoRepository.HasChanges();
                }
            };
        }

        private async void MakeNonameReservation()
        {
            NewReservationHelper.MakeNonameReservation(BookingWr, await BanskoRepository.GetByIdAsync<RoomType>(RoomTypes[SelectedRoomType - 1].Id), HB, All, OnlyStay);

            if (All)
            {
                FilteredRoomList.Clear();
            }
            else
            {
                ShowFilteredRoomsCommand.Execute(null);
            }
        }

        private async Task OverBookHotelAsync()
        {
            await NewReservationHelper.OverBookHotelAsync(
                BookingWr, await BanskoRepository.GetByIdAsync<Hotel>(Hotels[SelectedHotelIndex - 1].Id), await BanskoRepository.GetByIdAsync<RoomType>(RoomTypes[SelectedRoomType - 1].Id), All, OnlyStay, HB);
            if (All)
            {
                FilteredRoomList.Clear();
            }
            else
            {
                ShowFilteredRoomsCommand.Execute(null);
            }
        }

        private void PrintVoucher()
        {
            using (DocumentsManagement vm = new DocumentsManagement(RefreshableContext))
            {
                vm.PrintSingleBookingVoucher(BookingWr);
            }
        }

        private async Task PutCustomersInRoomAsync()
        {
            NewReservationHelper.PutCustomersInRoomAsync(BookingWr,await BanskoRepository.GetByIdAsync<Room>(SelectedRoom.Id), All, OnlyStay, HB);
            if (All)
            {
                FilteredRoomList.Clear();
            }
            else
            {
                ShowFilteredRoomsCommand.Execute(false);
            }
        }

        private void ReadNextLine()
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.xlsx";
            if (File.Exists(fileName))
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                    {
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringTable sst = sstpart.SharedStringTable;

                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        Worksheet sheet = worksheetPart.Worksheet;

                        IEnumerable<Cell> cells = sheet.Descendants<Cell>();
                        IEnumerable<Row> rows = sheet.Descendants<Row>();

                        Customer tmpCustomer;
                        int i = 0;
                        tmpCustomer = new Customer();
                        int rowNum = 0;
                        if (rows.Count() > Line)
                        {
                            Row row = rows.ElementAt(Line);
                            foreach (Cell c in row.Elements<Cell>())
                            {
                                i++;
                                switch (c.CellReference.ToString().Substring(0, 1))
                                {
                                    case "A":
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            tmpCustomer.Name = sst.ChildElements[ssid].InnerText;
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case "B":
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            tmpCustomer.Surename = sst.ChildElements[ssid].InnerText;
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case "C":
                                        if (c.CellValue != null)
                                        {
                                            tmpCustomer.Tel = c.CellValue.Text.ToString();
                                            if (tmpCustomer.Tel.Length != 10)
                                            {
                                                if (tmpCustomer.Tel == "4" || tmpCustomer.Tel.Length < 10)
                                                {
                                                    tmpCustomer.PriceString = tmpCustomer.Tel;
                                                    tmpCustomer.Tel = string.Empty;
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case "D":
                                        if (c.CellValue != null)
                                        {
                                            if (tmpCustomer.Price < 1 && float.TryParse(c.CellValue.Text, out float price))
                                            {
                                                tmpCustomer.Price = (float)Math.Round(price, 2); ;
                                            }
                                            if (tmpCustomer.Price <= 0)
                                            {
                                            }
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;
                                }
                            }
                            BookingWr.Customers.Add(tmpCustomer);
                            Line++;
                        }
                        else
                        {
                        }
                    }
                }
            BookingWr.CalculateRemainingAmount();
        }

        private async Task ResetAllRefreshableDataASync()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                RefreshableContext = new BanskoRepository();
                RoomsManager = new RoomsManager();

                RoomTypes = new ObservableCollection<RoomType>(await RefreshableContext.GetAllAsync<RoomType>());
                StartingPlaces = new ObservableCollection<StartingPlace>(await RefreshableContext.GetAllAsyncSortedByName<StartingPlace>());
                Users = new ObservableCollection<User>(await RefreshableContext.GetAllAsyncSortedByName<User>());
                Partners = new ObservableCollection<Partner>(await RefreshableContext.GetAllAsyncSortedByName<Partner>());
                Hotels = new ObservableCollection<Hotel>(await RefreshableContext.GetAllHotelsInBanskoAsync());
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
            }
        }

        private async Task SaveAsync()
        {
            BookingWr = await NewReservationHelper.SaveAsync(Payment, BookingWr, StartingBooking);
            FilteredRoomList.Clear();
            Payment = new Payment();
            BookedMessage = "H κράτηση αποθηκέυτηκε επιτυχώς";
        }

        private void SetupCollections()
        {
        }

        //private void ReloadViewModel()
        //{
        //    UOW.Reload();
        //    SelectedExcursion = UOW.GenericRepository.GetById<Excursion>(SelectedExcursion.Id);
        //}
        private string ValidateBooking()
        {
            string localError = string.Empty;
            if (BookingWr.Customers.Count <= 0)
            {
                return "Προσθέστε Πελάτες!";
            }

            if (BookingWr.IsPartners)
            {
                if (BookingWr.NetPrice <= 0)
                {
                    return "Δέν έχετε ορίσει ΝΕΤ τιμή!";
                }

                if (BookingWr.Partner == null)
                {
                    return "Δέν έχετε επιλέξει συνεργάτη";
                }
            }
            foreach (Customer customer in BookingWr.Customers)
            {
                // localError = customer.Error;
                if (!string.IsNullOrEmpty(localError))
                {
                    return localError;
                }
            }
            if (Payment.Amount > BookingWr.Remaining)
            {
                return "Το ποσό υπερβαίνει το υπόλοιπο της κράτησης";
            }

            if (PhoneMissing && !BookingWr.IsPartners)
            {
                return "Παρακαλώ προσθέστε έστω έναν αριθμό τηλεφώνου!";
            }

            return null;
        }

        private string ValidatePayment()
        {
            if (Payment.Amount < 0)
            {
                return "Το ποσό πληρωμής δεν μπορεί να είναι αρνητικό";
            }
            if (Payment.Amount > BookingWr.Remaining)
            {
                return "Το ποσό πληρωμής υπερβαίνει το υπολυπόμενο ποσό";
            }
            return null;
        }

        private string ValidateReservations()
        {
            foreach (Customer customer in BookingWr.Customers)
            {
                if (!customer.Handled)
                {
                    return "Παρακαλώ τοποθετήστε όλους τους πελάτες σε δωμάτια";
                }
            }
            return null;
        }

        #endregion Methods
    }
}