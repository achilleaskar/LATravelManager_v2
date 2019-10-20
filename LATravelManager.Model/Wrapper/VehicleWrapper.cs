using LATravelManager.Model.Lists;

namespace LATravelManager.Model.Wrapper
{
    public class VehicleWrapper : ModelWrapper<Vehicle>
    {
        #region Constructors

        public VehicleWrapper() : this(new Vehicle())
        {
        }

        public VehicleWrapper(Vehicle model) : base(model)
        {
            Title = "Το λεωφορείο";
        }

        #endregion Constructors

        #region Properties

        public int DoorSeat
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Driver
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public string DriverTel
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public string Plate
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public override string ToString()
        {
            return Name;
        }

        public int SeatsFront
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int SeatsPassengers
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        #endregion Properties
    }
}