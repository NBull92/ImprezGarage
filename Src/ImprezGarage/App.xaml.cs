//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage
{
    using ImprezGarage.Infrastructure.Services;
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
        }

        private void SystemEventsOnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            settingsService.PrintConfigurationFile();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            settingsService.PrintConfigurationFile();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            settingsService.PrintConfigurationFile();
        }
    }
}   //ImprezGarage namespace 