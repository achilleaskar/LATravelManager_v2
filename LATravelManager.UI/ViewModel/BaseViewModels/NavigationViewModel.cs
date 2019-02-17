using CommonServiceLocator;
using GalaSoft.MvvmLight;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, IViewModel
    {
        public NavigationViewModel(MainUserControl_ViewModel mainUserControl_ViewModel)
        {
            try
            {
                MainUserControl_ViewModel = mainUserControl_ViewModel;
                _SecondaryTabs = new List<TabsBaseViewModel>();
                SecondaryViewModels = new List<MyViewModelBase>();
                SecondaryTabs.Add(new AddRoomsTab());
                SecondaryTabs.Add(new SettingsTab());
                SetTabs(null);
                MessengerInstance.Register<ResetNavigationTabsMessage>(this, msg => SetTabs(msg));
                MessengerInstance.Register<SelectedTabChangedMessage>(this, async tab => await SelectTab(tab));
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }

        }

        private async Task SelectTab(SelectedTabChangedMessage tab)
        {
            var viewmodelIndex = -1;
            if (tab.IsChild)
            {
                switch (tab.TabName)
                {
                    case nameof(AddRooms_ViewModel):
                        viewmodelIndex = SecondaryViewModels.FindIndex(x => x.GetType() == typeof(AddRooms_ViewModel));
                        if (viewmodelIndex >= 0)
                            MessengerInstance.Send(new SetSecondaryChildViewModelMessage(SecondaryViewModels[viewmodelIndex]));
                        else
                        {
                            var addRoomsViewModel = new AddRooms_ViewModel();
                            SecondaryViewModels.Add(addRoomsViewModel);
                            MessengerInstance.Send(new SetSecondaryChildViewModelMessage(addRoomsViewModel));
                            await addRoomsViewModel.LoadAsync(0);
                        }
                        break;

                    case nameof(Settings_Viewmodel):
                        viewmodelIndex = SecondaryViewModels.FindIndex(x => x.GetType() == typeof(Settings_Viewmodel));
                        if (viewmodelIndex >= 0)
                            MessengerInstance.Send(new SetSecondaryChildViewModelMessage(SecondaryViewModels[viewmodelIndex]));
                        else
                        {
                            var settingsViewModel = new Settings_Viewmodel();
                            SecondaryViewModels.Add(settingsViewModel);
                            MessengerInstance.Send(new SetSecondaryChildViewModelMessage(settingsViewModel));
                            await settingsViewModel.LoadAsync(0);
                        }
                        break;
                }
            }
            else
            {
                MessengerInstance.Send(new ChangeChildViewModelMessage(tab.Index));
            }
        }

        private TabsBaseViewModel _SelectedTab;

        public TabsBaseViewModel SelectedTab
        {
            get
            {
                return _SelectedTab;
            }

            set
            {
                if (_SelectedTab == value)
                {
                    return;
                }

                _SelectedTab = value;
                RaisePropertyChanged();
            }
        }

        private void SetTabs(ResetNavigationTabsMessage obj)
        {
            parent = MainUserControl_ViewModel.SelectedExcursionType;
            List<TabsBaseViewModel> tabs;
            if (parent is ExcursionCategory_ViewModelBase)
            {
                Tabs.Clear();
                tabs = (parent as ExcursionCategory_ViewModelBase).Tabs ?? new List<TabsBaseViewModel>();
                foreach (TabsBaseViewModel tab in tabs)
                {
                    Tabs.Add(tab);
                }
            }
        }


        public async Task LoadAsync(int id)
        {
            await Task.Delay(0);
        }

        public Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        #region Fields

        private ExcursionCategory_ViewModelBase parent;

        private bool _Expanded = true;

        private ObservableCollection<TabsBaseViewModel> _Tabs = new ObservableCollection<TabsBaseViewModel>();
        private List<TabsBaseViewModel> _SecondaryTabs;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Expanded property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool Expanded
        {
            get
            {
                return _Expanded;
            }

            set
            {
                if (_Expanded == value)
                {
                    return;
                }

                _Expanded = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the SecondaryTabs property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public List<TabsBaseViewModel> SecondaryTabs
        {
            get
            {
                return _SecondaryTabs;
            }

            set
            {
                if (_SecondaryTabs == value)
                {
                    return;
                }

                _SecondaryTabs = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the Tabs property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<TabsBaseViewModel> Tabs
        {
            get
            {
                return _Tabs;
            }

            set
            {
                if (_Tabs == value)
                {
                    return;
                }

                _Tabs = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLoaded { get; set; }
        public MainUserControl_ViewModel MainUserControl_ViewModel { get; }

        #endregion Properties

        #region Methods

        private List<MyViewModelBase> SecondaryViewModels;

        //private void SetProperViewModel()
        //{
        //    if (SelectedTabIndex >= 0)
        //    {
        //        if (!(ServiceLocator.Current.GetInstance<MainUserControl_ViewModel>().SelectedExcursionType is MainExcursionCategoryViewModelBase))
        //            ServiceLocator.Current.GetInstance<MainUserControl_ViewModel>().SetProperViewModel();
        //        MessengerInstance.Send(new SelectedTabChangedMessage(SelectedTabIndex));
        //        if (SelectedSecondaryTabIndex >= 0)
        //            SecondaryTabs[SelectedSecondaryTabIndex].IsSelected = false;
        //    }
        //}

        #endregion Methods
    }
}