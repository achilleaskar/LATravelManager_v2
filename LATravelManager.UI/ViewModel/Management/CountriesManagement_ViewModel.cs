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
    public class CountriesManagement_ViewModel : AddEditBase<CountryWrapper, Country>
    {
        public CountriesManagement_ViewModel(GenericRepository context) : base(context)
        {
            ControlName = "Διαχείριση Χωρών";
        }

        public ObservableCollection<string> Continents { get; set; } = new ObservableCollection<string>(Definitions.Continents);

        public override async Task LoadAsync(int id = 0)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                
                MainCollection = new ObservableCollection<CountryWrapper>((await Context.GetAllAsyncSortedByName<Country>()).Select(c => new CountryWrapper(c)));
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