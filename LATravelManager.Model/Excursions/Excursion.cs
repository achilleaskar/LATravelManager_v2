using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Locations;

namespace LATravelManager.Model.Excursions
{
    public class Excursion : EditTracker
    {
        #region Constructors

        public Excursion()
        {
            ExcursionDates = new ObservableCollection<ExcursionDate>();
            Destinations = new ObservableCollection<City>();
            Periods = new ObservableCollection<PricingPeriod>();
        }

        public ObservableCollection<Bus> Buses { get; set; }

        public bool FixedDates { get; set; }

        public DateTime FirstDate => ExcursionDates != null && ExcursionDates.Count > 0 ? ExcursionDates.OrderBy(e => e.CheckIn).FirstOrDefault().CheckIn : new DateTime(1, 1, 1, 1, 1, 1);
        public DateTime LastDate => ExcursionDates != null && ExcursionDates.Count > 0 ? ExcursionDates.OrderByDescending(e => e.CheckOut).FirstOrDefault().CheckOut : new DateTime(1, 1, 1, 1, 1, 1);

        #endregion Constructors

        #region Properties

        public int SecondDepartMinDiff { get; set; }

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

        private bool _Deactivated;

        public bool Deactivated
        {
            get
            {
                return _Deactivated;
            }

            set
            {
                if (_Deactivated == value)
                {
                    return;
                }

                _Deactivated = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<PricingPeriod> _Periods;

        public ObservableCollection<PricingPeriod> Periods
        {
            get
            {
                return _Periods;
            }

            set
            {
                if (_Periods == value)
                {
                    return;
                }

                _Periods = value;
                RaisePropertyChanged();
            }
        }
    }
}