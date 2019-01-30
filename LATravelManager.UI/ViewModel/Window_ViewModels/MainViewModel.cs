using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views;
using LATravelManager.UI.Views.Management;
using System.Threading.Tasks;
using System.Windows;

namespace LATravelManager.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        #region Constructors

        public MainViewModel()
        {
            OpenHotelEditCommand = new RelayCommand(OpenHotelsWindow, CanEditWindows);
            OpenCitiesEditCommand = new RelayCommand(OpenCitiesWindow, CanEditWindows);
            OpenCountriesEditCommand = new RelayCommand(OpenCountriesWindow, CanEditWindows);
            OpenExcursionsEditCommand = new RelayCommand(OpenExcursionsWindow, CanEditWindows);
            OpenUsersEditCommand = new RelayCommand(OpenUsersWindow, CanEditWindows);
            Messenger.Default.Register<ChangeVisibilityMessage>(this, msg => { Visibility = msg.Visible ? Visibility.Visible : Visibility.Collapsed; });
            Messenger.Default.Register<IsBusyChangedMessage>(this, msg => { IsBusy = msg.IsBusy; });
        }

        #endregion Constructors

        #region Fields

        public IViewModel SelectedViewmodel;
        private bool _IsBusy = false;
        private Visibility _Visibility;
        private ViewModelBase CurrentViewModel;

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

        /// <summary>
        /// Sets and gets the MenuVisibility property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
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

        public async Task LoadAsync()
        {
#if DEBUG
            Helpers.StaticResources.User = new User { Name = "Admin", Id = -12, BaseLocation = User.GrafeiaXriston.Thessalonikis, Level = 0 };
#endif

            if (Helpers.StaticResources.User == null)
                SelectedViewmodel = new LoginViewModel();//TODO
            else
                SelectedViewmodel = new MainUserControl_ViewModel();//TODO
            await SelectedViewmodel.LoadAsync();
        }

        public void OpenCitiesWindow()
        {
            //SimpleIoc.Default.Register<HotelsManagement_ViewModel>();
            MessengerInstance.Send(new OpenChildWindowCommand(new CitiesManagement_Window()));
        }

        public void OpenCountriesWindow()
        {
            //SimpleIoc.Default.Register<HotelsManagement_ViewModel>();
            MessengerInstance.Send(new OpenChildWindowCommand(new CountriesManagement_Window()));
        }

        public void OpenHotelsWindow()
        {
            //SimpleIoc.Default.Register<HotelsManagement_ViewModel>();
            MessengerInstance.Send(new OpenChildWindowCommand(new HotelsManagement_Window()));
        }

        private bool CanEditWindows()
        {
            if (IsInDesignMode)
                return true;
            return IsBusy;
        }

        private void ChangeVisibility(ChangeVisibilityMessage msg)
        {
            Visibility = msg.Visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OpenExcursionsWindow()
        {
            MessengerInstance.Send(new OpenChildWindowCommand(new ExcursionsManagement_Window()));
        }

        private void OpenUsersWindow()
        {
            MessengerInstance.Send(new OpenChildWindowCommand(new UsersManagement_Window()));
        }

        #endregion Methods

    }
}