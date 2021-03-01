using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using LATravelManager.Model.Wrapper;
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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is Cards_ViewModel svm && svm.SelectedReservation != null)
            {

                reservations.ScrollIntoView(svm.SelectedReservation);

                //reservations.SelectedItem = reservations.Items.GetItemAt(reservations.SelectedIndex);
                //reservations.ScrollIntoView(reservations.SelectedItem);
                //ListViewItem item = reservations.ItemContainerGenerator.ContainerFromItem(reservations.SelectedItem) as ListViewItem;
                //item.Focus();
                // reservations.ScrollIntoView(reservations.SelectedItem);
            }
        }

        private void BulkPayments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}