﻿using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public interface IViewModelAsync
    {
        Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null, MyViewModelBase parent = null);

        Task ReloadAsync();

        bool IsLoaded { get; set; }
    }
}