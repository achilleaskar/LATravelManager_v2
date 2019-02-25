using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LaTravelManager.ViewModel;
using LaTravelManager.ViewModel.Management;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Views;
using LATravelManager.UI.Views.Management;
using System.Threading.Tasks;
using System.Windows;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Constructors

        public MainViewModel()
        {
           
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
            await SelectedViewmodel.LoadAsync();
            RaisePropertyChanged(nameof(MenuVisibility));
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
            Helpers.StaticResources.User = new User { BaseLocation = 1, Id = 1, Level = 0, UserName = "admin" };
            RaisePropertyChanged(nameof(MenuVisibility));
#endif
            if (Helpers.StaticResources.User == null)
                SelectedViewmodel = new LoginViewModel(StartingRepository);//TODO
            else
                SelectedViewmodel = new MainUserControl_ViewModel(StartingRepository);//TODO
            await SelectedViewmodel.LoadAsync();
        }

      

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

       
    }
}