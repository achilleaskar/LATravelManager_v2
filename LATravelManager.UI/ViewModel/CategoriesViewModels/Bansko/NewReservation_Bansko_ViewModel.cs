using System;
using System.Threading.Tasks;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko
{
    public class NewReservation_Bansko_ViewModel : NewReservationGroup_Base
    {
        public NewReservation_Bansko_ViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
        }

        #region Methods

 

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null, MyViewModelBase parent = null)
        {
            try
            {
                Parent = parent;

                SelectedExcursion = new ExcursionWrapper(await GenericRepository.GetByIdAsync<Excursion>(2));

                Model.BookingData.Booking booking = id > 0
                    ? await GenericRepository.GetFullBookingByIdAsync(id)
                    : await CreateNewBooking();

                if (id > 0)
                    await GenericRepository.GetAllAsync<Reciept>(r => r.BookingId == id);
                //Id = id;

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

        #endregion Methods
    }
}