using LaTravelManager.BaseTypes;
using LATravelManager.Model.Excursions;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Management
{
    public class OptionalExcursions_Management_ViewModel : AddEditBase<OptionalExcursionsWrapper, OptionalExcursion>
    {
        public OptionalExcursions_Management_ViewModel(BasicDataManager basicDataManager) : base(basicDataManager)
        {
            ControlName = "Διαχείριση Προαιρετικών εκδρομών";
        }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<OptionalExcursionsWrapper>(BasicDataManager.OptionalExcursions.Select(c => new OptionalExcursionsWrapper(c)));
            Excursions = new ObservableCollection<Excursion>(BasicDataManager.Excursions.Where(e => e.ExcursionDates.Any(d => d.CheckOut >= DateTime.Today) &&e.Id>0));
        }







        private ObservableCollection<Excursion> _Excursions;


        public ObservableCollection<Excursion> Excursions
        {
            get
            {
                return _Excursions;
            }

            set
            {
                if (_Excursions == value)
                {
                    return;
                }

                _Excursions = value;
                RaisePropertyChanged();
            }
        }
    }
}
