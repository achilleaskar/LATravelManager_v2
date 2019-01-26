using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
   public class SettingsTab : TabsBaseViewModel
    {
        public SettingsTab()
        {
            IconName = "SettingsOutline";
            Level = 1;
            IsChild = true;
            Name = nameof(Settings_Viewmodel);
        }
    }
}
