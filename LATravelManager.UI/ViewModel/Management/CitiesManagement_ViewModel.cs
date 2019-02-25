using LaTravelManager.BaseTypes;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LaTravelManager.ViewModel.Management
{
    public class CitiesManagement_ViewModel : AddEditBase<CityWrapper, City>
    {
        public CitiesManagement_ViewModel(GenericRepository context) : base(context)
        {
            ControlName = "Διαχείριση Πόλεων";
            Countries = new ObservableCollection<Country>();
        }

        public override async Task LoadAsync(int id = 0)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                

                MainCollection = new ObservableCollection<CityWrapper>((await Context.GetAllCitiesAsyncSortedByName()).Select(c => new CityWrapper(c)));
                Countries = new ObservableCollection<Country>(await Context.GetAllAsyncSortedByName<Country>());

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

        private ObservableCollection<Country> _Countries;

        /// <summary>
        /// Sets and gets the Countries property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<Country> Countries
        {
            get
            {
                return _Countries;
            }

            set
            {
                if (_Countries == value)
                {
                    return;
                }

                _Countries = value;
                RaisePropertyChanged();
            }
        }
    }
}