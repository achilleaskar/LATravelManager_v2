using LATravelManager.Models;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Bansko;
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

namespace LATravelManager.UI.Views.Personal
{
    /// <summary>
    /// Interaction logic for NewReservation_Personal_UserControl.xaml
    /// </summary>
    public partial class NewReservation_Personal_UserControl : UserControl
    {
        public NewReservation_Personal_UserControl()
        {
            InitializeComponent();
        }
        private void IsPartnerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CustomersDataGrid.Columns[5].Visibility = Visibility.Collapsed;
        }

        private void IsPartnerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomersDataGrid.Columns[5].Visibility = Visibility.Visible;
        }
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            var child = default(T);
            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
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
            else if (e.Key == Key.Delete && e.OriginalSource.GetType() != typeof(TextBox))
            {
                e.Handled = true;
                (DataContext as NewReservation_Bansko_ViewModel).DeleteSelectedCustomers();
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Z)
            {
                ((NewReservation_Bansko_ViewModel)DataContext).ReadNextLineCommand.Execute(null);
            }
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem lvi && lvi.DataContext is Room)
            {
                if ((DataContext as NewReservation_Bansko_ViewModel).PutCustomersInRoomCommand.CanExecute(null))
                    (DataContext as NewReservation_Bansko_ViewModel).PutCustomersInRoomCommand.Execute(null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}