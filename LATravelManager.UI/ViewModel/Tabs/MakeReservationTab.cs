using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
   public class MakeReservationTab: TabsBaseViewModel
    {
        public MakeReservationTab()
        {
            Level = 20;
            IconName = "Plus";
            Content = "Νέα Κράτηση";

        }
    }
}
