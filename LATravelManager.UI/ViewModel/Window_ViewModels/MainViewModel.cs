using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System.Collections.Generic;
using System.Linq;
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

          //  Messenger.Default.Register<LoginLogOutMessage>(this, async msg => { await ChangeViewModel(msg.Login); });
        }

        public async Task ChangeViewModel()
        {
            if (StaticResources.User != null)
            {
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

        private bool _IsBusy = false;

        private ViewModelBase _SelectedViewmodel;

        private Visibility _Visibility;


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

        public async Task LoadAsync(GenericRepository startingRepository)
        {
            BasicDataManager = new BasicDataManager();

            await BasicDataManager.LoadAsync();


#if DEBUG
            StaticResources.User = new User { BaseLocation = 1, Id = 1, Level = 0, UserName = "admin" };
            RaisePropertyChanged(nameof(MenuVisibility));
#endif
            await ChangeViewModel();

        }

        private void ManageIsBusy(bool add)
        {
            _ = add ? IsBusyCounter++ : IsBusyCounter--;
            IsBusy = IsBusyCounter > 0;
        }

        #endregion Methods
    }
}