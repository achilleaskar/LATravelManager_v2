using GalaSoft.MvvmLight;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, IViewModel
    {
        #region Constructors

        public NavigationViewModel(MainUserControl_ViewModel mainUserControl_ViewModel)
        {
            try
            {
                MainUserControl_ViewModel = mainUserControl_ViewModel;
                _SecondaryTabs = new List<TabsBaseViewModel>();
                SecondaryViewModels = new List<MyViewModelBase>();
                SecondaryTabs.Add(new GlobalSearchTab());
                SecondaryTabs.Add(new InfoTab());
                SecondaryTabs.Add(new AddRoomsTab());
                SecondaryTabs.Add(new SettingsTab());
                // SetTabs();
                SecondaryViewModels.Add(new GlobalSearch_ViewModel());
                SecondaryViewModels.Add(new Info_ViewModel());
                SecondaryViewModels.Add(new AddRooms_ViewModel());
                SecondaryViewModels.Add(new Settings_Viewmodel());
                MessengerInstance.Register<ResetNavigationTabsMessage>(this, async msg => { await SetTabs(); });
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        #endregion Constructors

        #region Fields

        private readonly List<MyViewModelBase> SecondaryViewModels;

        private bool _Expanded = true;

        private List<TabsBaseViewModel> _SecondaryTabs;

        private int _SelectedPrimaryTabIndex;

        private int _SelectedSecondaryTabIndex;

        private ObservableCollection<TabsBaseViewModel> _Tabs = new ObservableCollection<TabsBaseViewModel>();

        private ExcursionCategory_ViewModelBase parent;

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

        public bool IsLoaded { get; set; }

        public MainUserControl_ViewModel MainUserControl_ViewModel { get; }

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

        public int SelectedPrimaryTabIndex
        {
            get
            {
                return _SelectedPrimaryTabIndex;
            }

            set
            {
                if (_SelectedPrimaryTabIndex == value)
                {
                    return;
                }

                _SelectedPrimaryTabIndex = value;
                if (value >= 0 && value < Tabs.Count)
                {
                    Tabs[value].IsSelected = true;
                    Task.Run(() => SelectTab(value));
                }
                RaisePropertyChanged();
            }
        }

        public int SelectedSecondaryTabIndex
        {
            get
            {
                return _SelectedSecondaryTabIndex;
            }

            set
            {
                if (_SelectedSecondaryTabIndex == value)
                {
                    return;
                }

                _SelectedSecondaryTabIndex = value;
                if (value >= 0 && value < Tabs.Count)
                {
                    SecondaryTabs[value].IsSelected = true;
                    Task.Run(() => SelectTab(value, true));
                }
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

        public async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            await Task.Delay(0);
        }

        public Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        internal async Task SelectTab(int index, bool secondary = false)
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                if (!secondary && i == index)
                {
                    await parent.SetProperChildViewModel(index);
                }
                else
                {
                    Tabs[i].IsSelected = false;
                }
            }
            for (int i = 0; i < SecondaryTabs.Count; i++)
            {
                if (secondary && i == index)
                {
                    await SelectSecondaryTab(i);
                }
                else
                {
                    SecondaryTabs[i].IsSelected = false;
                }
            }

        }

        private async Task SelectSecondaryTab(int tabIndex)
        {
            parent.SelectedChildViewModel = SecondaryViewModels[tabIndex];
            await parent.SelectedChildViewModel.LoadAsync();
        }
        private async Task SetTabs()
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
            _SelectedPrimaryTabIndex = 0;
            await SelectTab(0);
        }

        #endregion Methods

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

    }
}