using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class PreviewIncomesOutcomes_ViewModel : MyViewModelBase
    {
        public PreviewIncomesOutcomes_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ShowTransactionsCommand = new RelayCommand(async () => { await ShowTransactions(); }, CanShowTransactions);
            Load();
        }

        GenericRepository Context { get; set; }

        private ObservableCollection<Transaction> _FilteredTransactions;

        public ObservableCollection<Transaction> FilteredTransactions
        {
            get
            {
                return _FilteredTransactions;
            }

            set
            {
                if (_FilteredTransactions == value)
                {
                    return;
                }

                _FilteredTransactions = value;
                RaisePropertyChanged();
            }
        }

        private Transaction _SelectedTransaction;


        public Transaction SelectedTransaction
        {
            get
            {
                return _SelectedTransaction;
            }

            set
            {
                if (_SelectedTransaction == value)
                {
                    return;
                }

                _SelectedTransaction = value;
                RaisePropertyChanged();
            }
        }
        private async Task ShowTransactions()
        {
            FilteredTransactions = new ObservableCollection<Transaction>((await Context.GetAllTransactionsFiltered()));
        }

        private bool CanShowTransactions()
        {
            return true;
        }

        public RelayCommand ShowTransactionsCommand { get; set; }
        public MainViewModel MainViewModel { get; }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Context = new GenericRepository();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}