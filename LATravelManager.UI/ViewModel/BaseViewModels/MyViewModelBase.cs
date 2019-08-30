using GalaSoft.MvvmLight;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class MyViewModelBase : ViewModelBase, IViewModel
    {
        public bool IsLoaded { get; set; }

        public abstract void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        public abstract void Reload();
    }
}