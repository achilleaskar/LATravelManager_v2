using LATravelManager.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class RoomOption : BaseModel
    {
        #region Fields

        public const string EnabledPropertyName = nameof(Enabled);
        public const string OptionDatePropertyName = nameof(OptionDate);

        public const string OptionNotePropertyName = nameof(OptionNote);
        private bool _Enabled = false;
        private DateTime _OptionDate;

        private string _OptionNote = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Enabled property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }

            set
            {
                if (_Enabled == value)
                {
                    return;
                }

                _Enabled = value;
                RaisePropertyChanged(EnabledPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the OptionDate property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public DateTime OptionDate
        {
            get
            {
                return _OptionDate;
            }

            set
            {
                if (_OptionDate == value)
                {
                    return;
                }

                _OptionDate = value;
                RaisePropertyChanged(OptionDatePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the OptionNote property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(20)]
        public string OptionNote
        {
            get
            {
                return _OptionNote;
            }

            set
            {
                if (_OptionNote == value)
                {
                    return;
                }

                _OptionNote = value;
                RaisePropertyChanged(OptionNotePropertyName);
            }
        }

        #endregion Properties
    }
}