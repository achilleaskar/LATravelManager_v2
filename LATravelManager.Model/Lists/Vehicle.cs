using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Lists
{
    public class Vehicle : EditTracker
    {
        #region Fields

        private int _DoorSeat;
        private string _Driver;

        private string _DriverTel;

        private string _Name;

        private string _Plate;

        private int _SeatsFront;

        private int _SeatsPassengers;

        #endregion Fields

        #region Properties

        [Required]
        [Range(10, 100, ErrorMessage = "Επιλέξτε την θέση πίσω από την πόρτα")]
        public int DoorSeat
        {
            get
            {
                return _DoorSeat;
            }

            set
            {
                if (_DoorSeat == value)
                {
                    return;
                }

                _DoorSeat = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [MaxLength(25), MinLength(3)]
        public string Driver
        {
            get
            {
                return _Driver;
            }

            set
            {
                if (_Driver == value)
                {
                    return;
                }

                _Driver = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [Phone]
        [MaxLength(15), MinLength(10)]
        public string DriverTel
        {
            get
            {
                return _DriverTel;
            }

            set
            {
                if (_DriverTel == value)
                {
                    return;
                }

                _DriverTel = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [MaxLength(25)]
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

        [Required]
        [MaxLength(10)]
        public string Plate
        {
            get
            {
                return _Plate;
            }

            set
            {
                if (_Plate == value)
                {
                    return;
                }

                _Plate = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [Range(1, 3)]
        public int SeatsFront
        {
            get
            {
                return _SeatsFront;
            }

            set
            {
                if (_SeatsFront == value)
                {
                    return;
                }

                _SeatsFront = value;
                RaisePropertyChanged();
            }
        }

        [Required, Range(10, 100)]
        public int SeatsPassengers
        {
            get
            {
                return _SeatsPassengers;
            }

            set
            {
                if (_SeatsPassengers == value)
                {
                    return;
                }

                _SeatsPassengers = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}