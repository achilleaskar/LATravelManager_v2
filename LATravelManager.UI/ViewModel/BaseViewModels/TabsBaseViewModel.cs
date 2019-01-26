using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LATravelManager.UI.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public class TabsBaseViewModel : ViewModelBase
    {
        public TabsBaseViewModel()
        {
            ChangeViewModelCommand = new RelayCommand(TabItemClicked);
        }

        private void TabItemClicked()
        {
            if (!IsSelected)
                MessengerInstance.Send(new SelectedTabChangedMessage(Name,IsChild));
        }
        public bool IsChild;
        public string Name { get; set; }
        public RelayCommand ChangeViewModelCommand { get; set; }

        private bool _IsSelected = false;

        /// <summary>
        /// Sets and gets the IsSelected property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }

            set
            {
                if (_IsSelected == value)
                {
                    return;
                }

                _IsSelected = value;
                RaisePropertyChanged();
            }
        }
        public string IconName { get; protected set; }

        private int _Level;

        /// <summary>
        /// Sets and gets the Level property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int Level
        {
            get
            {
                return _Level;
            }

            set
            {
                if (_Level == value)
                {
                    return;
                }

                _Level = value;
                RaisePropertyChanged();
            }
        }




    }
}
