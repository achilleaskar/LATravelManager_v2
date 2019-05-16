using LATravelManager.Model.Excursions;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Skiathos
{
    public class NewReservation_Skiathos_ViewModel : NewReservationGroup_Base
    {

        public NewReservation_Skiathos_ViewModel(GenericRepository genericRepository) : base(genericRepository)
        {
        }


        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                SelectedExcursion = new ExcursionWrapper(await GenericRepository.GetByIdAsync<Excursion>(29));
                    Model.BookingData.Booking booking = id > 0
                        ? await GenericRepository.GetFullBookingByIdAsync(id)
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

        public override async Task ReloadAsync()
        {
            await ResetAllRefreshableDataASync();
        }

        #endregion Methods
    }
}