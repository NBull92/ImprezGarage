//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks
{
    using Infrastructure;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;

    public class PerformChecksModule : IModule
    {
        /// <summary>
        /// Store the region manager.
        /// </summary>
        private readonly IRegionManager _regionManager;

        #region Methods
        /// <summary>
        /// Construct the module and inject the container and region manager.
        /// </summary>
        public PerformChecksModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, Main>(typeof(Main).FullName);
            containerRegistry.Register<object, PerformNewCheck>(typeof(PerformNewCheck).FullName);

            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Main));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks namespace 