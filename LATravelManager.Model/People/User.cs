using LATravelManager.Model;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class User : EditTracker, INamed
    {

        #region Constructors

        public User()
        {
        }

        #endregion Constructors

        #region Fields

        public const string BaseLocationPropertyName = nameof(BaseLocation);
        public const string HashedPasswordPropertyName = nameof(HashedPassword);
        public const string LevelPropertyName = nameof(Level);
        public const string NamePropertyName = nameof(Name);
        public const string SurenamePropertyName = nameof(Surename);
        public const string TelPropertyName = nameof(Tel);
        public const string UserNamePropertyName = nameof(UserName);

        private GrafeiaXriston _BaseLocation;
        private byte[] _HashedPassword = new byte[0];
        private int _Level;
        private string _Name = string.Empty;
        private string _Surename = string.Empty;
        private string _Tel = string.Empty;
        private string _UserName = string.Empty;

        #endregion Fields

        #region Properties

        public enum GrafeiaXriston
        {
            Allo = 0,
            Thessalonikis = 1,
            Larisas = 2
        }

        /// <summary>
        /// Sets and gets the BaseCity property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public  GrafeiaXriston BaseLocation
        {
            get
            {
                return _BaseLocation;
            }

            set
            {
                if (_BaseLocation == value)
                {
                    return;
                }

                _BaseLocation = value;
                RaisePropertyChanged(BaseLocationPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the HashedPassword property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public byte[] HashedPassword
        {
            get
            {
                return _HashedPassword;
            }

            set
            {
                if (_HashedPassword == value)
                {
                    return;
                }

                _HashedPassword = value;
                RaisePropertyChanged(HashedPasswordPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Level property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int Level
        {
            get
            {
                return _Level;
            }

            set
            {
                if (_Level == value)
                {
                    return;
                }

                _Level = value;
                RaisePropertyChanged(LevelPropertyName);
            }
        }
        /// <summary>
        /// Sets and gets the Name property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Το όνομα απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το όνομα μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
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
                RaisePropertyChanged(NamePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Surename property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Το επίθετο απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Επίθετο μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
        public string Surename
        {
            get
            {
                return _Surename;
            }

            set
            {
                if (_Surename == value)
                {
                    return;
                }

                _Surename = value;
                RaisePropertyChanged(SurenamePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Tel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Το Τηλέφωνο απαιτείται!")]
        [StringLength(18, MinimumLength = 3, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες.")]
        [Phone(ErrorMessage = "Το τηλέφωνο δν έχει τη σωστή μορφή")]
        public string Tel
        {
            get
            {
                return _Tel;
            }

            set
            {
                if (_Tel == value)
                {
                    return;
                }

                _Tel = value;
                RaisePropertyChanged(TelPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the UserName property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        /// [DisplayName("Username")]
        [Required(ErrorMessage = "Το username απαιτείται!")]
        [StringLength(12, MinimumLength = 3, ErrorMessage = "Το username μπορεί να είναι απο 3 έως 12 χαρακτήρες.")]
        public string UserName
        {
            get
            {
                return _UserName;
            }

            set
            {
                if (_UserName == value)
                {
                    return;
                }

                _UserName = value;
                RaisePropertyChanged(UserNamePropertyName);
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return UserName;
        }

        #endregion Methods

    }
}