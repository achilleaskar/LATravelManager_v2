using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.Model.Services;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Personal
{
    public class NewReservation_Personal_ViewModel : MyViewModelBase
    {

        #region Constructors

        public NewReservation_Personal_ViewModel()
        {
            GenericRepository = new GenericRepository();

            //Commands
            ClearBookingCommand = new RelayCommand(async () => { await ClearBooking(); });
            AddCustomerCommand = new RelayCommand(AddRandomCustomer);

            DeletePaymentCommand = new RelayCommand(DeletePayment, CanDeletePayment);
            ClearServiceCommand = new RelayCommand(ClearService, CanClearService);
            AddServiceCommand = new RelayCommand(AddService, CanAddService);
            SaveCommand = new RelayCommand(async () => { await SaveAsync(); }, CanSave);
            Payment = new Payment();
            LoadTemplates();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Airline> _AirLines;

        private string _ErrorsInDatagrid;

        private ObservableCollection<Partner> _Partners;

        private Personal_BookingWrapper _PersonalWr;

        private ObservableCollection<RoomType> _RoomTypes;

        private Airline _SelectedAirline;

        private Customer _SelectedCustomer;

        private int _SelectedPartnerIndex;

        private Payment _SelectedPayment;

        private Service _SelectedService;

        private List<Service> _Templates;

        #endregion Fields

        #region Properties

        public RelayCommand AddCustomerCommand { get; }

        public RelayCommand AddServiceCommand { get; set; }

        public ObservableCollection<Airline> AirLines
        {
            get
            {
                return _AirLines;
            }

            set
            {
                if (_AirLines == value)
                {
                    return;
                }

                _AirLines = value;
                RaisePropertyChanged();
            }
        }

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

        public GenericRepository GenericRepository { get; private set; }

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
                RaisePropertyChanged();
            }
        }

        public GenericRepository RefreshableContext { get; private set; }

        public ObservableCollection<RoomType> RoomTypes
        {
            get => _RoomTypes;

            set
            {
                if (_RoomTypes == value)
                {
                    return;
                }

                _RoomTypes = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; }

        public Airline SelectedAirline
        {
            get
            {
                return _SelectedAirline;
            }

            set
            {
                if (_SelectedAirline == value)
                {
                    return;
                }

                _SelectedAirline = value;
                if (value != null && SelectedService is PlaneService p)
                {
                    p.Airline = GenericRepository.GetById<Airline>(value.Id);
                }
                else
                {
                    throw new Exception("Μπήκε ενώ δεν έπρεπε :/");
                }
                RaisePropertyChanged();
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

        public List<Service> Templates
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

        private bool AreContexesFree => (RefreshableContext != null && RefreshableContext.IsContextAvailable) && (GenericRepository != null && GenericRepository.IsContextAvailable);

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            try
            {
                Personal_Booking booking = id > 0
                    ? await GenericRepository.GetFullPersonalBookingByIdAsync(id)
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
            PersonalWr.Customers.Add(new Customer() { Age = 18, Name = name, Surename = surename, Price = 35, Tel = "6981001676", StartingPlace = "Αθήνα", PassportNum = passport, DOB = new DateTime(1991, 10, 4) });
        }

        private void AddService()
        {
            PersonalWr.Services.Add(SelectedService);
        }

        private bool CanAddService()
        {
            return true;
        }

        private bool CanClearService()
        {
            return SelectedService != null;
        }

        private bool CanDeletePayment()
        {
            return SelectedPayment != null && AreContexesFree;
        }

        private bool CanSave()
        {
            return true;
        }

        private Task ClearBooking()
        {
            throw new NotImplementedException();
        }

        private void ClearService()
        {
            Type t = SelectedService.GetType();
            int index = Templates.IndexOf(SelectedService);
            SelectedService = (Service)Activator.CreateInstance(t);
            Templates[index] = SelectedService;
        }
        private async Task<Personal_Booking> CreateNewBooking()
        {
            return new Personal_Booking { User = await GenericRepository.GetByIdAsync<User>(StaticResources.User.Id) };
        }

        private void DeletePayment()
        {
            GenericRepository.Delete(SelectedPayment);
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
            Templates = new List<Service>
            {
                new PlaneService(),
                new FerryService(),
                new HotelService(),
                new CruiseService(),
                new BusService(),
                new GuideService(),
                new OptionalService(),
                new TransferService()
            };
        }
        private async Task ResetAllRefreshableDataASync()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                if (RefreshableContext == null)
                {
                    RefreshableContext = GenericRepository;
                }
                else
                {
                    if (RefreshableContext != null && !RefreshableContext.IsTaskOk)
                    {
                        await RefreshableContext.LastTask;
                    }
                    RefreshableContext = new GenericRepository();
                }
                int partnerId = SelectedPartnerIndex >= 0 && Partners != null && SelectedPartnerIndex < Partners.Count ? Partners[SelectedPartnerIndex].Id : -1;
                // Hotels = new ObservableCollection<Hotel>(await RefreshableContext.GetAllHotelsInCityAsync(SelectedExcursion.Destinations[0].Id));
                RoomTypes = new ObservableCollection<RoomType>(await RefreshableContext.GetAllAsync<RoomType>());
                Partners = new ObservableCollection<Partner>(await RefreshableContext.GetAllAsyncSortedByName<Partner>());
                AirLines = new ObservableCollection<Airline>(await RefreshableContext.GetAllAsync<Airline>());

                SelectedPartnerIndex = Partners.IndexOf(Partners.Where(x => x.Id == partnerId).FirstOrDefault());

                if (PersonalWr.Id > 0 && PersonalWr.IsPartners)
                {
                    SelectedPartnerIndex = Partners.IndexOf(Partners.Where(p => p.Id == PersonalWr.Partner.Id).FirstOrDefault());
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
            GenericRepository.Add(PersonalWr.Model);
            await GenericRepository.SaveAsync();
        }

        #endregion Methods

    }
}