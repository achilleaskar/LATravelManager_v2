using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
