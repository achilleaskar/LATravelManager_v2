using LATravelManager.Model.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.People
{
    public class Company : BaseModel, INamed
    {

        #region Fields

        private CompanyActivity _Activity;
        private City _AddressCity;
        private int _AddressNumber;
        private string _AddressRoad;
        private string _AddressZipCode;
        private int _BillAddressNumber;
        private City _BillCity;
        private string _BillRoad;
        private string _BillZipCode;
        private int _Code;

        private string _Comment;

        private string _CompanyName;

        private Country _Country;

        private DateTime _CreationDate;

        private string _Email;

        private bool _IsAgent;

        private string _LastName;

        private string _MobilePhone;

        private string _Name;
        private string _Phone1;

        private string _Phone2;
        private string _TaxationNumber;

        private string _TaxOffice;

        #endregion Fields

        #region Properties

        public CompanyActivity Activity
        {
            get
            {
                return _Activity;
            }

            set
            {
                if (_Activity == value)
                {
                    return;
                }

                _Activity = value;
                RaisePropertyChanged();
            }
        }

        public int? ActivityId { get; set; }
        public City AddressCity
        {
            get
            {
                return _AddressCity;
            }

            set
            {
                if (_AddressCity == value)
                {
                    return;
                }

                _AddressCity = value;
                RaisePropertyChanged();
            }
        }

        public int AddressNumber
        {
            get
            {
                return _AddressNumber;
            }

            set
            {
                if (_AddressNumber == value)
                {
                    return;
                }

                _AddressNumber = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(100)]
        public string AddressRoad
        {
            get
            {
                return _AddressRoad;
            }

            set
            {
                if (_AddressRoad == value)
                {
                    return;
                }

                _AddressRoad = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(40)]
        public string AddressZipCode
        {
            get
            {
                return _AddressZipCode;
            }

            set
            {
                if (_AddressZipCode == value)
                {
                    return;
                }

                _AddressZipCode = value;
                RaisePropertyChanged();
            }
        }

        public string BillAddressFull => GetBillingAddress();

        [Required]
        public int BillAddressNumber
        {
            get
            {
                return _BillAddressNumber;
            }

            set
            {
                if (_BillAddressNumber == value)
                {
                    return;
                }

                _BillAddressNumber = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        public City BillCity
        {
            get
            {
                return _BillCity;
            }

            set
            {
                if (_BillCity == value)
                {
                    return;
                }

                _BillCity = value;
                RaisePropertyChanged();
            }
        }

        public int? BillCityId { get; set; }

        [Required]
        [StringLength(100)]
        public string BillRoad
        {
            get
            {
                return _BillRoad;
            }

            set
            {
                if (_BillRoad == value)
                {
                    return;
                }

                _BillRoad = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [MaxLength(10)]
        public string BillZipCode
        {
            get
            {
                return _BillZipCode;
            }

            set
            {
                if (_BillZipCode == value)
                {
                    return;
                }

                _BillZipCode = value;
                RaisePropertyChanged();
            }
        }

        public int? CityId { get; set; }

        public int Code
        {
            get
            {
                return _Code;
            }

            set
            {
                if (_Code == value)
                {
                    return;
                }

                _Code = value;
                RaisePropertyChanged();
            }
        }

        public string Comment
        {
            get
            {
                return _Comment;
            }

            set
            {
                if (_Comment == value)
                {
                    return;
                }

                _Comment = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [StringLength(120,MinimumLength =3)]
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }

            set
            {
                if (_CompanyName == value)
                {
                    return;
                }

                _CompanyName = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        public Country Country
        {
            get
            {
                return _Country;
            }

            set
            {
                if (_Country == value)
                {
                    return;
                }

                _Country = value;
                RaisePropertyChanged();
            }
        }

        public int? CountryId { get; set; }

        public DateTime CreationDate
        {
            get
            {
                return _CreationDate;
            }

            set
            {
                if (_CreationDate == value)
                {
                    return;
                }

                _CreationDate = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(50, MinimumLength = 0)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        [EmailAddress(ErrorMessage = "Το Email δεν έχει τη σωστή μορφή")]
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                if (_Email == value)
                {
                    return;
                }

                _Email = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAgent
        {
            get
            {
                return _IsAgent;
            }

            set
            {
                if (_IsAgent == value)
                {
                    return;
                }

                _IsAgent = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(40, MinimumLength = 3, ErrorMessage = "Το Όνομα μπορεί να είναι από 3 έως 30 χαρακτήρες")]
        public string LastName
        {
            get
            {
                return _LastName;
            }

            set
            {
                if (_LastName == value)
                {
                    return;
                }

                _LastName = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες")]
        [Phone(ErrorMessage = "Το τηλέφωνο δεν έχει τη σωστή μορφή")]
        public string MobilePhone
        {
            get
            {
                return _MobilePhone;
            }
            set
            {
                if (_MobilePhone == value)
                {
                    return;
                }
                _MobilePhone = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(120, MinimumLength = 3, ErrorMessage = "Το Όνομα μπορεί να είναι από 3 έως 120 χαρακτήρες")]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες")]
        [Phone(ErrorMessage = "Το τηλέφωνο δεν έχει τη σωστή μορφή")]
        public string Phone1
        {
            get
            {
                return _Phone1;
            }

            set
            {
                if (_Phone1 == value)
                {
                    return;
                }

                _Phone1 = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(18, MinimumLength = 10, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες")]
        [Phone(ErrorMessage = "Το τηλέφωνο δεν έχει τη σωστή μορφή")]
        public string Phone2
        {
            get
            {
                return _Phone2;
            }

            set
            {
                if (_Phone2 == value)
                {
                    return;
                }

                _Phone2 = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [StringLength(20)]
        public string TaxationNumber
        {
            get
            {
                return _TaxationNumber;
            }

            set
            {
                if (_TaxationNumber == value)
                {
                    return;
                }

                _TaxationNumber = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [StringLength(40)]
        public string TaxOffice
        {
            get
            {
                return _TaxOffice;
            }

            set
            {
                if (_TaxOffice == value)
                {
                    return;
                }

                _TaxOffice = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public bool IsValidToPrint()
        {
            //id 6 is Greece
            return Country.Id != 6 || TaxationNumberIsValid();
        }

        public override string ToString()
        {
            return CompanyName;
        }

        private string GetBillingAddress()
        {
            if (BillAddressNumber == 0 || BillRoad.TrimEnd().EndsWith(BillAddressNumber.ToString()))
            {
                return BillRoad;
            }
            else
            {
                return BillRoad + " " + BillAddressNumber;
            }
        }
        private bool TaxationNumberIsValid()
        {
            bool result = false;
            if (TaxationNumber.Length != 9)
            {
                return result;
            }
            int total = 0;
            foreach (char c in TaxationNumber)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            int currentDigit = 0;
            for (int i = 8; i >= 1; i--)
            {
                currentDigit = TaxationNumber[i - 1] - '0';
                total += currentDigit * (int)Math.Pow(2, 9 - i);
            }
            return (total % 11) % 10 == (int)TaxationNumber[8] - '0';
        }

        #endregion Methods
    }
}