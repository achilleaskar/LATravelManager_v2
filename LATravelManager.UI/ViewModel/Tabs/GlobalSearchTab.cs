using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
   public class GlobalSearchTab : TabsBaseViewModel
    {
        public GlobalSearchTab()
        {
            IconName = "Magnify";
            IsChild = true;
            Level = 10;
            Content = "Γενική Αναζήτηση";
        }
    }
}