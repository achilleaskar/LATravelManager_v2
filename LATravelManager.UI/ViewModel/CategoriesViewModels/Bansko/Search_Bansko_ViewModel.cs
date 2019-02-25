using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Models;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Views.Bansko;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko
{
    public class Search_Bansko_ViewModel : BanskoChilds_BaseViewModel
    {
        #region Constructors

        public Search_Bansko_ViewModel()
        {
            FilteredReservations = new ObservableCollection<Reservation>();
            _customerView = CollectionViewSource.GetDefaultView(FilteredReservations);

            ShowReservationsCommand = new RelayCommand<string>(async (obj) => { await ShowReservations(obj); }, CanShowReservations);
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            DeleteReservationCommand = new RelayCommand(DeleteReservation, CanDeleteReservation);
            DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);

            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(3);
        }

        #endregion Constructors

        #region Fields

        private DateTime _CheckIn;
        private DateTime _CheckOut;

        private bool _EnableCheckInFilter = false;
        private bool _EnableCheckOutFilter = false;

        private ObservableCollection<Reservation> _FilteredReservations = new ObservableCollection<Reservation>();
        private string _FilterString = string.Empty;
        private Booking _SelectedBooking;
        private Reservation _SelectedReservation;
        private int _UserBookingFilterIndex;
        private ObservableCollection<User> _Users;
        private SearchBookingsHelper SearchBookingsHelper;

        #endregion Fields

        #region Properties

        public ICollectionView _customerView { get; set; }

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
                if (_customerView != null)
                    _customerView.Refresh();
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
                if (_customerView != null)
                    _customerView.Refresh();
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
                if (_customerView != null)
                    _customerView.Refresh();
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
                if (_customerView != null)
                    _customerView.Refresh();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Reservation> FilteredReservations
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
                if (_customerView != null)
                    _customerView.Refresh();
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

        public Reservation SelectedReservation
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

        public override async Task LoadAsync(int id = 0)
        {
            await ReloadAsync();
        }

        public override async Task ReloadAsync()
        {
            Context = new GenericRepository();
            Users = new ObservableCollection<User>(await Context.GetAllAsyncSortedByName<User>());
            SearchBookingsHelper = new SearchBookingsHelper(Context);
            FilteredReservations.Clear();
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
            return true;
        }

        private bool CustomerFilter(object item)
        {
            Reservation reservation = item as Reservation;
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
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                var viewModel = new NewReservation_Bansko_ViewModel();
                await viewModel.LoadAsync(SelectedReservation.Booking.Id);
                MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
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

        private async Task ShowReservations(string parameter)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                await ReloadAsync();
                DateTime dateLimit = SearchBookingsHelper.GetDateLimit(parameter);
                List<Reservation> list = (await Context.GetAllReservationsByCreationDate(dateLimit, 2)).ToList();
                foreach (var r in list)
                {
                    FilteredReservations.Add(r);
                }

                _customerView = CollectionViewSource.GetDefaultView(FilteredReservations);
                _customerView.Filter = CustomerFilter;
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

        #endregion Methods
    }
}