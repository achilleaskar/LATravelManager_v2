using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.Plan;
using LATravelManager.Model.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.Wrapper
{
    public class RoomWrapper : ModelWrapper<Room>
    {
        #region Constructors

        public RoomWrapper(Room model) : base(model)
        {
            PlanDailyInfo = new List<PlanDailyInfo>();
        }

        public RoomWrapper() : this(new Room())
        {
        }

        #endregion Constructors

        #region Fields

        private bool _IsSelected;
        private string _LocalNote;
        private List<PlanDailyInfo> _PlanDailyInfo;

        #endregion Fields

        [NotMapped]
        public bool Handled { get; set; }

        private bool _Merged;

        public bool Merged
        {
            get
            {
                return _Merged;
            }

            set
            {
                if (_Merged == value)
                {
                    return;
                }

                _Merged = value;
                RaisePropertyChanged();
            }
        }

        #region Properties

        public bool CanMerge(List<PlanDailyInfo> datesNeeded)
        {
            for (var i = 0; i < PlanDailyInfo.Count; i++)
                if (datesNeeded[i].RoomState != RoomStateEnum.NotAvailable && PlanDailyInfo[i].RoomState != RoomStateEnum.NotAvailable) //ksanades to
                    return false;
            return true;
        }

        public List<BookingInfoPerDay> DailyBookingInfo
        {
            get { return GetValue<List<BookingInfoPerDay>>(); }
            set { SetValue(value); }
        }

        public string Dates => GetDates();

        public DateTime EndDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public Hotel Hotel
        {
            get { return GetValue<Hotel>(); }
            set { SetValue(value); }
        }

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }

            set
            {
                if (_IsSelected == value)
                {
                    return;
                }

                _IsSelected = value;
                RaisePropertyChanged();
            }
        }

        public string LocalNote
        {
            get
            {
                return _LocalNote;
            }

            set
            {
                if (_LocalNote == value)
                {
                    return;
                }

                _LocalNote = value;
                RaisePropertyChanged();
            }
        }

        public string Name => GetRoomName();

        public string Note
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<Option> Options
        {
            get { return GetValue<ObservableCollection<Option>>(); }
            set { SetValue(value); }
        }

        public List<PlanDailyInfo> PlanDailyInfo
        {
            get
            {
                return _PlanDailyInfo;
            }

            set
            {
                if (_PlanDailyInfo == value)
                {
                    return;
                }

                _PlanDailyInfo = value;
                RaisePropertyChanged();
            }
        }

        public RoomType RoomType
        {
            get { return GetValue<RoomType>(); }
            set { SetValue(value); }
        }

        public DateTime StartDate
        {
            get
            {
                DateTime _StartDate = GetValue<DateTime>();
                if (_StartDate.Year < 2000)
                {
                    if (DailyBookingInfo.Count > 0)
                    {
                        StartDate = EndDate = DailyBookingInfo[0].Date;
                        foreach (BookingInfoPerDay d in DailyBookingInfo)
                        {
                            if (d.Date > EndDate)
                                EndDate = d.Date;
                            if (d.Date < _StartDate)
                                StartDate = d.Date;
                        }
                    }
                }

                return GetValue<DateTime>();
            }
            set { SetValue(value); }
        }

        #endregion Properties

        #region Methods

        public bool CanAddReservationToRoom(ReservationWrapper reservationWr, bool includeSelf = false, bool includeAllotment = false)
        {
            DateTime tmpDate = reservationWr.CheckIn;
            for (int i = 0; i < PlanDailyInfo.Count; i++)
            {
                if (PlanDailyInfo[i].Date > reservationWr.CheckOut)
                    return false;
                if (tmpDate == PlanDailyInfo[i].Date)
                {
                    bool positive;
                    if ((PlanDailyInfo[i].RoomState == RoomStateEnum.Available && (PlanDailyInfo[i].RoomTypeEnm==RoomTypeEnum.Available || (includeAllotment&& PlanDailyInfo[i].RoomTypeEnm == RoomTypeEnum.Booking))) ||
                       (PlanDailyInfo[i].RoomState == RoomStateEnum.MovableNoName && includeSelf))
                    {
                        positive = true;
                    }
                    else
                        return false;
                    if (tmpDate == reservationWr.CheckOut.AddDays(-1) && positive)
                        return true;
                    tmpDate = tmpDate.AddDays(1);
                }
            }
            return false;
        }

        public void CancelReservation(ReservationWrapper reservationWr)
        {
            DateTime tmpDate = reservationWr.CheckIn;
            PlanDailyInfo tmpBookedInfo = null;

            while (tmpDate < reservationWr.CheckOut)
            {
                tmpBookedInfo = PlanDailyInfo.Find(x => x.Date == tmpDate);
                tmpBookedInfo.Reservation = null;
                tmpBookedInfo.RoomState = RoomStateEnum.Available;
                tmpDate = tmpDate.AddDays(1);
            }
            reservationWr.Room = null;
        }

        public bool CanFit(ReservationWrapper reservationWr)
        {
            return reservationWr.NoNameRoomType.Id == RoomType.Id;
            //if (reservationWr.CustomersList.Count >= RoomType.MinCapacity && reservationWr.CustomersList.Count <= RoomType.MaxCapacity)
            //{
            //    return true;
            //}
            //if (reservationWr.CustomersList.Count==1 && RoomType.MinCapacity==2)
            //{
            //    return true;

            //}
            //return false;
        }

        public bool MakeNoNameReservation(ReservationWrapper reservationWr)
        {
            DateTime tmpDate = reservationWr.CheckIn;
            int dayNum = 0;
            if (reservationWr.Room != null)
                (new RoomWrapper(reservationWr.Room)).CancelReservation(reservationWr);
            if (reservationWr.NoNameRoom != null)
                (new RoomWrapper(reservationWr.NoNameRoom)).CancelReservation(reservationWr);
            reservationWr.NoNameRoom = Model;
            PlanDailyInfo tmpPlanInfo = null;
            RoomStateEnum roomState = RoomStateEnum.MovableNoName;
            while (tmpDate < reservationWr.CheckOut)
            {
                dayNum++;
                tmpPlanInfo = PlanDailyInfo.Find(x => x.Date == tmpDate);
                if (tmpPlanInfo != null)
                {
                    if (tmpPlanInfo.RoomState == RoomStateEnum.Available)
                    {
                        tmpPlanInfo.RoomState = roomState;
                        tmpPlanInfo.Reservation = reservationWr;
                        if (tmpDate == reservationWr.CheckIn)
                        {
                            tmpPlanInfo.DayState = DayStateEnum.FirstDay;
                            //tmpPlanInfo.Text = dayNum.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("The room is not available on the day you try to do a noname reservation!");
                    }
                    tmpDate = tmpDate.AddDays(1);
                }
                else
                {
                    MessageBox.Show("Egine m@l@ki@");
                    return false;
                }
            }
            //if (tmpBookedInfo != null)
            //    tmpBookedInfo.Text = dayNum.ToString();
            return true;
        }

        public void MakeReservation(ReservationWrapper reservationWr)
        {
            try
            {
                if (Id==1800)
                {

                }
                DateTime tmpDate = reservationWr.CheckIn;
                int dayNum = 0;
                PlanDailyInfo tmpPlanInfo = null;
                while (tmpDate < reservationWr.CheckOut)
                {
                    //mporei na ginei pio grhgoro na kses alla isws dn aksizei.
                    dayNum++;
                    tmpPlanInfo = PlanDailyInfo.Find(x => x.Date == tmpDate);
                    if (tmpPlanInfo == null)
                    {
                        MessageBox.Show("Egine m@l@ki@");
                        return;
                    }
                    if (tmpPlanInfo.RoomState == RoomStateEnum.Available || tmpPlanInfo.RoomState == RoomStateEnum.MovableNoName || tmpPlanInfo.RoomState == RoomStateEnum.Allotment)
                    {
                        tmpPlanInfo.Reservation = reservationWr;
                        tmpPlanInfo.RoomState = RoomStateEnum.Booked;
                        if (tmpDate == reservationWr.CheckIn)
                        {
                            tmpPlanInfo.DayState = DayStateEnum.FirstDay;
                            //tmpPlanInfo.Text = dayNum.ToString();
                        }
                    }
                    else
                        tmpPlanInfo.RoomState = RoomStateEnum.OverBookedByMistake;
                    tmpDate = tmpDate.AddDays(1);
                }
                //if (tmpBookedInfo != null)
                //    tmpBookedInfo.Text = dayNum.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool IsAvailable(DateTime checkIn, DateTime checkOut)
        {
            DateTime tmpDate = checkIn;
            BookingInfoPerDay tmpBookedInfo = null;
            while (tmpDate < checkOut)
            {
                tmpBookedInfo = DailyBookingInfo.Find(x => x.Date == tmpDate);
                if (tmpBookedInfo == null)
                    return false;
                tmpDate = tmpDate.AddDays(1);
            }

            return true;
        }

        private string GetDates()
        {
            string dates = "";
            DailyBookingInfo = DailyBookingInfo.OrderBy(o => o.Date).ToList();

            if (DailyBookingInfo.Count > 0)
            {
                int i = 0;
                bool started = false;
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append(dates);
                for (i = 0; i < DailyBookingInfo.Count - 1; i++)
                {
                    if (!started)
                    {
                        builder.Append(DailyBookingInfo[i].Date.ToString("dd/MM"));
                        if (DailyBookingInfo[i].Date.AddDays(1) == DailyBookingInfo[i + 1].Date)
                        {
                            started = true;
                            builder.Append("-");
                        }
                        else
                            builder.Append(", ");
                    }
                    else
                    {
                        if (DailyBookingInfo[i].Date.AddDays(1) != DailyBookingInfo[i + 1].Date)
                        {
                            builder.Append(DailyBookingInfo[i].Date.AddDays(1).ToString("dd/MM") + ", ");
                            started = false;
                        }
                    }
                }
                builder.Append(DailyBookingInfo[i].Date.AddDays(1).ToString("dd/MM"));
                dates = builder.ToString();
            }
            return dates;
        }

        private string GetRoomName()
        {
            if (Id > 0)
            {
                return Id + Hotel.Name.Substring(0, 5);
            }
            else
            {
                return "ΟΧΙ";
            }
        }

        #endregion Methods
    }
}