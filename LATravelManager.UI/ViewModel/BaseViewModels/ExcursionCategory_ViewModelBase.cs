using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public class ExcursionCategory_ViewModelBase : ViewModelBase
    {
        public ExcursionCategory_ViewModelBase()
        {
            Childs = new List<MyViewModelBase>();
            Tabs = new List<TabsBaseViewModel>();
        }

        public List<TabsBaseViewModel> Tabs { get; internal set; }

        internal async Task SetProperChildViewModel(int index)
        {
            if (index <= Childs.Count)
            {
                SelectedChildViewModel = Childs[index];
                if (!SelectedChildViewModel.IsLoaded)
                    await SelectedChildViewModel.LoadAsync(0);
            }
        }

        private MyViewModelBase _SelectedChildViewModel;

        public MyViewModelBase SelectedChildViewModel
        {
            get
            {
                return _SelectedChildViewModel;
            }

            set
            {
                if (_SelectedChildViewModel == value)
                {
                    return;
                }

                _SelectedChildViewModel = value;
                RaisePropertyChanged();
                ChildChanged();
            }
        }

        private List<MyViewModelBase> _Childs;

        public List<MyViewModelBase> Childs
        {
            get
            {
                return _Childs;
            }

            set
            {
                if (_Childs == value)
                {
                    return;
                }

                _Childs = value;
                RaisePropertyChanged();
            }
        }

        private void ChildChanged()
        {

        }
    }
}