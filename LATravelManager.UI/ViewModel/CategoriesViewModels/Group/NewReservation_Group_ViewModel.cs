using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Group
{
    public class NewReservation_Group_ViewModel : NewReservationGroup_Base
    {
        private bool _enabled;

        public NewReservation_Group_ViewModel(GenericRepository genericRepository) : base(genericRepository)
        {
            MessengerInstance.Register<SelectedExcursionChangedMessage>(this, async exc => { await SelectedExcursionChanged(exc.SelectedExcursion); });

        }

        #region Constructors



        #endregion Constructors

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

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                if (SelectedExcursion != null || id > 0)
                {
                    Model.BookingData.Booking booking = id > 0
                        ? await GenericRepository.GetFullBookingByIdAsync(id)
                        : await CreateNewBooking();

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
                InitializeBooking(await CreateNewBooking());

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