using GalaSoft.MvvmLight;
using LATravelManager.Model.Services;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.UI.ViewModel.ServiceViewModels
{
    public abstract class ServiceViewModel : ViewModelBase
    {
        public ServiceViewModel(NewReservation_Personal_ViewModel parent)
        {
            Parent = parent;
        }

        private Service _Service;


        public Service Service
        {
            get
            {
                return _Service;
            }

            set
            {
                if (_Service == value)
                {
                    return;
                }

                _Service = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
       
        public NewReservation_Personal_ViewModel Parent { get; }
    }
}