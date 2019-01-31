using GalaSoft.MvvmLight;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class LoginViewModel: IViewModel
    {
        public bool IsLoaded { get; set; }

        public Task LoadAsync()
        {
            throw new NotImplementedException();
        }

        public Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
