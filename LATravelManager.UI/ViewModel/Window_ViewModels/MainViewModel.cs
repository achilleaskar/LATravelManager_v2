using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Constructors

        public MainViewModel()
        {
            Visibility = Visibility.Hidden;
            Messenger.Default.Register<ChangeVisibilityMessage>(this, msg => { Visibility = msg.Visible ? Visibility.Visible : Visibility.Collapsed; });
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

        public async Task ChangeViewModel()
        {
            if (StaticResources.User != null)
            {
                // await Task.Run(async () => { await LoadOptions(); });
                SelectedViewmodel = new MainUserControl_ViewModel(this);//TODO
                await (SelectedViewmodel as MainUserControl_ViewModel).LoadAsync();
            }
            else
            {
                SelectedViewmodel = new LoginViewModel(this);
            }
            RaisePropertyChanged(nameof(MenuVisibility));
        }

        #endregion Constructors

        #region Fields

        private ViewModelBase _SelectedViewmodel;

        private Visibility _Visibility;

        #endregion Fields

        #region Properties

        public int IsBusyCounter { get; private set; }

        public Visibility MenuVisibility
        {
            get
            {
                if (StaticResources.User != null && StaticResources.User.Level <= 1)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public ViewModelBase SelectedViewmodel
        {
            get
            {
                return _SelectedViewmodel;
            }

            set
            {
                if (_SelectedViewmodel == value)
                {
                    return;
                }

                _SelectedViewmodel = value;
                RaisePropertyChanged();
            }
        }

        public Visibility Visibility
        {
            get
            {
                return _Visibility;
            }

            set
            {
                if (_Visibility == value)
                {
                    return;
                }

                _Visibility = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public BasicDataManager BasicDataManager { get; set; }

        private GenericRepository _StartingRepository;

        public GenericRepository StartingRepository
        {
            get
            {
                return _StartingRepository;
            }

            set
            {
                if (_StartingRepository == value)
                {
                    return;
                }

                _StartingRepository = value;
                RaisePropertyChanged();
            }
        }


        private DispatcherTimer dispatcherTimer;

        public async Task LoadAsync()
        {
            StartingRepository = new GenericRepository();
            BasicDataManager = new BasicDataManager(StartingRepository);

            await BasicDataManager.LoadAsync();

            //dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(CheckForNotifications);
            //dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            //dispatcherTimer.Start();
            var x = GetaAllNotifications().ConfigureAwait(false);

#if DEBUG
            StaticResources.User = new User { BaseLocation = 1, Id = 1, Level = 0, UserName = "admin" };
            RaisePropertyChanged(nameof(MenuVisibility));
#endif
            await ChangeViewModel();
        }

        private async Task GetaAllNotifications()
        {
            var Repository = new GenericRepository();
            List<string> nots = new List<string>();

            nots.AddRange(await LoadOptions(Repository));
            //nots.AddRange(await Repository.GetNotifications());
            nots.AddRange(await GetPersonalOptios(Repository));
            nots.AddRange(await GetPersonalCheckIns(Repository));
            nots.AddRange(await GetNonPayers(Repository));
        }

        private async Task<IEnumerable<string>> GetNonPayers(GenericRepository repository)
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

        private Task<IEnumerable<string>> GetPersonalCheckIns(GenericRepository repository)
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

        private async Task<IEnumerable<string>> GetPersonalOptios(GenericRepository repository)
        {
            List<HotelService> options = await repository.GetAllPersonalOptions();
            List<string> reply = new List<string>();

            if (options.Count > 0)
            {
                foreach (var option in options)
                {
                    if (option.Personal_Booking==null)
                    {
                        continue;
                    }
                    reply.Add($"Η Option για το ατομικό {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())} στο {(option.Hotel!=null? option.Hotel.Name:"Αγνωστο Ξενοδοχείο")} λήγει στις {option.Option.ToShortDateString()}");
                }
            }
            return reply;
        }
        #endregion Methods
    }


    public class HotelOptions
    {
        public Hotel Hotel { get; set; }
        public int Counter { get; set; }
        public DateTime Date { get; set; }
    }
}