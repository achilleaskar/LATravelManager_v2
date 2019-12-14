﻿using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Data.Workers;
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
    internal class GlobalSearch_ViewModel : MyViewModelBase
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
            PrintBusListsCommand = new RelayCommand(async () => { await PrintBusLists(); });
            PrintTheseisCommand = new RelayCommand(async () => { await PrintTheseis(); });

            ShowReservationsCommand = new RelayCommand<string>(async (obj) => { await ShowReservations(obj, false); }, CanShowReservations);
            ShowCanceled = new RelayCommand<string>(async (obj) => { await ShowReservations(obj, true); }, CanShowReservations);
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            DeleteReservationCommand = new RelayCommand(DeleteReservation, CanDeleteReservation);
            DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);
            Messenger.Default.Register<ReservationChanged_Message>(this, ReservationChanged);

            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(3);
            MainViewModel = mainViewModel;
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<DailyDepartureInfo> _AllDaysDeparturesList;
        private bool _Bank;
        private int _BookingIdFilter;
        private DateTime _CheckIn;
        private bool _CheckInOut;
        private DateTime _CheckOut;
        private bool _Completed = false;
        private DailyDepartureInfo _DailyDepartureInfo;
        private int _DepartmentIndexBookingFilter;
        private bool _EnableCheckInFilter = false;
        private bool _EnableCheckOutFilter = false;
        private int _ExcursionCategoryIndexBookingFilter;
        private int _ExcursionIndexBookingFilter;
        private ObservableCollection<Excursion> _Excursions;
        private ICollectionView _ExcursionsCollectionView;
        private ObservableCollection<ReservationWrapper> _FilteredReservations = new ObservableCollection<ReservationWrapper>();
        private string _FilterString = string.Empty;
        private bool _FromTo = true;
        private int _GroupIndexBookingFilter;

        private bool _IsOk = true;

        private ObservableCollection<string> _People;

        private bool _Remaining;

        private Booking _SelectedBooking;

        private Excursion _SelectedExcursionFilter;

        private ReservationWrapper _SelectedReservation;

        private int _UserIndexBookingFilter;

        private ObservableCollection<User> _Users;

        private SearchBookingsHelper SearchBookingsHelper;

        #endregion Fields

        #region Properties

        public ObservableCollection<DailyDepartureInfo> AllDaysDeparturesList
        {
            get
            {
                return _AllDaysDeparturesList;
            }

            set
            {
                if (_AllDaysDeparturesList == value)
                {
                    return;
                }

                _AllDaysDeparturesList = value;
                RaisePropertyChanged();
            }
        }

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
                CountCustomers();
                RaisePropertyChanged();
            }
        }

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
                if (CheckOut < CheckIn)
                {
                    CheckOut = CheckIn.AddDays(3);
                }
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    CountCustomers();
                }
                RaisePropertyChanged();
            }
        }

        public bool CheckInOut
        {
            get
            {
                return _CheckInOut;
            }

            set
            {
                if (_CheckInOut == value)
                {
                    return;
                }

                _CheckInOut = value;
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
                if (CheckOut < CheckIn)
                {
                    CheckIn = CheckOut;
                }
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    CountCustomers();
                }
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

        public DailyDepartureInfo DailyDepartureInfo
        {
            get
            {
                return _DailyDepartureInfo;
            }

            set
            {
                if (_DailyDepartureInfo == value)
                {
                    return;
                }

                _DailyDepartureInfo = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand DeleteBookingCommand { get; set; }

        public RelayCommand DeleteReservationCommand { get; set; }

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
                    CountCustomers();
                }
            }
        }

        public bool DepartureInfoVisible => DailyDepartureInfo != null;

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
                {
                    ReservationsCollectionView.Refresh();
                    CountCustomers();
                }
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
                {
                    ReservationsCollectionView.Refresh();
                    CountCustomers();
                }
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
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    CountCustomers();
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
                    CountCustomers();
                }
                RaisePropertyChanged();
            }
        }

        public bool FromTo
        {
            get
            {
                return _FromTo;
            }

            set
            {
                if (_FromTo == value)
                {
                    return;
                }

                _FromTo = value;
                RaisePropertyChanged();
            }
        }

        public int GroupIndexBookingFilter
        {
            get
            {
                return _GroupIndexBookingFilter;
            }

            set
            {
                if (_GroupIndexBookingFilter == value)
                {
                    return;
                }

                _GroupIndexBookingFilter = value;
                RaisePropertyChanged();
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    CountCustomers();
                }
            }
        }

        public bool HasPeople => People.Count() > 0;

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

        public ObservableCollection<string> People
        {
            get
            {
                return _People;
            }

            set
            {
                if (_People == value)
                {
                    return;
                }

                _People = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand PrintAllLettersCommand { get; set; }

        public RelayCommand PrintAllVouchersCommand { get; set; }

        public RelayCommand PrintBusListsCommand { get; }

        public RelayCommand PrintListCommand { get; set; }

        public RelayCommand PrintRoomingListsCommand { get; set; }

        public RelayCommand PrintTheseisCommand { get; }

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
                    CountCustomers();
                }
                RaisePropertyChanged();
            }
        }

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

        public RelayCommand<string> ShowCanceled { get; set; }

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
                if (ReservationsCollectionView != null)
                {
                    ReservationsCollectionView.Refresh();
                    CountCustomers();
                }
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

        public void CountCustomers()
        {
            if (People == null)
            {
                People = new ObservableCollection<string>();
            }
            People.Clear();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            Dictionary<string, int> dict2 = new Dictionary<string, int>();
            int count, counter = 0;
            foreach (ReservationWrapper rw in ReservationsCollectionView)
            {
                count = 0;
                counter += rw.CustomersList.Count();
                switch (rw.CustomersList.Count)
                {
                    case 1:
                        dict.TryGetValue("Single", out count);
                        dict["Single"] = count + 1;
                        break;

                    case 2:
                        dict.TryGetValue("Double", out count);
                        dict["Double"] = count + 1;
                        break;

                    case 3:
                        dict.TryGetValue("Triple", out count);
                        dict["Triple"] = count + 1;
                        break;

                    case 4:
                        dict.TryGetValue("Quad", out count);
                        dict["Quad"] = count + 1;
                        break;

                    case 5:
                        dict.TryGetValue("5Bed", out count);
                        dict["5Bed"] = count + 1;
                        break;

                    default:
                        break;
                }
                try
                {
                    if (rw.Room != null)
                    {
                        dict2.TryGetValue(rw.Room.Hotel.Name, out count);
                        dict2[rw.Room.Hotel.Name] = count + rw.CustomersList.Count;
                    }
                    else if (rw.NoNameRoom != null)
                    {
                        dict2.TryGetValue(rw.Hotel.Name, out count);
                        dict2[rw.Hotel.Name] = count + rw.CustomersList.Count;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            var dictordered = from pair in dict2
                              orderby pair.Key ascending
                              select pair;

            if (dict.Count() > 0)
            {
                People.Add($"\"Άτομα\": {counter}");
                foreach (KeyValuePair<string, int> entry in dict)
                {
                    People.Add($"{entry.Key}: {entry.Value}");
                }
            }
            if (dictordered.Count() > 0)
            {
                People.Add("");
                People.Add($"\"Hotels\": {counter}");
                foreach (KeyValuePair<string, int> entry in dictordered)
                {
                    People.Add($"{entry.Key}: {entry.Value}");
                }
            }
            if (EnableCheckInFilter && SelectedExcursionFilter != null && SelectedExcursionFilter.Id > 0 && FilteredReservations != null && FilteredReservations.Count > 0)
            {
                CreateDepartureInfo();
            }
            else
            {
                if (AllDaysDeparturesList != null)
                {
                    AllDaysDeparturesList.Clear();
                }
            }

            //decimal total = 0, recieved = 0;
            //List<Booking> books = new List<Booking>();
            //foreach (var re in ReservationsCollectionView)
            //{
            //    if (re is ReservationWrapper r)
            //    {
            //        if (!books.Any(b => b.Id == r.Booking.Id))
            //        {
            //            books.Add(r.Booking);
            //        }
            //    }
            //}
            //foreach (var b in books.Select(ba => new BookingWrapper(ba)))
            //{
            //    b.CalculateRemainingAmount();
            //    total += b.NetPrice;
            //    recieved += b.Recieved;
            //}
            //People.Add("");
            //People.Add($"Total: {total.ToString("C2")}");
            //People.Add($"Recieved: {recieved.ToString("C2")}");
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                Context = new GenericRepository();
                Users = MainViewModel.BasicDataManager.Users;
                Excursions = MainViewModel.BasicDataManager.Excursions;
                SearchBookingsHelper = new SearchBookingsHelper(Context);
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

        public async Task PrintBusLists()
        {
            var buses = await Context.GetAllBusesAsync((ExcursionsCollectionView.CurrentItem as Excursion).Id, CheckIn);
            foreach (var b in buses)
            {
                List<Booking> bookings = new List<Booking>();
                bookings = new List<Booking>();

                foreach (var c in b.Customers)
                {
                    if (c.Reservation.Booking != null && !bookings.Contains(c.Reservation.Booking))
                    {
                        bookings.Add(c.Reservation.Booking);
                    }
                }
                int coutner = 0;

                foreach (Booking b1 in bookings)
                {
                    coutner += b1.ReservationsInBooking.Count;
                }

                using (DocumentsManagement vm = new DocumentsManagement(new GenericRepository()))
                {
                    // await vm.PrintList(bookings, CheckIn, b, true);
                    await vm.PrintList(bookings, CheckIn,true, b, false);
                }
            }
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private static bool Fits(int people, List<Thesi> fullbus, int arithmos)
        {
            for (int i = arithmos; i < arithmos + people; i++)
            {
                if (fullbus[arithmos].Customer != null)
                {
                    return false;
                }
            }

            return true;
        }

        private static int Getnextfree(List<Thesi> fullbus, int thesi)
        {
            for (int i = thesi; i <= fullbus.Count; i++)
            {
                if (fullbus[i].Customer == null)
                {
                    return i;
                }
                if (i == fullbus.Count)
                {
                    i = 0;
                }
            }
            return 0;
        }

        private bool CanDeleteBooking()
        {
            return SelectedReservation != null && SelectedReservation.Booking != null;
        }

        //    UserIndexBookingFilter = userId > 0 ? Users.IndexOf(Users.Where(u => u.Id == userId).FirstOrDefault()) + 1 : 0;
        //    if (excursionId > 0)
        //    {
        //        Excursion tmpExc = Excursions.First(e => e.Id == excursionId);
        //        if (ExcursionsCollectionView.Contains(tmpExc))
        //        {
        //            ExcursionsCollectionView.MoveCurrentTo(tmpExc);
        //        }
        //    }
        //    else
        //    {
        //        ExcursionCategoryIndexBookingFilter = 0;
        //    }
        //}
        private bool CanDeleteReservation()
        {
            return SelectedReservation != null;
        }

        //    SearchBookingsHelper = new SearchBookingsHelper(Context);
        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        //    //fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
        //    p.SaveAs(fileInfo);
        //    // Process.Start(path);
        //}
        //public override async Task ReloadAsync()
        //{
        //    if (Context != null && !Context.IsTaskOk)
        //    {
        //        await Context.LastTask;
        //    }
        //    Context = new GenericRepository();
        //    int userId = UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0;
        //    int excursionId = ExcursionIndexBookingFilter > 0 ? (ExcursionsCollectionView.CurrentItem as Excursion).Id : 0;
        //    Users = new ObservableCollection<User>(await Context.GetAllAsyncSortedByName<User>());
        //    Excursions = new ObservableCollection<Excursion>((await Context.GetAllExcursionsAsync()));
        private bool CanPrintRoomingLists()
        {
            return ExcursionIndexBookingFilter > 0;
        }

        //    FileInfo fileInfo = new FileInfo(path);
        //    ExcelPackage p = new ExcelPackage();
        //    p.Workbook.Worksheets.Add($"1");
        //    ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];
        //    //  myWorksheet.Cells["A1"].Value = roomingList.Hotel.Name;
        //    myWorksheet.Cells["A1:B1"].Merge = true;
        //    lineNum = 2;
        //    int thesi = 1;
        //    foreach (var rt in fullbus)
        //    {
        //        if (rt.Customer != null)
        //        {
        //            myWorksheet.Cells["A" + lineNum].Value = rt.Customer.Name;
        //            myWorksheet.Cells["B" + lineNum].Value = rt.Customer.Surename;
        //            myWorksheet.Cells["D" + lineNum].Value = thesi;
        //        }
        //        lineNum++;
        //        thesi += 1;
        //    }
        private bool CanShowReservations(string arg)
        {
            return IsOk;
        }

        private void CreateDepartureInfo()
        {
            DateTime tmpDate = CheckIn;
            DateTime maxDay = FilteredReservations.Max(r => r.CheckOut);
            if (AllDaysDeparturesList == null)
            {
                AllDaysDeparturesList = new ObservableCollection<DailyDepartureInfo>();
            }
            else
            {
                AllDaysDeparturesList.Clear();
            }
            DailyDepartureInfo dayDeparture = new DailyDepartureInfo(Context, SelectedExcursionFilter.Id);
            DailyDepartureInfo tmpDayDeparture = new DailyDepartureInfo(Context, SelectedExcursionFilter.Id);
            CityDepartureInfo cityDepartureInfo;
            if (SelectedExcursionFilter.ExcursionType.Category == ExcursionTypeEnum.Group)
            {
                foreach (ExcursionDate datePair in SelectedExcursionFilter.ExcursionDates)
                {
                    dayDeparture = new DailyDepartureInfo(Context, SelectedExcursionFilter.Id) { ExcursionDate = datePair, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };

                    foreach (ReservationWrapper r in ReservationsCollectionView)
                    {
                        if (r.Booking.ExcursionDate.Id == datePair.Id)
                        {
                            foreach (Customer customer in r.CustomersList)
                            {
                                if (!r.Booking.DifferentDates || (customer.CheckIn == r.Booking.ExcursionDate.CheckIn && customer.CheckOut == r.Booking.ExcursionDate.CheckOut))
                                {
                                    cityDepartureInfo = dayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                    if (cityDepartureInfo != null)
                                    {
                                        cityDepartureInfo.Going += 1;
                                        cityDepartureInfo.Returning += 1;
                                    }
                                    else
                                    {
                                        dayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace, Going = 1, Returning = 1 });
                                    }
                                }
                                else
                                {
                                    //checkin
                                    if (AllDaysDeparturesList.Any(d => d.Date == customer.CheckIn))
                                    {
                                        tmpDayDeparture = AllDaysDeparturesList.Where(d => d.Date == customer.CheckIn).FirstOrDefault();
                                    }
                                    else
                                    {
                                        tmpDayDeparture = new DailyDepartureInfo(Context, SelectedExcursionFilter.Id) { Date = customer.CheckIn, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };
                                        AllDaysDeparturesList.Add(tmpDayDeparture);
                                    }
                                    cityDepartureInfo = tmpDayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                    if (cityDepartureInfo != null)
                                    {
                                        cityDepartureInfo.Going += 1;
                                    }
                                    else
                                    {
                                        tmpDayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace, Going = 1, Returning = 0 });
                                    }

                                    //checkout
                                    if (AllDaysDeparturesList.Any(d => d.Date == customer.CheckOut))
                                    {
                                        tmpDayDeparture = AllDaysDeparturesList.Where(d => d.Date == customer.CheckOut).FirstOrDefault();
                                    }
                                    else
                                    {
                                        tmpDayDeparture = new DailyDepartureInfo(Context, SelectedExcursionFilter.Id) { Date = customer.CheckOut, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };
                                        AllDaysDeparturesList.Add(tmpDayDeparture);
                                    }
                                    cityDepartureInfo = tmpDayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                    if (cityDepartureInfo != null)
                                    {
                                        cityDepartureInfo.Returning += 1;
                                    }
                                    else
                                    {
                                        tmpDayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace, Going = 0, Returning = 1 });
                                    }
                                }
                            }
                        }
                    }
                    if (dayDeparture.PerCityDepartureList.Count > 0)
                        AllDaysDeparturesList.Add(dayDeparture);
                }
            }
            else
            {
                while (tmpDate <= maxDay)
                {
                    dayDeparture = new DailyDepartureInfo(Context, SelectedExcursionFilter.Id) { Date = tmpDate, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };

                    foreach (ReservationWrapper r in ReservationsCollectionView)
                    {
                        if (r.Booking.CheckIn == tmpDate)
                        {
                            foreach (Customer customer in r.CustomersList)
                            {
                                cityDepartureInfo = dayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                if (cityDepartureInfo == null)
                                {
                                    dayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace });
                                    cityDepartureInfo = dayDeparture.PerCityDepartureList[dayDeparture.PerCityDepartureList.Count - 1];
                                }
                                if (!r.OnlyStay && ((SelectedExcursionFilter.IncludesShip && customer.CustomerHasShipIndex < 2) || (SelectedExcursionFilter.IncludesBus && customer.CustomerHasBusIndex < 2)))
                                {
                                    if (SelectedExcursionFilter.IncludesShip && SelectedExcursionFilter.IncludesBus && customer.CustomerHasShipIndex > 1)
                                    {
                                        cityDepartureInfo.OnlyBusGo++;
                                    }
                                    else if (SelectedExcursionFilter.IncludesShip && SelectedExcursionFilter.IncludesBus && customer.CustomerHasBusIndex > 1)
                                    {
                                        cityDepartureInfo.OnlyShipGo++;
                                    }
                                    else if (r.ReservationType == Model.ReservationTypeEnum.OneDay)
                                    {
                                        cityDepartureInfo.OneDayGo++;
                                    }
                                    else
                                    {
                                        cityDepartureInfo.Going++;
                                    }
                                }
                                else
                                {
                                    cityDepartureInfo.OnlyStayGo++;
                                }
                            }
                        }
                        if (r.Booking.CheckOut == tmpDate)
                        {
                            foreach (Customer customer in r.CustomersList)
                            {
                                cityDepartureInfo = dayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                if (cityDepartureInfo == null)
                                {
                                    dayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace });
                                    cityDepartureInfo = dayDeparture.PerCityDepartureList[dayDeparture.PerCityDepartureList.Count - 1];
                                }
                                if (!r.OnlyStay && ((SelectedExcursionFilter.IncludesShip && (customer.CustomerHasShipIndex == 0 || customer.CustomerHasShipIndex == 2)) || (SelectedExcursionFilter.IncludesBus && (customer.CustomerHasBusIndex == 0 || customer.CustomerHasBusIndex == 2))))
                                {
                                    if (SelectedExcursionFilter.IncludesShip && SelectedExcursionFilter.IncludesBus && (customer.CustomerHasShipIndex == 1 || customer.CustomerHasShipIndex == 3))
                                    {
                                        cityDepartureInfo.OnlyBusReturn++;
                                    }
                                    else if (SelectedExcursionFilter.IncludesShip && SelectedExcursionFilter.IncludesBus && (customer.CustomerHasBusIndex == 1 || customer.CustomerHasBusIndex == 3))
                                    {
                                        cityDepartureInfo.OnlyShipReturn++;
                                    }
                                    else if (r.ReservationType == Model.ReservationTypeEnum.OneDay)
                                    {
                                        cityDepartureInfo.OneDayReturn++;
                                    }
                                    else
                                    {
                                        cityDepartureInfo.Returning++;
                                    }
                                }
                                else
                                {
                                    cityDepartureInfo.OnlyStayReturn++;
                                }
                            }
                        }
                    }

                    if (dayDeparture.PerCityDepartureList.Count > 0)
                        AllDaysDeparturesList.Add(dayDeparture);
                    tmpDate = tmpDate.AddDays(1);
                }
            }

            foreach (var day in AllDaysDeparturesList)
            {
                day.CalculateTotals();
            }
        }

        //    int lineNum;
        //    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RoomingLists");
        //    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"{bus.Vehicle.Name}.xlsx";
        private bool CustomerExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return Completed || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today) || excursion.Id == 0;
        }

        //    int arithmos = 1;
        //    int ypoloipo = 0;
        //    int aasd = 0;
        //    while (parees.Any(t => !t.Done))
        //    {
        //        aasd++;
        //        if (aasd > 100)
        //        {
        //            break;
        //        }
        //        counter++;
        //        arithmos = Getnextfree(fullbus, arithmos);
        //        //if (first && parees[0].Customers.Count % 2 == 1)
        //        //{
        //        //    arithmos = 5;
        //        //}
        //        if (arithmos > 1)
        //            ypoloipo = arithmos % 2;
        //        foreach (var parea in parees)
        //        {
        //            if ((arithmos % 2 == 1 || parea.Customers.Count % 2 == ((arithmos + 1) % 2)) && !parea.Done && Fits(parea.Customers.Count, fullbus, arithmos))
        //            {
        //                foreach (var c in parea.Customers)
        //                {
        //                    if (fullbus[arithmos].Customer == null)
        //                    {
        //                        fullbus[arithmos].Customer = c;
        //                        arithmos++;
        //                    }
        //                }
        //                parea.Done = true;
        //                ypoloipo = arithmos % 2;
        //            }
        //            //if (!perase && parea.Customers.Count % 2 != ypoloipo)
        //            //{
        //            //    arithmos += 3;
        //            //    ypoloipo += 1 % 2;
        //            //    break;
        //            //}
        //        }
        //    }
        private bool CustomerFilter(object item)
        {
            ReservationWrapper reservation = item as ReservationWrapper;
            var x = reservation.Contains(FilterString) &&
                (GroupIndexBookingFilter == 0 || (reservation.ExcursionType != ExcursionTypeEnum.Personal && reservation.ExcursionType != ExcursionTypeEnum.ThirdParty && ((GroupIndexBookingFilter == 1 && reservation.Booking != null && reservation.Booking.GroupBooking) || (GroupIndexBookingFilter == 2 && !reservation.Booking.GroupBooking)))) &&
                (BookingIdFilter == 0 || reservation.Booking.Id == BookingIdFilter) &&
                (!CheckInOut || ((!EnableCheckInFilter || reservation.CustomersList.Any(c => c.CheckIn == CheckIn || ((reservation.PersonalModel != null || reservation.ThirdPartyModel != null) && reservation.CheckIn == CheckIn))) &&
                (!EnableCheckOutFilter || reservation.CustomersList.Any(c => c.CheckOut == CheckOut || ((reservation.PersonalModel != null || reservation.ThirdPartyModel != null) && reservation.CheckOut == CheckOut))))) &&
                (!Remaining || reservation.Remaining > 3) &&
                (UserIndexBookingFilter == 0 || reservation.UserWr.Id == Users[UserIndexBookingFilter - 1].Id) &&
                (DepartmentIndexBookingFilter == 0 || reservation.UserWr.BaseLocation == DepartmentIndexBookingFilter) &&
                (ExcursionCategoryIndexBookingFilter == 0 || (ExcursionCategoryIndexBookingFilter == 1 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Bansko) || (ExcursionCategoryIndexBookingFilter == 2 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Skiathos) || (ExcursionCategoryIndexBookingFilter == 3 && reservation.PersonalModel != null) || (ExcursionCategoryIndexBookingFilter == 4 && reservation.Booking != null && reservation.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Group) || (ExcursionCategoryIndexBookingFilter == 5 && reservation.ThirdPartyModel != null)) &&
                (ExcursionIndexBookingFilter == 0 || (reservation.Booking != null && ExcursionsCollectionView.CurrentItem != null && ExcursionsCollectionView.CurrentItem is Excursion && reservation.Booking.Excursion != null && reservation.Booking.Excursion.Id == ((Excursion)ExcursionsCollectionView.CurrentItem).Id)) &&
                (!Bank || (reservation.ExcursionType != ExcursionTypeEnum.Personal && reservation.ExcursionType != ExcursionTypeEnum.ThirdParty && reservation.Booking.Payments.Any(p => p.PaymentMethod > 0)) || ((reservation.ExcursionType == ExcursionTypeEnum.Personal) && reservation.PersonalModel.Payments.Any(p => p.PaymentMethod > 0)) || ((reservation.ExcursionType == ExcursionTypeEnum.ThirdParty) && reservation.ThirdPartyModel.Payments.Any(p => p.PaymentMethod > 0)));
            return x;
        }

        //    parees = parees.OrderByDescending(p1 => p1.Customers.Count).ToList();
        private async void DeleteBooking()
        {
            await SearchBookingsHelper.DeleteBooking(SelectedReservation.Booking);
        }

        //private static void PrintTheseisBus(Bus bus)
        //{
        //    List<Parea> parees = new List<Parea>();
        //    Parea tmpparea;
        //    int counter = 0;
        //    foreach (var c in bus.Customers.Where(y => y.LeaderDriver == 0))
        //    {
        //        if (parees.Any(r => r.BookingId == c.Reservation.Booking.Id))
        //        {
        //            parees.Where(o => o.BookingId == c.Reservation.Booking.Id).FirstOrDefault().Customers.Add(new CustomerWrapper(c));
        //        }
        //        else
        //        {
        //            counter++;
        //            tmpparea = new Parea { Counter = counter, BookingId = c.Reservation.Booking.Id };
        //            tmpparea.Customers.Add(new CustomerWrapper(c));
        //            parees.Add(tmpparea);
        //        }
        //    }
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
            CountCustomers();
        }

        private void PrintAllLetters()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                List<Booking> bookings = new List<Booking>();

                foreach (ReservationWrapper r in CollectionViewSource.GetDefaultView(FilteredReservations))
                {
                    if (r.ExcursionType != ExcursionTypeEnum.Personal && r.ReservationType != ReservationTypeEnum.OneDay && !bookings.Contains(r.Booking))
                    {
                        bookings.Add((r as ReservationWrapper).Booking);
                    }
                }
                int coutner = 0;

                foreach (Booking b in bookings)
                {
                    if (b.Excursion.ExcursionType.Category != ExcursionTypeEnum.Personal)
                    {
                        try
                        {
                            coutner += b.ReservationsInBooking.Count;
                        }
                        catch (Exception ex)
                        {
                            MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                        }
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
                var x = await Context.GetAllCitiesAsyncSortedByName();
                var buses = await Context.GetAllBusesAsync();
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
                    if (bookings.Any(b => b.ReservationsInBooking.Any(r => r.CustomersList.Any(c => c.BusGo == null)))) ;
                    {
                    }
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
                    //await vm.PrintAllPhones();
                    await vm.PrintList(bookings, CheckIn,true);
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
            //var date = new DateTime(2019, 5, 28);
            // int counter = 0;
            // var dimitris = await Context.GetAllReservationsDimitri(r => r.Booking.Excursion.ExcursionType.Category == ExcursionTypeEnum.Skiathos && r.CustomersList.Any(c => c.CheckIn >= date));

            //foreach (var r in dimitris)
            //{
            //    counter += r.CustomersList.Count;
            //}

            Mouse.OverrideCursor = Cursors.Wait;
            if (!EnableCheckInFilter || !EnableCheckOutFilter)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε ημερομηνίες"));
            }
            else
            {
                Lists listmanager = new Lists(ExcursionsCollectionView.CurrentItem as Excursion, CheckIn, CheckOut);
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

        private async Task PrintTheseis()
        {
            var buses = await Context.GetAllBusesAsync((ExcursionsCollectionView.CurrentItem as Excursion).Id, CheckIn);

            foreach (var bus in buses)
            {
                //  PrintTheseisBus(bus);
            }
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
                CountCustomers();
            }
        }

        private async Task ShowReservations(string parameter, bool canceled)
        {
            try
            {
                if (ExcursionIndexBookingFilter > 0)
                {
                    SelectedExcursionFilter = ExcursionsCollectionView.CurrentItem as Excursion;
                }
                else
                {
                    SelectedExcursionFilter = null;
                }
                IsOk = false;
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                //await ReloadAsync();
                if (Context != null)
                {
                    Context.Dispose();
                }
                Context = new GenericRepository(true);
                SearchBookingsHelper = new SearchBookingsHelper(Context);
                DateTime dateLimit = SearchBookingsHelper.GetDateLimit(parameter);
                //if (parameter!="40")
                //{
                List<ReservationWrapper> list = (await Context.GetAllReservationsFiltered(ExcursionIndexBookingFilter > 0 ? (ExcursionsCollectionView.CurrentItem as Excursion).Id : 0, UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, Completed, ExcursionCategoryIndexBookingFilter > 0 ? ExcursionCategoryIndexBookingFilter - 1 : -1, dateLimit, EnableCheckInFilter, EnableCheckOutFilter, CheckIn, CheckOut, FromTo, CheckInOut, canceled: canceled)).Select(r => new ReservationWrapper(r)).ToList();
                //}
                //else
                //{
                //    List<ReservationWrapper> list = (await Context.GetAllReservationsFilteredWithName(ExcursionIndexBookingFilter > 0 ? (ExcursionsCollectionView.CurrentItem as Excursion).Id : 0, UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, Completed, ExcursionCategoryIndexBookingFilter > 0 ? ExcursionCategoryIndexBookingFilter - 1 : -1, dateLimit, EnableCheckInFilter, EnableCheckOutFilter, CheckIn, CheckOut, FromTo, CheckInOut, canceled: canceled)).Select(r => new ReservationWrapper(r)).ToList();

                //}

                //int counter = 0;
                //foreach (var reservation in list)
                //{
                // Clipboard.SetText(sb.ToString());
                //    if (reservation.CheckIn > new DateTime(2019, 7, 3))
                //    {
                //        counter += reservation.CustomersList.Count;
                //    }
                //}

                //foreach (var item in list)
                //{
                //    if (item.ReservationType == ReservationTypeEnum.Overbooked)
                //    {
                //    }
                //    try
                //    {
                //        if (item.Room != null && item.CustomersList.Count < item.Room.RoomType.MinCapacity)
                //        {
                //            if (item.CustomersList.Count == 1 && item.Room.RoomType.MinCapacity==2)
                //            {
                //            }
                //            else
                //            {
                //            }

                //        }
                //    }
                //    catch (Exception)
                //    {
                //        throw;
                //    }
                //}
                //mporei na ginei poly kalytero kratwntas se mia var to apo panw
                int counter = 0;
                if (!Completed)
                    for (int i = 0; i < list.Count; i++)
                    {
                        foreach (var c in list[i].CustomersList)
                        {
                            for (counter = i; counter < list.Count; counter++)
                            {
                                foreach (var c2 in list[counter].CustomersList)
                                {
                                    if (c.Id != c2.Id && !string.IsNullOrEmpty(c.Tel) && c.Tel.Length > 9 && c.Tel.StartsWith("6") && c.Tel == c2.Tel)
                                        System.Windows.MessageBox.Show($"{c.Name} {c.Surename} - {c2.Name} {c2.Surename}");
                                }
                            }
                        }
                    }

                if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 3 || ExcursionCategoryIndexBookingFilter == 0))
                {
                    await Context.GetAllCitiesAsyncSortedByName();
                    await Context.GetAllHotelsAsync<Hotel>();
                    list.AddRange((await Context.GetAllPersonalBookingsFiltered(UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, Completed, dateLimit, canceled: canceled)).Select(r => new ReservationWrapper { Id = r.Id, PersonalModel = new Personal_BookingWrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                }
                if (ExcursionIndexBookingFilter == 0 && (ExcursionCategoryIndexBookingFilter == 5 || ExcursionCategoryIndexBookingFilter == 0))
                {
                    list.AddRange((await Context.GetAllThirdPartyBookingsFiltered(UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, Completed, dateLimit, canceled: canceled)).Select(r => new ReservationWrapper { Id = r.Id, ThirdPartyModel = new ThirdParty_Booking_Wrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                }

                FilteredReservations = new ObservableCollection<ReservationWrapper>(list);
                Parallel.ForEach(FilteredReservations, wr => wr.CalculateAmounts());
                ReservationsCollectionView = CollectionViewSource.GetDefaultView(FilteredReservations);
                ReservationsCollectionView.Filter = CustomerFilter;
                ReservationsCollectionView.SortDescriptions.Add(new SortDescription("CreatedDate", ListSortDirection.Ascending));
                CountCustomers();
                RaisePropertyChanged(nameof(HasPeople));
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