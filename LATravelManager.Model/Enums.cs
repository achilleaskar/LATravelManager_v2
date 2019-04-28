﻿namespace LATravelManager.Model
{
    public static class Enums
    {
        public enum ExcursionTypeEnum
        {
            Bansko = 0,
            Skiathos = 1,
            Personal = 2,
            Group = 3,
            ThirdParty = 4
        }

        public enum ReservationTypeEnum
        {
            Normal,
            Noname,
            Overbooked,
            NoRoom,
            Transfer
        }

        public enum FlightMode
        {
            allerRetour,
            onlyGo,
            OnlyReturn
        }

        public enum RoomStateEnum
        {
            NotAvailable,
            Allotment,
            Available,
            MovaBleNoName,
            NotMovableNoName,
            Booked,
            OverBookedByMistake
        }

        public enum GrafeiaXriston
        {
            Allo = 0,
            Thessalonikis = 1,
            Larisas = 2
        }

        public enum HotelCategoryEnum
        {
            apart,
            threestar,
            fourstar,
            fivestar,
            house
        }

        public enum DayStateEnum
        {
            Empty,
            FirstDay,
            LastDay
        };

        public enum ContinentsEnum
        {
            NorthAmerica,
            SouthAmerica,
            Africa,
            Europe,
            Asia,
            Australia,
            Antarctica
        }
    }
}