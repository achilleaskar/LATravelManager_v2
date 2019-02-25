using LATravelManager.Models;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.Wrapper;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Parents
{
    public class BanskoParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public BanskoParent_ViewModel(GenericRepository startingReposiroty) : base(startingReposiroty)
        {
            Tabs.Add(new MakeReservationTab { Index = Tabs.Count });
            Tabs.Add(new SearchTab { Index = Tabs.Count });
            Tabs.Add(new MoveTab { Index = Tabs.Count });
            Tabs.Add(new ListManagementTab { Index = Tabs.Count });
            Tabs.Add(new OptionalActivitiesTab { Index = Tabs.Count });
            LoadChildViewModels();
        }

        public override async Task LoadAsync(int id = 0)
        {
            SelectedExcursion = new ExcursionWrapper( await StartingReposiroty.GetByIdAsync<Excursion>(2));
        }

      

        public void LoadChildViewModels()
        {
            Childs.Add(new NewReservation_Bansko_ViewModel());
            Childs.Add(new Search_Bansko_ViewModel());
            //Childs.Add(new MoveReservation_Bansko_ViewModel());
            //Childs.Add(new Lists_Bansko_ViewModel());
            //Childs.Add(new OptionalActivities_Bansko_ViewModel());
        }

        public override Task ReloadAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}