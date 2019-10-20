using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
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
using NuGet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class Remaining_ViewModel : MyViewModelBase
    {
        #region Constructors

        public Remaining_ViewModel(MainViewModel mainViewModel)
        {
            FilteredReservations = new ObservableCollection<ReservationWrapper>();
            ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
            Excursions = new ObservableCollection<Excursion>();
            ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);
            ExcursionsCollectionView.Filter = CustomerExcursionsFilter;

            ShowAllReservationsCommand = new RelayCommand<string>(async (obj) => { await ShowReservations(obj); }, CanShowReservations);
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            Messenger.Default.Register<ReservationChanged_Message>(this, ReservationChanged);

            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(3);
            MainViewModel = mainViewModel;
            Load();
        }

        #endregion Constructors

        #region Fields

        private DateTime _CheckIn;

        private DateTime _CheckOut;

        private bool _Completed = false;

        private int _DepartmentIndexBookingFilter;

        private int _ExcursionCategoryIndexBookingFilter;

        private int _ExcursionIndexBookingFilter;

        private ObservableCollection<Excursion> _Excursions;

        private ICollectionView _ExcursionsCollectionView;

        private ObservableCollection<ReservationWrapper> _FilteredReservations = new ObservableCollection<ReservationWrapper>();

        private string _FilterString = string.Empty;

        private bool _IsOk = true;


        private Excursion _SelectedExcursionFilter;

        private ReservationWrapper _SelectedReservation;

        private int _UserIndexBookingFilter;

        private ObservableCollection<User> _Users;

        private SearchBookingsHelper SearchBookingsHelper;

        #endregion Fields

        #region Properties

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

        public GenericRepository Context { get; set; }

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

                Excursions.Insert(0, new Excursion { ExcursionDates = new ObservableCollection<ExcursionDate> { new ExcursionDate { CheckIn = new DateTime() } }, Name = "Ολες" });

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

        public ICollectionView ReservationsCollectionView { get; set; }

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

        public RelayCommand<string> ShowAllReservationsCommand { get; set; }

        public int UserIndexBookingFilter
        {
            get
            {
                return _UserIndexBookingFilter;
            }

            set
            {
                if (_UserIndexBookingFilter == value)
                {
                    return;
                }

                _UserIndexBookingFilter = value;
                RaisePropertyChanged();
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                }
            }
        }

        public ObservableCollection<User> Users
        {
            get
            {
                return _Users;
            }

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

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Context = new GenericRepository();
            Users = MainViewModel.BasicDataManager.Users;
            Excursions = MainViewModel.BasicDataManager.Excursions;
            SearchBookingsHelper = new SearchBookingsHelper(Context);
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        private bool CanShowReservations(string arg)
        {
            return IsOk;
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
                RaisePropertyChanged();
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
            return reservation.Contains(FilterString) &&
                (UserIndexBookingFilter == 0 || reservation.UserWr.Id == Users[UserIndexBookingFilter - 1].Id) &&
                (DepartmentIndexBookingFilter == 0 || reservation.UserWr.BaseLocation == DepartmentIndexBookingFilter) &&
                (ExcursionCategoryIndexBookingFilter == 0 || (ExcursionCategoryIndexBookingFilter == 1 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Bansko) || (ExcursionCategoryIndexBookingFilter == 2 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Skiathos) || (ExcursionCategoryIndexBookingFilter == 3 && reservation.PersonalModel != null) || (ExcursionCategoryIndexBookingFilter == 4 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Group) || (ExcursionCategoryIndexBookingFilter == 5 && reservation.ThirdPartyModel != null)) &&
                (ExcursionIndexBookingFilter == 0 || (reservation.Booking != null && ExcursionsCollectionView.CurrentItem != null && ExcursionsCollectionView.CurrentItem is Excursion && reservation.Booking.Excursion != null && reservation.Booking.Excursion.Id == ((Excursion)ExcursionsCollectionView.CurrentItem).Id));
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
                ReservationsCollectionView.Refresh();
        }

        private void ReservationChanged(ReservationChanged_Message obj)
        {
            List<ReservationWrapper> toremove = FilteredReservations.Where(r => r.ExcursionType != ExcursionTypeEnum.Personal && r.Booking != null && r.Booking.Id == obj.Booking.Id).ToList();

            foreach (var r in toremove)
            {
                FilteredReservations.Remove(r);
            }
            if (toremove.Count > 0)
            {
                FilteredReservations.AddRange(obj.Booking.ReservationsInBooking.Select(w => new ReservationWrapper(w)));
                ReservationsCollectionView.Refresh();
            }
        }

        private async Task ShowReservations(string parameter)
        {
            try
            {
                IsOk = false;
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                //await ReloadAsync();
                Context = new GenericRepository();

                SearchBookingsHelper = new SearchBookingsHelper(Context);
                DateTime dateLimit = new DateTime();
                List<ReservationWrapper> list = new List<ReservationWrapper>();
                if (parameter == "0")
                {
                    list = (await Context.GetAllRemainingReservationsFiltered(ExcursionIndexBookingFilter > 0 ? (ExcursionsCollectionView.CurrentItem as Excursion).Id : 0, UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, ExcursionCategoryIndexBookingFilter > 0 ? ExcursionCategoryIndexBookingFilter - 1 : -1)).Select(r => new ReservationWrapper(r)).ToList();

                    if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 3 || ExcursionCategoryIndexBookingFilter == 0))
                    {
                        list.AddRange((await Context.GetAllPersonalBookingsFiltered(UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, true, dateLimit)).Select(r => new ReservationWrapper { Id = r.Id, PersonalModel = new Personal_BookingWrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                    }
                    if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 5 || ExcursionCategoryIndexBookingFilter == 0))
                    {
                        list.AddRange((await Context.GetAllThirdPartyBookingsFiltered(UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, true, dateLimit)).Select(r => new ReservationWrapper { Id = r.Id, ThirdPartyModel = new ThirdParty_Booking_Wrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                    }
                }
                else if (parameter == "1")
                {
                    list = (await Context.GetAllRemainingReservationsFiltered(ExcursionIndexBookingFilter > 0 ? (ExcursionsCollectionView.CurrentItem as Excursion).Id : 0, UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, ExcursionCategoryIndexBookingFilter > 0 ? ExcursionCategoryIndexBookingFilter - 1 : -1, 1, CheckIn, CheckOut)).Select(r => new ReservationWrapper(r)).ToList();

                    if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 3 || ExcursionCategoryIndexBookingFilter == 0))
                    {
                        list.AddRange((await Context.GetAllPersonalBookingsFiltered(UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, true, dateLimit, 1, CheckIn, CheckOut)).Select(r => new ReservationWrapper { Id = r.Id, PersonalModel = new Personal_BookingWrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                    }
                    if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 5 || ExcursionCategoryIndexBookingFilter == 0))
                    {
                        list.AddRange((await Context.GetAllThirdPartyBookingsFiltered(UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, true, dateLimit, 1, CheckIn, CheckOut)).Select(r => new ReservationWrapper { Id = r.Id, ThirdPartyModel = new ThirdParty_Booking_Wrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                    }
                }

                Parallel.ForEach(list, wr => wr.CalculateAmounts());
                FilteredReservations = new ObservableCollection<ReservationWrapper>(list.Where(r => r.Remaining > 3));
                //User u = Context.GetById<User>(StaticResources.User.Id);
                //foreach (var r in FilteredReservations)
                //{
                //    if (r.Booking != null && r.Booking.IsPartners && (r.Booking.Partner.Id == 199 || r.Booking.Partner.Id == 219) && r.Remaining > 0)
                //    {
                //        r.Booking.Payments.Add(new Payment { Amount = r.Remaining, Checked = true, Comment = "Auto", PaymentMethod = 0, User = u });
                //    }
                //}
                //await Context.SaveAsync();
                ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
                ReservationsCollectionView.Filter = CustomerFilter;
                ReservationsCollectionView.SortDescriptions.Add(new SortDescription("CreatedDate", ListSortDirection.Ascending));
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

        #endregion Methods
    }
}