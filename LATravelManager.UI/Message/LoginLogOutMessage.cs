using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.Message
{
    public class LoginLogOutMessage
    {
        public LoginLogOutMessage(bool login)
        {
            Login = login;
        }

        public bool Login { get; }
    }
}
