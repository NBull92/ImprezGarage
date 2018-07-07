//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage
{
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.Services;
    using ImprezGarage.Views;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Unity;
    using System.Windows;

    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            //register the interface to the container
            Container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(Modules.Notifications.NotificationsModule));
            moduleCatalog.AddModule(typeof(Modules.Settings.SettingsModule));
            moduleCatalog.AddModule(typeof(Modules.Logger.LoggerModule));
            moduleCatalog.AddModule(typeof(Modules.StatusBar.StatusBarModule));
            moduleCatalog.AddModule(typeof(Modules.PerformChecks.PerformChecksModule));
            moduleCatalog.AddModule(typeof(Modules.PetrolExpenditure.PetrolExpenditureModule));

            //MyGarage needs to be last as it is the one that loads the vehicles initially.
            moduleCatalog.AddModule(typeof(Modules.MyGarage.MyGarageModule));
        }
    }
}   //ImprezGarage namespace 
