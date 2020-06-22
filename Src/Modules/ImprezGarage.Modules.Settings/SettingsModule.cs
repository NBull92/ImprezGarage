//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Settings
{
    using ImprezGarage.Modules.Logger.Views;
    using Infrastructure;
    using Infrastructure.Services;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;

    public class SettingsModule : IModule
    {
        /// <summary>
        /// Store the region manager.
        /// </summary>
        private readonly IRegionManager _regionManager;

        #region Methods
        /// <summary>
        /// Construct the module and inject the container and region manager.
        /// </summary>
        public SettingsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ISettingsService, SettingsService>();
            containerRegistry.Register<object, Main>(typeof(Main).FullName);
            containerRegistry.Register<object, LogSettings>(typeof(LogSettings).FullName);

            _regionManager.RegisterViewWithRegion(RegionNames.SettingsRegion, typeof(Main));
            _regionManager.RegisterViewWithRegion(RegionNames.LogSettingsRegion, typeof(LogSettings));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var settingsService = containerProvider.Resolve<ISettingsService>();
            settingsService.LoadConfigurationFile();
        }
        #endregion
    }
}   //ImprezGarage.Modules.Settings namespace 