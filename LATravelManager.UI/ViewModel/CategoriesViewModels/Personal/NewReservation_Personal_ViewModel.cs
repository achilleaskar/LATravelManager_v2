using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.Model.Services;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.ServiceViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Personal
{
    public class NewReservation_Personal_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors
        public RelayCommand SearchForCustomerCommand { get; set; }
        private async Task OpenInvoicesWindow(object obj)
        {
            InvoicesManagement_ViewModel vm = new InvoicesManagement_ViewModel(BasicDataManager, personal: PersonalWr, parameter: obj);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new InvoicesManagementWindow(), vm));
        }

        public RelayCommand ToggleDisabilityCommand { get; set; }
        public RelayCommand<object> OpenInvoicesWindowCommand { get; set; }

        public NewReservation_Personal_ViewModel(MainViewModel mainViewModel)
        {
            OpenInvoicesWindowCommand = new RelayCommand<object>(async (obj) => await OpenInvoicesWindow(obj));

            GenericRepository = mainViewModel.StartingRepository;
            BasicDataManager = mainViewModel.BasicDataManager;
            //Commands
            ClearBookingCommand = new RelayCommand(async () => { await ClearBooking(); });
            AddCustomerCommand = new RelayCommand(AddRandomCustomer);

            DeletePaymentCommand = new RelayCommand(DeletePayment, CanDeletePayment);
            ClearServiceCommand = new RelayCommand(ClearService, CanClearService);
            AddServiceCommand = new RelayCommand(AddService, CanAddService);
            ToggleDisabilityCommand = new RelayCommand(ToggleDisability, CanToggleDisability);
            OpenFileCommand = new RelayCommand<Reciept>(async (obj) => await OpenRecieptFile(obj));
            ReplaceFileCommand = new RelayCommand(ReplaceFile);
            DeleteSelectedCustomersCommand = new RelayCommand(DeleteSelectedCustomers, CanDeleteCustomers);
            SearchForCustomerCommand = new RelayCommand(SearchForCustomer);

            SaveCommand = new RelayCommand(async () => { await SaveAsync(); }, CanSave);
            Payment = new Payment();
            MainViewModel = mainViewModel;

            PrintRecieptCommand = new RelayCommand(PrintReciept);

            EditServiceCommand = new RelayCommand(EditService);
            Emails = new ObservableCollection<Email>();
        }

        public void SearchForCustomer()
        {
            CustomerSearch_ViewModel vm = new CustomerSearch_ViewModel(GenericRepository, personal: PersonalWr);
            Window window = new CustomerSearchWindow
            {
                DataContext = vm
            };
            window.ShowDialog();
        }

        private void ReplaceFile()
        {
            if (SelectedReciept == null)
            {
                return;
            }
            OpenFileDialog dlg = new OpenFileDialog
            {
                //FileName = "Document", // Default file name
                //DefaultExt = ".pdf", // Default file extension
                Filter = "All files (*.*)|*.*" // Filter files by extension
            };
            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                // Open document
                string fileName = dlg.FileName;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    FileInfo file = new FileInfo(fileName);
                    if (file.Exists)
                    {
                        SelectedReciept.Content = File.ReadAllBytes(fileName);
                        SelectedReciept.FileName = file.Name;
                    }
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private Reciept _SelectedReciept;


        public Reciept SelectedReciept
        {
            get
            {
                return _SelectedReciept;
            }

            set
            {
                if (_SelectedReciept == value)
                {
                    return;
                }

                _SelectedReciept = value;
                RaisePropertyChanged();
            }
        }

        public static string GetPath(string fileName, string folderpath)
        {
            int i = 1;
            string resultPath;
            string filenm = fileName.Substring(0, fileName.LastIndexOf('.'));
            string fileExtension = fileName.Substring(fileName.LastIndexOf('.'));
            resultPath = folderpath + filenm + fileExtension;
            while (File.Exists(resultPath))
            {
                i++;
                resultPath = folderpath + filenm + "(" + i + ")" + fileExtension;
            }
            return resultPath;
        }

        private async Task OpenRecieptFile(Reciept Reciept)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            string outputpath = Path.GetTempPath();

            Directory.CreateDirectory(outputpath + @"\Παραστατικά");
            string path = GetPath(Reciept.FileName, outputpath + @"\Παραστατικά\");
            File.WriteAllBytes(path, Reciept.Content);

            Mouse.OverrideCursor = Cursors.Arrow;

            Process.Start(path);
        }

        public RelayCommand<Reciept> OpenFileCommand { get; set; }
        public RelayCommand ReplaceFileCommand { get; set; }
        public RelayCommand DeleteSelectedCustomersCommand { get; set; }

        private bool CanDeleteCustomers()
        {
            return PersonalWr.CustomerWrappers.Any(c => c.IsSelected);
        }

        public void DeleteSelectedCustomers()
        {
            List<CustomerWrapper> toRemove = new List<CustomerWrapper>();
            foreach (CustomerWrapper customer in PersonalWr.CustomerWrappers)
            {
                if (customer.IsSelected)
                {
                    toRemove.Add(customer);
                    customer.BusGo = null;
                    customer.BusReturn = null;
                }
            }
            foreach (CustomerWrapper c in toRemove)
            {
                PersonalWr.CustomerWrappers.Remove(c);
            }
        }

        private bool CanToggleDisability()
        {
            return PersonalWr != null && !string.IsNullOrEmpty(PersonalWr.CancelReason);
        }

        private void ToggleDisability()
        {
            if (PersonalWr.Disabled)
            {
                PersonalWr.Disabled = false;
            }
            else
            {
                PersonalWr.DisableDate = DateTime.Now;
                PersonalWr.DisabledBy = GenericRepository.GetById<User>(StaticResources.User.Id);
                PersonalWr.Disabled = true;
            }
        }

        public DocumentsManagement DocumentsManagement { get; set; }

        private void PrintReciept()
        {
            if (SelectedCustomer == null)
            {
                if (PersonalWr.CustomerWrappers.Count == 1)
                {
                    SelectedCustomer = PersonalWr.CustomerWrappers.First();
                }
                else
                {
                    MessageBox.Show("Παρακαλώ επιλέξτε πελάτη");
                    return;
                }
            }
            if (DocumentsManagement == null)
            {
                DocumentsManagement = new DocumentsManagement(GenericRepository);
            }
            DocumentsManagement.PrintPaymentsReciept(SelectedPayment, SelectedCustomer.Model);
        }

        public RelayCommand PrintRecieptCommand { get; set; }

        #endregion Constructors

        #region Fields

        private readonly string[] ValidateRoomsProperties =
               {
            nameof(PersonalWr),nameof(Payment)
        };

        private bool _All;
        private string _BookedMessage;
        private string _ErrorsInDatagrid;
        private ObservableCollection<Partner> _Partners;
        private Personal_BookingWrapper _PersonalWr;
        private CustomerWrapper _SelectedCustomer;
        private int _SelectedPartnerIndex;
        private Payment _SelectedPayment;
        private Service _SelectedService;

        private ServiceViewModel _SelectedServiceViewModel;

        private int _SelectedUserIndex;

        private ObservableCollection<Service> _Services;

        private List<ServiceViewModel> _Templates;

        private ObservableCollection<User> _Users;
        private ObservableCollection<Email> _Emails;
        private Email _SelectedEmail;

        #endregion Fields

        #region Properties

        public RelayCommand AddCustomerCommand { get; }

        public RelayCommand AddServiceCommand { get; set; }

        public bool All
        {
            get
            {
                return _All;
            }

            set
            {
                if (_All == value)
                {
                    return;
                }

                _All = value;
                RaisePropertyChanged();
            }
        }

        public bool AreBookingDataValid
        {
            get
            {
                foreach (string property in ValidateRoomsProperties)
                {
                    ErrorsInDatagrid = GetBookingDataValidationError(property);
                    if (ErrorsInDatagrid != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public BasicDataManager BasicDataManager { get; private set; }

        public string BookedMessage
        {
            get
            {
                return _BookedMessage;
            }

            set
            {
                if (_BookedMessage == value)
                {
                    return;
                }

                _BookedMessage = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ClearBookingCommand { get; }

        public RelayCommand ClearServiceCommand { get; set; }

        public RelayCommand DeletePaymentCommand { get; }

        public RelayCommand EditServiceCommand { get; set; }

        public string ErrorsInDatagrid
        {
            get
            {
                return _ErrorsInDatagrid;
            }

            set
            {
                if (_ErrorsInDatagrid == value)
                {
                    return;
                }

                _ErrorsInDatagrid = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public ObservableCollection<Partner> Partners
        {
            get
            {
                return _Partners;
            }

            set
            {
                if (_Partners == value)
                {
                    return;
                }

                _Partners = value;
                RaisePropertyChanged();
            }
        }

        public Payment Payment { get; private set; }

        public Personal_BookingWrapper PersonalWr
        {
            get
            {
                return _PersonalWr;
            }

            set
            {
                if (_PersonalWr == value)
                {
                    return;
                }

                _PersonalWr = value;
                PersonalWr.PropertyChanged += PersonalWr_PropertyChanged;

                RaisePropertyChanged();
            }
        }

        public bool IsPartners
        {
            get
            {
                return PersonalWr != null && PersonalWr.IsPartners;
            }

            set
            {
                if (PersonalWr.IsPartners == value)
                {
                    return;
                }

                PersonalWr.IsPartners = value;
                if (!value)
                {
                    SelectedPartnerIndex = -1;
                }
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; }

        public CustomerWrapper SelectedCustomer
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

        public int SelectedPartnerIndex
        {
            get
            {
                return _SelectedPartnerIndex;
            }
            set
            {
                if (_SelectedPartnerIndex == value)
                {
                    return;
                }
                _SelectedPartnerIndex = value;
                if (SelectedPartnerIndex >= 0 && (PersonalWr.Partner == null || (PersonalWr.Partner != null && PersonalWr.Partner.Id != Partners[SelectedPartnerIndex].Id)))
                {
                    PersonalWr.Partner = GenericRepository.GetById<Partner>(Partners[SelectedPartnerIndex].Id);
                    Emails = (PersonalWr.Partner.Emails != null && PersonalWr.Partner.Emails.Any()) ? new ObservableCollection<Email>(PersonalWr.Partner.Emails.Split(',').Select(e => new Email(e))) : new ObservableCollection<Email>();
                    if (Emails.Count > 0)
                    {
                        SelectedEmail = Emails[0];
                    }
                }
                RaisePropertyChanged();
            }
        }

        public Email SelectedEmail
        {
            get
            {
                return _SelectedEmail;
            }

            set
            {
                if (_SelectedEmail == value)
                {
                    return;
                }

                _SelectedEmail = value;
                if (value != null && PersonalWr.Partner != null)
                {
                    PersonalWr.PartnerEmail = value.EValue;
                }
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Email> Emails
        {
            get
            {
                return _Emails;
            }

            set
            {
                if (_Emails == value)
                {
                    return;
                }

                _Emails = value;
                RaisePropertyChanged();
            }
        }

        public Payment SelectedPayment
        {
            get => _SelectedPayment;

            set
            {
                if (_SelectedPayment == value)
                {
                    return;
                }

                _SelectedPayment = value;
                RaisePropertyChanged();
            }
        }

        public Service SelectedService
        {
            get
            {
                return _SelectedService;
            }

            set
            {
                if (_SelectedService == value)
                {
                    return;
                }

                _SelectedService = value;
                RaisePropertyChanged();
            }
        }

        public ServiceViewModel SelectedServiceViewModel
        {
            get
            {
                return _SelectedServiceViewModel;
            }

            set
            {
                if (_SelectedServiceViewModel == value)
                {
                    return;
                }

                _SelectedServiceViewModel = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedUserIndex
        {
            get
            {
                return _SelectedUserIndex;
            }
            set
            {
                if (_SelectedUserIndex == value)
                {
                    return;
                }
                _SelectedUserIndex = value;
                if (SelectedUserIndex >= 0 && (PersonalWr.User == null || (PersonalWr.User != null && PersonalWr.User.Id != Users[SelectedUserIndex].Id)))
                {
                    PersonalWr.User = GenericRepository.GetById<User>(Users[SelectedUserIndex].Id);
                }
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Service> Services
        {
            get
            {
                return _Services;
            }

            set
            {
                if (_Services == value)
                {
                    return;
                }

                _Services = value;
                RaisePropertyChanged();
            }
        }

        public GenericRepository GenericRepository { get; private set; }

        public List<ServiceViewModel> Templates
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
            }
        }

        public ObservableCollection<User> Users
        {
            get => _Users;

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

        private bool AreContexesFree => (BasicDataManager != null && BasicDataManager.IsContextAvailable) && (GenericRepository != null && GenericRepository.IsContextAvailable);

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                if (id > 0)
                {
                    GenericRepository = new GenericRepository();
                    BasicDataManager = new BasicDataManager(GenericRepository);
                    await BasicDataManager.LoadPersonal();
                }

                Personal_Booking booking = id > 0
                      ? await GenericRepository.GetFullPersonalBookingByIdAsync(id)
                      : await CreateNewBooking();

                if (id > 0)
                    await GenericRepository.GetAllAsync<Reciept>(r => r.Personal_BookingId == id);

                InitializeBooking(booking);

                Services = new ObservableCollection<Service>();

                await ResetAllRefreshableDataASync();

                LoadTemplates();
                GetServices();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                IsLoaded = true;
            }
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        private void AddRandomCustomer()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string name = new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            string surename = new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            string passport = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            PersonalWr.CustomerWrappers.Add(new CustomerWrapper() { Age = 18, Name = name, Surename = surename, Price = 35, Tel = "6981001676", StartingPlace = "Αθήνα", PassportNum = passport, DOB = new DateTime(1991, 10, 4) });
        }

        private void AddService()
        {
            bool hasCustomers = false;
            foreach (var c in PersonalWr.CustomerWrappers)
            {
                if (c.IsSelected || All)
                {
                    if (!c.Services.Contains(SelectedServiceViewModel.Service))
                    {
                        c.Services.Add(SelectedServiceViewModel.Service);
                    }
                    else
                    {
                        MessengerInstance.Send(new ShowExceptionMessage_Message($"Το επιλεγμένο πακέτο υπάρχει ήδη στον πελάτη {c.Surename} {c.Name}"));
                        continue;
                    }
                    hasCustomers = true;
                }
            }
            if (!hasCustomers)
                MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ επιλέξτε πελάτες"));
            else
                PersonalWr.Services.Add(SelectedServiceViewModel.Service);
        }

        private bool CanAddService()
        {
            return SelectedServiceViewModel != null && SelectedServiceViewModel.Service.Id == 0; ;
        }

        private bool CanClearService()
        {
            return SelectedServiceViewModel != null;
        }

        private bool CanDeletePayment()
        {
            return SelectedPayment != null && AreContexesFree;
        }

        private bool CanSave()
        {
            if (PersonalWr == null || (!HasChanges && Payment.Amount == 0) || !AreContexesFree)
            {
                return false;
            }
            return AreBookingDataValid && ValidateServices() == null;
        }

        private async Task ClearBooking()
        {
            InitializeBooking(await CreateNewBooking());
        }

        private void ClearService()
        {
            Type t = SelectedServiceViewModel.GetType();
            int index = Templates.IndexOf(SelectedServiceViewModel);
            SelectedServiceViewModel = (ServiceViewModel)Activator.CreateInstance(t, this);
            Templates[index] = SelectedServiceViewModel;
        }

        private async Task<Personal_Booking> CreateNewBooking()
        {
            return new Personal_Booking { User = await MainViewModel.StartingRepository.GetByIdAsync<User>(StaticResources.User.Id, true) };
        }

        private void DeletePayment()
        {
            GenericRepository.Delete(SelectedPayment);
        }

        private void EditService()
        {
            SelectedServiceViewModel = Templates.Where(t => t.Service.Title == SelectedService.Title).FirstOrDefault();
            SelectedServiceViewModel.Service = SelectedService;
        }

        private string GetBookingDataValidationError(string propertyName)
        {
            string error = null;

            //Reservation.OnPropertyChanged("CanAddRows");
            switch (propertyName)
            {
                case nameof(PersonalWr):
                    error = ValidatePersonalWr();
                    break;

                case nameof(Payment):
                    error = ValidatePayment();
                    break;
            }
            return error;
        }

        private void GetServices()
        {
            Services.Clear();
            if (SelectedCustomer == null)
            {
                foreach (var c in PersonalWr.CustomerWrappers)
                {
                    foreach (var s in c.Services)
                    {
                        if (!Services.Contains(s))
                        {
                            Services.Add(s);
                        }
                    }
                }
            }
            else
            {
                Services.Clear();
                foreach (var s in SelectedCustomer.Services)
                {
                    Services.Add(s);
                }
            }
        }

        private void InitializeBooking(Personal_Booking booking)
        {
            PersonalWr = new Personal_BookingWrapper(booking);

            PersonalWr.PropertyChanged += (s, e) =>
            {
                string x = e.PropertyName;
                if (!HasChanges)
                {
                    HasChanges = GenericRepository.HasChanges();
                    if (PersonalWr.Id == 0)
                    {
                        HasChanges = true;
                    }
                }
            };
        }

        private void LoadTemplates()
        {
            Templates = new List<ServiceViewModel>
            {
                new Plane_ViewModel(this),
                new Ferry_ViewModel(this),
                new Hotel_ViewModel(this),
                new Transfer_ViewModel(this),
                new GuideViewModel(this),
                new Optional_ViewModel(this),
            };
        }

        private void PersonalWr_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Personal_BookingWrapper.Customers))
            {
                GetServices();
            }
        }

        private async Task ResetAllRefreshableDataASync(bool makeNew = false)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                if (makeNew)
                    await MainViewModel.BasicDataManager.Refresh();
                if (GenericRepository == null)
                {
                    GenericRepository = MainViewModel.StartingRepository;
                }
                int userId = SelectedUserIndex >= 0 && Users != null && SelectedUserIndex < Users.Count ? Users[SelectedUserIndex].Id : -1;
                int partnerId = -1;
                if (PersonalWr.IsPartners && SelectedPartnerIndex > 0)
                {
                    partnerId = SelectedPartnerIndex >= 0 && Partners != null && SelectedPartnerIndex < Partners.Count ? Partners[SelectedPartnerIndex].Id : -1;
                }
                Users = BasicDataManager.Users;
                Partners = BasicDataManager.Partners;

                if (SelectedUserIndex < 0)
                {
                    SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == PersonalWr.User.Id).FirstOrDefault());
                }
                else
                {
                    SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == userId).FirstOrDefault());
                }
                if (PersonalWr.Id > 0 && PersonalWr.IsPartners)
                {
                    SelectedPartnerIndex = Partners.IndexOf(Partners.Where(p => p.Id == PersonalWr.Partner.Id).FirstOrDefault());
                    SelectedEmail = Emails.Where(e => e.EValue == PersonalWr.PartnerEmail).FirstOrDefault();
                }
                else
                {
                    SelectedPartnerIndex = Partners.IndexOf(Partners.Where(x => x.Id == partnerId).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                if (Payment.Amount > 0)
                {
                    PersonalWr.Payments.Add(new Payment { Amount = Payment.Amount, Comment = Payment.Comment, Date = DateTime.Now, PaymentMethod = Payment.PaymentMethod, User = await GenericRepository.GetByIdAsync<User>(StaticResources.User.Id), Checked = (Payment.PaymentMethod == PaymentMethod.Cash || Payment.PaymentMethod == PaymentMethod.Visa) ? (bool?)false : null });
                }

                if (PersonalWr.Id == 0)
                {
                    GenericRepository.Add(PersonalWr.Model);
                }
                foreach (var s in Services)
                {
                    if (s is PlaneService ps)
                    {
                        if (ps.Airline != null)
                            ps.Airline = GenericRepository.GetById<Airline>(ps.Airline.Id);
                    }
                    else if (s is HotelService hs)
                    {
                        if (hs.Hotel != null)
                            hs.Hotel = GenericRepository.GetById<Hotel>(hs.Hotel.Id);
                        if (hs.City != null)
                            hs.City = GenericRepository.GetById<City>(hs.City.Id);
                        if (hs.RoomType != null)
                            hs.RoomType = GenericRepository.GetById<RoomType>(hs.RoomType.Id);
                    }
                }

                await GenericRepository.SaveAsync();

                Payment = new Payment();
                BookedMessage = "H κράτηση απόθηκέυτηκε επιτυχώς";
                HasChanges = false;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
            }
        }

        private string ValidatePayment()
        {
            if (Payment.Amount > (PersonalWr.Remaining + 0.05m))
            {
                return "Το ποσό υπερβαίνει το υπόλοιπο της κράτησης";
            }

            if (Payment.Amount < 0)
            {
                return "Το ποσό πληρωμής δεν μπορεί να είναι αρνητικό";
            }
            if (Payment.Amount > (PersonalWr.Remaining + 0.05m))
            {
                return "Το ποσό πληρωμής υπερβαίνει το υπολειπόμενο ποσό";
            }
            return null;
        }

        private string ValidatePersonalWr()
        {
            return PersonalWr.ValidatePersonalBooking();
        }

        private string ValidateServices()
        {
            return null;
        }

        #endregion Methods
    }
}