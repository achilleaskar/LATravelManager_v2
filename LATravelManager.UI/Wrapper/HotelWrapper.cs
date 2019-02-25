using LATravelManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LATravelManager.UI.Wrapper
{
    public class HotelWrapper : ModelWrapper<Hotel>
    {
        #region Constructors

        public HotelWrapper(Hotel model) : base(model)
        {
            Title = "Το ξενοδοχείο";
        }

        public HotelWrapper():base(new Hotel())
        {
            Title = "Το ξενοδοχείο";
        }

        #endregion Constructors

        #region Properties

        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public City City
        {
            get { return GetValue<City>(); }
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