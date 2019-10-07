using GalaSoft.MvvmLight.CommandWpf;
using LaTravelManager.ViewModel.Management;
using LATravelManager.Model;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Plan;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class AddRooms_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public AddRooms_ViewModel(MainViewModel mainViewModel)
        {
            try
            {
                RoomUnderEdit = new RoomWrapper();
                Rooms = new ObservableCollection<RoomWrapper>();
                GenericRepository = new GenericRepository();

                ShowRoomsCommnad = new RelayCommand(async () => { await ShowRooms(); }, CanShowRooms);
                SelectedDatesChangedCommand = new RelayCommand<Calendar>(SelectedDatesChanged);
                SaveRoomsCommand = new RelayCommand(async () => { await SaveRooms(); }, CanSaveRooms);
                DeleteRoomCommand = new RelayCommand(async () => { await DeleteRoom(); }, CanDeleteRoom);

                OpenHotelEditCommand = new RelayCommand(OpenHotelsWindow);
                CheckIn = DateTime.Today;
                Option.Date = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }

            MainViewModel = mainViewModel;
        }




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

        private void OpenHotelsWindow()
        {
            HotelsManagement_ViewModel vm = new HotelsManagement_ViewModel(MainViewModel.BasicDataManager);
            vm.ReLoad();
            MessengerInstance.Send(new OpenChildWindowCommand(new HotelsManagement_Window { DataContext = vm }));
            Hotels = new ObservableCollection<Hotel>(MainViewModel.BasicDataManager.Hotels);
        }

        #endregion Constructors

        #region Fields

        private ICollectionView _HotelsCV;

        public ICollectionView HotelsCV
        {
            get
            {
                return _HotelsCV;
            }

            set
            {
                if (_HotelsCV == value)
                {
                    return;
                }

                _HotelsCV = value;
                RaisePropertyChanged();
            }
        }

        private Calendar _calendar;
        private DateTime _CheckIn;
        private DateTime _CheckOut;
        private ObservableCollection<City> _Cities;
        private bool _DatesFilter;

        private bool _HasOption = false;

        private ObservableCollection<Hotel> _Hotels;

        private bool _IsAllotment = false;

        private Option _Option = new Option();

        private string _OutPut = string.Empty;

        private int _Quantity;

        private ObservableCollection<RoomWrapper> _Rooms;

        private ObservableCollection<RoomType> _RoomTypes;

        private RoomWrapper _RoomUnderEdit;

        private City _SelectedCity;

        private int _SelectedCityIndex;

        private List<DateTime> _SelectedDates = new List<DateTime>();

        private string _SelectedDatesString = string.Empty;

        private int _SelectedHotelIndex;

        private RoomWrapper _SelectedRoom;

        private int _SelectedRoomTypeIndex;

        #endregion Fields

        #region Properties

        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }
                _CheckIn = value;
                if (CheckOut < CheckIn)
                {
                    CheckOut = CheckIn.AddDays(3);
                }

                RaisePropertyChanged();
            }
        }

        public DateTime CheckOut
        {
            get
            {
                return _CheckOut;
            }

            set
            {
                if (_CheckOut == value)
                {
                    return;
                }
                _CheckOut = value;
                if (CheckIn > CheckOut)
                {
                    CheckIn = CheckOut.AddDays(-3);
                }

                RaisePropertyChanged();
            }
        }

        public ObservableCollection<City> Cities
        {
            get
            {
                return _Cities;
            }

            set
            {
                if (_Cities == value)
                {
                    return;
                }

                _Cities = value;
                RaisePropertyChanged();
            }
        }

        public bool DatesFilter
        {
            get
            {
                return _DatesFilter;
            }

            set
            {
                if (_DatesFilter == value)
                {
                    return;
                }

                _DatesFilter = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand DeleteRoomCommand { get; set; }
        public RelayCommand OpenHotelEditCommand { get; }

        public bool HasOption
        {
            get
            {
                return _HasOption;
            }

            set
            {
                if (_HasOption == value)
                {
                    return;
                }

                _HasOption = value;
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
                HotelsCV = (CollectionView)CollectionViewSource.GetDefaultView(Hotels);
                if (HotelsCV.Filter == null)
                {
                    HotelsCV.Filter = HotelsFilter;
                }
                RaisePropertyChanged();
            }
        }

        private bool HotelsFilter(object obj)
        {
            return obj != null && obj is Hotel h && SelectedCity != null && SelectedCity.Id == h.City.Id;
        }

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
                RaisePropertyChanged();
            }
        }

        public MainViewModel MainViewModel { get; }

        public Option Option
        {
            get
            {
                return _Option;
            }

            set
            {
                if (_Option == value)
                {
                    return;
                }

                _Option = value;
                RaisePropertyChanged();
            }
        }

        public string OutPut
        {
            get
            {
                return _OutPut;
            }

            set
            {
                if (_OutPut == value)
                {
                    return;
                }

                _OutPut = value;
                RaisePropertyChanged();
            }
        }

        public int Quantity
        {
            get
            {
                return _Quantity;
            }

            set
            {
                if (_Quantity == value)
                {
                    return;
                }

                _Quantity = value;
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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the RoomTypes property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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

        public RoomWrapper RoomUnderEdit
        {
            get
            {
                return _RoomUnderEdit;
            }

            set
            {
                if (_RoomUnderEdit == value)
                {
                    return;
                }

                _RoomUnderEdit = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveRoomsCommand { get; }

        public City SelectedCity
        {
            get
            {
                return _SelectedCity;
            }

            set
            {
                if (_SelectedCity == value)
                {
                    return;
                }
                _SelectedCity = value;
                HotelsCV.Refresh();
                RaisePropertyChanged();
            }
        }

        public int SelectedCityIndex
        {
            get
            {
                return _SelectedCityIndex;
            }

            set
            {
                if (_SelectedCityIndex == value)
                {
                    return;
                }
                _SelectedCityIndex = value;

                RaisePropertyChanged();
            }
        }

        public List<DateTime> SelectedDates
        {
            get
            {
                return _SelectedDates;
            }

            set
            {
                if (_SelectedDates == value)
                {
                    return;
                }
                OutPut = null;
                _SelectedDates = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<Calendar> SelectedDatesChangedCommand { get; }

        public string SelectedDatesString
        {
            get
            {
                return _SelectedDatesString;
            }

            set
            {
                if (_SelectedDatesString == value)
                {
                    return;
                }

                _SelectedDatesString = value;
                RaisePropertyChanged();
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

        public RoomWrapper SelectedRoom
        {
            get
            {
                return _SelectedRoom;
            }

            set
            {
                if (_SelectedRoom == value)
                {
                    return;
                }

                _SelectedRoom = value;
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

        public RelayCommand ShowRoomsCommnad { get; set; }

        #endregion Properties

        private GenericRepository _GenericRepository;

        public GenericRepository GenericRepository
        {
            get
            {
                return _GenericRepository;
            }

            set
            {
                if (_GenericRepository == value)
                {
                    return;
                }

                _GenericRepository = value;
                RaisePropertyChanged();
            }
        }

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            RoomTypes = MainViewModel.BasicDataManager.RoomTypes;
            Cities = MainViewModel.BasicDataManager.Cities;
            Hotels = new ObservableCollection<Hotel>(MainViewModel.BasicDataManager.Hotels);
            await Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        public void SelectedDatesChanged(Calendar c)
        {
            _calendar = c;
            SelectedDates = c.SelectedDates.ToList();
            SelectedDates.Sort();

            SelectedDatesString = "";
            if (SelectedDates.Count > 0)
            {
                var started = false;
                var builder = new StringBuilder();
                builder.Append(SelectedDatesString);
                int i;
                for (i = 0; i < SelectedDates.Count - 1; i++)
                {
                    if (!started)
                    {
                        builder.Append(SelectedDates[i].ToString("dd/MM"));
                        if (SelectedDates[i].AddDays(1) == SelectedDates[i + 1])
                        {
                            started = true;
                            builder.Append("-");
                        }
                        else
                            builder.Append(", ");
                    }
                    else
                    {
                        if (SelectedDates[i].AddDays(1) != SelectedDates[i + 1])
                        {
                            builder.Append(SelectedDates[i].ToString("dd/MM") + ", ");
                            started = false;
                        }
                    }
                }
                builder.Append(SelectedDates[i].ToString("dd/MM"));
                SelectedDatesString = builder.ToString();

                RaisePropertyChanged(nameof(SelectedDatesString));
            }
        }

        private bool CanDeleteRoom()
        {
            return SelectedRoom != null;
        }

        private bool CanSaveRooms()
        {
            if (Quantity <= 0 || SelectedDates.Count == 0)
            {
                return false;
            }
            if (IsInDesignMode)
                return true;
            return !RoomUnderEdit.HasErrors;
        }

        private bool CanShowRooms()
        {
            return CheckIn <= CheckOut && SelectedCityIndex >= 0 && SelectedHotelIndex >= 0 && SelectedRoomTypeIndex >= 0;
        }

        private async Task DeleteRoom()
        {
            List<RoomWrapper> RoomsToDelete = new List<RoomWrapper>();
            try
            {
                foreach (RoomWrapper room in Rooms)
                {
                    if (room.IsSelected)
                    {
                        GenericRepository.Delete(room.Model);
                        RoomsToDelete.Add(room);
                    }
                }
                await GenericRepository.SaveAsync();
                foreach (var r in RoomsToDelete)
                {
                    Rooms.Remove(r);
                }
                CountRooms();
            }
            catch (DbUpdateException)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message("Το δωμάτιο δεν μπορεί να διαγραφεί γιατι περιέχει κράτηση"));
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }




        private string _RoomsCounter;


        public string RoomsCounter
        {
            get
            {
                return _RoomsCounter;
            }

            set
            {
                if (_RoomsCounter == value)
                {
                    return;
                }

                _RoomsCounter = value;
                RaisePropertyChanged();
            }
        }

        private async Task SaveRooms()
        {
            Room tmproom;
            try
            {
                for (int i = 0; i < Quantity; i++)
                {
                    tmproom = new Room
                    {
                        DailyBookingInfo = new List<BookingInfoPerDay>(),
                        Hotel = await GenericRepository.GetByIdAsync<Hotel>(RoomUnderEdit.Hotel.Id),
                        Options = new ObservableCollection<Option>(),
                        Note = RoomUnderEdit.Note,
                        RoomType = await GenericRepository.GetByIdAsync<RoomType>(RoomUnderEdit.RoomType.Id)
                    };
                    if (HasOption)
                        tmproom.Options.Add(new Option { Date = Option.Date, Note = Option.Note });
                    if (MinimumStay > 7 || MinimumStay > 0)
                    {

                    }
                    for (int j = 0; j < SelectedDates.Count - 1; j++)
                    {
                        tmproom.DailyBookingInfo.Add(new BookingInfoPerDay { Date = SelectedDates[j], MinimunStay = MinimumStay, RoomTypeEnm = IsAllotment ? RoomTypeEnum.Allotment : RoomTypeEnum.Available });
                    }
                    GenericRepository.Add(tmproom);
                }
                await GenericRepository.SaveAsync();
                OutPut = "Rooms Saved";
                IsAllotment = false;
                RoomUnderEdit = new RoomWrapper { Hotel = RoomUnderEdit.Hotel };
                Quantity = 0;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        public void CountRooms()
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<RoomType, int> dict = new Dictionary<RoomType, int>();
            int count;
            if (Rooms != null)
                foreach (var rw in Rooms)
                {
                    count = 0;
                    dict.TryGetValue(rw.RoomType, out count);
                    dict[rw.RoomType] = count + 1;
                }

            if (dict.Count() > 0)
            {

                foreach (KeyValuePair<RoomType, int> entry in dict)
                {
                    sb.Append(entry.Key.Name);
                    sb.Append(": ");
                    sb.Append(entry.Value);
                    sb.Append(", ");
                }
            }
            RoomsCounter = sb.ToString().TrimEnd(' ').TrimEnd(',');
        }
        private async Task ShowRooms()
        {
            GenericRepository = new GenericRepository();
            Mouse.OverrideCursor = Cursors.AppStarting;
            Rooms.Clear();
            int cityId = -1, hotelId = -1, roomtypeId = -1;
            if (SelectedCityIndex > 0)
                cityId = Cities[SelectedCityIndex - 1].Id;
            if (SelectedHotelIndex > 0)
                hotelId = Hotels[SelectedHotelIndex - 1].Id;
            if (SelectedRoomTypeIndex > 0)
                roomtypeId = RoomTypes[SelectedRoomTypeIndex - 1].Id;

            ObservableCollection<RoomWrapper> tmpList = new ObservableCollection<RoomWrapper>((await GenericRepository.GetAllRoomsFiltered(cityId, hotelId, roomtypeId)).Select(x => new RoomWrapper(x)));

            //foreach (var room in tmpList)
            //{
            //    if ((room.Id >= 3105 && room.Id <= 3124) || (room.Id >= 3145 && room.Id <= 3156))
            //    {
            //        GenericRepository.Add(new Option { Date = new DateTime(2019, 10, 21), Room = room.Model });
            //    }
            //}
            //await GenericRepository.SaveAsync();
            if (DatesFilter)
            {
                foreach (RoomWrapper room in tmpList)
                    if (room.IsAvailable(CheckIn, CheckOut))
                        Rooms.Add(room);
            }
            else
                Rooms = tmpList;
            CountRooms();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }
}