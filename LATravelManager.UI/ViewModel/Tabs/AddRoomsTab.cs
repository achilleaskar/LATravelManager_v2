﻿using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class AddRoomsTab : TabsBaseViewModel
    {
        public AddRoomsTab()
        {
            IconName = "Home";
            IsChild = true;
            Level = 1;
            Content = "Πρ. Δωματίων";
        }
    }
}