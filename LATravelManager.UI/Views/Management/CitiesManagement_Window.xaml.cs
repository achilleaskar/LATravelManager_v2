using LATravelManager.UI.ViewModel.Management;
using System.Windows;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for CitiesManagementWindow.xaml
    /// </summary>
    public partial class CitiesManagement_Window : Window
    {
        public CitiesManagement_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is CitiesManagement_ViewModel u && u.BasicDataManager.Context.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                    u.BasicDataManager.Context.RollBack();
            }
            Helpers.StaticResources.Close(this);
        }
    }
}