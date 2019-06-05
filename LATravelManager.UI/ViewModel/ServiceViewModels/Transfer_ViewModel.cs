using LATravelManager.Model.Services;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;

namespace LATravelManager.UI.ViewModel.ServiceViewModels
{
    public class Transfer_ViewModel : ServiceViewModel
    {
        public Transfer_ViewModel(NewReservation_Personal_ViewModel parent) : base(parent)
        {
            Service = new TransferService();
        }
    }
}