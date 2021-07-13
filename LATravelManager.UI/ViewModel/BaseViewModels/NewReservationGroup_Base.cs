﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.CategoriesViewModels;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views;
using LATravelManager.UI.Views.Universal;
using LATravelManager.UI.Wrapper;
using Microsoft.Win32;
using NuGet;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public class HotelWithRooms
    {
        #region Properties

        public List<RoomWrapper> Rooms { get; set; }

        #endregion Properties
    }

    public abstract class NewReservationGroup_Base : MyViewModelBaseAsync
    {
        #region Fields

        private readonly string[] ValidateRoomsProperties =
                {
            nameof(BookingWr),nameof(Payment)
        };

        private bool _All;

        private ObservableCollection<HotelWrapper> _AvailableHotels;

        private string _BookedMessage = string.Empty;

        private BookingWrapper _BookingWrapper;

        private ObservableCollection<Email> _Emails;

        private string _ErrorsInCanAddReservationToRoom = string.Empty;

        private string _ErrorsInDatagrid = string.Empty;

        private ObservableCollection<RoomWrapper> _FilteredRoomList;

        private ObservableCollection<RoomTypeWrapper> _FilteredRoomTypesList;

        private ObservableCollection<Excursion> _GroupExcursions;
        private bool _HB = false;

        private ObservableCollection<Hotel> _Hotels;

        private int _Line;

        private ExtraService _NewExtraService;

        private Transaction _NewTransaction;
        private int _NoNames;

        private int _NumOfSelectedCustomers;

        private bool _OnlyStay = false;

        private ObservableCollection<Partner> _Partners;

        private Payment _Payment = new Payment();

        private ObservableCollection<RoomType> _RoomTypes;

        private HashSet<RoomType> _RoomTypesCount;
        private CustomerWrapper _SelectedCustomer;

        private string _SelectedEmail;

        private ExcursionWrapper _SelectedExcursion;

        private Excursion _SelectedExcursionToChange;
        private ExtraService _SelectedExtraService;

        private int _SelectedHotelIndex;

        private int _SelectedPartnerIndex = -1;

        private Payment _SelectedPayment;

        private RoomWrapper _SelectedRoom;

        private RoomType _SelectedRoomTypeCount;
        private int _SelectedRoomTypeIndex;

        private Transaction _SelectedTransaction;
        private int _SelectedUserIndex;

        private ObservableCollection<User> _Users;

        #endregion Fields

        #region Constructors

        public NewReservationGroup_Base(MainViewModel mainViewModel)
        {
            GenericRepository = mainViewModel.StartingRepository;
            BasicDataManager = mainViewModel.BasicDataManager;
            NewReservationHelper = new NewReservationHelper();
            RoomsManager = new RoomsManager();
            NewTransaction = new Transaction();

            //Commands
            ShowFilteredRoomsCommand = new RelayCommand(async () => await ShowFilteredRoomsAsync(), CanShowFilteredRooms);
            ClearBookingCommand = new RelayCommand(async () => await ClearBooking());
            ReadNextLineCommand = new RelayCommand(ReadNextLine);

            AddCustomerCommand = new RelayCommand(AddRandomCustomer);
            AddFromFileCommand = new RelayCommand(AddFromFile);

            DeletePaymentCommand = new RelayCommand(DeletePayment, CanDeletePayment);
            DeleteTransactionCommand = new RelayCommand(DeleteTransaction, CanDeleteTransaction);
            AddNewTransactionCommand = new RelayCommand(AddNewTransaction, CanAddNewTransaction);
            PrintRecieptCommand = new RelayCommand(PrintReciept);
            DeleteExtraServiceCommand = new RelayCommand(DeleteExtraService, CanDeleteExtraservice);
            AddExraServiceCommand = new RelayCommand(AddExtraService, CanAddExtraservice);
            ChangeBookingCommand = new RelayCommand(async () => await TryChangeBooking());

            DeleteSelectedCustomersCommand = new RelayCommand(DeleteSelectedCustomers, CanDeleteCustomers);

            UpdateAllCommand = new RelayCommand(async () => await UpdateAll(), CanUpdateAll);
            ShowAltairnativesCommand = new RelayCommand(ShowAlernatives, CanShowAltairnatives);

            BookRoomNoNameCommand = new RelayCommand<RoomType>(async (obj) => { await MakeNonameReservation(obj); }, CanMakeNoNameReservation);
            OverBookHotelCommand = new RelayCommand(async () => await OverBookHotelAsync(), CanOverBookHotel);
            PutCustomersInRoomCommand = new RelayCommand(async () => await PutCustomersInRoomAsync(), CanPutCustomersInRoom);
            AddTransferCommand = new RelayCommand(AddTransfer, CanAddTransfer);
            AddOneDayCommand = new RelayCommand(AddOneDay, CanAddOneDay);

            DoubleClickRoomTypeCommand = new RelayCommand(ExpandRooms, CanExpand);
            SearchForCustomerCommand = new RelayCommand(SearchForCustomer);
            OpenInvoicesWindowCommand = new RelayCommand<object>(async (obj) => await OpenInvoicesWindow(obj), canOpenInvoiceWindow);

            ManagePartnersCommand = new RelayCommand(ManagePartners);

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            CheckOutCommand = new RelayCommand(() => { CheckOutCustomers(); }, CanCheckOut);
            PrintVoucherCommand = new RelayCommand(async () => await PrintVoucher(), CanPrintDoc);
            PrintContractCommand = new RelayCommand(PrintContract, CanPrintDoc);
            OpenFileCommand = new RelayCommand<Reciept>(async (obj) => await OpenRecieptFile(obj));
            ReplaceFileCommand = new RelayCommand(ReplaceFile);
            Payment = new Payment();

            ToggleDisabilityCommand = new RelayCommand(ToggleDisability, CanToggleDisability);

            FilteredRoomList = new ObservableCollection<RoomWrapper>();
            SelectedUserIndex = -1;
            Emails = new ObservableCollection<Email>();
            NewExtraService = new ExtraService();
        }

        private bool canOpenInvoiceWindow(object obj)
        {
            return BookingWr != null && BookingWr.Id > 0;
        }

        private Reciept _SelectedReciept;

        public Reciept SelectedReciept
        {
            get
            {
                return _SelectedReciept;
            }

            set
            {
                if (_SelectedReciept == value)
                {
                    return;
                }

                _SelectedReciept = value;
                RaisePropertyChanged();
            }
        }

        private void ReplaceFile()
        {
            if (SelectedReciept == null)
            {
                return;
            }
            OpenFileDialog dlg = new OpenFileDialog
            {
                //FileName = "Document", // Default file name
                //DefaultExt = ".pdf", // Default file extension
                Filter = "All files (*.*)|*.*" // Filter files by extension
            };
            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                // Open document
                string fileName = dlg.FileName;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    FileInfo file = new FileInfo(fileName);
                    if (file.Exists)
                    {
                        SelectedReciept.Content = File.ReadAllBytes(fileName);
                        SelectedReciept.FileName = file.Name;
                    }
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public static string GetPath(string fileName, string folderpath)
        {
            int i = 1;
            string resultPath;
            string filenm = fileName.Substring(0, fileName.LastIndexOf('.'));
            string fileExtension = fileName.Substring(fileName.LastIndexOf('.'));
            resultPath = folderpath + filenm + fileExtension;
            while (File.Exists(resultPath))
            {
                i++;
                resultPath = folderpath + filenm + "(" + i + ")" + fileExtension;
            }
            return resultPath;
        }

        private async Task OpenRecieptFile(Reciept Reciept)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            string outputpath = Path.GetTempPath();

            Directory.CreateDirectory(outputpath + @"\Παραστατικά");
            string path = GetPath(Reciept.FileName, outputpath + @"\Παραστατικά\");
            File.WriteAllBytes(path, Reciept.Content);

            Mouse.OverrideCursor = Cursors.Arrow;

            Process.Start(path);
        }

        #endregion Constructors

        #region Properties

        public RelayCommand AddCustomerCommand { get; set; }

        public RelayCommand AddExraServiceCommand { get; set; }

        public RelayCommand AddFromFileCommand { get; set; }

        public RelayCommand AddNewTransactionCommand { get; set; }

        public RelayCommand AddOneDayCommand { get; set; }

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
                if (All)
                {
                    NumOfSelectedCustomers = BookingWr.Customers.Count;
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

        public BasicDataManager BasicDataManager { get; set; }

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

        public RelayCommand<Reciept> OpenFileCommand { get; set; }
        public RelayCommand ReplaceFileCommand { get; set; }

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

        public RelayCommand<RoomType> BookRoomNoNameCommand { get; set; }

        public bool CanEditDates => BookingWr != null && BookingWr.ReservationsInBooking != null && BookingWr.ReservationsInBooking.Count == 0;

        public RelayCommand ChangeBookingCommand { get; set; }

        public RelayCommand CheckOutCommand { get; set; }

        public RelayCommand ClearBookingCommand { get; set; }

        public RelayCommand DeleteExtraServiceCommand { get; set; }

        public RelayCommand DeletePaymentCommand { get; set; }

        public RelayCommand DeleteSelectedCustomersCommand { get; set; }

        public RelayCommand DeleteTransactionCommand { get; set; }

        public DocumentsManagement DocumentsManagement { get; set; }

        public RelayCommand DoubleClickRoomTypeCommand { get; set; }

        public ObservableCollection<Email> Emails
        {
            get
            {
                return _Emails;
            }

            set
            {
                if (_Emails == value)
                {
                    return;
                }

                _Emails = value;
                RaisePropertyChanged();
            }
        }

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

        public ObservableCollection<RoomTypeWrapper> FilteredRoomTypesList
        {
            get
            {
                return _FilteredRoomTypesList;
            }

            set
            {
                if (_FilteredRoomTypesList == value)
                {
                    return;
                }

                _FilteredRoomTypesList = value;
                RaisePropertyChanged();
            }
        }

        public GenericRepository GenericRepository { get; set; }

        public ObservableCollection<Excursion> GroupExcursions
        {
            get
            {
                return _GroupExcursions;
            }

            set
            {
                if (_GroupExcursions == value)
                {
                    return;
                }

                _GroupExcursions = value;
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

        public ExtraService NewExtraService
        {
            get
            {
                return _NewExtraService;
            }

            set
            {
                if (_NewExtraService == value)
                {
                    return;
                }

                _NewExtraService = value;
                RaisePropertyChanged();
            }
        }

        public NewReservationHelper NewReservationHelper { get; set; }

        public Transaction NewTransaction
        {
            get
            {
                return _NewTransaction;
            }

            set
            {
                if (_NewTransaction == value)
                {
                    return;
                }

                _NewTransaction = value;
                RaisePropertyChanged();
            }
        }

        public int NoNames
        {
            get
            {
                return _NoNames;
            }

            set
            {
                if (_NoNames == value)
                {
                    return;
                }

                _NoNames = value;
                RaisePropertyChanged();
            }
        }

        public int NumOfSelectedCustomers
        {
            get { return _NumOfSelectedCustomers; }
            set
            {
                if (_NumOfSelectedCustomers == value)
                {
                    return;
                }

                _NumOfSelectedCustomers = value;
                RaisePropertyChanged();
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

                if (SelectedCustomer != null && SelectedCustomer.Reservation != null)
                {
                    foreach (var c in SelectedCustomer.Reservation.CustomersList)
                    {
                        c.StartingPlace = "ONLY STAY";
                        c.CustomerHasBusIndex = 3;
                        c.CustomerHasShipIndex = 3;
                        c.CustomerHasPlaneIndex = 3;
                    }
                }
                foreach (var c in BookingWr.Customers)
                {
                    if (!c.Handled)
                    {
                        c.StartingPlace = "ONLY STAY";
                        c.CustomerHasBusIndex = 3;
                        c.CustomerHasShipIndex = 3;
                        c.CustomerHasPlaneIndex = 3;
                    }
                }
                RaisePropertyChanged();
            }
        }

        public RelayCommand<object> OpenInvoicesWindowCommand { get; set; }

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
                _Payment.PropertyChanged += PaymentChanged;
                RaisePropertyChanged();
            }
        }

        public RelayCommand PrintContractCommand { get; set; }

        public RelayCommand PrintRecieptCommand { get; set; }

        public RelayCommand PrintVoucherCommand { get; set; }

        public RelayCommand PutCustomersInRoomCommand { get; set; }

        public RelayCommand ReadNextLineCommand { get; set; }

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

        public HashSet<RoomType> RoomTypesCount
        {
            get
            {
                return _RoomTypesCount;
            }

            set
            {
                if (_RoomTypesCount == value)
                {
                    return;
                }

                _RoomTypesCount = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; set; }

        public RelayCommand SearchForCustomerCommand { get; set; }

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
                if (BookingWr != null)
                {
                    RaisePropertyChanged(nameof(BookingWr));
                    BookingWr.RaisePropertyChanged(nameof(BookingWr.IsGroup));
                    BookingWr.RaisePropertyChanged(nameof(BookingWr.IsNotGroup));
                }
            }
        }

        public Excursion SelectedExcursionToChange
        {
            get
            {
                return _SelectedExcursionToChange;
            }

            set
            {
                if (_SelectedExcursionToChange == value)
                {
                    return;
                }

                _SelectedExcursionToChange = value;
                RaisePropertyChanged();
            }
        }

        public ExtraService SelectedExtraService
        {
            get
            {
                return _SelectedExtraService;
            }

            set
            {
                if (_SelectedExtraService == value)
                {
                    return;
                }

                _SelectedExtraService = value;
                if (BookingWr != null && SelectedExtraService.Customer != null && BookingWr.Customers.Any(c => c.Id == SelectedExtraService.Customer.Id))
                {
                    foreach (var cu in BookingWr.Customers)
                    {
                        cu.IsSelected = cu.Id == SelectedExtraService.Customer.Id;
                    }
                }
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
                AvailableHotels = null;
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
                    Emails = (BookingWr.Partner.Emails != null && BookingWr.Partner.Emails.Any()) ? new ObservableCollection<Email>(BookingWr.Partner.Emails.Split(',').Select(e => new Email(e))) : new ObservableCollection<Email>();
                    if (Emails.Count > 0)
                    {
                        BookingWr.PartnerEmail = Emails[0].EValue;
                    }
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

        public RoomWrapper SelectedRoom
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

        public RoomType SelectedRoomTypeCount
        {
            get
            {
                return _SelectedRoomTypeCount;
            }

            set
            {
                if (_SelectedRoomTypeCount == value)
                {
                    return;
                }

                _SelectedRoomTypeCount = value;
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
                AvailableHotels = null;
            }
        }

        public Transaction SelectedTransaction
        {
            get
            {
                return _SelectedTransaction;
            }

            set
            {
                if (_SelectedTransaction == value)
                {
                    return;
                }

                _SelectedTransaction = value;
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

        public RelayCommand ShowAltairnativesCommand { get; set; }

        public RelayCommand ShowFilteredRoomsCommand { get; set; }

        public RelayCommand ToggleDisabilityCommand { get; set; }

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

        private bool AreContexesFree => BasicDataManager != null && BasicDataManager.IsContextAvailable && GenericRepository != null && GenericRepository.IsContextAvailable;

        #endregion Properties

        #region Methods

        public void DeleteSelectedCustomers()
        {
            List<CustomerWrapper> toRemove = new List<CustomerWrapper>();
            bool showMessage = false;
            foreach (CustomerWrapper customer in BookingWr.Customers)
            {
                if (customer.IsSelected)
                {
                    if (!customer.Handled)
                    {
                        toRemove.Add(customer);
                        customer.BusGo = null;
                        customer.BusReturn = null;
                    }
                    else
                    {
                        showMessage = true;
                    }
                }
            }
            foreach (CustomerWrapper c in toRemove)
            {
                BookingWr.Customers.Remove(c);
            }
            if (showMessage)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Κάποιοι πελάτες δεν διαγράφηκαν επειδή συμμετέχουν σε κράτηση. Παρακαλώ κάντε τους CheckOut και δοκιμάστε ξανά!"));
            }
        }

        public override abstract Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null, MyViewModelBase parent = null);

        public override async Task ReloadAsync()
        {
            await ResetAllRefreshableDataASync();
        }

        public void SearchForCustomer()
        {
            CustomerSearch_ViewModel vm = new CustomerSearch_ViewModel(GenericRepository, booking: BookingWr);
            Window window = new CustomerSearchWindow
            {
                DataContext = vm
            };
            window.ShowDialog();
        }

        public async Task ShowFilteredRoomsAsync()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (BookingWr.CheckIn == BookingWr.CheckOut)
                {
                    return;
                }
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                RoomsManager = new RoomsManager();
                if (BookingWr.ExcursionDate != null && BookingWr.ExcursionDate.NightStart)
                    AvailableHotels = new ObservableCollection<HotelWrapper>(await RoomsManager.GetAllAvailableRooms(BookingWr.CheckIn.AddDays(1), BookingWr.CheckOut, SelectedExcursion, false, unSavedBooking: BookingWr.Model));
                else
                    AvailableHotels = new ObservableCollection<HotelWrapper>(await RoomsManager.GetAllAvailableRooms(BookingWr.CheckIn, BookingWr.CheckOut, SelectedExcursion, false, unSavedBooking: BookingWr.Model));

                FilteredRoomList = new ObservableCollection<RoomWrapper>();
                List<HotelWithRooms> tmplist = new List<HotelWithRooms>();
                HotelWithRooms hotelwr;
                int allotmentDays = 0;
                bool addThis, isfree;

                foreach (HotelWrapper hotel in AvailableHotels)
                {
                    hotelwr = new HotelWithRooms { Rooms = new List<RoomWrapper>() };
                    foreach (RoomWrapper room in hotel.RoomWrappers)
                    {
                        allotmentDays = 0;
                        isfree = addThis = true;
                        foreach (PlanDailyInfo day in room.PlanDailyInfo)
                        {
                            if (day.Date < BookingWr.CheckIn && (BookingWr.ExcursionDate == null || !BookingWr.ExcursionDate.NightStart))
                            {
                                continue;
                            }
                            if (BookingWr.ExcursionDate != null && BookingWr.ExcursionDate.NightStart && day.Date < BookingWr.CheckIn.AddDays(1))
                            {
                                continue;
                            }
                            if (!addThis || day.Date >= BookingWr.CheckOut)
                            {
                                break;
                            }
                            addThis &= (day.RoomState == RoomStateEnum.Available
                                || day.RoomState == RoomStateEnum.MovableNoName
                                || day.RoomState == RoomStateEnum.Allotment) &&
                                ((BookingWr.ExcursionDate != null && BookingWr.ExcursionDate.NightStart && (BookingWr.CheckOut - BookingWr.CheckIn).TotalDays - 1 >= day.MinimumStay) ||
                                ((BookingWr.ExcursionDate == null || !BookingWr.ExcursionDate.NightStart) && (BookingWr.CheckOut - BookingWr.CheckIn).TotalDays >= day.MinimumStay));
                            isfree &= day.RoomState == RoomStateEnum.Available;

                            if (day.RoomState == RoomStateEnum.Allotment)
                            {
                                allotmentDays++;
                            }
                        }
                        if (isfree)
                        {
                            if (hotel.NoName)
                                room.RoomType.freeRooms++;
                            else
                                room.RoomType.Named++;
                        }
                        addThis &= SelectedHotelIndex == 0 || hotel.Id == Hotels[SelectedHotelIndex - 1].Id;

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

                            hotelwr.Rooms.Add(room);
                            ValidateRoom(room);
                            if (room.Hotel.NoName)
                            {
                                NoNames++;
                            }
                        }
                    }
                    if (hotelwr.Rooms.Count > 0)
                    {
                        tmplist.Add(hotelwr);
                    }
                }

                List<RoomWrapper> rooms = new List<RoomWrapper>();

                RoomTypesCount = new HashSet<RoomType>();

                foreach (var item in tmplist)
                {
                    foreach (var room in item.Rooms)
                    {
                        if (!string.IsNullOrEmpty(room.LocalNote))
                        {
                            rooms.Add(room);
                        }
                        else if (!room.Hotel.NoName || (rooms.Where(r => r.Hotel == room.Hotel && r.RoomType == room.RoomType).Count() < room.RoomType.freeRooms))
                        {
                            rooms.Add(room);
                            RoomTypesCount.Add(room.RoomType);
                        }
                    }
                }
                RoomTypesCount = new HashSet<RoomType>(RoomTypesCount.OrderBy(r => r.MinCapacity).ThenBy(t => t.MaxCapacity));

                //TODO an mporei na fygei

                //foreach (var hotel in tmplist)
                //{
                //    foreach (var roomtype in hotel.)
                //    {
                //    }
                //}

                FilteredRoomList = new ObservableCollection<RoomWrapper>(rooms.OrderBy(f => f.Hotel.Name).ThenBy(t => t.RoomType.MaxCapacity).ThenByDescending(r => r.Rating).ToList());
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilteredRoomList);
                //PropertyGroupDescription groupDescriptionb = new PropertyGroupDescription(nameof(Hotel));
                //PropertyGroupDescription groupDescription = new PropertyGroupDescription(nameof(RoomType));
                //view.GroupDescriptions.Add(groupDescriptionb);
                //view.GroupDescriptions.Add(groupDescription);
                RaisePropertyChanged(nameof(FilteredRoomList));
            }
            //try
            //{
            //    MessengerInstance.Send(new IsBusyChangedMessage(true));

            //    AvailableHotels = new ObservableCollection<HotelWrapper>((await RoomsManager.GetAllAvailableRooms(BookingWr.CheckIn, BookingWr.CheckOut, SelectedExcursion, unSavedBooking: BookingWr.Model)));
            //    FilteredRoomList = new ObservableCollection<RoomWrapper>();
            //    List<RoomTypeWrapper> tmplist = new List<RoomTypeWrapper>();

            //    int allotmentDays = 0;
            //    bool addThis = false;
            //    int indexOfRoomtype = 0, indexOfHotel = 0;

            //    foreach (HotelWrapper hotel in AvailableHotels)
            //    {
            //        if (SelectedHotelIndex == 0 || hotel.Id == Hotels[SelectedHotelIndex - 1].Id)
            //        {
            //            foreach (RoomWrapper room in hotel.RoomWrappers)
            //            {
            //                indexOfHotel = indexOfRoomtype = -1;
            //                allotmentDays = 0;
            //                addThis = true;
            //                for (int i = 0; i < room.PlanDailyInfo.Count; i++)
            //                {
            //                    addThis &= room.PlanDailyInfo[i].RoomState == RoomStateEnum.Available || room.PlanDailyInfo[i].RoomState == RoomStateEnum.MovableNoName || room.PlanDailyInfo[i].RoomState == RoomStateEnum.Allotment;
            //                    if (room.PlanDailyInfo[i].RoomTypeEnm == RoomTypeEnum.Allotment)
            //                    {
            //                        allotmentDays++;
            //                    }
            //                }
            //                //todelete
            //                if (addThis)
            //                {
            //                    if (allotmentDays > 0)
            //                    {
            //                        if (room.PlanDailyInfo.Count != allotmentDays)
            //                        {
            //                            //room.IsAllotment = true;
            //                            room.LocalNote = "Allotment" + ((allotmentDays == 1) ? " η 1 μέρα" : (" οι " + allotmentDays + " ημέρες."));
            //                        }
            //                        else
            //                        {
            //                            // room.IsAllotment = true;
            //                            room.LocalNote = "Allotment";
            //                        }
            //                    }
            //                    else
            //                    {
            //                        // room.IsAllotment = false;
            //                        room.LocalNote = "";
            //                    }

            //                    indexOfRoomtype = tmplist.FindIndex(x => x.RoomType == room.RoomType);
            //                    if (indexOfRoomtype >= 0)
            //                    {
            //                        indexOfHotel = tmplist[indexOfRoomtype].Hotels.FindIndex(x => x.Hotel == room.Hotel);
            //                    }
            //                    else
            //                    {
            //                        indexOfRoomtype = tmplist.Count;
            //                        tmplist.Add(new RoomTypeWrapper { Hotels = new ObservableCollection<RoomsInHotel>(), RoomType = room.RoomType });
            //                    }
            //                    if (indexOfHotel >= 0)
            //                    {
            //                        tmplist[indexOfRoomtype].Hotels[indexOfHotel].Rooms.Add(room);
            //                    }
            //                    else
            //                    {
            //                        tmplist[indexOfRoomtype].Hotels.Add(new RoomsInHotel { Rooms = new List<RoomWrapper>(), Hotel = room.Hotel });
            //                        tmplist[indexOfRoomtype].Hotels[0].Rooms.Add(room);
            //                    }
            //                }
            //                else
            //                {
            //                }
            //            }
            //        }
            //    }

            //   // FilteredRoomList = new ObservableCollection<RoomWrapper>(tmplist.OrderBy(f => f.RoomType.MinCapacity).ToList());
            //    //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilteredRoomList);
            //   // PropertyGroupDescription groupDescription = new PropertyGroupDescription(nameof(RoomType));
            //   // view.GroupDescriptions.Add(groupDescription);
            //    RaisePropertyChanged(nameof(FilteredRoomList));
            //    FilteredRoomTypesList = new ObservableCollection<RoomTypeWrapper>(tmplist);
            //}
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));

                ShowFilteredRoomsCommand.RaiseCanExecuteChanged();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        protected async Task<Booking> CreateNewBooking()
        {
            var booki = new Booking { CheckIn = BookingWr != null ? BookingWr.CheckIn : DateTime.Today, CheckOut = BookingWr != null ? BookingWr.CheckOut : DateTime.Today.AddDays(3) };
            var exc = await GenericRepository.GetExcursionByIdAsync(SelectedExcursion.Id, true);
            var user = await GenericRepository.GetByIdAsync<User>(StaticResources.User.Id, true);
            booki.User = user;
            booki.Excursion = exc;

            return booki;
            //return new Booking { Excursion = e, CheckIn = BookingWr != null ? BookingWr.CheckIn : new DateTime(2018, 12, 28), CheckOut = BookingWr != null ? BookingWr.CheckOut : new DateTime(2018, 12, 30), User = u};
        }

        protected void InitializeBooking(Booking booking, ObservableCollection<CustomerWrapper> oldCusts = null)
        {
            BookingWr = new BookingWrapper(booking, true);

            BookingWr.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = GenericRepository.HasChanges();
                    if (BookingWr.Id == 0)
                    {
                        HasChanges = true;
                    }
                }
            };
            BookingWr.Transactions.CollectionChanged += (s, e) =>
             {
                 if (!HasChanges)
                 {
                     HasChanges = GenericRepository.HasChanges();
                     if (BookingWr.Id == 0)
                     {
                         HasChanges = true;
                     }
                 }
             };
            BookingWr.Payments.CollectionChanged += (s, e) =>
            {
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

            Emails = (BookingWr.IsPartners && BookingWr.Partner != null && BookingWr.Partner.Emails != null && BookingWr.Partner.Emails.Any()) ? new ObservableCollection<Email>(BookingWr.Partner.Emails.Split(',').Select(e => new Email(e))) : new ObservableCollection<Email>();
            if (oldCusts != null)
                BookingWr.Customers.AddRange(oldCusts);

            if (BookingWr?.Excursion?.FixedDates == true)
            {
                if (BookingWr.CheckOut >= DateTime.Now)
                    BookingWr.ExcursionDatesFiltered = BookingWr.Excursion.ExcursionDates;
                else
                    BookingWr.ExcursionDatesFiltered = new ObservableCollection<ExcursionDate>(BookingWr.Excursion.ExcursionDates.Where(r => r.CheckOut >= DateTime.Today).OrderBy(t => t.CheckIn));
            }
        }

        protected async Task ResetAllRefreshableDataASync(bool makeNew = false)
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

                    if (makeNew)
                        await BasicDataManager.Refresh();

                    RoomsManager = new RoomsManager();

                    int hotelId = SelectedHotelIndex > 0 && Hotels != null && SelectedHotelIndex < Hotels.Count ? Hotels[SelectedHotelIndex - 1].Id : 0;
                    int roomTypeId = SelectedRoomTypeIndex > 0 && RoomTypes != null && SelectedRoomTypeIndex < RoomTypes.Count ? RoomTypes[SelectedRoomTypeIndex - 1].Id : 0;
                    int partnerId = -1;
                    if (BookingWr.IsPartners && SelectedPartnerIndex > 0)
                    {
                        partnerId = SelectedPartnerIndex >= 0 && Partners != null && SelectedPartnerIndex < Partners.Count ? Partners[SelectedPartnerIndex].Id : -1;
                    }
                    int userId = SelectedUserIndex >= 0 && Users != null && SelectedUserIndex < Users.Count ? Users[SelectedUserIndex].Id : -1;

                    Hotels = new ObservableCollection<Hotel>(BasicDataManager.Hotels.Where(h => h.City.Id == SelectedExcursion.Destinations[0].Id));
                    RoomTypes = BasicDataManager.RoomTypes;
                    StaticResources.StartingPlaces = BasicDataManager.StartingPlaces;
                    Users = BasicDataManager.Users;
                    Partners = BasicDataManager.Partners;
                    GroupExcursions = new ObservableCollection<Excursion>(BasicDataManager.GroupExcursions.Where(e => !e.Deactivated && e.ExcursionDates.Any(b => b.CheckOut > DateTime.Today)));

                    SelectedHotelIndex = hotelId > 0 ? Hotels.IndexOf(Hotels.Where(x => x.Id == hotelId).FirstOrDefault()) + 1 : 0;
                    SelectedRoomTypeIndex = roomTypeId > 0 ? RoomTypes.IndexOf(RoomTypes.Where(x => x.Id == roomTypeId).FirstOrDefault()) + 1 : 0;

                    if (SelectedUserIndex < 0)
                    {
                        SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == BookingWr.User.Id).FirstOrDefault());
                    }
                    else
                    {
                        SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == userId).FirstOrDefault());
                    }
                    //#if DEBUG
                    //                    if (SelectedRoomTypeIndex == 0)
                    //                    {
                    //                        SelectedRoomTypeIndex = 1;
                    //                    }
                    //#endif
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

        private void AddExtraService()
        {
            BookingWr.ExtraServices.Add(new ExtraService
            {
                Description = NewExtraService.Description,
                Amount = NewExtraService.Amount,
                Customer = SelectedCustomer.Model
            });
            BookingWr.CalculateRemainingAmount();
        }

        private void AddFromFile()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                FileName = "Document", // Default file name
                DefaultExt = ".txt", // Default file extension
                Filter = "Excel Documents (.xlsx)|*.xlsx", // Filter files by extension
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
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
                                            tmpCustomer.Name = ToLatin(sst.ChildElements[ssid].InnerText);
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
                                            tmpCustomer.Surename = ToLatin(sst.ChildElements[ssid].InnerText);
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

                                    case 5:
                                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                        {
                                            int ssid = int.Parse(c.CellValue.Text);
                                            tmpCustomer.Tel = sst.ChildElements[ssid].InnerText;
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

        private void AddNewTransaction()
        {
            NewTransaction.User = GenericRepository.Context.Users.FirstOrDefault(u => u.Id == StaticResources.User.Id);
            NewTransaction.Date = DateTime.Now;
            NewTransaction.Excursion = BookingWr.Excursion != null ? BookingWr.Excursion : null;
            NewTransaction.ExcursionExpenseCategory = ExcursionExpenseCategories.Booking;
            NewTransaction.ExpenseBaseCategory = ExpenseBaseCategories.GroupExpense;
            NewTransaction.TransactionType = TransactionType.Expense;
            BookingWr.Transactions.Add(NewTransaction);
            NewTransaction = new Transaction();
        }

        private void AddOneDay()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                NewReservationHelper.AddOneDay(BookingWr, All);
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

        private void AddRandomCustomer()
        {
            BookingWr.Customers.Add(NewReservationHelper.CreateRandomCustomer());
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

        private void Booking_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //TODO mporei na mporei na ginei kalytera vazontas to mesa sto checkin
            //episis na to kanw na vazei ws epistrofi thn epomenh kyriakh ektos apo xristugenna
            if (e.PropertyName == nameof(BookingWr.CheckIn) || e.PropertyName == nameof(BookingWr.CheckOut))
            {
                FilteredRoomList.Clear();
                AvailableHotels = null;
            }
        }

        private bool CanAddExtraservice()
        {
            return !string.IsNullOrEmpty(NewExtraService.Description) &&
                NewExtraService.Description.Length > 3 &&
                SelectedCustomer != null;
        }

        private bool CanAddNewTransaction()
        {
            return NewTransaction != null && NewTransaction.Amount > 0;
        }

        private bool CanAddOneDay()
        {
            if (BookingWr == null || !AreContexesFree || BookingWr.CheckIn != BookingWr.CheckOut)
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

            ErrorsInCanAddReservationToRoom = "";
            return true;
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
            ErrorsInCanAddReservationToRoom = "";

            return true;
        }

        private bool CanCheckOut()
        {
            return SelectedCustomer != null && SelectedCustomer.Handled;
        }

        private bool CanDeleteCustomers()
        {
            return BookingWr.Customers.Any(c => c.IsSelected);
        }

        private bool CanDeleteExtraservice()
        {
            return SelectedExtraService != null && AreContexesFree;
        }

        private bool CanDeletePayment()
        {
            return SelectedPayment != null && AreContexesFree;
        }

        private bool CanDeleteTransaction()
        {
            return SelectedTransaction != null;
        }

        private bool CanExpand()
        {
            return SelectedRoomTypeCount != null;
        }

        private bool CanMakeNoNameReservation(RoomType roomType)
        {
            if (BookingWr == null || FilteredRoomList == null || (SelectedCustomer == null && !All) || !AreBookingDataValid || !AreContexesFree ||
                !FilteredRoomList.Any(o => o.Hotel.NoName &&
                ((o.RoomType.MinCapacity <= NumOfSelectedCustomers && o.RoomType.MaxCapacity >= NumOfSelectedCustomers)
                || (NumOfSelectedCustomers == 1 && o.RoomType.MaxCapacity == 2))))
            {
                return false;
            }

            return true;
        }

        private bool CanOverBookHotel()
        {
            if (SelectedHotelIndex > 0)
            {
                if (SelectedRoomTypeIndex > 0)
                {
                    if (AreBookingDataValid)
                    {
                        if (SelectedCustomer != null || All)
                        {
                            if (AreContexesFree)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
            //           return SelectedHotelIndex > 0 && SelectedRoomTypeIndex > 0 && AreBookingDataValid && (SelectedCustomer != null || All) && AreContexesFree;
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

            ErrorsInCanAddReservationToRoom = "";

            return true;
        }

        private bool CanSave()
        {
            RaisePropertyChanged(nameof(CanEditDates));
            if (BookingWr == null || (!HasChanges && Payment.Amount == 0) || !AreContexesFree)
            {
                return false;
            }

            if (!AreBookingDataValid)
            {
                return false;
            }
            ErrorsInDatagrid = ValidateReservations();
            return ErrorsInDatagrid == null;
        }

        private bool CanShowAltairnatives()
        {
            return AvailableHotels != null && FilteredRoomList != null;
        }

        private bool CanShowFilteredRooms()
        {
            if (BookingWr == null || !AreContexesFree)
            {
                return false;
            }
            return SelectedHotelIndex >= 0 && SelectedRoomTypeIndex >= 0 && BookingWr != null && BookingWr.AreDatesValid();
        }

        private bool CanToggleDisability()
        {
            return BookingWr != null && !string.IsNullOrEmpty(BookingWr.CancelReason);
        }

        private bool CanUpdateAll()
        {
            return AreContexesFree;
        }

        private void CheckOutCustomers(bool all = false)
        {
            try
            {
                bool delete;
                foreach (Reservation r in BookingWr.ReservationsInBooking)
                {
                    delete = true;
                    if (all == false)
                        foreach (var c in r.CustomersList)
                        {
                            if (!BookingWr.Customers.Where(c1 => c1.Id == c.Id).FirstOrDefault().IsSelected)
                            {
                                delete = false;
                            }
                        }

                    if (delete)
                    {
                        var rw = new ReservationWrapper(r);
                        r.LastHotel = rw.HotelName;
                        r.LastRoomtype = rw.RoomTypeName;
                        r.LastCustomers = rw.Names;
                    }
                }

                List<Reservation> toRemove = new List<Reservation>();
                foreach (CustomerWrapper c in BookingWr.Customers)
                {
                    if ((c.IsSelected && c.Handled) || all)
                    {
                        foreach (Reservation r in BookingWr.ReservationsInBooking)
                        {
                            r.CustomersList.RemoveAll(x => x.Id == c.Id);
                        }
                        c.Handled = false;
                        c.RoomColor = new SolidColorBrush(System.Windows.Media.Colors.Green);
                        c.HotelName = "KENO";
                        c.RoomTypeName = "KENO";
                        c.RoomNumber = "ΌΧΙ";
                        c.Reservation = null;
                    }
                }
                foreach (Reservation r in BookingWr.ReservationsInBooking)
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
                AvailableHotels = null;

                Payment = new Payment();
                BookedMessage = string.Empty;
                HB = false;
                OnlyStay = false;
                SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == StaticResources.User.Id).FirstOrDefault());
                SelectedPartnerIndex = -1;
                IsPartners = false;
                SelectedHotelIndex = 0;
                RaisePropertyChanged(nameof(IsPartners));
                GenericRepository.RollBack();
            }
            RaisePropertyChanged(nameof(CanEditDates));
        }

        private void DeleteExtraService()
        {
            GenericRepository.Delete(SelectedExtraService);
        }

        private void DeletePayment()
        {
            GenericRepository.Delete(SelectedPayment);
        }

        private void DeleteTransaction()
        {
            GenericRepository.Delete(SelectedTransaction);
        }

        private void ExpandRooms()
        {
            if (SelectedRoomTypeCount != null && FilteredRoomList.Count > 0)
            {
                foreach (var room in FilteredRoomList)
                {
                    room.Hotel.IsExpanded = false;
                }
                foreach (var room in FilteredRoomList)
                {
                    if (room.RoomType == SelectedRoomTypeCount)
                    {
                        room.Hotel.IsExpanded = true;
                    }
                }
                foreach (var roomtype in RoomTypesCount)
                {
                    roomtype.IsExpanded = false;
                }
                SelectedRoomTypeCount.IsExpanded = true;
            }
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

        private async Task MakeNonameReservation(RoomType roomtype)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                NewReservationHelper.MakeNonameReservation(BookingWr, HB, All, OnlyStay);

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

        private async Task OpenInvoicesWindow(object obj)
        {
            InvoicesManagement_ViewModel vm = new InvoicesManagement_ViewModel(BasicDataManager, GenericRepository, booking: BookingWr, parameter: obj);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new InvoicesManagementWindow(), vm));
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

        private void PaymentChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void PrintContract()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                using (DocumentsManagement vm = new DocumentsManagement(BasicDataManager.Context))
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

        private void PrintReciept()
        {
            if (DocumentsManagement == null)
            {
                DocumentsManagement = new DocumentsManagement(GenericRepository);
            }
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Παρακαλώ επιλέξτε πελάτη");
                return;
            }
            DocumentsManagement.PrintPaymentsReciept(SelectedPayment, SelectedCustomer.Model);
        }

        private async Task PrintVoucher(bool send = false)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                using (DocumentsManagement vm = new DocumentsManagement(BasicDataManager.Context))
                {
                    await vm.PrintSingleBookingVoucher(new List<BookingWrapper> { BookingWr }, send);
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

                NewReservationHelper.PutCustomersInRoomAsync(BookingWr, new RoomWrapper(await GenericRepository.GetRoomById(SelectedRoom.Id)), All, OnlyStay, HB);
                if (BookingWr.ReservationsInBooking.Sum(r => r.CustomersList.Count) < BookingWr.Customers.Count)
                {
                    ShowFilteredRoomsCommand.Execute(false);
                }
                else
                {
                    FilteredRoomList.Clear();
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
                                            tmpCustomerWr.Name = ToLatin(sst.ChildElements[ssid].InnerText);
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
                                            tmpCustomerWr.Surename = ToLatin(sst.ChildElements[ssid].InnerText);
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
                                            if (tmpCustomerWr.Tel.Length < 10)
                                            {
                                                int ssid = int.Parse(c.CellValue.Text);
                                                if (ssid < sst.ChildElements.Count)
                                                {
                                                    tmpCustomerWr.Tel = sst.ChildElements[ssid].InnerText;
                                                    if (tmpCustomerWr.Tel.Length < 10)
                                                    {
                                                        tmpCustomerWr.Tel = string.Empty;
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Λάθος τηλέφωνο");
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
                                            string tmpValue = c.CellValue.Text.Replace('.', ',');
                                            if (tmpCustomerWr.Price < 1 && decimal.TryParse(tmpValue, out decimal price))
                                            {
                                                tmpCustomerWr.Price = Math.Round(price, 2); ;
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
                                                DateTime dt = DateTime.FromOADate(d);
                                                if (dt != new DateTime(1999, 2, 21))
                                                {
                                                    tmpCustomerWr.DOB = dt;
                                                }
                                                if (d < 1000)
                                                {
                                                    tmpCustomerWr.DOB = DateTime.Parse(sst.ChildElements[(int)d].InnerText);
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

        private async Task SaveAsync()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                BookingWr = await NewReservationHelper.SaveAsync(GenericRepository, Payment, BookingWr);
                FilteredRoomList.Clear();
                Payment = new Payment();
                BookedMessage = "H κράτηση αποθηκεύτηκε επιτυχώς";
                HasChanges = false;
                MessengerInstance.Send(new ReservationChanged_Message(BookingWr.Model));
                if (Parent is Cards_ViewModel cvm)
                {
                    await cvm.Refresh(this);
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

        private void ShowAlernatives()
        {
            var vm = new Availabilities_ViewModel(AvailableHotels, BookingWr.CheckIn, BookingWr.CheckOut, 
                SelectedRoomTypeIndex == 0 ? null : RoomTypes[SelectedRoomTypeIndex - 1], SelectedHotelIndex == 0 ? null : Hotels[SelectedHotelIndex - 1]);
            Availabilities_Window window = new Availabilities_Window
            {
                DataContext = vm
            };
            window.ShowDialog();
        }

        private void ToggleDisability()
        {
            if (BookingWr.Disabled)
            {
                BookingWr.Disabled = false;
            }
            else
            {
                BookingWr.DisableDate = DateTime.Now;
                BookingWr.DisabledBy = GenericRepository.GetById<User>(StaticResources.User.Id);
                BookingWr.Disabled = true;
                foreach (var c in BookingWr.Customers)
                {
                    c.BusGo = null;
                    c.BusReturn = null;
                }
            }
        }

        private string ToLatin(string innerText)
        {
            string toReturn = string.Empty;
            string str = innerText.ToUpper();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == ' ')
                    toReturn += c;
                else
                {
                    switch ((int)c)
                    {
                        case '-':
                        case '_':
                            toReturn += ' ';
                            break;

                        case 'Α':
                        case 902:
                            toReturn += 'A';
                            break;

                        case 'Β':
                            toReturn += 'V';
                            break;

                        case 'Γ':
                            toReturn += 'G';
                            break;

                        case 'Δ':
                            toReturn += 'D';
                            break;

                        case 'Ε':
                        case 'Έ':
                            toReturn += 'E';
                            break;

                        case 'Ζ':
                            toReturn += 'Z';
                            break;

                        case 'Η':
                        case 'Ή':
                        case 'Ι':
                        case 'Ί':
                            toReturn += 'I';
                            break;

                        case 'Υ':
                        case 'Ύ':
                            if (i > 0 && (str[i - 1] == 'Ο' || str[i - 1] == 'Ό'))
                                toReturn += 'Y';
                            else
                                toReturn += 'I';
                            break;

                        case 'Θ':
                            toReturn += 'T';
                            toReturn += 'H';
                            break;

                        case 'Κ':
                            toReturn += 'K';
                            break;

                        case 'Λ':
                            toReturn += 'L';
                            break;

                        case 'Μ':
                            toReturn += 'M';
                            break;

                        case 'Ν':
                            toReturn += 'N';
                            break;

                        case 'Ξ':
                            toReturn += 'K';
                            toReturn += 'S';
                            break;

                        case 'Ο':
                        case 'Ό':
                        case 'Ω':
                        case 'Ώ':
                            toReturn += 'O';
                            break;

                        case 'Π':
                            toReturn += 'P';
                            break;

                        case 'Ρ':
                            toReturn += 'R';
                            break;

                        case 'Σ':
                            toReturn += 'S';
                            break;

                        case 'Τ':
                            toReturn += 'T';
                            break;

                        case 'Φ':
                            toReturn += 'F';
                            break;

                        case 'Χ':
                            toReturn += 'X';
                            break;

                        case 'Ψ':
                            toReturn += 'P';
                            toReturn += 'S';
                            break;

                        default:
                            break;
                    }
                }
                if (toReturn.Any(c1 => c1 < 'A' && c1 > 'Z' && c1 != ' ' && c1 != '_' && c1 != '-'))
                {
                }
            }
            return toReturn.Trim();
        }

        private async Task TryChangeBooking()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SelectedExcursionToChange != null && BookingWr.Excursion != null && SelectedExcursionToChange.Id != BookingWr.Excursion.Id)
            {
                BookingWr.Excursion = await GenericRepository.GetFullExcursionByIdAsync(SelectedExcursionToChange.Id);
                CheckOutCustomers(true);
                SelectedExcursion = new ExcursionWrapper(SelectedExcursionToChange);
                Hotels = new ObservableCollection<Hotel>(BasicDataManager.Hotels.Where(h => h.City.Id == SelectedExcursion.Destinations[0].Id));
                if (BookingWr.CheckIn < DateTime.Today)
                {
                    BookingWr.CheckIn = DateTime.Today;
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task UpdateAll()
        {
            await ResetAllRefreshableDataASync(true);
            UpdateAllCommand.RaiseCanExecuteChanged();
        }

        //private void ReloadViewModel()
        //{
        //    UOW.Reload();
        //    SelectedExcursion = UOW.GenericRepository.GetById<Excursion>(SelectedExcursion.Id);
        //}
        private string ValidateBooking()
        {
            return BookingWr.ValidateBooking();
        }

        private string ValidatePayment()
        {
            if (Payment.Amount > (BookingWr.Remaining + 0.05m))
            {
                return "Το ποσό υπερβαίνει το υπόλοιπο της κράτησης";
            }

            if (Payment.Amount < 0)
            {
                return "Το ποσό πληρωμής δεν μπορεί να είναι αρνητικό";
            }
            if (Payment.Amount > (BookingWr.Remaining + 0.05m))
            {
                return "Το ποσό πληρωμής υπερβαίνει το υπολειπόμενο ποσό";
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
                if ((BookingWr.IsPartners && BookingWr.NetPrice == 0) || (!BookingWr.IsPartners && customer.Price == 0))
                {
                    return "Δεν έχετε ορίσει τιμή";
                }
            }
            return null;
        }

        private void ValidateRoom(RoomWrapper room)
        {
            //if (!string.IsNullOrEmpty(room.LocalNote))
            //{
            //    room.Rating=0;
            //    return;
            //}
            var before = room.PlanDailyInfo.FirstOrDefault(d => d.Date == BookingWr.CheckIn.AddDays(-1));
            var after = room.PlanDailyInfo.FirstOrDefault(d => d.Date == BookingWr.CheckOut);
            if (before == null || before.RoomState != RoomStateEnum.Available)
            {
            }
            else if (before.RoomState == RoomStateEnum.Available)
            {
                before = room.PlanDailyInfo.FirstOrDefault(d => d.Date == BookingWr.CheckIn.AddDays(-2));
                if (before == null || before.RoomState != RoomStateEnum.Available)
                {
                }
            }
            if (after == null || after.RoomState != RoomStateEnum.Available)
            {
            }
            else if (after.RoomState == RoomStateEnum.Available)
            {
                after = room.PlanDailyInfo.FirstOrDefault(d => d.Date == BookingWr.CheckOut.AddDays(1));
                if (after == null || after.RoomState != RoomStateEnum.Available)
                {
                }
            }
            room.Rating = ((before != null && before.RoomState != RoomStateEnum.Available) ? 1 : 0) + ((after != null && after.RoomState != RoomStateEnum.Available) ? 1 : 0);
        }

        #endregion Methods
    }

    public class RoomsInHotel
    {
        #region Properties

        public Hotel Hotel { get; set; }
        public List<RoomWrapper> Rooms { get; set; }

        #endregion Properties
    }

    public class RoomTypeWrapper
    {
        #region Properties

        public int AvailableRooms { get; set; }
        public ObservableCollection<RoomsInHotel> Hotels { get; set; }
        public int RoomsCounter { get; set; }
        public RoomType RoomType { get; set; }

        #endregion Properties
    }
}