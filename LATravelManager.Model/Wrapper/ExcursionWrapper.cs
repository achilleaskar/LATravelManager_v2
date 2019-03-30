using LATravelManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LATravelManager.UI.Wrapper
{
    public class ExcursionWrapper : ModelWrapper<Excursion>
    {
        #region Constructors

        public ExcursionWrapper(Excursion model) : base(model)
        {
            Title = "Η εκδρομή";
        }

        public ExcursionWrapper() : this(new Excursion())
        {
        }

        public DateTime Start
        {
            get
            {
                var tmpMinDate = ExcursionDates[0].CheckIn;
                foreach (var date in ExcursionDates)
                {
                    if (date.CheckIn < tmpMinDate)
                    {
                        tmpMinDate = date.CheckIn;
                    }
                }
                return tmpMinDate;
            }
        }

        public DateTime End
        {
            get
            {
                var tmpMaxDate = ExcursionDates[0].CheckOut;
                foreach (var date in ExcursionDates)
                {
                    if (date.CheckOut > tmpMaxDate)
                    {
                        tmpMaxDate = date.CheckOut;
                    }
                }
                return tmpMaxDate;
            }
        }

        #endregion Constructors

        #region Properties

        public ObservableCollection<City> Destinations
        {
            get { return GetValue<ObservableCollection<City>>(); }
            set { SetValue(value); }
        }

        public bool DiscountsExist
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool NightStart
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool DOBNeeded
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool AgeNeeded => !DOBNeeded;

        public ObservableCollection<ExcursionDate> ExcursionDates
        {
            get { return GetValue<ObservableCollection<ExcursionDate>>(); }
            set { SetValue(value); }
        }

        public ExcursionCategory ExcursionType
        {
            get { return GetValue<ExcursionCategory>(); }
            set { SetValue(value); }
        }

        public bool IncludesBus
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IncludesPlane
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IncludesShip
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool PassportNeeded
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Destinations):
                    if (Destinations.Count == 0)
                    {
                        yield return "Παρακαλώ επιλέξτε προορισμό";
                    }
                    break;

                case nameof(ExcursionDates):
                    if (ExcursionDates.Count == 0)
                    {
                        yield return "Παρακαλώ επιλέξτε Ημερομηνίες";
                    }
                    break;
            }
        }
    }
}