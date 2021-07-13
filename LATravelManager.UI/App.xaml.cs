using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views;

namespace LATravelManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent,
               new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent,
              new RoutedEventHandler(SelectAllText), true);

            var vCulture = new CultureInfo("el-GR");

            Thread.CurrentThread.CurrentCulture = vCulture;
            Thread.CurrentThread.CurrentUICulture = vCulture;
#if DEBUG
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
#endif
            CultureInfo.DefaultThreadCurrentCulture = vCulture;
            CultureInfo.DefaultThreadCurrentUICulture = vCulture;

            base.OnStartup(e);
        }

        private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox != null && !textbox.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true;
                    textbox.Focus();
                }
            }
        }

        public SplashScreen splashScreen;
        public MainViewModel _viewModel;

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TextBox textBox)
                textBox.SelectAll();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            splashScreen = new SplashScreen("latravel_logo1.jpg");
            splashScreen.Show(false);
            // Bootstrapper bootstrapper = new Bootstrapper();
            // IContainer container = bootstrapper.Bootstrap();
            MainWindow mainWindow = new MainWindow();
            StaticResources.Windows.Add(mainWindow);
            _viewModel = new MainViewModel();
            mainWindow.DataContext = _viewModel;
            await _viewModel.LoadAsync();
            splashScreen.Close(TimeSpan.FromMilliseconds(250));
            // mainWindow.Visibility = Visibility.Hidden;
            mainWindow.Show();
            mainWindow.WindowState = WindowState.Maximized;
           // AutoUpdater.Start("http://demo.gotoskiathos.com/AutoUpdaterManager.xml");
        }

        // private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void Application_DispatcherUnhandledException(object sender,
          System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occurred. Please inform the admin."
              + Environment.NewLine + e.Exception.Message, "Unexpected error");
            // log.Error("error", e.Exception);
            e.Handled = true;
        }
    }
}