﻿using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Skiathos
{
    public class SkiathosParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public SkiathosParent_ViewModel(GenericRepository startingReposiroty, NavigationViewModel navigationViewModel) : base(startingReposiroty, navigationViewModel)
        {
        }

        public override Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            return Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            return Task.Delay(0);
        }
    }
}