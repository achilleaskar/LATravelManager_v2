﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public class ExcursionCategory_ViewModelBase : ViewModelBase
    {
        public ExcursionCategory_ViewModelBase()
        {
            Childs = new List<MyViewModelBase>();
        }
        public List<TabsBaseViewModel> Tabs { get; internal set; }


        internal void SetProperChildViewModel(string name)
        {
            var child = Childs.Find(x => x.GetType().Name == name);
            if (child != null)
                SelectedChildViewModel = child;
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
            throw new NotImplementedException();
        }

        
    }
}
