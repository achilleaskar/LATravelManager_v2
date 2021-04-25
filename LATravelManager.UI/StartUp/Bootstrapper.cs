using Autofac;
using LATravelManager.DataAccess;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views;

namespace LATravelManager.UI.StartUp
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<MainDatabase>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<NavigationViewModel>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<RoomsManager>().AsSelf().SingleInstance();
            builder.RegisterType<NewReservationHelper>().AsSelf().SingleInstance();
            builder.RegisterType<GenericRepository>().As<IGenericRepository>();

            return builder.Build();
        }
    }
}