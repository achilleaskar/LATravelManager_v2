using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Repositories;
using NuGet;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel
{
    internal class CustomerSearch_ViewModel : ViewModelBase
    {
        private readonly GenericRepository repository;
        private readonly BookingWrapper booking;
        private readonly Personal_BookingWrapper personal;
        private readonly ThirdParty_Booking_Wrapper thirdParty;

        public CustomerSearch_ViewModel(GenericRepository repository, BookingWrapper booking = null,
            Personal_BookingWrapper personal = null, ThirdParty_Booking_Wrapper thirdParty = null)
        {
            this.repository = repository;
            this.booking = booking;
            this.personal = personal;
            this.thirdParty = thirdParty;
            Customers = new ObservableCollection<Customer>();
            SearchByPhoneCommand = new RelayCommand(async () => { await SearchByTel(); }, canExecute:()=> CanSearch(Tel));
            SearchBySurenameCommand = new RelayCommand(async () => { await SearchBySurename(); },canExecute:()=> CanSearch(Surename));
            ShowAllCustomersCommand = new RelayCommand(async () => { await ShowAllCustomers(); });
        }

        private bool CanSearch(object text)
        {
            return text is string b && b.Length >= 3;
        }

        private async Task ShowAllCustomers()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            AllCustomers = new ObservableCollection<Customer>();
            var c = await repository.GetFullCustomerByID(SelectedCustomer.Id);

            if (c.Reservation != null)
            {
                if (c.Reservation.Booking == null || c.Reservation.Booking.ReservationsInBooking == null)
                {
                    AllCustomers.Add(SelectedCustomer);
                }
                else
                    foreach (var re in c.Reservation.Booking.ReservationsInBooking)
                    {
                        AllCustomers.AddRange(re.CustomersList);
                    }
            }
            else if (c.Personal_Booking != null)
            {
                AllCustomers.AddRange(c.Personal_Booking.Customers);
            }
            else if (c.ThirdParty_Booking != null)
            {
                AllCustomers.AddRange(c.ThirdParty_Booking.Customers);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task SearchBySurename()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Customers = new ObservableCollection<Customer>(await repository.FindCustomerBySurename(Surename).ConfigureAwait(true));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        internal void AddCustomerToBooking(Customer c)
        {
            var cw = new CustomerWrapper(new Customer
            {
                Age = c.Age,
                DOB = c.DOB,
                Email = c.Email,
                Name = c.Name,
                PassportExpiration = c.PassportExpiration,
                PassportPublish = c.PassportPublish,
                StartingPlace = c.StartingPlace,
                ReturningPlace = c.ReturningPlace,
                Surename = c.Surename,
                PassportNum = c.PassportNum,
                Tel = c.Tel
            });
            if (booking != null)
            {
                booking.Customers.Add(cw);
            }
            else if (personal != null)
            {
                personal.CustomerWrappers.Add(cw);
            }
            else if (thirdParty != null)
            {
                thirdParty.CustomerWrappers.Add(cw);
            }
        }




        private string _Tel;


        public string Tel
        {
            get
            {
                return _Tel;
            }

            set
            {
                if (_Tel == value)
                {
                    return;
                }

                _Tel = value;
                RaisePropertyChanged();
            }
        }




        private string _Surename;


        public string Surename
        {
            get
            {
                return _Surename;
            }

            set
            {
                if (_Surename == value)
                {
                    return;
                }

                _Surename = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand SearchByPhoneCommand { get; set; }
        public RelayCommand SearchBySurenameCommand { get; set; }
        public RelayCommand ShowAllCustomersCommand { get; set; }

        private ObservableCollection<Customer> _Customers;

        private ObservableCollection<Customer> _AllCustomers;

        public ObservableCollection<Customer> AllCustomers
        {
            get
            {
                return _AllCustomers;
            }

            set
            {
                if (_AllCustomers == value)
                {
                    return;
                }

                _AllCustomers = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Customer> Customers
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

        private Customer _SelectedCustomer;

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

        public async Task SearchByTel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Customers = new ObservableCollection<Customer>(await repository.FindCustomerByTel(Tel).ConfigureAwait(true));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        //public void SearchByName(string name, string surename)
        //{
        //    Customers = new ObservableCollection<Customer>(repository.FindCustomerByName(name, surename));
        //}
    }
}