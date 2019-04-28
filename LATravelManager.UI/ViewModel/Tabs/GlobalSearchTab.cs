using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class GlobalSearchTab : TabsBaseViewModel
    {
        public GlobalSearchTab()
        {
            IconName = "Magnify";
            IsChild = true;
            Level = 10;
            Content = "Γενική Αναζήτηση";
        }
    }
}