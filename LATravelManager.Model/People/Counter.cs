using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.Model.People
{
    public class Counter : ViewModelBase
    {
        #region Fields

        private string _Name;
        private bool _Selected;
        private int _Total;
        private int _UnHandled;

        #endregion Fields

        #region Properties

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged();
            }
        }

        public bool Selected
        {
            get
            {
                return _Selected;
            }

            set
            {
                if (_Selected == value)
                {
                    return;
                }

                _Selected = value;
                RaisePropertyChanged();
            }
        }

        public int Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged();
            }
        }





        private int _Both;


        public int Both
        {
            get
            {
                return _Both;
            }

            set
            {
                if (_Both == value)
                {
                    return;
                }

                _Both = value;
                RaisePropertyChanged();
            }
        }

        public int UnHandled
        {
            get
            {
                return _UnHandled;
            }

            set
            {
                if (_UnHandled == value)
                {
                    return;
                }

                _UnHandled = value;
                RaisePropertyChanged();
            }
        }

        public string Values => UnHandled + " / " + Total;

        #endregion Properties
    }
}
