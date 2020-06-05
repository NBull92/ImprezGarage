//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PetrolExpenditure
{
    using Infrastructure;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using Views;

    public class PetrolExpenditureModule : IModule
    {
        /// <summary>
        /// Store the container manager.
        /// </summary>
        private readonly IRegionManager _regionManager;

        #region Methods
        /// <summary>
        /// Construct the module and inject the container and region manager.
        /// </summary>
        public PetrolExpenditureModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<object, Main>(typeof(Main).FullName);

            _regionManager.RegisterViewWithRegion(RegionNames.PetrolEntriesRegion, typeof(PetrolExpenditure));
            _regionManager.RegisterViewWithRegion(RegionNames.PetrolHistoryRegion, typeof(PetrolUsageGraph));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Main));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
        #endregion
    }
}   //ImprezGarage.Modules.PetrolExpenditure namespace 