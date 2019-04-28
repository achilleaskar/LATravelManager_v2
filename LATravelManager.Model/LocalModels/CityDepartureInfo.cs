using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.Model.LocalModels
{

    public class CityDepartureInfo : INotifyPropertyChanged
    {
        #region Fields

        private string _City;
        private int _Going;
        private bool _IsChecked;

        private int _Returning;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public string City
        {
            get => _City;
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        public int Going
        {
            get => _Going;
            set
            {
                if (_Going != value)
                {
                    _Going = value;
                    OnPropertyChanged(nameof(Going));
                }
            }
        }

        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public int Returning
        {
            get => _Returning;
            set
            {
                if (_Returning != value)
                {
                    _Returning = value;
                    OnPropertyChanged(nameof(Returning));
                }
            }
        }

        #endregion Properties

        #region Methods

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion Methods
    }
}