using LATravelManager.UI.ViewModel.Management;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for LoginDataManagement_Window.xaml
    /// </summary>
    public partial class LoginDataManagement_Window : Window
    {
        public LoginDataManagement_Window()
        {
            InitializeComponent();
        }

        private void DataGridCell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridCell dgc && dgc.Content is TextBlock tb)
            {
                Clipboard.SetText(tb.Text);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is LoginDataManagement_ViewModel u && u.Context.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                    u.Context.RollBack();
            }
            Helpers.StaticResources.Close(this);
        }
    }
}