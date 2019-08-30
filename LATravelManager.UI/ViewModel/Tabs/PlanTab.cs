using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class PlanTab : TabsBaseViewModel
    {
        public PlanTab()
        {
            IconName = "Grid";
            IsChild = true;
            Level = 10;
            Content = "Πλάνο";
        }
    }
}