﻿using GalaSoft.MvvmLight.CommandWpf;
using LaTravelManager.ViewModel.Management;
using LATravelManager.Model.Excursions;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Management;
using LATravelManager.UI.ViewModel.Parents;
using LATravelManager.UI.Views;
using LATravelManager.UI.Views.Management;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class MainUserControl_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public MainUserControl_ViewModel(MainViewModel mainViewModel)
        {
            LogOutCommand = new RelayCommand(async () => { await TryLogOut(); });

            OpenHotelEditCommand = new RelayCommand(OpenHotelsWindow, CanEditWindows);
            OpenCitiesEditCommand = new RelayCommand(OpenCitiesWindow, CanEditWindows);
            OpenCountriesEditCommand = new RelayCommand(OpenCountriesWindow, CanEditWindows);
            OpenExcursionsEditCommand = new RelayCommand(OpenExcursionsWindow, CanEditWindows);
            OpenUsersEditCommand = new RelayCommand(OpenUsersWindow, CanEditWindows);
            OpenPartnersEditCommand = new RelayCommand(OpenPartnersWindow, CanEditWindows);

            TemplateViewmodels = new List<ExcursionCategory_ViewModelBase>();

            MessengerInstance.Register<IsBusyChangedMessage>(this, msg => { ManageIsBusy(msg.IsBusy); });

            SelectedTemplateChangedCommand = new RelayCommand(async () => { await SetProperViewModel(); });

            MainViewModel = mainViewModel;

            // MessengerInstance.Register<ChangeChildViewModelMessage>(this, async vm => { await SelectedExcursionType.SetProperChildViewModel(vm.ViewModelindex); });

            //  MessengerInstance.Register<ExcursionCategoryChangedMessage>(this, async index => { await SetProperViewModel(); });
        }

        #endregion Constructors

        #region Fields

        private bool _IsBusy = false;

        private NavigationViewModel _NavigationViewModel;

        private ExcursionCategory_ViewModelBase _SelectedExcursionType;

        private int _SelectedTemplateIndex;

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
                Application.Current.Dispatcher.Invoke(delegate
                {
                    if (value)
                        Mouse.OverrideCursor = Cursors.Wait;
                    else
                        Mouse.OverrideCursor = Cursors.Arrow;
                });
                RaisePropertyChanged();
            }
        }

        public int IsBusyCounter { get; set; }

        public RelayCommand LogOutCommand { get; set; }

        public MainViewModel MainViewModel { get; }

        public NavigationViewModel NavigationViewModel
        {
            get
            {
                return _NavigationViewModel;
            }

            set
            {
                if (_NavigationViewModel == value)
                {
                    return;
                }

                _NavigationViewModel = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenCitiesEditCommand { get; }

        public RelayCommand OpenCountriesEditCommand { get; }

        public RelayCommand OpenExcursionsEditCommand { get; }

        public RelayCommand OpenHotelEditCommand { get; }

        public RelayCommand OpenPartnersEditCommand { get; }

        public RelayCommand OpenUsersEditCommand { get; set; }

        public ExcursionCategory_ViewModelBase SelectedExcursionType
        {
            get
            {
                return _SelectedExcursionType;
            }

            set
            {
                if (_SelectedExcursionType == value)
                {
                    return;
                }

                _SelectedExcursionType = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SelectedTemplateChangedCommand { get; set; }

        public int SelectedTemplateIndex
        {
            get
            {
                return _SelectedTemplateIndex;
            }

            set
            {
                if (_SelectedTemplateIndex == value)
                {
                    return;
                }

                _SelectedTemplateIndex = value;
                //if (value >= 0 && value <= Templates.Count)
                //{
                //    MessengerInstance.Send(new ExcursionCategoryChangedMessage());
                //}
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ExcursionCategory> _Templates;

        public ObservableCollection<ExcursionCategory> Templates
        {
            get
            {
                return _Templates;
            }

            set
            {
                if (_Templates == value)
                {
                    return;
                }

                _Templates = value;
                RaisePropertyChanged();
            }
        }

        public List<ExcursionCategory_ViewModelBase> TemplateViewmodels { get; set; }

        public string UserName => StaticResources.User != null ?
            $"{StaticResources.User.Name} {((!string.IsNullOrEmpty(StaticResources.User.Surename) && StaticResources.User.Surename.Length > 4) ? StaticResources.User.Surename.Substring(0, 5) : "")}"
            : "Error";

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            NavigationViewModel = new NavigationViewModel(this, MainViewModel);
            Templates = MainViewModel.BasicDataManager.ExcursionCategories;

            await Task.Delay(0);
            // await SetProperViewModel();
        }

        public void OpenCitiesWindow()
        {
            CitiesManagement_ViewModel vm = new CitiesManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new CitiesManagement_Window { DataContext = vm }));
        }

        public void OpenCountriesWindow()
        {
            CountriesManagement_ViewModel vm = new CountriesManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new CountriesManagement_Window { DataContext = vm }));
        }

        public void OpenHotelsWindow()
        {
            HotelsManagement_ViewModel vm = new HotelsManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new HotelsManagement_Window { DataContext = vm }));
        }

        public async Task TryLogOut()
        {
            StaticResources.User = null;
            await MainViewModel.ChangeViewModel();
        }

        private bool CanEditWindows()
        {
            return true;
            // return MainViewModel. StartingRepository.IsContextAvailable;
        }

        private void ManageIsBusy(bool add)
        {
            _ = add ? IsBusyCounter++ : IsBusyCounter--;
            IsBusy = IsBusyCounter > 0;
        }

        private void OpenExcursionsWindow()
        {
            ExcursionsManagement_ViewModel vm = new ExcursionsManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new ExcursionsManagement_Window { DataContext = vm }));
        }

        private void OpenPartnersWindow()
        {
            PartnerManagement_ViewModel vm = new PartnerManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new PartnersManagement_Window { DataContext = vm }));
        }

        private void OpenUsersWindow()
        {
            UsersManagement_viewModel vm = new UsersManagement_viewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new UsersManagement_Window { DataContext = vm }));
        }

        private async Task SetProperViewModel()
        {
            if (SelectedTemplateIndex < Templates.Count)
            {
                int index = -1;
                switch (Templates[SelectedTemplateIndex].Category)
                {
                    case Model.ExcursionTypeEnum.Bansko:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(BanskoParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new BanskoParent_ViewModel(MainViewModel);
                        }
                        break;

                    case Model.ExcursionTypeEnum.Group:

                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(GroupParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new GroupParent_ViewModel(MainViewModel);
                        }
                        break;

                    case Model.ExcursionTypeEnum.Personal:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(PersonalParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new PersonalParent_ViewModel(MainViewModel);
                        }
                        break;

                    case Model.ExcursionTypeEnum.ThirdParty:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(ThirdPartyParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new ThirdPartyParent_ViewModel(MainViewModel);
                        }

                        break;

                    case Model.ExcursionTypeEnum.Skiathos:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(SkiathosParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new SkiathosParent_ViewModel(MainViewModel);
                        }

                        break;
                }
                if (index < 0)
                {
                    TemplateViewmodels.Add(SelectedExcursionType);
                }
                //set to default tab
                if (SelectedExcursionType is ExcursionCategory_ViewModelBase)
                {
                    if (index < 0)
                        SelectedExcursionType.Load();
                    await NavigationViewModel.SetTabs();
                }
            }
            else
            {
                throw new Exception("SelectedTemplateIndex greater than Templates count");
            }
        }

        public override async Task ReloadAsync()
        {
            await Task.Delay(0);
        }

        #endregion Methods
    }
}