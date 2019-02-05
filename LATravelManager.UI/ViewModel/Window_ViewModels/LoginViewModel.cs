using GalaSoft.MvvmLight;
using LATravelManager.UI.Repositories;
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
        private GenericRepository startingRepository;

        public LoginViewModel(GenericRepository startingRepository)
        {
            this.startingRepository = startingRepository;
        }

        public bool IsLoaded { get; set; }

        public Task LoadAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        public static implicit operator ViewModelBase(LoginViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
