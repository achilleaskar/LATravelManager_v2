using GalaSoft.MvvmLight;
using LATravelManager.UI.Repositories;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class MyViewModelBase : ViewModelBase, IViewModel
    {
        private bool _hasChanges;




        private bool _isContextInUse = false;

        public bool isContextInUse
        {
            get
            {
                return _isContextInUse;
            }

            set
            {
                if (_isContextInUse == value)
                {
                    return;
                }

                _isContextInUse = value;
                RaisePropertyChanged();
            }
        }
        public bool IsLoaded { get; set; }

        public abstract Task LoadAsync(int id);
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