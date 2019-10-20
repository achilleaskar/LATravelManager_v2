using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using System.Windows;
using System.Windows.Controls;

namespace LATravelManager.UI.Views.Tabs
{
    /// <summary>
    /// Interaction logic for Remaining_UserControl.xaml
    /// </summary>
    public partial class Remaining_UserControl : UserControl
    {
        public Remaining_UserControl()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((Remaining_ViewModel)DataContext).EditBookingCommand.Execute(null);
        }
    }
}