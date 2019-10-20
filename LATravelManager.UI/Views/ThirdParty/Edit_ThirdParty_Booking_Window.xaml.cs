using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
using System.Windows;

namespace LATravelManager.UI.Views.ThirdParty
{
    /// <summary>
    /// Interaction logic for Edit_ThirdParty_Booking_Window.xaml
    /// </summary>
    public partial class Edit_ThirdParty_Booking_Window : Window
    {
        public Edit_ThirdParty_Booking_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is NewReservation_ThirdParty_VIewModel a && a.HasChanges)
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη απόθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                    a.GenericRepository.RollBack();
            }
        }
    }
}