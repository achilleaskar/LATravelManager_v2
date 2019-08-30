using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class EconomicData_Tab : TabsBaseViewModel
    {
        public EconomicData_Tab()
        {
            IconName = "CurrencyEur";
            IsChild = true;
            Level = 2;
            Content = "Λογιστήριο";
        }
    }
}