using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Skiathos
{
    public class SkiathosParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public SkiathosParent_ViewModel(GenericRepository startingReposiroty) : base(startingReposiroty)
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
