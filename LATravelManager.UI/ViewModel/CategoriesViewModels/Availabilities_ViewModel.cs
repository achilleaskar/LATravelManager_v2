using GalaSoft.MvvmLight;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels
{
    public class Availabilities_ViewModel : ViewModelBase
    {
        #region Constructors

        public Availabilities_ViewModel()
        {
        }

        public Availabilities_ViewModel(ObservableCollection<HotelWrapper> availableHotels, DateTime checkIn, DateTime checkOut, RoomType SelectedRoomType, Hotel SelectedHotel)
        {
            AvailableHotels = availableHotels;
            CheckIn = checkIn;
            CheckOut = checkOut;
            this.SelectedRoomType = SelectedRoomType;
            this.SelectedHotel = SelectedHotel;
            daysCount = (CheckOut - CheckIn).Days;
            Pairs = new List<Pair>();
            Lists = new ObservableCollection<AvailabilitiesList>();
            SecondaryLists = new ObservableCollection<AvailabilitiesList>();
            SetUp(0, Lists);
            SetUp(1, SecondaryLists);
        }

        #endregion Constructors

        #region Fields

        public int daysCount;

        private ObservableCollection<AvailabilitiesList> _Lists;

        private ObservableCollection<AvailabilitiesList> _SecondaryLists;

        #endregion Fields

        #region Properties

        public ObservableCollection<HotelWrapper> AvailableHotels { get; }

        public DateTime CheckIn { get; }

        public DateTime CheckOut { get; }

        public ObservableCollection<AvailabilitiesList> Lists
        {
            get
            {
                return _Lists;
            }

            set
            {
                if (_Lists == value)
                {
                    return;
                }

                _Lists = value;
                RaisePropertyChanged();
            }
        }

        public List<Pair> Pairs { get; set; }

        public ObservableCollection<AvailabilitiesList> SecondaryLists
        {
            get
            {
                return _SecondaryLists;
            }

            set
            {
                if (_SecondaryLists == value)
                {
                    return;
                }

                _SecondaryLists = value;
                RaisePropertyChanged();
            }
        }

        public Hotel SelectedHotel { get; }

        public RoomType SelectedRoomType { get; }

        #endregion Properties

        #region Methods

        private void SetUp(int minus, ObservableCollection<AvailabilitiesList> lists)
        {
            int currentcount;
            if (daysCount - minus >= 1)
            {
                Pairs.Clear();
                DateTime tmpCheckin = CheckIn;
                DateTime tmpCheckOut = CheckOut;
                int counter = 0;
                while (counter < 2)
                {
                    tmpCheckin = tmpCheckin.AddDays(-1);
                    if (tmpCheckin < DateTime.Today || (CheckIn - tmpCheckin).Days > 15)
                    {
                        break;
                    }
                    tmpCheckOut = tmpCheckin.AddDays(daysCount - minus);
                    if ((tmpCheckin.DayOfWeek == DayOfWeek.Wednesday || tmpCheckin.DayOfWeek == DayOfWeek.Thursday || tmpCheckin.DayOfWeek == DayOfWeek.Saturday || tmpCheckin.DayOfWeek == DayOfWeek.Sunday) &&
                        (tmpCheckOut.DayOfWeek == DayOfWeek.Wednesday || tmpCheckOut.DayOfWeek == DayOfWeek.Thursday || tmpCheckOut.DayOfWeek == DayOfWeek.Saturday || tmpCheckOut.DayOfWeek == DayOfWeek.Sunday))
                    {
                        Pairs.Insert(0, new Pair(tmpCheckin, tmpCheckin.AddDays(daysCount - minus)));
                        counter++;
                    }
                }
                if (minus == 0)
                    Pairs.Add(new Pair(CheckIn, CheckOut));
                counter = 0;
                tmpCheckin = CheckIn;
                while (counter < 2)
                {
                    tmpCheckin = tmpCheckin.AddDays(1);
                    if (tmpCheckin < DateTime.Today || (tmpCheckin - CheckIn).Days > 15)
                    {
                        break;
                    }
                    tmpCheckOut = tmpCheckin.AddDays(daysCount - minus);
                    if ((tmpCheckin.DayOfWeek == DayOfWeek.Wednesday || tmpCheckin.DayOfWeek == DayOfWeek.Thursday || tmpCheckin.DayOfWeek == DayOfWeek.Saturday || tmpCheckin.DayOfWeek == DayOfWeek.Sunday) &&
                        (tmpCheckOut.DayOfWeek == DayOfWeek.Wednesday || tmpCheckOut.DayOfWeek == DayOfWeek.Thursday || tmpCheckOut.DayOfWeek == DayOfWeek.Saturday || tmpCheckOut.DayOfWeek == DayOfWeek.Sunday))
                    {
                        Pairs.Add(new Pair(tmpCheckin, tmpCheckin.AddDays(daysCount - minus)));
                        counter++;
                    }
                }

                List<RoomWrapper> tmplist;

                foreach (var pair in Pairs)
                {
                    IDictionary<int, int> RoomTYpesLeft = new Dictionary<int, int>();

                    tmplist = new List<RoomWrapper>();
                    int allotmentDays = 0;
                    bool addThis, isfree;

                    foreach (HotelWrapper hotel in AvailableHotels)
                    {
                        foreach (RoomWrapper room in hotel.RoomWrappers)
                        {
                            allotmentDays = 0;
                            isfree = addThis = true;

                            foreach (PlanDailyInfo pi in room.PlanDailyInfo)
                            {
                                if (pi.Date < pair.CheckIn)
                                {
                                    continue;
                                }
                                if (!addThis || pi.Date >= pair.CheckOut)
                                {
                                    break;
                                }
                                addThis &= pi.RoomState == RoomStateEnum.Available
                                    || pi.RoomState == RoomStateEnum.MovableNoName;
                                //|| pi.RoomState == RoomStateEnum.Allotment;
                                isfree &= pi.RoomState == RoomStateEnum.Available;

                                //if (room.PlanDailyInfo[i].IsAllotment)
                                //{
                                //    allotmentDays++;
                                //}
                            }
                            if (isfree)
                            {
                                if (RoomTYpesLeft.ContainsKey(room.RoomType.Id))
                                {
                                    RoomTYpesLeft.TryGetValue(room.RoomType.Id, out currentcount);
                                    RoomTYpesLeft[room.RoomType.Id] = currentcount + 1;
                                }
                                else
                                {
                                    RoomTYpesLeft.Add(room.RoomType.Id, 1);
                                }
                            }
                            addThis &= (SelectedHotel == null || hotel.Id == SelectedHotel.Id) && (SelectedRoomType == null || room.RoomType.Id == SelectedRoomType.Id);

                            //todelete
                            if (addThis)
                            {
                                if (allotmentDays > 0)
                                {
                                    if (room.PlanDailyInfo.Count != allotmentDays)
                                    {
                                        //room.IsAllotment = true;
                                        room.LocalNote = "Allotment" + ((allotmentDays == 1) ? " η 1 μέρα" : (" οι " + allotmentDays + " ημέρες."));
                                    }
                                    else
                                    {
                                        // room.IsAllotment = true;
                                        room.LocalNote = "Allotment";
                                    }
                                }
                                else
                                {
                                    // room.IsAllotment = false;
                                    room.LocalNote = "";
                                }

                                tmplist.Add(room);
                            }
                        }
                    }
                    lists.Add(new AvailabilitiesList { Dates = pair, IsMain = (pair.CheckIn == CheckIn && pair.CheckOut == CheckOut), Rooms = new ObservableCollection<RoomWrapper>(tmplist.OrderBy(f => f.RoomType.MinCapacity).ToList()),RoomTYpesLeft=RoomTYpesLeft });
                }
            }
        }

        #endregion Methods
    }

    public class AvailabilitiesList: ViewModelBase
    {


        #region Fields

        private ObservableCollection<RoomWrapper> _Rooms;

        #endregion Fields

        #region Properties







        private IDictionary<int, int> _RoomTYpesLeft;


        public IDictionary<int, int> RoomTYpesLeft
        {
            get
            {
                return _RoomTYpesLeft;
            }

            set
            {
                if (_RoomTYpesLeft == value)
                {
                    return;
                }

                _RoomTYpesLeft = value;
                RaisePropertyChanged();
            }
        }

        IDictionary<int, string> dict = new Dictionary<int, string>();
        public Pair Dates { get; set; }
        public bool IsMain { get; set; }

        public ObservableCollection<RoomWrapper> Rooms
        {
            get
            {
                return _Rooms;
            }

            set
            {
                if (_Rooms == value)
                {
                    return;
                }

                _Rooms = value;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Rooms);
                view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(RoomType)));
            }
        }

        #endregion Properties
    }

    public class Pair
    {
        #region Constructors

        public Pair(DateTime checkIn, DateTime checkOut)
        {
            CheckIn = checkIn;
            CheckOut = checkOut;
        }

        #endregion Constructors

        #region Properties

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{CheckIn.ToString("ddd dd/MM - ")}{CheckOut.ToString("ddd dd/MM")}({((CheckOut - CheckIn).TotalDays)} νύχτες)";
        }

        #endregion Methods
    }
}