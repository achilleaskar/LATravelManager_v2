using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Models;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    internal class GlobalSearch_ViewModel : MyViewModelBase
    {
        #region Constructors

        public GlobalSearch_ViewModel()
        {
            FilteredReservations = new ObservableCollection<ReservationWrapper>();
            ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
            Excursions = new ObservableCollection<Excursion>();
            ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);

            PrintRoomingListsCommand = new RelayCommand(async () => { await PrintRoomingLists(); },CanPrintRoomingLists);
            PrintAllVouchersCommand = new RelayCommand(async () => { await PrintAllVouchers(); });
            PrintAllLettersCommand = new RelayCommand(PrintAllLetters);
            PrintListCommand = new RelayCommand(async () => { await PrintList(); });

            ShowReservationsCommand = new RelayCommand<string>(async (obj) => { await ShowReservations(obj); }, CanShowReservations);
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            DeleteReservationCommand = new RelayCommand(DeleteReservation, CanDeleteReservation);
            DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);

            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(3);

            MessengerInstance.Register<SelectedExcursionChangedMessage>(this, async exc => { await SelectedExcursionChanged(exc.SelectedExcursion); });
        }

        private bool CanPrintRoomingLists()
        {
            return ExcursionIndexBookingFilter > 0;
        }

        private Excursion _SelectedExcursionFilter;

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

        #endregion Constructors

        #region Fields

        private DateTime _CheckIn;
        private DateTime _CheckOut;
        private bool _Completed = false;
        private bool _EnableCheckInFilter = false;
        private bool _EnableCheckOutFilter = false;
        private int _ExcursionCategoryIndexBookingFilter;

        private int _ExcursionIndexBookingFilter;

        private ObservableCollection<Excursion> _Excursions;

        private ObservableCollection<ReservationWrapper> _FilteredReservations = new ObservableCollection<ReservationWrapper>();

        private string _FilterString = string.Empty;

        private Booking _SelectedBooking;

        private ExcursionWrapper _SelectedExcursion;

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
                if (ReservationsCollectionView != null)
                    ReservationsCollectionView.Refresh();
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
                if (ReservationsCollectionView != null)
                    ReservationsCollectionView.Refresh();
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
                RaisePropertyChanged();
            }
        }

        public GenericRepository Context { get; set; }

        public RelayCommand DeleteBookingCommand { get; set; }

        public RelayCommand DeleteReservationCommand { get; set; }

        public RelayCommand EditBookingCommand { get; set; }

        public bool EnableCheckInFilter
        {
            get
            {
                return _EnableCheckInFilter;
            }

            set
            {
                if (_EnableCheckInFilter == value)
                {
                    return;
                }

                _EnableCheckInFilter = value;
                if (ReservationsCollectionView != null)
                    ReservationsCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        public bool EnableCheckOutFilter
        {
            get
            {
                return _EnableCheckOutFilter;
            }

            set
            {
                if (_EnableCheckOutFilter == value)
                {
                    return;
                }

                _EnableCheckOutFilter = value;
                if (ReservationsCollectionView != null)
                    ReservationsCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

        public ICollectionView ExcursionsCollectionView { get; set; }

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
                    ReservationsCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        public RelayCommand PrintAllLettersCommand { get; set; }
        public RelayCommand PrintAllVouchersCommand { get; set; }
        public RelayCommand PrintListCommand { get; set; }
        public RelayCommand PrintRoomingListsCommand { get; set; }
        public ICollectionView ReservationsCollectionView { get; set; }

        public Booking SelectedBooking
        {
            get
            {
                return _SelectedBooking;
            }

            set
            {
                if (_SelectedBooking == value)
                {
                    return;
                }

                _SelectedBooking = value;
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

        public RelayCommand<string> ShowReservationsCommand { get; set; }

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

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            await ReloadAsync();
        }

        public override async Task ReloadAsync()
        {
            if (Context == null)
            {
            }
            if (Context != null && !Context.IsTaskOk)
            {
                await Context.LastTask;
            }
            Context = new GenericRepository();
            int userId = UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0;
            int excursionId = ExcursionIndexBookingFilter > 0 ? Excursions[ExcursionIndexBookingFilter-1].Id : 0;
            Users = new ObservableCollection<User>(await Context.GetAllAsyncSortedByName<User>());
            Excursions = new ObservableCollection<Excursion>((await Context.GetAllAsync<Excursion>()).OrderBy(e => e.ExcursionDates.OrderBy(ed => ed.CheckIn).FirstOrDefault().CheckIn));
            SearchBookingsHelper = new SearchBookingsHelper(Context);
            FilteredReservations.Clear();
            ShowReservationsCommand.RaiseCanExecuteChanged();
            ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);
            ExcursionsCollectionView.Refresh();

            UserIndexBookingFilter = userId > 0 ? Users.IndexOf(Users.Where(u => u.Id == userId).FirstOrDefault()) + 1 : 0;
            ExcursionIndexBookingFilter = excursionId > 0 ? Excursions.IndexOf(Excursions.Where(e => e.Id == excursionId).FirstOrDefault()) + 1 : 0;
        }

        public async Task SelectedExcursionChanged(ExcursionWrapper selectedExcursion)
        {
            if (selectedExcursion != null)
            {
                SelectedExcursion = selectedExcursion;
                await LoadAsync();
            }
        }

        private bool CanDeleteBooking()
        {
            return SelectedBooking != null;
        }

        private bool CanDeleteReservation()
        {
            return SelectedReservation != null;
        }

        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        private bool _IsOk = true;

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

        private bool CanShowReservations(string arg)
        {
            return IsOk;
        }

        private bool CustomerFilter(object item)
        {
            ReservationWrapper reservation = item as ReservationWrapper;
            return reservation.Contains(FilterString) && (!EnableCheckInFilter || reservation.CheckIn == CheckIn) && (!EnableCheckOutFilter || reservation.CheckOut == CheckOut) && (UserIndexBookingFilter == 0 || reservation.Booking.User.Id == Users[UserIndexBookingFilter - 1].Id);
        }

        private async void DeleteBooking()
        {
            await SearchBookingsHelper.DeleteBooking(SelectedBooking);
        }

        private async void DeleteReservation()
        {
            if (SelectedReservation != null)
            {
                await SearchBookingsHelper.DeleteReservation(SelectedReservation);
                FilteredReservations.Remove(SelectedReservation);
            }
        }

        private async Task EditBooking()
        {
            try
            {
                var viewModel = new NewReservation_Group_ViewModel();
                await viewModel.LoadAsync(SelectedReservation.Booking.Id);
                MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private void PrintAllLetters()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                List<Booking> bookings = new List<Booking>();

                foreach (object r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (!bookings.Contains((r as ReservationWrapper).Booking))
                    {
                        bookings.Add((r as ReservationWrapper).Booking);
                    }
                }
                int coutner = 0;

                foreach (var b in bookings)
                {
                    coutner += b.ReservationsInBooking.Count;
                }

                using (DocumentsManagement vm = new DocumentsManagement(new GenericRepository()))
                {
                    foreach (var b in bookings)
                    {
                        vm.PrintSingleBookingLetter(new BookingWrapper(b));
                    }
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

        private async Task PrintAllVouchers()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                List<Booking> bookings = new List<Booking>();

                foreach (object r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (!bookings.Contains((r as ReservationWrapper).Booking))
                    {
                        bookings.Add((r as ReservationWrapper).Booking);
                    }
                }
                int coutner = 0;

                foreach (var b in bookings)
                {
                    coutner += b.ReservationsInBooking.Count;
                }

                using (DocumentsManagement vm = new DocumentsManagement(new GenericRepository()))
                {
                    foreach (var b in bookings)
                    {
                        await vm.PrintSingleBookingVoucher(new BookingWrapper(b));
                    }
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

        private async Task PrintList()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                List<Booking> bookings = new List<Booking>();

                foreach (object r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (!bookings.Contains((r as ReservationWrapper).Booking))
                    {
                        bookings.Add((r as ReservationWrapper).Booking);
                    }
                }
                int coutner = 0;

                foreach (var b in bookings)
                {
                    coutner += b.ReservationsInBooking.Count;
                }

                using (DocumentsManagement vm = new DocumentsManagement(new GenericRepository()))
                {
                    await vm.PrintList(bookings, CheckIn);
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

        private async Task PrintRoomingLists()
        {
            Lists listmanager = new Lists(Excursions[ExcursionIndexBookingFilter-1], CheckIn, CheckOut);
            await listmanager.LoadAsync();
            await listmanager.PrintAllRoomingLists();
        }

        private async Task ShowReservations(string parameter)
        {
            try
            {
                IsOk = false;
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                await ReloadAsync();
                DateTime dateLimit = SearchBookingsHelper.GetDateLimit(parameter);

              
                List<ReservationWrapper> list = (await Context.GetAllReservationsFiltered(ExcursionIndexBookingFilter > 0 ? Excursions[ExcursionIndexBookingFilter - 1].Id : 0, UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, Completed, ExcursionCategoryIndexBookingFilter > 0 ? ExcursionCategoryIndexBookingFilter : -1,dateLimit)).Select(r => new ReservationWrapper(r)).ToList();

                //foreach (var r in list)
                //{
                //    //  r.CalculateAmounts();
                //    FilteredReservations.Add(r);
                //}
                FilteredReservations = new ObservableCollection<ReservationWrapper>(list);
                Parallel.ForEach(FilteredReservations, wr => wr.CalculateAmounts());
                ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
               ReservationsCollectionView.Filter = CustomerFilter;
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