using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class InfoTab : TabsBaseViewModel
    {
        public InfoTab()
        {
            Level = 5;
            IconName = "InformationVariant";
            Content = "Πληροφορίες";

        }
    }
}
