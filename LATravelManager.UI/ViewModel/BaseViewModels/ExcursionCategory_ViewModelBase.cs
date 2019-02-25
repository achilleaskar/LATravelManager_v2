using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class ExcursionCategory_ViewModelBase : MyViewModelBase
    {
        public ExcursionCategory_ViewModelBase(GenericRepository startingReposiroty)
        {
            Childs = new List<MyViewModelBase>();
            Tabs = new List<TabsBaseViewModel>();
            StartingReposiroty = startingReposiroty;
        }

        private ExcursionWrapper _SelectedExcursion;

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
                MessengerInstance.Send(new SelectedExcursionChangedMessage(value));
                RaisePropertyChanged();
                RaisePropertyChanged("Enable");
            }
        }




       

        public List<TabsBaseViewModel> Tabs { get; internal set; }

        internal async Task SetProperChildViewModel(int index)
        {
            if (index <= Childs.Count)
            {
                SelectedChildViewModel = Childs[index];
                if (!SelectedChildViewModel.IsLoaded)
                    await SelectedChildViewModel.LoadAsync();
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

        public GenericRepository StartingReposiroty { get; set; }

        private void ChildChanged()
        {
        }
    }
}