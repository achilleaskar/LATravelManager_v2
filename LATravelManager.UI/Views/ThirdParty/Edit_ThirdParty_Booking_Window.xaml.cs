using LATravelManager.UI.ViewModel.CategoriesViewModels.ThirdParty;
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
            }

        }
    }
}
