//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage
{
    using Microsoft.Practices.ServiceLocation;
    using ImprezGarage.Infrastructure.Services;
    using System.Windows;
    using System.Windows.Threading;

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

            Current.DispatcherUnhandledException += UnhandledException;
        }

        private void UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
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