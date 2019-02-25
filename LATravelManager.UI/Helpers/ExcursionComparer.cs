using LATravelManager.Models;
using System;
using System.Collections.Generic;

namespace LATravelManager.UI.Helpers
{
    public class ExcursionComparer : IComparer<Excursion>
    {
        public int Compare(Excursion x, Excursion y)
        {
            DateTime xStart = x.ExcursionDates[0].CheckIn, yStart = y.ExcursionDates[0].CheckIn;

            foreach (var d in x.ExcursionDates)
            {
                if (d.CheckIn <= xStart)
                    xStart = d.CheckIn;
            }
            foreach (var d in y.ExcursionDates)
            {
                if (d.CheckIn <= yStart)
                    yStart = d.CheckIn;
            }

            if (xStart > yStart)
                return -1;

            if (xStart == yStart)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}