﻿using Autofac;
using LATravelManager.DataAccess;
using LATravelManager.UI.ViewModel;
using LATravelManager.UI.ViewModel.Window_ViewModels;
using LATravelManager.UI.Views;

namespace LATravelManager.UI.StartUp
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();


            builder.RegisterType<MainDatabase>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<LoginTab>().AsSelf();


            return builder.Build();
        }
    }
}
