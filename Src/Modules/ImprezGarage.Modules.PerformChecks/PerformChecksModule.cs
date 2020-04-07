//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks
{
    using Microsoft.Practices.Unity;
    using Prism.Regions;
    using Prism.Modularity;
    using Views;
    using Infrastructure;

    public class PerformChecksModule : IModule
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
        public PerformChecksModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        /// <summary>
        /// Initialize the module and register any types, whether it be an interface and it's implementation, or it be a view.
        /// </summary>
        public void Initialize()
        {   
            _container.RegisterType<object, Main>(typeof(Main).FullName);
            _container.RegisterType<object, PerformNewCheck>(typeof(PerformNewCheck).FullName);

            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Main));
        }
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks namespace 