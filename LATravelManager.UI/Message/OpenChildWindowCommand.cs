using System.Windows;

namespace LATravelManager.UI.Message
{
    internal class OpenChildWindowCommand
    {
        #region constructors and destructors

        public OpenChildWindowCommand(Window window)
        {
            Window = window;
        }

        public OpenChildWindowCommand(Window window, BaseViewModel viewModel)
        {
            Window = window;
            ViewModel = viewModel;
        }

        #endregion constructors and destructors

        public Window Window { get; set; }
        public BaseViewModel ViewModel { get; set; }
    }
}