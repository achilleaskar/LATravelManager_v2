using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Lists
{
    public class Bus : EditTracker
    {
        #region Fields

        public const string CustomersPropertyName = nameof(Customers);
        public const string ExcursionPropertyName = nameof(Excursion);

        public const string NamePropertyName = nameof(Name);
        public const string StartingPlacePropertyName = nameof(StartingPlace);

        public const string TelPropertyName = nameof(Tel);
        private ObservableCollection<Customer> _Customers = new ObservableCollection<Customer>();
        private Excursion _Excursion;
        private string _Name = string.Empty;
        private string _StartingPlace = string.Empty;

        private string _Tel = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Customers property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                RaisePropertyChanged(CustomersPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Excursion property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Excursion Excursion
        {
            get
            {
                return _Excursion;
            }

            set
            {
                if (_Excursion == value)
                {
                    return;
                }

                _Excursion = value;
                RaisePropertyChanged(ExcursionPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Name property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(20)]
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
        /// Sets and gets the StartingPlace property. Changes to that property's value raise the
        /// PropertyChanged event.
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

        /// <summary>
        /// Sets and gets the Tel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(20)]
        public string Tel
        {
            get
            {
                return _Tel;
            }

            set
            {
                if (_Tel == value)
                {
                    return;
                }

                _Tel = value;
                RaisePropertyChanged(TelPropertyName);
            }
        }

        #endregion Properties
    }
}