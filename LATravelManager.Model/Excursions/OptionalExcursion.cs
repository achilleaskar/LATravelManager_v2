using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Excursions
{
    public class OptionalExcursion : EditTracker
    {
        #region Fields + Constructors

        private DateTime _Date = DateTime.Today;

        private Excursion _Excursion;

        private string _Name = string.Empty;

        private int _Cost;

        [NotMapped]
        public int FirstPrice { get; set; }

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

        public OptionalExcursion()
        {
        }

        #endregion Fields + Constructors

        #region Properties

        private string _Note;

        [NotMapped]
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