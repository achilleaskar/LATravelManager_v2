using LATravelManager.Model.BookingData;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Plan
{
    public class BookingInfoPerDay : BaseModel
    {
        #region Fields

        private DateTime _Date;
        private Reservation _Reservation;

        #endregion Fields

        #region Properties

        private int _MinimunStay;

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

        private RoomTypeEnum _RoomTypeEnm;

        public RoomTypeEnum RoomTypeEnm
        {
            get
            {
                return _RoomTypeEnm;
            }

            set
            {
                if (_RoomTypeEnm == value)
                {
                    return;
                }

                _RoomTypeEnm = value;
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
            }
        }

        public RoomStateEnum GetRoomState()
        {
            throw new NotImplementedException();
        }

        #endregion Properties
    }
}