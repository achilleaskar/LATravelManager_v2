using LaTravelManager.BaseTypes;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
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
        public HotelsManagement_ViewModel(GenericRepository context) : base(context)
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

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            try
            {
                if (Context.HasChanges())
                {
                    await Context.SaveAsync();
                }

                MessengerInstance.Send(new IsBusyChangedMessage(true));

                Cities = new ObservableCollection<City>(await Context.GetAllCitiesAsyncSortedByName());
                HotelCategories = new ObservableCollection<HotelCategory>(await Context.GetAllAsync<HotelCategory>());
                if (Context.HasChanges())
                {
                    await Context.SaveAsync();
                }
                MainCollection = new ObservableCollection<HotelWrapper>((await Context.GetAllAsyncSortedByName<Hotel>()).Select(c => new HotelWrapper(c)));
                if (Context.HasChanges())
                {
                    await Context.SaveAsync();
                }
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

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}