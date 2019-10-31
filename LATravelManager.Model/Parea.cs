using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.Model
{
    public class Parea : BaseModel
    {

        public Parea()
        {
            Customers = new ObservableCollection<CustomerWrapper>();
        }

        private bool _done;


        public bool Done
        {
            get
            {
                return _done;
            }

            set
            {
                if (_done == value)
                {
                    return;
                }

                _done = value;
                RaisePropertyChanged();
            }
        }




        private int _Counter;


        public int Counter
        {
            get
            {
                return _Counter;
            }

            set
            {
                if (_Counter == value)
                {
                    return;
                }

                _Counter = value;
                RaisePropertyChanged();
            }
        }

        private int _bookingId;


        public int BookingId
        {
            get
            {
                return _bookingId;
            }

            set
            {
                if (_bookingId == value)
                {
                    return;
                }

                _bookingId = value;
                RaisePropertyChanged();
            }
        }



        private ObservableCollection<CustomerWrapper> _Customers;


        public ObservableCollection<CustomerWrapper> Customers
        {
            get
            {
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                RaisePropertyChanged();
            }
        }
    }
}
