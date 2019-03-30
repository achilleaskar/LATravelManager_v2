using LATravelManager.Models;

namespace LATravelManager.UI.Wrapper
{
    public class CityWrapper : ModelWrapper<City>
    {

        public CityWrapper() : this(new City())
        {
        }

        public CityWrapper(City model) : base(model)
        {
            Title = "Η πόλη";
        }

        public Country Country
        {
            get { return GetValue<Country>(); }
            set { SetValue(value); }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public override string ToString()
        {
            return Name;
        }

    }
}