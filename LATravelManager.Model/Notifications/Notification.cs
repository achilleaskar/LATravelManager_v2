using System;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Services;
using LATravelManager.Model.Wrapper;

namespace LATravelManager.Model.Notifications
{
    public class Notification : BaseModel
    {
        private string _Details;

        private HotelOptions _HotelOptions;

        private Service _Service;

        public Service Service
        {
            get
            {
                return _Service;
            }

            set
            {
                if (_Service == value)
                {
                    return;
                }

                _Service = value;
                RaisePropertyChanged();
            }
        }

        public HotelOptions HotelOptions
        {
            get
            {
                return _HotelOptions;
            }

            set
            {
                if (_HotelOptions == value)
                {
                    return;
                }

                _HotelOptions = value;
                RaisePropertyChanged();
            }
        }

        public string UserName => GetUserName();
        public string PartnerName => GetPartnerName();

        private string GetPartnerName()
        {
            try
            {
                if (Service != null && Service.Personal_Booking.IsPartners)
                {
                    return Service.Personal_Booking.Partner.Name;
                }
                //if (ReservationWrapper != null && ReservationWrapper.ThirdPartyModel != null&& ReservationWrapper.ThirdPartyModel.part)
                //{
                //    return ReservationWrapper.ThirdPartyModel.Partner.Name;
                //}
                if (ReservationWrapper != null && ReservationWrapper.BookingWrapper != null && ReservationWrapper.BookingWrapper.IsPartners)
                {
                    return ReservationWrapper.BookingWrapper.Partner.Name;
                }
                return "";
            }
            catch (NullReferenceException)
            {
                return "Κενό";
            }
            catch (Exception)
            {
                return "Σφάλμα";
            }
        }

        private string GetUserName()
        {
            try
            {
                if (Service != null)
                {
                    return Service.Personal_Booking.User.UserName;
                }
                if (ReservationWrapper != null && ReservationWrapper.ThirdPartyModel != null)
                {
                    return ReservationWrapper.ThirdPartyModel.User.UserName;
                }
                if (ReservationWrapper != null && ReservationWrapper.BookingWrapper != null)
                {
                    return ReservationWrapper.BookingWrapper.User.UserName;
                }
                if (HotelOptions != null && HotelOptions.Options.Count > 0)
                {
                    return HotelOptions.Options[0].Room.UserName;
                }
                return "";
            }
            catch (NullReferenceException)
            {
                return "Κενό";
            }
            catch (Exception)
            {
                return "Σφάλμα";
            }
        }

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