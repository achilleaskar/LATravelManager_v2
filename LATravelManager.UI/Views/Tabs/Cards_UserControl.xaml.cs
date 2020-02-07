using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
