//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.StatusBar
{
    using Prism.Modularity;
    using Prism.Regions;
    using Microsoft.Practices.Unity;
    using Infrastructure;

    public class StatusBarModule : IModule
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
        /// Status module constructor. Store the dependencies.
        /// </summary>
        public StatusBarModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        /// <summary>
        /// Register the status bar view to the container and then register the view to the region.
        /// </summary>
        public void Initialize()
        {            
            _container.RegisterType<object, Views.StatusBar>(typeof(Views.StatusBar).FullName);
            _regionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(Views.StatusBar));
        }
        #endregion
    }
}   //ImprezGarage.Modules.StatusBar namespace 