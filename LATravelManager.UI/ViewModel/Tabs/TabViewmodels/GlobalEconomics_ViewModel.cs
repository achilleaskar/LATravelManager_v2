using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class GlobalEconomics_ViewModel : MyViewModelBase
    {
        public GlobalEconomics_ViewModel()
        {
        }

        public GlobalEconomics_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            EconomicData_ViewModel = new EconomicData_ViewModel(MainViewModel);
            IncomesOutcomes_ViewModel = new IncomesOutcomes_ViewModel(MainViewModel);
            TotalEarns_ViewModel = new TotalEarns_ViewModel(MainViewModel);
            Remaining_ViewModel = new Remaining_ViewModel(MainViewModel);
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

        private Remaining_ViewModel _Remaining_ViewModel;

        public Remaining_ViewModel Remaining_ViewModel
        {
            get
            {
                return _Remaining_ViewModel;
            }

            set
            {
                if (_Remaining_ViewModel == value)
                {
                    return;
                }

                _Remaining_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        private IncomesOutcomes_ViewModel _Incomes_ViewModel;

        public IncomesOutcomes_ViewModel IncomesOutcomes_ViewModel
        {
            get
            {
                return _Incomes_ViewModel;
            }

            set
            {
                if (_Incomes_ViewModel == value)
                {
                    return;
                }

                _Incomes_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        private EconomicData_ViewModel _EconomicData_ViewModel;

        public EconomicData_ViewModel EconomicData_ViewModel
        {
            get
            {
                return _EconomicData_ViewModel;
            }

            set
            {
                if (_EconomicData_ViewModel == value)
                {
                    return;
                }

                _EconomicData_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        private TotalEarns_ViewModel _TotalEarns_ViewModel;

        public TotalEarns_ViewModel TotalEarns_ViewModel
        {
            get
            {
                return _TotalEarns_ViewModel;
            }

            set
            {
                if (_TotalEarns_ViewModel == value)
                {
                    return;
                }

                _TotalEarns_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
        }

        public override void Reload()
        {
        }
    }
}