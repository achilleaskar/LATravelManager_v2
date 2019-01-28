using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.Message
{
   public class DeselectAllOtherTabsMessage
    {
        public DeselectAllOtherTabsMessage(string newTab)
        {
            NewTab = newTab;
        }

        public string NewTab { get; }
    }
}
