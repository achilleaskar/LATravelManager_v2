using EnumsNET;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
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

        private ObservableCollection<City> _AddressCities;
        private CollectionView _AddressCitiesCV;
        private ObservableCollection<City> _BillCities;
        private CollectionView _BillCitiesCV;
        private ObservableCollection<Company> _Companies;
        private CollectionView _CompaniesCV;
        private ObservableCollection<CompanyActivity> _CompanyActivities;
        private string _CompanyFilterText;
        private ObservableCollection<Country> _Countries;
        private Company _SelectedCompany;
        private RecieptSeries _SelectedSerie;
        private int _SelectedTabIndex;

        private ObservableCollection<RecieptSeries> _Series;

        #endregion Fields

        #region Properties

        public ObservableCollection<City> AddressCities
        {
            get
            {
                return _AddressCities;
            }

            set
            {
                if (_AddressCities == value)
                {
                    return;
                }

                _AddressCities = value;
                RaisePropertyChanged();
            }
        }

        public CollectionView AddressCitiesCV
        {
            get
            {
                return _AddressCitiesCV;
            }

            set
            {
                if (_AddressCitiesCV == value)
                {
                    return;
                }

                _AddressCitiesCV = value;
                RaisePropertyChanged();
            }
        }

        public BasicDataManager BasicDataManager { get; set; }

        public ObservableCollection<City> BillCities
        {
            get
            {
                return _BillCities;
            }

            set
            {
                if (_BillCities == value)
                {
                    return;
                }

                _BillCities = value;
                RaisePropertyChanged();
            }
        }

        public CollectionView BillCitiesCV
        {
            get
            {
                return _BillCitiesCV;
            }

            set
            {
                if (_BillCitiesCV == value)
                {
                    return;
                }

                _BillCitiesCV = value;
                RaisePropertyChanged();
            }
        }

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
                CompaniesCV = (CollectionView)CollectionViewSource.GetDefaultView(Companies);
                CompaniesCV.Filter = CompaniesFilter;
                RaisePropertyChanged();
            }
        }

        public CollectionView CompaniesCV
        {
            get
            {
                return _CompaniesCV;
            }

            set
            {
                if (_CompaniesCV == value)
                {
                    return;
                }

                _CompaniesCV = value;
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

        public string CompanyFilterText
        {
            get
            {
                return _CompanyFilterText;
            }

            set
            {
                if (_CompanyFilterText == value)
                {
                    return;
                }

                _CompanyFilterText = value.ToUpper();
                if (Companies != null && CompaniesCV != null)
                    CompaniesCV.Refresh();
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

                if (BasicDataManager.HasChanges())
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
                RaisePropertyChanged(nameof(IsSerieNew));
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

        private void CheckValues()
        {
            foreach (var prop in typeof(Company).GetProperties())
            {
                if (prop.PropertyType != typeof(string) || !prop.CanWrite)
                {
                    continue;
                }

                var val = prop.GetValue(SelectedCompany, null);
                if (prop.Name == nameof(SelectedCompany.Phone1) || prop.Name == nameof(SelectedCompany.Phone2) || prop.Name == nameof(SelectedCompany.MobilePhone))
                {
                    prop.SetValue(SelectedCompany, val?.ToString().Replace(" ", ""));
                }
                else
                {
                    prop.SetValue(SelectedCompany, val?.ToString().Trim());
                }
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
            Companies = new ObservableCollection<Company>((await BasicDataManager.Context.GetAllAsync<Company>(c => !c.Disabled)).OrderBy(c => c.CompanyName));
            CompanyActivities = new ObservableCollection<CompanyActivity>((await BasicDataManager.Context.GetAllAsync<CompanyActivity>()).OrderBy(a => a.Name));
            Series = new ObservableCollection<RecieptSeries>((await BasicDataManager.Context.GetAllAsync<RecieptSeries>(s=>s.AgencyId==StaticResources.User.BaseLocation)).OrderBy(a => a.DateStarted));

            Countries = BasicDataManager.Countries;

            AddressCities = new ObservableCollection<City>(BasicDataManager.Cities);
            AddressCitiesCV = (CollectionView)CollectionViewSource.GetDefaultView(AddressCities);
            AddressCitiesCV.Filter = CitiesFilter;

            BillCities = new ObservableCollection<City>(BasicDataManager.Cities);
            BillCitiesCV = (CollectionView)CollectionViewSource.GetDefaultView(BillCities);
            BillCitiesCV.Filter = CitiesFilter;
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        internal void UpdateCities()
        {
            BillCitiesCV.Refresh();
            AddressCitiesCV.Refresh();
        }

        private bool CanSaveCompanyChanges()
        {
            return SelectedCompany != null && SelectedCompany.IsValidToPrint() && (BasicDataManager.HasChanges() || SelectedCompany.Id == 0);
        }

        private bool CanSaveRecieptSeriesChanges()
        {
            return SelectedSerie != null && SerieDataAreValid() && (SelectedSerie.Id == 0 || BasicDataManager.HasChanges());
        }

        private bool CitiesFilter(object obj)
        {
            return SelectedCompany == null || (obj is City c && SelectedCompany != null && SelectedCompany.Country != null && c.Country.Id == SelectedCompany.Country.Id);
        }

        private bool CompaniesFilter(object obj)
        {
            return obj is Company c && !c.Disabled && (string.IsNullOrEmpty(CompanyFilterText) || c.CompanyName?.Contains(CompanyFilterText) == true || c.Name?.Contains(CompanyFilterText)==true || c.LastName?.Contains(CompanyFilterText) == true);
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
            else
                SelectedCompany = new Company();
        }

        private void CreateNewRecieptSerie()
        {
            if (BasicDataManager.Context.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές. Εάν συνεχίσετε θα απορριφθούν. Συνέχεια?", "Προσοχή", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    BasicDataManager.Context.RollBack();
                    SelectedSerie = new RecieptSeries { DateStarted = DateTime.Today };
                }
            }
            else
                SelectedSerie = new RecieptSeries { DateStarted = DateTime.Today };
        }

        private async Task DeleteSelectedCompany()
        {
            SelectedCompany.Disabled = true;
            await BasicDataManager.Context.SaveAsync();

            if (SelectedCompany != null)
                BasicDataManager.Context.Context.Entry(SelectedCompany).State = EntityState.Detached;
            //SelectedCompany = Companies[0];
            Companies.Remove(SelectedCompany);
        }

        public bool IsCompanyNew => SelectedCompany != null && SelectedCompany.Id == 0;
        public bool IsSerieNew => SelectedSerie != null && SelectedSerie.Id == 0;

        private async Task SaveCompanyChangesAsync()
        {
            if (SelectedCompany != null)
            {
                CheckValues();
            }

            if (SelectedCompany != null && SelectedCompany.Id == 0)
            {
                BasicDataManager.Add(SelectedCompany);
                await BasicDataManager.SaveAsync();
            }
            else
            {
                await BasicDataManager.SaveAsync();
            }
        }

        private async Task SaveRecieptSeriesChangesAsync()
        {
            try
            {
                if (SelectedSerie != null && SelectedSerie.Id == 0)
                {
                    MessageBoxResult result = MessageBox.Show($"Είσαστε σίγουρος ότι θέλετε να προσθέσετε νέα σειρά {SelectedSerie.RecieptType.AsString(EnumFormat.Description)} με συμβολισμό {SelectedSerie.Letter ?? " 'χωρίς σύμβολο'"}?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }

                    switch (SelectedSerie.RecieptType)
                    {
                        case Model.RecieptTypeEnum.ServiceReciept:
                            SelectedSerie.SerieCode = "ΑΠΥ";
                            break;

                        case Model.RecieptTypeEnum.ServiceInvoice:
                            SelectedSerie.SerieCode = "ΤΠΥ";
                            break;

                        case Model.RecieptTypeEnum.AirTicketsReciept:
                            SelectedSerie.SerieCode = "ΑΠΕ";
                            break;

                        case Model.RecieptTypeEnum.FerryTicketsReciept:
                            SelectedSerie.SerieCode = "ΑΠΑ";
                            break;

                        case Model.RecieptTypeEnum.CancelationInvoice:
                            SelectedSerie.SerieCode = "ΑΤ";
                            break;

                        case Model.RecieptTypeEnum.CreditInvoice:
                            SelectedSerie.SerieCode = "ΠΤ";
                            break;

                        default:
                            break;
                    }
                    SelectedSerie.SerieCode += ("-" + SelectedSerie.Letter ?? "").TrimEnd('-');
                    SelectedSerie.DateStarted = DateTime.Today;
                    SelectedSerie.AgencyId = StaticResources.User.BaseLocation;
                    BasicDataManager.Add(SelectedSerie);
                    await BasicDataManager.SaveAsync();
                }
                else
                {
                    await BasicDataManager.SaveAsync();
                }
                RaisePropertyChanged(nameof(IsSerieNew));

            }
            catch (Exception)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                MessengerInstance.Send(new ShowExceptionMessage_Message("Η προσθήκη σειράς απέτυχε"));
            }
        }

        private bool SerieDataAreValid()
        {
            return SelectedSerie.Name != null && SelectedSerie.Name.Length > 3;
        }

        #endregion Methods
    }
}