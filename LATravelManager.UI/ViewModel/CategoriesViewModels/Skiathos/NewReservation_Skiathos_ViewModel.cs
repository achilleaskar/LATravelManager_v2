using LATravelManager.Model.Excursions;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Skiathos
{
    public class NewReservation_Skiathos_ViewModel : NewReservationGroup_Base
    {
        #region Constructors

        public NewReservation_Skiathos_ViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
        }

        #endregion Constructors

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                if (id > 0)
                {
                    GenericRepository = new GenericRepository();
                }

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