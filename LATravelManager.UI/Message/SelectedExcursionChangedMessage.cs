using LATravelManager.UI.Wrapper;

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
