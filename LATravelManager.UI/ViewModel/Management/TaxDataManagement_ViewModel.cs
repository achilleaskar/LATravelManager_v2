using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Extensions;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace LATravelManager.UI.ViewModel.Management
{
    public class TaxDataManagement_ViewModel : MyViewModelBaseAsync
    {

        #region Constructors

        public TaxDataManagement_ViewModel(BasicDataManager basicDataManager)
        {
            this.BasicDataManager = basicDataManager;
            DeleteSelectedCompanyCommand = new RelayCommand(async () => await DeleteSelectedCompany(), SelectedCompany != null && SelectedCompany.Id > 0);
            SaveChangesCommand = new RelayCommand(async () => await SaveCompanyChangesAsync(), CanSaveCompanyChanges);
            
            
            SaveRecieptSeriesChangesCommand = new RelayCommand(async () => await SaveRecieptSeriesChangesAsync(), CanSaveRecieptSeriesChanges);
            
            
            CreateNewCompanyCommand = new RelayCommand(CreateNewCompany);
            CreateNewRecieptSerieCommand = new RelayCommand(CreateNewRecieptSerie);
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<City> _Cities;
        private ObservableCollection<Company> _Companies;
        private ObservableCollection<CompanyActivity> _CompanyActivities;
        private string _CompanyError;
        private ObservableCollection<Country> _Countries;
        private Company _SelectedCompany;
        private RecieptSeries _SelectedSerie;
        private int _SelectedTabIndex;

        private ObservableCollection<RecieptSeries> _Series;

        private Dictionary<string, string> Companyerrors;

        #endregion Fields

        #region Properties

        public BasicDataManager BasicDataManager { get; set; }

        public ObservableCollection<City> Cities
        {
            get
            {
                return _Cities;
            }

            set
            {
                if (_Cities == value)
                {
                    return;
                }

                _Cities = value;
                RaisePropertyChanged();
            }
        }

        public CollectionView CitiesCV { get; set; }

        public ObservableCollection<Company> Companies
        {
            get
            {
                return _Companies;
            }

            set
            {
                if (_Companies == value)
                {
                    return;
                }

                _Companies = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CompanyActivity> CompanyActivities
        {
            get
            {
                return _CompanyActivities;
            }

            set
            {
                if (_CompanyActivities == value)
                {
                    return;
                }

                _CompanyActivities = value;
                RaisePropertyChanged();
            }
        }

        public string CompanyError
        {
            get
            {
                return _CompanyError;
            }

            set
            {
                if (_CompanyError == value)
                {
                    return;
                }

                _CompanyError = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Country> Countries
        {
            get
            {
                return _Countries;
            }

            set
            {
                if (_Countries == value)
                {
                    return;
                }

                _Countries = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CreateNewCompanyCommand { get; set; }

        public RelayCommand CreateNewRecieptSerieCommand { get; set; }

        public RelayCommand DeleteSelectedCompanyCommand { get; set; }

        public bool NewSerie => SelectedSerie != null && SelectedSerie.Id == 0;

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand SaveRecieptSeriesChangesCommand { get; set; }

        public Company SelectedCompany
        {
            get
            {
                return _SelectedCompany;
            }

            set
            {
                var origValue = _SelectedCompany;
                if (_SelectedCompany == value)
                {
                    return;
                }
                _SelectedCompany = value;

                if (BasicDataManager.HasChanges() || SelectedCompany?.Id == 0)
                {
                    MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές. Εάν συνεχίσετε θα απορριφθούν. Συνέχεια?", "Προσοχή", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        BasicDataManager.Context.RollBack();
                        RaisePropertyChanged();
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                       new Action(() =>
                       {
                           _SelectedCompany = origValue;
                           RaisePropertyChanged();
                       }),
                       DispatcherPriority.ContextIdle, null);

                        // Exit early.
                        return;
                    }
                }
                else
                {
                    RaisePropertyChanged();
                }
            }
        }

        public RecieptSeries SelectedSerie
        {
            get
            {
                return _SelectedSerie;
            }

            set
            {
                if (_SelectedSerie == value)
                {
                    return;
                }

                _SelectedSerie = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedTabIndex
        {
            get
            {
                return _SelectedTabIndex;
            }

            set
            {
                if (_SelectedTabIndex == value)
                {
                    return;
                }

                if (BasicDataManager.HasChanges())// || SelectedCompany?.Id == 0)
                {
                    MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές. Εάν συνεχίσετε θα απορριφθούν. Συνέχεια?", "Προσοχή", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        BasicDataManager.Context.RollBack();
                        SelectedCompany = null;
                        SelectedSerie = null;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    RaisePropertyChanged();
                }

                _SelectedTabIndex = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<RecieptSeries> Series
        {
            get
            {
                return _Series;
            }

            set
            {
                if (_Series == value)
                {
                    return;
                }

                _Series = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public async Task GetAllCompaniesAsync(bool allProperties = false)
        {
            MessengerInstance.Send(new IsBusyChangedMessage(true));
            Companies = new ObservableCollection<Company>(await BasicDataManager.Context.GetAllCompaniesAsync(allProperties));
            MessengerInstance.Send(new IsBusyChangedMessage(false));
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Companies = new ObservableCollection<Company>((await BasicDataManager.Context.GetAllAsync<Company>()).OrderBy(c => c.CompanyName));
            CompanyActivities = new ObservableCollection<CompanyActivity>((await BasicDataManager.Context.GetAllAsync<CompanyActivity>()).OrderBy(a => a.Name));
            Cities = new ObservableCollection<City>(BasicDataManager.Cities);
            CitiesCV = (CollectionView)CollectionViewSource.GetDefaultView(Cities);
            CitiesCV.Filter = CitiesFilter;
            Countries = BasicDataManager.Countries;
            Series = new ObservableCollection<RecieptSeries>((await BasicDataManager.Context.GetAllAsync<RecieptSeries>()).OrderBy(a => a.DateStarted));
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        internal void UpdateCities()
        {
            CitiesCV.Refresh();
        }

        private bool CanSaveCompanyChanges()
        {
            if (SelectedCompany != null && SelectedCompany.Id == 0)
            {
                SelectedCompany.IsValid(ref Companyerrors);
                if (CompanyError.Length > 0)
                {
                    CompanyError = Companyerrors.First().Value;
                }
            }
            return BasicDataManager.HasChanges();
        }

        private bool CanSaveRecieptSeriesChanges()
        {
            return true;
        }

        private bool CitiesFilter(object obj)
        {
            return obj is City c && SelectedCompany != null && c.Country.Id == SelectedCompany.Country.Id;
        }

        private void CreateNewCompany()
        {
            if (BasicDataManager.Context.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές. Εάν συνεχίσετε θα απορριφθούν. Συνέχεια?", "Προσοχή", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    BasicDataManager.Context.RollBack();
                    SelectedCompany = new Company();
                }
            }
        }

        private void CreateNewRecieptSerie()
        {
            throw new NotImplementedException();
        }
        private Task DeleteSelectedCompany()
        {
            throw new NotImplementedException();
        }

        private async Task SaveCompanyChangesAsync()
        {
            if (SelectedCompany.Id == 0)
            {
                BasicDataManager.Add(SelectedCompany);
                await BasicDataManager.SaveAsync();
            }
            else
            {
                await BasicDataManager.SaveAsync();
            }
        }

        private Task SaveRecieptSeriesChangesAsync()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}