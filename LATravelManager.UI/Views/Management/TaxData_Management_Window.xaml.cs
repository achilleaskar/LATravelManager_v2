using LATravelManager.UI.ViewModel.Management;
using System.Windows;
using System.Windows.Controls;

namespace LATravelManager.UI.Views.Management
{
    /// <summary>
    /// Interaction logic for TaxData_Management_Window.xaml
    /// </summary>
    public partial class TaxData_Management_Window : Window
    {
        public TaxData_Management_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is TaxDataManagement_ViewModel u && u.BasicDataManager.Context.HasChanges())
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is TaxDataManagement_ViewModel u)
            {
                u.UpdateCities();
            }
        }

        private void FilterableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (sender is FilterableComboBox cb && cb.SelectedItem is Company c && cb.SelectedValue != null && c.ToString() != cb.SelectedValue.ToString())
            //{
            //    e.Handled=true;
            //}
        }
    }
}