using LaTravelManager.BaseTypes;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace LATravelManager.UI.ViewModel.Management
{
    public class LeadersManagement_ViewModel : AddEditBase<LeaderWrapper, Leader>
    {
        public LeadersManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Αρχηγών";
        }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<LeaderWrapper>(BasicDataManager.Leaders.Select(c => new LeaderWrapper(c)));
        }
    }
}