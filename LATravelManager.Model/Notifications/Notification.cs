using LATravelManager.Model.Wrapper;

namespace LATravelManager.Model.Notifications
{
    public class Notification : BaseModel
    {
        private string _Details;

        public string Details
        {
            get
            {
                return _Details;
            }

            set
            {
                if (_Details == value)
                {
                    return;
                }

                _Details = value;
                RaisePropertyChanged();
            }
        }





        private NotificaationType _NotificaationType;


        public NotificaationType NotificaationType
        {
            get
            {
                return _NotificaationType;
            }

            set
            {
                if (_NotificaationType == value)
                {
                    return;
                }

                _NotificaationType = value;
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


        private ReservationWrapper _ReservationWrapper;

        public ReservationWrapper ReservationWrapper
        {
            get
            {
                return _ReservationWrapper;
            }

            set
            {
                if (_ReservationWrapper == value)
                {
                    return;
                }

                _ReservationWrapper = value;
                RaisePropertyChanged();
            }
        }
    }
}