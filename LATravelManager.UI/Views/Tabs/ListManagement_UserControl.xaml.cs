using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
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
    }
}