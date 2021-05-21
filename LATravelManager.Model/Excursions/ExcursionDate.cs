using System;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Excursions
{
    public class ExcursionDate : BaseModel
    {
        #region Constructors

        public ExcursionDate()
        {
            CheckIn = DateTime.Today;
        }

        #endregion Constructors

        #region Fields

        public const string CheckInPropertyName = nameof(CheckIn);

        public const string CheckOutPropertyName = nameof(CheckOut);
        public const string NamePropertyName = nameof(Name);
        private DateTime _CheckIn;

        private DateTime _CheckOut;

        private string _Name = string.Empty;

        #endregion Fields




        private Excursion _Excursion;


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

        private bool _Night = true;

        public bool NightStart
        {
            get
            {
                return _Night;
            }

            set
            {
                if (_Night == value)
                {
                    return;
                }

                _Night = value;
                RaisePropertyChanged();
            }
        }

        #region Properties

        /// <summary>
        /// Sets and gets the CheckIn property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }

                _CheckIn = value;
                RaisePropertyChanged();
                if (CheckIn > CheckOut)
                {
                    CheckOut = CheckIn.AddDays(3);
                }
            }
        }

        /// <summary>
        /// Sets and gets the CheckOut property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime CheckOut
        {
            get
            {
                return _CheckOut;
            }

            set
            {
                if (_CheckOut == value)
                {
                    return;
                }

                _CheckOut = value;
                RaisePropertyChanged();
                if (CheckIn > CheckOut)
                {
                    CheckOut = CheckIn.AddDays(3);
                }
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
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name + CheckIn.ToString(" dd-") + CheckOut.ToString("dd/MM");
        }

        #endregion Methods
    }
}