using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Models;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class NewReservationGroup_Base : MyViewModelBase
    {
        #region Constructors

        public NewReservationGroup_Base()
        {
            GenericRepository = new GenericRepository();
            NewReservationHelper = new NewReservationHelper(RefreshableContext);
            RoomsManager = new RoomsManager();

            //Commands
            ShowFilteredRoomsCommand = new RelayCommand(async () => { await ShowFilteredRoomsAsync(true); }, CanShowFilteredRooms);
            ClearBookingCommand = new RelayCommand(async () => { await ClearBooking(); });
            ReadNextLineCommand = new RelayCommand(ReadNextLine);
            AddCustomerCommand = new RelayCommand(AddRandomCustomer);
            AddFromFileCommand = new RelayCommand(AddFromFile);

            DeletePaymentCommand = new RelayCommand(DeletePayment, CanDeletePayment);
            UpdateAllCommand = new RelayCommand(async () => { await UpdateAll(); }, CanUpdateAll);

            BookRoomNoNameCommand = new RelayCommand(async () => { await MakeNonameReservation(); }, CanMakeNoNameReservation);
            OverBookHotelCommand = new RelayCommand(async () => { await OverBookHotelAsync(); }, CanOverBookHotel);
            PutCustomersInRoomCommand = new RelayCommand(async () => { await PutCustomersInRoomAsync(); }, CanPutCustomersInRoom);
            AddTransferCommand = new RelayCommand(AddTransfer, CanAddTransfer);

            ManagePartnersCommand = new RelayCommand(ManagePartners);

            SaveCommand = new RelayCommand(async () => { await SaveAsync(); }, CanSave);
            CheckOutCommand = new RelayCommand(() => { CheckOut(); }, CanCheckOut);
            PrintVoucherCommand = new RelayCommand(async () => { await PrintVoucher(); }, CanPrintDoc);
            PrintContractCommand = new RelayCommand(PrintContract, CanPrintDoc);
            Payment = new Payment();
            FilteredRoomList = new ObservableCollection<RoomWrapper>();
            SelectedUserIndex = -1;
        }

        #endregion Constructors

        #region Fields

        private readonly string[] ValidateRoomsProperties =
            {
            nameof(BookingWr),nameof(Payment)
        };

        private bool _All = true;

        private ObservableCollection<HotelWrapper> _AvailableHotels;

        private string _BookedMessage = string.Empty;

        private BookingWrapper _BookingWrapper;

        private string _ErrorsInCanAddReservationToRoom = string.Empty;

        private string _ErrorsInDatagrid = string.Empty;

        private ObservableCollection<RoomWrapper> _FilteredRoomList;

        private bool _HB = false;

        private ObservableCollection<Hotel> _Hotels;

        private int _Line;

        private bool _OnlyStay = false;

        private ObservableCollection<Partner> _Partners;

        private Payment _Payment = new Payment();

        private ObservableCollection<RoomType> _RoomTypes;

        private CustomerWrapper _SelectedCustomer;

        private int _SelectedHotelIndex;

        private int _SelectedPartnerIndex = -1;

        private Payment _SelectedPayment;

        private Room _SelectedRoom;

        private int _SelectedRoomTypeIndex;

        private int _SelectedUserIndex;

        private ObservableCollection<User> _Users;

        private int NumOfSelectedCustomers;
        private ExcursionWrapper _SelectedExcursion;

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

        public bool AreCustomersLeft
        {
            get
            {
                NumOfSelectedCustomers = 0;
                bool areAnyUnhandled = false;
                foreach (CustomerWrapper c in BookingWr.Customers)
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

        public ObservableCollection<HotelWrapper> AvailableHotels
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
            get => _BookingWrapper;

            set
            {
                if (_BookingWrapper == value)
                {
                    return;
                }

                _BookingWrapper = value;
                _BookingWrapper.PropertyChanged += Booking_PropertyChanged;
                RaisePropertyChanged();
            }
        }

        public RelayCommand BookRoomNoNameCommand { get; set; }

        public RelayCommand CheckOutCommand { get; set; }

        public RelayCommand ClearBookingCommand { get; set; }

        public RelayCommand DeletePaymentCommand { get; set; }

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

        public ObservableCollection<RoomWrapper> FilteredRoomList
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

        public GenericRepository GenericRepository { get; set; }

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

        public bool IsAnyResSelected => SelectedCustomer != null && SelectedCustomer.Reservation != null;

        public bool IsPartners
        {
            get
            {
                return BookingWr != null && BookingWr.IsPartners;
            }

            set
            {
                if (BookingWr.IsPartners == value)
                {
                    return;
                }

                BookingWr.IsPartners = value;
                if (!value)
                {
                    SelectedPartnerIndex = -1;
                }
                RaisePropertyChanged();
            }
        }

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

        public RelayCommand ManagePartnersCommand { get; set; }

        public NewReservationHelper NewReservationHelper { get; set; }

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

        public ObservableCollection<Partner> Partners
        {
            get
            {
                return _Partners;
            }

            set
            {
                if (_Partners == value)
                {
                    return;
                }

                _Partners = value;
                RaisePropertyChanged();
            }
        }

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
                foreach (CustomerWrapper customer in BookingWr.Customers)
                {
                    if (customer.Tel != null && (customer.Tel.Length > 0 || BookingWr.IsPartners))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public RelayCommand PrintContractCommand { get; set; }

        public RelayCommand PrintVoucherCommand { get; set; }

        public RelayCommand PutCustomersInRoomCommand { get; set; }

        public RelayCommand ReadNextLineCommand { get; set; }

        public GenericRepository RefreshableContext { get; set; }

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

        public CustomerWrapper SelectedCustomer
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

        public ExcursionWrapper SelectedExcursion
        {
            get
            {
                return _SelectedExcursion;
            }

            set
            {
                if (_SelectedExcursion == value)
                {
                    return;
                }

                _SelectedExcursion = value;
                RaisePropertyChanged();
            }
        }

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

        public int SelectedPartnerIndex
        {
            get
            {
                return _SelectedPartnerIndex;
            }
            set
            {
                if (_SelectedPartnerIndex == value)
                {
                    return;
                }
                _SelectedPartnerIndex = value;
                if (SelectedPartnerIndex >= 0 && (BookingWr.Partner == null || (BookingWr.Partner != null && BookingWr.Partner.Id != Partners[SelectedPartnerIndex].Id)))
                {
                    BookingWr.Partner = GenericRepository.GetById<Partner>(Partners[SelectedPartnerIndex].Id);
                }
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

        public int SelectedRoomTypeIndex
        {
            get => _SelectedRoomTypeIndex;

            set
            {
                if (_SelectedRoomTypeIndex == value)
                {
                    return;
                }

                _SelectedRoomTypeIndex = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedUserIndex
        {
            get
            {
                return _SelectedUserIndex;
            }
            set
            {
                if (_SelectedUserIndex == value)
                {
                    return;
                }
                _SelectedUserIndex = value;
                if (SelectedUserIndex >= 0 && (BookingWr.User == null || (BookingWr.User != null && BookingWr.User.Id != Users[SelectedUserIndex].Id)))
                {
                    BookingWr.User = GenericRepository.GetById<User>(Users[SelectedUserIndex].Id);
                }
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowFilteredRoomsCommand { get; set; }

        public RelayCommand UpdateAllCommand { get; set; }

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

        private bool AreContexesFree => (RefreshableContext != null && RefreshableContext.IsContextAvailable) && (GenericRepository != null && GenericRepository.IsContextAvailable);

        #endregion Properties
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
                        int i = 0;
                        CustomerWrapper tmpCustomerWr = new CustomerWrapper();
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
                                            tmpCustomerWr.Name = sst.ChildElements[ssid].InnerText;
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
                                            tmpCustomerWr.Surename = sst.ChildElements[ssid].InnerText;
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case "C":
                                        if (c.CellValue != null)
                                        {
                                            tmpCustomerWr.Tel = c.CellValue.Text.ToString();
                                            if (tmpCustomerWr.Tel.Length != 10)
                                            {
                                                int ssid = int.Parse(c.CellValue.Text);
                                                tmpCustomerWr.Tel = sst.ChildElements[ssid].InnerText;
                                                if (tmpCustomerWr.Tel.Length < 10)
                                                {
                                                    tmpCustomerWr.Tel = string.Empty;
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
                                            var tmpValue = c.CellValue.Text.Replace('.', ',');
                                            if (tmpCustomerWr.Price < 1 && float.TryParse(tmpValue, out float price))
                                            {
                                                tmpCustomerWr.Price = (float)Math.Round(price, 2); ;
                                            }
                                            if (tmpCustomerWr.Price <= 0)
                                            {
                                            }
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case "E":
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            tmpCustomerWr.PassportNum = sst.ChildElements[ssid].InnerText;
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;

                                    case "F":
                                        if (c.CellValue != null)
                                        {
                                            if (double.TryParse(c.CellValue.Text, out double d))
                                            {
                                                var dt = DateTime.FromOADate(d);
                                                if (dt != new DateTime(1999, 2, 21))
                                                {
                                                    tmpCustomerWr.DOB = dt;
                                                }
                                            }
                                        }
                                        else if (c.CellValue != null)
                                        {
                                            Console.WriteLine("Faulty data on row {0} - {1}", rowNum, c.CellValue.Text);
                                        }
                                        break;
                                }
                            }
                            BookingWr.Customers.Add(tmpCustomerWr);
                            Line++;
                        }
                        else
                        {
                        }
                    }
                }
            BookingWr.CalculateRemainingAmount();
        }
        public override async Task ReloadAsync()
        {
            await ResetAllRefreshableDataASync();
        }
        protected async Task ResetAllRefreshableDataASync()
        {
            if (SelectedExcursion == null)
            {
                SelectedExcursion = new ExcursionWrapper(await GenericRepository.GetExcursionByIdAsync(BookingWr.Excursion.Id));
            }
            if (SelectedExcursion != null)
            {
                try
                {
                    MessengerInstance.Send(new IsBusyChangedMessage(true));

                    if (RefreshableContext == null)
                    {
                        RefreshableContext = GenericRepository;
                    }
                    else
                    {
                        if (RefreshableContext != null && !RefreshableContext.IsTaskOk)
                        {
                            await RefreshableContext.LastTask;
                        }
                        RefreshableContext = new GenericRepository();
                    }
                    RoomsManager = new RoomsManager();

                    int hotelId = SelectedHotelIndex > 0 && Hotels != null && SelectedHotelIndex < Hotels.Count ? Hotels[SelectedHotelIndex - 1].Id : 0;
                    int roomTypeId = SelectedRoomTypeIndex > 0 && RoomTypes != null && SelectedRoomTypeIndex < RoomTypes.Count ? RoomTypes[SelectedRoomTypeIndex - 1].Id : 0;
                    var partnerId = SelectedPartnerIndex >= 0 && Partners != null && SelectedPartnerIndex < Partners.Count ? Partners[SelectedPartnerIndex].Id : -1;
                    if (BookingWr.IsPartners && SelectedPartnerIndex > 0)
                    {
                        partnerId = SelectedPartnerIndex >= 0 && Partners != null && SelectedPartnerIndex < Partners.Count ? Partners[SelectedPartnerIndex].Id : -1;
                    }
                    var userId = SelectedUserIndex >= 0 && Users != null && SelectedUserIndex < Users.Count ? Users[SelectedUserIndex].Id : -1;

                    Hotels = new ObservableCollection<Hotel>(await RefreshableContext.GetAllHotelsInCityAsync(SelectedExcursion.Destinations[0].Id));
                    RoomTypes = new ObservableCollection<RoomType>(await RefreshableContext.GetAllAsync<RoomType>());
                    StaticResources.StartingPlaces = new ObservableCollection<StartingPlace>(await RefreshableContext.GetAllAsyncSortedByName<StartingPlace>());
                    Users = new ObservableCollection<User>(await RefreshableContext.GetAllAsyncSortedByName<User>());
                    Partners = new ObservableCollection<Partner>(await RefreshableContext.GetAllAsyncSortedByName<Partner>());

                    SelectedHotelIndex = hotelId > 0 ? Hotels.IndexOf(Hotels.Where(x => x.Id == hotelId).FirstOrDefault()) + 1 : 0;
                    SelectedRoomTypeIndex = roomTypeId > 0 ? RoomTypes.IndexOf(RoomTypes.Where(x => x.Id == roomTypeId).FirstOrDefault()) + 1 : 0;

                    if (SelectedUserIndex < 0)
                    {
                        SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == StaticResources.User.Id).FirstOrDefault());
                    }
                    else
                    {
                        SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == userId).FirstOrDefault());
                    }
#if DEBUG
                    if (SelectedRoomTypeIndex == 0)
                    {
                        SelectedRoomTypeIndex = 1;
                    }
#endif
                    if (BookingWr.Id > 0 && BookingWr.IsPartners)
                    {
                        SelectedPartnerIndex = Partners.IndexOf(Partners.Where(p => p.Id == BookingWr.Partner.Id).FirstOrDefault());
                    }
                    else
                    {
                        SelectedPartnerIndex = Partners.IndexOf(Partners.Where(x => x.Id == partnerId).FirstOrDefault());
                    }
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
        }


        private async Task ClearBooking()
        {
            //DocumentsManagement dm = new DocumentsManagement(RefreshableContext);
            // dm.PrintAllBookings();
            //CalculateSum();

            MessageBoxResult result = MessageBoxResult.Yes;
            if (HasChanges)
            {
                result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
            if (result == MessageBoxResult.Yes)
            {
                InitializeBooking(await CreateNewBooking());
                FilteredRoomList.Clear();
                Payment = new Payment();
                BookedMessage = string.Empty;
                HB = false;
                All = true;
                OnlyStay = false;
                SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == StaticResources.User.Id).FirstOrDefault());
                SelectedPartnerIndex = -1;
                IsPartners = false;
                SelectedHotelIndex = 0;
                RaisePropertyChanged(nameof(IsPartners));
            }
            GenericRepository.RejectChanges();
        }

        protected async Task<Booking> CreateNewBooking()
        {
            return new Booking { Excursion = await GenericRepository.GetExcursionByIdAsync(SelectedExcursion.Id), CheckIn = BookingWr != null ? BookingWr.CheckIn : new DateTime(2018, 12, 28), CheckOut = BookingWr != null ? BookingWr.CheckOut : new DateTime(2018, 12, 30), User = await GenericRepository.GetByIdAsync<User>(StaticResources.User.Id) };

        }

        private void DeletePayment()
        {
            GenericRepository.Delete(SelectedPayment);
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
            }
            return error;
        }

        protected void InitializeBooking(Booking booking)
        {
            BookingWr = new BookingWrapper(booking);

            BookingWr.PropertyChanged += (s, e) =>
            {
                var x = e.PropertyName;
                if (!HasChanges)
                {
                    HasChanges = GenericRepository.HasChanges();
                    if (BookingWr.Id == 0)
                    {
                        HasChanges = true;
                    }
                }
            };
            if (BookingWr.Id > 0)
            {
                if (BookingWr.Customers.Count > 0)
                {
                    SelectedCustomer = BookingWr.Customers[0];
                }
            }
        }

        private async Task MakeNonameReservation()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                NewReservationHelper.MakeNonameReservation(BookingWr, await GenericRepository.GetByIdAsync<RoomType>(RoomTypes[SelectedRoomTypeIndex - 1].Id), HB, All, OnlyStay);

                if (All)
                {
                    FilteredRoomList.Clear();
                }
                else
                {
                    ShowFilteredRoomsCommand.Execute(null);
                }
                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));

                BookRoomNoNameCommand.RaiseCanExecuteChanged();
            }
        }

        private void ManagePartners()
        {
        }

        private async Task OverBookHotelAsync()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                NewReservationHelper.OverBookHotelAsync(
                      BookingWr, await GenericRepository.GetByIdAsync<Hotel>(Hotels[SelectedHotelIndex - 1].Id),
                      await GenericRepository.GetByIdAsync<RoomType>(RoomTypes[SelectedRoomTypeIndex - 1].Id), All, OnlyStay, HB);
                if (All)
                {
                    FilteredRoomList.Clear();
                }
                else
                {
                    ShowFilteredRoomsCommand.Execute(null);
                }
                SaveCommand.RaiseCanExecuteChanged();
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

        private void PrintContract()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                using (DocumentsManagement vm = new DocumentsManagement(RefreshableContext))
                {
                    vm.PrintSingleBookingContract(BookingWr);
                }
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

        private async Task PrintVoucher()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                using (DocumentsManagement vm = new DocumentsManagement(RefreshableContext))
                {
                    await vm.PrintSingleBookingVoucher(BookingWr);
                }
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

        private async Task PutCustomersInRoomAsync()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                NewReservationHelper.PutCustomersInRoomAsync(BookingWr, new RoomWrapper( await GenericRepository.GetByIdAsync<Room>(SelectedRoom.Id)), All, OnlyStay, HB);
                if (All)
                {
                    FilteredRoomList.Clear();
                }
                else
                {
                    ShowFilteredRoomsCommand.Execute(false);
                }
                SaveCommand.RaiseCanExecuteChanged();
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
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                BookingWr = await NewReservationHelper.SaveAsync(GenericRepository, Payment, BookingWr);
                FilteredRoomList.Clear();
                Payment = new Payment();
                BookedMessage = "H κράτηση αποθηκέυτηκε επιτυχώς";
                HasChanges = false;
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

        private async Task UpdateAll()
        {
            if (RefreshableContext != null && !RefreshableContext.IsTaskOk)
            {
                await RefreshableContext.LastTask;
            }
            RefreshableContext = new GenericRepository();
            await ResetAllRefreshableDataASync();
            UpdateAllCommand.RaiseCanExecuteChanged();
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
            foreach (CustomerWrapper customer in BookingWr.Customers)
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
            foreach (CustomerWrapper customer in BookingWr.Customers)
            {
                if (!customer.Handled)
                {
                    return "Παρακαλώ τοποθετήστε όλους τους πελάτες σε δωμάτια";
                }
            }
            return null;
        }

        #region Methods
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
                        CustomerWrapper tmpCustomer;
                        // Or... via each row
                        foreach (Row row in rows)
                        {
                            rowNum++;
                            i = 0;
                            bool addHim = true;
                            tmpCustomer = new CustomerWrapper() { StartingPlace = "Θεσσαλονίκη", Price = 70 };
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

                            //if (SelectedExcursion.DOBNeeded && (tmpCustomer.DOB == null || tmpCustomer.DOB.Year < 1918))
                            //{
                            //    addHim = false;
                            //}
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

        public void DeleteSelectedCustomers()
        {
            List<CustomerWrapper> toRemove = new List<CustomerWrapper>();
            bool showMessage = false;
            foreach (var customer in BookingWr.Customers)
            {
                if (customer.IsSelected)
                {
                    if (!customer.Handled)
                    {
                        toRemove.Add(customer);
                    }
                    else
                    {
                        showMessage = true;
                    }
                }
            }
            foreach (var c in toRemove)
            {
                BookingWr.Customers.Remove(c);
            }
            if (showMessage)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Καποιοι πελάτες δεν διαγράφηκαν επειδή συμμετέχουν σε κράτηση. Παλακαλώ κάντε τους CheckOut και δοκιμάστε ξανά!"));
            }
        }

        public override abstract Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null);

        public async Task ShowFilteredRoomsAsync(bool reset = true)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                if (reset)
                {
                    if (RefreshableContext != null && !RefreshableContext.IsTaskOk)
                    {
                        await RefreshableContext.LastTask;
                    }
                    RefreshableContext = new GenericRepository();
                }
                AvailableHotels = new ObservableCollection<HotelWrapper>();
               // AvailableHotels = new ObservableCollection<HotelWrapper>((await Task.Run(() => RoomsManager.GetAllAvailableRooms(RefreshableContext, BookingWr.CheckIn, BookingWr.CheckOut, SelectedExcursion, unSavedBooking: BookingWr.Model))));
                FilteredRoomList = new ObservableCollection<RoomWrapper>();
                List<RoomWrapper> tmplist = new List<RoomWrapper>();

                int allotmentDays = 0;
                bool addThis = false;

                foreach (HotelWrapper hotel in AvailableHotels)
                {
                    if (SelectedHotelIndex == 0 || hotel.Id == Hotels[SelectedHotelIndex - 1].Id)
                    {
                        foreach (RoomWrapper room in hotel.RoomWrappers)
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
                                tmplist.Add(room);
                            }
                            else
                            {
                            }
                        }
                    }
                }
                //TODO an mporei na fygei

                FilteredRoomList = new ObservableCollection<RoomWrapper>(tmplist.OrderBy(f => f.RoomType.MinCapacity).ToList());
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilteredRoomList);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription(nameof(Hotel));
                view.GroupDescriptions.Add(groupDescription);
                RaisePropertyChanged(nameof(FilteredRoomList));
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));

                ShowFilteredRoomsCommand.RaiseCanExecuteChanged();
            }
        }


        private void AddTransfer()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                NewReservationHelper.AddTransfer(BookingWr, All);
                if (All)
                {
                    FilteredRoomList.Clear();
                }
                else
                {
                    ShowFilteredRoomsCommand.Execute(null);
                }
                SaveCommand.RaiseCanExecuteChanged();
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
            if (BookingWr == null || !AreContexesFree)
            {
                return false;
            }
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

        private bool CanDeletePayment()
        {
            return SelectedPayment != null && AreContexesFree;
        }

        private bool CanMakeNoNameReservation()
        {//TODO tsekare to opws kai ola ta TODO
            if (SelectedRoomTypeIndex <= 0 || (SelectedCustomer == null && !All) || FilteredRoomList.Count == 0 || !AreBookingDataValid || !AreContexesFree)
            {
                return false;
            }
            return true;
        }

        private bool CanOverBookHotel()
        {
            return SelectedHotelIndex > 0 && SelectedRoomTypeIndex > 0 && AreBookingDataValid && (SelectedCustomer != null || All) && AreContexesFree;
        }

        private bool CanPrintDoc()
        {
            if (BookingWr == null || !AreContexesFree)
            {
                return false;
            }
            return BookingWr.Id > 0;
        }

        private bool CanPutCustomersInRoom()
        {
            if (BookingWr == null || !AreContexesFree)
            {
                return false;
            }
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
            if (BookingWr == null || !HasChanges || !AreContexesFree)
            {
                return false;
            }
            return AreBookingDataValid && ValidateReservations() == null;
        }

        private bool CanShowFilteredRooms()
        {
            if (BookingWr == null || !AreContexesFree)
            {
                return false;
            }
            return SelectedHotelIndex >= 0 && SelectedRoomTypeIndex >= 0 && BookingWr != null && BookingWr.AreDatesValid();
        }

        private bool CanUpdateAll()
        {
            return AreContexesFree;
        }

        private void CheckOut()
        {
            try
            {
                List<Reservation> toRemove = new List<Reservation>();
                foreach (CustomerWrapper c in BookingWr.Customers)
                {
                    if (c.IsSelected && c.Handled)
                    {
                        foreach (Reservation r in BookingWr.ReservationsInBooking)
                        {
                            r.CustomersList.RemoveAll(x => x.Id == c.Id);
                        }
                        c.Handled = false;
                        c.RoomColor = new SolidColorBrush(System.Windows.Media.Colors.Green);
                        c.Reservation = null;
                        c.HotelName = "KENO";
                        c.RoomTypeName = "KENO";
                        c.RoomNumber = "OXI";
                        c.Reservation = null;
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
                    if (r.Id > 0)
                    {
                        GenericRepository.Delete(r);
                    }
                }
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        #endregion Methods
    }
}