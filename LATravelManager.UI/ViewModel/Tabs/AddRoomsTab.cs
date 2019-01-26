using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class AddRoomsTab : TabsBaseViewModel
    {
        public AddRoomsTab()
        {
            IconName = "Home";
            IsChild = true;
            Level = 1;
            Name = nameof(AddRooms_ViewModel);
        }
      
    }
}
