using System.ComponentModel;

namespace LATravelManager.Model.LocalModels
{
    public class CityDepartureInfo : INotifyPropertyChanged
    {
        #region Fields

        public string GoingText
        {
            get
            {
                string s = $"{(Going > 0 ? Going.ToString() : "")}{(OnlyBusGo > 0 ? ("+" + OnlyBusGo + " MΛ") : "")}{(OnlyShipGo > 0 ? ("+" + OnlyShipGo + " MΠ") : "")}{(OneDayGo > 0 ? ("+" + OneDayGo + " 1H") : "")}{(OnlyStayGo > 0 ? ("+" + OnlyStayGo + " OS") : "")}";
                return s.Length > 0 ? s.Trim('+') : "0";
            }
        }

        public string ReturningText
        {
            get
            {
                string s = $"{(Returning > 0 ? Returning.ToString() : "")}{(OnlyBusReturn > 0 ? ("+" + OnlyBusReturn + " MΛ") : "")}{(OnlyShipReturn > 0 ? ("+" + OnlyShipReturn + " MΠ") : "")}{(OneDayReturn > 0 ? ("+" + OneDayReturn + " 1H") : "")}{(OnlyStayReturn > 0 ? ("+" + OnlyStayReturn + " OS") : "")}";
                return s.Length > 0 ? s.Trim('+') : "0";
            }
        }

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

        public int OnlyShipReturn { get; set; }
        public int OnlyShipGo { get; set; }
        public int OnlyBusGo { get; set; }
        public int OnlyBusReturn { get; set; }
        public int OnlyStayReturn { get; set; }
        public int OnlyStayGo { get; set; }
        public int OneDayReturn { get; set; }
        public int OneDayGo { get; set; }

        #endregion Properties

        #region Methods

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion Methods
    }
}