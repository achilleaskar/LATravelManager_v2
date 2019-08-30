using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for Login_UserControl.xaml
    /// </summary>
    public partial class Login_UserControl : UserControl
    {
        public Login_UserControl()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Cast the 'sender' to a PasswordBox
            PasswordBox pBox = sender as PasswordBox;

            //Set this "EncryptedPassword" dependency property to the "SecurePassword"
            //of the PasswordBox.
            AttachedProperties.PasswordBoxMVVMAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            username.Focus();
        }

        private void SelectAllPassword(object sender, RoutedEventArgs e)
        {
            var pb = (sender as PasswordBox);
            if (pb != null)
                pb.SelectAll();
        }

        private void PasswordOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pb = (sender as PasswordBox);
            if (pb != null)
                if (!pb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    pb.Focus();
                }
        }
    }
}