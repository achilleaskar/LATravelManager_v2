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
