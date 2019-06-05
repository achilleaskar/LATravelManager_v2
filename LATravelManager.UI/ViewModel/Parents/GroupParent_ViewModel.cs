using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LATravelManager.UI.ViewModel.Parents
{
    public class GroupParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public GroupParent_ViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            GroupExcursions = new ObservableCollection<ExcursionWrapper>();
            UpdateExcursionsCommand = new RelayCommand(Reload, CanUpdate);

            Tabs.Add(new MakeReservationTab { Index = Tabs.Count });
            Tabs.Add(new PlanTab { Index = Tabs.Count });
            // Tabs.Add(new SearchTab { Index = Tabs.Count });
            // Tabs.Add(new MoveTab { Index = Tabs.Count });
            // Tabs.Add(new ListManagementTab { Index = Tabs.Count });
            // Tabs.Add(new OptionalActivitiesTab { Index = Tabs.Count });
            LoadChildViewModels();
        }

        public bool Enable => SelectedExcursion != null || SelectedChildViewModel is GlobalSearch_ViewModel || SelectedChildViewModel is Info_ViewModel;

        public override void ChildChanged()
        {
            base.ChildChanged();
            RaisePropertyChanged(nameof(Enable));
        }

        private bool CanUpdate()
        {
            return BasicDataManager.IsContextAvailable;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            GroupExcursions = new ObservableCollection<ExcursionWrapper>(BasicDataManager.GroupExcursions.Where(e => e.ExcursionDates.Any(b => b.CheckOut > DateTime.Today)).Select(c => new ExcursionWrapper(c)));
        }

        public RelayCommand UpdateExcursionsCommand { get; set; }

        private ObservableCollection<ExcursionWrapper> _GroupExcursions;

        public ObservableCollection<ExcursionWrapper> GroupExcursions
        {
            get
            {
                return _GroupExcursions;
            }

            set
            {
                if (_GroupExcursions == value)
                {
                    return;
                }

                _GroupExcursions = value;
                RaisePropertyChanged();
            }
        }

        public void LoadChildViewModels()
        {
            Childs.Add(new NewReservation_Group_ViewModel(MainViewModel));
            Childs.Add(new Plan_ViewModel(this, MainViewModel));

            //Childs.Add(new Search_Group_ViewModel());
            //Childs.Add(new MoveReservation_Bansko_ViewModel());
            //Childs.Add(new Lists_Bansko_ViewModel());
            //Childs.Add(new OptionalActivities_Bansko_ViewModel());
        }

        public override void Reload()
        {
            Load();
            UpdateExcursionsCommand.RaiseCanExecuteChanged();
        }
    }
}