using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Parents
{
   public class GroupParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public GroupParent_ViewModel()
        {
            Tabs.Add(new MakeReservationTab { Index = Tabs.Count });
            Tabs.Add(new SearchTab { Index = Tabs.Count });
            Tabs.Add(new MoveTab { Index = Tabs.Count });
            Tabs.Add(new ListManagementTab { Index = Tabs.Count });
            Tabs.Add(new OptionalActivitiesTab { Index = Tabs.Count });
            LoadChildViewModels();
        }


        public void LoadChildViewModels()
        {
            Childs.Add(new NewReservation_Group_ViewModel());
            Childs.Add(new Search_Group_ViewModel());
            //Childs.Add(new MoveReservation_Bansko_ViewModel());
            //Childs.Add(new Lists_Bansko_ViewModel());
            //Childs.Add(new OptionalActivities_Bansko_ViewModel());
        }
    }
}