using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using System.Windows.Controls;

namespace LATravelManager.UI.Views.Group
{
    /// <summary>
    /// Interaction logic for Search_Group_UserControl.xaml
    /// </summary>
    public partial class Search_Group_UserControl : UserControl
    {
        public Search_Group_UserControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((Search_Group_ViewModel)DataContext).EditBookingCommand.Execute(null);
        }

        private void ListViewSub_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }
    }
}