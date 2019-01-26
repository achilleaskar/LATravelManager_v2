namespace LATravelManager.UI.Message
{
    public class SelectedTabChangedMessage
    {
        #region constructors and destructors

        public SelectedTabChangedMessage(string tabName,bool isChild)
        {
            TabName = tabName;
            IsChild = isChild;
        }

        #endregion constructors and destructors

        #region properties

        public string TabName { get; }
        public bool IsChild { get; }

        #endregion properties
    }
}