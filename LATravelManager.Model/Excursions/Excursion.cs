using LATravelManager.Model.Locations;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LATravelManager.Model.Excursions
{
    public class Excursion : EditTracker
    {
        #region Constructors

        public Excursion()
        {
            ExcursionDates = new ObservableCollection<ExcursionDate>();
            Destinations = new ObservableCollection<City>();
        }

        public DateTime FirstDate => ExcursionDates.OrderBy(e => e.CheckIn).FirstOrDefault().CheckIn;

        #endregion Constructors

        #region Properties

        public virtual ObservableCollection<City> Destinations { get; set; }

        public bool DiscountsExist { get; set; }
        public bool NightStart { get; set; }
        public bool DOBNeeded { get; set; }

        public virtual ObservableCollection<ExcursionDate> ExcursionDates { get; set; }

        [Required]
        public virtual ExcursionCategory ExcursionType { get; set; }

        public bool IncludesBus { get; set; }

        public bool IncludesPlane { get; set; }

        public bool IncludesShip { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; }

        public bool PassportNeeded { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}