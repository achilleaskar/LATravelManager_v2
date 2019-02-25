using GalaSoft.MvvmLight;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class GroupChilds_BaseViewModel : MyViewModelBase
    {
        private ExcursionWrapper _SelectedExcursion;

        public GroupChilds_BaseViewModel()
        {

        }


        public ExcursionWrapper SelectedExcursion
        {
            get
            {
                return _SelectedExcursion;
            }

            set
            {
                if (_SelectedExcursion == value)
                {
                    return;
                }
                _SelectedExcursion = value;
                RaisePropertyChanged();
            }
        }
    }
}
