using System.Windows;
using LaTravelManager.ViewModel.Management;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for HotelsManagement_Window.xaml
    /// </summary>
    public partial class HotelsManagement_Window : Window
    {
        public HotelsManagement_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is HotelsManagement_ViewModel u && u.BasicDataManager.Context.HasChanges())
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