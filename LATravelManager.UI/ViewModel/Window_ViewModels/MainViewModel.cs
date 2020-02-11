using DocumentFormat.OpenXml.Office2010.ExcelAc;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

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

        public async Task LoadAsync()
        {
            StartingRepository = new GenericRepository();
            BasicDataManager = new BasicDataManager(StartingRepository);

            await BasicDataManager.LoadAsync();

            //dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(CheckForNotifications);
            //dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            //dispatcherTimer.Start();

#if DEBUG
            StaticResources.User = new User { BaseLocation = 1, Id = 1, Level = 0, UserName = "admin" };
            RaisePropertyChanged(nameof(MenuVisibility));
#endif
            await ChangeViewModel();
        }
    }

   
}