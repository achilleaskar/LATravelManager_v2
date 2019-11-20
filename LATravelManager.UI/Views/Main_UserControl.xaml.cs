using LATravelManager.UI.ViewModel.Window_ViewModels;
using System.Windows.Controls;

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


    }
}