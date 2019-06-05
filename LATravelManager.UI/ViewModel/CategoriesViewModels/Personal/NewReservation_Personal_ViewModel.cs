using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.ServiceViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Personal
{
    public class NewReservation_Personal_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public NewReservation_Personal_ViewModel(MainViewModel mainViewModel)
        {
            StartingRepository = mainViewModel.StartingRepository;
            BasicDataManager = mainViewModel.BasicDataManager;
            //Commands
            ClearBookingCommand = new RelayCommand(async () => { await ClearBooking(); });
            AddCustomerCommand = new RelayCommand(AddRandomCustomer);

            DeletePaymentCommand = new RelayCommand(DeletePayment, CanDeletePayment);
            ClearServiceCommand = new RelayCommand(ClearService, CanClearService);
            AddServiceCommand = new RelayCommand(AddService, CanAddService);
            SaveCommand = new RelayCommand(async () => { await SaveAsync(); }, CanSave);
            Payment = new Payment();
            MainViewModel = mainViewModel;
        }

        #endregion Constructors

        #region Fields

        private bool _All;

        private string _ErrorsInDatagrid;

        private ObservableCollection<Partner> _Partners;

        private Personal_BookingWrapper _PersonalWr;

        private CustomerWrapper _SelectedCustomer;

        private int _SelectedPartnerIndex;

        private Payment _SelectedPayment;

        private ServiceViewModel _SelectedServiceViewModel;

        private ObservableCollection<Service> _Services;

        private List<ServiceViewModel> _Templates;

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

        public BasicDataManager BasicDataManager { get; private set; }

        public RelayCommand ClearBookingCommand { get; }

        public RelayCommand ClearServiceCommand { get; set; }

        public RelayCommand DeletePaymentCommand { get; }

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

        public GenericRepository StartingRepository { get; private set; }

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

        private bool AreContexesFree => (BasicDataManager != null && BasicDataManager.IsContextAvailable) && (StartingRepository != null && StartingRepository.IsContextAvailable);

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                if (id > 0)
                {
                    StartingRepository = new GenericRepository();
                }
                Personal_Booking booking = id > 0
                      ? await StartingRepository.GetFullPersonalBookingByIdAsync(id)
                      : await CreateNewBooking();

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
            return true;
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
            return true;
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
            StartingRepository.Delete(SelectedPayment);
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
                    HasChanges = StartingRepository.HasChanges();
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
                if (StartingRepository == null)
                {
                    StartingRepository = MainViewModel.StartingRepository;
                }

                int partnerId = SelectedPartnerIndex >= 0 && Partners != null && SelectedPartnerIndex < Partners.Count ? Partners[SelectedPartnerIndex].Id : -1;
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




        private string _BookedMessage;


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

        private async Task SaveAsync()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                if (Payment.Amount > 0)
                {
                    PersonalWr.Payments.Add(new Payment { Amount = Payment.Amount, Comment = Payment.Comment, Date = DateTime.Now, PaymentMethod = Payment.PaymentMethod, User = await StartingRepository.GetByIdAsync<User>(StaticResources.User.Id) });
                }

                if (PersonalWr.Id == 0)
                {
                    StartingRepository.Add(PersonalWr.Model);
                }

                await StartingRepository.SaveAsync();

                Payment = new Payment();
                BookedMessage = "H κράτηση αποθηκέυτηκε επιτυχώς";
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

        #endregion Methods
    }
}