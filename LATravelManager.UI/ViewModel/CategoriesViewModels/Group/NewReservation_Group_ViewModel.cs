using LATravelManager.Model.BookingData;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Group
{
    public class NewReservation_Group_ViewModel : NewReservationGroup_Base
    {
        #region Constructors

        public NewReservation_Group_ViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            MessengerInstance.Register<SelectedExcursionChangedMessage>(this, async exc => { await SelectedExcursionChanged(exc.SelectedExcursion); });
        }

        #endregion Constructors

        #region Fields

        private bool _enabled;

        #endregion Fields

        #region Properties

        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (_enabled == value)
                {
                    return;
                }

                _enabled = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

 

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null, MyViewModelBase parent = null)
        {
            try
            {
                Parent = parent;

                if (id > 0)
                    GenericRepository = new GenericRepository();
                if (SelectedExcursion != null || id > 0)
                {
                    Booking booking = id > 0
                         ? await GenericRepository.GetFullBookingByIdAsync(id)
                         : await CreateNewBooking();
                    if (id > 0)
                        await GenericRepository.GetAllAsync<Reciept>(r => r.BookingId == id);
                    InitializeBooking(booking);

                    await ResetAllRefreshableDataASync();
                }
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
            await ResetAllRefreshableDataASync();
        }

        public async Task SelectedExcursionChanged(ExcursionWrapper selectedExcursion)
        {
            if (selectedExcursion != null)
            {
                Enabled = true;
                SelectedExcursion = selectedExcursion;
                ObservableCollection<CustomerWrapper> oldCusts = null;
                if (BookingWr != null && BookingWr.Id == 0)
                {
                    oldCusts = BookingWr.Customers;
                }
                InitializeBooking(await CreateNewBooking(), oldCusts);

                await ResetAllRefreshableDataASync();
            }
            else
            {
                Enabled = false;
            }
        }

        #endregion Methods
    }
}