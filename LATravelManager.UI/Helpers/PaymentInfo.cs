using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.Views.Bansko;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LATravelManager.UI.Helpers
{
    public class PaymentInfo : ViewModelBase
    {
        #region Constructors

        private float _Total;

        public float Total
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

        private float _Cash;

        public float Cash
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

        private float _Peiraios;


        public float Peiraios
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




        private float _Ethniki;


        public float Ethniki
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




        private float _Eurobank;


        public float Eurobank
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



        private float _AlphaBank;


        public float AlphaBank
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

        private float _VISA;

        public float VISA
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

        public PaymentInfo()
        {
            Payments = new ObservableCollection<Payment>();
            OpenBookingCommand = new RelayCommand(async () => { await OpenBooking(); }, CanOpenBooking);
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Payment> _Payments;
        private Payment _SelectedPayment;
        private User _User;

        #endregion Fields

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
                }

            }
        }

        #region Properties

        public RelayCommand OpenBookingCommand { get; set; }

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

        #endregion Properties

        #region Methods

        private bool CanOpenBooking()
        {
            return SelectedPayment != null;
        }

        private async Task OpenBooking()
        {
            try
            {
                NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel();
                await viewModel.LoadAsync(SelectedPayment.Booking.Id);
                MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        #endregion Methods
    }
}