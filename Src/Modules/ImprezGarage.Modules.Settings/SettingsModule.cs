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

        #region Methods
        /// <summary>
        /// Construct the module and inject the container and region manager.
        /// </summary>
        public SettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        /// <summary>
        /// Initialize the module and register any types, whether it be an interface and it's implementation, or it be a view.
        /// Get the current instance of the settings service and load the current file.
        /// </summary>
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
        #endregion
    }
}   //ImprezGarage.Modules.Settings namespace 