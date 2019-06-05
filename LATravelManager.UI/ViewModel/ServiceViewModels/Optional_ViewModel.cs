using LATravelManager.Model.Services;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;

namespace LATravelManager.UI.ViewModel.ServiceViewModels
{
    public class Optional_ViewModel : ServiceViewModel
    {
        public Optional_ViewModel(NewReservation_Personal_ViewModel parent) : base(parent)
        {
            Service = new OptionalService();
        }
    }
}