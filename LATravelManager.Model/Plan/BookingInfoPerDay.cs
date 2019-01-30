using LATravelManager.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static LATravelManager.Model.Enums;

namespace LATravelManager.Models
{
    public class BookingInfoPerDay : BaseModel
    {
        #region Fields

        public const string DatePropertyName = nameof(Date);

        public const string IsAllotmentPropertyName = nameof(IsAllotment);
        public const string ReservationPropertyName = nameof(Reservation);
        public const string RoomStatePropertyName = nameof(RoomState);
        private DateTime _Date;
        private bool _IsAllotment = false;
        private Reservation _Reservation;
        private RoomStateEnum _RoomState;

        #endregion Fields

        #region Properties

        public const string MinimunStayPropertyName = nameof(MinimunStay);

        private int _MinimunStay;

        /// <summary>
        /// Sets and gets the MinimunStay property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int MinimunStay
        {
            get
            {
                return _MinimunStay;
            }

            set
            {
                if (_MinimunStay == value)
                {
                    return;
                }

                _MinimunStay = value;
                RaisePropertyChanged(MinimunStayPropertyName);
            }
        }

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
        /// Sets and gets the IsAllotment property. Changes to that property's value raise the
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
                RaisePropertyChanged(IsAllotmentPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Reservation property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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

        public RoomStateEnum GetRoomState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets and gets the RoomState property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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