using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Lists
{
    public class Department : BaseModel
    {
        #region Fields + Constructors

        private bool _IsChecked = false;
        private int _Quantity = 1;
        private string _StartingPlace = string.Empty;
        public const string IsCheckedPropertyName = nameof(IsChecked);
        public const string QuantityPropertyName = nameof(Quantity);

        public const string StartingPlacePropertyName = nameof(StartingPlace);

        #endregion Fields + Constructors

        #region Properties

        /// <summary>
        /// Sets and gets the IsChecked property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }

            set
            {
                if (_IsChecked == value)
                {
                    return;
                }

                _IsChecked = value;
                RaisePropertyChanged(IsCheckedPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Quantity property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int Quantity
        {
            get
            {
                return _Quantity;
            }

            set
            {
                if (_Quantity == value)
                {
                    return;
                }

                _Quantity = value;
                RaisePropertyChanged(QuantityPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the StartingPlace property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        [StringLength(20)]
        public string StartingPlace
        {
            get
            {
                return _StartingPlace;
            }

            set
            {
                if (_StartingPlace == value)
                {
                    return;
                }

                _StartingPlace = value;
                RaisePropertyChanged(StartingPlacePropertyName);
            }
        }

        #endregion Properties
    }
}