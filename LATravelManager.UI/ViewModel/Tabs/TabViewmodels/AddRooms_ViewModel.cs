using GalaSoft.MvvmLight;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs
{
    public class AddRooms_ViewModel : MyViewModelBase
    {
        public AddRooms_ViewModel()
        {
        }

        public override async Task LoadAsync(int id)
        {
            await Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
