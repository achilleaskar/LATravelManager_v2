using System.Windows;
using LATravelManager.UI.ViewModel.Management;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for LeadersManagement_Widnow.xaml
    /// </summary>
    public partial class LeadersManagement_Widnow : Window
    {
        public LeadersManagement_Widnow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is LeadersManagement_ViewModel u && u.BasicDataManager.Context.HasChanges())
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