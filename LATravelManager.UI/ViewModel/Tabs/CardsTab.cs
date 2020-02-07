using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class CardsTab : TabsBaseViewModel
    {
        public CardsTab()
        {
            Level = 2;
            IconName = "FormatListBulleted";
            Content = "Καρτέλες";
        }
    }
}