//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Settings
{
    using ImprezGarage.Infrastructure.Services;
    using ImprezGarage.Modules.Settings.Views;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Regions;
    using Prism.Unity;

    public class SettingsModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public SettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());

            _container.RegisterTypeForNavigation<ViewA>();


            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            settingsService.LoadConfigurationFile();
        }
    }
}   //ImprezGarage.Modules.Settings namespace 