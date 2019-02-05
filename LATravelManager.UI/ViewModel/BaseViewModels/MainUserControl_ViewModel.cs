using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Skiathos;
using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using LATravelManager.UI.ViewModel.Parents;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel
{
    public class MainUserControl_ViewModel : MyViewModelBase
    {

        #region Constructors

        public MainUserControl_ViewModel(GenericRepository startingRepository)
        {
            LogOutCommand = new RelayCommand(TryLogOut, CanLogout);

            Templates = new ObservableCollection<ExcursionCategory>();
            TemplateViewmodels = new List<ExcursionCategory_ViewModelBase>();

            MessengerInstance.Register<IsBusyChangedMessage>(this, msg => { ManageIsBusy(msg.IsBusy); });
            MessengerInstance.Register<SetSecondaryChildViewModelMessage>(this, tab => { SelectedExcursionType.SelectedChildViewModel = tab.Viewmodel; });
            MessengerInstance.Register<ChangeChildViewModelMessage>(this, async vm => { await SelectedExcursionType.SetProperChildViewModel(vm.ViewModelindex); });
            MessengerInstance.Register<ExcursionCategoryChanged>(this, async index => { await SetProperViewModel(); });
            StartingRepository = startingRepository;
        }

        #endregion Constructors

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
                RaisePropertyChanged();
            }
        }

        public int IsBusyCounter { get; set; }

        public bool IsLoaded { get; set; }

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
                    MessengerInstance.Send(new ExcursionCategoryChanged());
                    MessengerInstance.Send(new ResetNavigationTabsMessage());
                }
                RaisePropertyChanged();
            }
        }

        public Repositories.GenericRepository StartingRepository { get; }

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

        public override async  Task LoadAsync(int id)
        {
            NavigationViewModel = new NavigationViewModel(this);
            await NavigationViewModel.LoadAsync(0);
            Templates = new ObservableCollection<ExcursionCategory>(await StartingRepository.GetAllAsync<ExcursionCategory>());
            MessengerInstance.Send(new ExcursionCategoryChanged());
            MessengerInstance.Send(new ResetNavigationTabsMessage());
        }

        public override async Task ReloadAsync()
        {
            throw new System.NotImplementedException();
        }

        public void TryLogOut()
        {
            //TODO
            //MessengerInstance.Send(new ChangeViewModelMessage(nameof(Login_ViewModel)));
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
            var index = -1;
            switch (SelectedTemplateIndex+1)
            {
                case 1:
                    index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(BanskoParent_ViewModel));
                    if (index >= 0)
                        SelectedExcursionType = TemplateViewmodels[index];
                    else
                    {
                        SelectedExcursionType = new BanskoParent_ViewModel();
                    }
                    break;

                case 2:

                    index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(GroupParent_ViewModel));
                    if (index >= 0)
                        SelectedExcursionType = TemplateViewmodels[index];
                    else
                    {
                        SelectedExcursionType = new GroupParent_ViewModel();
                    }
                    break;

                case 3:
                    index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(PersonalParent_ViewModel));
                    if (index >= 0)
                        SelectedExcursionType = TemplateViewmodels[index];
                    else
                    {
                        SelectedExcursionType = new PersonalParent_ViewModel();
                    }
                    break;

                case 4:
                    index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(ThirdPartyParent_ViewModel));
                    if (index >= 0)
                        SelectedExcursionType = TemplateViewmodels[index];
                    else
                    {
                        SelectedExcursionType = new ThirdPartyParent_ViewModel();
                    }

                    break;

                case 5:
                    index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(SkiathosParent_ViewModel));
                    if (index >= 0)
                        SelectedExcursionType = TemplateViewmodels[index];
                    else
                    {
                        SelectedExcursionType = new SkiathosParent_ViewModel();
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
                Messenger.Default.Send(new ResetNavigationTabsMessage());
                await (SelectedExcursionType as ExcursionCategory_ViewModelBase).SetProperChildViewModel(0);
            }
        }

        #endregion Methods

    }
}