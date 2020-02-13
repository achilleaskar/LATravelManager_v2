using System.Windows.Controls;
using System.Windows.Input;
using LATravelManager.Model.Notifications;
using LATravelManager.UI.ViewModel.Window_ViewModels;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for Main_UserControl.xaml
    /// </summary>
    public partial class Main_UserControl : UserControl
    {
        public Main_UserControl()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is TabControl & DataContext is MainUserControl_ViewModel)
            {
                (DataContext as MainUserControl_ViewModel).SelectedTemplateChangedCommand.Execute(null);
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var t = sender.GetType();
            if (DataContext is MainUserControl_ViewModel mucvm
                && sender is ListViewItem lvi
                && lvi.DataContext is Notification n
                && n.ReservationWrapper != null)
            {
                mucvm.SelectedReservation = n.ReservationWrapper;
                mucvm.EditBookingCommand.Execute(null);
            }
        }
    }
}