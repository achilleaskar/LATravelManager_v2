using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class SettingsTab : TabsBaseViewModel
    {
        public SettingsTab()
        {
            IconName = "SettingsOutline";
            Level = 1;
            IsChild = true;
            Content = "Ρυθμίσεις";
        }
    }
}