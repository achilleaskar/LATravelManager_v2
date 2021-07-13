using System;
using System.Windows.Media;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Wrapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace LATravelManager.Model.LocalModels
{
    public class
        PlanDailyInfo : BaseModel
    {
        #region Fields

        private SolidColorBrush _CellColor;
        private DateTime _Date;
        private bool _IsDateSelected;
        private ReservationWrapper _Reservation;
        private RoomWrapper _Room;
        private RoomTypeEnum _RoomTypeEnm;


        public SolidColorBrush Foreground => !string.IsNullOrEmpty(Text) && Reservation != null && Reservation.Confirmed ? new SolidColorBrush(Colors.Blue) : new SolidColorBrush(Colors.Black);

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

        private RoomStateEnum _RoomState;

        private string _Text;

        #endregion Fields

        #region Properties

        private int _MinimumStay;

        public int MinimumStay
        {
            get
            {
                return _MinimumStay;
            }

            set
            {
                if (_MinimumStay == value)
                {
                    return;
                }

                _MinimumStay = value;
                RaisePropertyChanged();
            }
        }

        public SolidColorBrush CellColor
        {
            get
            {
                return _CellColor;
            }

            set
            {
                if (_CellColor == value)
                {
                    return;
                }

                _CellColor = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }

        public DayStateEnum DayState { get; set; }

        /// <summary>
        /// Sets and gets the MyProperty property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>

        public bool IsDateSelected
        {
            get => _IsDateSelected;
            set
            {
                if (_IsDateSelected != value)
                {
                    _IsDateSelected = value;
                    if (value)
                        CellColor.Opacity = 0.4;
                    else
                        CellColor.Opacity = 1;

                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CellColor));
                }
            }
        }

        /// <summary>
        /// Sets and gets the Reservation property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ReservationWrapper Reservation
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

        public RoomWrapper Room
        {
            get
            {
                return _Room;
            }

            set
            {
                if (_Room == value)
                {
                    return;
                }

                _Room = value;
                RaisePropertyChanged();
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
                if (_RoomState != value)
                {
                    _RoomState = value;
                    switch (value)
                    {
                        case RoomStateEnum.NotAvailable:
                            CellColor = new SolidColorBrush(Colors.DarkGray);
                            Text = " ";
                            break;

                        case RoomStateEnum.Available:
                            if (RoomTypeEnm == RoomTypeEnum.Allotment)
                                CellColor = new SolidColorBrush(Colors.DeepSkyBlue);
                            else if (RoomTypeEnm == RoomTypeEnum.Booking)
                                CellColor = new SolidColorBrush(Colors.Blue);
                            else
                                CellColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#72e600");
                            Text = " ";
                            break;

                        case RoomStateEnum.MovableNoName:
                            CellColor = new SolidColorBrush(Colors.Yellow);
                            break;

                        case RoomStateEnum.NotMovableNoName:
                            CellColor = new SolidColorBrush(Colors.Orange);
                            break;

                        case RoomStateEnum.Booked:
                            if (Reservation != null && Reservation.Booking != null && Reservation.Booking.IsPartners && Reservation.Booking.Partner.Id == 219)
                                CellColor = new SolidColorBrush(Colors.Blue);
                            else
                                CellColor = new SolidColorBrush(Colors.Red);
                            break;

                        case RoomStateEnum.OverBookedByMistake:
                            CellColor = new SolidColorBrush(Colors.Pink);
                            break;

                        case RoomStateEnum.Allotment:
                            CellColor = new SolidColorBrush(Colors.DeepSkyBlue);
                            break;

                        case RoomStateEnum.Booking:
                            CellColor = new SolidColorBrush(Colors.Blue);
                            break;

                        default:
                            CellColor = new SolidColorBrush(Colors.White);
                            break;
                    }
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CellColor));
                }
            }
        }

        public string Text
        {
            get
            {
                return _Text;
            }

            set
            {
                if (_Text == value)
                {
                    return;
                }

                _Text = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}