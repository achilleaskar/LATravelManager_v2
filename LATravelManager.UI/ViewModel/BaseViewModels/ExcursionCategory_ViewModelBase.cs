using GalaSoft.MvvmLight;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public abstract class ExcursionCategory_ViewModelBase : MyViewModelBase
    {
        #region Constructors

       public BasicDataManager BasicDataManager => MainViewModel.BasicDataManager;


        public ExcursionCategory_ViewModelBase(MainViewModel mainViewModel)
        {
            Childs = new List<MyViewModelBaseAsync>();
            Tabs = new List<TabsBaseViewModel>();
            MainViewModel = mainViewModel;
        }

        #endregion Constructors

        #region Fields

        private List<MyViewModelBaseAsync> _Childs;
        private MyViewModelBaseAsync _SelectedChildViewModel;
        private ExcursionWrapper _SelectedExcursion;

        #endregion Fields

        #region Properties

        public List<MyViewModelBaseAsync> Childs
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


        public MyViewModelBaseAsync SelectedChildViewModel
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

        public List<TabsBaseViewModel> Tabs { get; internal set; }
        public MainViewModel MainViewModel { get; }

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