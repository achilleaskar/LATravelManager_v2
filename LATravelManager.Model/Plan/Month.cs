using System.Collections.Generic;
using System.Windows.Media;

namespace LATravelManager.Model.Plan
{
    public class Month
    {
        #region Properties

        public Brush Background { get; set; }

        public List<PlanDatetInfo> Days { get; set; } = new List<PlanDatetInfo>();

        public string Name { get; set; }

        #endregion Properties
    }
}