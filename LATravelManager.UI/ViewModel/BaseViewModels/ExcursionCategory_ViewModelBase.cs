using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class ExcursionCategory_ViewModelBase : MyViewModelBase
    {
        #region Constructors

        public ExcursionCategory_ViewModelBase(GenericRepository startingReposiroty, NavigationViewModel navigationViewModel)
        {
            Childs = new List<MyViewModelBase>();
            Tabs = new List<TabsBaseViewModel>();
            StartingReposiroty = startingReposiroty;
            NavigationViewModel = navigationViewModel;
        }

        #endregion Constructors

        #region Fields

        private List<MyViewModelBase> _Childs;
        private MyViewModelBase _SelectedChildViewModel;
        private ExcursionWrapper _SelectedExcursion;

        #endregion Fields

        #region Properties

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

        public NavigationViewModel NavigationViewModel { get; }

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

        public GenericRepository StartingReposiroty { get; set; }
        public List<TabsBaseViewModel> Tabs { get; internal set; }

        #endregion Properties

        #region Methods

        internal async Task SetProperChildViewModel(int index)
        {
            if (index < Childs.Count)
            {
                SelectedChildViewModel = Childs[index];
                if (!SelectedChildViewModel.IsLoaded)
                    await SelectedChildViewModel.LoadAsync();
            }
        }

        public virtual void ChildChanged()
        {
        }

        #endregion Methods
    }
}