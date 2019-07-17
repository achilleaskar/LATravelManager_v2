using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class EconomicData_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public EconomicData_ViewModel(MainViewModel mainViewModel)
        {
            Excursions = new ObservableCollection<Excursion>();
            ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);

            PrintEconomicDataCommand = new RelayCommand(async () => { await PrintEconomicData(); }, CanPrintEconomicData);

            ShowEconomicDataCommand = new RelayCommand<string>(async (obj) => { await ShowEconomicData(obj); }, CanShowPayments);
            UsersList = new ObservableCollection<PaymentInfo>();
            From = To = DateTime.Today;
            MainViewModel = mainViewModel;
        }

        #endregion Constructors

        #region Fields

        private decimal _AlphaBank;
        private decimal _Cash;
        private bool _EnableDatesFilter;
        private decimal _Ethniki;
        private decimal _Eurobank;
        private int _ExcursionIndexBookingFilter;
        private ObservableCollection<Excursion> _Excursions;
        private DateTime _From;
        private bool _IsOk = true;
        private decimal _Peiraios;
        private ExcursionWrapper _SelectedExcursion;
        private Excursion _SelectedExcursionFilter;
        private PaymentInfo _SelectedUserList;

        private DateTime _To;

        private decimal _Total;
        private int _UserIndexBookingFilter;

        private ObservableCollection<User> _Users;

        private ObservableCollection<PaymentInfo> _UsersList;

        private decimal _VISA;
        private SearchBookingsHelper SearchBookingsHelper;

        #endregion Fields

        #region Properties

        public decimal AlphaBank
        {
            get
            {
                return _AlphaBank;
            }

            set
            {
                if (_AlphaBank == value)
                {
                    return;
                }

                _AlphaBank = value;
                RaisePropertyChanged();
            }
        }

        public decimal Cash
        {
            get
            {
                return _Cash;
            }

            set
            {
                if (_Cash == value)
                {
                    return;
                }

                _Cash = value;
                RaisePropertyChanged();
            }
        }

        public GenericRepository Context { get; set; }

        public bool EnableDatesFilter
        {
            get
            {
                return _EnableDatesFilter;
            }

            set
            {
                if (_EnableDatesFilter == value)
                {
                    return;
                }

                _EnableDatesFilter = value;
                RaisePropertyChanged();
            }
        }

        public decimal Ethniki
        {
            get
            {
                return _Ethniki;
            }

            set
            {
                if (_Ethniki == value)
                {
                    return;
                }

                _Ethniki = value;
                RaisePropertyChanged();
            }
        }

        public decimal Eurobank
        {
            get
            {
                return _Eurobank;
            }

            set
            {
                if (_Eurobank == value)
                {
                    return;
                }

                _Eurobank = value;
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
                RaisePropertyChanged();
            }
        }

        public ICollectionView ExcursionsCollectionView { get; set; }

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
                RaisePropertyChanged();
            }
        }

        public bool IsOk
        {
            get
            {
                return _IsOk;
            }

            set
            {
                if (_IsOk == value)
                {
                    return;
                }

                _IsOk = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public decimal Peiraios
        {
            get
            {
                return _Peiraios;
            }

            set
            {
                if (_Peiraios == value)
                {
                    return;
                }

                _Peiraios = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand PrintEconomicDataCommand { get; set; }

        public ExcursionWrapper SelectedExcursion
        {
            get
            {
                return _SelectedExcursion;
            }

            set
            {
                if (_SelectedExcursion == value)
                {
                    return;
                }

                _SelectedExcursion = value;
                RaisePropertyChanged();
            }
        }

        public Excursion SelectedExcursionFilter
        {
            get
            {
                return _SelectedExcursionFilter;
            }

            set
            {
                if (_SelectedExcursionFilter == value)
                {
                    return;
                }

                _SelectedExcursionFilter = value;
                RaisePropertyChanged();
            }
        }

        public PaymentInfo SelectedUserList
        {
            get
            {
                return _SelectedUserList;
            }

            set
            {
                if (_SelectedUserList == value)
                {
                    return;
                }

                _SelectedUserList = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<string> ShowEconomicDataCommand { get; set; }

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
                RaisePropertyChanged();
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

        public ObservableCollection<PaymentInfo> UsersList
        {
            get
            {
                return _UsersList;
            }

            set
            {
                if (_UsersList == value)
                {
                    return;
                }

                _UsersList = value;
                RaisePropertyChanged();
            }
        }

        public decimal VISA
        {
            get
            {
                return _VISA;
            }

            set
            {
                if (_VISA == value)
                {
                    return;
                }

                _VISA = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                await ReloadAsync();
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

        public override async Task ReloadAsync()
        {
            if (Context != null && !Context.IsTaskOk)
            {
                await Context.LastTask;
            }
            Context = new GenericRepository();
            int userId = UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0;
            int excursionId = ExcursionIndexBookingFilter > 0 ? Excursions[ExcursionIndexBookingFilter - 1].Id : 0;

            Users = MainViewModel.BasicDataManager.Users;// new ObservableCollection<User>(await Context.GetAllAsyncSortedByName<User>());
            Excursions = MainViewModel.BasicDataManager.Excursions;//new ObservableCollection<Excursion>((await Context.GetAllAsync<Excursion>()).OrderBy(e => e.ExcursionDates.OrderBy(ed => ed.CheckIn).FirstOrDefault().CheckIn));
            ShowEconomicDataCommand.RaiseCanExecuteChanged();
            ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);
            ExcursionsCollectionView.Refresh();

            UserIndexBookingFilter = userId > 0 ? Users.IndexOf(Users.Where(u => u.Id == userId).FirstOrDefault()) + 1 : 0;
            ExcursionIndexBookingFilter = excursionId > 0 ? Excursions.IndexOf(Excursions.Where(e => e.Id == excursionId).FirstOrDefault()) + 1 : 0;
        }

        private bool CanPrintEconomicData()
        {
            return UsersList != null && UsersList.Count > 0;
        }

        private bool CanShowPayments(string arg)
        {
            return IsOk;
        }

        private async Task PrintEconomicData()
        {
            //Lists listmanager = new Lists(Excursions[ExcursionIndexBookingFilter - 1], CheckIn, CheckOut);
            //await listmanager.LoadAsync();
            //await listmanager.PrintAllRoomingLists();
            await Task.Delay(0);
        }

        private async Task ShowEconomicData(string parameter)
        {
            try
            {
                IsOk = false;
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                await ReloadAsync();
                if (SearchBookingsHelper == null)
                    SearchBookingsHelper = new SearchBookingsHelper(Context);
                DateTime dateLimit = SearchBookingsHelper.GetDateLimit(parameter);
                UsersList.Clear();
                Total = Cash = Peiraios = Ethniki = Eurobank = AlphaBank = VISA = 0;
                List<Payment> list = (await Context.GetAllPaymentsFiltered(ExcursionIndexBookingFilter > 0 ? Excursions[ExcursionIndexBookingFilter - 1].Id : 0, UserIndexBookingFilter > 0 ? Users[UserIndexBookingFilter - 1].Id : 0, dateLimit, EnableDatesFilter, From, To)).ToList();
                foreach (Payment payment in list)
                {
                    Total += payment.Amount;

                    switch (payment.PaymentMethod)
                    {
                        case 0:
                            Cash += payment.Amount;
                            break;

                        case 1:
                            Peiraios += payment.Amount;
                            break;

                        case 2:
                            Ethniki += payment.Amount;
                            break;

                        case 3:
                            Eurobank += payment.Amount;
                            break;

                        case 4:
                            AlphaBank += payment.Amount;
                            break;

                        case 5:
                            VISA += payment.Amount;
                            break;
                    }
                    if (!UsersList.Any(u => u.User.Id == payment.User.Id))
                    {
                        UsersList.Add(new PaymentInfo(MainViewModel,this) { User = Users.Where(u => u.Id == payment.User.Id).FirstOrDefault() });
                    }
                    UsersList.Where(u => u.User.Id == payment.User.Id).FirstOrDefault().Payments.Add(payment);
                }

                Parallel.ForEach(UsersList, wr => wr.LoadAmounts());
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
                IsOk = true;
            }
        }

        #endregion Methods
    }
}