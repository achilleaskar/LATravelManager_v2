using LATravelManager.Model.Hotels;
using LATravelManager.UI.ViewModel.CategoriesViewModels.Group;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LATravelManager.UI.Views.Group
{
    /// <summary>
    /// Interaction logic for NewReservation_Bansko_UserControl.xaml
    /// </summary>
    public partial class NewReservation_Group_UserControl : UserControl
    {
        public NewReservation_Group_UserControl()
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
            //}
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.Z)
            {
                ((NewReservation_Group_ViewModel)DataContext).ReadNextLineCommand.Execute(null);
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
                if ((DataContext as NewReservation_Group_ViewModel).PutCustomersInRoomCommand.CanExecute(null))
                    (DataContext as NewReservation_Group_ViewModel).PutCustomersInRoomCommand.Execute(null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }
    }
}