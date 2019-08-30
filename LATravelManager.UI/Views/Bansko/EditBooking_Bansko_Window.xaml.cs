using LATravelManager.Model.Hotels;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LATravelManager.UI.Views.Bansko
{
    /// <summary>
    /// Interaction logic for EditBooking_Window.xaml
    /// </summary>
    public partial class EditBooking_Bansko_Window : Window
    {
        public EditBooking_Bansko_Window()
        {
            InitializeComponent();
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default;
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        private void IsPartnerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CustomersDataGrid.Columns[5].Visibility = Visibility.Collapsed;
        }

        private void IsPartnerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomersDataGrid.Columns[5].Visibility = Visibility.Visible;
        }

        private void CustomersDataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            UIElement inputElement;

            /// Texbox is the first control in my template column
            inputElement = GetVisualChild<ComboBox>(e.EditingElement);
            if (inputElement != null)
            {
                Keyboard.Focus(inputElement);
                return;
            }
        }

        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CustomersDataGrid.Items.Count > 1)
                {
                    CustomersDataGrid.CurrentCell = new DataGridCellInfo(CustomersDataGrid.SelectedItem,
                  CustomersDataGrid.Columns[1]);
                }
            }
            //else if (e.Key == Key.Delete && e.OriginalSource.GetType() != typeof(TextBox))
            //{
            //    e.Handled = true;
            //    (DataContext as NewReservation_Group_ViewModel).DeleteSelectedCustomers();
            //    e.Handled = true;
            //}
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Z)
            //{
            //    ((NewReservation_Group_ViewModel)DataContext).ReadNextLineCommand.Execute(null);
            //}
            //else if (e.Key == Key.Delete && e.OriginalSource.GetType() != typeof(TextBox))
            //{
            //    e.Handled = true;
            //    (DataContext as NewReservation_Group_ViewModel).DeleteSelectedCustomers();
            //    e.Handled = true;
            //}
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem lvi && lvi.DataContext is Room)
            {
                if ((DataContext as NewReservation_Group_ViewModel).PutCustomersInRoomCommand.CanExecute(null))
                    (DataContext as NewReservation_Group_ViewModel).PutCustomersInRoomCommand.Execute(null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CustomersDataGrid.Items.Count > 1)
                {
                    CustomersDataGrid.CurrentCell = new DataGridCellInfo(CustomersDataGrid.SelectedItem,
                  CustomersDataGrid.Columns[1]);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is NewReservation_Bansko_ViewModel a && a.HasChanges)
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη απόθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                    a.GenericRepository.RollBack();
            }
            else if (DataContext is NewReservation_Group_ViewModel b && b.HasChanges)
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη απόθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                    b.GenericRepository.RollBack();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ((NewReservationGroup_Base)DataContext).BookingWr.RaisePropertyChanged();
        }
    }
}