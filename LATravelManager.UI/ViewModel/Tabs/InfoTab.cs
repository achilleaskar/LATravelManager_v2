using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class InfoTab : TabsBaseViewModel
    {
        public InfoTab()
        {
            Level = 2;
            IconName = "InformationVariant";
            Content = "Πληροφορίες";
        }
    }
}