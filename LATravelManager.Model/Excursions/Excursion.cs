using LATravelManager.Model.Lists;
using LATravelManager.Model.Locations;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Data;

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

        public ObservableCollection<Bus> Buses { get; set; }

        public bool FixedDates { get; set; }

        public DateTime FirstDate => ExcursionDates != null && ExcursionDates.Count > 0 ? ExcursionDates.OrderBy(e => e.CheckIn).FirstOrDefault().CheckIn : new DateTime(1, 1, 1, 1, 1, 1);
        public DateTime LastDate => ExcursionDates != null && ExcursionDates.Count > 0 ? ExcursionDates.OrderByDescending(e => e.CheckOut).FirstOrDefault().CheckOut : new DateTime(1, 1, 1, 1, 1, 1);

        #endregion Constructors

        #region Properties

        public virtual ObservableCollection<City> Destinations { get; set; }

        public bool DiscountsExist { get; set; }
        public bool DOBNeeded { get; set; }

        public ObservableCollection<ExcursionDate> ExcursionDates { get; set; }

        [Required]
        public ExcursionCategory ExcursionType { get; set; }

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