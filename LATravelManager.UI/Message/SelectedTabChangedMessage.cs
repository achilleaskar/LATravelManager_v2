namespace LATravelManager.UI.Message
{
    public class SelectedTabChangedMessage
    {
        #region constructors and destructors

        public SelectedTabChangedMessage(string tabName, bool isChild, int index)
        {
            TabName = tabName;
            IsChild = isChild;
            Index = index;
        }

        #endregion constructors and destructors

        #region properties

        public string TabName { get; }
        public bool IsChild { get; }
        public int Index { get; }

        #endregion properties
    }
}