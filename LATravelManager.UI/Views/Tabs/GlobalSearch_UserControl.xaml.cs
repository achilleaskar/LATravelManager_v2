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
    }
}