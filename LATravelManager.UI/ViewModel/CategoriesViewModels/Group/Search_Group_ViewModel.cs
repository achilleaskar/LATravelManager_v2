using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Group
{
    public class Search_Group_ViewModel : GroupChilds_BaseViewModel
    {
        #region Constructors

        public RelayCommand PrintRoomingListsCommand { get; set; }
        public RelayCommand PrintAllVouchersCommand { get; set; }
        public RelayCommand PrintAllLettersCommand { get; set; }
        public RelayCommand PrintListCommand { get; set; }

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

        public Search_Group_ViewModel()
        {
            FilteredReservations = new ObservableCollection<ReservationWrapper>();
            CustomerView = CollectionViewSource.GetDefaultView(FilteredReservations);

            PrintRoomingListsCommand = new RelayCommand(async () => { await PrintRoomingLists(); });
            PrintAllVouchersCommand = new RelayCommand(async () => { await PrintAllVouchers(); });
            PrintAllLettersCommand = new RelayCommand(PrintAllLetters);
            PrintListCommand = new RelayCommand(async () => { await PrintList(); });

            ShowReservationsCommand = new RelayCommand<string>(async (obj) => { await ShowReservations(obj); }, CanShowReservations);
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            DeleteReservationCommand = new RelayCommand(DeleteReservation, CanDeleteReservation);
            DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);

            AllExcursions = true;

            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(3);

            MessengerInstance.Register<SelectedExcursionChangedMessage>(this, async exc => { await SelectedExcursionChanged(exc.SelectedExcursion); });
        }

        private void PrintAllLetters()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                List<Booking> bookings = new List<Booking>();

                foreach (object r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (!bookings.Contains((r as Reservation).Booking))
                    {
                        bookings.Add((r as Reservation).Booking);
                    }
                }
                int coutner = 0;

                foreach (Booking b in bookings)
                {
                    coutner += b.ReservationsInBooking.Count;
                }

                using (DocumentsManagement vm = new DocumentsManagement(new GenericRepository()))
                {
                    foreach (Booking b in bookings)
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
                    if (!bookings.Contains((r as Reservation).Booking))
                    {
                        bookings.Add((r as Reservation).Booking);
                    }
                }
                int coutner = 0;

                foreach (Booking b in bookings)
                {
                    coutner += b.ReservationsInBooking.Count;
                }

                using (DocumentsManagement vm = new DocumentsManagement(new GenericRepository()))
                {
                    foreach (Booking b in bookings)
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

        private async Task PrintRoomingLists()
        {
            Lists listmanager = new Lists(SelectedExcursion.Model, CheckIn, CheckOut);
            await listmanager.LoadAsync();
            await listmanager.PrintAllRoomingLists();
        }

        private async Task PrintList()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                List<Booking> bookings = new List<Booking>();

                foreach (object r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (!bookings.Contains((r as Reservation).Booking))
                    {
                        bookings.Add((r as Reservation).Booking);
                    }
                }
                int coutner = 0;

                foreach (Booking b in bookings)
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

        #endregion Constructors

        #region Fields

        private DateTime _CheckIn;
        private DateTime _CheckOut;

        private bool _EnableCheckInFilter = false;
        private bool _EnableCheckOutFilter = false;

        private ObservableCollection<ReservationWrapper> _FilteredReservations = new ObservableCollection<ReservationWrapper>();
        private string _FilterString = string.Empty;
        private Booking _SelectedBooking;
        private ReservationWrapper _SelectedReservation;
        private int _UserBookingFilterIndex;
        private ObservableCollection<User> _Users;
        private SearchBookingsHelper SearchBookingsHelper;

        #endregion Fields

        private bool _AllExcursions = false;
        private ExcursionWrapper _SelectedExcursion;

        public bool AllExcursions
        {
            get
            {
                return _AllExcursions;
            }

            set
            {
                if (_AllExcursions == value)
                {
                    return;
                }

                _AllExcursions = value;
                RaisePropertyChanged();
            }
        }

        #region Properties

        public ICollectionView CustomerView { get; set; }

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
                if (CustomerView != null)
                    CustomerView.Refresh();
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
                if (CustomerView != null)
                    CustomerView.Refresh();
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
                if (CustomerView != null)
                    CustomerView.Refresh();
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
                if (CustomerView != null)
                    CustomerView.Refresh();
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
                if (CustomerView != null)
                    CustomerView.Refresh();
                RaisePropertyChanged();
            }
        }

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

        public int UserBookingFilterIndex
        {
            get
            {
                return _UserBookingFilterIndex;
            }

            set
            {
                if (_UserBookingFilterIndex == value)
                {
                    return;
                }

                _UserBookingFilterIndex = value;
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

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
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
            Users = new ObservableCollection<User>(await Context.GetAllAsyncSortedByName<User>());
            SearchBookingsHelper = new SearchBookingsHelper(Context);
            FilteredReservations.Clear();
            ShowReservationsCommand.RaiseCanExecuteChanged();
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

        private bool CanShowReservations(string arg)
        {
            return Context.IsTaskOk;
        }

        private bool CustomerFilter(object item)
        {
            ReservationWrapper reservation = item as ReservationWrapper;
            return reservation.Contains(FilterString) && (!EnableCheckInFilter || reservation.CheckIn == CheckIn) && (!EnableCheckOutFilter || reservation.CheckOut == CheckOut) && (UserBookingFilterIndex == 0 || reservation.Booking.User.Id == Users[UserBookingFilterIndex - 1].Id);
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
                NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel();
                await viewModel.LoadAsync(SelectedReservation.Booking.Id);
                MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private async Task ShowReservations(string parameter)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                await ReloadAsync();
                DateTime dateLimit = SearchBookingsHelper.GetDateLimit(parameter);

                List<ReservationWrapper> list = AllExcursions ?
                    (await Context.GetAllGroupReservationsByCreationDate(dateLimit)).Select(r => new ReservationWrapper(r)).ToList() :
                    (await Context.GetAllReservationsByCreationDate(dateLimit, SelectedExcursion.Id)).Select(r => new ReservationWrapper(r)).ToList();
                foreach (ReservationWrapper r in list)
                {
                    FilteredReservations.Add(r);
                }

                CustomerView = CollectionViewSource.GetDefaultView(FilteredReservations);
                CustomerView.Filter = CustomerFilter;
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

        public async Task SelectedExcursionChanged(ExcursionWrapper selectedExcursion)
        {
            if (selectedExcursion != null)
            {
                SelectedExcursion = selectedExcursion;
                await LoadAsync();
            }
        }

        #endregion Methods
    }
}