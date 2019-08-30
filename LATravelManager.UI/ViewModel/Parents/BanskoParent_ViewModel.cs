﻿using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System.Linq;

namespace LATravelManager.UI.ViewModel.Parents
{
    public class BanskoParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public BanskoParent_ViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Tabs.Add(new MakeReservationTab { Index = Tabs.Count });
            Tabs.Add(new PlanTab { Index = Tabs.Count });

            //Tabs.Add(new MoveTab { Index = Tabs.Count });
            //Tabs.Add(new ListManagementTab { Index = Tabs.Count });
            //Tabs.Add(new OptionalActivitiesTab { Index = Tabs.Count });
            LoadChildViewModels();
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            SelectedExcursion = BasicDataManager.Excursions.Where(e => e.Id == 2).Select(e => new ExcursionWrapper(e)).FirstOrDefault();
        }

        public void LoadChildViewModels()
        {
            Childs.Add(new NewReservation_Bansko_ViewModel(MainViewModel));
            Childs.Add(new Plan_ViewModel(this, MainViewModel));

            //Childs.Add(new Search_Bansko_ViewModel());
            //Childs.Add(new MoveReservation_Bansko_ViewModel());
            //Childs.Add(new Lists_Bansko_ViewModel());
            //Childs.Add(new OptionalActivities_Bansko_ViewModel());
        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }
    }
}