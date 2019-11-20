using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.Message
{
    public class NotsChangedMessage
    {
        public NotsChangedMessage(List<string> nots)
        {
            Nots = nots;
        }

        public List<string> Nots { get; }
    }
}
