using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
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
    public class Cards_ViewModel : MyViewModelBase
    {

        #region Fields

        private decimal _Available;

        private ObservableCollection<Booking> _Bookings;

        private ICollectionView _BookingsCollectionView;

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

        private bool _NoProforma;

        private bool _NoReciept;

        private bool _NoVoucher;

        private int _PartnerIndex = -1;

        private ObservableCollection<Partner> _Partners;

        private ObservableCollection<Payment> _Payments;

        private bool _Remaining;

        private decimal _RemainingA;

        private Excursion _SelectedExcursionFilter;

        private Payment _SelectedPayment;

        private ReservationWrapper _SelectedReservation;

        private decimal _Total;

        private bool _ΒyCheckIn;

        #endregion Fields

        #region Constructors

        public Cards_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ShowReservationsCommand = new RelayCommand<string>(async (obj) => { await ShowReservations(obj); }, CanShowReservations);
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            SetNonPartnerCommand = new RelayCommand(() => { PartnerIndex = -1; }, PartnerIndex >= 0);

            Load();
        }

        #endregion Constructors

        #region Properties

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

        public RelayCommand EditBookingCommand { get; set; }

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

        public MainViewModel MainViewModel { get; }

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
                RaisePropertyChanged();
            }
        }

        public RelayCommand SetNonPartnerCommand { get; set; }

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

        private void CalculateAmounts()
        {
            Total = RemainingA = Available = Commision = 0;
            foreach (ReservationWrapper res in ReservationsCollectionView)
            {
                Total += res.FullPrice;
                RemainingA += res.Remaining;
                if (res.Booking != null && res.Booking.IsPartners)
                {
                    Commision += res.Booking.Commision;
                }
            }
        }

        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        private bool CanShowReservations(string arg)
        {
            return IsOk;
        }

        private bool CustomerExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return Completed || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today) || excursion.Id == 0;
        }

        private bool CustomerFilter(object item)
        {
            ReservationWrapper reservation = item as ReservationWrapper;
            var x = reservation.Contains(FilterString, false) &&
                (!Remaining || reservation.Remaining > 3) &&
                (!NoProforma || !reservation.ProformaSent) &&
                (!NoVoucher || !reservation.VoucherSent) &&
                (!NoReciept || !reservation.Reciept) &&
                (DepartmentIndexBookingFilter == 0 || reservation.UserWr.BaseLocation == DepartmentIndexBookingFilter) &&
                (ExcursionCategoryIndexBookingFilter == 0 || (ExcursionCategoryIndexBookingFilter == 1 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Bansko) || (ExcursionCategoryIndexBookingFilter == 2 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Skiathos) || (ExcursionCategoryIndexBookingFilter == 3 && reservation.PersonalModel != null) || (ExcursionCategoryIndexBookingFilter == 4 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Group) || (ExcursionCategoryIndexBookingFilter == 5 && reservation.ThirdPartyModel != null)) &&
                (ExcursionIndexBookingFilter == 0 || (reservation.Booking != null && ExcursionsCollectionView.CurrentItem != null && ExcursionsCollectionView.CurrentItem is Excursion && reservation.Booking.Excursion != null && reservation.Booking.Excursion.Id == ((Excursion)ExcursionsCollectionView.CurrentItem).Id));
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
                    await viewModel.LoadAsync(SelectedReservation.PersonalModel.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditPersonalBooking_Window(), viewModel));
                }
                else if (SelectedReservation.ExcursionType == ExcursionTypeEnum.ThirdParty)
                {
                    NewReservation_ThirdParty_VIewModel viewModel = new NewReservation_ThirdParty_VIewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.ThirdPartyModel.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new Edit_ThirdParty_Booking_Window(), viewModel));
                }
                else
                {
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.Booking.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }
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
                Context = new GenericRepository(true);
                DateTime From, To = DateTime.Today;
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
                    listr.AddRange((await Context.GetAllPersonalBookingsFiltered(0, true, new DateTime(), remainingPar: 1, checkin: From, checkout: To, partnerId: PartnerIndex >= 0 ? Partners[PartnerIndex].Id : 0, onlyPartners: true)).Select(r => new ReservationWrapper { Id = r.Id, PersonalModel = new Personal_BookingWrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                }
                //if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 5 || ExcursionCategoryIndexBookingFilter == 0))
                //{
                //    listr.AddRange((await Context.GetAllThirdPartyBookingsFiltered(0, Completed, new DateTime(), checkin: From, checkout: To, byCheckIn: ByCheckIn)).Select(r => new ReservationWrapper { Id = r.Id, ThirdPartyModel = new ThirdParty_Booking_Wrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                //}

                FilteredReservations = new ObservableCollection<ReservationWrapper>(listr);
                Parallel.ForEach(FilteredReservations, wr => { wr.CalculateAmounts(); _ = wr.Partner; });
                ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
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
                        pa.Parent = p;
                        payments.Add(pa);
                    }
                }
                else if (p.PersonalModel != null && p.PersonalModel.Payments != null)
                {
                    foreach (var pa in p.PersonalModel.Payments)
                    {
                        pa.Parent = p;
                        payments.Add(pa);
                    }
                }
                else if (p.ThirdPartyModel != null && p.ThirdPartyModel.Payments != null)
                {
                    foreach (var pa in p.ThirdPartyModel.Payments)
                    {
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