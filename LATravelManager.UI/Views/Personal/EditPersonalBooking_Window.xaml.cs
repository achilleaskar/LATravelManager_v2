using LATravelManager.UI.ViewModel.CategoriesViewModels.Personal;
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
using System.Windows.Shapes;

namespace LATravelManager.UI.Views.Personal
{
    /// <summary>
    /// Interaction logic for EditPersonalBooking_Window.xaml
    /// </summary>
    public partial class EditPersonalBooking_Window : Window
    {
        public EditPersonalBooking_Window()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is NewReservation_Personal_ViewModel a && a.HasChanges)
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη απόθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
          
        }
    }
}
