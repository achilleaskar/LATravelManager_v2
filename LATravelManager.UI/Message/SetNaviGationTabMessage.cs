namespace LATravelManager.UI.Message
{
    public class SetNaviGationTabMessage
    {
        #region constructors and destructors

        public SetNaviGationTabMessage(int tabIndex)
        {
            TabIndex = tabIndex;
        }

        #endregion constructors and destructors

        #region properties

        /// <summary>
        /// Just some text that comes from the sender.
        /// </summary>
        public int TabIndex { get; private set; }

        #endregion properties
    }
}