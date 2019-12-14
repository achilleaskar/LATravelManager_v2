using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class Management_ViewModel : MyViewModelBase
    {
        private readonly MainViewModel MainViewModel;

        public Management_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            InfoViewModel = new Info_ViewModel(MainViewModel);
            InfoViewModel.Load();
            ListManagement = new ListManagement_ViewModel(MainViewModel);
            OptionalsManagement = new OptionalsManagement_ViewModel();
        }

        private int _SelectedIndex;

        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }

            set
            {
                if (_SelectedIndex == value)
                {
                    return;
                }

                _SelectedIndex = value;
                RaisePropertyChanged();
            }
        }

        private Info_ViewModel _InfoViewModel;

        public Info_ViewModel InfoViewModel
        {
            get
            {
                return _InfoViewModel;
            }

            set
            {
                if (_InfoViewModel == value)
                {
                    return;
                }

                _InfoViewModel = value;
                RaisePropertyChanged();
            }
        }

        private ListManagement_ViewModel _ListManagementViewModel;

        public ListManagement_ViewModel ListManagementViewModel
        {
            get
            {
                return _ListManagementViewModel;
            }

            set
            {
                if (_ListManagementViewModel == value)
                {
                    return;
                }

                _ListManagementViewModel = value;
                RaisePropertyChanged();
            }
        }




        private OptionalsManagement_ViewModel _OptionalsManagement;


        public OptionalsManagement_ViewModel OptionalsManagement
        {
            get
            {
                return _OptionalsManagement;
            }

            set
            {
                if (_OptionalsManagement == value)
                {
                    return;
                }

                _OptionalsManagement = value;
                RaisePropertyChanged();
            }
        }

        private ListManagement_ViewModel _ListManagement;

        public ListManagement_ViewModel ListManagement
        {
            get
            {
                return _ListManagement;
            }

            set
            {
                if (_ListManagement == value)
                {
                    return;
                }

                _ListManagement = value;
                RaisePropertyChanged();
            }
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }
    }
}