using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.UI.Message;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public class TabsBaseViewModel : ViewModelBase
    {
        #region Constructors

        public TabsBaseViewModel()
        {
           
            MessengerInstance.Register<DeselectAllOtherTabsMessage>(this, (msg) =>
            {
                if (IsSelected && GetType().Name != msg.NewTab)
                {
                    IsSelected = false;
                }
            });
        }

        #endregion Constructors

        #region Fields

        public bool IsChild;
        private string _Content = string.Empty;

        private bool _IsSelected = false;

        private int _Level;

        #endregion Fields

        #region Properties


        public string Content
        {
            get
            {
                return _Content;
            }

            set
            {
                if (_Content == value)
                {
                    return;
                }

                _Content = value;
                RaisePropertyChanged();
            }
        }

        public string IconName { get; protected set; }

        public int Index { get; set; }

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
             //   TabItemClicked();
                RaisePropertyChanged();
            }
        }

       

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


        #endregion Properties
    }
}