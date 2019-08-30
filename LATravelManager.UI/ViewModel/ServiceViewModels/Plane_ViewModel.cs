using LATravelManager.Model;
using LATravelManager.Model.Services;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace LATravelManager.UI.ViewModel.ServiceViewModels
{
    public class Plane_ViewModel : ServiceViewModel
    {
        public Plane_ViewModel(NewReservation_Personal_ViewModel parent) : base(parent)
        {
            Service = new PlaneService();
            string[] logFile = File.ReadAllLines(@"Sources\airports.txt");
            Airports = new List<string>(logFile);
            AirLines = Parent.BasicDataManager.Airlines;
            Refresh();
        }

        public override void Refresh()
        {
            AirLines = Parent.BasicDataManager.Airlines;
        }

        private IEnumerable<string> _Airports;
        private ObservableCollection<Airline> _AirLines;

        public IEnumerable<string> Airports
        {
            get
            {
                return _Airports;
            }

            set
            {
                if (_Airports == value)
                {
                    return;
                }

                _Airports = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Airline> AirLines
        {
            get
            {
                return _AirLines;
            }

            set
            {
                if (_AirLines == value)
                {
                    return;
                }

                _AirLines = value;
                RaisePropertyChanged();
            }
        }
    }
}