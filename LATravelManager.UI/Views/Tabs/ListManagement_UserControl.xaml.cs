using LATravelManager.Model.Lists;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views.Tabs
{
    /// <summary>
    /// Interaction logic for ListManagement_UserControl.xaml
    /// </summary>
    public partial class ListManagement_UserControl : UserControl
    {
        public ListManagement_UserControl()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var d = sender as DataGrid;
            if (d.SelectedItem is Counter c)
                c.Selected = !c.Selected;
            if (d.SelectedItem is CustomerWrapper cu)
                cu.Selected = !cu.Selected;
        }

        private void b1SetColor(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridCell dc)
            {
                if (dc.DataContext is Counter c)
                    c.Selected = !c.Selected;
                else if (dc.DataContext is CustomerWrapper cu)
                    cu.Selected = !cu.Selected;
            }
        }

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(sender is RadioButton radioButton) || !(radioButton.DataContext is Seat seat))
            {
                return;
            }
            ((ListManagement_ViewModel)DataContext).StartSeat = seat.Number;
            // int intIndex = Convert.ToInt32(radioButton.Content.ToString());
        }

        private void RadioButtonRet_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(sender is RadioButton radioButton) || !(radioButton.DataContext is Seat seat))
            {
                return;
            }
            ((ListManagement_ViewModel)DataContext).StartSeatRet = seat.Number;
            // int intIndex = Convert.ToInt32(radioButton.Content.ToString());
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            datagrid.Columns[5].Visibility = System.Windows.Visibility.Collapsed;
            datagrid.Columns[6].Visibility = System.Windows.Visibility.Visible;
            datagrid.Columns[7].Visibility = System.Windows.Visibility.Collapsed;
            datagrid.Columns[8].Visibility = System.Windows.Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            datagrid.Columns[5].Visibility = System.Windows.Visibility.Visible;
            datagrid.Columns[6].Visibility = System.Windows.Visibility.Collapsed;
            datagrid.Columns[7].Visibility = System.Windows.Visibility.Visible;
            datagrid.Columns[8].Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}