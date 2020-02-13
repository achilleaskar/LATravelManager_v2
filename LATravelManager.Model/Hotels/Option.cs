using System;
using System.ComponentModel.DataAnnotations;
using LATravelManager.Model.Notifications;

namespace LATravelManager.Model.Hotels
{
    public class Option : EditTracker
    {
        #region Fields

        public Room Room { get; set; }

        public const string DatePropertyName = nameof(Date);

        public const string NotePropertyName = nameof(Note);
        private DateTime _Date;

        private string _Note = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Date property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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

        private NotifStatus _NotifStatus;

        public NotifStatus NotifStatus
        {
            get
            {
                return _NotifStatus;
            }

            set
            {
                if (_NotifStatus == value)
                {
                    return;
                }

                _NotifStatus = value;
                RaisePropertyChanged();
            }
        }

        [StringLength(100)]
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
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}