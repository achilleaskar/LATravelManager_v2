namespace LATravelManager.Model
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

        public enum TransactionType //1
        {
            Income,
            Expense
        }

        public enum IncomeBaseCategories
        {
            OptionalActivities
        }

        public enum ExpenseBaseCategories //10
        {
            GroupExpense,
            PersonalExpense,
            PersonelExpense,
            StandardExpense,
            TaxExpense

        }

        public enum GroupExpenseCategories //100
        {
            Hotel,
            Bus,
            Escort,
            Guide
        }


        public enum StandardExpenseCategories //100
        {
            Power,
            Water,
            Rent,
            BuildingFees,
            Telephone,
            Mobile
        }

        public enum TaxExpenseCategories //100
        {
            Setting, //rythmisi ofeilhs
            IncomeTax, //foros eisodhmatos
            GeneralTaxes,//allo foroi
            EFKA,
            IKA

        }

        public enum FileType
        {
            Avatar = 1, Photo
        }

        public enum ReservationTypeEnum
        {
            Normal,
            Noname,
            Overbooked,
            NoRoom,
            Transfer,
            OneDay
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
            MovableNoName,
            NotMovableNoName,
            Booked,
            OverBookedByMistake,
            Booking
        }

        public enum RoomTypeEnum
        {
            Available,
            Allotment,
            Booking
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