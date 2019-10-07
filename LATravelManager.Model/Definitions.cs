using System;
using System.Collections.Generic;

namespace LATravelManager.Model
{
    public static class Definitions
    {
        //public enum ExcursionDurationTypeEnum
        //{
        //    //  NotSet,
        //    OneTime,
        //    Period,
        //    Continious
        //}

        public static int SelectedTemplate { get; set; } = -1;

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static Dictionary<HotelCategoryEnum, string> HotelCategoryDictionary = new Dictionary<HotelCategoryEnum, string>()
          {
            {HotelCategoryEnum.house,"Σπίτι" },
            {HotelCategoryEnum.apart,"Δωμάτια" },
            {HotelCategoryEnum.threestar,"3 Αστέρων" },
            {HotelCategoryEnum.fourstar,"4 Αστέρων" },
            {HotelCategoryEnum.fivestar,"5 Αστέρων" }
        };

        public static List<string> Continents = new List<string>() { "Ανταρκτική", "Ασία", "Αυστραλία", "Αφρική", "Ευρώπη", "Βόρεια Αμερική", "Νότια Αμερική" };
    }
}