using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views.Tabs
{
    /// <summary>
    /// Interaction logic for AddIncome_UserControl.xaml
    /// </summary>
    public partial class AddIncomeOutcome_UserControl : UserControl
    {
        public AddIncomeOutcome_UserControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((AddIncomeOutcome_ViewModel)DataContext).EditBookingCommand.Execute(null);
        }
    }
}