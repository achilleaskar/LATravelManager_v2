using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Lists
{
    public class Department : BaseModel
    {
        #region Fields + Constructors

        private bool _IsChecked = false;
        private int _Quantity = 1;
        private string _StartingPlace = string.Empty;

        #endregion Fields + Constructors

        #region Properties

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
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}