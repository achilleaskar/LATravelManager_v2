using GalaSoft.MvvmLight;
using LATravelManager.UI.Repositories;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class MyViewModelBase : ViewModelBase, IViewModel
    {
       
        public bool IsLoaded { get; set; }

        public abstract Task LoadAsync(int id);
        public abstract Task ReloadAsync();

        private bool _HasChanges;

        public bool HasChanges
        {
            get
            {
                return _HasChanges;
            }

            set
            {
                if (_HasChanges == value)
                {
                    return;
                }

                _HasChanges = value;
                RaisePropertyChanged();
            }
        }

    }
}