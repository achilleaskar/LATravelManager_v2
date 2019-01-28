using LATravelManager.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Models
{
    public class Hotel : BaseModel
    {

        #region Constructors

        public Hotel()
        {
            Tittle = "Το ξενοδοχείο";
            Rooms = new List<Room>();
        }

        #endregion Constructors

        #region Fields

        public const string AvailableRoomsPropertyName = nameof(AvailableRooms);
        public const string CityPropertyName = nameof(City);
        public const string HotelCategoryPropertyName = nameof(HotelCategory);
        public const string NamePropertyName = nameof(Name);
        public const string RoomsPropertyName = nameof(Rooms);
        public const string TelPropertyName = nameof(Tel);
        private ObservableCollection<Room> _AvailableRooms = new ObservableCollection<Room>();
        private City _City;
        private HotelCategory _HotelCategory;
        private string _Name = string.Empty;
        private List<Room> _Rooms;
        private string _Tel = string.Empty;

        #endregion Fields

        #region Properties


        public const string AddressPropertyName = nameof(Address);

        private string _Address = string.Empty;

        /// <summary>
        /// Sets and gets the Address property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                if (_Address == value)
                {
                    return;
                }

                _Address = value;
                RaisePropertyChanged(AddressPropertyName);
            }
        }

        [NotMapped]
        public int AllotmentRooms { get; set; }

        /// <summary>
        /// Sets and gets the AvailableRooms property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public ObservableCollection<Room> AvailableRooms
        {
            get
            {
                return _AvailableRooms;
            }

            set
            {
                if (_AvailableRooms == value)
                {
                    return;
                }

                _AvailableRooms = value;
                RaisePropertyChanged(AvailableRoomsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the City property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Η πόλη στην οποία βρίσκεται το ξενοδοχείο απαιτείται!")]
        public virtual City City
        {
            get
            {
                return _City;
            }

            set
            {
                if (_City == value)
                {
                    return;
                }

                _City = value;
                RaisePropertyChanged(CityPropertyName);
            }
        }

        [NotMapped]
        public int FreeRooms { get; set; }

        /// <summary>
        /// Sets and gets the Category property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Η κατηγορία του ξενοδοχείου απαιτείται!")]
        public virtual HotelCategory HotelCategory
        {
            get
            {
                return _HotelCategory;
            }

            set
            {
                if (_HotelCategory == value)
                {
                    return;
                }

                _HotelCategory = value;
                RaisePropertyChanged(HotelCategoryPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Name property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Το Όνομα ξενοδοχείου απαιτείται!")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Το Όνομα ξενοδοχείου μπορεί να είναι απο 3 έως 25 χαρακτήρες.")]
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

                _Name = value.ToUpper();
                RaisePropertyChanged(NamePropertyName);
            }
        }

        [NotMapped]
        public int ReservedRooms { get; set; }

        public virtual List<Room> Rooms
        {
            get
            {
                return _Rooms;
            }

            set
            {
                if (_Rooms == value)
                {
                    return;
                }

                _Rooms = value;
                RaisePropertyChanged(RoomsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Tel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Phone]
        [Required(ErrorMessage = "Το Τηλέφωνο απαιτείται!")]
        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες.")]
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

        #region Methods

        public override string ToString() => Name;

        #endregion Methods

    }
}