using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Parents
{
    public class ThirdPartyParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public ThirdPartyParent_ViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Tabs.Add(new MakeReservationTab { Index = Tabs.Count });
            LoadChildViewModels();

        }
        public override void ChildChanged()
        {
            base.ChildChanged();
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
        }

        public void LoadChildViewModels()
        {
            Childs.Add(new NewReservation_ThirdParty_VIewModel(MainViewModel));
           // Childs.Add(new Plan_ViewModel(this, MainViewModel));
            //Childs.Add(new MoveReservation_Bansko_ViewModel());
            //Childs.Add(new Lists_Bansko_ViewModel());
            //Childs.Add(new OptionalActivities_Bansko_ViewModel());
        }

        public override void Reload()
        {
            Load();
        }
    }
}