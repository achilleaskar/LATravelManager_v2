using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko;
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
    public class MainUserControl_ViewModel : ViewModelBase, IViewModel
    {
        #region Constructors

        public MainUserControl_ViewModel()
        {
            LogOutCommand = new RelayCommand(TryLogOut, CanLogout);

            Templates = new ObservableCollection<ExcursionCategory>();
            TemplateViewmodels = new List<ExcursionCategory_ViewModelBase>();

            MessengerInstance.Register<IsBusyChangedMessage>(this, msg => { ManageIsBusy(msg.IsBusy); });
            MessengerInstance.Register<SetSecondaryChildViewModelMessage>(this, tab => { SelectedExcursionType.SelectedChildViewModel = tab.Viewmodel; });
            MessengerInstance.Register<ChangeChildViewModelMessage>(this, async vm => { await SelectedExcursionType.SetProperChildViewModel(vm.ViewModelindex); });
            MessengerInstance.Register<ExcursionCategoryChanged>(this, async index => { await SetProperViewModel(); });
        }

        public int IsBusyCounter { get; set; }

        private void ManageIsBusy(bool add)
        {
            _ = add ? IsBusyCounter++ : IsBusyCounter--;
            IsBusy = IsBusyCounter > 0;
        }

        #endregion Constructors

        #region Fields

        public List<ExcursionCategory_ViewModelBase> TemplateViewmodels { get; set; }

        private bool _IsBusy = false;

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

        public bool IsLoaded { get; set; }

        public RelayCommand LogOutCommand { get; set; }

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
                }
                RaisePropertyChanged();
            }
        }

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

        public string Username => Helpers.StaticResources.User.Name;

        #endregion Properties

        #region Methods

        public Task LoadAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task ReloadAsync()
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

        private async Task SetProperViewModel()
        {
            var index = -1;
            switch (SelectedTemplateIndex + 1)
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