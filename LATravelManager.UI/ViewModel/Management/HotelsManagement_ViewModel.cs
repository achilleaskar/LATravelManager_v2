using LaTravelManager.BaseTypes;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LaTravelManager.ViewModel.Management
{
    public class HotelsManagement_ViewModel : AddEditBase<HotelWrapper, Hotel>
    {
        public HotelsManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Ξενοδοχείων";
        }

        private ObservableCollection<City> _Cities;

        private ObservableCollection<HotelCategory> _HotelCategories;

        /// <summary>
        /// Sets and gets the Cities property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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

        /// <summary>
        /// Sets and gets the HotelCategories property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<HotelCategory> HotelCategories
        {
            get
            {
                return _HotelCategories;
            }

            set
            {
                if (_HotelCategories == value)
                {
                    return;
                }

                _HotelCategories = value;
                RaisePropertyChanged();
            }
        }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {

                MessengerInstance.Send(new IsBusyChangedMessage(true));

                Cities = new ObservableCollection<City>(BasicDataManager.Context.GetAllSortedByName<City>());
                HotelCategories = BasicDataManager.HotelCategories;
                foreach (Hotel item in BasicDataManager.Hotels)
                {
                    if (Cities.Contains(item.City))
                    {

                    }
                }
                MainCollection = new ObservableCollection<HotelWrapper>(BasicDataManager.Hotels.Select(c => new HotelWrapper(c)));

            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
            }
        }
    }
}