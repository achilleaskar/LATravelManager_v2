using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class NewReservationGroup_Base : MyViewModelBaseAsync
    {
        #region Constructors

        public NewReservationGroup_Base(MainViewModel mainViewModel)
        {
            StartingRepository = mainViewModel.StartingRepository;
            BasicDataManager = mainViewModel.BasicDataManager;
            NewReservationHelper = new NewReservationHelper();
            RoomsManager = new RoomsManager();

            //Commands
            ShowFilteredRoomsCommand = new RelayCommand(async () => { await ShowFilteredRoomsAsync(); }, CanShowFilteredRooms);
            ClearBookingCommand = new RelayCommand(async () => { await ClearBooking(); });
            ReadNextLineCommand = new RelayCommand(ReadNextLine);
            AddCustomerCommand = new RelayCommand(AddRandomCustomer);
            AddFromFileCommand = new RelayCommand(AddFromFile);

            DeletePaymentCommand = new RelayCommand(DeletePayment, CanDeletePayment);
            UpdateAllCommand = new RelayCommand(async () => { await UpdateAll(); }, CanUpdateAll);

            BookRoomNoNameCommand = new RelayCommand<RoomType>(async (obj) => { await MakeNonameReservation(obj); }, CanMakeNoNameReservation);
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
            Emails = new ObservableCollection<Email>();
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
        private ObservableCollection<Email> _Emails;

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

        private Email _SelectedEmail;

        private ExcursionWrapper _SelectedExcursion;

        private int _SelectedHotelIndex;

        private int _SelectedPartnerIndex = -1;

        private Payment _SelectedPayment;

        private RoomWrapper _SelectedRoom;

        private int _SelectedRoomTypeIndex;

        private int _SelectedUserIndex;

        private ObservableCollection<User> _Users;

        private int _NumOfSelectedCustomers;

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

        public RelayCommand CheckOutCommand { get; set; }

        public RelayCommand ClearBookingCommand { get; set; }

        public RelayCommand DeletePaymentCommand { get; set; }

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
                _Payment.PropertyChanged += PaymentChanged;
                RaisePropertyChanged();
            }
        }

        public RelayCommand PrintContractCommand { get; set; }
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

        public Email SelectedEmail
        {
            get
            {
                return _SelectedEmail;
            }

            set
            {
                if (_SelectedEmail == value)
                {
                    return;
                }

                _SelectedEmail = value;
                if (value != null && BookingWr.Partner != null)
                {
                    BookingWr.PartnerEmail = value.EValue;
                }
                RaisePropertyChanged();
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
                    BookingWr.Partner = StartingRepository.GetById<Partner>(Partners[SelectedPartnerIndex].Id);
                    Emails = (BookingWr.Partner.Emails != null && BookingWr.Partner.Emails.Count() > 0) ? new ObservableCollection<Email>(BookingWr.Partner.Emails.Split(',').Select(e => new Email(e))) : new ObservableCollection<Email>();
                    if (Emails.Count > 0)
                    {
                        SelectedEmail = Emails[0];
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




        private ObservableCollection<RoomTypeWrapper> _FilteredRoomTypesList;


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
                    BookingWr.User = StartingRepository.GetById<User>(Users[SelectedUserIndex].Id);
                }
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowFilteredRoomsCommand { get; set; }
        public GenericRepository StartingRepository { get; set; }
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

        private bool AreContexesFree => (BasicDataManager != null && BasicDataManager.IsContextAvailable) && (StartingRepository != null && StartingRepository.IsContextAvailable);

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
                MessengerInstance.Send(new ShowExceptionMessage_Message("Καποιοι πελάτες δεν διαγράφηκαν επειδή συμμετέχουν σε κράτηση. Παλακαλώ κάντε τους CheckOut και δοκιμάστε ξανά!"));
            }
        }

        public override abstract Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        public override async Task ReloadAsync()
        {
            await ResetAllRefreshableDataASync();
        }

        public async Task ShowFilteredRoomsAsync()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                //if (reset)
                //{
                //    if (BasicDataManager != null && !BasicDataManager.IsTaskOk)
                //    {
                //        await BasicDataManager.LastTask;
                //    }
                //    BasicDataManager = new GenericRepository();
                //}
                AvailableHotels = new ObservableCollection<HotelWrapper>((await RoomsManager.GetAllAvailableRooms(BookingWr.CheckIn, BookingWr.CheckOut, SelectedExcursion, unSavedBooking: BookingWr.Model)));
                FilteredRoomList = new ObservableCollection<RoomWrapper>();
                List<RoomWrapper> tmplist = new List<RoomWrapper>();

                int allotmentDays = 0;
                bool addThis, isfree;



                foreach (HotelWrapper hotel in AvailableHotels)
                {
                    {
                        foreach (RoomWrapper room in hotel.RoomWrappers)
                        {
                            allotmentDays = 0;
                            isfree = addThis = true;

                            foreach (PlanDailyInfo pi in room.PlanDailyInfo)
                            {
                                if (!addThis)
                                {
                                    break;
                                }
                                addThis &= pi.RoomState == RoomStateEnum.Available
                                    || pi.RoomState == RoomStateEnum.MovableNoName
                                    || pi.RoomState == RoomStateEnum.Allotment;
                                isfree &= pi.RoomState == RoomStateEnum.Available;

                                //if (room.PlanDailyInfo[i].IsAllotment)
                                //{
                                //    allotmentDays++;
                                //}
                            }
                            if (isfree)
                                room.RoomType.freeRooms++;
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

                                tmplist.Add(room);
                            }
                        }
                    }
                }
                //TODO an mporei na fygei

                FilteredRoomList = new ObservableCollection<RoomWrapper>(tmplist.OrderBy(f => f.RoomType.MinCapacity).ToList());
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(FilteredRoomList);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription(nameof(RoomType));
                view.GroupDescriptions.Add(groupDescription);
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
            //    //TODO an mporei na fygei

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
            }
        }

        protected async Task<Booking> CreateNewBooking()
        {
            var b = new Booking { CheckIn = BookingWr != null ? BookingWr.CheckIn : new DateTime(2019, 06, 14), CheckOut = BookingWr != null ? BookingWr.CheckOut : new DateTime(2019, 06, 17) };
            var e = await StartingRepository.GetExcursionByIdAsync(SelectedExcursion.Id, true);
            var u = await StartingRepository.GetByIdAsync<User>(StaticResources.User.Id, true);
            b.User = u;
            b.Excursion = e;

            return b;
            //return new Booking { Excursion = e, CheckIn = BookingWr != null ? BookingWr.CheckIn : new DateTime(2018, 12, 28), CheckOut = BookingWr != null ? BookingWr.CheckOut : new DateTime(2018, 12, 30), User = u};
        }

        protected void InitializeBooking(Booking booking)
        {
            BookingWr = new BookingWrapper(booking);

            BookingWr.PropertyChanged += (s, e) =>
            {
                string x = e.PropertyName;
                if (!HasChanges)
                {
                    HasChanges = StartingRepository.HasChanges();
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

        protected async Task ResetAllRefreshableDataASync(bool makeNew = false)
        {
            if (SelectedExcursion == null)
            {
                SelectedExcursion = new ExcursionWrapper(await StartingRepository.GetExcursionByIdAsync(BookingWr.Excursion.Id));
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
                    int partnerId = SelectedPartnerIndex >= 0 && Partners != null && SelectedPartnerIndex < Partners.Count ? Partners[SelectedPartnerIndex].Id : -1;
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
#if DEBUG
                    if (SelectedRoomTypeIndex == 0)
                    {
                        SelectedRoomTypeIndex = 1;
                    }
#endif
                    if (BookingWr.Id > 0 && BookingWr.IsPartners)
                    {
                        SelectedPartnerIndex = Partners.IndexOf(Partners.Where(p => p.Id == BookingWr.Partner.Id).FirstOrDefault());
                        SelectedEmail = Emails.Where(e => e.EValue == BookingWr.PartnerEmail).FirstOrDefault();
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

        private bool CanMakeNoNameReservation(RoomType roomType)
        {//TODO tsekare to opws kai ola ta TODO
            if (roomType == null || (NumOfSelectedCustomers > 1 && !(roomType.MinCapacity <= NumOfSelectedCustomers && roomType.MaxCapacity >= NumOfSelectedCustomers))
                || (NumOfSelectedCustomers == 1 && roomType.MinCapacity != 2))
            {
                return false;
            }

            if (SelectedRoomTypeIndex <= 0 || (SelectedCustomer == null && !All) || !AreBookingDataValid || !AreContexesFree)
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
                        if ((SelectedCustomer != null || All))
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
            return true;
        }

        private bool CanSave()
        {
            if (BookingWr == null || (!HasChanges && Payment.Amount == 0) || !AreContexesFree)
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
                        StartingRepository.Delete(r);
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
            StartingRepository.RejectChanges();
        }

        private void DeletePayment()
        {
            StartingRepository.Delete(SelectedPayment);
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

                NewReservationHelper.MakeNonameReservation(BookingWr, await StartingRepository.GetByIdAsync<RoomType>(roomtype.Id), HB, All, OnlyStay);

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
                      BookingWr, await StartingRepository.GetByIdAsync<Hotel>(Hotels[SelectedHotelIndex - 1].Id),
                      await StartingRepository.GetByIdAsync<RoomType>(RoomTypes[SelectedRoomTypeIndex - 1].Id), All, OnlyStay, HB);
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

        private async Task PrintVoucher()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                using (DocumentsManagement vm = new DocumentsManagement(BasicDataManager.Context))
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

                NewReservationHelper.PutCustomersInRoomAsync(BookingWr, new RoomWrapper(await StartingRepository.GetRoomById(SelectedRoom.Id)), All, OnlyStay, HB);
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

                BookingWr = await NewReservationHelper.SaveAsync(StartingRepository, Payment, BookingWr);
                FilteredRoomList.Clear();
                Payment = new Payment();
                BookedMessage = "H κράτηση αποθηκέυτηκε επιτυχώς";
                HasChanges = false;
                MessengerInstance.Send(new ReservationChanged_Message(BookingWr.Model));
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
            if (Payment.Amount > BookingWr.Remaining)
            {
                return "Το ποσό υπερβαίνει το υπόλοιπο της κράτησης";
            }

            if (Payment.Amount < 0)
            {
                return "Το ποσό πληρωμής δεν μπορεί να είναι αρνητικό";
            }
            if (Payment.Amount > BookingWr.Remaining)
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
                if (BookingWr.NetPrice == 0 && customer.Price == 0)
                {
                    return "Δέν έχετε ορίσει τιμή";
                }
            }
            return null;
        }

        #endregion Methods
    }

    public class RoomTypeWrapper
    {
        public RoomType RoomType { get; set; }
        public int RoomsCounter { get; set; }
        public int AvailableRooms { get; set; }

        public ObservableCollection<RoomsInHotel> Hotels { get; set; }
    }

    public class RoomsInHotel
    {
        public Hotel Hotel { get; set; }
        public List<RoomWrapper> Rooms { get; set; }


    }
}