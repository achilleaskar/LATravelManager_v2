using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty
{
    public class ThirdPartyParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public ThirdPartyParent_ViewModel(GenericRepository startingReposiroty) : base(startingReposiroty)
        {
        }

        public override Task LoadAsync(int id = 0)
        {
            return Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            return Task.Delay(0);
        }
    }
}