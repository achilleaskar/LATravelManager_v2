using GalaSoft.MvvmLight;
using LATravelManager.UI.ViewModel.BaseViewModels;
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

        public OpenChildWindowCommand(Window window, ViewModelBase viewModel)
        {
            Window = window;
            ViewModel = viewModel;
        }

        #endregion constructors and destructors

        public Window Window { get; set; }
        public ViewModelBase ViewModel { get; set; }
    }
}