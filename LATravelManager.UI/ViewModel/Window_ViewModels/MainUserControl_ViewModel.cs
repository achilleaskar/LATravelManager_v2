using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using LaTravelManager.ViewModel.Management;
using LATravelManager.Model;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Notifications;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using LATravelManager.UI.ViewModel.Management;
using LATravelManager.UI.ViewModel.Parents;
using LATravelManager.UI.Views;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Views.Management;
using LATravelManager.UI.Views.Personal;
using LATravelManager.UI.Views.ThirdParty;
using Notifications.Wpf;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class MainUserControl_ViewModel : MyViewModelBaseAsync
    {
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
            OpenOptionalsEditCommand = new RelayCommand(OpenOpionalsWindow, CanEditWindows);

            NotIsOkCommand = new RelayCommand(async () => { await NotIsOk(); });

            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);

            TemplateViewmodels = new List<ExcursionCategory_ViewModelBase>();

            MessengerInstance.Register<IsBusyChangedMessage>(this, msg => { ManageIsBusy(msg.IsBusy); });

            SelectedTemplateChangedCommand = new RelayCommand(SetProperViewModel);

            MainViewModel = mainViewModel;
            var x = GetaAllNotifications().ConfigureAwait(false);

            // MessengerInstance.Register<ChangeChildViewModelMessage>(this, async vm => { await SelectedExcursionType.SetProperChildViewModel(vm.ViewModelindex); });

            //  MessengerInstance.Register<ExcursionCategoryChangedMessage>(this, async index => { await SetProperViewModel(); });
        }

        private Notification _SelectedNot;

        public Notification SelectedNot
        {
            get
            {
                return _SelectedNot;
            }

            set
            {
                if (_SelectedNot == value)
                {
                    return;
                }

                _SelectedNot = value;
                RaisePropertyChanged();
            }
        }

        private async Task NotIsOk()
        {
            if (SelectedNot != null)
            {
                if (SelectedNot.NotificaationType == NotificaationType.Option && SelectedNot.HotelOptions != null)
                {
                    foreach (var o in SelectedNot.HotelOptions.Options)
                    {
                        o.NotifStatus = new NotifStatus { IsOk = true, OkByUser = NotsRepository.GetById<User>(StaticResources.User.Id), OkDate = DateTime.Now };
                    }
                    Nots.Remove(SelectedNot);
                }
                else if (SelectedNot.NotificaationType == NotificaationType.CheckIn && SelectedNot.Service != null && SelectedNot.Service is PlaneService ps)
                {
                    ps.NotifStatus = new NotifStatus { IsOk = true, OkByUser = NotsRepository.GetById<User>(StaticResources.User.Id), OkDate = DateTime.Now };
                }
                else if (SelectedNot.NotificaationType == NotificaationType.PersonalOption && SelectedNot.Service != null && SelectedNot.Service is HotelService hs)
                {
                    hs.NotifStatus = new NotifStatus { IsOk = true, OkByUser = NotsRepository.GetById<User>(StaticResources.User.Id), OkDate = DateTime.Now };
                }
            }
            if (NotsRepository.HasChanges())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await NotsRepository.SaveAsync();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OpenOpionalsWindow()
        {
            OptionalExcursions_Management_ViewModel vm = new OptionalExcursions_Management_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new OptionalExcursionsManagement_Window { DataContext = vm }));
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
                else if (SelectedReservation.Booking != null)
                {
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.Booking.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }
                else if (SelectedReservation.BookingWrapper != null)
                {
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.BookingWrapper.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
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

        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        #endregion Constructors

        #region Fields

        private bool _HasNots;

        private bool _IsBusy = false;

        private NavigationViewModel _NavigationViewModel;

        private List<Notification> _Nots;

        private ExcursionCategory_ViewModelBase _SelectedExcursionType;

        private int _SelectedTemplateIndex;

        public RelayCommand EditBookingCommand { get; set; }

        private ObservableCollection<ExcursionCategory> _Templates;

        #endregion Fields

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

        private ICollectionView _NotsCV;
        private ReservationWrapper _SelectedReservation;

        public ICollectionView NotsCV
        {
            get
            {
                return _NotsCV;
            }

            set
            {
                if (_NotsCV == value)
                {
                    return;
                }

                _NotsCV = value;
                RaisePropertyChanged();
            }
        }

        public List<Notification> Nots
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
                NotsCV = CollectionViewSource.GetDefaultView(Nots);
                NotsCV.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Notification.NotificaationType)));
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenCitiesEditCommand { get; }

        public RelayCommand OpenCountriesEditCommand { get; }

        public RelayCommand OpenExcursionsEditCommand { get; }
        public RelayCommand OpenOptionalsEditCommand { get; }

        public RelayCommand OpenHotelEditCommand { get; }
        public RelayCommand NotIsOkCommand { get; }

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

        private GenericRepository _NotsRepository;

        public GenericRepository NotsRepository
        {
            get
            {
                return _NotsRepository;
            }

            set
            {
                if (_NotsRepository == value)
                {
                    return;
                }

                _NotsRepository = value;
                RaisePropertyChanged();
            }
        }

        public async Task<List<Notification>> LoadOptions(GenericRepository repository)
        {
            List<Option> options = await repository.GetAllPendingOptions();
            List<Notification> reply = new List<Notification>();

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
                            HotelOptions.Options.Add(option);
                        }
                        else
                        {
                            hotelOptionsLis.Add(new HotelOptions { Counter = 1, Date = option.Date, Hotel = option.Room.Hotel, Options = new List<Option>() });
                            hotelOptionsLis.Last().Options.Add(option);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Option Hotel is null error. Inform the Admin");
                    }
                }
                foreach (var hotel in hotelOptionsLis)
                {
                    reply.Add(new Notification { Details = $"Οι Option για { hotel.Counter} δωμάτια στο { hotel.Hotel.Name} λήγουν στις {hotel.Date.ToShortDateString()}", NotificaationType = NotificaationType.Option, HotelOptions = hotel });
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

            NotsRepository = new GenericRepository();
            List<Notification> nots = new List<Notification>();

            nots.AddRange(await LoadOptions(NotsRepository));
            nots.AddRange(await GetPersonalOptions(NotsRepository));
            nots.AddRange(await GetPersonalCheckIns(NotsRepository));
            nots.AddRange(await GetNonPayersGroup(NotsRepository));
            nots.AddRange(await GetNonPayersPersonal(NotsRepository));
            nots.AddRange(await GetNonPayersThirdParty(NotsRepository));

            foreach (var not in nots)
            {
                notificationManager.Show(new NotificationContent
                {
                    Message = not.Details,
                    Type = NotificationType.Warning
                });
            }
            Nots = nots;
        }

        private async Task<IEnumerable<Notification>> GetNonPayersGroup(GenericRepository repository)
        {
            List<BookingWrapper> options = (await repository.GetAllNonPayersGroup()).Select(b => new BookingWrapper(b)).ToList();
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if (((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || (booking.CheckIn - DateTime.Today).TotalDays <= 5) && booking.FullPrice > booking.Customers.Count && booking.Recieved < 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Η κράτηση για { booking.Excursion.Destinations[0]}  " +
                            $"στο όνομα { booking.ReservationsInBooking[0].CustomersList[0] } δέν έχει δώσει προκαταβολή",
                            ReservationWrapper = new ReservationWrapper { BookingWrapper = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                    else if ((booking.CheckIn - DateTime.Today).TotalDays <= 5 && booking.Remaining > 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Η κράτηση για { booking.Excursion.Destinations[0]}  " +
                            $"στο όνομα { booking.ReservationsInBooking[0].CustomersList[0] } δέν έχει κάνει εξόφληση",
                            ReservationWrapper = new ReservationWrapper { BookingWrapper = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<Notification>> GetNonPayersPersonal(GenericRepository repository)
        {
            List<Personal_BookingWrapper> options = (await repository.GetAllNonPayersPersonal()).Select(b => new Personal_BookingWrapper(b)).ToList();
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if ((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || booking.Services.Any(a => (a.TimeGo - DateTime.Today).TotalDays <= 5) && booking.FullPrice > booking.Customers.Count && booking.Recieved < 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Το ατομικό πακέτο στο όνομα { booking.Customers[0] } δέν έχει δώσει προκαταβολή",
                            ReservationWrapper = new ReservationWrapper { PersonalModel = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                    else if (booking.Services.Any(a => (a.TimeGo - DateTime.Today).TotalDays <= 5) && booking.Remaining > 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Το ατομικό πακέτο στο όνομα { booking.Customers[0] } δέν έχει δώσει προκαταβολή",
                            ReservationWrapper = new ReservationWrapper { PersonalModel = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<Notification>> GetNonPayersThirdParty(GenericRepository repository)
        {
            List<ThirdParty_Booking_Wrapper> options = (await repository.GetAllNonPayersThirdparty()).Select(b => new ThirdParty_Booking_Wrapper(b)).ToList();
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if (((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || (booking.CheckIn - DateTime.Today).TotalDays <= 5) && booking.FullPrice > booking.Customers.Count && booking.Recieved < 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Το πακέτο συνεργάτη για { booking.City} " +
                            $"στο όνομα { booking.Customers[0]} δέν έχει δώσει προκαταβολή",
                            ReservationWrapper = new ReservationWrapper { ThirdPartyModel = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                    else if ((booking.CheckIn - DateTime.Today).TotalDays <= 5 && booking.Remaining > 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Το πακέτο συνεργάτη για { booking.City} " +
                            $"στο όνομα { booking.Customers[0]} δέν έχει κάνει εξόφληση",
                            ReservationWrapper = new ReservationWrapper { ThirdPartyModel = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<Notification>> GetPersonalCheckIns(GenericRepository repository)
        {
            List<PlaneService> options = await repository.GetAllPlaneOptions();
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                foreach (var option in options)
                {
                    if (option.Personal_Booking == null)
                    {
                        continue;
                    }
                    if (option.Airline != null && option.Airline.Checkin > 10)
                    {
                        if (option.Airline.Checkin != 0 && option.TimeGo > DateTime.Now &&
                            (option.TimeGo - DateTime.Now).TotalHours <= option.Airline.Checkin)
                        {
                            reply.Add(new Notification { Details = $"Ανοιξε το CheckIn {option.From}-{option.To} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}", NotificaationType = NotificaationType.CheckIn, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) }, Service = option });
                        }
                        if (option.Airline.Checkin != 0 && option.TimeReturn > DateTime.Now && option.Allerretour &&
                            (option.TimeReturn - DateTime.Now).TotalHours <= option.Airline.Checkin)
                        {
                            reply.Add(new Notification { Details = $"Ανοιξε το το CheckIn Επιστροφής {option.To}-{option.From} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}", NotificaationType = NotificaationType.CheckIn, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) }, Service = option });
                        }
                    }
                    else
                    {
                        if (option.Airline.Checkin != 0 && option.TimeGo > DateTime.Now &&
                           (option.TimeGo - DateTime.Now).TotalHours <= 48)
                        {
                            reply.Add(new Notification { Details = $"Ίσως άνοιξε το CheckIn {option.From}-{option.To} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}", NotificaationType = NotificaationType.CheckIn, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) }, Service = option });
                        }
                        if (option.TimeReturn > DateTime.Now && option.Allerretour &&
                            (option.TimeReturn - DateTime.Now).TotalHours <= 48)
                        {
                            reply.Add(new Notification { Details = $"Ίσως άνοιξε το το CheckIn Επιστροφής {option.To}-{option.From} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}", NotificaationType = NotificaationType.CheckIn, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) }, Service = option });
                        }
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<Notification>> GetPersonalOptions(GenericRepository repository)
        {
            List<HotelService> options = await repository.GetAllPersonalOptions();
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                foreach (var option in options)
                {
                    if (option.Personal_Booking == null)
                    {
                        continue;
                    }
                    reply.Add(new Notification { Details = $"Η Option για το ατομικό {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())} στο {(option.Hotel != null ? option.Hotel.Name : "Αγνωστο Ξενοδοχείο")} λήγει στις {option.Option.ToShortDateString()}", NotificaationType = NotificaationType.PersonalOption, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) } });
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
                    case ExcursionTypeEnum.Bansko:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(BanskoParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new BanskoParent_ViewModel(MainViewModel);
                        }
                        break;

                    case ExcursionTypeEnum.Group:

                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(GroupParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new GroupParent_ViewModel(MainViewModel);
                        }
                        break;

                    case ExcursionTypeEnum.Personal:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(PersonalParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new PersonalParent_ViewModel(MainViewModel);
                        }
                        break;

                    case ExcursionTypeEnum.ThirdParty:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(ThirdPartyParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new ThirdPartyParent_ViewModel(MainViewModel);
                        }

                        break;

                    case ExcursionTypeEnum.Skiathos:
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