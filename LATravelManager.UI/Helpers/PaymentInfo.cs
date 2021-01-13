using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Views.Personal;
using LATravelManager.UI.Views.ThirdParty;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LATravelManager.UI.Helpers
{
    public class PaymentInfo : ViewModelBase
    {
        #region Constructors

        public PaymentInfo()
        {
        }

        public PaymentInfo(MainViewModel mainViewModel, EconomicData_ViewModel parent)
        {
            Payments = new ObservableCollection<Payment>();
            OpenBookingCommand = new RelayCommand(async () => { await OpenBooking(); }, CanOpenBooking);
            ConfirmPaymentCommand = new RelayCommand(async () => { await ConfirmPayment(); }, CanConfirmPayment);
            MainViewModel = mainViewModel;
            Parent = parent;
        }

        private bool CanConfirmPayment()
        {
            return SelectedPayment != null;
        }

        #endregion Constructors

        #region Fields

        private decimal _AlphaBank;
        private decimal _Cash;
        private decimal _Ethniki;
        private decimal _Eurobank;
        private ObservableCollection<Payment> _Payments;
        private decimal _Peiraios;
        private Payment _SelectedPayment;
        private decimal _Total;

        private User _User;

        private decimal _VISA;

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

        public RelayCommand OpenBookingCommand { get; set; }
        public RelayCommand ConfirmPaymentCommand { get; set; }

        public ObservableCollection<Payment> Payments
        {
            get
            {
                return _Payments;
            }

            set
            {
                if (_Payments == value)
                {
                    return;
                }

                _Payments = value;
                RaisePropertyChanged();
            }
        }

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

        public Payment SelectedPayment
        {
            get
            {
                return _SelectedPayment;
            }

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

        public User User
        {
            get
            {
                return _User;
            }

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
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

        public MainViewModel MainViewModel { get; }
        public EconomicData_ViewModel Parent { get; }
        public decimal VIVA_ONL { get; private set; }
        public decimal PAYPAL { get; private set; }

        #endregion Properties

        #region Methods

        public void LoadAmounts()
        {
            foreach (Payment payment in Payments)
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

                    case 6:
                        VIVA_ONL += payment.Amount;
                        break;

                    case 7:
                        PAYPAL += payment.Amount;
                        break;
                }
            }
        }

        private bool CanOpenBooking()
        {
            return SelectedPayment != null;
        }

        private async Task OpenBooking()
        {
            try
            {
                if (SelectedPayment.Personal_Booking != null)
                {
                    NewReservation_Personal_ViewModel viewModel = new NewReservation_Personal_ViewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedPayment.Personal_Booking.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new EditPersonalBooking_Window(), viewModel));
                }
                else if (SelectedPayment.ThirdParty_Booking != null)
                {
                    NewReservation_ThirdParty_VIewModel viewModel = new NewReservation_ThirdParty_VIewModel(MainViewModel);
                    await viewModel.LoadAsync(SelectedPayment.ThirdParty_Booking.Id);
                    MessengerInstance.Send(new OpenChildWindowCommand(new Edit_ThirdParty_Booking_Window(), viewModel));
                }
                else
                {
                    if (StaticResources.User.BaseLocation == 1 || (SelectedPayment.Booking.IsPartners && SelectedPayment.Booking.Partner.Id != 219))
                    {
                        NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                        await viewModel.LoadAsync(SelectedPayment.Booking.Id);
                        MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                    }
                }
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        private async Task ConfirmPayment()
        {
            try
            {
                if (SelectedPayment.Checked != true)
                    SelectedPayment.Checked = true;
                else
                    if (SelectedPayment.PaymentMethod != 0 && SelectedPayment.PaymentMethod != 5)
                    SelectedPayment.Checked = null;
                else
                    SelectedPayment.Checked = false;
                await Parent.Context.SaveAsync();
                SelectedPayment.SetPColor();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        #endregion Methods
    }
}