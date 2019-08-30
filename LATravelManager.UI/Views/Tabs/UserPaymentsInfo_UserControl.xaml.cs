using LATravelManager.UI.Helpers;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views.Tabs
{
    /// <summary>
    /// Interaction logic for UserPaymentsInfo_UserControl.xaml
    /// </summary>
    public partial class UserPaymentsInfo_UserControl : UserControl
    {
        public UserPaymentsInfo_UserControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((PaymentInfo)DataContext).OpenBookingCommand.Execute(null);
        }
    }
}