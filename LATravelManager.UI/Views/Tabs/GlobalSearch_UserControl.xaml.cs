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
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((GlobalSearch_ViewModel)DataContext).EditBookingCommand.Execute(null);
        }
    }
}