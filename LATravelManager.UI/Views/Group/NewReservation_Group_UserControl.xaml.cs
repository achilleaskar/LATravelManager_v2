using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LATravelManager.UI.Views.Group
{
    /// <summary>
    /// Interaction logic for NewReservation_Group_UserControl.xaml
    /// </summary>
    public partial class NewReservation_Group_UserControl : UserControl
    {
        #region Constructors

        public NewReservation_Group_UserControl()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

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

        private void IsPartnerCheckBox_Checked(object sender, RoutedEventArgs e) => CustomersDataGrid.Columns[6].IsReadOnly = true;

        private void IsPartnerCheckBox_Unchecked(object sender, RoutedEventArgs e) => CustomersDataGrid.Columns[6].IsReadOnly = false;

        #endregion Methods

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }
    }
}