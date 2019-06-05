using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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