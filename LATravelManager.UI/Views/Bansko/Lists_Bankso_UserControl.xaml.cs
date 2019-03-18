using LATravelManager.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views.Bansko
{
    /// <summary>
    /// Interaction logic for Lists_Bankso_UserControl.xaml
    /// </summary>
    public partial class Lists_Bankso_UserControl : UserControl
    {
        public Lists_Bankso_UserControl()
        {
            InitializeComponent();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                ScrollViewer scv = (ScrollViewer)sender;
                scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
                e.Handled = true;
        }

        private void CustomersDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        //    if (e.OriginalSource is TextBlock tb )
        //    {
        //        //if (tb.DataContext is Reservation r)
        //        //{
        //        //   // r.SelectAll();
        //        //}
        //    }
        //    else if (e.OriginalSource is Border b)
        //    {
        //        //if (b.DataContext is Reservation r)
        //        //{
        //        //  //  r.SelectAll();
        //        //}
        //    }
            e.Handled = true;
        }

        private void DataGrid_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
