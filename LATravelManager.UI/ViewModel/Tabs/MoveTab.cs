using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
