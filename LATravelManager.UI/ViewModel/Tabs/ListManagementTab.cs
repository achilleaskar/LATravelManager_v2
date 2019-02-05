using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class ListManagementTab : TabsBaseViewModel
    {
        public ListManagementTab()
        {
            Level = 1;
            IconName = "FormatListNumbers";
            Content = "Λίστες";

        }
    }
}
