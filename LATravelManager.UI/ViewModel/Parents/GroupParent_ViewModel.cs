using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using LATravelManager.UI.ViewModel.Tabs;
using LATravelManager.UI.Wrapper;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Parents
{
    public class GroupParent_ViewModel : ExcursionCategory_ViewModelBase
    {
        public GroupParent_ViewModel(GenericRepository startingReposiroty) : base(startingReposiroty)
        {
            GroupExcursions = new ObservableCollection<ExcursionWrapper>();
            UpdateExcursionsCommand = new RelayCommand(async () => { await ReloadAsync(); }, CanUpdate);

            Tabs.Add(new MakeReservationTab { Index = Tabs.Count });
            Tabs.Add(new SearchTab { Index = Tabs.Count });
           // Tabs.Add(new MoveTab { Index = Tabs.Count });
           // Tabs.Add(new ListManagementTab { Index = Tabs.Count });
           // Tabs.Add(new OptionalActivitiesTab { Index = Tabs.Count });
            LoadChildViewModels();
            SelectedExcursionIndex = -1;
        }

        public bool Enable => SelectedExcursion != null;

        private bool CanUpdate()
        {
            return StartingReposiroty.IsContextAvailable;
        }

        public override async Task LoadAsync(int id = 0)
        {
            GroupExcursions = new ObservableCollection<ExcursionWrapper>((await StartingReposiroty.GetAllUpcomingGroupExcursionsAsync()).OrderBy(ex => ex, new ExcursionComparer()).Select(c => new ExcursionWrapper(c)));
        }

        public RelayCommand UpdateExcursionsCommand { get; set; }

        private int _SelectedExcursionIndex;

        public int SelectedExcursionIndex
        {
            get
            {
                return _SelectedExcursionIndex;
            }

            set
            {
                if (_SelectedExcursionIndex == value)
                {
                    return;
                }

                _SelectedExcursionIndex = value;
                SelectedExcursion = (value <= GroupExcursions.Count && value >= 0) ? GroupExcursions[SelectedExcursionIndex] : null;
                RaisePropertyChanged();
            }
        }

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
            Childs.Add(new NewReservation_Group_ViewModel());
            Childs.Add(new Search_Group_ViewModel());
            //Childs.Add(new MoveReservation_Bansko_ViewModel());
            //Childs.Add(new Lists_Bansko_ViewModel());
            //Childs.Add(new OptionalActivities_Bansko_ViewModel());
        }

        public override async Task ReloadAsync()
        {
            StartingReposiroty = new GenericRepository();
            GroupExcursions = new ObservableCollection<ExcursionWrapper>((await StartingReposiroty.GetAllUpcomingGroupExcursionsAsync()).OrderBy(ex => ex, new ExcursionComparer()).Select(c => new ExcursionWrapper(c)));
            UpdateExcursionsCommand.RaiseCanExecuteChanged();
        }
    }
}