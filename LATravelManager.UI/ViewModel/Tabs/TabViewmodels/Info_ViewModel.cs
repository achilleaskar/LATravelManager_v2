using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class Info_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public Info_ViewModel(Window_ViewModels.MainViewModel mainViewModel)
        {
            ToDepartureInfo = /*(Properties.Settings.Default.toDeparture >= DateTime.Today) ? Properties.Settings.Default.toDeparture :*/ DateTime.Today;
            FromDepartureInfo = /*(Properties.Settings.Default.fromDepartures > DateTime.Today) ? Properties.Settings.Default.fromDepartures :*/ DateTime.Today;
            ShowDepartureInfoCommand = new RelayCommand(async () => { await ShowDepartureInfo(); }, CanShowDepartureInfoM);
            //SaveTransfersCommand = new SaveTransfersCommand(this);
            ShowIncomesCommand = new RelayCommand(async () => { await ShowIncomes(); }, CanShowIncomes);
            MainViewModel = mainViewModel;
            From = To = DateTime.Today;
        }

        private bool CanShowDepartureInfoM()
        {
            return SelectedFilterExcursion != null;
        }

        private int _Customers;


        public int Customers
        {
            get
            {
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Partner> _Partners;


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
        private int _PartnerIndexBookingFilter;


        public int PartnerIndexBookingFilter
        {
            get
            {
                return _PartnerIndexBookingFilter;
            }

            set
            {
                if (_PartnerIndexBookingFilter == value)
                {
                    return;
                }

                _PartnerIndexBookingFilter = value;
                RaisePropertyChanged();
            }
        }
        private bool CanShowIncomes()
        {
            return ExcursionCategoryIndexBookingFilter != 3 && ExcursionCategoryIndexBookingFilter != 5;
        }

        private async Task ShowIncomes()
        {
            Total = Net = Commission = Customers = 0;
            IncomesContext = new GenericRepository();
            List<BookingWrapper> bookings = (await IncomesContext.GetAllBookingsFiltered(
                ExcursionIndexBookingFilter == 0 ? -1 : ((Excursion)ExcursionsIncomeCollectionView.CurrentItem).Id,
                ExcursionCategoryIndexBookingFilter == 0 ? -1 : ExcursionCategoryIndexBookingFilter - 1,
                DepartmentIndexBookingFilter,
                UserIndexBookingFilter == 0 ? -1 : Users[UserIndexBookingFilter - 1].Id,
                PartnerIndexBookingFilter == 0 ? -1 : Partners[PartnerIndexBookingFilter - 1].Id,
                EnableFromFilter ? From : DateTime.MinValue,
                EnableToFilter ? To : DateTime.MaxValue)).Select(b => new BookingWrapper(b)).ToList();

            foreach (var b in bookings)
            {
                _Total += b.FullPrice;
                foreach (var r in b.ReservationsInBooking)
                {
                    _Customers += r.CustomersList.Count;
                }
                if (b.IsPartners)
                {
                    _Net += b.NetPrice;
                }
                else
                {
                    _Net += b.FullPrice;
                }
            }
            _Commission = Total - Net;
            RaisePropertyChanged(nameof(Total));
            RaisePropertyChanged(nameof(Customers));
            RaisePropertyChanged(nameof(Net));
            RaisePropertyChanged(nameof(Commission));

        }

        #endregion Constructors

        #region Fields

        private static readonly string[] ValidateDepartureInfoProperties =
        {
            nameof(FromDepartureInfo),nameof(ToDepartureInfo)
        };

        private ObservableCollection<DailyDepartureInfo> _AllDaysDeparturesList = new ObservableCollection<DailyDepartureInfo>();

        private string _CanShowDepartureInfoError;

        private GenericRepository _Context;

        private ObservableCollection<Customer> _CustomersToAdd = new ObservableCollection<Customer>();

        private ObservableCollection<Excursion> _Excursions;

        private DateTime _FromDateTime = DateTime.Today;

        private DateTime _FromDepartureInfo;

        private DateTime _ToDateTime = DateTime.Today;

        private DateTime _ToDepartureInfo;

        #endregion Fields

        #region Properties

        private ICollectionView _ExcursionsDepartureCollectionView;

        public ICollectionView ExcursionsDepartureCollectionView
        {
            get
            {
                return _ExcursionsDepartureCollectionView;
            }

            set
            {
                if (_ExcursionsDepartureCollectionView == value)
                {
                    return;
                }

                _ExcursionsDepartureCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<DailyDepartureInfo> AllDaysDeparturesList
        {
            get => _AllDaysDeparturesList;
            set
            {
                if (_AllDaysDeparturesList != value)
                {
                    _AllDaysDeparturesList = value;
                    RaisePropertyChanged(nameof(AllDaysDeparturesList));
                }
            }
        }

        public bool AreDepartureInfoDataValid
        {
            get
            {
                foreach (string property in ValidateDepartureInfoProperties)
                {
                    string tmpError = GetDepartureInfoError(property);
                    if (tmpError != null)
                    {
                        ShowDepartureInfoError = tmpError;
                        return false;
                    }
                }
                ShowDepartureInfoError = "";
                return true;
            }
        }

        public bool CanShowDepartureInfo => AreDepartureInfoDataValid;

        private GenericRepository _IncomesContext;

        public GenericRepository IncomesContext
        {
            get
            {
                return _IncomesContext;
            }

            set
            {
                if (_IncomesContext == value)
                {
                    return;
                }

                _IncomesContext = value;
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

        public ObservableCollection<Customer> CustomersToAdd
        {
            get => _CustomersToAdd;
            set
            {
                if (_CustomersToAdd != value)
                {
                    _CustomersToAdd = value;
                    RaisePropertyChanged(nameof(CustomersToAdd));
                }
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

                ExcursionsIncomeCollectionView = new CollectionViewSource { Source = Excursions }.View;
                ExcursionsIncomeCollectionView.Filter = IncomeExcursionsFilter;
                ExcursionsIncomeCollectionView.SortDescriptions.Add(new SortDescription("FirstDate", ListSortDirection.Ascending));

                ExcursionsDepartureCollectionView = new CollectionViewSource { Source = Excursions }.View;
                ExcursionsDepartureCollectionView.Filter = InfoExcursionsFilter;
                ExcursionsDepartureCollectionView.SortDescriptions.Add(new SortDescription("FirstDate", ListSortDirection.Ascending));

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ExcursionsIncomeCollectionView));
                RaisePropertyChanged(nameof(ExcursionsDepartureCollectionView));
            }
        }

        private DateTime _To;

        public DateTime To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }

                _To = value;
                if (value < From)
                {
                    From = value;
                }
                RaisePropertyChanged();
            }
        }

        private DateTime _From;

        public DateTime From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                _From = value;
                if (value > To)
                {
                    To = value;
                }
                RaisePropertyChanged();
            }
        }

        private bool _EnableToFilter;

        public bool EnableToFilter
        {
            get
            {
                return _EnableToFilter;
            }

            set
            {
                if (_EnableToFilter == value)
                {
                    return;
                }

                _EnableToFilter = value;
                RaisePropertyChanged();
            }
        }

        private bool _EnableFromFilter;

        public bool EnableFromFilter
        {
            get
            {
                return _EnableFromFilter;
            }

            set
            {
                if (_EnableFromFilter == value)
                {
                    return;
                }

                _EnableFromFilter = value;
                RaisePropertyChanged();
            }
        }

        private int _ExcursionCategoryIndexBookingFilter;

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

        public RelayCommand ShowIncomesCommand { get; set; }

        private decimal _Commission;

        public decimal Commission
        {
            get
            {
                return _Commission;
            }

            set
            {
                if (_Commission == value)
                {
                    return;
                }

                _Commission = value;
                RaisePropertyChanged();
            }
        }

        private decimal _Net;

        public decimal Net
        {
            get
            {
                return _Net;
            }

            set
            {
                if (_Net == value)
                {
                    return;
                }

                _Net = value;
                RaisePropertyChanged();
            }
        }

        private decimal _Total;

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

        private int _UserIndexBookingFilter;

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

        private ObservableCollection<User> _Users;

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
            }
        }

        private int _ExcursionIndexBookingFilter;

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

        private bool _CompletedIncomeFilter;

        public bool CompletedIncomeFilter
        {
            get
            {
                return _CompletedIncomeFilter;
            }

            set
            {
                if (_CompletedIncomeFilter == value)
                {
                    return;
                }

                _CompletedIncomeFilter = value;
                ExcursionsIncomeCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        private bool IncomeExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return CompletedIncomeFilter || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today);
        }

        private bool InfoExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return excursion.Id > 0 && (Completed || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today));
        }

        public DateTime FromDateTime
        {
            get => _FromDateTime;
            set
            {
                if (_FromDateTime != value)
                {
                    _FromDateTime = value;
                    RaisePropertyChanged(nameof(FromDateTime));
                }
            }
        }

        public DateTime FromDepartureInfo
        {
            get => _FromDepartureInfo;
            set
            {
                if (_FromDepartureInfo != value)
                {
                    _FromDepartureInfo = value;
                    //if (Properties.Settings.Default.fromDepartures != value)
                    //{
                    //    Properties.Settings.Default.fromDepartures = value;
                    //    Properties.Settings.Default.Save();
                    //}
                    if (ToDepartureInfo < FromDepartureInfo)
                        ToDepartureInfo = FromDepartureInfo;
                    RaisePropertyChanged(nameof(FromDepartureInfo));
                }
            }
        }

        public RelayCommand SaveTransfersCommand
        {
            get;
            private set;
        }

        public RelayCommand ShowDepartureInfoCommand { get; set; }

        public string ShowDepartureInfoError
        {
            get => _CanShowDepartureInfoError;
            set
            {
                if (_CanShowDepartureInfoError != value)
                {
                    _CanShowDepartureInfoError = value;
                    RaisePropertyChanged(nameof(ShowDepartureInfoError));
                }
                _CanShowDepartureInfoError = value;
            }
        }

        public DateTime ToDateTime
        {
            get => _ToDateTime;
            set
            {
                if (_ToDateTime != value)
                {
                    _ToDateTime = value;
                    RaisePropertyChanged(nameof(ToDateTime));
                }
            }
        }

        public DateTime ToDepartureInfo
        {
            get => _ToDepartureInfo;
            set
            {
                if (_ToDepartureInfo != value)
                {
                    _ToDepartureInfo = value;
                    //if (Properties.Settings.Default.toDeparture != value)
                    //{
                    //    Properties.Settings.Default.toDeparture = value;
                    //    Properties.Settings.Default.Save();
                    //}
                    RaisePropertyChanged(nameof(ToDepartureInfo));
                }
            }
        }

        #endregion Properties

        #region Methods

        public ICollectionView ExcursionsIncomeCollectionView
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

        private bool _Completed;

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
                ExcursionsDepartureCollectionView.Refresh();

                RaisePropertyChanged();
            }
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                Thread.Sleep(0);
                Context = new GenericRepository();

                Excursions = new ObservableCollection<Excursion>(MainViewModel.BasicDataManager.Excursions.OrderBy(e => e.ExcursionDates.OrderBy(ed => ed.CheckIn).FirstOrDefault().CheckIn));
                Users = new ObservableCollection<User>(MainViewModel.BasicDataManager.Users);
                Partners = new ObservableCollection<Partner>(MainViewModel.BasicDataManager.Partners);
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

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        private Excursion _SelectedFilterExcursion;

        public Excursion SelectedFilterExcursion
        {
            get
            {
                return _SelectedFilterExcursion;
            }

            set
            {
                if (_SelectedFilterExcursion == value)
                {
                    return;
                }

                _SelectedFilterExcursion = value;
                RaisePropertyChanged();
            }
        }

        //internal void AddTranferReservation()
        //{
        //    Window mw = Application.Current.MainWindow;
        //    mw.Hide();
        //    var brw = new BookRoomWindow(new Room { Id = -2, RoomType = new RoomType { Name = "Bus", Id = -2, MainCapacity = 64 }, Hotel = new Hotel { Name = "TANSFER", Id = -2 } }, FromDateTime, ToDateTime, mw);
        //    brw.ShowDialog();
        //}

        public List<BookingWrapper> Bookings { get; set; }

        internal async Task ShowDepartureInfo()
        {
            Context = new GenericRepository();
            Mouse.OverrideCursor = Cursors.Wait;
            Bookings = (await Context.GetAllBookingInPeriodNoTracking(FromDepartureInfo, ToDepartureInfo, SelectedFilterExcursion.Id)).Select(b => new BookingWrapper(b)).ToList();
            DateTime tmpDate = FromDepartureInfo;
            AllDaysDeparturesList.Clear();
            DailyDepartureInfo dayDeparture = new DailyDepartureInfo();
            DailyDepartureInfo tmpDayDeparture = new DailyDepartureInfo();
            CityDepartureInfo cityDepartureInfo;
            if (SelectedFilterExcursion.ExcursionType.Category == Model.Enums.ExcursionTypeEnum.Group)
            {
                foreach (ExcursionDate datePair in SelectedFilterExcursion.ExcursionDates)
                {
                    dayDeparture = new DailyDepartureInfo { ExcursionDate = datePair, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };

                    foreach (BookingWrapper bwr in Bookings)
                    {
                        if (bwr.DifferentDates)
                        {
                        }
                        if (bwr.ExcursionDate == datePair)
                        {
                            foreach (CustomerWrapper customer in bwr.Customers)
                            {
                                if (!bwr.DifferentDates || (customer.CheckIn == bwr.ExcursionDate.CheckIn && customer.CheckOut == bwr.ExcursionDate.CheckOut))
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
                                        tmpDayDeparture = new DailyDepartureInfo { Date = customer.CheckIn, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };
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
                                        tmpDayDeparture = new DailyDepartureInfo { Date = customer.CheckOut, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };
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
                while (tmpDate <= ToDepartureInfo)
                {
                    dayDeparture = new DailyDepartureInfo { Date = tmpDate, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };

                    foreach (BookingWrapper bwr in Bookings)
                    {
                        if (bwr.CheckIn == tmpDate)
                        {
                            foreach (CustomerWrapper customer in bwr.Customers)
                            {
                                cityDepartureInfo = dayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                if (cityDepartureInfo != null)
                                    cityDepartureInfo.Going += 1;
                                else
                                    dayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace, Going = 1, Returning = 0 });
                            }
                        }
                        if (bwr.CheckOut == tmpDate)
                        {
                            foreach (CustomerWrapper customer in bwr.Customers)
                            {
                                cityDepartureInfo = dayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                if (cityDepartureInfo != null)
                                    cityDepartureInfo.Returning += 1;
                                else
                                    dayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace, Returning = 1, Going = 0 });
                            }
                        }
                    }

                    if (dayDeparture.PerCityDepartureList.Count > 0)
                        AllDaysDeparturesList.Add(dayDeparture);
                    tmpDate = tmpDate.AddDays(1);
                }
            }
            AllDaysDeparturesCollectionView = CollectionViewSource.GetDefaultView(AllDaysDeparturesList);
            AllDaysDeparturesCollectionView.SortDescriptions.Add(new SortDescription(nameof(DailyDepartureInfo.Date), ListSortDirection.Ascending));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private ICollectionView _AllDaysDeparturesCollectionView;
        private ICollectionView _ExcursionsCollectionView;

        public ICollectionView AllDaysDeparturesCollectionView
        {
            get
            {
                return _AllDaysDeparturesCollectionView;
            }

            set
            {
                if (_AllDaysDeparturesCollectionView == value)
                {
                    return;
                }

                _AllDaysDeparturesCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public Window_ViewModels.MainViewModel MainViewModel { get; }

        private string GetDepartureInfoError(string propertyName)
        {
            string error = null;
            switch (propertyName)
            {
                case nameof(FromDepartureInfo):
                    error = ValidateFromDepartureInfo();
                    break;

                case nameof(ToDepartureInfo):
                    error = ValidateToDepartureInfo();
                    break;
            }
            return error;
        }

        private string ValidateFromDepartureInfo()
        {
            if (FromDepartureInfo < DateTime.Today)
            {
                return "Η ημερομηνία έναρξης έχει παρέλθει";
            }
            return null;
        }

        private string ValidateToDepartureInfo()
        {
            if (ToDepartureInfo < DateTime.Today)
            {
                return "Η ημερομηνία λήξης έχει παρέλθει";
            }
            if (ToDepartureInfo < FromDepartureInfo)
            {
                return "Η ημερομηνία λήξης πρέπει να είναι πρίν την ημερομηνία λήξης";
            }
            return null;
        }

        #endregion Methods
    }
}