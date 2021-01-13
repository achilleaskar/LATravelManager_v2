using System.Windows;
using LATravelManager.UI.ViewModel.Management;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for OptionalExcursionsManagement_Window.xaml
    /// </summary>
    public partial class OptionalExcursionsManagement_Window : Window
    {
        public OptionalExcursionsManagement_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is OptionalExcursions_Management_ViewModel u && u.BasicDataManager.Context.HasChanges())
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