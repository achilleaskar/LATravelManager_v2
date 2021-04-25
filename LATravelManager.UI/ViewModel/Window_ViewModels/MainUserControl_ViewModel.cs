using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Notifications;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using LATravelManager.UI.ViewModel.Management;
using LATravelManager.UI.ViewModel.Parents;
using LATravelManager.UI.Views;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Views.Management;
using LATravelManager.UI.Views.Personal;
using LATravelManager.UI.Views.ThirdParty;
using Microsoft.Win32;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class MainUserControl_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public MainUserControl_ViewModel(MainViewModel mainViewModel)
        {
            LogOutCommand = new RelayCommand(async () => await TryLogOut());

            OpenHotelEditCommand = new RelayCommand(OpenHotelsWindow, CanEditWindows);
            InsertPartnersFromExcelCommand = new RelayCommand(async () => await InsertPartnerDataFromExcel());
            OpenCitiesEditCommand = new RelayCommand(OpenCitiesWindow, CanEditWindows);
            OpenCountriesEditCommand = new RelayCommand(OpenCountriesWindow, CanEditWindows);
            OpenExcursionsEditCommand = new RelayCommand(OpenExcursionsWindow, CanEditWindows);
            OpenUsersEditCommand = new RelayCommand(OpenUsersWindow, CanEditWindows);
            OpenPartnersEditCommand = new RelayCommand(OpenPartnersWindow, CanEditWindows);
            OpenLeadersEditCommand = new RelayCommand(OpenLeadersWindow, CanEditWindows);
            OpenVehiclesEditCommand = new RelayCommand(OpenVehiclesWindow, CanEditWindows);
            OpenOptionalsEditCommand = new RelayCommand(OpenOpionalsWindow, CanEditWindows);
            OpenTaxDataEditCommand = new RelayCommand(async () => await OpenTaxDataEditWindow(), CanEditWindows);
            OpenLoginDataManagementCommand = new RelayCommand(async () => await OpenLoginDataManagementWindow(), CanEditWindows);

            ToggleTestModeCommand = new RelayCommand(async () => await ToggleTestMode());

            NotIsOkCommand = new RelayCommand(async () => await NotIsOk(), CanSetOk);

            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);

            TemplateViewmodels = new List<ExcursionCategory_ViewModelBase>();

            MessengerInstance.Register<IsBusyChangedMessage>(this, msg => { ManageIsBusy(msg.IsBusy); });

            SelectedTemplateChangedCommand = new RelayCommand(SetProperViewModel);

            MainViewModel = mainViewModel;
            var x = GetaAllNotifications().ConfigureAwait(false);

            TestmodeMessage = Properties.Settings.Default.isTest ? "Εναλλαγή σε κανονικό" : "Εναλλαγή σε test";

            // MessengerInstance.Register<ChangeChildViewModelMessage>(this, async vm => { await SelectedExcursionType.SetProperChildViewModel(vm.ViewModelindex); });

            //  MessengerInstance.Register<ExcursionCategoryChangedMessage>(this, async index => { await SetProperViewModel(); });
        }

        private async Task OpenTaxDataEditWindow()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var vm = new TaxDataManagement_ViewModel(MainViewModel.BasicDataManager);
            await vm.LoadAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
            MessengerInstance.Send(new OpenChildWindowCommand(new TaxData_Management_Window { DataContext = vm }));
        }

        private async Task OpenLoginDataManagementWindow()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var vm = new LoginDataManagement_ViewModel();
            await vm.LoadAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
            MessengerInstance.Send(new OpenChildWindowCommand(new LoginDataManagement_Window { DataContext = vm }));
        }

        private bool CanSetOk()
        {
            return SelectedNot != null && SelectedNot.NotificaationType != NotificaationType.NoPay;
        }

        private async Task ToggleTestMode()
        {
            await MainViewModel.BasicDataManager.ToggleTestMode(Properties.Settings.Default.isTest);
            TestmodeMessage = Properties.Settings.Default.isTest ? "Εναλλαγή σε κανονικό" : "Εναλλαγή σε test";
            Properties.Settings.Default.Save();
        }

        private Notification _SelectedNot;

        public Notification SelectedNot
        {
            get
            {
                return _SelectedNot;
            }

            set
            {
                if (_SelectedNot == value)
                {
                    return;
                }

                _SelectedNot = value;
                RaisePropertyChanged();
            }
        }

        private async Task NotIsOk()
        {
            if (SelectedNot != null)
            {
                if (SelectedNot.NotificaationType == NotificaationType.Option && SelectedNot.HotelOptions != null)
                {
                    foreach (var o in SelectedNot.HotelOptions.Options)
                    {
                        o.NotifStatus = new NotifStatus { IsOk = true, OkByUser = NotsRepository.GetById<User>(StaticResources.User.Id), OkDate = DateTime.Now };
                    }
                }
                else if (SelectedNot.NotificaationType == NotificaationType.CheckIn && SelectedNot.Service != null && SelectedNot.Service is PlaneService ps)
                {
                    ps.NotifStatus = new NotifStatus { IsOk = true, OkByUser = NotsRepository.GetById<User>(StaticResources.User.Id), OkDate = DateTime.Now };
                }
                else if (SelectedNot.NotificaationType == NotificaationType.PersonalOption && SelectedNot.Service != null && SelectedNot.Service is HotelService hs)
                {
                    hs.NotifStatus = new NotifStatus { IsOk = true, OkByUser = NotsRepository.GetById<User>(StaticResources.User.Id), OkDate = DateTime.Now };
                }
            }
            if (NotsRepository.HasChanges())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await NotsRepository.SaveAsync();
                Nots.Remove(SelectedNot);
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OpenOpionalsWindow()
        {
            OptionalExcursions_Management_ViewModel vm = new OptionalExcursions_Management_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new OptionalExcursionsManagement_Window { DataContext = vm }));
        }

        private async Task EditBooking()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (SelectedReservation.ExcursionType == ExcursionTypeEnum.Personal)
                {
                    NewReservation_Personal_ViewModel viewModel = new NewReservation_Personal_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.PersonalModel.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditPersonalBooking_Window(), viewModel));
                }
                else if (SelectedReservation.ExcursionType == ExcursionTypeEnum.ThirdParty)
                {
                    NewReservation_ThirdParty_VIewModel viewModel = new NewReservation_ThirdParty_VIewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.ThirdPartyModel.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new Edit_ThirdParty_Booking_Window(), viewModel));
                }
                else if (SelectedReservation.Booking != null)
                {
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.Booking.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }
                else if (SelectedReservation.BookingWrapper != null)
                {
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.BookingWrapper.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        public ReservationWrapper SelectedReservation
        {
            get
            {
                return _SelectedReservation;
            }

            set
            {
                if (_SelectedReservation == value)
                {
                    return;
                }

                _SelectedReservation = value;
                RaisePropertyChanged();
            }
        }

        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        #endregion Constructors

        #region Fields

        private bool _HasNots;

        private bool _IsBusy = false;

        private NavigationViewModel _NavigationViewModel;

        private ObservableCollection<Notification> _Nots;

        private ExcursionCategory_ViewModelBase _SelectedExcursionType;

        private int _SelectedTemplateIndex;

        public RelayCommand EditBookingCommand { get; set; }

        private ObservableCollection<ExcursionCategory> _Templates;

        #endregion Fields

        #region Properties

        public bool HasNots
        {
            get
            {
                return _HasNots;
            }

            set
            {
                if (_HasNots == value)
                {
                    return;
                }

                _HasNots = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }

            set
            {
                if (_IsBusy == value)
                {
                    return;
                }

                _IsBusy = value;
                Application.Current.Dispatcher.Invoke(delegate
                {
                    if (value)
                        Mouse.OverrideCursor = Cursors.Wait;
                    else
                        Mouse.OverrideCursor = Cursors.Arrow;
                });
                RaisePropertyChanged();
            }
        }

        public int IsBusyCounter { get; set; }

        public RelayCommand LogOutCommand { get; set; }
        public RelayCommand ToggleTestModeCommand { get; set; }
        public RelayCommand OpenLoginDataManagementCommand { get; set; }
        public RelayCommand OpenTaxDataEditCommand { get; set; }

        private string _TestmodeMessage;

        public string TestmodeMessage
        {
            get
            {
                return _TestmodeMessage;
            }

            set
            {
                if (_TestmodeMessage == value)
                {
                    return;
                }

                _TestmodeMessage = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public NavigationViewModel NavigationViewModel
        {
            get
            {
                return _NavigationViewModel;
            }

            set
            {
                if (_NavigationViewModel == value)
                {
                    return;
                }

                _NavigationViewModel = value;
                RaisePropertyChanged();
            }
        }

        private ICollectionView _NotsCV;
        private ReservationWrapper _SelectedReservation;

        public ICollectionView NotsCV
        {
            get
            {
                return _NotsCV;
            }

            set
            {
                if (_NotsCV == value)
                {
                    return;
                }

                _NotsCV = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Notification> Nots
        {
            get
            {
                return _Nots;
            }

            set
            {
                if (_Nots == value)
                {
                    return;
                }

                _Nots = value;
                HasNots = Nots.Count > 0;
                NotsCV = CollectionViewSource.GetDefaultView(Nots);
                NotsCV.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Notification.NotificaationType)));
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenCitiesEditCommand { get; }

        public RelayCommand OpenCountriesEditCommand { get; }

        public RelayCommand OpenExcursionsEditCommand { get; }
        public RelayCommand OpenOptionalsEditCommand { get; }

        public RelayCommand OpenHotelEditCommand { get; }
        public RelayCommand InsertPartnersFromExcelCommand { get; }
        public RelayCommand NotIsOkCommand { get; }

        public RelayCommand OpenLeadersEditCommand { get; }

        public RelayCommand OpenPartnersEditCommand { get; }

        public RelayCommand OpenUsersEditCommand { get; set; }

        public RelayCommand OpenVehiclesEditCommand { get; }

        public ExcursionCategory_ViewModelBase SelectedExcursionType
        {
            get
            {
                return _SelectedExcursionType;
            }

            set
            {
                if (_SelectedExcursionType == value)
                {
                    return;
                }

                _SelectedExcursionType = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SelectedTemplateChangedCommand { get; set; }

        public int SelectedTemplateIndex
        {
            get
            {
                return _SelectedTemplateIndex;
            }

            set
            {
                if (_SelectedTemplateIndex == value)
                {
                    return;
                }

                _SelectedTemplateIndex = value;
                //if (value >= 0 && value <= Templates.Count)
                //{
                //    MessengerInstance.Send(new ExcursionCategoryChangedMessage());
                //}
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ExcursionCategory> Templates
        {
            get
            {
                return _Templates;
            }

            set
            {
                if (_Templates == value)
                {
                    return;
                }

                _Templates = value;
                RaisePropertyChanged();
                SetProperViewModel();
            }
        }

        public List<ExcursionCategory_ViewModelBase> TemplateViewmodels { get; set; }

        public string UserName => StaticResources.User != null ?
            $"{StaticResources.User.Name} {((!string.IsNullOrEmpty(StaticResources.User.Surename) && StaticResources.User.Surename.Length > 4) ? StaticResources.User.Surename.Substring(0, 5) : "")}"
            : "Error";

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null, MyViewModelBase parent = null)
        {
            NavigationViewModel = new NavigationViewModel(this, MainViewModel);
            Templates = MainViewModel.BasicDataManager.ExcursionCategories;

            await Task.Delay(0);
            // await SetProperViewModel();
        }

        private GenericRepository _NotsRepository;

        public GenericRepository NotsRepository
        {
            get
            {
                return _NotsRepository;
            }

            set
            {
                if (_NotsRepository == value)
                {
                    return;
                }

                _NotsRepository = value;
                RaisePropertyChanged();
            }
        }

        public async Task<List<Notification>> LoadOptions(GenericRepository repository, User user)
        {
            List<Option> options = await repository.GetAllPendingOptions(user);
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                HotelOptions HotelOptions;
                List<HotelOptions> hotelOptionsLis = new List<HotelOptions>();
                foreach (Option option in options)
                {
                    if (option.Room?.Hotel != null)
                    {
                        HotelOptions = hotelOptionsLis.Where(ho => ho.Hotel == option.Room.Hotel && ho.Date == option.Date).FirstOrDefault();
                        if (HotelOptions != null)
                        {
                            HotelOptions.Counter++;
                            HotelOptions.Options.Add(option);
                        }
                        else
                        {
                            hotelOptionsLis.Add(new HotelOptions { Counter = 1, Date = option.Date, Hotel = option.Room.Hotel, Options = new List<Option>() });
                            hotelOptionsLis.Last().Options.Add(option);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Option Hotel is null error. Inform the Admin");
                    }
                }
                foreach (var hotel in hotelOptionsLis)
                {
                    reply.Add(new Notification { Details = $"Οι Option για { hotel.Counter} δωμάτια στο { hotel.Hotel.Name} λήγουν στις {hotel.Date.ToShortDateString()}", NotificaationType = NotificaationType.Option, HotelOptions = hotel });
                }
            }
            return reply;
        }

        public void OpenCitiesWindow()
        {
            CitiesManagement_ViewModel vm = new CitiesManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new CitiesManagement_Window { DataContext = vm }));
        }

        public void OpenCountriesWindow()
        {
            CountriesManagement_ViewModel vm = new CountriesManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new CountriesManagement_Window { DataContext = vm }));
        }

        public void OpenHotelsWindow()
        {
            HotelsManagement_ViewModel vm = new HotelsManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new HotelsManagement_Window { DataContext = vm }));
        }

        public async Task InsertPartnerDataFromExcel()
        {
            var cities = MainViewModel.BasicDataManager.Cities;
            var countries = MainViewModel.BasicDataManager.Countries;
            var activities = await MainViewModel.BasicDataManager.Context.GetAllACtivitiesAsync();
            OpenFileDialog dlg = new OpenFileDialog
            {
                //FileName = "Document", // Default file name
                //DefaultExt = ".pdf", // Default file extension
                Filter = "Excel Files (*.xlsx*)|*.xlsx*" // Filter files by extension
            };
            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;

                Dictionary<string, string> pointers = new Dictionary<string, string>();
                string fileName = dlg.FileName;
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    using FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false);
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringTable sst = sstpart.SharedStringTable;

                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    Worksheet sheet = worksheetPart.Worksheet;

                    IEnumerable<Row> rows = sheet.Descendants<Row>();
                    int i = 0;
                    CustomerWrapper tmpCustomerWr = new CustomerWrapper();
                    Company comp;
                    List<Company> companies = new List<Company>();
                    if (rows.Any())
                    {
                        //// for (int l = char.ToUpper('A', CultureInfo.CurrentCulture); l <= char.ToUpper('Z', CultureInfo.CurrentCulture); l++)
                        // {
                        // }
                        int ssid = 0;
                        string content;
                        string pointer;
                        int parsedint;
                        foreach (var row in rows)
                        {
                            comp = new Company();
                            if (row.RowIndex == 1)
                            {
                                foreach (Cell c in row.Elements<Cell>())
                                {
                                    if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                    {
                                        ssid = int.Parse(c.CellValue.Text);
                                        content = sst.ChildElements[ssid].InnerText;
                                        pointers.Add(c.CellReference.ToString().Trim('1'), content);
                                    }
                                }
                                continue;
                            }
                            foreach (Cell c in row.Elements<Cell>())
                            {
                                if (c.CellValue == null)
                                    continue;
                                if (!int.TryParse(c.CellValue.Text, out ssid))
                                    ssid = -1;
                                if (!((c.DataType != null) && (c.DataType == CellValues.SharedString)))
                                    content = c.CellValue.Text;
                                else
                                {
                                    if (ssid < 0)
                                        continue;
                                    else
                                        content = sst.ChildElements[ssid].InnerText;
                                }
                                content = content.Trim();
                                if (content.ToUpperInvariant() == "NULL")
                                {
                                    continue;
                                }
                                pointer = Regex.Replace(c.CellReference.ToString(), @"\d", "");
                                if (pointers.TryGetValue(pointer, out string val))
                                {
                                    switch (val)
                                    {
                                        case "Code":
                                            comp.Code = int.TryParse(content, out parsedint) ? parsedint : 0;
                                            break;

                                        case "Name":
                                            comp.Name = content;
                                            break;

                                        case "LastName":
                                            comp.LastName = content;
                                            break;
                                        //case "NameInLatin":
                                        //    comp.name = content;
                                        //    break;
                                        case "CompanyName":
                                            comp.CompanyName = content;
                                            break;

                                        case "Address":
                                            comp.AddressRoad = content;
                                            break;

                                        case "AddressNumber":
                                            comp.AddressNumber = int.TryParse(content, out parsedint) ? parsedint : 0;
                                            break;

                                        case "ZipCode":
                                            comp.AddressZipCode = content;
                                            break;

                                        case "City":
                                            var ct = cities.FirstOrDefault(c => c.Name.ToUpperInvariant() == content.ToUpperInvariant());
                                            if (ct != null)
                                            {
                                                comp.AddressCity = ct;
                                            }
                                            else
                                            {
                                                comp.AddressCity = new City { Name = content, Country = countries.FirstOrDefault(r => r.Id == 6) };
                                                MainViewModel.BasicDataManager.Cities.Add(comp.AddressCity);
                                            }
                                            break;

                                        case "Country":
                                            var ctry = countries.FirstOrDefault(c => c.Name.ToUpperInvariant() == content.ToUpperInvariant());
                                            if (ctry != null)
                                            {
                                                comp.Country = ctry;
                                            }
                                            else
                                            {
                                                comp.Country = new Country { Name = content };
                                                MainViewModel.BasicDataManager.Countries.Add(comp.Country);
                                            }
                                            break;

                                        case "Activity":
                                            var act = activities.FirstOrDefault(c => c.Name.ToUpperInvariant() == content.ToUpperInvariant());
                                            if (act != null)
                                            {
                                                comp.Activity = act;
                                            }
                                            else
                                            {
                                                comp.Activity = new CompanyActivity { Name = content };
                                                activities.Add(comp.Activity);
                                            }
                                            break;

                                        case "TaxationNo":
                                            comp.TaxationNumber = content;
                                            break;

                                        case "TaxOffice":
                                            comp.TaxOffice = content;
                                            break;

                                        case "BillAddress":
                                            comp.BillRoad = content;
                                            break;

                                        case "BillZipCode":
                                            comp.BillZipCode = content;
                                            break;

                                        case "BillCity":
                                            var bct = cities.FirstOrDefault(c => c.Name.ToUpperInvariant() == content.ToUpperInvariant());
                                            if (bct != null)
                                            {
                                                comp.BillCity = bct;
                                            }
                                            else
                                            {
                                                comp.BillCity = new City { Name = content, Country = countries.FirstOrDefault(r => r.Id == 6) };
                                                MainViewModel.BasicDataManager.Cities.Add(comp.BillCity);
                                            }
                                            break;

                                        case "Phone":
                                            comp.Phone1 = content;
                                            break;

                                        case "SecondPhone":
                                            comp.Phone2 = content;
                                            break;

                                        case "MobilePhone":
                                            comp.MobilePhone = content;
                                            break;

                                        case "EMail":
                                            comp.Email = content;
                                            break;

                                        case "Details":
                                            comp.Comment = content;
                                            break;

                                        case "CreationDate":
                                            comp.CreationDate = DateTime.TryParse(content, out DateTime date) ? date : new DateTime();
                                            break;

                                        case "IsAgent":
                                            comp.IsAgent = content == "1";
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(comp.Name) && !string.IsNullOrEmpty(comp.LastName) && comp.Name == comp.LastName)
                            {
                                comp.LastName = "";
                            }
                            if (comp.AddressNumber > 0 && !string.IsNullOrEmpty(comp.AddressRoad) && comp.AddressRoad.EndsWith(comp.AddressNumber.ToString()))
                            {
                                comp.AddressRoad = comp.AddressRoad.Substring(0, comp.AddressRoad.LastIndexOf(comp.AddressNumber.ToString())).Trim();
                            }
                            if (comp.Code > 0)
                            {
                                comp.Id = comp.Code;
                            }
                            companies.Add(comp);
                            //if (!string.IsNullOrEmpty(comp.BillAddressNumber.ToString()) && comp.BillRoad.EndsWith(comp.BillAddressNumber.ToString()))
                            //{
                            //    comp.BillRoad = comp.BillRoad.Substring(0, comp.BillRoad.LastIndexOf(comp.BillAddressNumber.ToString()));
                            //}
                        }

                        int ctr = 0;
                        foreach (var act in activities.Where(c => c.Id == 0))
                        {
                            MainViewModel.BasicDataManager.Context.Add(act);
                            ctr++;
                            if (ctr % 10 == 9)
                            {
                                await MainViewModel.BasicDataManager.Context.SaveAsync();
                            }
                        }

                        foreach (var cte in cities.Where(c => c.Id == 0))
                        {
                            MainViewModel.BasicDataManager.Context.Add(cte);
                            ctr++;
                            if (ctr % 10 == 9)
                            {
                                await MainViewModel.BasicDataManager.Context.SaveAsync();
                            }
                        }

                        foreach (var cte in countries.Where(c => c.Id == 0))
                        {
                            MainViewModel.BasicDataManager.Context.Add(cte);
                            ctr++;
                            if (ctr % 10 == 9)
                            {
                                await MainViewModel.BasicDataManager.Context.SaveAsync();
                            }
                        }

                        foreach (var com in companies)
                        {
                            MainViewModel.BasicDataManager.Context.Add(com);
                            ctr++;
                            if (ctr % 10 == 9)
                            {
                                await MainViewModel.BasicDataManager.Context.SaveAsync();
                            }
                        }
                        await MainViewModel.BasicDataManager.Context.SaveAsync();
                    }
                    else
                    {
                    }
                    // BookingWr.CalculateRemainingAmount();
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public override async Task ReloadAsync()
        {
            await Task.Delay(0);
        }

        public async Task TryLogOut()
        {
            StaticResources.User = null;
            await MainViewModel.ChangeViewModel();
        }

        private bool CanEditWindows()
        {
            return true;
            // return MainViewModel. StartingRepository.IsContextAvailable;
        }

        private async Task GetaAllNotifications()
        {
            var notificationManager = new NotificationManager();

            NotsRepository = new GenericRepository();
            List<Notification> nots = new List<Notification>();
            var user = StaticResources.User;
            nots.AddRange(await LoadOptions(NotsRepository,user));
            nots.AddRange(await GetPersonalOptions(NotsRepository,user));
            nots.AddRange(await GetPersonalCheckIns(NotsRepository,user));
            nots.AddRange(await GetNonPayersGroup(NotsRepository,user));
            nots.AddRange(await GetNonPayersPersonal(NotsRepository,user));
            nots.AddRange(await GetNonPayersThirdParty(NotsRepository,user));

            //foreach (var not in nots)
            //{
            //    notificationManager.Show(new NotificationContent
            //    {
            //        Message = not.Details,
            //        Type = NotificationType.Warning
            //    });
            //}
            Nots = new ObservableCollection<Notification>(nots);
        }

        private async Task<IEnumerable<Notification>> GetNonPayersGroup(GenericRepository repository, User user)
        {
            List<BookingWrapper> options = (await repository.GetAllNonPayersGroup(user)).Select(b => new BookingWrapper(b)).ToList();
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if (((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || (booking.CheckIn - DateTime.Today).TotalDays <= 5) && booking.FullPrice > booking.Customers.Count && booking.Recieved < 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Η κράτηση για { booking.Excursion.Destinations[0]}  " +
                            $"στο όνομα { booking.ReservationsInBooking[0].CustomersList[0] } δεν έχει δώσει προκαταβολή",
                            ReservationWrapper = new ReservationWrapper { BookingWrapper = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                    else if ((booking.CheckIn - DateTime.Today).TotalDays <= 5 && booking.Remaining > 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Η κράτηση για { booking.Excursion.Destinations[0]}  " +
                            $"στο όνομα { booking.ReservationsInBooking[0].CustomersList[0] } δεν έχει κάνει εξόφληση",
                            ReservationWrapper = new ReservationWrapper { BookingWrapper = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<Notification>> GetNonPayersPersonal(GenericRepository repository,User user)
        {
            List<Personal_BookingWrapper> options = (await repository.GetAllNonPayersPersonal(user)).Select(b => new Personal_BookingWrapper(b)).ToList();
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if ((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || booking.Services.Any(a => (a.TimeGo - DateTime.Today).TotalDays <= 5) && booking.FullPrice > booking.Customers.Count && booking.Recieved < 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Το ατομικό πακέτο στο όνομα { booking.Customers[0] } δεν έχει δώσει προκαταβολή",
                            ReservationWrapper = new ReservationWrapper { PersonalModel = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                    else if (booking.Services.Any(a => (a.TimeGo - DateTime.Today).TotalDays <= 5) && booking.Remaining > 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Το ατομικό πακέτο στο όνομα { booking.Customers[0] } δεν έχει δώσει προκαταβολή",
                            ReservationWrapper = new ReservationWrapper { PersonalModel = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<Notification>> GetNonPayersThirdParty(GenericRepository repository, User user)
        {
            List<ThirdParty_Booking_Wrapper> options = (await repository.GetAllNonPayersThirdparty(user)).Select(b => new ThirdParty_Booking_Wrapper(b)).ToList();
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                Parallel.ForEach(options, o => o.CalculateRemainingAmount());
                foreach (var booking in options)
                {
                    if (((DateTime.Today - booking.CreatedDate).TotalDays >= 5 || (booking.CheckIn - DateTime.Today).TotalDays <= 5) && booking.FullPrice > booking.Customers.Count && booking.Recieved < 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Το πακέτο συνεργάτη για { booking.City} " +
                            $"στο όνομα { booking.Customers[0]} δεν έχει δώσει προκαταβολή",
                            ReservationWrapper = new ReservationWrapper { ThirdPartyModel = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                    else if ((booking.CheckIn - DateTime.Today).TotalDays <= 5 && booking.Remaining > 1)
                    {
                        reply.Add(new Notification
                        {
                            Details = $"Το πακέτο συνεργάτη για { booking.City} " +
                            $"στο όνομα { booking.Customers[0]} δεν έχει κάνει εξόφληση",
                            ReservationWrapper = new ReservationWrapper { ThirdPartyModel = booking },
                            NotificaationType = NotificaationType.NoPay
                        });
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<Notification>> GetPersonalCheckIns(GenericRepository repository, User user)
        {
            List<PlaneService> options = await repository.GetAllPlaneOptions(user);
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                foreach (var option in options)
                {
                    if (option.Personal_Booking == null)
                    {
                        continue;
                    }
                    if (option.Airline != null && option.Airline.Checkin > 10)
                    {
                        if (option.Airline.Checkin != 0 && option.TimeGo > DateTime.Now &&
                            (option.TimeGo - DateTime.Now).TotalHours <= option.Airline.Checkin)
                        {
                            reply.Add(new Notification { Details = $"Άνοιξε το CheckIn {option.From}-{option.To} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}", NotificaationType = NotificaationType.CheckIn, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) }, Service = option });
                        }
                        if (option.Airline.Checkin != 0 && option.TimeReturn > DateTime.Now && option.Allerretour &&
                            (option.TimeReturn - DateTime.Now).TotalHours <= option.Airline.Checkin)
                        {
                            reply.Add(new Notification { Details = $"Άνοιξε το CheckIn Επιστροφής {option.To}-{option.From} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}", NotificaationType = NotificaationType.CheckIn, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) }, Service = option });
                        }
                    }
                    else
                    {
                        if (option.TimeGo > DateTime.Now &&
                           (option.TimeGo - DateTime.Now).TotalHours <= 48)
                        {
                            reply.Add(new Notification { Details = $"Ίσως άνοιξε το CheckIn {option.From}-{option.To} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}", NotificaationType = NotificaationType.CheckIn, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) }, Service = option });
                        }
                        if (option.TimeReturn > DateTime.Now && option.Allerretour &&
                            (option.TimeReturn - DateTime.Now).TotalHours <= 48)
                        {
                            reply.Add(new Notification { Details = $"Ίσως άνοιξε το CheckIn Επιστροφής {option.To}-{option.From} του {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())}", NotificaationType = NotificaationType.CheckIn, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) }, Service = option });
                        }
                    }
                }
            }
            return reply;
        }

        private async Task<IEnumerable<Notification>> GetPersonalOptions(GenericRepository repository, User user)
        {
            List<HotelService> options = await repository.GetAllPersonalOptions(user);
            List<Notification> reply = new List<Notification>();

            if (options.Count > 0)
            {
                foreach (var option in options)
                {
                    if (option.Personal_Booking == null)
                    {
                        continue;
                    }
                    reply.Add(new Notification { Details = $"Η Option για το ατομικό {(option.Personal_Booking.Customers.Count > 0 ? option.Personal_Booking.Customers.ToList()[0].ToString() : option.Id.ToString())} στο {(option.Hotel != null ? option.Hotel.Name : "Αγνωστο Ξενοδοχείο")} λήγει στις {option.Option.ToShortDateString()}", NotificaationType = NotificaationType.PersonalOption, ReservationWrapper = new ReservationWrapper { PersonalModel = new Personal_BookingWrapper(option.Personal_Booking) } });
                }
            }
            return reply;
        }

        private void ManageIsBusy(bool add)
        {
            _ = add ? IsBusyCounter++ : IsBusyCounter--;
            IsBusy = IsBusyCounter > 0;
        }

        private void OpenExcursionsWindow()
        {
            ExcursionsManagement_ViewModel vm = new ExcursionsManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new ExcursionsManagement_Window { DataContext = vm }));
        }

        private void OpenLeadersWindow()
        {
            LeadersManagement_ViewModel vm = new LeadersManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new LeadersManagement_Widnow { DataContext = vm }));
        }

        private void OpenPartnersWindow()
        {
            PartnerManagement_ViewModel vm = new PartnerManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new PartnersManagement_Window { DataContext = vm }));
        }

        private void OpenUsersWindow()
        {
            UsersManagement_viewModel vm = new UsersManagement_viewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new UsersManagement_Window { DataContext = vm }));
        }

        private void OpenVehiclesWindow()
        {
            VehiclesManagement_viewModel vm = new VehiclesManagement_viewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new BusesManagement_Window { DataContext = vm }));
        }

        private void SetProperViewModel()
        {
            if (Templates == null)
            {
                return;
            }
            if (SelectedTemplateIndex < 0)
            {
                SelectedTemplateIndex = 1;
            }
            if (SelectedTemplateIndex < Templates.Count)
            {
                int index = -1;
                switch (Templates[SelectedTemplateIndex].Category)
                {
                    case ExcursionTypeEnum.Bansko:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(BanskoParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new BanskoParent_ViewModel(MainViewModel);
                        }
                        break;

                    case ExcursionTypeEnum.Group:

                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(GroupParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new GroupParent_ViewModel(MainViewModel);
                        }
                        break;

                    case ExcursionTypeEnum.Personal:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(PersonalParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new PersonalParent_ViewModel(MainViewModel);
                        }
                        break;

                    case ExcursionTypeEnum.ThirdParty:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(ThirdPartyParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new ThirdPartyParent_ViewModel(MainViewModel);
                        }

                        break;

                    case ExcursionTypeEnum.Skiathos:
                        index = TemplateViewmodels.FindIndex(x => x.GetType() == typeof(SkiathosParent_ViewModel));
                        if (index >= 0)
                            SelectedExcursionType = TemplateViewmodels[index];
                        else
                        {
                            SelectedExcursionType = new SkiathosParent_ViewModel(MainViewModel);
                        }

                        break;
                }
                if (index < 0)
                {
                    TemplateViewmodels.Add(SelectedExcursionType);
                }
                //set to default tab
                if (SelectedExcursionType is ExcursionCategory_ViewModelBase)
                {
                    if (index < 0)
                        SelectedExcursionType.Load();
                    NavigationViewModel.SetTabs();
                }
            }
            else
            {
                throw new Exception("SelectedTemplateIndex greater than Templates count");
            }
        }

        #endregion Methods
    }
}