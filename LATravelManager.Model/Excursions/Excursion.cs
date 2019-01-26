using LATravelManager.BaseTypes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class Excursion : BaseModel
    {

        #region Constructors

        public Excursion()
        {
            Tittle = "Η Εκδρομή";
            ExcursionDates = new ObservableCollection<ExcursionDate>();
        }

        #endregion Constructors

        #region Fields

        public const string DestinationsPropertyName = nameof(City);
        public const string DiscountsExistPropertyName = nameof(DiscountsExist);
        public const string DOBNeededPropertyName = nameof(DOBNeeded);

        public const string ExcursionDatesPropertyName = nameof(ExcursionDates);
        public const string ExcursionTypePropertyName = nameof(ExcursionType);
        public const string IncludesBusPropertyName = nameof(IncludesBus);
        public const string IncludesPlanePropertyName = nameof(IncludesPlane);
        public const string IncludesShipPropertyName = nameof(IncludesShip);
        public const string NamePropertyName = nameof(Name);
        public const string PassportNeededPropertyName = nameof(PassportNeeded);

        // public const string ExcursionDurationTypePropertyName = nameof(ExcursionDurationType);
        private ObservableCollection<City> _Destinations = null;

        private bool _DiscountsExist = false;
        private bool _DOBNeeded = false;
        private ObservableCollection<ExcursionDate> _ExcursionDates;

        // private ExcursionDurationTypeEnum _ExcursionDurationType;
        //        _ExcursionDurationType = value;
        //        RaisePropertyChanged(ExcursionDurationTypePropertyName);
        //    }
        //}
        private ExcursionCategory _ExcursionType;

        private bool _IncludesBus = false;
        private bool _IncludesPlane = false;
        private bool _IncludesShip = false;

        private string _Name = string.Empty;

        private bool _PassportNeeded = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Destination property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public virtual ObservableCollection<City> Destinations
        {
            get
            {
                if (_Destinations == null)
                {
                    _Destinations = new ObservableCollection<City>();
                }
                return _Destinations;
            }

            set
            {
                if (_Destinations == value)
                {
                    return;
                }

                _Destinations = value;
                RaisePropertyChanged(DestinationsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the DiscountsExist property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool DiscountsExist
        {
            get
            {
                return _DiscountsExist;
            }

            set
            {
                if (_DiscountsExist == value)
                {
                    return;
                }

                _DiscountsExist = value;
                RaisePropertyChanged(DiscountsExistPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the DOBNeeded property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool DOBNeeded
        {
            get
            {
                return _DOBNeeded;
            }

            set
            {
                if (_DOBNeeded == value)
                {
                    return;
                }

                _DOBNeeded = value;
                RaisePropertyChanged(DOBNeededPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the ExcursionDates property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public virtual ObservableCollection<ExcursionDate> ExcursionDates
        {
            get
            {
                return _ExcursionDates;
            }

            set
            {
                if (_ExcursionDates == value)
                {
                    return;
                }

                _ExcursionDates = value;
                RaisePropertyChanged(ExcursionDatesPropertyName);
            }
        }

        // set { if (_ExcursionDurationType == value) { return; }
        /// <summary>
        /// Sets and gets the ExcursionType property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public virtual ExcursionCategory ExcursionType
        {
            get
            {
                return _ExcursionType;
            }

            set
            {
                if (_ExcursionType == value)
                {
                    return;
                }

                _ExcursionType = value;
                RaisePropertyChanged(ExcursionTypePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the IncludesBus property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IncludesBus
        {
            get
            {
                return _IncludesBus;
            }

            set
            {
                if (_IncludesBus == value)
                {
                    return;
                }

                _IncludesBus = value;
                RaisePropertyChanged(IncludesBusPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the IncludePlane property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IncludesPlane
        {
            get
            {
                return _IncludesPlane;
            }

            set
            {
                if (_IncludesPlane == value)
                {
                    return;
                }

                _IncludesPlane = value;
                RaisePropertyChanged(IncludesPlanePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the IncludesShip property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IncludesShip
        {
            get
            {
                return _IncludesShip;
            }

            set
            {
                if (_IncludesShip == value)
                {
                    return;
                }

                _IncludesShip = value;
                RaisePropertyChanged(IncludesShipPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Name property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(30)]
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
                RaisePropertyChanged(NamePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the PassportNeeded property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool PassportNeeded
        {
            get
            {
                return _PassportNeeded;
            }

            set
            {
                if (_PassportNeeded == value)
                {
                    return;
                }

                _PassportNeeded = value;
                RaisePropertyChanged(PassportNeededPropertyName);
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