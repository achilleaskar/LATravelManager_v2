using System;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class IncomesOutcomes_ViewModel : MyViewModelBase
    {
        public IncomesOutcomes_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Load();
        }

        private AddIncomeOutcome_ViewModel _AddIncomeOutcome_ViewModel;

        public AddIncomeOutcome_ViewModel AddIncomeOutcome_ViewModel
        {
            get
            {
                return _AddIncomeOutcome_ViewModel;
            }

            set
            {
                if (_AddIncomeOutcome_ViewModel == value)
                {
                    return;
                }

                _AddIncomeOutcome_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        private PreviewIncomesOutcomes_ViewModel _PreviewIncomesOutcomes_ViewModel;

        public PreviewIncomesOutcomes_ViewModel PreviewIncomesOutcomes_ViewModel
        {
            get
            {
                return _PreviewIncomesOutcomes_ViewModel;
            }

            set
            {
                if (_PreviewIncomesOutcomes_ViewModel == value)
                {
                    return;
                }

                _PreviewIncomesOutcomes_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            PreviewIncomesOutcomes_ViewModel = new PreviewIncomesOutcomes_ViewModel(MainViewModel);
            AddIncomeOutcome_ViewModel = new AddIncomeOutcome_ViewModel(MainViewModel);
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}