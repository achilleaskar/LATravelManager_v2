using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
   public class EconomicData_Tab : TabsBaseViewModel
    {
        public EconomicData_Tab()
        {
            IconName = "CurrencyEur";
            IsChild = true;
            Level = 2;
            Content = "Ταμεία";
        }
    }
}