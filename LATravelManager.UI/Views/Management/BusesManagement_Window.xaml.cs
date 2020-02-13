using System.Windows;
using LaTravelManager.ViewModel.Management;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for BusesManagement_Window.xaml
    /// </summary>
    public partial class BusesManagement_Window : Window
    {
        public BusesManagement_Window()
        {
            InitializeComponent();
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