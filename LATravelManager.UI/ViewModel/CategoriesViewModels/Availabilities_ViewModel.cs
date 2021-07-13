using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using LATravelManager.Model;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;

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
                    if ((tmpCheckin.DayOfWeek == DayOfWeek.Wednesday || tmpCheckin.DayOfWeek == DayOfWeek.Thursday || tmpCheckin.DayOfWeek == DayOfWeek.Sunday || tmpCheckin.DayOfWeek == DayOfWeek.Monday) &&
                        (tmpCheckOut.DayOfWeek == DayOfWeek.Wednesday || tmpCheckOut.DayOfWeek == DayOfWeek.Thursday || tmpCheckOut.DayOfWeek == DayOfWeek.Sunday || tmpCheckOut.DayOfWeek == DayOfWeek.Monday))
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
                    if ((tmpCheckin.DayOfWeek == DayOfWeek.Wednesday || tmpCheckin.DayOfWeek == DayOfWeek.Thursday || tmpCheckin.DayOfWeek == DayOfWeek.Sunday || tmpCheckin.DayOfWeek == DayOfWeek.Monday) &&
                        (tmpCheckOut.DayOfWeek == DayOfWeek.Wednesday || tmpCheckOut.DayOfWeek == DayOfWeek.Thursday || tmpCheckOut.DayOfWeek == DayOfWeek.Sunday || tmpCheckOut.DayOfWeek == DayOfWeek.Monday))
                    {
                        Pairs.Add(new Pair(tmpCheckin, tmpCheckin.AddDays(daysCount - minus)));
                        counter++;
                    }
                }

                foreach (var pair in Pairs)
                {
                    List<HotelWithRooms> tmplist = new List<HotelWithRooms>();
                    HotelWithRooms hotelwr;
                    int allotmentDays = 0;
                    bool addThis, isfree;

                    foreach (HotelWrapper hotel in AvailableHotels)
                    {
                        hotelwr = new HotelWithRooms { Rooms = new List<RoomWrapper>() };

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
                                addThis &= (pi.RoomState == RoomStateEnum.Available || pi.RoomState == RoomStateEnum.MovableNoName ||
                                    pi.RoomState == RoomStateEnum.Allotment) && ((pair.CheckOut - pair.CheckIn).TotalDays - 1 >= pi.MinimumStay);

                                isfree &= pi.RoomState == RoomStateEnum.Available;

                                if (pi.RoomState == RoomStateEnum.Allotment)
                                {
                                    allotmentDays++;
                                }
                            }

                            if (isfree)
                            {
                                if (hotel.NoName)
                                    room.RoomType.freeRooms++;
                                else
                                    room.RoomType.Named++;
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
                                        room.LocalNote = "Allotment" + ((allotmentDays == 1) ? " η 1 μέρα" : (" οι " + allotmentDays + " ημέρες"));
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

                                hotelwr.Rooms.Add(room);
                                ValidateRoom(room, pair);
                            }
                        }
                        if (hotelwr.Rooms.Count > 0)
                        {
                            tmplist.Add(hotelwr);
                        }
                    }

                    List<RoomWrapper> rooms = new List<RoomWrapper>();

                    foreach (var item in tmplist)
                    {
                        foreach (var room in item.Rooms)
                        {
                            if (!string.IsNullOrEmpty(room.LocalNote))
                            {
                                rooms.Add(room);
                            }
                            else if (!room.Hotel.NoName || (rooms.Where(r => r.Hotel == room.Hotel && r.RoomType == room.RoomType).Count() < room.RoomType.freeRooms))
                            {
                                rooms.Add(room);
                            }
                        }
                    }

                    lists.Add(new AvailabilitiesList { Dates = pair, IsMain = pair.CheckIn == CheckIn && pair.CheckOut == CheckOut, Rooms = new ObservableCollection<RoomWrapper>(rooms.OrderBy(f => f.Hotel.Name).ThenBy(t => t.RoomType.MaxCapacity).ThenByDescending(r => r.Rating).ToList()) });
                }
                foreach (var item in lists)
                {
                    item.SetUp();
                }
            }
        }

        private void ValidateRoom(RoomWrapper room, Pair pair)
        {
            //if (!string.IsNullOrEmpty(room.LocalNote))
            //{
            //    room.Rating=0;
            //    return;
            //}
            var before = room.PlanDailyInfo.FirstOrDefault(d => d.Date == pair.CheckIn.AddDays(-1));
            var after = room.PlanDailyInfo.FirstOrDefault(d => d.Date == pair.CheckOut);
            if (before == null || before.RoomState != RoomStateEnum.Available)
            {
            }
            else if (before.RoomState == RoomStateEnum.Available)
            {
                before = room.PlanDailyInfo.FirstOrDefault(d => d.Date == pair.CheckIn.AddDays(-2));
                if (before == null || before.RoomState != RoomStateEnum.Available)
                {
                }
            }
            if (after == null || after.RoomState != RoomStateEnum.Available)
            {
            }
            else if (after.RoomState == RoomStateEnum.Available)
            {
                after = room.PlanDailyInfo.FirstOrDefault(d => d.Date == pair.CheckOut.AddDays(1));
                if (after == null || after.RoomState != RoomStateEnum.Available)
                {
                }
            }
            room.Rating = ((before != null && before.RoomState != RoomStateEnum.Available) ? 1 : 0) + ((after != null && after.RoomState != RoomStateEnum.Available) ? 1 : 0);
        }

        #endregion Methods
    }

    public class AvailabilitiesList : ViewModelBase
    {
        #region Fields

        private ObservableCollection<RoomWrapper> _Rooms;

        #endregion Fields

        public void SetUp()
        {
            RoomsCv = (CollectionViewSource.GetDefaultView(Rooms));
            RoomsCv.GroupDescriptions.Add(new PropertyGroupDescription("Hotel"));
            RoomsCv.GroupDescriptions.Add(new PropertyGroupDescription("RoomType"));
        }

        #region Properties

        private readonly IDictionary<int, string> dict = new Dictionary<int, string>();
        public Pair Dates { get; set; }
        public bool IsMain { get; set; }

        private ICollectionView _RoomsCv;

        public ICollectionView RoomsCv
        {
            get
            {
                return _RoomsCv;
            }

            set
            {
                if (_RoomsCv == value)
                {
                    return;
                }

                _RoomsCv = value;
                RaisePropertyChanged();
            }
        }

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
            return $"{CheckIn:ddd dd/MM -} {CheckOut:ddd dd/MM}({(CheckOut - CheckIn).TotalDays} νύχτες)";
        }

        #endregion Methods
    }
}