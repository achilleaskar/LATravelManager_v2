using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Personal
{
    internal class Search_Personal_ViewModel : GroupChilds_BaseViewModel
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