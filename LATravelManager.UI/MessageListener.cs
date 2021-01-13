using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using LATravelManager.UI.Message;

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
            //msg =>
            //{
            //    Window window = msg.Window;
            //    if (msg.ViewModel != null)
            //        window.DataContext = msg.ViewModel;
            //    Application.Current.MainWindow.Visibility = Visibility.Hidden;
            //    window.ShowDialog();
            //    Application.Current.MainWindow.Visibility = Visibility.Visible;
            //});


            msg =>
            {
                Window window = msg.Window;
                Helpers.StaticResources.Windows.Add(msg.Window);
                if (msg.ViewModel != null)
                    window.DataContext = msg.ViewModel;
                if (Helpers.StaticResources.VisibleWindows.Count == 0)
                {
                    Application.Current.MainWindow.Visibility = Visibility.Hidden;
                }
                else
                {
                    Helpers.StaticResources.VisibleWindows.Last().Hide();
                }
                window.Show();
                //Helpers.StaticResources.Windows.Remove(msg.Window);
                //if (Helpers.StaticResources.HiddenWindows.Count == 0)
                //{
                //    Application.Current.MainWindow.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    Helpers.StaticResources.HiddenWindows.Last().Show();
                //}
                //window.ShowDialog();
                //Application.Current.MainWindow.Visibility = Visibility.Visible;
            });



        }

        #endregion Methods
    }
}