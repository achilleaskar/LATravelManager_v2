using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel
{
    public class MainUserControl_ViewModel : ViewModelBase, IViewModel
    {
        public MainUserControl_ViewModel()
        {
            LogOutCommand = new RelayCommand(TryLogOut, CanLogout);
            MessengerInstance.Register<IsBusyChangedMessage>(this, msg => { IsBusy = msg.IsVisible; });
            MessengerInstance.Register<SetChildViewModelMessage>(this, tab => { SelectedExcursionType.SelectedChildViewModel = tab.Viewmodel; });
            MessengerInstance.Register<ChangeViewModelMessage>(this, name => { SelectedExcursionType.SetProperChildViewModel(name.NameOfViewModel); });
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

        public RelayCommand LogOutCommand { get; set; }

        public string Username => Helpers.StaticResources.User.Name;

        private bool _IsBusy = false;

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

        public Task LoadAsync()
        {
            throw new System.NotImplementedException();
        }

        private int _SelectedTemplateIndex;
        private List<ExcursionCategory> _Templates;
        private ExcursionCategory_ViewModelBase _SelectedExcursionType;

        public List<ExcursionCategory> Templates
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
                    SetProperViewModel();
                }
                RaisePropertyChanged();
            }
        }

        public bool IsLoaded { get; set; }

        private void SetProperViewModel()
        {
            switch (SelectedTemplateIndex + 1)
            {
                case 1:
                    if (!SimpleIoc.Default.IsRegistered<BanskoParent_ViewModel>())
                        SimpleIoc.Default.Register<BanskoParent_ViewModel>();
                    SelectedExcursionType = ServiceLocator.Current.GetInstance<BanskoParent_ViewModel>();
                    break;

                case 2:
                    if (!SimpleIoc.Default.IsRegistered<GroupExcursion_ViewModel>())
                        SimpleIoc.Default.Register<GroupExcursion_ViewModel>();
                    SelectedExcursionType = ServiceLocator.Current.GetInstance<GroupExcursion_ViewModel>();
                    break;

                case 3:
                    if (!SimpleIoc.Default.IsRegistered<PersonalExcursion_ViewModel>())
                        SimpleIoc.Default.Register<PersonalExcursion_ViewModel>();
                    SelectedExcursionType = ServiceLocator.Current.GetInstance<PersonalExcursion_ViewModel>();
                    break;

                case 4:
                    if (!SimpleIoc.Default.IsRegistered<ThirdPartyExcursions_ViewModel>())
                        SimpleIoc.Default.Register<ThirdPartyExcursions_ViewModel>();
                    SelectedExcursionType = ServiceLocator.Current.GetInstance<ThirdPartyExcursions_ViewModel>();
                    break;

                case 5:
                    if (!SimpleIoc.Default.IsRegistered<Skiathos_ViewModel>())
                        SimpleIoc.Default.Register<Skiathos_ViewModel>();
                    SelectedExcursionType = ServiceLocator.Current.GetInstance<Skiathos_ViewModel>();
                    break;
            }
            //set to default tab
            if (SelectedExcursionType is ExcursionCategory_ViewModelBase)
            {
                Messenger.Default.Send(new ResetNavigationTabsMessage());
                (SelectedExcursionType as ExcursionCategory_ViewModelBase).SetProperChildViewModel(0);
            }
        }

        public Task ReloadAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}