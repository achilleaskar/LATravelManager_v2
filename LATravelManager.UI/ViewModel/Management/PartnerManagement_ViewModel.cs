using LaTravelManager.BaseTypes;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LATravelManager.UI.ViewModel.Management
{
    public class PartnerManagement_ViewModel : AddEditBase<PartnerWrapper, Partner>
    {
        public PartnerManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Συνεργατών";
        }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                MainCollection = new ObservableCollection<PartnerWrapper>((BasicDataManager.Partners).Select(p => new PartnerWrapper(p)));
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