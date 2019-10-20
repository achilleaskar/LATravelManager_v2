﻿using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Views.Personal;
using LATravelManager.UI.Views.ThirdParty;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class AddIncomeOutcome_ViewModel : MyViewModelBase
    {
        #region Constructors

        public AddIncomeOutcome_ViewModel(MainViewModel mainViewModel)
        {
            EditBookingCommand = new RelayCommand(async () => { await EditBooking(); }, CanEditBooking);
            SearchForReservationsCommand = new RelayCommand(async () => { await SearchForReservations(); }, CanSearchForReservations);
            MainViewModel = mainViewModel;
            SaveTransactionCommand = new RelayCommand(async () => { await SaveTransaction(); }, CanSaveTransaction);
            ClearTransactionCommand = new RelayCommand(ClearTransaction, CanClearTransaction);
            Load();
        }

        private void ClearTransaction()
        {
            Transaction = new Transaction();
            Keyword = string.Empty;
            IdInt = 0;
            FilteredReservations.Clear();
            SelectedExcursion = null;
            SelectedReservation = null;
        }

        private bool CanClearTransaction()
        {
            return true;
        }

        private Excursion _SelectedExcursion;

        public Excursion SelectedExcursion
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

        private bool CanSaveTransaction()
        {
            if (Transaction.ExpenseBaseCategory == ExpenseBaseCategories.GroupExpense && SelectedExcursion == null)
            {
                return false;
            }
            if ((Transaction.ExpenseBaseCategory == ExpenseBaseCategories.PersonelExpense || Transaction.ExcursionExpenseCategory == ExcursionExpenseCategories.Booking) && SelectedReservation == null)
            {
                return false;
            }

            return Transaction != null &&
                !string.IsNullOrEmpty(Transaction.Description) &&
                Transaction.Description.Length > 3 &&
                Transaction.Amount > 0;
        }

        private async Task SaveTransaction()
        {
            if (SelectedReservation != null)
            {
                if (SelectedReservation.Booking != null)
                {
                    Transaction.Booking = SelectedReservation.Booking;
                    Transaction.PersonalBooking = null;
                    Transaction.ThirdPartyBooking = null;
                }
                else if (SelectedReservation.PersonalModel != null)
                {
                    Transaction.Booking = null;
                    Transaction.PersonalBooking = SelectedReservation.PersonalModel.Model;
                    Transaction.ThirdPartyBooking = null;
                }
                else if (SelectedReservation.ThirdPartyModel != null)
                {
                    Transaction.Booking = null;
                    Transaction.PersonalBooking = null;
                    Transaction.ThirdPartyBooking = SelectedReservation.ThirdPartyModel.Model;
                }
            }
            Transaction.User = await Context.GetByIdAsync<User>(Helpers.StaticResources.User.Id);
            Transaction.Excursion = SelectedExcursion ?? null;
            Context.Add(Transaction);
            await Context.SaveAsync();
            Transaction.RaisePropertyChanged("Saved");
        }

        public RelayCommand SaveTransactionCommand { get; set; }
        public RelayCommand ClearTransactionCommand { get; set; }

        #endregion Constructors

        #region Fields

        private bool _Completed;
        private ObservableCollection<Excursion> _Excursions;
        private ICollectionView _ExcursionsCollectionView;
        private ObservableCollection<ReservationWrapper> _FilteredReservations;

        private int _Grafeio;

        private int _Idword;

        private string _Keyword;

        private ReservationWrapper _SelectedReservation;

        private Transaction _Transaction;

        #endregion Fields

        #region Properties

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
                RaisePropertyChanged();
                ExcursionsCollectionView.Refresh();
            }
        }

        public GenericRepository Context { get; private set; }

        public RelayCommand EditBookingCommand { get; set; }

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

                if (Excursions != null && !Excursions.Any(e => e.Id == 0))
                {
                    Excursions.Insert(0, new Excursion { ExcursionDates = new ObservableCollection<ExcursionDate> { new ExcursionDate { CheckIn = new DateTime() } }, Name = "Όλες", Id = 0 });
                }

                ExcursionsCollectionView = CollectionViewSource.GetDefaultView(Excursions);
                ExcursionsCollectionView.Filter = CustomerExcursionsFilter;
                ExcursionsCollectionView.SortDescriptions.Add(new SortDescription("FirstDate", ListSortDirection.Ascending));

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ExcursionsCollectionView));
            }
        }

        public ICollectionView ExcursionsCollectionView
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

        public ObservableCollection<ReservationWrapper> FilteredReservations
        {
            get
            {
                return _FilteredReservations;
            }

            set
            {
                if (_FilteredReservations == value)
                {
                    return;
                }

                _FilteredReservations = value;
                RaisePropertyChanged();
            }
        }

        public int Grafeio
        {
            get
            {
                return _Grafeio;
            }

            set
            {
                if (_Grafeio == value)
                {
                    return;
                }

                _Grafeio = value;
                RaisePropertyChanged();
            }
        }

        public bool HasItem => SelectedReservation != null;

        public int IdInt
        {
            get
            {
                return _Idword;
            }

            set
            {
                if (_Idword == value)
                {
                    return;
                }

                _Idword = value;
                Keyword = string.Empty;
                RaisePropertyChanged();
            }
        }


        public bool IsOk { get; private set; }

        public string Keyword
        {
            get
            {
                return _Keyword;
            }

            set
            {
                if (_Keyword == value)
                {
                    return;
                }

                _Keyword = value;
                IdInt = 0;
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public RelayCommand SearchForReservationsCommand { get; set; }

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
                RaisePropertyChanged(nameof(HasItem));
            }
        }

        public Transaction Transaction
        {
            get
            {
                return _Transaction;
            }

            set
            {
                if (_Transaction == value)
                {
                    return;
                }

                _Transaction = value;
                RaisePropertyChanged();
                _Transaction.PropertyChanged += Transaction_PropertyChanged;
            }
        }

        #endregion Properties

        #region Methods

        public RelayCommand UpdateExcursions { get; set; }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Transaction = new Transaction();
            Context = new GenericRepository();
            FilteredReservations = new ObservableCollection<ReservationWrapper>();
            Excursions = new ObservableCollection<Excursion>(MainViewModel.BasicDataManager.Excursions.Where(i => i.Id > 0));
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private bool CanEditBooking()
        {
            return SelectedReservation != null;
        }

        private bool CanSearchForReservations()
        {
            return (!string.IsNullOrEmpty(Keyword) && Keyword.Length > 2) || IdInt > 0;
        }

        private bool CustomerExcursionsFilter(object item)
        {
            Excursion excursion = item as Excursion;
            return Completed || excursion.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today) || excursion.Id == 0;
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
                else
                {
                    NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedReservation.Booking.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private async Task SearchForReservations()
        {
            try
            {
                IsOk = false;
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                Context = new GenericRepository();

                DateTime dateLimit = DateTime.Today.AddMonths(-6);

                await Context.GetAllCitiesAsyncSortedByName();
                await Context.GetAllHotelsAsync<Hotel>();
                List<ReservationWrapper> list = new List<ReservationWrapper>();
                if (Transaction.ExpenseBaseCategory != ExpenseBaseCategories.PersonelExpense)
                    list = (await Context.GetAllReservationsForAddIncome(Grafeio, Keyword, dateLimit, id: IdInt)).Select(r => new ReservationWrapper(r)).ToList();
                if (Transaction.ExpenseBaseCategory != ExpenseBaseCategories.GroupExpense)
                    list.AddRange((await Context.GetAllPersonalBookingsFiltered(0, true, dateLimit, canceled: false, keyword: Keyword, id: IdInt, common: false)).Select(r => new ReservationWrapper { Id = r.Id, PersonalModel = new Personal_BookingWrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());
                if (Transaction.ExpenseBaseCategory != ExpenseBaseCategories.PersonelExpense && Transaction.ExpenseBaseCategory != ExpenseBaseCategories.GroupExpense)
                    list.AddRange((await Context.GetAllThirdPartyBookingsFiltered(0, true, dateLimit, canceled: false, keyword: Keyword, id: IdInt, common: false)).Select(r => new ReservationWrapper { Id = r.Id, ThirdPartyModel = new ThirdParty_Booking_Wrapper(r), CreatedDate = r.CreatedDate, CustomersList = r.Customers.ToList() }).ToList());

                FilteredReservations = new ObservableCollection<ReservationWrapper>(list);
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

        private void Transaction_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Transaction));
        }

        #endregion Methods
    }
}