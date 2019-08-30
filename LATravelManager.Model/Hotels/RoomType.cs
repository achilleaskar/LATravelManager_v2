using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Hotels
{
    public class RoomType : BaseModel
    {
        #region Constructors

        public RoomType()
        {
        }

        #endregion Constructors

        #region Fields

        public const string MaxCapacityPropertyName = nameof(MaxCapacity);
        public const string MinCapacityPropertyName = nameof(MinCapacity);
        public const string NamePropertyName = nameof(Name);

        private int _MaxCapacity = -1;
        private int _MinCapacity = -1;
        private string _Name = string.Empty;

        #endregion Fields

        [NotMapped]
        public int freeRooms { get; set; }

        private int _Index;

        public int Index
        {
            get
            {
                return _Index;
            }

            set
            {
                if (_Index == value)
                {
                    return;
                }

                _Index = value;
                RaisePropertyChanged();
            }
        }

        #region Properties

        /// <summary>
        /// Sets and gets the MaxCapacity property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int MaxCapacity
        {
            get
            {
                return _MaxCapacity;
            }

            set
            {
                if (_MaxCapacity == value)
                {
                    return;
                }

                _MaxCapacity = value;
                RaisePropertyChanged(MaxCapacityPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the MinCapacity property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int MinCapacity
        {
            get
            {
                return _MinCapacity;
            }

            set
            {
                if (_MinCapacity == value)
                {
                    return;
                }

                _MinCapacity = value;
                RaisePropertyChanged(MinCapacityPropertyName);
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

        #endregion Properties

        public string Capacity => GetPeopleLimit();

        #region Methods

        public string GetPeopleLimit()
        {
            if (MinCapacity == MaxCapacity)
                return MinCapacity.ToString();
            return MinCapacity + " - " + MaxCapacity;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}