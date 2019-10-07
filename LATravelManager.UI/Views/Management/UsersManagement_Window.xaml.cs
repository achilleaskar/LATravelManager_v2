using LaTravelManager.ViewModel.Management;
using System.Windows;
using System.Windows.Controls;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for UsersManagement_Window.xaml
    /// </summary>
    public partial class UsersManagement_Window : Window
    {
        public UsersManagement_Window()
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (DataContext is UsersManagement_viewModel u && u.BasicDataManager.Context.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη απόθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                    u.BasicDataManager.Context.RollBack();
            }
        }
    }
}