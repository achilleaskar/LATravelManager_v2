using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Pricing;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Views.Management;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Management
{
    public class ExcursionsManagement_ViewModel : AddEditBase<ExcursionWrapper, Excursion>
    {
        #region Constructors

        public ExcursionsManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Εκδρομών";
            OpenCitiesEditCommand = new RelayCommand(OpenCitiesWindow, CanEditWindows);
            RemoveDateCommand = new RelayCommand(RemoveDate, CanRemoveDate);
            RemoveCityCommand = new RelayCommand(RemoveCity, CanRemoveCity);
            AddCityCommand = new RelayCommand(AddCity, CanAddCity);

            AddPeriodCommand = new RelayCommand(AddPeriod, CanAddPeriod);
            ClearPeriodCommand = new RelayCommand(CleanPeriod, CanCleanPeriod);
            SavePeriodChangesCommand = new RelayCommand(SavePeriodChanges, CanSavePeriodChanges);
            RemovePeriodCommand = new RelayCommand(RemovePeriod, CanRemovePeriod);
            UpdatePricesCommand = new RelayCommand(async () => { await ReloadPrices(); }, true);

            SavePriceChangesChangesCommand = new RelayCommand(SavePriceChanges, BasicDataManager.HasChanges());
            AddNewHotelsCommand = new RelayCommand(async () => await AddNewHotels(), SelectedEntity != null && SelectedPeriodP != null);

            AddDateCommand = new RelayCommand(AddDate, CanAddDate);
            AddTimesCommand = new RelayCommand(AddTimes, CanAddTimes);
            AddHalfHourCommand = new RelayCommand<int>(AddHalfHour);
            SelectedPeriod = new PricingPeriod();
        }

        private async Task AddNewHotels()
        {
            var hotels = await BasicDataManager.Context.GetAllHotelsWithRooms(SelectedEntity.Destinations[0].Id, SelectedPeriodP);
            foreach (var hotel in hotels)
            {
                if (!SelectedPeriodP.HotelPricings.Any(h => h.Hotel.Id == hotel.Id))
                {
                    SelectedPeriodP.HotelPricings.Add(new HotelPricing { Hotel = hotel });
                }
            }
            UpdateHotels();
        }

        private async void SavePriceChanges()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                await BasicDataManager.SaveAsync();
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

        private bool CanSavePriceChanges()
        {
            throw new NotImplementedException();
        }

        private async Task ReloadPrices()
        {
            await ReloadEntities();
            await BasicDataManager.GetAllPrices();
        }

        private bool CanRemovePeriod()
        {
            return SelectedPeriod != null && SelectedPeriod.Id > 0;
        }

        private PricingPeriod _SelectedPeriodP;

        public PricingPeriod SelectedPeriodP
        {
            get
            {
                return _SelectedPeriodP;
            }

            set
            {
                if (_SelectedPeriodP == value)
                {
                    return;
                }

                _SelectedPeriodP = value;
                UpdateHotels();
                RaisePropertyChanged();
            }
        }

        private void UpdateHotels()
        {
            if (SelectedPeriodP != null)
            {
                HotelPricings = new ObservableCollection<HotelPricing>(SelectedPeriodP.HotelPricings);
                if (!HotelPricings.Contains(SelectedPeriodP.ChilndEtcPrices.Transfer))
                {
                    HotelPricings.Insert(0, SelectedPeriodP.ChilndEtcPrices.Transfer);
                }
            }
        }

        private ObservableCollection<HotelPricing> _HotelPricings;

        public ObservableCollection<HotelPricing> HotelPricings
        {
            get
            {
                return _HotelPricings;
            }

            set
            {
                if (_HotelPricings == value)
                {
                    return;
                }

                _HotelPricings = value;
                RaisePropertyChanged();
            }
        }

        private void RemovePeriod()
        {
            try
            {
                SelectedEntity.Periods.Remove(SelectedPeriod);
                SelectedPeriod = new PricingPeriod();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private bool CanSavePeriodChanges()
        {
            return BasicDataManager.HasChanges() && SelectedPeriod != null && SelectedPeriodIsOk();
        }

        private async void SavePeriodChanges()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                await BasicDataManager.SaveAsync();
                PeriodResultMessage = "Όι αλλαγές απόθηκεύτηκαν επιτυχώς";
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

        private bool CanCleanPeriod()
        {
            return SelectedPeriod != null;
        }

        private void CleanPeriod()
        {
            SelectedPeriod = new PricingPeriod();
            PeriodResultMessage = "";
        }

        private bool CanAddPeriod()
        {
            return SelectedPeriod != null && SelectedPeriodIsOk() && SelectedPeriod.Id == 0;
        }

        public bool SelectedPeriodIsOk()
        {
            return SelectedPeriod != null && SelectedPeriod.To > SelectedPeriod.From &&
                (!SelectedPeriod.Parted ||
                (SelectedPeriod.FromB > SelectedPeriod.To && SelectedPeriod.ToB > SelectedPeriod.FromB));
        }

        private string _PeriodResultMessage;

        public string PeriodResultMessage
        {
            get
            {
                return _PeriodResultMessage;
            }

            set
            {
                if (_PeriodResultMessage == value)
                {
                    return;
                }

                _PeriodResultMessage = value;
                RaisePropertyChanged();
            }
        }

        private void AddPeriod()
        {
            try
            {
                SelectedEntity.Periods.Add(SelectedPeriod);
                PeriodResultMessage = "Η περίοδος προστέθηκε επιτυχώς";
                RemovePeriodCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private void AddHalfHour(int obj)
        {
            if (SelectedEntity != null && SelectedEntity.Destinations != null && SelectedEntity.Destinations.Count > 0 &&
                SelectedEntity.Destinations[0].ExcursionTimes != null && SelectedEntity.Destinations[0].ExcursionTimes.Count > 0)
            {
                DateTime dt;
                int m = 30 * obj;

                foreach (var t in SelectedEntity.Destinations[0].ExcursionTimes)
                {
                    dt = DateTime.Today.AddTicks(t.Time.Ticks).AddMinutes(m);
                    t.Time = dt.TimeOfDay;
                }
            }
        }

        private bool CanAddDate()
        {
            if (SelectedDate == null)
            {
                SelectedDate = new ExcursionDate();
            }
            return SelectedDate.CheckIn >= DateTime.Today && SelectedDate.CheckOut >= SelectedDate.CheckIn && SelectedDate.Name.Length > 3;
        }

        private void AddTimes()
        {
            if (SelectedEntity.Destinations != null && SelectedEntity.Destinations.Count > 0)
            {
                foreach (var s in StaticResources.StartingPlaces)
                {
                    if (!SelectedEntity.Destinations[0].ExcursionTimes.Any(t => t.StartingPlace.Id == s.Id))
                    {
                        SelectedEntity.Destinations[0].ExcursionTimes.Add(new ExcursionTime { StartingPlace = s });
                    }
                }
                RaisePropertyChanged(nameof(SelectedEntity));
            }
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<City> _Cities;

        private ObservableCollection<ExcursionCategory> _ExcursionCategories;

        private City _SelectedCity;

        private City _SelectedCityToAdd;

        private ExcursionDate _SelectedDate = new ExcursionDate();

        #endregion Fields

        #region Properties

        public RelayCommand AddPeriodCommand { get; set; }
        public RelayCommand UpdatePricesCommand { get; set; }
        public RelayCommand SavePeriodChangesCommand { get; set; }
        public RelayCommand AddNewHotelsCommand { get; set; }
        public RelayCommand SavePriceChangesChangesCommand { get; set; }
        public RelayCommand ClearPeriodCommand { get; set; }
        public RelayCommand RemovePeriodCommand { get; set; }

        private PricingPeriod _SelectedPeriod;

        public PricingPeriod SelectedPeriod
        {
            get
            {
                return _SelectedPeriod;
            }

            set
            {
                if (_SelectedPeriod == value)
                {
                    return;
                }

                _SelectedPeriod = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Adds the selected city to the excursion Destinations
        /// </summary>
        public RelayCommand AddCityCommand { get; }

        public RelayCommand AddDateCommand { get; }
        public RelayCommand AddTimesCommand { get; }
        public RelayCommand<int> AddHalfHourCommand { get; }

        /// <summary>
        /// Sets and gets the Cities property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<City> Cities
        {
            get
            {
                return _Cities;
            }

            set
            {
                if (_Cities == value)
                {
                    return;
                }

                _Cities = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the ExcursionCategories property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public ObservableCollection<ExcursionCategory> ExcursionCategories
        {
            get
            {
                return _ExcursionCategories;
            }

            set
            {
                if (_ExcursionCategories == value)
                {
                    return;
                }

                _ExcursionCategories = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenCitiesEditCommand { get; }

        /// <summary>
        /// Removes the selected city from the excursion Destinations
        /// </summary>
        public RelayCommand RemoveCityCommand { get; }

        public RelayCommand RemoveDateCommand { get; }

        /// <summary>
        /// Sets and gets the SelectedCity property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public City SelectedCity
        {
            get
            {
                return _SelectedCity;
            }

            set
            {
                if (_SelectedCity == value)
                {
                    return;
                }

                _SelectedCity = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the SelectedCityToAdd property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public City SelectedCityToAdd
        {
            get
            {
                return _SelectedCityToAdd;
            }

            set
            {
                if (_SelectedCityToAdd == value)
                {
                    return;
                }

                _SelectedCityToAdd = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the SelectedDate property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ExcursionDate SelectedDate
        {
            get
            {
                return _SelectedDate;
            }

            set
            {
                if (_SelectedDate == value)
                {
                    return;
                }
                _SelectedDate = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public bool ShowFinished { get; set; }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                MainCollection = new ObservableCollection<ExcursionWrapper>(BasicDataManager.Excursions.Where(c => ShowFinished || c.ExcursionDates.Any(d => d.Id > 0 && d.CheckOut >= DateTime.Today)).Select(c => new ExcursionWrapper(c)));

                Cities = BasicDataManager.Cities;
                ExcursionCategories = BasicDataManager.ExcursionCategories;

                //foreach (var e in MainCollection)
                //{
                //    foreach (var s in StaticResources.StartingPlaces)
                //    {
                //        if (!e.ExcursionTimes.Any(t => t.StartingPlace.Id == s.Id))
                //        {
                //            e.ExcursionTimes.Add(new ExcursionTime { StartingPlace = BasicDataManager.Context.GetById<StartingPlace>(s.Id) });
                //        }
                //    }
                //}
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

        public void OpenCitiesWindow()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                CitiesManagement_ViewModel vm = new CitiesManagement_ViewModel(BasicDataManager);
                vm.ReLoad();
                MessengerInstance.Send(new OpenChildWindowCommand(new CitiesManagement_Window { DataContext = vm }));
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

        private void AddCity()
        {
            SelectedEntity.Destinations.Add(SelectedCityToAdd);
            SelectedEntity.ValidateAllProperties();
        }

        private void AddDate()
        {
            SelectedEntity.ExcursionDates.Add(SelectedDate);
            SelectedDate = new ExcursionDate();
            SelectedEntity.ValidateAllProperties();
        }

        private bool CanAddCity()
        {
            return _SelectedCityToAdd != null;
        }

        private bool CanAddTimes()
        {
            return SelectedEntity != null;
        }

        private bool CanEditWindows()
        {
            return true;
        }

        private bool CanRemoveCity()
        {
            return SelectedCity != null;
        }

        private bool CanRemoveDate()
        {
            if (SelectedDate != null && SelectedEntity != null)
            {
                return SelectedEntity.ExcursionDates.Contains(SelectedDate);
            }
            return false;
        }

        private void RemoveCity()
        {
            SelectedEntity.Destinations.Remove(SelectedCity);
            SelectedEntity.ValidateAllProperties();
        }

        private void RemoveDate()
        {
            try
            {
                SelectedEntity.ExcursionDates.Remove(SelectedDate);
                SelectedDate = new ExcursionDate();
                SelectedEntity.ValidateAllProperties();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        #endregion Methods
    }
}