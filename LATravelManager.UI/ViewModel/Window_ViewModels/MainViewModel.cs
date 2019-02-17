using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Constructors

        public MainViewModel()
        {
            //OpenHotelEditCommand = new RelayCommand(OpenHotelsWindow, CanEditWindows);
            //OpenCitiesEditCommand = new RelayCommand(OpenCitiesWindow, CanEditWindows);
            //OpenCountriesEditCommand = new RelayCommand(OpenCountriesWindow, CanEditWindows);
            //OpenExcursionsEditCommand = new RelayCommand(OpenExcursionsWindow, CanEditWindows);
            //OpenUsersEditCommand = new RelayCommand(OpenUsersWindow, CanEditWindows);
            Messenger.Default.Register<ChangeVisibilityMessage>(this, msg => { Visibility = msg.Visible ? Visibility.Visible : Visibility.Collapsed; });
            Messenger.Default.Register<IsBusyChangedMessage>(this, msg => { ManageIsBusy(msg.IsBusy); });
            Messenger.Default.Register<LoginLogOutMessage>(this, async msg => { await ChangeViewModel(msg.Login); });
        }

        private async Task ChangeViewModel(bool login)
        {
            if (login)
            {
                SelectedViewmodel = new MainUserControl_ViewModel(StartingRepository);//TODO
            }
            else
            {
                SelectedViewmodel = new LoginViewModel(StartingRepository);
            }
            await SelectedViewmodel.LoadAsync(0);
        }

        #endregion Constructors

        #region Fields

        private bool _IsBusy = false;

        private IViewModel _SelectedViewmodel;

        private Visibility _Visibility;

        private GenericRepository StartingRepository;

        #endregion Fields

        #region Properties

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
                RaisePropertyChanged();
            }
        }

        public int IsBusyCounter { get; private set; }

        public Visibility MenuVisibility
        {
            get
            {
                if (Helpers.StaticResources.User != null && Helpers.StaticResources.User.Level <= 1)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public RelayCommand OpenCitiesEditCommand { get; }

        public RelayCommand OpenCountriesEditCommand { get; }

        public RelayCommand OpenExcursionsEditCommand { get; }

        public RelayCommand OpenHotelEditCommand { get; }

        public RelayCommand OpenUsersEditCommand { get; set; }

        public IViewModel SelectedViewmodel
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

        /// <summary>
        /// Sets and gets the Visibility property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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

        public async Task LoadAsync(GenericRepository startingRepository)
        {
            StartingRepository = startingRepository;
#if DEBUG
            Helpers.StaticResources.User = new User { BaseLocation = User.GrafeiaXriston.Thessalonikis, Id = 1, Level = 0, UserName = "admin" };
#endif
            if (Helpers.StaticResources.User == null)
                SelectedViewmodel = new LoginViewModel(StartingRepository);//TODO
            else
                SelectedViewmodel = new MainUserControl_ViewModel(StartingRepository);//TODO
            await SelectedViewmodel.LoadAsync(0);
        }

        private bool CanEditWindows()
        {
            if (IsInDesignMode)
                return true;
            return IsBusy;
        }

        //public void OpenHotelsWindow()
        //{
        //    //SimpleIoc.Default.Register<HotelsManagement_ViewModel>();
        //    MessengerInstance.Send(new OpenChildWindowCommand(new HotelsManagement_Window()));
        //}
        private void ChangeVisibility(ChangeVisibilityMessage msg)
        {
            Visibility = msg.Visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ManageIsBusy(bool add)
        {
            _ = add ? IsBusyCounter++ : IsBusyCounter--;
            IsBusy = IsBusyCounter > 0;
        }

        #endregion Methods

        //public void OpenCitiesWindow()
        //{
        //    //SimpleIoc.Default.Register<HotelsManagement_ViewModel>();
        //    MessengerInstance.Send(new OpenChildWindowCommand(new CitiesManagement_Window()));
        //}

        //public void OpenCountriesWindow()
        //{
        //    //SimpleIoc.Default.Register<HotelsManagement_ViewModel>();
        //    MessengerInstance.Send(new OpenChildWindowCommand(new CountriesManagement_Window()));
        //}
        //private void OpenExcursionsWindow()
        //{
        //    MessengerInstance.Send(new OpenChildWindowCommand(new ExcursionsManagement_Window()));
        //}

        //private void OpenUsersWindow()
        //{
        //    MessengerInstance.Send(new OpenChildWindowCommand(new UsersManagement_Window()));
        //}
    }
}