using LATravelManager.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LATravelManager.UI.Views.Bansko
{
    /// <summary>
    /// Interaction logic for NewReservation_Bansko_UserControl.xaml
    /// </summary>
    public partial class NewReservation_Bansko_UserControl : UserControl
    {
        public NewReservation_Bansko_UserControl()
        {
            InitializeComponent();
        }
        private void IsPartnerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CustomersDataGrid.Columns[5].Visibility = Visibility.Collapsed;
        }

        private void IsPartnerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomersDataGrid.Columns[5].Visibility= Visibility.Visible;
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

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl)&& e.Key == Key.Z)
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
    }
}