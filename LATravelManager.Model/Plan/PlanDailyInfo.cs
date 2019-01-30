using LATravelManager.Model;
using System;
using static LATravelManager.Model.Enums;

namespace LATravelManager.Models
{
    public class PlanDailyInfo : BaseModel
    {
        #region Fields

        public const string DatePropertyName = nameof(Date);

        public const string IsAllotmentsPropertyName = nameof(IsAllotment);
        public const string ReservationPropertyName = nameof(Reservation);
        public const string RoomStatePropertyName = nameof(RoomState);
        private DateTime _Date;

        private bool _IsAllotment = false;
        private Reservation _Reservation;

        private RoomStateEnum _RoomState;

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
                RaisePropertyChanged(DatePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the MyProperty property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool IsAllotment
        {
            get
            {
                return _IsAllotment;
            }

            set
            {
                if (_IsAllotment == value)
                {
                    return;
                }

                _IsAllotment = value;
                RaisePropertyChanged(IsAllotmentsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Reservation property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Reservation Reservation
        {
            get
            {
                return _Reservation;
            }

            set
            {
                if (_Reservation == value)
                {
                    return;
                }

                _Reservation = value;
                RaisePropertyChanged(ReservationPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the RoomState property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public RoomStateEnum RoomState
        {
            get
            {
                return _RoomState;
            }

            set
            {
                if (_RoomState == value)
                {
                    return;
                }

                _RoomState = value;
                RaisePropertyChanged(RoomStatePropertyName);
            }
        }

        #endregion Properties
    }
}