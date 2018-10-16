//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure
{
    using Views;
    using Prism.Modularity;
    using Prism.Regions;
    using Microsoft.Practices.Unity;
    using Infrastructure;

    public class PetrolExpenditureModule : IModule
    {
        #region Attributes
        /// <summary>
        /// Store the container manager.
        /// </summary>
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// Store the region manager.
        /// </summary>
        private readonly IUnityContainer _container;
        #endregion

        #region Methods
        /// <summary>
        /// Construct the module and inject the container and region manager.
        /// </summary>
        public PetrolExpenditureModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        /// <summary>
        /// Initialize the module and register any types, whether it be an interface and it's implementation, or it ve a view.
        /// Get the current instance of the settings service and load the current file.
        /// </summary>
        public void Initialize()
        {
            _container.RegisterType<object, Main>(typeof(Main).FullName);
            
            _regionManager.RegisterViewWithRegion(RegionNames.PetrolRegion, typeof(Main)); 
            _regionManager.RegisterViewWithRegion(RegionNames.PetrolEntriesRegion, typeof(PetrolExpenditure)); 
            _regionManager.RegisterViewWithRegion(RegionNames.PetrolHistoryRegion, typeof(PetrolUsageGraph));
        }
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure namespace 