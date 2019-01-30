using LATravelManager.Model;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class StartingPlace : BaseModel,INamed
    {
        #region Fields + Constructors

        private string _Details = string.Empty;
        private string _ReturnTime = string.Empty;

        private string _StartTime = string.Empty;

        public const string DetailsPropertyName = nameof(Details);
        public const string ReturnTimePropertyName = nameof(ReturnTime);

        public const string StartTimePropertyName = nameof(StartTime);

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
                RaisePropertyChanged(DetailsPropertyName);
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
                RaisePropertyChanged(ReturnTimePropertyName);
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
                RaisePropertyChanged(StartTimePropertyName);
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