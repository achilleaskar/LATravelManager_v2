using LATravelManager.Model;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Wrapper;

namespace LATravelManager.UI.Wrapper
{
    public class CountryWrapper : ModelWrapper<Country>
    {
        #region Constructors

        public CountryWrapper() : this(new Country())
        {
        }

        public CountryWrapper(Country model) : base(model)
        {
            Title = "Η χώρα";
        }

        #endregion Constructors

        #region Fields

        private string _Continent;

        #endregion Fields

        #region Properties

        public string Continent
        {
            get
            {
                if (_Continent == null && Continentindex >= 0 && Continentindex <= Definitions.Continents.Count)
                {
                    _Continent = Definitions.Continents[Continentindex];
                }
                return _Continent;
            }

            set
            {
                if (_Continent == value)
                {
                    return;
                }

                _Continent = value;
                RaisePropertyChanged();
            }
        }

        public int Continentindex
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);

                if (value >= 0 && value < Definitions.Continents.Count)
                    Continent = Definitions.Continents[value];
            }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        #endregion Properties
    }
}