using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.Tabs;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Parents
{
    public class PersonalParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public PersonalParent_ViewModel(GenericRepository startingReposiroty, NavigationViewModel navigationViewModel) : base(startingReposiroty, navigationViewModel)
        {
            Tabs.Add(new MakeReservationTab { Index = Tabs.Count });
            Tabs.Add(new SearchTab { Index = Tabs.Count });
            LoadChildViewModels();
        }

        public override Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            return Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        public void LoadChildViewModels()
        {
            Childs.Add(new NewReservation_Personal_ViewModel());
            Childs.Add(new Search_Personal_ViewModel());
            //Childs.Add(new MoveReservation_Bansko_ViewModel());
            //Childs.Add(new Lists_Bansko_ViewModel());
            //Childs.Add(new OptionalActivities_Bansko_ViewModel());
        }
    }
}