//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Settings
{
    using ImprezGarage.Modules.Logger.Views;
    using Infrastructure;
    using Infrastructure.Services;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;

    public class SettingsModule : IModule
    {
        #region Attributes
        /// <summary>
        /// Store the container manager.
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Store the region manager.
        /// </summary>
        private readonly IRegionManager _regionManager;
        #endregion

        public SettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<object, Main>(typeof(Main).FullName);
            _container.RegisterType<object, LogSettings>(typeof(LogSettings).FullName);

            var settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();
            settingsService.LoadConfigurationFile();

            _regionManager.RegisterViewWithRegion(RegionNames.SettingsRegion, typeof(Main));
            _regionManager.RegisterViewWithRegion(RegionNames.LogSettingsRegion, typeof(LogSettings));
        }
    }
}   //ImprezGarage.Modules.Settings namespace 