namespace LATravelManager.UI.Message
{
    public class ChangeViewModelMessage
    {
        #region constructors and destructors

        public ChangeViewModelMessage(string nameOfViewModel)
        {
            NameOfViewModel = nameOfViewModel;
        }

        #endregion constructors and destructors

        #region properties

        /// <summary>
        /// Just some text that comes from the sender.
        /// </summary>
        public string NameOfViewModel { get; private set; }

        #endregion properties
    }
}