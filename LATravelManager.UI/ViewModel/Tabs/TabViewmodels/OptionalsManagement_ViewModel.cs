using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Lists;
using LATravelManager.Model.People;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class OptionalsManagement_ViewModel : MyViewModelBase
    {
        public OptionalsManagement_ViewModel(MainViewModel mainViewModel)
        {
            ShowCustomersCommand = new RelayCommand(async () => await ShowCustomers());
            Context = new GenericRepository();
            Load();
            MainViewModel = mainViewModel;
        }

        private GenericRepository _Context;

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

        private DateTime _CheckIn = DateTime.Today;

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
            }
        }

        private async Task ShowCustomers()
        {
            if (Context.HasChanges())
            {
                var answer = MessageBox.Show("Εχετε αλλαγές, να χαθούν?", "danger", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.Yes)
                {
                    Context = new GenericRepository();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Context = new GenericRepository();
            }
            Customers = new ObservableCollection<Customer>(await Context.GetAllCustomersAsync(CheckIn));
            Buses = new ObservableCollection<Bus>(await Context.GetAllBusesAsync(checkIn: CheckIn));
        }

        public RelayCommand ShowCustomersCommand { get; set; }

        private Customer _SelectedCustomer;

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

        private ObservableCollection<Customer> _Customers;

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

        private ICollectionView _View;

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

        private ObservableCollection<Bus> _Buses;

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

        private Bus _Bus;

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
                View.Refresh();
            }
        }

        private bool CustomersFilter(object obj)
        {
            if (obj is Customer c)
            {
                if (Bus == null || (c.Bus != null && Bus.Id == c.Bus.Id))
                {
                    return string.IsNullOrEmpty(SearchTerm) ||
                            (!string.IsNullOrEmpty(SearchTerm) &&
                            (c.Name.StartsWith(SearchTerm) || c.Surename.StartsWith(SearchTerm)));
                }
            }
            return false;
        }

        private string _SearchTerm;

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
                View.Refresh();
            }
        }

        private ObservableCollection<OptionalExcursion> _Optionals;

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

        public MainViewModel MainViewModel { get; }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}