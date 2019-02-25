using LATravelManager.Models;

namespace LATravelManager.UI.Wrapper
{
    public class CityWrapper : ModelWrapper<City>
    {
        #region Constructors

        public CityWrapper() : base(new City())
        {
            Title = "Η πόλη";
        }

        public CityWrapper(City model) : base(model)
        {
            Title = "Η πόλη";
        }

        #endregion Constructors

        #region Properties

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

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}