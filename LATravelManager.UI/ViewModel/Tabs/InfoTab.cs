using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class InfoTab : TabsBaseViewModel
    {
        public InfoTab()
        {
            Level = 5;
            IconName = "InformationVariant";
            Content = "Πληροφορίες";
        }
    }
}