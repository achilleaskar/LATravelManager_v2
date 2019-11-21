using GalaSoft.MvvmLight.CommandWpf;
using LaTravelManager.ViewModel.Management;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Services;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Management;
using LATravelManager.UI.ViewModel.Parents;
using LATravelManager.UI.Views;
using LATravelManager.UI.Views.Management;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class MainUserControl_ViewModel : MyViewModelBaseAsync
    {
        #region Fields

        private bool _HasNots;

        private bool _IsBusy = false;

        private NavigationViewModel _NavigationViewModel;

        private List<string> _Nots;

        private ExcursionCategory_ViewModelBase _SelectedExcursionType;

        private int _SelectedTemplateIndex;

        private ObservableCollection<ExcursionCategory> _Templates;

        #endregion Fields

        #region Constructors

        public MainUserControl_ViewModel(MainViewModel mainViewModel)
        {
            LogOutCommand = new RelayCommand(async () => { await TryLogOut(); });

            OpenHotelEditCommand = new RelayCommand(OpenHotelsWindow, CanEditWindows);
            OpenCitiesEditCommand = new RelayCommand(OpenCitiesWindow, CanEditWindows);
            OpenCountriesEditCommand = new RelayCommand(OpenCountriesWindow, CanEditWindows);
            OpenExcursionsEditCommand = new RelayCommand(OpenExcursionsWindow, CanEditWindows);
            OpenUsersEditCommand = new RelayCommand(OpenUsersWindow, CanEditWindows);
            OpenPartnersEditCommand = new RelayCommand(OpenPartnersWindow, CanEditWindows);
            OpenLeadersEditCommand = new RelayCommand(OpenLeadersWindow, CanEditWindows);
            OpenVehiclesEditCommand = new RelayCommand(OpenVehiclesWindow, CanEditWindows);

            TemplateViewmodels = new List<ExcursionCategory_ViewModelBase>();

            MessengerInstance.Register<IsBusyChangedMessage>(this, msg => { ManageIsBusy(msg.IsBusy); });

            SelectedTemplateChangedCommand = new RelayCommand(SetProperViewModel);

            MainViewModel = mainViewModel;
            var x = GetaAllNotifications().ConfigureAwait(false);

            // MessengerInstance.Register<ChangeChildViewModelMessage>(this, async vm => { await SelectedExcursionType.SetProperChildViewModel(vm.ViewModelindex); });

            //  MessengerInstance.Register<ExcursionCategoryChangedMessage>(this, async index => { await SetProperViewModel(); });
        }

        #endregion Constructors

        #region Properties

        public bool HasNots
        {
            get
            {
                return _HasNots;
            }

            set
            {
                if (_HasNots == value)
                {
                    return;
                }

                _HasNots = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }

            set
            {
                if (_IsBusy == value)
                {
                    return;
                }

                _IsBusy = value;
                Application.Current.Dispatcher.Invoke(delegate
                {
                    if (value)
                        Mouse.OverrideCursor = Cursors.Wait;
                    else
                        Mouse.OverrideCursor = Cursors.Arrow;
                });
                RaisePropertyChanged();
            }
        }

        public int IsBusyCounter { get; set; }

        public RelayCommand LogOutCommand { get; set; }

        public MainViewModel MainViewModel { get; }

        public NavigationViewModel NavigationViewModel
        {
            get
            {
                return _NavigationViewModel;
            }

            set
            {
                if (_NavigationViewModel == value)
                {
                    return;
                }

                _NavigationViewModel = value;
                RaisePropertyChanged();
            }
        }

        public List<string> Nots
        {
            get
            {
                return _Nots;
            }

            set
            {
                if (_Nots == value)
                {
                    return;
                }

                _Nots = value;
                HasNots = Nots.Count > 0;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenCitiesEditCommand { get; }

        public RelayCommand OpenCountriesEditCommand { get; }

        public RelayCommand OpenExcursionsEditCommand { get; }

        public RelayCommand OpenHotelEditCommand { get; }

        public RelayCommand OpenLeadersEditCommand { get; }

        public RelayCommand OpenPartnersEditCommand { get; }

        public RelayCommand OpenUsersEditCommand { get; set; }

        public RelayCommand OpenVehiclesEditCommand { get; }

        public ExcursionCategory_ViewModelBase SelectedExcursionType
        {
            get
            {
                return _SelectedExcursionType;
            }

            set
            {
                if (_SelectedExcursionType == value)
                {
                    return;
                }

                _SelectedExcursionType = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SelectedTemplateChangedCommand { get; set; }

        public int SelectedTemplateIndex
        {
            get
            {
                return _SelectedTemplateIndex;
            }

            set
            {
                if (_SelectedTemplateIndex == value)
                {
                    return;
                }

                _SelectedTemplateIndex = value;
                //if (value >= 0 && value <= Templates.Count)
                //{
                //    MessengerInstance.Send(new ExcursionCategoryChangedMessage());
                //}
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ExcursionCategory> Templates
        {
            get
            {
                return _Templates;
            }

            set
            {
                if (_Templates == value)
                {
                    return;
                }

                _Templates = value;
                RaisePropertyChanged();
            }
        }

        public List<ExcursionCategory_ViewModelBase> TemplateViewmodels { get; set; }

        public string UserName => StaticResources.User != null ?
            $"{StaticResources.User.Name} {((!string.IsNullOrEmpty(StaticResources.User.Surename) && StaticResources.User.Surename.Length > 4) ? StaticResources.User.Surename.Substring(0, 5) : "")}"
            : "Error";

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            NavigationViewModel = new NavigationViewModel(this, MainViewModel);
            Templates = MainViewModel.BasicDataManager.ExcursionCategories;

            await Task.Delay(0);
            // await SetProperViewModel();
        }

        public async Task<List<string>> LoadOptions(GenericRepository repository)
        {
            List<Option> options = await repository.GetAllPendingOptions();
            List<string> reply = new List<string>();

            if (options.Count > 0)
            {
                HotelOptions HotelOptions;
                List<HotelOptions> hotelOptionsLis = new List<HotelOptions>();
                foreach (Option option in options)
                {
                    if (option.Room?.Hotel != null)
                    {
                        HotelOptions = hotelOptionsLis.Where(ho => ho.Hotel == option.Room.Hotel && ho.Date == option.Date).FirstOrDefault();
                        if (HotelOptions != null)
                        {
                            HotelOptions.Counter++;
                        }
                        else
                        {
                            hotelOptionsLis.Add(new HotelOptions { Counter = 1, Date = option.Date, Hotel = option.Room.Hotel });
                        }
                    }
                    else
                    {
                        MessageBox.Show("Option Hotel is null error. Inform the Admin");
                    }
                }
                foreach (var hotel in hotelOptionsLis)
                {
                    reply.Add($"Οι Option για { hotel.Counter} δωμάτια στο { hotel.Hotel.Name} λήγουν στις {hotel.Date.ToShortDateString()}");
                }
            }
            return reply;
        }

        public void OpenCitiesWindow()
        {
            CitiesManagement_ViewModel vm = new CitiesManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new CitiesManagement_Window { DataContext = vm }));
        }

        public void OpenCountriesWindow()
        {
            CountriesManagement_ViewModel vm = new CountriesManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new CountriesManagement_Window { DataContext = vm }));
        }

        public void OpenHotelsWindow()
        {
            HotelsManagement_ViewModel vm = new HotelsManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new HotelsManagement_Window { DataContext = vm }));
        }

        public override async Task ReloadAsync()
        {
            await Task.Delay(0);
        }

        public async Task TryLogOut()
        {
            StaticResources.User = null;
            await MainViewModel.ChangeViewModel();
        }

        private bool CanEditWindows()
        {
            return true;
            // return MainViewModel. StartingRepository.IsContextAvailable;
        }

        private async Task GetaAllNotifications()
        {
            var notificationManager = new NotificationManager();

            var Repository = new GenericRepository();
            List<string> nots = new List<string>();

            nots.AddRange(await LoadOptions(Repository));
            //nots.AddRange(await Repository.GetNotifications());
            nots.AddRange(await GetPersonalOptions(Repository));
            nots.AddRange(await GetPersonalCheckIns(Repository));
            nots.AddRange(await GetNonPayersGroup(Repository));
            nots.AddRange(await GetNonPayersPersonal(Repository));
            nots.AddRange(await GetNonPayersThirdParty(Repository));
            Repository.Dispose();

            foreach (var not in nots)
            {
                notificationManager.Show(new NotificationContent
                {
                    Message = not,
                    Type = NotificationType.Warning
                });
            }
            Nots = nots;
        }

        private async Task<IEnumerable<string>> GetNonPayersGroup(GenericRepository repository)
        {
            List<BookingWrapper> options = (await repository.GetAllNonPayersGroup()).Select(b => new BookingWrapper(b)).ToList();
            List<string> reply = new List<string>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if (((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || (booking.CheckIn - DateTime.Today).TotalDays <= 5) && booking.FullPrice>booking.Customers.Count&& booking.Recieved < 1)
                    {
                        reply.Add($"Η κράτηση για { booking.Excursion.Destinations[0]}  " +
                            $"στο όνομα { booking.ReservationsInBooking[0].CustomersList[0] } δέν έχει δώσει προκαταβολή");
                    }
                    else if ((booking.CheckIn - DateTime.Today).TotalDays <= 5 && booking.Remaining > 1)
                    {
                        reply.Add($"Η κράτηση για { booking.Excursion.Destinations[0]}  " +
                            $"στο όνομα { booking.ReservationsInBooking[0].CustomersList[0] } δέν έχει κάνει εξόφληση");
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<string>> GetNonPayersPersonal(GenericRepository repository)
        {
            List<Personal_BookingWrapper> options = (await repository.GetAllNonPayersPersonal()).Select(b => new Personal_BookingWrapper(b)).ToList();
            List<string> reply = new List<string>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if ((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || booking.Services.Any(a => (a.TimeGo - DateTime.Today).TotalDays <= 5) && booking.FullPrice > booking.Customers.Count && booking.Recieved < 1)
                    {
                        reply.Add($"Το ατομικό πακέτο στο όνομα { booking.Customers[0] } δέν έχει δώσει προκαταβολή");
                    }
                    else if (booking.Services.Any(a => (a.TimeGo - DateTime.Today).TotalDays <= 5) && booking.Remaining > 1)
                    {
                        reply.Add($"Το ατομικό πακέτο στο όνομα { booking.Customers[0] } δέν έχει δώσει προκαταβολή");
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<string>> GetNonPayersThirdParty(GenericRepository repository)
        {
            List<ThirdParty_Booking_Wrapper> options = (await repository.GetAllNonPayersThirdparty()).Select(b => new ThirdParty_Booking_Wrapper(b)).ToList();
            List<string> reply = new List<string>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if (((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || (booking.CheckIn - DateTime.Today).TotalDays <= 5) && booking.FullPrice > booking.Customers.Count && booking.Recieved < 1)
                    {
                        reply.Add($"Το πακέτο συνεργάτη για { booking.City} " +
                            $"στο όνομα { booking.Customers[0]} δέν έχει δώσει προκαταβολή");
                    }
                    else if ((booking.CheckIn - DateTime.Today).TotalDays <= 5 && booking.Remaining > 1)
                    {
                        reply.Add($"Το πακέτο συνεργάτη για { booking.City} " +
                            $"στο όνομα { booking.Customers[0]} δέν έχει κάνει εξόφληση");
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<string>> GetPersonalCheckIns(GenericRepository repository)
        {
            List<PlaneService> options = await repository.GetAllPlaneOptions();
            List<string> reply = new List<string>();

            if (options.Count > 0)
            {
                foreach (var option in options)
                {
                    if (option.Personal_Booking == null)
                    {
                        continue;
                    }
                    if (option.Airline != null)
                    {
                        if (option.Airline.Checkin != 0 && option.TimeGo > DateTime.Now &&
                            (option.TimeGo - DateTime.Now).TotalHours <= option.Airline.Checkin)
                        {
                            reply.Add($"Ανοιξε το CheckIn {option.From}-{option.To} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}");
                        }
                        if (option.Airline.Checkin != 0 && option.TimeReturn > DateTime.Now && option.Allerretour &&
                            (option.TimeReturn - DateTime.Now).TotalHours <= option.Airline.Checkin)
                        {
                            reply.Add($"Ανοιξε το το CheckIn {option.To}-{option.From} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}");
                        }
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<string>> GetPersonalOptions(GenericRepository repository)
        {
            List<HotelService> options = await repository.GetAllPersonalOptions();
            List<string> reply = new List<string>();

            if (options.Count > 0)
            {
                foreach (var option in options)
                {
                    if (option.Personal_Booking == null)
                    {
                        continue;
                    }
                    reply.Add($"Η Option για το ατομικό {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())} στο {(option.Hotel != null ? option.Hotel.Name : "Αγνωστο Ξενοδοχείο")} λήγει στις {option.Option.ToShortDateString()}");
                }
            }
            return reply;
        }

        private void ManageIsBusy(bool add)
        {
            _ = add ? IsBusyCounter++ : IsBusyCounter--;
            IsBusy = IsBusyCounter > 0;
        }

        private void OpenExcursionsWindow()
        {
            ExcursionsManagement_ViewModel vm = new ExcursionsManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new ExcursionsManagement_Window { DataContext = vm }));
        }

        private void OpenLeadersWindow()
        {
            LeadersManagement_ViewModel vm = new LeadersManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new LeadersManagement_Widnow { DataContext = vm }));
        }

        private void OpenPartnersWindow()
        {
            PartnerManagement_ViewModel vm = new PartnerManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new PartnersManagement_Window { DataContext = vm }));
        }

        private void OpenUsersWindow()
        {
            UsersManagement_viewModel vm = new UsersManagement_viewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new UsersManagement_Window { DataContext = vm }));
        }

        private void OpenVehiclesWindow()
        {
            VehiclesManagement_viewModel vm = new VehiclesManagement_viewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new BusesManagement_Window { DataContext = vm }));
        }

        private void SetProperViewModel()
        {
            if (SelectedTemplateIndex < Templates.Count)
            {
                int index = -1;
                switch (Templates[SelectedTemplateIndex].Category)
                {
                    case Model.ExcursionTypeEnum.Bansko:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(BanskoParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new BanskoParent_ViewModel(MainViewModel);
                        }
                        break;

                    case Model.ExcursionTypeEnum.Group:

                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(GroupParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new GroupParent_ViewModel(MainViewModel);
                        }
                        break;

                    case Model.ExcursionTypeEnum.Personal:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(PersonalParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new PersonalParent_ViewModel(MainViewModel);
                        }
                        break;

                    case Model.ExcursionTypeEnum.ThirdParty:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(ThirdPartyParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new ThirdPartyParent_ViewModel(MainViewModel);
                        }

                        break;

                    case Model.ExcursionTypeEnum.Skiathos:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(SkiathosParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new SkiathosParent_ViewModel(MainViewModel);
                        }

                        break;
                }
                if (index < 0)
                {
                    TemplateViewmodels.Add(SelectedExcursionType);
                }
                //set to default tab
                if (SelectedExcursionType is ExcursionCategory_ViewModelBase)
                {
                    if (index < 0)
                        SelectedExcursionType.Load();
                    NavigationViewModel.SetTabs();
                }
            }
            else
            {
                throw new Exception("SelectedTemplateIndex greater than Templates count");
            }
        }

        #endregion Methods
    }
}