using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Lists;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class OptionalsManagement_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public OptionalsManagement_ViewModel()
        {
            ShowCustomersCommand = new RelayCommand(async () => await ShowCustomers());
            PrintSofiaCommand = new RelayCommand(async () => await PrintSofia(), SelectedMainOptional != null);
            AddOptionalGotCommand = new RelayCommand<object>(async (par) => await AddOptional(par, 1), CanAddOptional);
            AddOptionalPaidCommand = new RelayCommand<object>(async (par) => await AddOptional(par, 2), CanAddOptional);
            AddOptionalNotPaidCommand = new RelayCommand<object>(async (par) => await AddOptional(par, 3), CanAddOptional);
            RecievedMoneyCommand = new RelayCommand<object>(async (par) => await ChangePaymentState(par), CanChange);
            DeleteOptionalCommand = new RelayCommand<object>(async (par) => await DeleteOption(par));
            Customers = new ObservableCollection<Customer>();
        }

        #endregion Constructors

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

        private CustomerOptional _SelectedOptional;

        private ICollectionView _View;

        #endregion Fields

        #region Properties

        public RelayCommand<object> AddOptionalGotCommand { get; set; }

        public RelayCommand<object> AddOptionalNotPaidCommand { get; set; }

        public RelayCommand<object> AddOptionalPaidCommand { get; set; }

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

        public RelayCommand<object> DeleteOptionalCommand { get; set; }

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

        public RelayCommand PrintSofiaCommand { get; set; }

        public RelayCommand<object> RecievedMoneyCommand { get; set; }

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

        private OptionalExcursion _SelectedMainOptional;

        public OptionalExcursion SelectedMainOptional
        {
            get
            {
                return _SelectedMainOptional;
            }

            set
            {
                if (_SelectedMainOptional == value)
                {
                    return;
                }

                _SelectedMainOptional = value;
                RaisePropertyChanged();
            }
        }

        public CustomerOptional SelectedOptional
        {
            get
            {
                return _SelectedOptional;
            }

            set
            {
                if (_SelectedOptional == value)
                {
                    return;
                }

                _SelectedOptional = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowCustomersCommand { get; set; }

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
            Optionals = new ObservableCollection<OptionalExcursion>((await Context.GetAllOptionalExcursionsAsync(CheckIn)).Where(e => e.Date == CheckIn));
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
                if (!SelectedCustomer.OptionalExcursions.Any(r => r.OptionalExcursion.Id == oe.Id))
                {
                    SelectedCustomer.OptionalExcursions.Add(
                        new CustomerOptional
                        {
                            Cost = oe.Cost,
                            Leader = SelectedLeader,
                            Note = oe.Note,
                            OptionalExcursion = oe,
                            PaymentType = (PaymentType)v
                        });
                }
                else
                    MessageBox.Show("Υπάρχει ήδη");
                oe.Note = "";
                oe.Cost = oe.FirstPrice;
                await Context.SaveAsync();
            }
        }

        private bool CanAddOptional(object arg)
        {
            return SelectedCustomer != null && SelectedLeader != null;
        }

        private bool CanChange(object a)
        {
            return SelectedOptional != null;
        }

        private async Task ChangePaymentState(object par)
        {
            if (par is string p && int.TryParse(p, out int pe))
            {
                SelectedOptional.PaymentType = (PaymentType)pe;
                await Context.SaveAsync();
            }
        }

        private bool CustomersFilter(object obj)
        {
            if (obj is Customer c)
            {
                if (Bus == null || Bus.Id == 0 || (c.BusGo != null && Bus.Id == c.BusGo.Id))
                {
                    return string.IsNullOrEmpty(SearchTerm) ||
                            (!string.IsNullOrEmpty(SearchTerm) &&
                            (c.Reservation.CustomersList.Any(t => t.Name.StartsWith(SearchTerm)) || c.Reservation.CustomersList.Any(t1 => t1.Surename.StartsWith(SearchTerm))));
                }
            }
            return false;
        }

        private async Task DeleteOption(object par)
        {
            if (par is CustomerOptional co && SelectedCustomer != null)
            {
                Context.Delete(co);
                await Context.SaveAsync();
                SelectedCustomer.OptionalExcursions.Remove(co);
            }
        }

        private async Task PrintSofia()
        {
            var sofia = (await Context.GetAllOptionalsAsync(SelectedMainOptional.Id))
                .OrderBy(y => y.Customer.HotelName)
                .ThenBy(u => u.Customer.Reservation.Id).ToList();
            using (DocumentsManagement vm = new DocumentsManagement(new GenericRepository()))
            {
                //await vm.PrintAllPhones();
                vm.PrintOptionals(sofia);
            }
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