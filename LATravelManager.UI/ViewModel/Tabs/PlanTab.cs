﻿using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class PlanTab : TabsBaseViewModel
    {
        public PlanTab()
        {
            IconName = "Grid";
            IsChild = true;
            Level = 10;
            Content = "Πλάνο";
        }
    }
}