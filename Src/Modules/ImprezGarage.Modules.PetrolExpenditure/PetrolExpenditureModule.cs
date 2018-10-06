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
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public PetrolExpenditureModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<object, Main>(typeof(Main).FullName);
            
            _regionManager.RegisterViewWithRegion(RegionNames.PetrolRegion, typeof(Main)); 
            _regionManager.RegisterViewWithRegion(RegionNames.PetrolEntriesRegion, typeof(PetrolExpenditure)); 
            _regionManager.RegisterViewWithRegion(RegionNames.PetrolHistoryRegion, typeof(PetrolUsageGraph));
        }
    }
}   //ImprezGarage.Modules.PetrolExpenditure namespace 