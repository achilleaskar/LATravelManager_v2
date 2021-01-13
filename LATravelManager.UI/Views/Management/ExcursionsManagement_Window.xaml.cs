using LATravelManager.UI.ViewModel.Management;
using System.Windows;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for ExcursionsManagement_Window.xaml
    /// </summary>
    public partial class ExcursionsManagement_Window : Window
    {
        public ExcursionsManagement_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ExcursionsManagement_ViewModel u && u.BasicDataManager.Context.HasChanges())
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