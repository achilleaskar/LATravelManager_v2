using CommonServiceLocator;
using GalaSoft.MvvmLight;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Tabs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        public NavigationViewModel()
        {
            try
            {
                SecondaryTabs.Add(new AddRoomsTab());
                SecondaryTabs.Add(new SettingsTab());
                SetTabs(null);
                _SecondaryTabs = new List<TabsBaseViewModel>();
                SecondaryViewModels = new List<MyViewModelBase>();
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
                            MessengerInstance.Send(new SetChildViewModelMessage(SecondaryViewModels[viewmodelIndex]));
                        else
                        {
                            var addRoomsViewModel = new AddRooms_ViewModel();
                            SecondaryViewModels.Add(addRoomsViewModel);
                            MessengerInstance.Send(new SetChildViewModelMessage(addRoomsViewModel));
                            await addRoomsViewModel.LoadAsync();
                        }
                        break;

                    case nameof(Settings_Viewmodel):
                        viewmodelIndex = SecondaryViewModels.FindIndex(x => x.GetType() == typeof(Settings_Viewmodel));
                        if (viewmodelIndex >= 0)
                            MessengerInstance.Send(new SetChildViewModelMessage(SecondaryViewModels[viewmodelIndex]));
                        else
                        {
                            var settingsViewModel = new Settings_Viewmodel();
                            SecondaryViewModels.Add(settingsViewModel);
                            MessengerInstance.Send(new SetChildViewModelMessage(settingsViewModel));
                            await settingsViewModel.LoadAsync();
                        }
                        break;
                }
            }
            else
            {
                MessengerInstance.Send(new ChangeViewModelMessage(tab.TabName));
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
            parent = ServiceLocator.Current.GetInstance<MainUserControl_ViewModel>().SelectedExcursionType;
            List<TabsBaseViewModel> tabs;
            if (parent is ExcursionCategory_ViewModelBase)
            {
                Tabs.Clear();
                tabs = (parent as ExcursionCategory_ViewModelBase).Tabs ?? new List<TabsBaseViewModel>();
                foreach (var tab in tabs)
                {
                    Tabs.Add(tab);
                }
            }
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