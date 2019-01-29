using GalaSoft.MvvmLight;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class MyViewModelBase : ViewModelBase, IViewModel
    {
        public bool IsLoaded { get; set; }

        public abstract Task LoadAsync();
        public abstract Task ReloadAsync();

    }
}