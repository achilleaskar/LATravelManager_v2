using LATravelManager.Model.Lists;
using LATravelManager.Model.People;

namespace LATravelManager.Model.Excursions
{
    public class CustomerOptional : BaseModel
    {
        #region Fields

        private int _Cost;
        private Customer _Customer;

        private Leader _Leader;

        private string _Note;

        private OptionalExcursion _OptionalExcursion;

        #endregion Fields

        #region Properties

        public int Cost
        {
            get
            {
                return _Cost;
            }

            set
            {
                if (_Cost == value)
                {
                    return;
                }

                _Cost = value;
                RaisePropertyChanged();
            }
        }

        public Customer Customer
        {
            get
            {
                return _Customer;
            }

            set
            {
                if (_Customer == value)
                {
                    return;
                }

                _Customer = value;
                RaisePropertyChanged();
            }
        }

        public Leader Leader
        {
            get
            {
                return _Leader;
            }

            set
            {
                if (_Leader == value)
                {
                    return;
                }

                _Leader = value;
                RaisePropertyChanged();
            }
        }

        public string Note
        {
            get
            {
                return _Note;
            }

            set
            {
                if (_Note == value)
                {
                    return;
                }

                _Note = value;
                RaisePropertyChanged();
            }
        }

        public OptionalExcursion OptionalExcursion
        {
            get
            {
                return _OptionalExcursion;
            }

            set
            {
                if (_OptionalExcursion == value)
                {
                    return;
                }

                _OptionalExcursion = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}