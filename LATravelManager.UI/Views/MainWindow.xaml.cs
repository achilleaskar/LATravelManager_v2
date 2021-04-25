using System;
using System.Net;
using System.Windows;
using LATravelManager.UI.Repositories;
using Squirrel;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();

            Environment.Exit(0);
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                using (UpdateManager updateManager = new UpdateManager("http://demo.gotoskiathos.com/latravel/"))
                {
                    ReleaseEntry releaseEntry = await updateManager.UpdateApp();
                    if (releaseEntry?.Version.ToString() != null)
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Εγκαταστάθηκε νέα ενημέρωση. Θέλετε να επανεκκινήσετε την εφαρμογή σας τώρα?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            await UpdateManager.RestartAppWhenExited();
                            Application.Current.Shutdown();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains(".exe"))
                    MessageBox.Show("Error updating:" + ex.Message + "   " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                else
                    throw ex;
            }
        }
    }
}