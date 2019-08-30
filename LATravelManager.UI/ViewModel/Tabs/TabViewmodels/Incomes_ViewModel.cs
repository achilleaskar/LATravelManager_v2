using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class Incomes_ViewModel : MyViewModelBase
    {
        public Incomes_ViewModel(MainViewModel mainViewModel)
        {
        }

        private AddIncome_ViewModel _AddIncome_ViewModel;

        public AddIncome_ViewModel AddIncome_ViewModel
        {
            get
            {
                return _AddIncome_ViewModel;
            }

            set
            {
                if (_AddIncome_ViewModel == value)
                {
                    return;
                }

                _AddIncome_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        private PreviewIncomes_ViewModel _PreviewIncomes_ViewModel;

        public PreviewIncomes_ViewModel PreviewIncomes_ViewModel
        {
            get
            {
                return _PreviewIncomes_ViewModel;
            }

            set
            {
                if (_PreviewIncomes_ViewModel == value)
                {
                    return;
                }

                _PreviewIncomes_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            PreviewIncomes_ViewModel = new PreviewIncomes_ViewModel();
            AddIncome_ViewModel = new AddIncome_ViewModel();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}