using LATravelManager.UI.Helpers;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using System.Windows.Controls;

namespace LATravelManager.UI.Views.Tabs
{
    /// <summary>
    /// Interaction logic for GlobalSearch_UserControl.xaml
    /// </summary>
    public partial class GlobalSearch_UserControl : UserControl
    {
        public GlobalSearch_UserControl()
        {
            InitializeComponent();
            datagrid.Columns[0].Visibility = StaticResources.User.Level <= 2 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((GlobalSearch_ViewModel)DataContext).EditBookingCommand.Execute(null);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            datagrid.Columns[13].Visibility = System.Windows.Visibility.Visible;
            datagrid.Columns[14].Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            datagrid.Columns[13].Visibility = System.Windows.Visibility.Collapsed;
            datagrid.Columns[14].Visibility = System.Windows.Visibility.Collapsed;

        }
    }
}