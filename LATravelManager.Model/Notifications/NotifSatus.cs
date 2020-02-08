using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LATravelManager.Model.People;

namespace LATravelManager.Model.Notifications
{
    public class NotifSatus : BaseModel
    {






        private User _OkByUser;


        public User OkByUser
        {
            get
            {
                return _OkByUser;
            }

            set
            {
                if (_OkByUser == value)
                {
                    return;
                }

                _OkByUser = value;
                RaisePropertyChanged();
            }
        }




        private DateTime _OkDate;


        public DateTime OkDate
        {
            get
            {
                return _OkDate;
            }

            set
            {
                if (_OkDate == value)
                {
                    return;
                }

                _OkDate = value;
                RaisePropertyChanged();
            }
        }




        private bool _IsOk;


        public bool IsOk
        {
            get
            {
                return _IsOk;
            }

            set
            {
                if (_IsOk == value)
                {
                    return;
                }

                _IsOk = value;
                RaisePropertyChanged();
            }
        }
    }
}
