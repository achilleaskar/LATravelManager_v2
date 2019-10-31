using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;

namespace LATravelManager.Model
{
    public class Thesi : BaseModel
    {







        private CustomerWrapper _customer;


        public CustomerWrapper Customer
        {
            get
            {
                return _customer;
            }

            set
            {
                if (_customer == value)
                {
                    return;
                }

                _customer = value;
                RaisePropertyChanged();
            }


        }
    }
}