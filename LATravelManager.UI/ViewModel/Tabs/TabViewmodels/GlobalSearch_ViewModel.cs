using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Views.Personal;
using LATravelManager.UI.Wrapper;
using NuGet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    internal class GlobalSearch_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public GlobalSearch_ViewModel(MainViewModel mainViewModel)
        {
            FilteredReservations = new ObservableCollection<ReservationWrapper>();
            ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
            Excursions = new ObservableCollection<Excursion>();
            ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);
            ExcursionsCollectionView.Filter = CustomerExcursionsFilter;

            PrintRoomingListsCommand = new RelayCommand(async () => { await PrintRoomingLists(); }, CanPrintRoomingLists);
            PrintAllVouchersCommand = new RelayCommand(async () => { await PrintAllVouchers(); });
            PrintAllLettersCommand = new RelayCommand(PrintAllLetters);
            PrintListCommand = new RelayCommand(async () => { await PrintList(); });

            ShowReservationsCommand = new RelayCommand<string>(async (obj) => { await ShowReservations(obj); }, CanShowReservations);
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            DeleteReservationCommand = new RelayCommand(DeleteReservation, CanDeleteReservation);
            DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);
            Messenger.Default.Register<ReservationChanged_Message>(this, ReservationChanged);

            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(3);
            MainViewModel = mainViewModel;
        }

        private int _BookingIdFilter;

        public int BookingIdFilter
        {
            get
            {
                return _BookingIdFilter;
            }

            set
            {
                if (_BookingIdFilter == value)
                {
                    return;
                }

                _BookingIdFilter = value;
                RaisePropertyChanged();
            }
        }

        #endregion Constructors

        #region Fields

        private bool _Bank;

        private DateTime _CheckIn;

        private DateTime _CheckOut;

        private bool _Completed = false;

        private bool _EnableCheckInFilter = false;

        private bool _EnableCheckOutFilter = false;

        private int _ExcursionCategoryIndexBookingFilter;

        private int _ExcursionIndexBookingFilter;

        private ObservableCollection<Excursion> _Excursions;

        private ICollectionView _ExcursionsCollectionView;

        private ObservableCollection<ReservationWrapper> _FilteredReservations = new ObservableCollection<ReservationWrapper>();

        private string _FilterString = string.Empty;

        private bool _IsOk = true;

        private Booking _SelectedBooking;

        private Excursion _SelectedExcursionFilter;

        private ReservationWrapper _SelectedReservation;

        private int _UserIndexBookingFilter;

        private ObservableCollection<User> _Users;

        private SearchBookingsHelper SearchBookingsHelper;

        #endregion Fields

        #region Properties

        public bool Bank
        {
            get
            {
                return _Bank;
            }

            set
            {
                if (_Bank == value)
                {
                    return;
                }

                _Bank = value;
                ReservationsCollectionView.Refresh();
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
                ExcursionsCollectionView.Refresh();
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

        private int _DepartmentIndexBookingFilter;

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
                ReservationsCollectionView.Refresh();
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
                    ReservationsCollectionView.Refresh();
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

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                Context = new GenericRepository();
                Users = MainViewModel.BasicDataManager.Users;
                Excursions = MainViewModel.BasicDataManager.Excursions;
                SearchBookingsHelper = new SearchBookingsHelper(Context);
                await Task.Delay(0);
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
            if (Context != null && !Context.IsTaskOk)
            {
                await Context.LastTask;
            }
            Context = new GenericRepository();
            int userId = UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0;
            int excursionId = ExcursionIndexBookingFilter > 0 ? (ExcursionsCollectionView.CurrentItem as Excursion).Id : 0;
            Users = new ObservableCollection<User>(await Context.GetAllAsyncSortedByName<User>());
            Excursions = new ObservableCollection<Excursion>((await Context.GetAllExcursionsAsync()));

            SearchBookingsHelper = new SearchBookingsHelper(Context);

            UserIndexBookingFilter = userId > 0 ? Users.IndexOf(Users.Where(u => u.Id == userId).FirstOrDefault()) + 1 : 0;
            if (excursionId > 0)
            {
                Excursion tmpExc = Excursions.First(e => e.Id == excursionId);
                if (ExcursionsCollectionView.Contains(tmpExc))
                {
                    ExcursionsCollectionView.MoveCurrentTo(tmpExc);
                }
            }
            else
            {
                ExcursionCategoryIndexBookingFilter = 0;
            }
        }

        private bool CanDeleteBooking()
        {
            return SelectedReservation != null && SelectedReservation.Booking != null;
        }

        private bool CanDeleteReservation()
        {
            return SelectedReservation != null;
        }

        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        private bool CanPrintRoomingLists()
        {
            return ExcursionIndexBookingFilter > 0;
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
            if (reservation.Id==3987)
            {

            }
            return reservation.Contains(FilterString) &&
                (BookingIdFilter == 0 || reservation.Booking.Id == BookingIdFilter) &&
                (!EnableCheckInFilter || reservation.CustomersList.Any(c=>c.CheckIn == CheckIn)) &&
                (!EnableCheckOutFilter || reservation.CustomersList.Any(c => c.CheckOut == CheckOut)) &&
                (UserIndexBookingFilter == 0 || reservation.UserWr.Id == Users[UserIndexBookingFilter - 1].Id) &&
                (DepartmentIndexBookingFilter == 0 || reservation.UserWr.BaseLocation == DepartmentIndexBookingFilter) &&
                (!Bank || (reservation.ExcursionType != ExcursionTypeEnum.Personal && reservation.ExcursionType != ExcursionTypeEnum.ThirdParty && reservation.Booking.Payments.Any(p => p.PaymentMethod > 0)) || ((reservation.ExcursionType == ExcursionTypeEnum.Personal) && reservation.PersonalModel.Payments.Any(p => p.PaymentMethod > 0)));
        }

        private async void DeleteBooking()
        {
            await SearchBookingsHelper.DeleteBooking(SelectedReservation.Booking);
        }

        private async void DeleteReservation()
        {
            if (SelectedReservation != null)
            {
                var tmpRes = SelectedReservation;
                FilteredReservations.Remove(tmpRes);
                await SearchBookingsHelper.DeleteReservation(tmpRes);
            }
        }

        private async Task EditBooking()
        {
            try
            {
                if (SelectedReservation.ExcursionType == ExcursionTypeEnum.Personal)
                {
                    NewReservation_Personal_ViewModel viewModel = new NewReservation_Personal_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.PersonalModel.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditPersonalBooking_Window(), viewModel));
                }
                else if (SelectedReservation.ExcursionType == ExcursionTypeEnum.ThirdParty)
                {
                }
                else
                {
                    bool reciept = SelectedReservation.Booking.Reciept;
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.Booking.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }
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

                foreach (ReservationWrapper r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (r.ExcursionType != ExcursionTypeEnum.Personal && !bookings.Contains(r.Booking))
                    {
                        bookings.Add((r as ReservationWrapper).Booking);
                    }
                }
                int coutner = 0;

                foreach (Booking b in bookings)
                {
                    if (b.Excursion.ExcursionType.Category != ExcursionTypeEnum.Personal)
                    {
                        coutner += b.ReservationsInBooking.Count;
                    }
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

                foreach (ReservationWrapper r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (r.ExcursionType != ExcursionTypeEnum.Personal && !bookings.Contains(r.Booking))
                    {
                        bookings.Add((r as ReservationWrapper).Booking);
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

        private async Task PrintList()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                List<Booking> bookings = new List<Booking>();

                foreach (ReservationWrapper r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (r.ExcursionType != ExcursionTypeEnum.Personal && !bookings.Contains(r.Booking))
                    {
                        bookings.Add((r as ReservationWrapper).Booking);
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

        private async Task PrintRoomingLists()
        {
            var date = new DateTime(2019, 5, 28);
            int counter = 0;
            var dimitris = await Context.GetAllReservationsDimitri(r => r.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Skiathos && r.CustomersList.Any(c => c.CheckIn >= date));

            foreach (var r in dimitris)
            {
                counter += r.CustomersList.Count;
            }

            Mouse.OverrideCursor = Cursors.Wait;
            if (!EnableCheckInFilter || !EnableCheckOutFilter)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε ημερομηνίες"));
            }
            else
            {
                Lists listmanager = new Lists((ExcursionsCollectionView.CurrentItem as Excursion), CheckIn, CheckOut);
                await listmanager.LoadAsync();
                await listmanager.PrintAllRoomingLists();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
            //List<Payment> payments = new List<Payment>();
            //foreach (ReservationWrapper item in ReservationsCollectionView)
            //{
            //    if (item.Booking != null)
            //    {

            //        foreach (var p in item.Booking.Payments)
            //        {
            //            if (!payments.Contains(p))
            //            {
            //                payments.Add(p);
            //            }
            //        }
            //    }
            //}

            //List<decimal> paymentsums = new List<decimal>
            //{
            //    0,
            //    0,
            //    0,
            //    0,
            //    0,
            //    0,
            //    0
            //};

            //foreach (var p in payments)
            //{
            //    paymentsums[p.PaymentMethod] += p.Amount;
            //}
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
                DateTime dateLimit = SearchBookingsHelper.GetDateLimit(parameter);

                List<ReservationWrapper> list = (await Context.GetAllReservationsFiltered(ExcursionIndexBookingFilter > 0 ? (ExcursionsCollectionView.CurrentItem as Excursion).Id : 0, UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, Completed, ExcursionCategoryIndexBookingFilter > 0 ? ExcursionCategoryIndexBookingFilter - 1 : -1, dateLimit)).Select(r => new ReservationWrapper(r)).ToList();




                foreach (var item in list)
                {
                    if (item.ReservationType == ReservationTypeEnum.Overbooked)
                    {
                    }
                }
                foreach (var res in list)
                {
                    foreach (var c in res.CustomersList)
                    {
                        foreach (var r in list)
                        {
                            foreach (var c2 in r.CustomersList)
                            {
                                if (c.Id != c2.Id && !string.IsNullOrEmpty(c.Tel) && c.Tel.Length > 9 && c.Tel.StartsWith("6") && c.Tel == c2.Tel)
                                    MessageBox.Show($"{c.Name} {c.Surename} - {c2.Name} {c2.Surename}");
                            }
                        }
                    }
                }

                if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 3 || ExcursionCategoryIndexBookingFilter == 0))
                {
                    list.AddRange((await Context.GetAllPersonalBookingsFiltered(UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, Completed, dateLimit)).Select(r => new ReservationWrapper { Id = r.Id, PersonalModel = new Personal_BookingWrapper(r), CustomersList = r.Customers.ToList() }).ToList());
                }

                list = list.OrderBy(r => r.CreatedDate).ToList();

                //foreach (var r in list)
                //{
                //    //  r.CalculateAmounts();
                //    FilteredReservations.Add(r);
                //}
                FilteredReservations = new ObservableCollection<ReservationWrapper>(list);
                Parallel.ForEach(FilteredReservations, wr => wr.CalculateAmounts());
                ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
                ReservationsCollectionView.Filter = CustomerFilter;
                ReservationsCollectionView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
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