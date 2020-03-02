//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

using CommonServiceLocator;

namespace ImprezGarage
{
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Win32;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);            
            
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            SystemEvents.SessionEnding += SystemEventsOnSessionEnding;
            //ShutdownMode = ShutdownMode.OnMainWindowClose;
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