using LATravelManager.UI.ViewModel.ServiceViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LATravelManager.UI.Views.Services
{
    /// <summary>
    /// Interaction logic for PlaneService_UserControl.xaml
    /// </summary>
    public partial class PlaneService_UserControl : UserControl
    {
        public PlaneService_UserControl()
        {
            InitializeComponent();
        }

        private void TextBox_KeyUp2(object sender, KeyEventArgs e)
        {
            bool found = false;
            var data = (DataContext as Plane_ViewModel).Airports;

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear
                resultStackto.Children.Clear();
                popupto.IsOpen = false;
            }
            else
            {
                popupto.IsOpen = true;
            }

            // Clear the list
            resultStackto.Children.Clear();

            // Add the result
            foreach (var obj in data)
            {
                if (obj.ToLower().Contains(query.ToLower()))
                {
                    // The word starts with this... Autocomplete must work
                    AddItem2(obj);
                    found = true;
                }
            }

            if (!found)
            {
                popupto.IsOpen = false;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var data = (DataContext as Plane_ViewModel).Airports;

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear
                resultStack.Children.Clear();
                popup.IsOpen = false;
            }
            else
            {
                popup.IsOpen = true;
            }

            // Clear the list
            resultStack.Children.Clear();

            // Add the result
            foreach (var obj in data)
            {
                if (obj.ToLower().Contains(query.ToLower()))
                {
                    // The word starts with this... Autocomplete must work
                    AddItem(obj);
                    found = true;
                }
            }

            if (!found)
            {
                popup.IsOpen = false;
            }
        }

        private void AddItem(string text)
        {
            TextBlock block = new TextBlock
            {
                // Add the text
                Text = text,

                // A little style...
                Margin = new Thickness(2, 3, 2, 3),
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 11
            };

            // Mouse events
            block.MouseLeftButtonDown += (sender, e) =>
            {
                textBox.Text = (sender as TextBlock).Text;
                popup.IsOpen = false;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };

            // Add to the panel
            resultStack.Children.Add(block);
        }

        private void AddItem2(string text)
        {
            TextBlock block = new TextBlock
            {
                // Add the text
                Text = text,

                // A little style...
                Margin = new Thickness(2, 3, 2, 3),
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 11
            };

            // Mouse events
            block.MouseLeftButtonDown += (sender, e) =>
            {
                textBoxto.Text = (sender as TextBlock).Text;
                popupto.IsOpen = false;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };

            // Add to the panel
            resultStackto.Children.Add(block);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void TextBox_LostFocus2(object sender, RoutedEventArgs e)
        {
            popupto.IsOpen = false;
        }
    }
}