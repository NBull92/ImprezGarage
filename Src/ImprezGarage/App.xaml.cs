//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage
{
    using CommonServiceLocator;
    using Infrastructure;
    using Infrastructure.Services;
    using Microsoft.Win32;
    using Modules.MyGarage;
    using Prism.Ioc;
    using Prism.Modularity;
    using System;
    using System.Windows;
    using Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);            
            
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            SystemEvents.SessionEnding += SystemEventsOnSessionEnding;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IVehicleService, VehicleService>();
        }

        /// <summary>
        /// Create the shell with the main window.
        /// </summary>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// Show the main window.
        /// </summary>
        protected override void InitializeShell(Window shell)
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

                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/NBull92/ImprezGarage"))
                {
                    var updateInfo = await mgr.CheckForUpdate();
                    if (updateInfo.ReleasesToApply.Any())
                    {
                        await mgr.UpdateApp();
                        UpdateManager.RestartApp();
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Application.Current.MainWindow?.Show();
                });
            });
#else
            Current.MainWindow?.Show();
#endif

            base.InitializeShell(shell);
        }

        /// <summary>
        /// Add all of the modules to the catalog for ImprezGarage.
        /// </summary>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule(typeof(Modules.Logger.LoggerModule));
            moduleCatalog.AddModule(typeof(Modules.Firebase.FirebaseModule));
            moduleCatalog.AddModule(typeof(Modules.FirebaseAuth.AuthModule));
            moduleCatalog.AddModule(typeof(Modules.Account.AccountModule));
            moduleCatalog.AddModule(typeof(Modules.Settings.SettingsModule));
            moduleCatalog.AddModule(typeof(Modules.Notifications.NotificationsModule));
            moduleCatalog.AddModule(typeof(Modules.PetrolExpenditure.PetrolExpenditureModule));
            moduleCatalog.AddModule(typeof(Modules.PerformChecks.PerformChecksModule));
            moduleCatalog.AddModule(typeof(MyGarageModule));
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        private void SystemEventsOnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            PrintFiles();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var loggerService = ServiceLocator.Current.GetInstance<ILoggerService>();
            loggerService.LogException((e.ExceptionObject as Exception));
            PrintFiles();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            PrintFiles();
        }

        /// <summary>
        /// Print out the log and settings files.
        /// </summary>
        private static void PrintFiles()
        {
            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            settingsService.SaveConfigurationFile();

            var loggerService = ServiceLocator.Current.GetInstance<ILoggerService>();
            loggerService.PrintLogFile();
            loggerService.PrintConfigurationFile();
        }
    }
}   //ImprezGarage namespace 