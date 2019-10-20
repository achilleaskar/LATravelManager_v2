using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class ManagementTab : TabsBaseViewModel
    {
        public ManagementTab()
        {
            Level = 2;
            IconName = "InformationVariant";
            Content = "Διαχείρηση";
        }
    }
}