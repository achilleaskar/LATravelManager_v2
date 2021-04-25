using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Views.Personal;
using LATravelManager.UI.Views.ThirdParty;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class CardItem : ObservableObject
    {
        #region Properties

        public DateTime Date { get; set; }

        public string Description { get; set; }
        public string Number { get; set; }

        //public RecieptTypeEnum RecieptType { get; set; }
        public CardTypeEnum CardType { get; set; }

        public decimal Cost { get; set; }
        public decimal Deposit { get; set; }
        public decimal Remaining { get; set; }

        #endregion Properties
    }

    public class Cards_ViewModel : MyViewModelBase
    {
        #region Constructors

        public Cards_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ShowReservationsCommand = new RelayCommand<string>(async (obj) => { await ShowReservations(obj); }, CanShowReservations);
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            AddBulkPayment_Command = new RelayCommand(async () => await AddBulkPayment(), CanAddBulkPayment);
            CommitPayment_Command = new RelayCommand(async () => await CommitPayment(), CanCommitPayment);
            PayFromBulkCommand = new RelayCommand<ReservationWrapper>(PayFromBulk, CanPayFromBulk);
            ClosePopup_Command = new RelayCommand(() => IsPaymentPopupOpen = false);

            CreateSalesPreview_Command = new RelayCommand(async () => await CreateSalesPreview(), CanCreateSalesPreview);

            Load();
        }

        #endregion Constructors

        #region Fields

        private decimal _Available;

        private ObservableCollection<Booking> _Bookings;

        private ICollectionView _BookingsCollectionView;

        private ObservableCollection<BulkPayment> _BulkPayments;

        private bool _ByCreated;

        private DateTime _CheckIn;

        private DateTime _CheckOut;

        private decimal _Commision;

        private bool _Completed;

        private GenericRepository _Context;

        private int _DepartmentIndexBookingFilter;

        private int _ExcursionCategoryIndexBookingFilter;

        private int _ExcursionIndexBookingFilter;

        private ObservableCollection<Excursion> _Excursions;

        private ICollectionView _ExcursionsCollectionView;

        private ObservableCollection<ReservationWrapper> _FilteredReservations;

        private string _FilterString;

        private bool _IsOk = true;

        private bool _IsPaymentPopupOpen;

        private BulkPayment _NewPayment;

        private bool _NoProforma;

        private bool _NoReciept;

        private bool _NoVoucher;

        private int _PartnerIndex = -1;

        private ObservableCollection<Partner> _Partners;

        private decimal _PaymentAmmount;

        private ObservableCollection<Payment> _Payments;

        private bool _Remaining;

        private decimal _RemainingA;

        private GenericRepository _SalesContext;

        private DateTime _SalesFrom = DateTime.Today;

        private BulkPayment _SelectedBulkPayment;

        private Excursion _SelectedExcursionFilter;

        private Payment _SelectedPayment;

        private ReservationWrapper _SelectedReservation;

        private decimal _Total;

        private bool _ΒyCheckIn;

        #endregion Fields

        #region Properties

        public RelayCommand AddBulkPayment_Command { get; }

        public decimal Available
        {
            get
            {
                return _Available;
            }

            set
            {
                if (_Available == value)
                {
                    return;
                }

                _Available = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Booking> Bookings
        {
            get
            {
                return _Bookings;
            }

            set
            {
                if (_Bookings == value)
                {
                    return;
                }

                _Bookings = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<BulkPayment> BulkPayments
        {
            get
            {
                return _BulkPayments;
            }

            set
            {
                if (_BulkPayments == value)
                {
                    return;
                }

                _BulkPayments = value;
                if (CollectionViewSource.GetDefaultView(BulkPayments).SortDescriptions.Count == 0)
                {
                    CollectionViewSource.GetDefaultView(BulkPayments).SortDescriptions.Add(new SortDescription(nameof(BulkPayment.Date), ListSortDirection.Ascending));
                }
                RaisePropertyChanged();
            }
        }

        public bool ByCheckIn
        {
            get
            {
                return _ΒyCheckIn;
            }

            set
            {
                if (_ΒyCheckIn == value)
                {
                    return;
                }

                _ΒyCheckIn = value;
                if (ReservationsCollectionView != null && ReservationsCollectionView.SortDescriptions != null)
                {
                    ReservationsCollectionView.SortDescriptions.Clear();
                    ReservationsCollectionView.SortDescriptions.Add(new SortDescription("PartnerName", ListSortDirection.Ascending));

                    if (ByCheckIn)
                        ReservationsCollectionView.SortDescriptions.Add(new SortDescription("CheckIn", ListSortDirection.Ascending));
                    else
                        ReservationsCollectionView.SortDescriptions.Add(new SortDescription("CreatedDate", ListSortDirection.Ascending));
                }
                RaisePropertyChanged();
            }
        }

        public bool ByCreated
        {
            get
            {
                return _ByCreated;
            }

            set
            {
                if (_ByCreated == value)
                {
                    return;
                }

                _ByCreated = value;
                if (ReservationsCollectionView != null && ReservationsCollectionView.SortDescriptions != null)
                {
                    ReservationsCollectionView.SortDescriptions.Clear();
                    ReservationsCollectionView.SortDescriptions.Add(new SortDescription("PartnerName", ListSortDirection.Ascending));

                    if (ByCheckIn)
                        ReservationsCollectionView.SortDescriptions.Add(new SortDescription("CheckIn", ListSortDirection.Ascending));
                    else
                        ReservationsCollectionView.SortDescriptions.Add(new SortDescription("CreatedDate", ListSortDirection.Ascending));
                }
                RaisePropertyChanged();
            }
        }

        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }

                _CheckIn = value;
                RaisePropertyChanged();
            }
        }

        public DateTime CheckOut
        {
            get
            {
                return _CheckOut;
            }

            set
            {
                if (_CheckOut == value)
                {
                    return;
                }

                _CheckOut = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ClosePopup_Command { get; }

        public decimal Commision
        {
            get
            {
                return _Commision;
            }

            set
            {
                if (_Commision == value)
                {
                    return;
                }

                _Commision = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CommitPayment_Command { get; }

        public bool Completed
        {
            get
            {
                return _Completed;
            }

            set
            {
                if (_Completed == value)
                {
                    return;
                }

                _Completed = value;
                ExcursionsCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        public GenericRepository Context
        {
            get
            {
                return _Context;
            }

            set
            {
                if (_Context == value)
                {
                    return;
                }

                _Context = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CreateSalesPreview_Command { get; set; }

        public int DepartmentIndexBookingFilter
        {
            get
            {
                return _DepartmentIndexBookingFilter;
            }

            set
            {
                if (_DepartmentIndexBookingFilter == value)
                {
                    return;
                }

                _DepartmentIndexBookingFilter = value;
                RaisePropertyChanged();
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    UpdatePayments();

                    CalculateAmounts();
                }
            }
        }

        public RelayCommand EditBookingCommand { get; }

        public int ExcursionCategoryIndexBookingFilter
        {
            get
            {
                return _ExcursionCategoryIndexBookingFilter;
            }

            set
            {
                if (_ExcursionCategoryIndexBookingFilter == value)
                {
                    return;
                }

                _ExcursionCategoryIndexBookingFilter = value;
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    UpdatePayments();

                    CalculateAmounts();
                }
                RaisePropertyChanged();
            }
        }

        public int ExcursionIndexBookingFilter
        {
            get
            {
                return _ExcursionIndexBookingFilter;
            }

            set
            {
                if (_ExcursionIndexBookingFilter == value)
                {
                    return;
                }

                _ExcursionIndexBookingFilter = value;
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    UpdatePayments();

                    CalculateAmounts();
                }
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Excursion> Excursions
        {
            get
            {
                return _Excursions;
            }

            set
            {
                if (_Excursions == value)
                {
                    return;
                }
                _Excursions = value;

                if (Excursions != null && !Excursions.Any(e => e.Id == 0))
                {
                    Excursions.Insert(0, new Excursion { ExcursionDates = new ObservableCollection<ExcursionDate> { new ExcursionDate { CheckIn = new DateTime() } }, Name = "Όλες", Id = 0 });
                }

                ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);
                ExcursionsCollectionView.Filter = CustomerExcursionsFilter;
                ExcursionsCollectionView.SortDescriptions.Add(new SortDescription("FirstDate", ListSortDirection.Ascending));

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ExcursionsCollectionView));
            }
        }

        public ICollectionView ExcursionsCollectionView
        {
            get
            {
                return _ExcursionsCollectionView;
            }

            set
            {
                if (_ExcursionsCollectionView == value)
                {
                    return;
                }

                _ExcursionsCollectionView = value;
                ExcursionsCollectionView.CurrentChanged += ExcursionsCollectionView_CurrentChanged;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ReservationWrapper> FilteredReservations
        {
            get
            {
                return _FilteredReservations;
            }

            set
            {
                if (_FilteredReservations == value)
                {
                    return;
                }

                _FilteredReservations = value;
                ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
                RaisePropertyChanged();
            }
        }

        public string FilterString
        {
            get
            {
                return _FilterString;
            }

            set
            {
                if (_FilterString == value)
                {
                    return;
                }

                _FilterString = value;
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    UpdatePayments();
                    CalculateAmounts();
                }
                RaisePropertyChanged();
            }
        }

        public bool IsOk
        {
            get
            {
                return _IsOk;
            }

            set
            {
                if (_IsOk == value)
                {
                    return;
                }

                _IsOk = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPaymentPopupOpen
        {
            get
            {
                return _IsPaymentPopupOpen;
            }

            set
            {
                if (_IsPaymentPopupOpen == value)
                {
                    return;
                }

                _IsPaymentPopupOpen = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public BulkPayment NewPayment
        {
            get
            {
                return _NewPayment;
            }

            set
            {
                if (_NewPayment == value)
                {
                    return;
                }

                _NewPayment = value;
                RaisePropertyChanged();
            }
        }

        public bool NoProforma
        {
            get
            {
                return _NoProforma;
            }

            set
            {
                if (_NoProforma == value)
                {
                    return;
                }

                _NoProforma = value;
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    UpdatePayments();

                    CalculateAmounts();
                }
                RaisePropertyChanged();
            }
        }

        public bool NoReciept
        {
            get
            {
                return _NoReciept;
            }

            set
            {
                if (_NoReciept == value)
                {
                    return;
                }

                _NoReciept = value;
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    UpdatePayments();

                    CalculateAmounts();
                }
                RaisePropertyChanged();
            }
        }

        public bool NoVoucher
        {
            get
            {
                return _NoVoucher;
            }

            set
            {
                if (_NoVoucher == value)
                {
                    return;
                }

                _NoVoucher = value;
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    UpdatePayments();

                    CalculateAmounts();
                }
                RaisePropertyChanged();
            }
        }

        public int PartnerIndex
        {
            get
            {
                return _PartnerIndex;
            }

            set
            {
                if (_PartnerIndex == value)
                {
                    return;
                }

                _PartnerIndex = value;
                RaisePropertyChanged();
            }
        }

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

        public RelayCommand<ReservationWrapper> PayFromBulkCommand { get; }

        public decimal PaymentAmmount
        {
            get
            {
                return _PaymentAmmount;
            }

            set
            {
                if (_PaymentAmmount == value)
                {
                    return;
                }

                //if (value > Available)
                //{
                //    value = Available;
                //}
                //if (SelectedReservation != null && value > SelectedReservation.Remaining)
                //{
                //    value = SelectedReservation.Remaining;
                //}
                _PaymentAmmount = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Payment> Payments
        {
            get
            {
                return _Payments;
            }

            set
            {
                if (_Payments == value)
                {
                    return;
                }

                _Payments = value;
                if (CollectionViewSource.GetDefaultView(Payments).SortDescriptions.Count == 0)
                {
                    CollectionViewSource.GetDefaultView(Payments).SortDescriptions.Add(new SortDescription(nameof(Payment.Date), ListSortDirection.Ascending));
                }
                RaisePropertyChanged();
            }
        }

        public bool Remaining
        {
            get
            {
                return _Remaining;
            }

            set
            {
                if (_Remaining == value)
                {
                    return;
                }

                _Remaining = value;
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    UpdatePayments();

                    CalculateAmounts();
                }
                RaisePropertyChanged();
            }
        }

        public decimal RemainingA
        {
            get
            {
                return _RemainingA;
            }

            set
            {
                if (_RemainingA == value)
                {
                    return;
                }

                _RemainingA = value;
                RaisePropertyChanged();
            }
        }

        public ICollectionView ReservationsCollectionView
        {
            get
            {
                return _BookingsCollectionView;
            }

            set
            {
                if (_BookingsCollectionView == value)
                {
                    return;
                }

                _BookingsCollectionView = value;
                ReservationsCollectionView.Filter = CustomerFilter;
                if (ReservationsCollectionView != null && ReservationsCollectionView.SortDescriptions != null)
                {
                    ReservationsCollectionView.SortDescriptions.Clear();
                    ReservationsCollectionView.SortDescriptions.Add(new SortDescription("PartnerName", ListSortDirection.Ascending));

                    if (ByCheckIn)
                        ReservationsCollectionView.SortDescriptions.Add(new SortDescription("CheckIn", ListSortDirection.Ascending));
                    else
                        ReservationsCollectionView.SortDescriptions.Add(new SortDescription("CreatedDate", ListSortDirection.Ascending));
                }
                RaisePropertyChanged();
            }
        }

        public GenericRepository SalesContext
        {
            get
            {
                return _SalesContext;
            }

            set
            {
                if (_SalesContext == value)
                {
                    return;
                }

                _SalesContext = value;
                RaisePropertyChanged();
            }
        }

        public DateTime SalesFrom
        {
            get
            {
                return _SalesFrom;
            }

            set
            {
                if (_SalesFrom == value)
                {
                    return;
                }

                _SalesFrom = value;
                RaisePropertyChanged();
            }
        }

        public BulkPayment SelectedBulkPayment
        {
            get
            {
                return _SelectedBulkPayment;
            }

            set
            {
                if (_SelectedBulkPayment == value)
                {
                    return;
                }

                _SelectedBulkPayment = value;
                RaisePropertyChanged();
            }
        }

        public Excursion SelectedExcursionFilter
        {
            get
            {
                return _SelectedExcursionFilter;
            }

            set
            {
                if (_SelectedExcursionFilter == value)
                {
                    return;
                }

                _SelectedExcursionFilter = value;
                RaisePropertyChanged();
            }
        }

        public Payment SelectedPayment
        {
            get
            {
                return _SelectedPayment;
            }

            set
            {
                if (_SelectedPayment == value)
                {
                    return;
                }

                _SelectedPayment = value;
                RaisePropertyChanged();
                UpdateSelectedReservation();
            }
        }

        public ReservationWrapper SelectedReservation
        {
            get
            {
                return _SelectedReservation;
            }

            set
            {
                if (_SelectedReservation == value)
                {
                    return;
                }

                _SelectedReservation = value;
                IsPaymentPopupOpen = false;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<string> ShowReservationsCommand { get; set; }

        public decimal Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                Context = new GenericRepository();

                Partners = new ObservableCollection<Partner>(MainViewModel.BasicDataManager.Partners);
                Excursions = new ObservableCollection<Excursion>(MainViewModel.BasicDataManager.Excursions);

                Bookings = new ObservableCollection<Booking>();
                PartnerIndex = -1;
                ByCreated = true;
                CheckOut = DateTime.Today;
                CheckIn = DateTime.Today.AddMonths(-1);
                DepartmentIndexBookingFilter = StaticResources.User.BaseLocation;
                FilteredReservations = new ObservableCollection<ReservationWrapper>();
                Payments = new ObservableCollection<Payment>();
                BulkPayments = new ObservableCollection<BulkPayment>();
                NewPayment = new BulkPayment();
#if DEBUG
                PartnerIndex = Partners.IndexOf(Partners.FirstOrDefault(t => t.Name == "KAZAKIS"));
#endif
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

        public override void Reload()
        {
        }

        internal void CalculateAmounts()
        {
            _Total = _RemainingA = _Available = _Commision = 0;
            foreach (ReservationWrapper res in ReservationsCollectionView)
            {
                _Total += res.FullPrice;
                _RemainingA += res.Remaining;
                if (res.Booking != null && res.Booking.IsPartners)
                {
                    _Commision += res.Booking.Commision;
                }
            }
            foreach (var item in BulkPayments)
            {
                _Available += item.Remaining;
            }
            _RemainingA -= Available;
            RaisePropertyChanged(nameof(RemainingA));
            RaisePropertyChanged(nameof(Total));
            RaisePropertyChanged(nameof(Available));
            RaisePropertyChanged(nameof(Commision));
        }

        private async Task AddBulkPayment()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            NewPayment.Date = DateTime.Now;
            NewPayment.PartnerId = Partners[PartnerIndex].Id;
            Context.Add(NewPayment);
            await Context.SaveAsync();
            BulkPayments.Add(NewPayment);
            NewPayment = new BulkPayment();
            CalculateAmounts();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool CanAddBulkPayment()
        {
            return PartnerIndex >= 0 && NewPayment != null && NewPayment.Amount > 0;
        }

        private bool CanCommitPayment()
        {
            return SelectedReservation != null && PaymentAmmount <= SelectedReservation.Remaining && PaymentAmmount > 0 && Available >= PaymentAmmount;
        }

        private bool CanCreateSalesPreview()
        {
            return PartnerIndex >= 0;
        }

        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        private bool CanPayFromBulk(ReservationWrapper obj)
        {
            return obj != null && obj.Remaining > 0 && Available > 0;
        }

        private bool CanShowReservations(string arg)
        {
            return IsOk;
        }

        private async Task CommitPayment()
        {
            var bulks = BulkPayments.Where(b => b.Remaining > 0).OrderBy(b => b.Date).ToList();
            if (bulks.Count == 0)
            {
                return;
            }
            if (SelectedReservation != null)
            {
                Mouse.OverrideCursor = Cursors.Arrow;

                var selectedBulk = bulks[0];
                List<Payment> payments = new List<Payment>();
                decimal remaining = PaymentAmmount;

                if (selectedBulk.Remaining >= remaining)
                {
                    payments.Add(new Payment
                    {
                        Amount = PaymentAmmount,
                        BulkPayment = selectedBulk,
                        Checked = true,
                        Comment = $"{DateTime.Now.ToShortDateString()} από απόθεμα",
                        Date = selectedBulk.Date,
                        PaymentMethod = selectedBulk.PaymentMethod,
                        User = Context.GetById<User>(StaticResources.User.Id)
                    });
                }
                else
                {
                    int i = 0;
                    while (remaining > 0 && i < bulks.Count)
                    {
                        selectedBulk = bulks[i];
                        if (selectedBulk.Remaining <= remaining)
                        {
                            payments.Add(new Payment
                            {
                                Amount = selectedBulk.Remaining,
                                BulkPayment = selectedBulk,
                                Checked = true,
                                Comment = $"{DateTime.Now.ToShortDateString()} από απόθεμα",
                                Date = selectedBulk.Date,
                                PaymentMethod = selectedBulk.PaymentMethod,
                                User = Context.GetById<User>(StaticResources.User.Id)
                            });
                            remaining -= selectedBulk.Remaining;
                        }
                    }
                }
                if (SelectedReservation.Booking != null)
                {
                    foreach (var p in payments)
                    {
                        SelectedReservation.Booking.Payments.Add(p);
                    }
                }
                if (Context.HasChanges())
                {
                    await Context.SaveAsync();
                }

                SelectedReservation.CalculateAmounts();

                CalculateAmounts();

                IsPaymentPopupOpen = false;

                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        internal async Task Refresh(MyViewModelBaseAsync vm)
        {
            try
            {
                if (vm is NewReservationGroup_Base nr)
                {
                    var bl = Context.Context.Bookings.FirstOrDefault(b => b.Id == nr.BookingWr.Model.Id);
                    //Context.Context.ReloadNavigationProperty(bl,r=>r.Payments);
                    await ((IObjectContextAdapter)Context.Context).ObjectContext.RefreshAsync(RefreshMode.StoreWins, bl.Payments);

                    var t = new ReservationWrapper((await Context.GetFullBookingByIdAsync(bl.Id)).ReservationsInBooking[0]);
                    FilteredReservations[FilteredReservations.IndexOf(SelectedReservation)] = t;
                    SelectedReservation = t;
                }
                else if (vm is NewReservation_Personal_ViewModel pm)
                {
                    var personal = await Context.GetFullPersonalBookingByIdAsync(pm.PersonalWr.Id);
                    SelectedReservation = new ReservationWrapper { Id = personal.Id, PersonalModel = new Personal_BookingWrapper(personal), CreatedDate = personal.CreatedDate, CustomersList = personal.Customers.ToList() };
                }
                else if (vm is NewReservation_ThirdParty_VIewModel rt)
                {
                    var tp = await Context.GetFullThirdPartyBookingByIdAsync(rt.ThirdPartyWr.Id);
                    SelectedReservation = new ReservationWrapper { Id = tp.Id, ThirdPartyModel = new ThirdParty_Booking_Wrapper(tp), CreatedDate = tp.CreatedDate, CustomersList = tp.Customers.ToList() };
                }

                SelectedReservation.CalculateAmounts();
                CalculateAmounts();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private ObservableCollection<CardItem> _SalesItems;

        public ObservableCollection<CardItem> SalesItems
        {
            get
            {
                return _SalesItems;
            }

            set
            {
                if (_SalesItems == value)
                {
                    return;
                }

                _SalesItems = value;
                RaisePropertyChanged();
            }
        }

        private async Task CreateSalesPreview()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                if (SalesContext != null)
                {
                    SalesContext.Dispose();
                }
                SalesContext = new GenericRepository();

                await SalesContext.GetAllCitiesAsyncSortedByName();
                await SalesContext.GetAllHotelsAsync<Hotel>();

                List<RecieptBase> reciepts = await SalesContext.GetAllRecieptsAsync(Partners[PartnerIndex].CompanyInfoId);

                ObservableCollection<BulkPayment> Bulkpayments = new ObservableCollection<BulkPayment>(await SalesContext.GetAllBulkPaymentsAsync(Partners[PartnerIndex].Id, DateTime.MinValue, DateTime.MaxValue));

                List<BookingWrapper> bookings = (await SalesContext.GetAllPartnersBookingsAsync(StaticResources.User.BaseLocation, Partners[PartnerIndex].Id)).Select(b => new BookingWrapper(b)).ToList();

                List<Personal_BookingWrapper> personals = (await SalesContext.GetAllPartnersPersonalBookingsFiltered(Partners[PartnerIndex].Id)).Select(p => new Personal_BookingWrapper(p)).ToList();
                var tmpSales = new List<CardItem>();

                SalesItems = new ObservableCollection<CardItem>();

                foreach (var r in reciepts)
                {
                    tmpSales.Add(new CardItem
                    {
                        CardType = CardTypeEnum.Reciept,
                        Cost = r.Total,
                        Date = r.Date,
                        Deposit = 0,
                        Description = r.RecieptDescription,
                        Number = r.GetNumber()
                    });
                }

                foreach (var r in Bulkpayments)
                {
                    tmpSales.Add(new CardItem
                    {
                        CardType = CardTypeEnum.Deposit,
                        Cost = r.Amount,
                        Date = r.Date,
                        Deposit = r.Amount,
                        Description = "Κατάθεση",
                        Number = "ΜΠ" + r.Id
                    });
                }

                foreach (var r in bookings)
                {
                    tmpSales.Add(new CardItem
                    {
                        CardType = CardTypeEnum.Booking,
                        Cost = r.GetAmountToPay(),
                        Date = r.CreatedDate,
                        Deposit = r.GetAmountToPay() * -1,
                        Description = r.GetPacketDescription(),
                        Number = r.GetNumber()
                    });

                    foreach (var p in r.Payments)
                    {
                        if (!p.IsBulk)
                            tmpSales.Add(new CardItem
                            {
                                CardType = CardTypeEnum.Deposit,
                                Cost = p.Amount,
                                Date = p.Date.AddSeconds(15),
                                Deposit = p.Amount,
                                Description = $"Κατάθεση για κράτηση {r.GetNumber()} - {r.CreatedDate.ToShortDateString()}",
                                Number = "Π" + p.Id
                            });
                    }
                }

                foreach (var r in personals)
                {
                    tmpSales.Add(new CardItem
                    {
                        CardType = CardTypeEnum.Booking,
                        Cost = r.GetAmountToPay(),
                        Date = r.CreatedDate,
                        Deposit = r.GetAmountToPay() * -1,
                        Description = r.GetPacketDescription(),
                        Number = r.GetNumber()
                    });

                    foreach (var p in r.Payments)
                    {
                        if (!p.IsBulk)
                            tmpSales.Add(new CardItem
                            {
                                CardType = CardTypeEnum.Deposit,
                                Cost = p.Amount,
                                Date = p.Date.AddSeconds(15),
                                Deposit = p.Amount,
                                Description = $"Κατάθεση για κράτηση {r.GetNumber()} - {r.CreatedDate.ToShortDateString()}",
                                Number = "Π" + p.Id
                            });
                    }
                }

                SalesItems = new ObservableCollection<CardItem>(tmpSales.OrderBy(r => r.Date));

                decimal remaining = 0;
                foreach (var item in SalesItems)
                {

                    remaining += item.Deposit;
                    item.Remaining = remaining;
                }

                // Parallel.ForEach(bookings, wr => { wr.CalculateRemainingAmount(); });
                // Parallel.ForEach(personals, wr => { wr.CalculateRemainingAmount(); });
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
                IsOk = true;
                RaisePropertyChanged(nameof(IsOk));
            }
        }

        private bool CustomerExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return Completed || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today) || excursion.Id == 0;
        }

        private bool CustomerFilter(object item)
        {
            ReservationWrapper reservation = item as ReservationWrapper;
            var x = reservation.Contains(FilterString, false, true) &&
                (!Remaining || reservation.Remaining > 3) &&
                (!NoProforma || !reservation.ProformaSent) &&
                (!NoVoucher || !reservation.VoucherSent) &&
                (!NoReciept || !reservation.Reciept) &&
                (DepartmentIndexBookingFilter == 0 || reservation.UserWr.BaseLocation == DepartmentIndexBookingFilter) &&
                (ExcursionCategoryIndexBookingFilter == 0 || (ExcursionCategoryIndexBookingFilter == 1 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Bansko) || (ExcursionCategoryIndexBookingFilter == 2 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Skiathos) || (ExcursionCategoryIndexBookingFilter == 3 && reservation.PersonalModel != null) || (ExcursionCategoryIndexBookingFilter == 4 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Group) || (ExcursionCategoryIndexBookingFilter == 5 && reservation.ThirdPartyModel != null)) &&
                (ExcursionIndexBookingFilter == 0 || (reservation.Booking != null && ExcursionsCollectionView.CurrentItem != null && ExcursionsCollectionView.CurrentItem is Excursion && reservation.Booking.Excursion != null && reservation.Booking.Excursion.Id == ((Excursion)ExcursionsCollectionView.CurrentItem).Id));
            if (!x)
            {

            }
            return x;
        }

        private async Task EditBooking()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                if (SelectedReservation.ExcursionType == ExcursionTypeEnum.Personal)
                {
                    NewReservation_Personal_ViewModel viewModel = new NewReservation_Personal_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.PersonalModel.Id, parent: this);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditPersonalBooking_Window(), viewModel));
                }
                else if (SelectedReservation.ExcursionType == ExcursionTypeEnum.ThirdParty)
                {
                    NewReservation_ThirdParty_VIewModel viewModel = new NewReservation_ThirdParty_VIewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.ThirdPartyModel.Id, parent: this);
                    MessengerInstance.Send(new OpenChildWindowCommand(new Edit_ThirdParty_Booking_Window(), viewModel));
                }
                else
                {
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.Booking.Id, parent: this);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }

                SelectedReservation.CalculateAmounts();
                CalculateAmounts();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private void ExcursionsCollectionView_CurrentChanged(object sender, EventArgs e)
        {
            if (ReservationsCollectionView != null)
            {
                ReservationsCollectionView.Refresh();
                UpdatePayments();

                CalculateAmounts();
            }
            SelectedExcursionFilter = ExcursionsCollectionView.CurrentItem as Excursion;
        }

        private void PayFromBulk(ReservationWrapper obj)
        {
            if (obj == null)
            {
                return;
            }
            SelectedReservation = obj;
            PaymentAmmount = Available >= obj.Remaining ? obj.Remaining : Available;
            IsPaymentPopupOpen = true;
        }

        private async Task ShowReservations(string parameter)
        {
            try
            {
                IsOk = false;
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                //await ReloadAsync();
                if (Context != null)
                {
                    Context.Dispose();
                }
                Context = new GenericRepository();
                DateTime From = DateTime.Today;
                DateTime To = DateTime.Today.AddDays(1);
                switch (parameter)
                {
                    case "1":
                        From = DateTime.Today.AddMonths(-1);
                        break;

                    case "3":
                        From = DateTime.Today.AddMonths(-3);
                        break;

                    case "6":
                        From = DateTime.Today.AddMonths(-6);
                        break;

                    case "10":
                        From = new DateTime();
                        break;

                    default:
                        From = CheckIn;
                        To = CheckOut;
                        break;
                }
                List<BookingWrapper> listg = (await Context.GetAllBookingsAsync(ExcursionIndexBookingFilter > 0 ? (ExcursionsCollectionView.CurrentItem as Excursion).Id : -1, ExcursionCategoryIndexBookingFilter > 0 ? ExcursionCategoryIndexBookingFilter - 1 : -1, DepartmentIndexBookingFilter, PartnerIndex >= 0 ? Partners[PartnerIndex].Id : -1, From, To, ByCheckIn, onlypartners: true)).Select(b => new BookingWrapper(b)).ToList();

                List<ReservationWrapper> listr = new List<ReservationWrapper>();
                foreach (var booking in listg)
                {
                    if (!listr.Any(r => r.Booking.Id == booking.Id && r.Booking.ReservationsInBooking != null && r.Booking.ReservationsInBooking.Count > 0))
                    {
                        listr.Add(new ReservationWrapper(booking.ReservationsInBooking[0]));
                    }
                }
                if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 3 || ExcursionCategoryIndexBookingFilter == 0))
                {
                    await Context.GetAllCitiesAsyncSortedByName();
                    await Context.GetAllHotelsAsync<Hotel>();
                    listr.AddRange((await Context.GetAllPersonalBookingsFiltered(0, true, new DateTime(), checkin: From, checkout: To, partnerId: PartnerIndex >= 0 ? Partners[PartnerIndex].Id : 0, onlyPartners: true)).Select(r => new ReservationWrapper { Id = r.Id, PersonalModel = new Personal_BookingWrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                }
                if (ExcursionIndexBookingFilter == 0 && ExcursionCategoryIndexBookingFilter == 5)
                {
                    listr.AddRange((await Context.GetAllThirdPartyBookingsFiltered(0, true, new DateTime(), checkin: From, checkout: To, byCheckIn: ByCheckIn)).Select(r => new ReservationWrapper { Id = r.Id, ThirdPartyModel = new ThirdParty_Booking_Wrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                }

                if (listr.Any(r=>r.Id==353))
                {

                }

                BulkPayments = new ObservableCollection<BulkPayment>(await Context.GetAllBulkPaymentsAsync(PartnerIndex > 0 ? Partners[PartnerIndex].Id : -1, From, To));

                FilteredReservations = new ObservableCollection<ReservationWrapper>(listr);
                Parallel.ForEach(FilteredReservations, wr => { wr.CalculateAmounts(); _ = wr.Partner; });
                UpdatePayments();
                CalculateAmounts();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
                IsOk = true;
                RaisePropertyChanged(nameof(IsOk));
            }
        }

        private void UpdatePayments()
        {
            List<Payment> payments = new List<Payment>();
            foreach (ReservationWrapper p in ReservationsCollectionView)
            {
                if (p.Booking != null && p.Booking.Payments != null)
                {
                    foreach (var pa in p.Booking.Payments)
                    {
                        if (pa.BulkPayment != null)
                        {
                            continue;
                        }
                        pa.Parent = p;
                        payments.Add(pa);
                    }
                }
                else if (p.PersonalModel != null && p.PersonalModel.Payments != null)
                {
                    foreach (var pa in p.PersonalModel.Payments)
                    {
                        if (pa.BulkPayment != null)
                        {
                            continue;
                        }
                        pa.Parent = p;
                        payments.Add(pa);
                    }
                }
                else if (p.ThirdPartyModel != null && p.ThirdPartyModel.Payments != null)
                {
                    foreach (var pa in p.ThirdPartyModel.Payments)
                    {
                        if (pa.BulkPayment != null)
                        {
                            continue;
                        }
                        pa.Parent = p;
                        payments.Add(pa);
                    }
                }
            }
            Payments = new ObservableCollection<Payment>(payments.OrderBy(p => p.Date));
        }

        private void UpdateSelectedReservation()
        {
            if (SelectedPayment != null && SelectedPayment.Parent != null)
            {
                foreach (ReservationWrapper wrapper in ReservationsCollectionView)
                {
                    wrapper.Selected = wrapper == SelectedPayment.Parent;
                }
                SelectedReservation = SelectedPayment.Parent;
            }
        }

        #endregion Methods
    }
}