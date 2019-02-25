using GalaSoft.MvvmLight.CommandWpf;
using LaTravelManager.BaseTypes;
using LATravelManager.Models;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Views.Management;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LaTravelManager.ViewModel.Management
{
    public class ExcursionsManagement_ViewModel : AddEditBase<ExcursionWrapper, Excursion>
    {
        #region Constructors

        public ExcursionsManagement_ViewModel(GenericRepository context) : base(context)
        {
            ControlName = "Διαχείριση Εκδρομών";
            OpenCitiesEditCommand = new RelayCommand(async () => { await OpenCitiesWindow(); }, CanEditWindows);
            RemoveDateCommand = new RelayCommand(RemoveDate, CanRemoveDate);
            RemoveCityCommand = new RelayCommand(RemoveCity, CanRemoveCity);
            AddCityCommand = new RelayCommand(AddCity, CanAddCity);
            AddDateCommand = new RelayCommand(AddDate, CanAddDate);
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

        /// <summary>
        /// Adds the selected city to the excursion Destinations
        /// </summary>
        public RelayCommand AddCityCommand { get; }

        public RelayCommand AddDateCommand { get; }

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

        public override async Task LoadAsync(int id = 0)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                MainCollection = new ObservableCollection<ExcursionWrapper>((await Context.GetAllUpcomingGroupExcursionsAsync()).OrderBy(ex => ex, new ExcursionComparer()).Select(c => new ExcursionWrapper(c)));
                Cities = new ObservableCollection<City>(await Context.GetAllCitiesAsyncSortedByName());
                ExcursionCategories = new ObservableCollection<ExcursionCategory>(await Context.GetAllAsync<ExcursionCategory>());
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

        public async Task OpenCitiesWindow()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                var vm = new CitiesManagement_ViewModel(Context);
                await vm.LoadAsync();
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

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
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

        private bool CanAddDate()
        {
            if (SelectedDate == null)
            {
                SelectedDate = new ExcursionDate();
            }
            return SelectedDate.CheckIn >= DateTime.Today && SelectedDate.CheckOut >= SelectedDate.CheckIn && SelectedDate.Name.Length > 3;
        }

        private bool CanEditWindows()
        {
            return Context.IsContextAvailable;
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