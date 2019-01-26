using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels
{
    public class ParentViewModel : ViewModelBase
    {

        public ParentViewModel()
        {
            Childs = new List<ChildViewModel>();
        }

        public List<ChildViewModel> Childs { get; }
    }
}