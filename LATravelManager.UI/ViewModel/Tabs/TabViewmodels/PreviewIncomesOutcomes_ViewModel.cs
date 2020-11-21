using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Lists;
using LATravelManager.Model.People;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class PreviewIncomesOutcomes_ViewModel : MyViewModelBase
    {
        #region Constructors

        public PreviewIncomesOutcomes_ViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            ShowTransactionsCommand = new RelayCommand(async () => { await ShowTransactions(); }, CanShowTransactions);
            DeleteTransactionCommand = new RelayCommand(async () => { await DeleteTransaction(); }, CanDeleteTransaction);

            UpdateBusesCommand = new RelayCommand(async () => { await UpdateBuses(); }, CanUpdateBuses);
            From = DateTime.Today;
            Load();
        }

        private async Task DeleteTransaction()
        {
            var tmp = SelectedTransaction;
            FilteredTransactions.Remove(SelectedTransaction);
            Context.Delete(tmp);
            await Context.SaveAsync();
        }

        private bool CanDeleteTransaction()
        {
            return SelectedTransaction != null;
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Bus> _Buses;
        private ICollectionView _BusesCollectionView;
        private bool _EnableFromFilter;
        private bool _EnableToFilter;
        private ObservableCollection<Excursion> _Excursions;
        private ICollectionView _ExcursionsCollectionView;
        private ObservableCollection<Transaction> _FilteredTransactions;
        private DateTime _From;
        private Excursion _SelectedExcursion;
        private DateTime _To;
        private string _Total;
        private Transaction _Transaction;

        private ObservableCollection<User> _Users;

        #endregion Fields

        #region Properties

        public ObservableCollection<Bus> Buses
        {
            get
            {
                return _Buses;
            }

            set
            {
                if (_Buses == value)
                {
                    return;
                }

                _Buses = value;
                RaisePropertyChanged();
                BusesCollectionView = CollectionViewSource.GetDefaultView(Buses);
                BusesCollectionView.Filter = BusesFilter;
            }
        }

        public ICollectionView BusesCollectionView
        {
            get
            {
                return _BusesCollectionView;
            }

            set
            {
                if (_BusesCollectionView == value)
                {
                    return;
                }

                _BusesCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableFromFilter
        {
            get
            {
                return _EnableFromFilter;
            }

            set
            {
                if (_EnableFromFilter == value)
                {
                    return;
                }

                _EnableFromFilter = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableToFilter
        {
            get
            {
                return _EnableToFilter;
            }

            set
            {
                if (_EnableToFilter == value)
                {
                    return;
                }

                _EnableToFilter = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Excursion> Excursions
        {
            get
            {
                return _Excursions;
            }

            set
            {
                if (_Excursions == value)
                {
                    return;
                }
                _Excursions = value;

                if (Excursions != null && !Excursions.Any(e => e.Id == 0))
                {
                    Excursions.Insert(0, new Excursion { ExcursionDates = new ObservableCollection<ExcursionDate> { new ExcursionDate { CheckIn = new DateTime(), CheckOut = DateTime.MaxValue } }, Name = "Όλες", Id = 0 });
                }

                ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);
                ExcursionsCollectionView.SortDescriptions.Add(new SortDescription("LastDate", ListSortDirection.Descending));

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ExcursionsCollectionView));
            }
        }

        public ICollectionView ExcursionsCollectionView
        {
            get
            {
                return _ExcursionsCollectionView;
            }

            set
            {
                if (_ExcursionsCollectionView == value)
                {
                    return;
                }

                _ExcursionsCollectionView = value;
                RaisePropertyChanged();
            }
        }

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

        public DateTime From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                _From = value;
                if (To < value)
                {
                    To = value;
                }
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public Excursion SelectedExcursion
        {
            get
            {
                return _SelectedExcursion;
            }

            set
            {
                if (_SelectedExcursion == value)
                {
                    return;
                }

                _SelectedExcursion = value;
                RaisePropertyChanged();
            }
        }

        private int _UserIndexFilter;

        public int UserIndexFilter
        {
            get
            {
                return _UserIndexFilter;
            }

            set
            {
                if (_UserIndexFilter == value)
                {
                    return;
                }

                _UserIndexFilter = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowTransactionsCommand { get; set; }
        public RelayCommand DeleteTransactionCommand { get; set; }

        public DateTime To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }

                _To = value;
                if (value < From)
                {
                    From = value;
                }
                RaisePropertyChanged();
            }
        }

        public string Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
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

        public Transaction Transaction
        {
            get
            {
                return _Transaction;
            }

            set
            {
                if (_Transaction == value)
                {
                    return;
                }

                _Transaction = value;
                RaisePropertyChanged();
                _Transaction.PropertyChanged += Transaction_PropertyChanged;
            }
        }

        public RelayCommand UpdateBusesCommand { get; set; }

        public ObservableCollection<User> Users
        {
            get
            {
                return _Users;
            }

            set
            {
                if (_Users == value)
                {
                    return;
                }

                _Users = value;
                RaisePropertyChanged();
            }
        }

        private GenericRepository Context { get; set; }

        #endregion Properties

        #region Methods

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Context = new GenericRepository();
            Transaction = new Transaction { Editing = true, FiltersEnabled = false };
            Excursions = new ObservableCollection<Excursion>(MainViewModel.BasicDataManager.Excursions.Where(i => i.Id > 0));
            Buses = new ObservableCollection<Bus>();
            Users = MainViewModel.BasicDataManager.Users;
            Total = $"Σύνολο: {  0:C2}";
            Incomes = $"Έσοδα:  {  0:C2}";
            Outcomes = $"Έξοδα:  { 0:C2}";
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private bool BusesFilter(object obj)
        {
            return obj is Bus b && SelectedExcursion != null && (b.Id == 0 || b.Excursion.Id == SelectedExcursion.Id);
        }

        private bool CanShowTransactions()
        {
            return true;
        }

        private bool CanUpdateBuses()
        {
            return SelectedExcursion != null && SelectedExcursion is Excursion b && b.Id > 0;
        }

        private async Task ShowTransactions()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Context = new GenericRepository();

            FilteredTransactions = new ObservableCollection<Transaction>((await Context.GetAllTransactionsFiltered(
                transaction: Transaction.FiltersEnabled ? Transaction : null,
                excursion: (ExcursionsCollectionView.CurrentItem != null && ExcursionsCollectionView.CurrentItem is Excursion e && e.Id > 0) ? e : null,
                bus: (BusesCollectionView.CurrentItem != null && BusesCollectionView.CurrentItem is Bus b && b.Id > 0) ? b : null,
                from: EnableFromFilter ? (DateTime?)From : null,
                to: EnableToFilter ? (DateTime?)To : null,
                user: UserIndexFilter > 0 ? Users[UserIndexFilter - 1] : null)));

            decimal incomes = 0;
            decimal outcomes = 0;
            foreach (var tran in FilteredTransactions)
            {
                if (tran.TransactionType == TransactionType.Expense)
                    outcomes += tran.Amount;
                else
                    incomes += tran.Amount;
            }

            Total = $"Σύνολο: {  incomes - outcomes:C2}";
            Incomes = $"Έσοδα:  {  incomes:C2}";
            Outcomes = $"Έξοδα:  { outcomes:C2}";
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private string _Incomes;

        public string Incomes
        {
            get
            {
                return _Incomes;
            }

            set
            {
                if (_Incomes == value)
                {
                    return;
                }

                _Incomes = value;
                RaisePropertyChanged();
            }
        }

        private string _Outcomes;

        public string Outcomes
        {
            get
            {
                return _Outcomes;
            }

            set
            {
                if (_Outcomes == value)
                {
                    return;
                }

                _Outcomes = value;
                RaisePropertyChanged();
            }
        }

        private Bus _SelectedBus;

        public Bus SelectedBus
        {
            get
            {
                return _SelectedBus;
            }

            set
            {
                if (_SelectedBus == value)
                {
                    return;
                }

                _SelectedBus = value;
                RaisePropertyChanged();
            }
        }

        private void Transaction_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Transaction));
        }

        private async Task UpdateBuses()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Buses = new ObservableCollection<Bus>(await Context.GetAllBusesAsync(SelectedExcursion != null ? SelectedExcursion.Id : 0));
            Buses.Insert(0, new Bus { Id = 0, Vehicle = new Vehicle { Name = "Όλα" } });
            SelectedBus = Buses.First();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }
}