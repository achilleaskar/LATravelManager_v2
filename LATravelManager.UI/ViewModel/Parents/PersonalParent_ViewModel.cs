using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Parents
{
    public class PersonalParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public PersonalParent_ViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Tabs.Add(new MakeReservationTab { Index = Tabs.Count });
            Tabs.Add(new SearchTab { Index = Tabs.Count });
            LoadChildViewModels();
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
        }

        public override void Reload()
        {
        }

        public void LoadChildViewModels()
        {
            Childs.Add(new NewReservation_Personal_ViewModel(MainViewModel));
            //Childs.Add(new Search_Personal_ViewModel());
            //Childs.Add(new MoveReservation_Bansko_ViewModel());
            //Childs.Add(new Lists_Bansko_ViewModel());
            //Childs.Add(new OptionalActivities_Bansko_ViewModel());
        }
    }
}