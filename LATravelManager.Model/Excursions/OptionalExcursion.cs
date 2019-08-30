using LATravelManager.Model.People;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Excursions
{
    public class OptionalExcursion : EditTracker
    {
        #region Fields + Constructors

        private ObservableCollection<Customer> _Customers;

        private DateTime _Date;

        private Excursion _Excursion;

        private string _Name = string.Empty;

        public OptionalExcursion()
        {
        }

        public const string CustomersPropertyName = nameof(Customers);
        public const string DatePropertyName = nameof(Date);
        public const string ExcursionPropertyName = nameof(Excursion);
        public const string NamePropertyName = nameof(Name);

        #endregion Fields + Constructors

        #region Properties

        /// <summary>
        /// Sets and gets the Customers property.
        /// Changes to that property's value raise the PropertyChanged event.
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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the Date property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _Date;
            }

            set
            {
                if (_Date == value)
                {
                    return;
                }

                _Date = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the Excursion property.
        /// Changes to that property's value raise the PropertyChanged event.
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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event.
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
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}