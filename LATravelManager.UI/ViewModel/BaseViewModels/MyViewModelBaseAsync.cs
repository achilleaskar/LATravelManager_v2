using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class MyViewModelBaseAsync : ViewModelBase, IViewModelAsync
    {
        private bool _hasChanges;

        public bool IsLoaded { get; set; }

        public abstract Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        public abstract Task ReloadAsync();

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}