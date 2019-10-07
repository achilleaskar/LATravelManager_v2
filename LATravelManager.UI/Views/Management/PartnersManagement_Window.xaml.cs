using LaTravelManager.ViewModel.Management;
using LATravelManager.UI.ViewModel.Management;
using System.Windows;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for PartnersManagement_Window.xaml
    /// </summary>
    public partial class PartnersManagement_Window : Window
    {
        public PartnersManagement_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (DataContext is PartnerManagement_ViewModel u && u.BasicDataManager.Context.HasChanges())
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