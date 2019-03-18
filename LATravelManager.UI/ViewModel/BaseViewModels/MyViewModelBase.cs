using GalaSoft.MvvmLight;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class MyViewModelBase : ViewModelBase, IViewModel
    {
        private bool _hasChanges;


        public bool IsLoaded { get; set; }

        public abstract Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null);

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