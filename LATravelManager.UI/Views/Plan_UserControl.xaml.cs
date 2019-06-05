using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.Hotels;
using LATravelManager.UI.ViewModel.CategoriesViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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




    }
}
