using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.People;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model
{
    public class Transaction : EditTracker
    {
        #region Constructors

        public Transaction()
        {
            Date = DateTime.Today;
            CheckIn = DateTime.Today;
        }

        #endregion Constructors

        #region Fields

        private decimal _Amount;
        private Booking _Booking;
        private DateTime _CheckIn;
        private DateTime _CheckOut;
        private DateTime _Date;
        private string _Description;
        private Excursion _Excursion;
        private ExcursionExpenseCategories _ExcursionExpenseCategory;
        private ExpenseBaseCategories _ExpenseBaseCategory;
        private GeneralOrDatesExpenseCategories _GeneralOrDatesExpenseCategory;
        private GroupExpenseCategories _GroupExpenseCategory;
        private IncomeBaseCategories _IncomeBaseCategory;
        private User _PaymentTo;
        private Personal_Booking _PersonalBooking;
        private StandardExpenseCategories _StandardExpenseCategory;
        private TaxExpenseCategories _TaxExpenseCategory;
        private ThirdParty_Booking _ThirdPartyBooking;
        private TransactionType _TransactionType;
        private User _User;

        #endregion Fields

        #region Properties

        public decimal Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (_Amount == value)
                {
                    return;
                }

                _Amount = value;
                RaisePropertyChanged();
            }
        }

        public Booking Booking
        {
            get
            {
                return _Booking;
            }

            set
            {
                if (_Booking == value)
                {
                    return;
                }

                _Booking = value;
                RaisePropertyChanged();
            }
        }

        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }

                _CheckIn = value;
                if (CheckOut < CheckIn)
                {
                    CheckOut = CheckIn.AddDays(3);
                }
                RaisePropertyChanged();
            }
        }

        public DateTime CheckOut
        {
            get
            {
                return _CheckOut;
            }

            set
            {
                if (_CheckOut == value)
                {
                    return;
                }

                _CheckOut = value;
                if (CheckOut < CheckIn)
                {
                    CheckIn = CheckOut;
                }
                RaisePropertyChanged();
            }
        }

        public DateTime Date
        {
            get
            {
                return _Date;
            }

            set
            {
                if (_Date == value)
                {
                    return;
                }

                _Date = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description == value)
                {
                    return;
                }

                _Description = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public bool Editing { get; set; } = false;

        public Excursion Excursion
        {
            get
            {
                return _Excursion;
            }

            set
            {
                if (_Excursion == value)
                {
                    return;
                }

                _Excursion = value;
                RaisePropertyChanged();
            }
        }

        public ExcursionExpenseCategories ExcursionExpenseCategory
        {
            get
            {
                return _ExcursionExpenseCategory;
            }

            set
            {
                if (_ExcursionExpenseCategory == value)
                {
                    return;
                }

                _ExcursionExpenseCategory = value;
                RaisePropertyChanged();
            }
        }

        public ExpenseBaseCategories ExpenseBaseCategory
        {
            get
            {
                return _ExpenseBaseCategory;
            }

            set
            {
                if (_ExpenseBaseCategory == value)
                {
                    return;
                }

                _ExpenseBaseCategory = value;
                RaisePropertyChanged();
                if (Editing)
                {
                    GroupExpenseCategory = GroupExpenseCategories.None;
                    IncomeBaseCategory = IncomeBaseCategories.None;
                    StandardExpenseCategory = StandardExpenseCategories.None;
                    TaxExpenseCategory = TaxExpenseCategories.None;
                    GeneralOrDatesExpenseCategory = GeneralOrDatesExpenseCategories.Total;
                }
            }
        }

        public GeneralOrDatesExpenseCategories GeneralOrDatesExpenseCategory
        {
            get
            {
                return _GeneralOrDatesExpenseCategory;
            }

            set
            {
                if (_GeneralOrDatesExpenseCategory == value)
                {
                    return;
                }

                _GeneralOrDatesExpenseCategory = value;
                RaisePropertyChanged();
            }
        }

        public GroupExpenseCategories GroupExpenseCategory
        {
            get
            {
                return _GroupExpenseCategory;
            }

            set
            {
                if (_GroupExpenseCategory == value)
                {
                    return;
                }

                _GroupExpenseCategory = value;
                RaisePropertyChanged();
                if (Editing)
                {
                    IncomeBaseCategory = IncomeBaseCategories.None;
                    StandardExpenseCategory = StandardExpenseCategories.None;
                    TaxExpenseCategory = TaxExpenseCategories.None;
                    ExcursionExpenseCategory = ExcursionExpenseCategories.Total;
                }
            }
        }

        public IncomeBaseCategories IncomeBaseCategory
        {
            get
            {
                return _IncomeBaseCategory;
            }

            set
            {
                if (_IncomeBaseCategory == value)
                {
                    return;
                }

                _IncomeBaseCategory = value;
                RaisePropertyChanged();
                if (Editing)
                {
                    GroupExpenseCategory = GroupExpenseCategories.None;
                    ExpenseBaseCategory = ExpenseBaseCategories.None;
                    StandardExpenseCategory = StandardExpenseCategories.None;
                    TaxExpenseCategory = TaxExpenseCategories.None;
                }
            }
        }

        public User PaymentTo
        {
            get
            {
                return _PaymentTo;
            }

            set
            {
                if (_PaymentTo == value)
                {
                    return;
                }

                _PaymentTo = value;
                RaisePropertyChanged();
            }
        }

        public Personal_Booking PersonalBooking
        {
            get
            {
                return _PersonalBooking;
            }

            set
            {
                if (_PersonalBooking == value)
                {
                    return;
                }

                _PersonalBooking = value;
                RaisePropertyChanged();
            }
        }

        public bool Saved => Id == 0;

        public StandardExpenseCategories StandardExpenseCategory
        {
            get
            {
                return _StandardExpenseCategory;
            }

            set
            {
                if (_StandardExpenseCategory == value)
                {
                    return;
                }

                _StandardExpenseCategory = value;
                RaisePropertyChanged();
                if (Editing)
                {
                    GroupExpenseCategory = GroupExpenseCategories.None;
                    IncomeBaseCategory = IncomeBaseCategories.None;
                    TaxExpenseCategory = TaxExpenseCategories.None;
                }
            }
        }

        public TaxExpenseCategories TaxExpenseCategory
        {
            get
            {
                return _TaxExpenseCategory;
            }

            set
            {
                if (_TaxExpenseCategory == value)
                {
                    return;
                }

                _TaxExpenseCategory = value;
                RaisePropertyChanged();
                if (Editing)
                {
                    GroupExpenseCategory = GroupExpenseCategories.None;
                    IncomeBaseCategory = IncomeBaseCategories.None;
                    StandardExpenseCategory = StandardExpenseCategories.None;
                }
            }
        }

        public ThirdParty_Booking ThirdPartyBooking
        {
            get
            {
                return _ThirdPartyBooking;
            }

            set
            {
                if (_ThirdPartyBooking == value)
                {
                    return;
                }

                _ThirdPartyBooking = value;
                RaisePropertyChanged();
            }
        }

        public string TransactionDescription => GetDescription();

        public TransactionType TransactionType
        {
            get
            {
                return _TransactionType;
            }

            set
            {
                if (_TransactionType == value)
                {
                    return;
                }

                _TransactionType = value;
                RaisePropertyChanged();
                if (Editing)
                {
                    GroupExpenseCategory = GroupExpenseCategories.None;
                    IncomeBaseCategory = IncomeBaseCategories.None;
                    ExpenseBaseCategory = ExpenseBaseCategories.None;
                    StandardExpenseCategory = StandardExpenseCategories.None;
                    TaxExpenseCategory = TaxExpenseCategories.None;
                }
            }
        }

        public User User
        {
            get
            {
                return _User;
            }

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public string GetDates()
        {
            if (CheckIn != CheckOut)
            {
                return " για " + CheckIn.ToString("dd/MM-") + CheckOut.ToString("dd/MM");
            }
            else
            {
                return " για " + CheckIn.ToString("dd/MM");
            }
        }

        public string GetNames()
        {
            string names = Booking != null ? Booking.Names : PersonalBooking != null ? PersonalBooking.Names : ThirdPartyBooking != null ? ThirdPartyBooking.Names : "";
            return (names.IndexOfAny(new char[] { ',' }) >= 0 ? " των " : " του ") + names;
        }

        private string GetDescription()
        {
            if (TransactionType == TransactionType.Income)
            {
                if (IncomeBaseCategory == IncomeBaseCategories.OptionalActivities)
                {
                    return GetNames();
                }
                return "Έσοδο";
            }
            else if (TransactionType == TransactionType.Expense)
            {
                switch (ExpenseBaseCategory)
                {
                    case ExpenseBaseCategories.None:
                        return "Έξοδο";

                    case ExpenseBaseCategories.GroupExpense:
                        switch (GroupExpenseCategory)
                        {
                            case GroupExpenseCategories.None:
                                return "Έξοδο για: " + Excursion.Name;

                            case GroupExpenseCategories.Hotel:
                                switch (ExcursionExpenseCategory)
                                {
                                    case ExcursionExpenseCategories.Dates:
                                        return "Ξενοδοχείο για: " + Excursion.Name + GetDates();

                                    case ExcursionExpenseCategories.Booking:
                                        return "Ξενοδοχείο για: " + Excursion.Name + GetNames();

                                    default:
                                        return "Ξενοδοχείο για: " + Excursion.Name;
                                }

                            case GroupExpenseCategories.Bus:
                                switch (ExcursionExpenseCategory)
                                {
                                    case ExcursionExpenseCategories.Dates:
                                        return "Λεοφωρείο για: " + Excursion.Name + GetDates();

                                    case ExcursionExpenseCategories.Booking:
                                        return "Λεοφωρείο για: " + Excursion.Name + GetNames();

                                    default:
                                        return "Λεοφωρείο για: " + Excursion.Name;
                                }

                            case GroupExpenseCategories.Escort:
                                switch (ExcursionExpenseCategory)
                                {
                                    case ExcursionExpenseCategories.Dates:
                                        return "Συνοδός για: " + Excursion.Name + GetDates();

                                    case ExcursionExpenseCategories.Booking:
                                        return "Συνοδός για: " + Excursion.Name + GetNames();

                                    default:
                                        return "Συνοδός για: " + Excursion.Name;
                                }

                            case GroupExpenseCategories.Guide:
                                switch (ExcursionExpenseCategory)
                                {
                                    case ExcursionExpenseCategories.Dates:
                                        return "Ξεναγός για: " + Excursion.Name + GetDates();

                                    case ExcursionExpenseCategories.Booking:
                                        return "Ξεναγός για: " + Excursion.Name + GetNames();

                                    default:
                                        return "Ξεναγός για: " + Excursion.Name;
                                }

                            default:
                                return "Έξοδο για:" + Excursion.Name;
                        }

                    case ExpenseBaseCategories.PersonelExpense:
                        return "Έξοδο ατομικού" + GetNames();

                    case ExpenseBaseCategories.StandardExpense:
                        switch (StandardExpenseCategory)
                        {
                            case StandardExpenseCategories.None:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Πάγιο έξοδο";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Πάγιο έξοδο" + GetDates();
                                }
                                return "Πάγιο έξοδο";

                            case StandardExpenseCategories.Power:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Ρεύμα";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Ρεύμα" + GetDates();
                                }
                                return "Ρεύμα";

                            case StandardExpenseCategories.Water:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Νερό";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Νερό" + GetDates();
                                }
                                return "Νερό";

                            case StandardExpenseCategories.Rent:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Ενοίκιο";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Ενοίκιο" + GetDates();
                                }
                                return "Ενοίκιο";

                            case StandardExpenseCategories.BuildingFees:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Κοινόχρηστα";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Κοινόχρηστα" + GetDates();
                                }
                                return "Κοινόχρηστα";

                            case StandardExpenseCategories.Telephone:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Σταθερό";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Σταθερό" + GetDates();
                                }
                                return "Σταθερό";

                            case StandardExpenseCategories.Mobile:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Κινητό";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Κινητό" + GetDates();
                                }
                                return "Κινητό";

                            default:
                                return "Πάγιο έξοδο";
                        }

                    case ExpenseBaseCategories.TaxExpense:
                        switch (TaxExpenseCategory)
                        {
                            case TaxExpenseCategories.None:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Φορολογικό Έξοδο";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Φορολογικό Έξοδο" + GetDates();
                                }
                                return "Φορολογικό Έξοδο";

                            case TaxExpenseCategories.Setting:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Ρύθμιση Οφειλής";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Ρύθμιση Οφειλής" + GetDates();
                                }
                                return "Ρύθμιση Οφειλής";

                            case TaxExpenseCategories.IncomeTax:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Φόρος Εισοδήματος";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Φόρος Εισοδήματος" + GetDates();
                                }
                                return "Φόρος Εισοδήματος";

                            case TaxExpenseCategories.GeneralTaxes:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Φόρος";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Φόρος" + GetDates();
                                }
                                return "Φόρος";

                            case TaxExpenseCategories.EFKA:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "ΕΦΚΑ";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "ΕΦΚΑ" + GetDates();
                                }
                                return "ΕΦΚΑ";

                            case TaxExpenseCategories.IKA:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "IKA";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "IKA" + GetDates();
                                }
                                return "IKA";

                            default:
                                switch (GeneralOrDatesExpenseCategory)
                                {
                                    case GeneralOrDatesExpenseCategories.Total:
                                        return "Φορολογικό Έξοδο";

                                    case GeneralOrDatesExpenseCategories.Dates:
                                        return "Φορολογικό Έξοδο" + GetDates();
                                }
                                return "Φορολογικό Έξοδο";
                        }
                    default:
                        return "Έξοδο";
                }
            }
            return "Error";
        }

        #endregion Methods
    }
}