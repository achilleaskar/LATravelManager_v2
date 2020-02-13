using System.Collections.Generic;

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