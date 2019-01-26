namespace LATravelManager.UI.Message
{
    public class ShowExceptionMessage_Message
    {
        #region constructors and destructors

        public ShowExceptionMessage_Message(string message)
        {
            Message = message;
        }

        #endregion constructors and destructors

        #region properties

        /// <summary>
        /// Just some text that comes from the sender.
        /// </summary>
        public string Message { get; private set; }

        #endregion properties
    }
}