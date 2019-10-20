using LaTravelManager.BaseTypes;
using LATravelManager.Model.Lists;
using LATravelManager.Model.Locations;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Management
{
    public class VehiclesManagement_viewModel : AddEditBase<VehicleWrapper, Vehicle>
    {
        public VehiclesManagement_viewModel(BasicDataManager basicDataManager) : base(basicDataManager)
        {
            ControlName = "Διαχείριση Λεωφορείων";
        }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<VehicleWrapper>(BasicDataManager.Vehicles.Select(v => new VehicleWrapper(v)));
        }
    }
}
