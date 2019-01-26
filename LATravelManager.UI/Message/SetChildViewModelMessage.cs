using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.Message
{
    public class SetChildViewModelMessage
    {
        public SetChildViewModelMessage(MyViewModelBase viewmodel)
        {
            Viewmodel = viewmodel;
        }

        public MyViewModelBase Viewmodel { get; }
    }
}
