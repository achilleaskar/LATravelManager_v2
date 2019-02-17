namespace LATravelManager.Model
{
    public static class Enums
    {
        public enum ExcursionTypeEnum
        {
            Bansko,
            Skiathos,
            Personal,
            Group,
            ThirdParty
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
