using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class MoveTab : TabsBaseViewModel
    {
        public MoveTab()
        {
            Level = 1;
            IconName = "SwapHorizontal";
            Content = "Μετακινήσεις";
        }
    }
}