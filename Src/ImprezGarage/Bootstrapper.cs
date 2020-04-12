//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ImprezGarage
{
    using Infrastructure.Services;
    using Microsoft.Practices.Unity;
    using Modules.MyGarage;
    using Prism.Modularity;
    using Prism.Unity;
    using Squirrel;
    using System.Threading.Tasks;
    using System.Windows;
    using Views;

    class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// Create the shell with the main window.
        /// </summary>
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// Show the main window.
        /// </summary>
        protected override void InitializeShell()
        {

#if !DEBUG
            Task.Run(async () =>
            {
                // Keep in for testing locally.
                //using (var mgr = new UpdateManager(@"D:\Documents\Nick\GitHub\ImprezGarage\Releases"))
                //{
                //    var updateInfo = await mgr.CheckForUpdate();
                //    if (updateInfo.ReleasesToApply.Any())
                //    {
                //        await mgr.UpdateApp();
                //        UpdateManager.RestartApp();
                //    }
                //}

                using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/NBull92/ImprezGarage"))
                {
                    var updateInfo = await mgr.Result.CheckForUpdate();
                    if (updateInfo.ReleasesToApply.Any())
                    {
                        await mgr.Result.UpdateApp();
                        UpdateManager.RestartApp();
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Application.Current.MainWindow?.Show();
                });
            });
#else
            Application.Current.MainWindow?.Show();
#endif
        }

        /// <summary>
        /// Register any interfaces and their implementation to the container.
        /// </summary>
        protected override void ConfigureContainer()
        {
            //register the interface to the container
            Container.RegisterType<IVehicleService, VehicleService>(new ContainerControlledLifetimeManager());

            base.ConfigureContainer();
        }

        /// <summary>
        /// Add all of the modules to the catalog for ImprezGarage.
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(Modules.FirebaseAuth.AuthModule));
            moduleCatalog.AddModule(typeof(Modules.Firebase.FirebaseModule));
            moduleCatalog.AddModule(typeof(Modules.Logger.LoggerModule));
            moduleCatalog.AddModule(typeof(Modules.Settings.SettingsModule));
            moduleCatalog.AddModule(typeof(Modules.Notifications.NotificationsModule));
            moduleCatalog.AddModule(typeof(Modules.PetrolExpenditure.PetrolExpenditureModule));
            moduleCatalog.AddModule(typeof(Modules.PerformChecks.PerformChecksModule));
            moduleCatalog.AddModule(typeof(MyGarageModule));
            moduleCatalog.AddModule(typeof(Modules.Account.AccountModule));
        }
    }
}   //ImprezGarage namespace 
