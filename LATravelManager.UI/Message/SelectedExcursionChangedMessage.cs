using LATravelManager.Models;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.Message
{
   public class SelectedExcursionChangedMessage
    {
        public SelectedExcursionChangedMessage(ExcursionWrapper selectedExcursion)
        {
            SelectedExcursion = selectedExcursion;
        }

        public ExcursionWrapper SelectedExcursion { get; }
    }
}
