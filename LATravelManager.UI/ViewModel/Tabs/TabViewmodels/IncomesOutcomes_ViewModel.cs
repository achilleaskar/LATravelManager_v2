using System;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class IncomesOutcomes_ViewModel : MyViewModelBase
    {
        #region Constructors

        public IncomesOutcomes_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Load();
        }

        #endregion Constructors

        #region Fields

        private AddIncomeOutcome_ViewModel _AddIncomeOutcome_ViewModel;

        private PreviewIncomesOutcomes_ViewModel _PreviewIncomesOutcomes_ViewModel;

        #endregion Fields

        #region Properties

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
        public MainViewModel MainViewModel { get; }

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

        #endregion Properties

        #region Methods

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            PreviewIncomesOutcomes_ViewModel = new PreviewIncomesOutcomes_ViewModel(MainViewModel);
            AddIncomeOutcome_ViewModel = new AddIncomeOutcome_ViewModel(MainViewModel);
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}