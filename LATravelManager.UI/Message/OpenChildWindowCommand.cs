using GalaSoft.MvvmLight;
using System.Windows;

namespace LATravelManager.UI.Message
{
    internal class OpenChildWindowCommand
    {
        #region Constructors

        public OpenChildWindowCommand(Window window)
        {
            Window = window;
        }

        public OpenChildWindowCommand(Window window, ViewModelBase viewModel, Window parent = null)
        {
            Window = window;
            ViewModel = viewModel;
            Parent = parent;
        }

        #endregion Constructors

        #region Properties

        public Window Parent { get; set; }
        public ViewModelBase ViewModel { get; set; }
        public Window Window { get; set; }

        #endregion Properties
    }
}