using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.LocalModels;
using LATravelManager.UI.ViewModel.CategoriesViewModels;
using LATravelManager.UI.Wrapper;
using System.Windows.Controls;
using System.Windows.Input;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for Plan_UserControl.xaml
    /// </summary>
    public partial class Plan_UserControl : UserControl
    {
        public Plan_UserControl()
        {
            InitializeComponent();
            //EditReservationCommand = new RelayCommand()
        }

        public RelayCommand EditReservationCommand { get; set; }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Plan_ViewModel)DataContext).RoomClicked(((Border)sender).DataContext as RoomWrapper);
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dc = DataContext as Plan_ViewModel;
            if (sender is Button b && b.DataContext is PlanDailyInfo pd)
            {
                dc.AddRoomThisDayCommand.Execute(pd);
            }

        }
    }
}