using LATravelManager.Model.Services;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;

namespace LATravelManager.UI.ViewModel.ServiceViewModels
{
    public class Ferry_ViewModel : ServiceViewModel
    {
        public Ferry_ViewModel(NewReservation_Personal_ViewModel parent) : base(parent)
        {
            Service = new FerryService();
        }
    }
}