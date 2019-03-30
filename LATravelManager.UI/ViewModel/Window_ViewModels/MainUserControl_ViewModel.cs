﻿using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LaTravelManager.ViewModel.Management;
using LATravelManager.Models;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Skiathos;
using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using LATravelManager.UI.ViewModel.Management;
using LATravelManager.UI.ViewModel.Parents;
using LATravelManager.UI.Views;
using LATravelManager.UI.Views.Management;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class MainUserControl_ViewModel : MyViewModelBase
    {
        #region Constructors

        public MainUserControl_ViewModel(GenericRepository startingRepository)
        {
            LogOutCommand = new RelayCommand(TryLogOut, CanLogout);
            OpenHotelEditCommand = new RelayCommand(async () => { await OpenHotelsWindow(); }, CanEditWindows);
            OpenCitiesEditCommand = new RelayCommand(async () => { await OpenCitiesWindow(); }, CanEditWindows);
            OpenCountriesEditCommand = new RelayCommand(async () => { await OpenCountriesWindow(); }, CanEditWindows);
            OpenExcursionsEditCommand = new RelayCommand(async () => { await OpenExcursionsWindow(); }, CanEditWindows);
            OpenUsersEditCommand = new RelayCommand(async () => { await OpenUsersWindow(); }, CanEditWindows);
            OpenPartnersEditCommand=new RelayCommand(async () => { await OpenPartnersWindow(); }, CanEditWindows);
            Templates = new ObservableCollection<ExcursionCategory>();
            TemplateViewmodels = new List<ExcursionCategory_ViewModelBase>();

            MessengerInstance.Register<IsBusyChangedMessage>(this, msg => { ManageIsBusy(msg.IsBusy); });
            MessengerInstance.Register<ChangeChildViewModelMessage>(this, async vm => { await SelectedExcursionType.SetProperChildViewModel(vm.ViewModelindex); });
            MessengerInstance.Register<ExcursionCategoryChangedMessage>(this, async index => { await SetProperViewModel(); });
            StartingRepository = startingRepository;
        }

        private async Task OpenPartnersWindow()
        {
            var vm = new PartnerManagement_ViewModel(StartingRepository);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new PartnersManagement_Window { DataContext = vm }));
        }

        #endregion Constructors

        #region Properties

        public RelayCommand OpenCitiesEditCommand { get; }
        public RelayCommand OpenCountriesEditCommand { get; }
        public RelayCommand OpenExcursionsEditCommand { get; }
        public RelayCommand OpenHotelEditCommand { get; }
        public RelayCommand OpenPartnersEditCommand { get; }
        public RelayCommand OpenUsersEditCommand { get; set; }
        public string UserName => StaticResources.User != null ? StaticResources.User.Name : "Error";

        #endregion Properties

        #region Methods

        public async Task OpenCitiesWindow()
        {
            var vm = new CitiesManagement_ViewModel(StartingRepository);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new CitiesManagement_Window { DataContext = vm }));
        }

        public async Task OpenCountriesWindow()
        {
            var vm = new CountriesManagement_ViewModel(StartingRepository);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new CountriesManagement_Window { DataContext = vm }));
        }

        public async Task OpenHotelsWindow()
        {
            var vm = new HotelsManagement_ViewModel(StartingRepository);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new HotelsManagement_Window { DataContext = vm }));
        }

        private bool CanEditWindows()
        {
            return StartingRepository.IsContextAvailable;
        }

        private async Task OpenExcursionsWindow()
        {
            var vm = new ExcursionsManagement_ViewModel(StartingRepository);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new ExcursionsManagement_Window { DataContext = vm }));
        }

        private async Task OpenUsersWindow()
        {
            var vm = new UsersManagement_viewModel(StartingRepository);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new UsersManagement_Window { DataContext = vm }));
        }

        #endregion Methods

        #region Fields

        private bool _IsBusy = false;
        private NavigationViewModel _NavigationViewModel;

        private ExcursionCategory_ViewModelBase _SelectedExcursionType;

        private int _SelectedTemplateIndex;

        private ObservableCollection<ExcursionCategory> _Templates;

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
                Application.Current.Dispatcher.Invoke((Action)delegate
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
                if (value >= 0 && value <= Templates.Count)
                {
                    MessengerInstance.Send(new ExcursionCategoryChangedMessage());
                }
                RaisePropertyChanged();
            }
        }

        public GenericRepository StartingRepository { get; }

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

        public string Username => Helpers.StaticResources.User.Name;

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            NavigationViewModel = new NavigationViewModel(this);
            Templates = new ObservableCollection<ExcursionCategory>((await StartingRepository.GetAllAsync<ExcursionCategory>()).OrderBy(e => e.IndexNum));
            if (StaticResources.StartingPlaces == null || Helpers.StaticResources.StartingPlaces.Count == 0)
            {
                StaticResources.StartingPlaces = new ObservableCollection<StartingPlace>(StartingRepository.GetAllSortedByName<StartingPlace>());
            }
            await SetProperViewModel();
        }

        public override async Task ReloadAsync()
        {
            await Task.Delay(0);
        }

        public void TryLogOut()
        {
            MessengerInstance.Send(new LoginLogOutMessage(false));
        }

        private bool CanLogout()
        {
            //ToDO
            return true;
        }

        private void ManageIsBusy(bool add)
        {
            _ = add ? IsBusyCounter++ : IsBusyCounter--;
            IsBusy = IsBusyCounter > 0;
        }

        private async Task SetProperViewModel()
        {
            if (SelectedTemplateIndex < Templates.Count)
            {
                var index = -1;
                switch (Templates[SelectedTemplateIndex].Category)
                {
                    case Model.Enums.ExcursionTypeEnum.Bansko:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(BanskoParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new BanskoParent_ViewModel(StartingRepository, NavigationViewModel);
                        }
                        break;

                    case Model.Enums.ExcursionTypeEnum.Group:

                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(GroupParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new GroupParent_ViewModel(StartingRepository, NavigationViewModel);
                        }
                        break;

                    case Model.Enums.ExcursionTypeEnum.Personal:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(PersonalParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new PersonalParent_ViewModel(StartingRepository, NavigationViewModel);
                        }
                        break;

                    case Model.Enums.ExcursionTypeEnum.ThirdParty:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(ThirdPartyParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new ThirdPartyParent_ViewModel(StartingRepository, NavigationViewModel);
                        }

                        break;

                    case Model.Enums.ExcursionTypeEnum.Skiathos:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(SkiathosParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new SkiathosParent_ViewModel(StartingRepository, NavigationViewModel);
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
                    await SelectedExcursionType.LoadAsync();
                    Messenger.Default.Send(new ResetNavigationTabsMessage());
                    //  await NavigationViewModel.SelectTab(0);
                }
            }
            else
            {
                throw new System.Exception("SelectedTemplateIndex greater than Templates count");
            }
        }

        #endregion Methods
    }
}