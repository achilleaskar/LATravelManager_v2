using System.Windows;
using LATravelManager.Model.Hotels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Skiathos;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views.Universal
{
    /// <summary>
    /// Interaction logic for Availabilities_Window.xaml
    /// </summary>
    public partial class Availabilities_Window : Window
    {
        public Availabilities_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Helpers.StaticResources.Close(this);
        }

         private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem lvi && lvi.DataContext is Room)
            {
                if ((DataContext as NewReservation_Skiathos_ViewModel).PutCustomersInRoomCommand.CanExecute(null))
                    (DataContext as NewReservation_Skiathos_ViewModel).PutCustomersInRoomCommand.Execute(null);
            }
        }
    }
}