namespace LATravelManager.UI.Message
{
    public class ChangeChildViewModelMessage
    {

        public ChangeChildViewModelMessage(int ViewModelindex)
        {
            this.ViewModelindex = ViewModelindex;
        }

        public int ViewModelindex { get; }
    }
}