using LATravelManager.Model.People;
using LATravelManager.UI.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for CustomerSearchWindow.xaml
    /// </summary>
    public partial class CustomerSearchWindow : Window
    {
        public CustomerSearchWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CustomerSearch_ViewModel dt && sender is DataGridRow dg && dg.DataContext is Customer c)
            {
                dt.AddCustomerToBooking(c);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Helpers.StaticResources.Close(this);
        }
    }
}
