using LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views.Bansko
{
    /// <summary>
    /// Interaction logic for Search_Bansko_UserControl.xaml
    /// </summary>
    public partial class Search_Bansko_UserControl : UserControl
    {
        public Search_Bansko_UserControl()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((Search_Bansko_ViewModel)DataContext).EditBookingCommand.Execute(null);
        }
    }
}