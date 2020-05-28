using System.Collections.Generic;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Wrapper;

namespace LATravelManager.UI.Wrapper
{
    public class HotelWrapper : ModelWrapper<Hotel>
    {
        #region Constructors

        public HotelWrapper(Hotel model) : base(model)
        {
            Title = "Το ξενοδοχείο";
            RoomWrappers = new List<RoomWrapper>();
        }

        public HotelWrapper() : this(new Hotel())
        {
        }

        #endregion Constructors

        #region Properties

        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public City City
        {
            get { return GetValue<City>(); }
            set { SetValue(value); }
        }

        public bool NoName
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public HotelCategory HotelCategory
        {
            get { return GetValue<HotelCategory>(); }
            set { SetValue(value); }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public List<Room> Rooms
        {
            get { return GetValue<List<Room>>(); }
            set { SetValue(value); }
        }

        private List<RoomWrapper> _RoomWrappers;

        public List<RoomWrapper> RoomWrappers
        {
            get
            {
                return _RoomWrappers;
            }

            set
            {
                if (_RoomWrappers == value)
                {
                    return;
                }

                _RoomWrappers = value;
                RaisePropertyChanged();
            }
        }

        public string Tel
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        #region Methods

        public override string ToString() => Name;

        #endregion Methods
    }
}