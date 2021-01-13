using LATravelManager.Model.Locations;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System.Collections.ObjectModel;
using System.Linq;

namespace LATravelManager.UI.ViewModel.Management
{
    public class CitiesManagement_ViewModel : AddEditBase<CityWrapper, City>
    {
        public CitiesManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Πόλεων";
            Countries = new ObservableCollection<Country>();
        }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<CityWrapper>(BasicDataManager.Cities.Select(c => new CityWrapper(c)));
            Countries = new ObservableCollection<Country>(BasicDataManager.Countries);
        }

        private ObservableCollection<Country> _Countries;

        /// <summary>
        /// Sets and gets the Countries property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<Country> Countries
        {
            get
            {
                return _Countries;
            }

            set
            {
                if (_Countries == value)
                {
                    return;
                }

                _Countries = value;
                RaisePropertyChanged();
            }
        }
    }
}