using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
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
    public class Info_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public Info_ViewModel(Window_ViewModels.MainViewModel mainViewModel)
        {
            ToDepartureInfo = /*(Properties.Settings.Default.toDeparture >= DateTime.Today) ? Properties.Settings.Default.toDeparture :*/ DateTime.Today;
            FromDepartureInfo = /*(Properties.Settings.Default.fromDepartures > DateTime.Today) ? Properties.Settings.Default.fromDepartures :*/ DateTime.Today;
            ShowDepartureInfoCommand = new RelayCommand(async () => { await ShowDepartureInfo(); });
            //SaveTransfersCommand = new SaveTransfersCommand(this);
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
                string tmpError = "";
                foreach (string property in ValidateDepartureInfoProperties)
                {
                    tmpError = GetDepartureInfoError(property);
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
                RaisePropertyChanged();
            }
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

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
           

            try
            {
                Context = new GenericRepository();

                Excursions = new ObservableCollection<Excursion>((await Context.GetAllAsync<Excursion>()).OrderBy(e => e.ExcursionDates.OrderBy(ed => ed.CheckIn).FirstOrDefault().CheckIn));

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
            Mouse.OverrideCursor = Cursors.Wait;
            Bookings = (await Context.GetAllBookingInPeriod(FromDepartureInfo, ToDepartureInfo, SelectedFilterExcursion.Id)).Select(b => new BookingWrapper(b)).ToList();
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