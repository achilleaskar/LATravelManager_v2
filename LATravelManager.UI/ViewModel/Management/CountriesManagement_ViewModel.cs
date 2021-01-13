using LATravelManager.Model.Locations;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LATravelManager.UI.ViewModel.Management
{
    public class CountriesManagement_ViewModel : AddEditBase<CountryWrapper, Country>
    {
        public CountriesManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Χωρών";
        }

        public ObservableCollection<string> Continents { get; set; } = new ObservableCollection<string>(Definitions.Continents);

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                MainCollection = new ObservableCollection<CountryWrapper>(BasicDataManager.Countries.Select(c => new CountryWrapper(c)));
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