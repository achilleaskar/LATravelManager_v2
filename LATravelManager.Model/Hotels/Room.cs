using LATravelManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows;
using static LATravelManager.Model.Enums;

namespace LATravelManager.Models
{
    public class Room : EditTracker
    {
        #region Constructors

        public Room()
        {
            Options = new ObservableCollection<Option>();
        }

        #endregion Constructors

        #region Fields

        public const string DailyBookingInfoPropertyName = nameof(DailyBookingInfo);
        public const string DatesPropertyName = nameof(Dates);
        public const string EndDatePropertyName = nameof(EndDate);
        public const string HotelPropertyName = nameof(Hotel);

        public const string IsSelectedPropertyName = nameof(IsSelected);
        public const string LocalNotePropertyName = nameof(LocalNote);
        public const string NotePropertyName = nameof(Note);

        public const string OptionsPropertyName = nameof(Options);
        public const string PlanDailyInfoPropertyName = nameof(PlanDailyInfo);
        public const string RoomTypePropertyName = nameof(RoomType);

        public const string StartDatePropertyName = nameof(StartDate);
        private List<BookingInfoPerDay> _DailyBookingInfo = new List<BookingInfoPerDay>();
        private DateTime _EndDate;
        private Hotel _Hotel = null;

        private bool _IsSelected = false;
        private string _LocalNote;
        private string _Note = string.Empty;

        private ObservableCollection<Option> _Options;
        private List<PlanDailyInfo> _PlanDailyInfo = new List<PlanDailyInfo>();
        private RoomType _RoomType;

        private DateTime _StartDate;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the BookingDates property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public virtual List<BookingInfoPerDay> DailyBookingInfo
        {
            get
            {
                return _DailyBookingInfo;
            }

            set
            {
                if (_DailyBookingInfo == value)
                {
                    return;
                }

                _DailyBookingInfo = value;
                RaisePropertyChanged(DailyBookingInfoPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Dates property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
        public string Dates => GetDates();

        /// <summary>
        /// Sets and gets the EndDate property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }

            set
            {
                if (_EndDate == value)
                {
                    return;
                }

                _EndDate = value;
                RaisePropertyChanged(EndDatePropertyName);
            }
        }

        internal bool IsAvailable(DateTime checkIn, DateTime checkOut)
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

        /// <summary>
        /// Sets and gets the Hotel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public virtual Hotel Hotel
        {
            get
            {
                return _Hotel;
            }

            set
            {
                if (_Hotel == value)
                {
                    return;
                }

                _Hotel = value;
                RaisePropertyChanged(HotelPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the IsSelected property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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
                RaisePropertyChanged(IsSelectedPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the LocalNote property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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
                RaisePropertyChanged(LocalNotePropertyName);
            }
        }

        public string Name => GetRoomName();

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
        /// Sets and gets the Options property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public virtual ObservableCollection<Option> Options
        {
            get
            {
                return _Options;
            }

            set
            {
                if (_Options == value)
                {
                    return;
                }

                _Options = value;
                RaisePropertyChanged(OptionsPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the PlanDailyInfo property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [NotMapped]
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
                RaisePropertyChanged(PlanDailyInfoPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the RoomType property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required(ErrorMessage = "Επιλέξτε τύπο δωματίου")]
        public virtual RoomType RoomType
        {
            get
            {
                return _RoomType;
            }

            set
            {
                if (_RoomType == value)
                {
                    return;
                }

                _RoomType = value;
                RaisePropertyChanged(RoomTypePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the StartDate property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                if (_StartDate.Year < 2000)
                {
                    if (DailyBookingInfo.Count > 0)
                    {
                        StartDate = EndDate = DailyBookingInfo[0].Date;
                        foreach (var d in DailyBookingInfo)
                        {
                            if (d.Date > EndDate)
                                EndDate = d.Date;
                            if (d.Date < _StartDate)
                                StartDate = d.Date;
                        }
                    }
                }
                return _StartDate;
            }

            set
            {
                if (_StartDate == value)
                {
                    return;
                }

                _StartDate = value;
                RaisePropertyChanged(StartDatePropertyName);
            }
        }

        #endregion Properties

        #region Methods

        public void CancelReservation(Reservation reservation)
        {
            DateTime tmpDate = reservation.CheckIn;
            PlanDailyInfo tmpBookedInfo = null;

            while (tmpDate < reservation.CheckOut)
            {
                tmpBookedInfo = PlanDailyInfo.Find(x => x.Date == tmpDate);
                tmpBookedInfo.Reservation = null;
                tmpBookedInfo.RoomState = RoomStateEnum.Available;
                tmpDate = tmpDate.AddDays(1);
            }
            reservation.Room = null;
        }

        public bool CanAddReservationToRoom(Reservation reservation, bool includeSelf = false, bool includeAllotment = false)
        {
            DateTime tmpDate = reservation.CheckIn;
            for (var i = 0; i < PlanDailyInfo.Count; i++)
            {
                if (PlanDailyInfo[i].Date > reservation.CheckOut)
                    return false;
                if (tmpDate == PlanDailyInfo[i].Date)
                {
                    bool positive;
                    if ((PlanDailyInfo[i].RoomState == RoomStateEnum.Available && (!PlanDailyInfo[i].IsAllotment || includeAllotment)) ||
                       (PlanDailyInfo[i].RoomState == RoomStateEnum.MovaBleNoName && includeSelf))
                    // || (includeSelf && DailyBookingInfo[i].Reservation != null &&
                    // DailyBookingInfo[i].Reservation.Id == reservation.Id &&
                    // DailyBookingInfo[i].Reservation.Room.Hotel.Id == 0))
                    {
                        positive = true;
                    }
                    else
                        return false;
                    if (tmpDate == reservation.CheckOut.AddDays(-1) && positive)
                        return true;
                    tmpDate = tmpDate.AddDays(1);
                }
            }
            return false;
        }

        public bool MakeNoNameReservation(Reservation reservation)
        {
            DateTime tmpDate = reservation.CheckIn;
            int dayNum = 0;
            if (reservation.Room != null)
                reservation.Room.CancelReservation(reservation);
            if (reservation.NoNameRoom != null)
                reservation.NoNameRoom.CancelReservation(reservation);
            reservation.NoNameRoom = this;
            PlanDailyInfo tmpPlanInfo = null;
            RoomStateEnum roomState = RoomStateEnum.MovaBleNoName;
            while (tmpDate < reservation.CheckOut)
            {
                dayNum++;
                tmpPlanInfo = PlanDailyInfo.Find(x => x.Date == tmpDate);
                if (tmpPlanInfo != null)
                {
                    if (tmpPlanInfo.RoomState == RoomStateEnum.Available)
                    {
                        tmpPlanInfo.RoomState = roomState;
                        tmpPlanInfo.Reservation = reservation;
                        if (tmpDate == reservation.CheckIn)
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

        public void MakeReservation(Reservation reservation)
        {
            try
            {
                DateTime tmpDate = reservation.CheckIn;
                int dayNum = 0;
                PlanDailyInfo tmpPlanInfo = null;
                while (tmpDate < reservation.CheckOut)
                {
                    //mporei na ginei pio grhgoro na kses alla isws dn aksizei.
                    dayNum++;
                    tmpPlanInfo = PlanDailyInfo.Find(x => x.Date == tmpDate);
                    if (tmpPlanInfo == null)
                    {
                        MessageBox.Show("Egine m@l@ki@");
                        return;
                    }
                    if (tmpPlanInfo.RoomState == RoomStateEnum.Available || tmpPlanInfo.RoomState == RoomStateEnum.MovaBleNoName || tmpPlanInfo.RoomState == RoomStateEnum.Allotment)
                    {
                        tmpPlanInfo.Reservation = reservation;
                        tmpPlanInfo.RoomState = RoomStateEnum.Booked;
                        if (tmpDate == reservation.CheckIn)
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

        private string GetDates()
        {
            var dates = "";
            DailyBookingInfo = DailyBookingInfo.OrderBy(o => o.Date).ToList();

            if (DailyBookingInfo.Count > 0)
            {
                var i = 0;
                var started = false;
                var builder = new System.Text.StringBuilder();
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

        public bool CanFit(Reservation reservation)
        {
            if (reservation.CustomersList.Count >= RoomType.MinCapacity && reservation.CustomersList.Count <= RoomType.MaxCapacity)
            {
                return true;
            }
            return false;
        }

        #endregion Methods
    }
}