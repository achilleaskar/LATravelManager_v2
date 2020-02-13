using System.Windows.Controls;
using System.Windows.Input;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;

namespace LATravelManager.UI.Views.Tabs
{
    /// <summary>
    /// Interaction logic for Cards_UserControl.xaml
    /// </summary>
    public partial class Cards_UserControl : UserControl
    {
        public Cards_UserControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((Cards_ViewModel)DataContext).EditBookingCommand.Execute(null);
        }
    }
}