using System;
using System.Collections.ObjectModel;
using System.Windows;
using LATravelManager.Models;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using Squirrel;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public GenericRepository StartingRepository;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
            StartingRepository = new GenericRepository();
#if DEBUG
            Helpers.StaticResources.StartingPlaces = new ObservableCollection<StartingPlace>(StartingRepository.GetAllSortedByName<StartingPlace>());
#endif
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync(StartingRepository);
        }


        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
#if !DEBUG
            try
            {
                //using (var updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/achilleaskar/LATravelManager_v2"))
                //{
                //    ReleaseEntry releaseEntry = await updateManager.UpdateApp();
                //    if (releaseEntry?.Version.ToString() != null)
                //    {
                //        MessageBoxResult dialogResult = MessageBox.Show("Εγκαταστάθηκε νέα ενημέρωση. Θέλετε να επανεκκινήσετε την εφαρμογή σας τώρα?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //        if (dialogResult == MessageBoxResult.Yes)
                //            UpdateManager.RestartApp();
                //    }
                //}
            }
            catch (Exception ex)
            {
                
                //if (!ex.Message.Contains(".exe"))
                //    MessageBox.Show("Error updating:" + ex.Message);
            }
#endif
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();

            Environment.Exit(0);
        }
    }
}