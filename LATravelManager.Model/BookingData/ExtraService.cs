using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.Model.BookingData
{
    public class ExtraService : BaseModel
    {



        private decimal _Amount;


        public decimal Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (_Amount == value)
                {
                    return;
                }

                _Amount = value;
                RaisePropertyChanged();
            }
        }




        private string _Description;


        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description == value)
                {
                    return;
                }

                _Description = value;
                RaisePropertyChanged();
            }
        }
    }
}
