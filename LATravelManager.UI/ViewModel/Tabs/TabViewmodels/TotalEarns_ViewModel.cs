using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class TotalEarns_ViewModel : MyViewModelBase
    {
        #region Constructors

        public TotalEarns_ViewModel(MainViewModel mainViewModel)
        {
            ShowIncomesCommand = new RelayCommand(async () => { await ShowIncomes(); }, CanShowIncomes);
            From = To = DateTime.Today;
            MainViewModel = mainViewModel;
            Load();
        }

        #endregion Constructors

        #region Fields

        private decimal _Commission;

        private bool _Completed;

        private bool _CompletedIncomeFilter;

        private int _Customers;

        private ObservableCollection<Customer> _CustomersToAdd = new ObservableCollection<Customer>();

        private int _DepartmentIndexBookingFilter;

        private bool _EnableFromFilter;

        private bool _EnableToFilter;

        private int _ExcursionCategoryIndexBookingFilter;

        private int _ExcursionIndexBookingFilter;

        private ObservableCollection<Excursion> _Excursions;

        private ICollectionView _ExcursionsCollectionView;

        private DateTime _From;

        private DateTime _FromDateTime = DateTime.Today;

        private GenericRepository _IncomesContext;

        private decimal _Net;

        private int _PartnerIndexBookingFilter;

        private ObservableCollection<Partner> _Partners;

        private DateTime _To;

        private DateTime _ToDateTime = DateTime.Today;

        private decimal _Total;

        private int _UserIndexBookingFilter;

        private ObservableCollection<User> _Users;

        #endregion Fields

        #region Properties

        public decimal Commission
        {
            get
            {
                return _Commission;
            }

            set
            {
                if (_Commission == value)
                {
                    return;
                }

                _Commission = value;
                RaisePropertyChanged();
            }
        }

        public bool Completed
        {
            get
            {
                return _Completed;
            }

            set
            {
                if (_Completed == value)
                {
                    return;
                }

                _Completed = value;
                //ExcursionsDepartureCollectionView.Refresh();

                RaisePropertyChanged();
            }
        }

        public bool CompletedIncomeFilter
        {
            get
            {
                return _CompletedIncomeFilter;
            }

            set
            {
                if (_CompletedIncomeFilter == value)
                {
                    return;
                }

                _CompletedIncomeFilter = value;
                ExcursionsIncomeCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        public int Customers
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
            }
        }

        public ObservableCollection<Customer> CustomersToAdd
        {
            get => _CustomersToAdd;
            set
            {
                if (_CustomersToAdd != value)
                {
                    _CustomersToAdd = value;
                    RaisePropertyChanged(nameof(CustomersToAdd));
                }
            }
        }

        public int DepartmentIndexBookingFilter
        {
            get
            {
                return _DepartmentIndexBookingFilter;
            }

            set
            {
                if (_DepartmentIndexBookingFilter == value)
                {
                    return;
                }

                _DepartmentIndexBookingFilter = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableFromFilter
        {
            get
            {
                return _EnableFromFilter;
            }

            set
            {
                if (_EnableFromFilter == value)
                {
                    return;
                }

                _EnableFromFilter = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableToFilter
        {
            get
            {
                return _EnableToFilter;
            }

            set
            {
                if (_EnableToFilter == value)
                {
                    return;
                }

                _EnableToFilter = value;
                RaisePropertyChanged();
            }
        }

        public int ExcursionCategoryIndexBookingFilter
        {
            get
            {
                return _ExcursionCategoryIndexBookingFilter;
            }

            set
            {
                if (_ExcursionCategoryIndexBookingFilter == value)
                {
                    return;
                }

                _ExcursionCategoryIndexBookingFilter = value;
                RaisePropertyChanged();
            }
        }

        public int ExcursionIndexBookingFilter
        {
            get
            {
                return _ExcursionIndexBookingFilter;
            }

            set
            {
                if (_ExcursionIndexBookingFilter == value)
                {
                    return;
                }

                _ExcursionIndexBookingFilter = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Excursion> Excursions
        {
            get
            {
                return _Excursions;
            }

            set
            {
                if (_Excursions == value)
                {
                    return;
                }

                _Excursions = value;
                Excursions.Insert(0, new Excursion { ExcursionDates = new ObservableCollection<ExcursionDate> { new ExcursionDate { CheckIn = new DateTime() } }, Name = "Ολες" });

                ExcursionsIncomeCollectionView = CollectionViewSource.GetDefaultView(Excursions);
                ExcursionsIncomeCollectionView.Filter = IncomeExcursionsFilter;
                ExcursionsIncomeCollectionView.SortDescriptions.Add(new SortDescription("FirstDate", ListSortDirection.Ascending));

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ExcursionsIncomeCollectionView));
            }
        }

        public ICollectionView ExcursionsIncomeCollectionView
        {
            get
            {
                return _ExcursionsCollectionView;
            }

            set
            {
                if (_ExcursionsCollectionView == value)
                {
                    return;
                }

                _ExcursionsCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public DateTime From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                _From = value;
                if (value > To)
                {
                    To = value;
                }
                RaisePropertyChanged();
            }
        }

        public DateTime FromDateTime
        {
            get => _FromDateTime;
            set
            {
                if (_FromDateTime != value)
                {
                    _FromDateTime = value;
                    RaisePropertyChanged(nameof(FromDateTime));
                }
            }
        }

        public GenericRepository IncomesContext
        {
            get
            {
                return _IncomesContext;
            }

            set
            {
                if (_IncomesContext == value)
                {
                    return;
                }

                _IncomesContext = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public decimal Net
        {
            get
            {
                return _Net;
            }

            set
            {
                if (_Net == value)
                {
                    return;
                }

                _Net = value;
                RaisePropertyChanged();
            }
        }

        public int PartnerIndexBookingFilter
        {
            get
            {
                return _PartnerIndexBookingFilter;
            }

            set
            {
                if (_PartnerIndexBookingFilter == value)
                {
                    return;
                }

                _PartnerIndexBookingFilter = value;
                RaisePropertyChanged();
            }
        }

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

        public RelayCommand SaveTransfersCommand
        {
            get;
            private set;
        }

        public RelayCommand ShowIncomesCommand { get; set; }

        public DateTime To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }

                _To = value;
                if (value < From)
                {
                    From = value;
                }
                RaisePropertyChanged();
            }
        }

        public DateTime ToDateTime
        {
            get => _ToDateTime;
            set
            {
                if (_ToDateTime != value)
                {
                    _ToDateTime = value;
                    RaisePropertyChanged(nameof(ToDateTime));
                }
            }
        }

        public decimal Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged();
            }
        }

        public int UserIndexBookingFilter
        {
            get
            {
                return _UserIndexBookingFilter;
            }

            set
            {
                if (_UserIndexBookingFilter == value)
                {
                    return;
                }

                _UserIndexBookingFilter = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<User> Users
        {
            get
            {
                return _Users;
            }

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

        #endregion Properties

        #region Methods

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                Excursions = new ObservableCollection<Excursion>(MainViewModel.BasicDataManager.Excursions.OrderBy(e => e.ExcursionDates.OrderBy(ed => ed.CheckIn).FirstOrDefault().CheckIn));
                Users = new ObservableCollection<User>(MainViewModel.BasicDataManager.Users);
                Partners = new ObservableCollection<Partner>(MainViewModel.BasicDataManager.Partners);
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

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        //internal void AddTranferReservation()
        //{
        //    Window mw = Application.Current.MainWindow;
        //    mw.Hide();
        //    var brw = new BookRoomWindow(new Room { Id = -2, RoomType = new RoomType { Name = "Bus", Id = -2, MainCapacity = 64 }, Hotel = new Hotel { Name = "TANSFER", Id = -2 } }, FromDateTime, ToDateTime, mw);
        //    brw.ShowDialog();
        //}

        private bool CanShowIncomes()
        {
            return ExcursionCategoryIndexBookingFilter != 3 && ExcursionCategoryIndexBookingFilter != 5;
        }

        private bool IncomeExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return CompletedIncomeFilter || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today);
        }

        private bool InfoExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return excursion.Id > 0 && (Completed || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today));
        }

        private async Task ShowIncomes()
        {
            Total = Net = Commission = Customers = 0;
            IncomesContext = new GenericRepository();
            List<BookingWrapper> bookings = (await IncomesContext.GetAllBookingsFiltered(
                ExcursionIndexBookingFilter == 0 ? -1 : ((Excursion)ExcursionsIncomeCollectionView.CurrentItem).Id,
                ExcursionCategoryIndexBookingFilter == 0 ? -1 : ExcursionCategoryIndexBookingFilter - 1,
                DepartmentIndexBookingFilter,
                UserIndexBookingFilter == 0 ? -1 : Users[UserIndexBookingFilter - 1].Id,
                PartnerIndexBookingFilter == 0 ? -1 : Partners[PartnerIndexBookingFilter - 1].Id,
                EnableFromFilter ? From : CompletedIncomeFilter ? DateTime.MinValue : DateTime.Today,
                EnableToFilter ? To : DateTime.MaxValue)).Select(b => new BookingWrapper(b)).ToList();

            // decimal pel = 0, chara = 0, plat = 0;

            //foreach (var b in bookings)
            //{
            //    b.CalculateRemainingAmount();
            //    if (IncomesContext.HasChanges())
            //    {
            //        await IncomesContext.SaveAsync();
            //    }
            //}

            //await IncomesContext.SaveAsync();

            foreach (var b in bookings)
            {
                if (b.IsPartners && b.Partner.Id == 219)
                {
                }
                //if (b.ReservationsInBooking.Count > 1 && b.ReservationsInBooking.Any(r=>r.Room.Hotel.Id!=b.ReservationsInBooking[0].Room.Hotel.Id))
                //{
                //}
                //else
                //{
                //    if (b.ReservationsInBooking[0].Room.Hotel.Id == 105)
                //    {
                //        chara += b.FullPrice;
                //    }
                //    else if (b.ReservationsInBooking[0].Room.Hotel.Id == 117)
                //    {
                //        plat += b.FullPrice;
                //    }
                //    else if (b.ReservationsInBooking[0].Room.Hotel.Id == 103)
                //    {
                //        pel += b.FullPrice;
                //    }
                //}
                _Total += b.FullPrice;
                foreach (var r in b.ReservationsInBooking)
                {
                    _Customers += r.CustomersList.Count;
                }
                if (b.IsPartners && b.NetPrice > 2)
                {
                    _Net += b.NetPrice;
                }
                else if (b.FullPrice > 2)
                {
                    _Net += b.FullPrice;
                }
            }
            _Commission = Total - Net;
            RaisePropertyChanged(nameof(Total));
            RaisePropertyChanged(nameof(Customers));
            RaisePropertyChanged(nameof(Net));
            RaisePropertyChanged(nameof(Commission));
        }

        #endregion Methods
    }
}