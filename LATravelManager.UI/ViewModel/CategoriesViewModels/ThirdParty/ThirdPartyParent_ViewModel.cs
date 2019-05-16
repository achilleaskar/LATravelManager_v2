using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty
{
    public class ThirdPartyParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public ThirdPartyParent_ViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
        }

        public override async void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            await Task.Delay(0);
        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }
    }
}