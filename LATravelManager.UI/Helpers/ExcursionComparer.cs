using System;
using System.Collections.Generic;
using LATravelManager.Model.Excursions;

namespace LATravelManager.UI.Helpers
{
    public class ExcursionComparer : IComparer<Excursion>
    {
        public int Compare(Excursion y, Excursion x)
        {

            if (x.ExcursionDates.Count == 0 || y.ExcursionDates.Count == 0)
            {
                if (x.Id > y.Id)
                    return -1;
                else if (x.Id == y.Id)
                    return 0;
                else
                    return 1;

            }
            DateTime xStart = x.ExcursionDates[0].CheckIn, yStart = y.ExcursionDates[0].CheckIn;

            foreach (ExcursionDate d in x.ExcursionDates)
            {
                if (d.CheckIn <= xStart)
                    xStart = d.CheckIn;
            }
            foreach (ExcursionDate d in y.ExcursionDates)
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