using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using LATravelManager.UI.ViewModel.Window_ViewModels;

namespace LATravelManager.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, IViewModelAsync
    {
        #region Constructors

        public NavigationViewModel(MainUserControl_ViewModel mainUserControl_ViewModel, MainViewModel mainViewModel)
        {
            try
            {
                MainUserControl_ViewModel = mainUserControl_ViewModel;
                MainViewModel = mainViewModel;

                _SecondaryTabs = new List<TabsBaseViewModel>();
                SecondaryViewModels = new List<MyViewModelBase>();

                SecondaryTabs.Add(new GlobalSearchTab());
                SecondaryTabs.Add(new CardsTab());
                SecondaryTabs.Add(new ManagementTab());
                SecondaryTabs.Add(new EconomicData_Tab());
                SecondaryTabs.Add(new AddRoomsTab());
                //  SecondaryTabs.Add(new SettingsTab());

                SecondaryViewModels.Add(new GlobalSearch_ViewModel(mainViewModel));//
                SecondaryViewModels.Add(new Cards_ViewModel(mainViewModel));
                SecondaryViewModels.Add(new Management_ViewModel(mainViewModel));//
                SecondaryViewModels.Add(new GlobalEconomics_ViewModel(mainViewModel));
                SecondaryViewModels.Add(new AddRooms_ViewModel(mainViewModel));
                // SecondaryViewModels.Add(new Settings_Viewmodel(mainViewModel));

                SelectedSecondaryTabIndex = -1;
                SelectedTabChangedCommand = new RelayCommand(() => SelectTab(SelectedPrimaryTabIndex));
                SelectedChildTabChangedCommand = new RelayCommand(() => SelectTab(SelectedSecondaryTabIndex, true));
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

        public RelayCommand SelectedTabChangedCommand { get; set; }
        public RelayCommand SelectedChildTabChangedCommand { get; set; }

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

        public MainViewModel MainViewModel { get; }

        #endregion Properties

        #region Methods

        public async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            await Task.Delay(0);
        }

        public Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        public void SetTabs()
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
            if (parent.SelectedChildViewModel == null)
            {
                _SelectedPrimaryTabIndex = 0;
            }
            else
            {
                _SelectedPrimaryTabIndex = parent.Childs.IndexOf(parent.SelectedChildViewModel);
            }
            SelectTab(_SelectedPrimaryTabIndex);
        }

        internal void SelectTab(int index, bool secondary = false)
        {
            if (index >= 0)
            {
                for (int i = 0; i < Tabs.Count; i++)
                {
                    if (!secondary && i == index)
                    {
                        if (!Tabs[index].Selected)
                        {
                            Tabs[index].Selected = true;
                            parent.SetProperChildViewModel(index);
                        }
                    }
                    else
                    {
                        Tabs[i].Selected = false;
                    }
                }
                for (int i = 0; i < SecondaryTabs.Count; i++)
                {
                    if (secondary && i == index)
                    {
                        if (!SecondaryTabs[index].Selected)
                        {
                            SecondaryTabs[index].Selected = true;
                            SelectSecondaryTab(i);
                        }
                    }
                    else
                    {
                        SecondaryTabs[i].Selected = false;
                    }
                }
            }
        }

        private void SelectSecondaryTab(int tabIndex)
        {
            parent.SelectedChildViewModel = SecondaryViewModels[tabIndex];
            if (parent.SelectedChildViewModel is MyViewModelBase mb && !mb.IsLoaded)
                mb.Load();
        }

        #endregion Methods
    }
}