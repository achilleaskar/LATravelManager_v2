using System.ComponentModel;

namespace LATravelManager.Model
{
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

    public enum ChangeType
    {
        Booking,
        LoginData
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PaymentMethod
    {
        [Description("Μετρητά")]
        Cash,
        [Description("Πειραιώς")]
        Peiraeus,
        [Description("Εθνική")]
        NBG,
        [Description("Eurobank")]
        Eurobank,
        [Description("Alpha Bank")]
        AlphaBank,
        [Description("VISA")]
        Visa,
        [Description("VIVA_ONL")]
        Viva_ONL,
        [Description("PAYPAL")]
        PAYPAL
    };

    public enum DayStateEnum
    {
        Empty,
        FirstDay,
        LastDay
    };


    public enum CardTypeEnum
    {
        [Description("Παραστατικό")]
        Reciept,
        [Description("Κράτηση")]
        Booking,
        [Description("Πληρωμή")]
        Deposit
    };

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum RecieptTypeEnum
    {
        [Description("ΑΠΥ - Απόδειξη Παροχής Υπηρεσιών")]
        ServiceReciept = 0,
        [Description("ΤΠΥ - Τιμολόγιο Παροχής Υπηρεσιών")]
        ServiceInvoice = 1,
        [Description("ΑΠΕ - Απόδειξη Πώλησης Εισιτηρίων")]
        AirTicketsReciept = 2,
        [Description("ΑΠΑ - Απόδειξη Πώλησης Ακτοπλοϊκών εισιτηρίων")]
        FerryTicketsReciept = 3,
        [Description("Ακυρωτικό Τιμολόγιο")]
        CancelationInvoice = 4,
        [Description("Πιστωτικό Τιμολόγιο")]
        CreditInvoice = 5,
        [Description("Προφόρμα")]
        Proforma = 6
    };

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ExcursionExpenseCategories //100
    {
        [Description("Σύνολο Εκδρομής")]
        Total = 0,

        [Description("Ημερομηνίες")]
        Dates = 1,

        [Description("Κράτηση")]
        Booking = 2
    }

    public enum ExcursionTypeEnum
    {
        Bansko = 0,
        Skiathos = 1,
        Personal = 2,
        Group = 3,
        ThirdParty = 4
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ExpenseBaseCategories //10
    {
        [Description("Γενικά")]
        None = 0,

        [Description("Οργανωμένων")]
        GroupExpense = 1,

        [Description("Ατομικών")]
        PersonelExpense = 2,

        [Description("Πάγια")]
        StandardExpense = 3,

        [Description("Φορολογικά")]
        TaxExpense = 4
    }

    public enum FileType
    {
        Avatar = 1,
        Photo = 2
    }

    public enum FlightMode
    {
        allerRetour,
        onlyGo,
        OnlyReturn
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum GeneralOrDatesExpenseCategories //100
    {
        [Description("Γενικά")]
        Total = 0,

        [Description("Ημερομηνίες")]
        Dates = 1,
    }

    public enum GrafeiaXriston
    {
        Allo = 0,
        Thessalonikis = 1,
        Larisas = 2
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum GroupExpenseCategories //100
    {
        [Description("Γενικά")]
        None = 0,

        [Description("Διαμονή")]
        Hotel = 1,

        [Description("Μεταφορά")]
        Bus = 2,

        [Description("Συνοδός")]
        Escort = 3,

        [Description("Ξεναγός")]
        Guide = 4
    }

    public enum HotelCategories
    {
        [Description("Σπίτι")]
        home = 6,

        [Description("Δωμάτιο")]
        room = 7,

        [Description("3 Αστέρων")]
        star3 = 8,

        [Description("4 Αστέρων")]
        star4 = 9,

        [Description("5 Αστέρων")]
        star5 = 10,
    };

    public enum HotelCategoryEnum
    {
        apart,
        threestar,
        fourstar,
        fivestar,
        house
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum IncomeBaseCategories
    {
        [Description("Γενικά")]
        None = 0,

        [Description("Προαιρετικά")]
        OptionalActivities = 1
    }

    public enum NotificaationType
    {
        CheckIn,
        Option,
        PersonalOption,
        NoPay,
    }

    public enum PaymentType
    {
        Recieved = 1,
        NotRecieved = 2,
        NotPaid = 3,
        Canceled = 4
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

    public enum SeatType
    {
        Driver,
        Leader,
        Normal,
        Road,
        Door
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum StandardExpenseCategories //100
    {
        [Description("Γενικά")]
        None = 0,

        [Description("Ρέυμα")]
        Power = 1,

        [Description("Νερό")]
        Water = 2,

        [Description("Ενοίκιο")]
        Rent = 3,

        [Description("Κοινόχρηστα")]
        BuildingFees = 4,

        [Description("Σταθερό")]
        Telephone = 5,

        [Description("Κινητό")]
        Mobile = 6
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum TaxExpenseCategories //100
    {
        [Description("Γενικά")]
        None = 0,

        [Description("Ρύθμιση Οφειλής")]
        Setting = 1, //rythmisi ofeilhs

        [Description("Φόρος Εισοδήματος")]
        IncomeTax = 2, //foros eisodhmatos

        [Description("Άλλοι Φόροι")]
        GeneralTaxes = 3,//allo foroi

        [Description("ΕΦΚΑ")]
        EFKA = 4,

        [Description("ΙΚΑ")]
        IKA = 5
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum TransactionType //1
    {
        [Description("Έσοδο")]
        Income = 0,

        [Description("Έξοδο")]
        Expense = 1
    }
}