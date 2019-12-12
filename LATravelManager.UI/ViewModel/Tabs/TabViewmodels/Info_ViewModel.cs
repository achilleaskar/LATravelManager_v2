using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
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
    public class Info_ViewModel : MyViewModelBase
    {
        #region Constructors

        public Info_ViewModel(MainViewModel mainViewModel)
        {
            ToDepartureInfo = DateTime.Today;
            FromDepartureInfo = DateTime.Today;
            ShowDepartureInfoCommand = new RelayCommand(async () => { await ShowDepartureInfo(); }, CanShowDepartureInfo);
            MainViewModel = mainViewModel;
            Load();
        }

        #endregion Constructors

        #region Fields

        private static readonly string[] ValidateDepartureInfoProperties =
       {
            nameof(FromDepartureInfo),nameof(ToDepartureInfo)
        };

        private ICollectionView _AllDaysDeparturesCollectionView;

        private ObservableCollection<DailyDepartureInfo> _AllDaysDeparturesList = new ObservableCollection<DailyDepartureInfo>();

        private string _CanShowDepartureInfoError;

        private bool _Completed;

        private GenericRepository _Context;

        private ObservableCollection<Excursion> _Excursions;

        private ICollectionView _ExcursionsDepartureCollectionView;

        private DateTime _FromDepartureInfo;

        private Excursion _SelectedFilterExcursion;

        private DateTime _ToDepartureInfo;

        #endregion Fields

        #region Properties

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
                return SelectedFilterExcursion != null;
            }
        }

        public List<Booking> Bookings { get; set; }

        public bool CanShowDepartureInfo => AreDepartureInfoDataValid;

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

                ExcursionsDepartureCollectionView = new CollectionViewSource { Source = Excursions }.View;
                ExcursionsDepartureCollectionView.Filter = InfoExcursionsFilter;
                ExcursionsDepartureCollectionView.SortDescriptions.Add(new SortDescription("FirstDate", ListSortDirection.Ascending));

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ExcursionsDepartureCollectionView));
            }
        }

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

        public MainViewModel MainViewModel { get; }

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
                if (SelectedFilterExcursion != null && SelectedFilterExcursion.ExcursionDates != null)
                {
                    try
                    {
                        if (SelectedFilterExcursion.ExcursionDates != null && SelectedFilterExcursion.ExcursionDates.Count > 0)
                        {
                            var x = SelectedFilterExcursion.ExcursionDates.Where(c => c.CheckIn >= DateTime.Today);
                            if (x != null && x.Count() > 0)
                            {
                                FromDepartureInfo = x.Min(e => e.CheckIn);

                            }
                            else
                                FromDepartureInfo = DateTime.Today;

                        }
                    }
                    catch
                    {
                        FromDepartureInfo = DateTime.Today;
                    }
                    if (FromDepartureInfo < DateTime.Today)
                        FromDepartureInfo = DateTime.Today;
                    ToDepartureInfo = SelectedFilterExcursion.ExcursionDates.Max(e => e.CheckOut);
                    if (ToDepartureInfo > FromDepartureInfo.AddDays(15))
                        ToDepartureInfo = FromDepartureInfo.AddDays(15);
                }
            }
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

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                Context = new GenericRepository();

                Excursions = new ObservableCollection<Excursion>(MainViewModel.BasicDataManager.Excursions.OrderBy(e => e.ExcursionDates.OrderBy(ed => ed.CheckIn).FirstOrDefault().CheckIn));
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
            throw new NotImplementedException();
        }

        internal async Task ShowDepartureInfo()
        {
            Context = new GenericRepository();
            Mouse.OverrideCursor = Cursors.Wait;
            Bookings = (await Context.GetAllBookingInPeriod(FromDepartureInfo, ToDepartureInfo, excursionId: SelectedFilterExcursion.Id)).ToList();
            //foreach (var b in Bookings)
            //{
            //    if (b.ReservationsInBooking.Count > 0)
            //    {
            //        ReservationWrapper res0 = new ReservationWrapper(b.ReservationsInBooking[0]);
            //        DateTime min = res0.CheckIn;
            //        DateTime max = res0.CheckOut;
            //        if (b.ReservationsInBooking.Count > 1)
            //        {
            //            for (int i = 1; i < b.ReservationsInBooking.Count; i++)
            //            {
            //                res0 = new ReservationWrapper(b.ReservationsInBooking[i]);
            //                foreach (var c in res0.CustomersList)
            //                {
            //                    if (c.CheckIn < min)
            //                    {
            //                        min = c.CheckIn;
            //                    }
            //                    if (c.CheckIn > max)
            //                    {
            //                        max = c.CheckOut;
            //                    }
            //                }
            //            }
            //        }
            //        if (min.Year > 2000 && max.Year > 2000)
            //        {
            //            if (b.CheckIn != min || b.CheckOut != max)
            //            {
            //                b.CheckIn = min;
            //                b.CheckOut = max;
            //            }
            //        }
            //        else
            //        {
            //        }
            //    }
            //}
            DateTime tmpDate = FromDepartureInfo;
            AllDaysDeparturesList.Clear();
            DailyDepartureInfo dayDeparture = new DailyDepartureInfo(Context, SelectedFilterExcursion.Id);
            DailyDepartureInfo tmpDayDeparture = new DailyDepartureInfo(Context, SelectedFilterExcursion.Id);
            CityDepartureInfo cityDepartureInfo;
            if (SelectedFilterExcursion.ExcursionType.Category == Model.ExcursionTypeEnum.Group)
            {
                foreach (ExcursionDate datePair in SelectedFilterExcursion.ExcursionDates)
                {
                    dayDeparture = new DailyDepartureInfo(Context, SelectedFilterExcursion.Id) { ExcursionDate = datePair, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };

                    foreach (Booking b in Bookings)
                    {
                        if (b.ExcursionDate.Id == datePair.Id)
                        {
                            foreach (var r in b.ReservationsInBooking)
                                foreach (Customer customer in r.CustomersList)
                                {
                                    if (!b.DifferentDates || (customer.CheckIn == b.ExcursionDate.CheckIn && customer.CheckOut == b.ExcursionDate.CheckOut))
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
                                            tmpDayDeparture = new DailyDepartureInfo(Context, SelectedFilterExcursion.Id) { Date = customer.CheckIn, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };
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
                                            tmpDayDeparture = new DailyDepartureInfo(Context, SelectedFilterExcursion.Id) { Date = customer.CheckOut, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };
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
                    dayDeparture = new DailyDepartureInfo(Context, SelectedFilterExcursion.Id) { Date = tmpDate, PerCityDepartureList = new ObservableCollection<CityDepartureInfo>() };

                    foreach (Booking b in Bookings)
                    {
                        if (b.CheckIn == tmpDate)
                        {
                            foreach (var r in b.ReservationsInBooking)
                                foreach (Customer customer in r.CustomersList)
                                {
                                    cityDepartureInfo = dayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                    if (cityDepartureInfo == null)
                                    {
                                        dayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace });
                                        cityDepartureInfo = dayDeparture.PerCityDepartureList[dayDeparture.PerCityDepartureList.Count - 1];
                                    }
                                    if (!r.OnlyStay && ((SelectedFilterExcursion.IncludesShip && customer.CustomerHasShipIndex < 2) || (SelectedFilterExcursion.IncludesBus && customer.CustomerHasBusIndex < 2)))
                                    {
                                        if (SelectedFilterExcursion.IncludesShip && SelectedFilterExcursion.IncludesBus && customer.CustomerHasShipIndex > 1)
                                        {
                                            cityDepartureInfo.OnlyBusGo++;
                                        }
                                        else if (SelectedFilterExcursion.IncludesShip && SelectedFilterExcursion.IncludesBus && customer.CustomerHasBusIndex > 1)
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
                        if (b.CheckOut == tmpDate)
                        {
                            foreach (var r in b.ReservationsInBooking)
                                foreach (Customer customer in r.CustomersList)
                                {
                                    cityDepartureInfo = dayDeparture.PerCityDepartureList.FirstOrDefault(p => p.City == customer.StartingPlace);
                                    if (cityDepartureInfo == null)
                                    {
                                        dayDeparture.PerCityDepartureList.Add(new CityDepartureInfo { City = customer.StartingPlace });
                                        cityDepartureInfo = dayDeparture.PerCityDepartureList[dayDeparture.PerCityDepartureList.Count - 1];
                                    }
                                    if (!r.OnlyStay && ((SelectedFilterExcursion.IncludesShip && (customer.CustomerHasShipIndex == 0 || customer.CustomerHasShipIndex == 2)) || (SelectedFilterExcursion.IncludesBus && (customer.CustomerHasBusIndex == 0 || customer.CustomerHasBusIndex == 2))))
                                    {
                                        if (SelectedFilterExcursion.IncludesShip && SelectedFilterExcursion.IncludesBus && (customer.CustomerHasShipIndex == 1 || customer.CustomerHasShipIndex == 3))
                                        {
                                            cityDepartureInfo.OnlyBusReturn++;
                                        }
                                        else if (SelectedFilterExcursion.IncludesShip && SelectedFilterExcursion.IncludesBus && (customer.CustomerHasBusIndex == 1 || customer.CustomerHasBusIndex == 3))
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
            AllDaysDeparturesCollectionView = CollectionViewSource.GetDefaultView(AllDaysDeparturesList);
            AllDaysDeparturesCollectionView.SortDescriptions.Add(new SortDescription(nameof(DailyDepartureInfo.Date), ListSortDirection.Ascending));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        //private bool CanShowDepartureInfoM()
        //{
        //    return SelectedFilterExcursion != null;
        //}

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

        private bool InfoExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return excursion.Id > 0 && (Completed || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today));
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