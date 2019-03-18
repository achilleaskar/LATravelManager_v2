using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Personal
{
    class Search_Personal_ViewModel : GroupChilds_BaseViewModel
    {
        public override Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            return Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
