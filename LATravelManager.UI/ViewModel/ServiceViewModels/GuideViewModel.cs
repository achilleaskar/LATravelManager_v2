using LATravelManager.Model.Services;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;

namespace LATravelManager.UI.ViewModel.ServiceViewModels
{
    public class GuideViewModel : ServiceViewModel
    {
        public GuideViewModel(NewReservation_Personal_ViewModel parent) : base(parent)
        {
            Service = new GuideService();
        }

        public override void Refresh()
        {
        }
    }
}