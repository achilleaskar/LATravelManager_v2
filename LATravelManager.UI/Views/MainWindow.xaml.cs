using System;
using System.Windows;
using LATravelManager.UI.ViewModel;
using Squirrel;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/achilleaskar/LaTravelManager"))
                {
                    var releaseEntry = await updateManager.UpdateApp();
                    if (releaseEntry?.Version.ToString() != null)
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Εγκαταστάθηκε νέα ενημέρωση. Θέλετε να επανεκκινήσετε την εφαρμογή σας τώρα?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dialogResult == MessageBoxResult.Yes)
                            UpdateManager.RestartApp();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains(".exe"))
                    MessageBox.Show("Error updating:" + ex.Message);
            }
        }
    }
}