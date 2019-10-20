using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class Management_ViewModel : MyViewModelBase
    {
        private MainViewModel MainViewModel;

        public Management_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
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
            InfoViewModel = new Info_ViewModel(MainViewModel);
            InfoViewModel.Load();
            ListManagement = new ListManagement_ViewModel(MainViewModel);


        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }
    }
}