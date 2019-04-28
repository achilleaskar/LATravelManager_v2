using GalaSoft.MvvmLight.Messaging;
using LATravelManager.UI.Message;
using System.Windows;

namespace LATravelManager.UI
{
    public class MessageListener
    {
        #region Constructors

        public MessageListener()
        {
            InitMessenger();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// We need this property so that this type can be put into the ressources.
        /// </summary>
        public bool BindableProperty => true;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Is called by the constructor to define the messages we are interested in.
        /// </summary>
        private void InitMessenger()
        {
            // Hook to the message that states that some caller wants to open a ChildWindow.
            Messenger.Default.Register<ShowExceptionMessage_Message>(
                this, msg =>
                {
                    MessageBox.Show(msg.Message);
                });
            Messenger.Default.Register<OpenChildWindowCommand>(
                this,
                msg =>
                {
                    Window window = msg.Window;
                    if (msg.ViewModel != null)
                        window.DataContext = msg.ViewModel;
                    Application.Current.MainWindow.Visibility = Visibility.Hidden;
                    window.ShowDialog();
                    Application.Current.MainWindow.Visibility = Visibility.Visible;
                });
        }

        #endregion Methods
    }
}