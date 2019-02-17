using LATravelManager.Model;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class Partner : EditTracker, INamed
    {

        #region Constructors

        public Partner()
        {
        }

        #endregion Constructors

        #region Fields

        public const string EmailPropertyName = nameof(Email);
        public const string NamePropertyName = nameof(Name);

        public const string NotePropertyName = nameof(Note);
        public const string TelPropertyName = nameof(Tel);
        private string _Email;
        private string _Name = string.Empty;

        private string _Note = string.Empty;

        private string _Tel = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Email property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [EmailAddress]
        [StringLength(30)]
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
                RaisePropertyChanged(EmailPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Name property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        [StringLength(20)]
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
        /// Sets and gets the Note property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string Note
        {
            get
            {
                return _Note;
            }

            set
            {
                if (_Note == value)
                {
                    return;
                }

                _Note = value;
                RaisePropertyChanged(NotePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Tel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        [StringLength(20)]
        [Phone]
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

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods

    }
}