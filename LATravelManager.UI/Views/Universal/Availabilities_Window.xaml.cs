using System.Windows;

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
    }
}