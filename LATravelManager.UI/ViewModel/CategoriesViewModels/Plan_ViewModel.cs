using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.Plan;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views.Bansko;
using LATravelManager.UI.Wrapper;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels
{
    public class Plan_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public Plan_ViewModel(ExcursionCategory_ViewModelBase parentExcursionCategory, MainViewModel mainViewModel)
        {
            ParentExcursionCategory = parentExcursionCategory;
            MainViewModel = mainViewModel;
            MonthsColors = new List<SolidColorBrush>();
            FilteredPlanList = new ObservableCollection<HotelWrapper>();
            Months = new ObservableCollection<Month>();
            From = DateTime.Today;
            To = DateTime.Today.AddDays(10);
            ShowPlanCommand = new RelayCommand(async () => { await ShowPlan(); });
            ViewReservationCommand = new RelayCommand<ReservationWrapper>(async (obj) => { await ViewReservation(obj); });
            ToggleDateCommand = new RelayCommand<DateTime>(ToggleDate);
            SelectedDatesAtPlan = new List<DateTime>();

            CancelRoomThisDayCommand = new RelayCommand<PlanDailyInfo>(async (obj) => { await CancelThisDay(obj); }, CanCancelThisDay);
            AddRoomThisDayCommand = new RelayCommand<PlanDailyInfo>(async (obj) => { await AddThisDay(obj); }, CanAddThisDay);
            MoveFromCommand = new RelayCommand<PlanDailyInfo>((obj) => { MoveFrom(obj); }, CanMoveFrom);
            MoveToCommand = new RelayCommand<PlanDailyInfo>(async (obj) => await MoveTo(obj));
            AddAllotmentRoomThisDayCommand = new RelayCommand<PlanDailyInfo>(async (obj) => { await AddAllotmentThisDay(obj); }, CanAddAllotmentRoomThisDay);
            AddBookingRoomThisDayCommand = new RelayCommand<PlanDailyInfo>(async (obj) => { await AddBookingThisDay(obj); }, CanAddBookingRoomThisDay);
            MessengerInstance.Register<SelectedExcursionChangedMessage>(this, exc => { SelectedExcursionChanged(exc.SelectedExcursion); });
        }

        private ReservationWrapper _TmpReservation;

        public ReservationWrapper TmpReservation
        {
            get
            {
                return _TmpReservation;
            }

            set
            {
                if (_TmpReservation == value)
                {
                    return;
                }

                _TmpReservation = value;
                RaisePropertyChanged();
            }
        }

        private PlanDailyInfo _TmpDailyInfo;

        public PlanDailyInfo TmpDailyInfo
        {
            get
            {
                return _TmpDailyInfo;
            }

            set
            {
                if (_TmpDailyInfo == value)
                {
                    return;
                }

                _TmpDailyInfo = value;
                RaisePropertyChanged();
            }
        }

        private async Task MoveTo(PlanDailyInfo obj)
        {
            if (obj == null || TmpReservation == null || TmpDailyInfo == null || !obj.Room.CanFit(TmpReservation) || !obj.Room.CanAddReservationToRoom(TmpReservation))
            {
                MessageBox.Show("Δεν Γίνεται");
                return;
            }
            obj.Room.MakeReservation(TmpReservation);
            TmpReservation.Model.Room = obj.Room.Model;
            await Context.SaveAsync();
            foreach (var d in TmpDailyInfo.Room.PlanDailyInfo.Where(t => t.Reservation != null && t.Reservation.Id == TmpReservation.Id))
            {
                d.Reservation = null;
                d.RoomState = RoomStateEnum.Available;
            }
        }

        private void MoveFrom(PlanDailyInfo obj)
        {
            TmpReservation = obj.Reservation;
            TmpDailyInfo = obj;
        }

        private bool CanMoveFrom(PlanDailyInfo arg)
        {
            if (arg != null && arg.Reservation != null && arg.Reservation.ReservationType == ReservationTypeEnum.Normal)
                return true;
            return false;
        }

        private void SelectedExcursionChanged(ExcursionWrapper selectedExcursion)
        {
            if (selectedExcursion != null && selectedExcursion.ExcursionType.Category == ExcursionTypeEnum.Group)
            {
                if (selectedExcursion != null && (selectedExcursion.Start > DateTime.Today || selectedExcursion.End < DateTime.Today))
                {
                    From = selectedExcursion.Start;
                    To = selectedExcursion.End;
                }
                else
                {
                    From = DateTime.Today;
                    To = DateTime.Today.AddDays(10);
                }
            }
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<HotelWrapper> _FilteredPlanList;

        private DateTime _From;

        private ObservableCollection<Hotel> _Hotels;

        private ObservableCollection<Month> _Months;

        private List<SolidColorBrush> _MonthsColors;

        private int _PeopleCount;

        private ObservableCollection<RoomType> _RoomTypes;

        private List<DateTime> _SelectedDatesAtPlan;

        private int _SelectedHotelIndex;

        private int _SelectedRoomTypeIndex;

        private DateTime _To;

        #endregion Fields

        #region Properties

        public RelayCommand<PlanDailyInfo> AddAllotmentRoomThisDayCommand { get; set; }
        public RelayCommand<PlanDailyInfo> AddBookingRoomThisDayCommand { get; set; }

        public RelayCommand<PlanDailyInfo> AddRoomThisDayCommand { get; set; }
        public RelayCommand<PlanDailyInfo> MoveFromCommand { get; set; }
        public RelayCommand<PlanDailyInfo> MoveToCommand { get; set; }

        public RelayCommand<PlanDailyInfo> CancelRoomThisDayCommand { get; set; }

        public GenericRepository Context
        {
            get;
            set;
        }

        public ObservableCollection<HotelWrapper> FilteredPlanList
        {
            get
            {
                return _FilteredPlanList;
            }

            set
            {
                if (_FilteredPlanList == value)
                {
                    return;
                }

                _FilteredPlanList = value;
                RaisePropertyChanged();
            }
        }

        public DateTime From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                _From = value;
                if (To <= From)
                    To = From.AddDays(5);
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Hotel> Hotels
        {
            get
            {
                return _Hotels;
            }

            set
            {
                if (_Hotels == value)
                {
                    return;
                }

                _Hotels = value;
                RaisePropertyChanged();
            }
        }

        public bool IsPlanVisible => FilteredPlanList.Count > 0;

        public MainViewModel MainViewModel { get; }

        public ObservableCollection<Month> Months
        {
            get
            {
                return _Months;
            }

            set
            {
                if (_Months == value)
                {
                    return;
                }

                _Months = value;
                RaisePropertyChanged();
            }
        }

        public List<SolidColorBrush> MonthsColors
        {
            get
            {
                return _MonthsColors;
            }

            set
            {
                if (_MonthsColors == value)
                {
                    return;
                }

                _MonthsColors = value;
                RaisePropertyChanged();
            }
        }

        public ExcursionCategory_ViewModelBase ParentExcursionCategory { get; }

        public int PeopleCount
        {
            get
            {
                return _PeopleCount;
            }

            set
            {
                if (_PeopleCount == value)
                {
                    return;
                }

                _PeopleCount = value;
                RaisePropertyChanged();
            }
        }

        public RoomsManager RoomsManager
        {
            get;
            set;
        }

        public ObservableCollection<RoomType> RoomTypes
        {
            get
            {
                return _RoomTypes;
            }

            set
            {
                if (_RoomTypes == value)
                {
                    return;
                }

                _RoomTypes = value;
                RaisePropertyChanged();
            }
        }

        public List<DateTime> SelectedDatesAtPlan
        {
            get { return _SelectedDatesAtPlan; }
            set
            {
                if (_SelectedDatesAtPlan != value)
                {
                    _SelectedDatesAtPlan = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int SelectedHotelIndex
        {
            get
            {
                return _SelectedHotelIndex;
            }

            set
            {
                if (_SelectedHotelIndex == value)
                {
                    return;
                }

                _SelectedHotelIndex = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedRoomTypeIndex
        {
            get
            {
                return _SelectedRoomTypeIndex;
            }

            set
            {
                if (_SelectedRoomTypeIndex == value)
                {
                    return;
                }

                _SelectedRoomTypeIndex = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowPlanCommand { get; set; }

        public DateTime To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }

                _To = value;
                if (To < From)
                {
                    From = To.AddDays(-5);
                }
                RaisePropertyChanged();
            }
        }

        public RelayCommand<DateTime> ToggleDateCommand { get; set; }

        public RelayCommand<ReservationWrapper> ViewReservationCommand { get; set; }

        #endregion Properties

        #region Methods

        public bool CanAddAllotmentRoomThisDay(PlanDailyInfo obj)
        {
            if (obj != null && obj.RoomState == RoomStateEnum.NotAvailable)
            {
                return true;
            }
            return false;
        }

        public bool CanAddBookingRoomThisDay(PlanDailyInfo obj)
        {
            if (obj != null && obj.RoomState == RoomStateEnum.NotAvailable)
            {
                return true;
            }
            return false;
        }

        public bool CanAddThisDay(PlanDailyInfo obj)
        {
            if (obj != null && obj.RoomState == RoomStateEnum.NotAvailable)
            {
                return true;
            }
            return false;
        }

        public bool CanCancelThisDay(PlanDailyInfo obj)
        {
            if (obj == null || obj.RoomState == RoomStateEnum.Booked || obj.RoomState == RoomStateEnum.NotAvailable || obj.RoomState == RoomStateEnum.NotMovableNoName)
            {
                return false;
            }
            return true;
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null, MyViewModelBase parent = null)
        {
            Hotels = new ObservableCollection<Hotel>(MainViewModel.BasicDataManager.Hotels.Where(h => h.City.Id == ParentExcursionCategory.SelectedExcursion.Destinations[0].Id));
            RoomTypes = MainViewModel.BasicDataManager.RoomTypes;
            SetColorsList();
            await Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        public void ToggleDate(DateTime selectedDate)
        {
            if (!SelectedDatesAtPlan.Contains(selectedDate))
            {
                if (SelectedDatesAtPlan.Count == 2)
                {
                    SelectedDatesAtPlan.RemoveAt(0);
                }
                SelectedDatesAtPlan.Add(selectedDate);
            }
            else
            {
                SelectedDatesAtPlan.Remove(selectedDate);
            }
            foreach (Month month in Months)
            {
                foreach (PlanDatetInfo day in month.Days)
                {
                    if (SelectedDatesAtPlan.Contains(day.Date))
                        day.IsSelected = true;
                    else
                        day.IsSelected = false;
                }
            }
            foreach (HotelWrapper hotel in FilteredPlanList)
            {
                foreach (RoomWrapper room in hotel.RoomWrappers)
                {
                    foreach (var bookinfo in room.PlanDailyInfo)
                    {
                        if (SelectedDatesAtPlan.Contains(bookinfo.Date))
                            bookinfo.IsDateSelected = true;
                        else
                            bookinfo.IsDateSelected = false;
                    }
                }
            }
        }

        internal void RoomClicked(RoomWrapper room)
        {
            foreach (HotelWrapper hotel in FilteredPlanList)
            {
                foreach (RoomWrapper r in hotel.RoomWrappers)
                {
                    foreach (PlanDailyInfo bookinfo in r.PlanDailyInfo)
                    {
                        bookinfo.IsDateSelected = false;
                    }
                }
            }
            foreach (PlanDailyInfo bookinfo in room.PlanDailyInfo)
            {
                bookinfo.IsDateSelected = true;
            }
        }

        private ObservableCollection<HotelWrapper> MergeRooms(ObservableCollection<HotelWrapper> filteredPlanList)
        {
            int counter = 0;
            var mergedList = new ObservableCollection<HotelWrapper>();
            BookingInfoPerDay vr;
            HotelWrapper tmpHotel;
            foreach (HotelWrapper hotel in filteredPlanList)
            {
                tmpHotel = new HotelWrapper { Name = hotel.Name, Id = hotel.Id };
                foreach (RoomWrapper room in hotel.RoomWrappers)
                {
                    if (!room.Handled)
                        foreach (RoomWrapper currentRoom in hotel.RoomWrappers)
                            if (room.Id != currentRoom.Id && !currentRoom.Handled && currentRoom.RoomType == room.RoomType &&
                                ((string.IsNullOrEmpty(room.Note) && string.IsNullOrEmpty(currentRoom.Note)) || room.Note == currentRoom.Note ||
                                (!string.IsNullOrEmpty(room.Note) && !string.IsNullOrEmpty(currentRoom.Note) && room.Note.Contains("eos") && currentRoom.Note.Contains("eos")))
                                && room.CanMerge(currentRoom.PlanDailyInfo))
                            {
                                for (var i = 0; i < currentRoom.PlanDailyInfo.Count; i++)
                                    if (currentRoom.PlanDailyInfo[i].RoomState != RoomStateEnum.NotAvailable)
                                    {
                                        room.PlanDailyInfo[i].RoomState = currentRoom.PlanDailyInfo[i].RoomState;
                                        room.PlanDailyInfo[i].DayState = currentRoom.PlanDailyInfo[i].DayState;
                                        room.PlanDailyInfo[i].CellColor = currentRoom.PlanDailyInfo[i].CellColor;
                                        room.PlanDailyInfo[i].Text = currentRoom.PlanDailyInfo[i].Text;
                                        room.PlanDailyInfo[i].Reservation = currentRoom.PlanDailyInfo[i].Reservation;
                                        room.PlanDailyInfo[i].Room = currentRoom.PlanDailyInfo[i].Room;
                                        room.PlanDailyInfo[i].Id = currentRoom.PlanDailyInfo[i].Id;

                                        // dailyInfos.Add();
                                        vr = currentRoom.DailyBookingInfo.Where(rt => rt.Date == room.PlanDailyInfo[i].Date).FirstOrDefault();
                                        currentRoom.DailyBookingInfo.Remove(vr);
                                        room.DailyBookingInfo.Add(vr);
                                        counter++;
                                        if (counter % 100 == 0)
                                        {
                                            Context.Save();
                                        }
                                        // room.DailyBookingInfo.Add(new BookingInfoPerDay { Date = currentRoom.PlanDailyInfo[i].Date });
                                        if (currentRoom.PlanDailyInfo[i].Reservation != null && currentRoom.PlanDailyInfo[i].Reservation.Room != null)
                                            currentRoom.PlanDailyInfo[i].Reservation.Room = room.Model;
                                        //roomstodelte.Add(currentRoom.Model);
                                    }
                                currentRoom.Handled = true;
                                // break;
                            }
                    //foreach (var item in dailyInfos)
                    //{
                    //    room.DailyBookingInfo.Add(item);
                    //}
                }

                List<DbEntityEntry> AddedEntities = Context.Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

                AddedEntities.ForEach(E =>
                {
                    if (E.CurrentValues.PropertyNames.Contains("CreatedDate"))
                    {
                        E.Property("CreatedDate").CurrentValue = DateTime.Now;
                    }
                });

                List<DbEntityEntry> EditedEntities = Context.Context.ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

                EditedEntities.ForEach(E =>
                {
                    if (E.OriginalValues.PropertyNames.Contains("ModifiedDate"))
                    {
                        //   E.Property("ModifiedDate").CurrentValue = DateTime.Now;
                    }
                });
                Context.Save();
                ////foreach (var item in roomstodelte)
                ////{
                ////}
                foreach (RoomWrapper room in hotel.RoomWrappers)
                    if (!room.Handled)
                        tmpHotel.RoomWrappers.Add(room);
                tmpHotel.RoomWrappers = tmpHotel.RoomWrappers.OrderBy(w => w.RoomType.Id).ToList();
                mergedList.Add(tmpHotel);
            }
            return mergedList;
        }

        private async Task AddAllotmentThisDay(PlanDailyInfo obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
              (await Context.GetRoomById(obj.Room.Id)).DailyBookingInfo.Add(new BookingInfoPerDay { Date = obj.Date, RoomTypeEnm = RoomTypeEnum.Allotment });
            await Context.SaveAsync();

            obj.RoomState = RoomStateEnum.Allotment;
        }

        private async Task AddBookingThisDay(PlanDailyInfo obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
              (await Context.GetRoomById(obj.Room.Id)).DailyBookingInfo.Add(new BookingInfoPerDay { Date = obj.Date, RoomTypeEnm = RoomTypeEnum.Booking });
            await Context.SaveAsync();

            obj.RoomState = RoomStateEnum.Booking;
        }

        private async Task AddThisDay(PlanDailyInfo obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            //foreach (var h in FilteredPlanList)
            //{
            //    if (h.Id==obj.Room.Hotel.Id)
            //    {
            //        foreach (var r in h.RoomWrappers)
            //        {
            //            if (r.DailyBookingInfo.Any(r1=>r1.Date==obj.Date.AddDays(1))&& !r.DailyBookingInfo.Any(r1 => r1.Date == obj.Date))
            //            {
            //                r.DailyBookingInfo.Add(new BookingInfoPerDay { Date = obj.Date });
            //            }
            //        }
            //    }

            //}

            (await Context.GetRoomById(obj.Room.Id)).DailyBookingInfo.Add(new BookingInfoPerDay { Date = obj.Date, RoomTypeEnm = RoomTypeEnum.Available });
            //   obj.Room.DailyBookingInfo.Add(new BookingInfoPerDay { Date = obj.Date, RoomTypeEnm = RoomTypeEnum.Available });
            await Context.SaveAsync();

            obj.RoomState = RoomStateEnum.Available;
        }

        private async Task CancelThisDay(PlanDailyInfo obj)
        {
            //foreach (var h in FilteredPlanList)
            //{
            //    foreach (var r in h.RoomWrappers)
            //    {
            //        if (r.DailyBookingInfo.Any(rt => rt.Date == obj.Date))
            //            r.DailyBookingInfo.Remove(r.DailyBookingInfo.Where(p => p.Date == obj.Date).FirstOrDefault());
            //    }
            //}
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj.Room.DailyBookingInfo.RemoveAll(x => x.Date == obj.Date) == 0)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Η ακύρωση απέτυχε."));
            }
            else
            {
                obj.RoomState = RoomStateEnum.NotAvailable;
                obj.Text = "";
                await Context.SaveAsync();
            }

            if (obj.RoomState == RoomStateEnum.MovableNoName)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Παρακαλώ ξαναπατήστε προβολή πλάνου"));
                obj.RoomState = RoomStateEnum.NotAvailable;
            }
        }

        private void SetColorsList()
        {
            _MonthsColors.Add(new SolidColorBrush(Colors.Aquamarine));
            _MonthsColors.Add(new SolidColorBrush(Colors.LightBlue));
            _MonthsColors.Add(new SolidColorBrush(Colors.Yellow));
            _MonthsColors.Add(new SolidColorBrush(Colors.LightPink));
            _MonthsColors.Add(new SolidColorBrush(Colors.Orange));
            _MonthsColors.Add(new SolidColorBrush(Colors.LightCoral));
            _MonthsColors.Add(new SolidColorBrush(Colors.LightSkyBlue));
            _MonthsColors.Add(new SolidColorBrush(Colors.Khaki));
            _MonthsColors.Add(new SolidColorBrush(Colors.Aqua));
        }

        private async Task ShowPlan()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Months.Clear();
            FilteredPlanList.Clear();
            Context = new GenericRepository();
            RoomsManager = new RoomsManager(Context);

            if (DateTime.DaysInMonth(From.Year, From.Month) - From.Day < 2)
            {
                From = From.AddDays(-1);
            }
            DateTime date = From;
            Months.Add(new Month { Name = date.ToString("MMMM"), Background = _MonthsColors[date.Month % 9] });
            var monthid = 0;
            for (var i = 0; i <= (To - From).TotalDays; i++)
            {
                if (date.Day == 1 && date != From)
                {
                    Months.Add(new Month { Name = date.ToString("MMMM"), Background = _MonthsColors[date.Month % 9] });
                    monthid++;
                }
                Months[monthid].Days.Add(new PlanDatetInfo { Date = date });

                date = date.AddDays(1);
            }

            FilteredPlanList = new ObservableCollection<HotelWrapper>(await RoomsManager.GetAllAvailableRooms(From, To.AddDays(1), ParentExcursionCategory.SelectedExcursion, true,
                selectedRoomType: ((SelectedRoomTypeIndex > 0) ? RoomTypes[SelectedRoomTypeIndex - 1] : null),
                selectedHotel: (SelectedHotelIndex > 0) ? Hotels[SelectedHotelIndex - 1] : null,
                PeopleCount));

            decimal total = 0, free = 0;

            ////List<BookingInfoPerDay> dbitd = new List<BookingInfoPerDay>();
            //foreach (var item in FilteredPlanList)
            //{
            //    foreach (var r in item.RoomWrappers)
            //    {
            //        foreach (var dbi in r.PlanDailyInfo)
            //            if (dbi.RoomState == RoomStateEnum.Available)
            //            {
            //                dbitd.Add(r.DailyBookingInfo.Where(d => d.Date == dbi.Date).FirstOrDefault());
            //            }
            //    }

            //}
            //for (int i = 0; i < dbitd.Count; i++)
            //{
            //    Context.Delete(dbitd[i]);
            //    if (i % 100 == 0 && Context.HasChanges())
            //    {
            //        await Context.SaveAsync();
            //    }
            //}
            //await Context.SaveAsync();

            FilteredPlanList = MergeRooms(FilteredPlanList);

            //List<BookingInfoPerDay> dbitd = new List<BookingInfoPerDay>();
            //foreach (var item in FilteredPlanList)
            //{
            //    if (item.Id == 72 || item.Id == 78 || item.Id == 88)
            //    {
            //        foreach (var r in item.RoomWrappers)
            //        {
            //            // if (r.RoomType.Id == 8)
            //            foreach (var dbi in r.PlanDailyInfo)
            //                if (dbi.RoomState == RoomStateEnum.Available)// && dbi.Date >= new DateTime(2019, 12, 31) && dbi.Date < new DateTime(2020, 01, 7))
            //                {
            //                    dbitd.Add(r.DailyBookingInfo.Where(d => d.Date == dbi.Date).FirstOrDefault());
            //                }
            //        }
            //    }
            //}
            //for (int i = 0; i < dbitd.Count; i++)
            //{
            //    Context.Delete(dbitd[i]);
            //    if (i % 100 == 0 && Context.HasChanges())
            //    {
            //        await Context.SaveAsync();
            //    }
            //}
            //await Context.SaveAsync();

            foreach (HotelWrapper h in FilteredPlanList)
            {
                foreach (var r in h.RoomWrappers)
                {
                    foreach (var day in r.PlanDailyInfo)
                    {
                        if (day.RoomState != RoomStateEnum.Allotment && day.RoomState != RoomStateEnum.NotAvailable && day.RoomState != RoomStateEnum.OverBookedByMistake)
                        {
                            total++;
                            if (day.RoomState != RoomStateEnum.Available)
                            {
                                free++;
                            }
                        }
                    }
                }
            }

            Fullness = total > 0 ? free / total : 0;

            RaisePropertyChanged(nameof(IsPlanVisible));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private decimal _Fullness;

        public decimal Fullness
        {
            get
            {
                return _Fullness;
            }

            set
            {
                if (_Fullness == value)
                {
                    return;
                }

                _Fullness = value;
                RaisePropertyChanged();
            }
        }

        private async Task ViewReservation(ReservationWrapper obj)
        {
            if (obj != null)
            {
                try
                {
                    if (StaticResources.User.BaseLocation == 1 || (obj.Booking.IsPartners && obj.Booking.Partner.Id != 219))
                    {
                        NewReservation_Group_ViewModel viewModel = new NewReservation_Group_ViewModel(MainViewModel);
                        await viewModel.LoadAsync(obj.Booking.Id);
                        MessengerInstance.Send(new OpenChildWindowCommand(new EditBooking_Bansko_Window(), viewModel));
                    }
                }
                catch (Exception ex)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                }
            }
        }

        #endregion Methods
    }
}