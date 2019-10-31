using LATravelManager.Model.Excursions;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Locations
{
    public class StartingPlace : BaseModel, INamed
    {
        public StartingPlace()
        {
        }

        #region Fields + Constructors

        private string _Details = string.Empty;
        private string _ReturnTime = string.Empty;
        private string _StartTime = string.Empty;

        #endregion Fields + Constructors

        #region Properties

        /// <summary>
        /// Sets and gets the Details property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Details
        {
            get
            {
                return _Details;
            }

            set
            {
                if (_Details == value)
                {
                    return;
                }

                _Details = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Sets and gets the ReturnTime property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        [StringLength(6)]
        public string ReturnTime
        {
            get
            {
                return _ReturnTime;
            }

            set
            {
                if (_ReturnTime == value)
                {
                    return;
                }

                _ReturnTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the StartTime property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        [StringLength(6)]
        public string StartTime
        {
            get
            {
                return _StartTime;
            }

            set
            {
                if (_StartTime == value)
                {
                    return;
                }

                _StartTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}