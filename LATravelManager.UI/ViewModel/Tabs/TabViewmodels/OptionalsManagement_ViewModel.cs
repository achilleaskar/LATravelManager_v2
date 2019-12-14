using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Lists;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class OptionalsManagement_ViewModel : MyViewModelBaseAsync
    {
        #region Fields

        private Bus _Bus;

        private ObservableCollection<Bus> _Buses;

        private DateTime _CheckIn = DateTime.Today;

        private GenericRepository _Context;

        private ObservableCollection<Customer> _Customers;

        private ObservableCollection<Leader> _Leaders;
        private ObservableCollection<OptionalExcursion> _Optionals;

        private string _SearchTerm;

        private Customer _SelectedCustomer;

        private Leader _SelectedLeader;
        private ICollectionView _View;

        #endregion Fields

        #region Constructors

        public OptionalsManagement_ViewModel()
        {
            ShowCustomersCommand = new RelayCommand(async () => await ShowCustomers());
            PrintSofiaCommand = new RelayCommand(async () => await PrintSofia());
            AddOptionalGotCommand = new RelayCommand<object>(async (par) => await AddOptional(par, 0), CanAddOptional);
            AddOptionalPaidCommand = new RelayCommand<object>(async (par) => await AddOptional(par, 1), CanAddOptional);
            AddOptionalNotPaidCommand = new RelayCommand<object>(async (par) => await AddOptional(par, 2), CanAddOptional);

            Customers = new ObservableCollection<Customer>();
        }

        private async Task PrintSofia()
        {
            var sofia = (await Context.GetAllOptionalsAsync(1))
                .OrderBy(y => y.Customer.HotelName)
                .ThenBy(u => u.Customer.Reservation.Id).ToList();
            using (DocumentsManagement vm = new DocumentsManagement(new GenericRepository()))
            {
                //await vm.PrintAllPhones();
                vm.PrintOptionals(sofia);
            }

        }

        #endregion Constructors

        #region Properties

        public RelayCommand<object> AddOptionalGotCommand { get; set; }
        public RelayCommand<object> AddOptionalPaidCommand { get; set; }
        public RelayCommand<object> AddOptionalNotPaidCommand { get; set; }

        public Bus Bus
        {
            get
            {
                return _Bus;
            }

            set
            {
                if (_Bus == value)
                {
                    return;
                }

                _Bus = value;

                RaisePropertyChanged();
                if (View != null)
                    View.Refresh();
            }
        }

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
            }
        }

        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }

                _CheckIn = value;
                RaisePropertyChanged();

                Customers.Clear();
                Context = null;
            }
        }

        public GenericRepository Context
        {
            get
            {
                return _Context;
            }

            set
            {
                if (_Context == value)
                {
                    return;
                }

                _Context = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get
            {
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                RaisePropertyChanged();

                View = CollectionViewSource.GetDefaultView(Customers);
                View.Filter = CustomersFilter;
            }
        }

        public ObservableCollection<Leader> Leaders
        {
            get
            {
                return _Leaders;
            }

            set
            {
                if (_Leaders == value)
                {
                    return;
                }

                _Leaders = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OptionalExcursion> Optionals
        {
            get
            {
                return _Optionals;
            }

            set
            {
                if (_Optionals == value)
                {
                    return;
                }

                _Optionals = value;
                RaisePropertyChanged();
            }
        }

        public string SearchTerm
        {
            get
            {
                return _SearchTerm;
            }

            set
            {
                if (_SearchTerm == value)
                {
                    return;
                }

                _SearchTerm = value.ToUpper();
                RaisePropertyChanged();
                if (View != null)
                    View.Refresh();
            }
        }

        public Customer SelectedCustomer
        {
            get
            {
                return _SelectedCustomer;
            }

            set
            {
                if (_SelectedCustomer == value)
                {
                    return;
                }

                _SelectedCustomer = value;
                RaisePropertyChanged();
            }
        }

        public Leader SelectedLeader
        {
            get
            {
                return _SelectedLeader;
            }

            set
            {
                if (_SelectedLeader == value)
                {
                    return;
                }

                _SelectedLeader = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowCustomersCommand { get; set; }
        public RelayCommand PrintSofiaCommand { get; set; }

        public ICollectionView View
        {
            get
            {
                return _View;
            }

            set
            {
                if (_View == value)
                {
                    return;
                }

                _View = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Optionals = new ObservableCollection<OptionalExcursion>(await Context.GetAllOptionalExcursionsAsync(CheckIn));
            Leaders = new ObservableCollection<Leader>((await Context.GetAllAsync<Leader>()).OrderBy(l => l.Name));
            Buses = new ObservableCollection<Bus>(await Context.GetAllBusesAsync(checkIn: CheckIn));
            Buses.Insert(0, new Bus { Vehicle = new Vehicle { Name = "Όλες" } });
            foreach (var o in Optionals)
            {
                o.FirstPrice = o.Cost;
            }
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        private async Task AddOptional(object par, int v)
        {
            if (par is OptionalExcursion oe)
            {
                SelectedCustomer.OptionalExcursions.Add(
                    new CustomerOptional
                    {
                        Cost = oe.Cost,
                        Leader = SelectedLeader,
                        Note = oe.Note,
                        OptionalExcursion = oe,
                        

                    });
                oe.Note = "";
                oe.Cost = oe.FirstPrice;
                await Context.SaveAsync();
            }
        }

        private bool CanAddOptional(object arg)
        {
            return SelectedCustomer != null && SelectedLeader != null;
        }

        private bool CustomersFilter(object obj)
        {
            if (obj is Customer c)
            {
                if (Bus == null || Bus.Id == 0 || (c.Bus != null && Bus.Id == c.Bus.Id))
                {
                    return string.IsNullOrEmpty(SearchTerm) ||
                            (!string.IsNullOrEmpty(SearchTerm) &&
                            (c.Reservation.CustomersList.Any(t => t.Name.StartsWith(SearchTerm)) || c.Reservation.CustomersList.Any(t1 => t1.Surename.StartsWith(SearchTerm))));
                }
            }
            return false;
        }

        private async Task ShowCustomers()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Context == null)
            {
                Context = new GenericRepository();
                await LoadAsync();
                Mouse.OverrideCursor = Cursors.Arrow;
                return;
            }
            if (Context.HasChanges())
            {
                var answer = MessageBox.Show("Εχετε αλλαγές, να χαθούν?", "danger", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.Yes)
                {
                    Context = new GenericRepository();
                    await LoadAsync();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Context = new GenericRepository();
                await LoadAsync();

            }
            Customers = new ObservableCollection<Customer>(await Context.GetAllCustomersAsync(CheckIn));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }
}