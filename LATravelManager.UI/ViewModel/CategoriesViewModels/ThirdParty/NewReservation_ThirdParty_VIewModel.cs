using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
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

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty
{
    public class NewReservation_ThirdParty_VIewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public NewReservation_ThirdParty_VIewModel(MainViewModel mainViewModel)
        {
            GenericRepository = mainViewModel.StartingRepository;
            BasicDataManager = mainViewModel.BasicDataManager;
            //Commands
            ClearBookingCommand = new RelayCommand(async () => { await ClearBooking(); });
            AddCustomerCommand = new RelayCommand(AddRandomCustomer);
            UploadFileCommand = new RelayCommand(UploadFile);
            OpenFileCommand = new RelayCommand(OpenFile, CanOpenFile);
            PrintRecieptCommand = new RelayCommand(PrintReciept);

            DeleteSelectedCustomersCommand = new RelayCommand(DeleteSelectedCustomers, CanDeleteCustomers);
            UpdateAllCommand = new RelayCommand(async () => { await UpdateAll(); }, CanUpdateAll);

            DeletePaymentCommand = new RelayCommand(DeletePayment, CanDeletePayment);
            SaveCommand = new RelayCommand(async () => { await SaveAsync(); }, CanSave);
            Payment = new Payment();
            MainViewModel = mainViewModel;
            ToggleDisabilityCommand = new RelayCommand(ToggleDisability, CanToggleDisability);
        }

        public DocumentsManagement DocumentsManagement { get; set; }

        private void PrintReciept()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Παρακαλώ επιλέξτε πελάτη");
                return;
            }
            if (DocumentsManagement == null)
            {
                DocumentsManagement = new DocumentsManagement(GenericRepository);
            }
            DocumentsManagement.PrintPaymentsReciept(SelectedPayment, SelectedCustomer.Model);
        }

        public RelayCommand PrintRecieptCommand { get; set; }

        public RelayCommand ToggleDisabilityCommand { get; set; }

        private void ToggleDisability()
        {
            if (ThirdPartyWr.Disabled)
            {
                ThirdPartyWr.Disabled = false;
            }
            else
            {
                ThirdPartyWr.DisableDate = DateTime.Now;
                ThirdPartyWr.DisabledBy = GenericRepository.GetById<User>(StaticResources.User.Id);
                ThirdPartyWr.Disabled = true;
            }
        }

        private bool CanToggleDisability()
        {
            return ThirdPartyWr != null && ThirdPartyWr.Id > 0 && !string.IsNullOrEmpty(ThirdPartyWr.CancelReason);
        }

        #endregion Constructors

        #region Fields

        private readonly string[] ValidateRoomsProperties =
               {
            nameof(ThirdPartyWr),nameof(Payment)
        };

        private string _BookedMessage;

        private string _ErrorsInDatagrid;

        private ObservableCollection<Partner> _Partners;

        private Payment _Payment;

        private CustomerWrapper _SelectedCustomer;

        private int _SelectedPartnerIndex;

        private Payment _SelectedPayment;

        private int _SelectedUserIndex;

        private ThirdParty_Booking_Wrapper _ThirdPartyWr;

        private ObservableCollection<User> _Users;

        #endregion Fields

        #region Properties

        public RelayCommand AddCustomerCommand { get; }

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

        public RelayCommand DeletePaymentCommand { get; }

        public RelayCommand DeleteSelectedCustomersCommand { get; }

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

        public RelayCommand OpenFileCommand { get; }

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

        public Payment Payment
        {
            get
            {
                return _Payment;
            }

            set
            {
                if (_Payment == value)
                {
                    return;
                }

                _Payment = value;
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
                if (SelectedPartnerIndex >= 0 && (ThirdPartyWr.Partner == null || (ThirdPartyWr.Partner != null && ThirdPartyWr.Partner.Id != Partners[SelectedPartnerIndex].Id)))
                {
                    ThirdPartyWr.Partner = GenericRepository.GetById<Partner>(Partners[SelectedPartnerIndex].Id);
                }
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
                if (SelectedUserIndex >= 0 && (ThirdPartyWr.User == null || (ThirdPartyWr.User != null && ThirdPartyWr.User.Id != Users[SelectedUserIndex].Id)))
                {
                    ThirdPartyWr.User = GenericRepository.GetById<User>(Users[SelectedUserIndex].Id);
                }
                RaisePropertyChanged();
            }
        }

        public GenericRepository GenericRepository { get; private set; }

        public ThirdParty_Booking_Wrapper ThirdPartyWr
        {
            get
            {
                return _ThirdPartyWr;
            }

            set
            {
                if (_ThirdPartyWr == value)
                {
                    return;
                }

                _ThirdPartyWr = value;

                RaisePropertyChanged();
            }
        }

        public RelayCommand UpdateAllCommand { get; set; }

        public RelayCommand UploadFileCommand { get; }

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

        public void DeleteSelectedCustomers()
        {
            List<CustomerWrapper> toRemove = new List<CustomerWrapper>();
            bool showMessage = false;
            foreach (CustomerWrapper customer in ThirdPartyWr.CustomerWrappers)
            {
                if (customer.IsSelected)
                {
                    if (!customer.Handled)
                    {
                        toRemove.Add(customer);
                    }
                    else
                    {
                        showMessage = true;
                    }
                }
            }
            foreach (CustomerWrapper c in toRemove)
            {
                ThirdPartyWr.CustomerWrappers.Remove(c);
            }
            if (showMessage)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Καπόιοι πελάτες δεν διαγράφηκαν επειδή συμμετέχουν σε κράτηση. Παλακαλώ κάντε τους CheckOut και δοκιμάστε ξανά!"));
            }
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                if (id > 0)
                {
                    GenericRepository = new GenericRepository();
                }
                ThirdParty_Booking booking = id > 0
                      ? await GenericRepository.GetFullThirdPartyBookingByIdAsync(id)
                      : await CreateNewBooking();

                InitializeBooking(booking);

                await ResetAllRefreshableDataASync();
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
            ThirdPartyWr.CustomerWrappers.Add(new CustomerWrapper() { Age = 18, Name = name, Surename = surename, Price = 35, Tel = "6981001676", StartingPlace = "Αθήνα", PassportNum = passport, DOB = new DateTime(1991, 10, 4) });
        }

        private bool CanDeleteCustomers()
        {
            return ThirdPartyWr.CustomerWrappers.Any(c => c.IsSelected);
        }

        private bool CanDeletePayment()
        {
            return SelectedPayment != null && AreContexesFree;
        }

        private bool CanOpenFile()
        {
            return ThirdPartyWr != null && ThirdPartyWr.File != null && ThirdPartyWr.File.Content != null;
        }

        private bool CanSave()
        {
            if (ThirdPartyWr == null || (!HasChanges && Payment.Amount == 0) || !AreContexesFree)
            {
                return false;
            }
            return AreBookingDataValid;
        }

        private bool CanUpdateAll()
        {
            return AreContexesFree;
        }

        private async Task ClearBooking()
        {
            MessageBoxResult result = MessageBoxResult.Yes;
            if (HasChanges)
            {
                result = MessageBox.Show("Υπάρχουν μη απόθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
            if (result == MessageBoxResult.Yes)
            {
                InitializeBooking(await CreateNewBooking());

                Payment = new Payment();
                BookedMessage = string.Empty;
                SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == StaticResources.User.Id).FirstOrDefault());
                SelectedPartnerIndex = -1;
                GenericRepository.RollBack();
            }
        }

        private async Task<ThirdParty_Booking> CreateNewBooking()
        {
            return new ThirdParty_Booking { User = await MainViewModel.StartingRepository.GetByIdAsync<User>(StaticResources.User.Id, true) };
        }

        private void DeletePayment()
        {
            GenericRepository.Delete(SelectedPayment);
        }

        private string GetBookingDataValidationError(string propertyName)
        {
            string error = null;

            //Reservation.OnPropertyChanged("CanAddRows");
            switch (propertyName)
            {
                case nameof(ThirdPartyWr):
                    error = ValidateThirdPartyWr();
                    break;

                case nameof(Payment):
                    error = ValidatePayment();
                    break;
            }
            return error;
        }

        private void InitializeBooking(ThirdParty_Booking booking)
        {
            ThirdPartyWr = new ThirdParty_Booking_Wrapper(booking);

            ThirdPartyWr.PropertyChanged += (s, e) =>
            {
                string x = e.PropertyName;
                if (!HasChanges)
                {
                    HasChanges = GenericRepository.HasChanges();
                    if (ThirdPartyWr.Id == 0)
                    {
                        HasChanges = true;
                    }
                }
            };
            if (booking.Id == 0)
            {
                SelectedPartnerIndex = -1;
                ThirdPartyWr.CheckIn = DateTime.Today;
            }
            if (booking.Id > 0)
            {
                ThirdPartyWr.FullPrice = ThirdPartyWr.NetPrice + ThirdPartyWr.Commision;
            }
        }

        private void OpenFile()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            string outputpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Directory.CreateDirectory(outputpath + @"\Προφόρμες");
            string path = GetPath(ThirdPartyWr.File.FileName, outputpath + @"\Προφόρμες\");
            File.WriteAllBytes(path, ThirdPartyWr.File.Content);

            Mouse.OverrideCursor = Cursors.Arrow;

            Process.Start(path);
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
                if (SelectedUserIndex < 0)
                {
                    SelectedUserIndex = Users.IndexOf(Users.Where(u => u.Id == StaticResources.User.Id).FirstOrDefault());
                }

                int userId = ThirdPartyWr.User != null ? ThirdPartyWr.User.Id : -1;
                //int userId = SelectedUserIndex >= 0 && Users != null && SelectedUserIndex < Users.Count ? Users[SelectedUserIndex].Id : -1;
                //int partnerId = SelectedPartnerIndex >= 0 && Partners != null && SelectedPartnerIndex < Partners.Count ? Partners[SelectedPartnerIndex].Id : -1; int userId = SelectedUserIndex >= 0 && Users != null && SelectedUserIndex < Users.Count ? Users[SelectedUserIndex].Id : -1;
                int partnerId = ThirdPartyWr.Partner != null ? ThirdPartyWr.Partner.Id : -1;
                Users = BasicDataManager.Users;
                Partners = BasicDataManager.Partners;

                if (SelectedUserIndex < 0)
                {
                    SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == ThirdPartyWr.User.Id).FirstOrDefault());
                }
                else
                {
                    SelectedUserIndex = Users.IndexOf(Users.Where(x => x.Id == userId).FirstOrDefault());
                }
                SelectedPartnerIndex = Partners.IndexOf(Partners.Where(x => x.Id == partnerId).FirstOrDefault());
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
                Mouse.OverrideCursor = Cursors.Wait;

                MessengerInstance.Send(new IsBusyChangedMessage(true));

                if (Payment.Amount > 0)
                {
                    ThirdPartyWr.Payments.Add(new Payment { Amount = Payment.Amount, Outgoing = Payment.Outgoing, Comment = Payment.Comment, Date = DateTime.Now, PaymentMethod = Payment.PaymentMethod, User = await GenericRepository.GetByIdAsync<User>(StaticResources.User.Id), Checked = (Payment.PaymentMethod == 0 || Payment.PaymentMethod == 5) ? (bool?)false : null });
                }

                if (ThirdPartyWr.Id == 0)
                {
                    GenericRepository.Add(ThirdPartyWr.Model);
                }

                await GenericRepository.SaveAsync();

                Payment = new Payment();
                BookedMessage = "H κράτηση απόθηκέυτηκε επιτυχώς";
                HasChanges = false;
                Mouse.OverrideCursor = Cursors.Arrow;
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

        private async Task UpdateAll()
        {
            await ResetAllRefreshableDataASync(true);
            UpdateAllCommand.RaiseCanExecuteChanged();
        }

        private void UploadFile()
        {
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
                        ThirdPartyWr.File = new CustomFile { Content = File.ReadAllBytes(fileName), FileName = file.Name };
                    }
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private string ValidatePayment()
        {
            if ((!Payment.Outgoing && Payment.Amount > (ThirdPartyWr.Remaining + 0.05m)) || (Payment.Outgoing && Payment.Amount > (ThirdPartyWr.NetRemaining + 0.05m)))
            {
                return "Το ποσό υπερβαίνει το υπολοιπόμενο ποσό";
            }
            if (Payment.Amount < 0)
            {
                return "Το ποσό πληρωμής δεν μπορεί να είναι αρνητικό";
            }

            return null;
        }

        private string ValidateThirdPartyWr()
        {
            return ThirdPartyWr.ValidateThirdPartyBooking();
        }

        #endregion Methods
    }
}