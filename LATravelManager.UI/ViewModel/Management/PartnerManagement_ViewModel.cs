using LaTravelManager.BaseTypes;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Management
{
    public class PartnerManagement_ViewModel : AddEditBase<PartnerWrapper, Partner>
    {
        public PartnerManagement_ViewModel(GenericRepository context) : base(context)
        {
            ControlName = "Διαχείριση Συνεργατών";
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                MainCollection = new ObservableCollection<PartnerWrapper>((await Context.GetAllAsyncSortedByName<Partner>()).Select(p => new PartnerWrapper(p)));
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