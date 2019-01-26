namespace LATravelManager.UI.Message
{
    public class ChangeVisibilityMessage
    {
        public bool Visible { get; private set; }

        public ChangeVisibilityMessage(bool visible)
        {
            Visible = visible;
        }
    }
}